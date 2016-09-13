using Brains.Classes;
using ContecAI.ROOT.CIMV2;
using NeuronDotNet.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brains
{
    public class BrainsProcessorSlave : BrainsProcessorNetBase
    {
        public string masterIP;
        public int masterPort;
        
        public BrainsProcessorSlave(string masterIp, int masterPort, BackgroundWorker backgroundWorker) : base(backgroundWorker)
        {
            masterIP = masterIp;
            this.masterPort = masterPort;
        }

        public BrainsProcessorSlave() : base()
        {
        }

        /// <summary>
        /// Process an incomming connection as slave
        /// </summary>
        /// <param name="socket">Connection</param>
        protected void ProcessConnection(Connection cxn)
        {
            var welcome = cxn.sr.ReadLine();
            logs.Enqueue(welcome);
            ReportStatus();
            welcome = cxn.sr.ReadLine();
            logs.Enqueue(welcome);
            var command = "";
            do
            {
                command = cxn.sr.ReadLine();
                switch (command)
                {
                    case NetCommands.RecieveBrains:
                        {
                            RecieveBrains();
                            break;
                        }
                    case NetCommands.RecieveTrainingSet:
                        {
                            RecieveTrainingSet();
                            break;
                        }
                    case NetCommands.RecieveVerificationSet:
                        {
                            RecieveVerificationSet();
                            break;
                        }
                    case NetCommands.RunTraining:
                        {
                            var epochs = int.Parse(cxn.sr.ReadLine());
                            SlaveTrain(epochs);
                            break;
                        }
                }
            } while (command != NetCommands.quit && command!=null);
            cxn.Close();
        }

        /// <summary>
        /// Starts the training epochs of assigned ANNs and when its done, send its back to the master machine.
        /// </summary>
        /// <param name="e">Training epochs</param>
        private void SlaveTrain(object e)
        {
            var epochs = (int)e;
            Parallel.ForEach(brains, new ParallelOptions() { MaxDegreeOfParallelism = (Environment.ProcessorCount - 1) > brains.Count ? brains.Count : (Environment.ProcessorCount - 1) }, item =>
            {
                item.Learn(training, verification, epochs);
            });
            foreach (var item in brains)
            {
                cxn.sw.WriteLine(item.id);
                cxn.sw.WriteLine(item.Serialize64());
            }           
            logs.Enqueue(DateTime.Now + ": ANNs processed and sent to the master instance.");
        }

        /// <summary>
        /// Register the program as slave
        /// </summary>
        /// <param name="ip">Master IP address</param>
        /// <param name="port">Master listening port</param>
        internal void RegisterAsSlave(string ip, int port)
        {
            cxn = new Connection(IPAddress.Parse(ip), port);
            logs.Enqueue(DateTime.Now + ": Connected.");
            t = new Thread(ProcessConnectionO);
            t.Start(cxn);
        }

        private void ProcessConnectionO(object o)
        {
            ProcessConnection((Connection)o);
        }

        Thread t;

        /// <summary>
        /// Unregister the program as slave
        /// </summary>
        /// <param name="ip">Master IP address</param>
        /// <param name="port">Master listening port</param>
        internal void UnregisterAsSlave()
        {
            var hello = cxn.sr.ReadLine();
            cxn.sw.WriteLine("UnregisterSlave");
            var done = cxn.sr.ReadLine();
            logs.Enqueue(DateTime.Now + ": Unregistered.");
        }

        /// <summary>
        /// Report to the master the current status and information of the slave system
        /// </summary>
        /// <param name="ns">Network channel</param>
        /// <param name="sr"></param>
        /// <param name="sw"></param>
        private void ReportStatus()
        {
            var proc = GetProcessorInfo();
            cxn.sw.WriteLine(Environment.ProcessorCount - 1);
            cxn.sw.WriteLine(proc.MaxClockSpeed);
            cxn.sw.WriteLine(proc.Manufacturer + " - " + proc.Name);
            cxn.sw.WriteLine(Dns.GetHostName());
            cxn.sw.WriteLine((training == null ? "No" : "") + "Training");
            cxn.sw.WriteLine((verification == null ? "No" : "") + "Verification");
            cxn.sw.Flush();
            logs.Enqueue(DateTime.Now + ": Status reported.");

        }

        /// <summary>
        /// Recieves the verification set from the master to test ANNs after the training
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="sr"></param>
        /// <param name="sw"></param>
        private void RecieveVerificationSet()
        {
            var bf = new BinaryFormatter();
            var length = int.Parse(cxn.sr.ReadLine());
            var data = cxn.sr.ReadLine();
            var d1 = Convert.FromBase64String(data);
            var ms = new MemoryStream(d1);
            verification = (TrainingSet)bf.Deserialize(ms);
            ms.Dispose();
            cxn.sw.WriteLine(training.TrainingSampleCount);
            logs.Enqueue(DateTime.Now + ": Recieved " + training.TrainingSampleCount + " verification samples.");
        }

        /// <summary>
        /// Recieves the training set from the master to test ANNs after the training
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="sr"></param>
        /// <param name="sw"></param>
        private void RecieveTrainingSet()
        {
            var bf = new BinaryFormatter();
            var length = int.Parse(cxn.sr.ReadLine());
            var data = cxn.sr.ReadLine();
            var d1 = Convert.FromBase64String(data);
            var ms = new MemoryStream(d1);
            training = (TrainingSet)bf.Deserialize(ms);
            ms.Dispose();
            cxn.sw.WriteLine(training.TrainingSampleCount);
            logs.Enqueue(DateTime.Now + ": Recieved " + training.TrainingSampleCount + " training samples.");
        }
    }
}
