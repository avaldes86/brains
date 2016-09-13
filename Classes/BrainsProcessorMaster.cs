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
    public class BrainsProcessorMaster : BrainsProcessorNetBase
    {
        public List<RegisteredSlave> slaves = new List<RegisteredSlave>();
        public event EventHandler OnNewSlave;
        public event EventHandler OnSlaveUnregister;
        public event EventHandler IterationComplete;

        public string masterIP;
        public int masterPort;

        public BrainsProcessorMaster(BackgroundWorker backgroundWorker) : base(backgroundWorker)
        {
        }

        public BrainsProcessorMaster() : base()
        {
        }

        /// <summary>
        /// Start the process as cluster master
        /// </summary>
        /// <param name="tc">Initial training epochs</param>
        /// <param name="bases">Base of exponential growing of number of training epochs</param>
        public override void ProcessBrains(int tc, int bases)
        {
            int ii = 0;
            while (brains.Count > 1) /// It only needed one winner
            {
                var avSlaves = slaves.Select(i => new { Pound = i.MaxClockSpeed * i.Cores, Slave = i });
                var sum = avSlaves.Sum(i => i.Pound);
                sum += Pound();
                brains.ForEach(i => i.Reset());
                brains.ForEach(i => i.pending = true);
                var copyb = brains.ToList();
                object flag = new object();
                var avSlavess = avSlaves.Select(i => new { Assigned = i.Pound / sum * brains.Count, Pound = i.Pound / sum, Slave = i.Slave }).OrderByDescending(i => i.Pound);
                var dict = new Dictionary<RegisteredSlave, Brain[]>();

                //mark all ANNs as pending
                copyb.ForEach(i => i.pending = true);
                bw.ReportProgress(0, "Prepating ANNs for slaves...");
                foreach (var item in avSlavess)
                {
                    int assigned = 0;
                    if (item.Assigned < 1)
                    {
                        assigned = 1;
                    }
                    else
                    {
                        assigned = (int)Math.Floor(item.Assigned);
                    }
                    IEnumerable<Brain> sbrains;
                    lock (flag)
                    {
                        sbrains = copyb.Take(assigned);
                        copyb = copyb.Except(sbrains).ToList();
                    }
                    dict.Add(item.Slave, sbrains.ToArray());
                    item.Slave.brains = sbrains.ToList();
                }
                copyb.ForEach(i => i.pending = true);
                bw.ReportProgress(0, "ANNs prepared. Training locals as new task...");


                //Determine the number of training epochs
                var trainCount = (int)(tc * Math.Pow(bases, ii));

                // Process the local ANNs 
                Task.Factory.StartNew(() =>
                {
                    Parallel.ForEach(copyb, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 }, item =>
                                    {
                                        item.Learn(training, verification, trainCount);
                                        item.pending = false;
                                    });
                });
                bw.ReportProgress(0, "Sending ANNs and 'train' commands to slaves...");

                //Send the ANNs to the slaves instances 
                var prremote = Parallel.ForEach(dict, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, item =>
                {
                    if (item.Key.noTraining)
                        item.Key.commands.Enqueue(NetCommands.SendTrainingSet);
                    if (item.Key.noVerification)
                        item.Key.commands.Enqueue(NetCommands.SendVerificationSet);
                    item.Key.commands.Enqueue(NetCommands.SendBrains);
                    item.Key.tcount = trainCount;
                    item.Key.commands.Enqueue(NetCommands.Train);
                    //cxn.Close();
                });

                bw.ReportProgress(0, "Waiting for all threads to complete...");
                //Wait for all training and evaluations to continue 
                int count = 0;
                do
                {
                    lock (brains)
                    {
                        count = brains.Count(i => i.pending);
                    }
                    Thread.Sleep(500);
                }
                while (count != 0);

                // Take the best half
                lock (brains)
                {
                    brains = brains.OrderBy(i => i.Prec).Take(brains.Count / 2).ToList();
                }

                bw.ReportProgress(0, $"Iteration complete. {brains.Count} ANNs remaining.");
                if (brain != null)
                    brain = brains.First().Prec < brain.Prec ? brains.First() : brain;
                else
                    brain = brains.First();
                IterationComplete?.Invoke(this, new EventArgs());
            }
        }
        /// <summary>
        /// Send the begin train epochs order to a cluster slave
        /// </summary>
        /// <param name="slave">Cluster slave</param>
        /// <param name="cxn">Connection</param>
        /// <param name="epochs">Training epochs</param>
        private void SendSlaveTrain(RegisteredSlave slave, int epochs)
        {
            var tcp = slave.cxn;
            tcp.sw.WriteLine(NetCommands.RunTraining);
            tcp.sw.WriteLine(epochs);
        }

        /// <summary>
        /// Send the verification set to a cluster slave
        /// </summary>
        /// <param name="slave">Cluster slave</param>
        /// <param name="cxn">Connection</param>
        /// <returns></returns>
        private Connection SendVerificationSet(RegisteredSlave slave)
        {
            var tcp = slave.cxn;
            SendVerificationSet(tcp.ns, tcp.sr, tcp.sw);
            return tcp;
        }

        /// <summary>
        /// Send the training set to a cluster slave
        /// </summary>
        /// <param name="slave">Cluster slave</param>
        /// <returns></returns>
        private Connection SendTrainingSet(RegisteredSlave slave)
        {
            var tcp = slave.cxn;
            SendTrainingSet(tcp.ns, tcp.sr, tcp.sw);
            return tcp;
        }

        /// <summary>
        /// Sends ANNs over a TcpConnection to a cluster slave
        /// </summary>
        /// <param name="slave">Cluster slave</param>
        /// <returns></returns>
        private Connection SendBrains(RegisteredSlave slave)
        {
            var tcp = slave.cxn;
            SendBrains(tcp.ns, tcp.sr, tcp.sw, slave.brains);
            return tcp;
        }

        Queue<string> scommands = new Queue<string>();

        public double maxSpeed { get
            {
                return (slaves.Sum(i => i.Cores * i.MaxClockSpeed) + Pound())/1024d;
            }
        }

        /// <summary>
        /// Process an incomming connection as cluster master
        /// </summary>
        /// <param name="socket">Connection</param>
        protected override void ProcessConnection(Socket socket)
        {
            var ns = new NetworkStream(socket);
            var sr = new StreamReader(ns);
            var sw = new StreamWriter(ns) { AutoFlush = true };
            sw.WriteLine("Hello " + Dns.GetHostEntry(socket.RemoteEndPoint.ToString().Split(':')[0]).HostName + " at " + DateTime.Now);
            var element = new RegisteredSlave(IPAddress.Parse(socket.RemoteEndPoint.ToString().Split(':')[0]), int.Parse(socket.RemoteEndPoint.ToString().Split(':')[1]));
            slaves.Add(element);
            element.Cores = int.Parse(sr.ReadLine());
            element.MaxClockSpeed = double.Parse(sr.ReadLine());
            element.CPU_Name = sr.ReadLine();
            element.DnsName = sr.ReadLine();
            element.noTraining = sr.ReadLine() == "NoTraining";
            element.noVerification = sr.ReadLine() == "NoVerification";
            element.cxn = new Connection(ns, sr, sw);
            OnNewSlave?.Invoke(element, new EventArgs()); // new c# syntaxis?
            sw.WriteLine("Registered");
            var command = "";
            do
            {
                while (!element.commands.Any())
                    Thread.Sleep(100);
                var cmd = element.commands.Dequeue();
                switch (cmd)
                {
                    case NetCommands.SendTrainingSet:
                        {
                            element.noTraining = false;
                            SendTrainingSet(element);
                            break;
                        }
                    case NetCommands.SendVerificationSet:
                        {
                            element.noVerification = false;
                            SendVerificationSet(element);
                            break;
                        }
                    case NetCommands.SendBrains:
                        {
                            SendBrains(element);
                            break;
                        }
                    case NetCommands.Train:
                        {
                            SendSlaveTrain(element, element.tcount);
                            var dict = new Dictionary<string, string>();
                            for (int i = 0; i < element.brains.Count; i++)
                            {
                                var id = sr.ReadLine();
                                var ann = sr.ReadLine();
                                double k;
                                dict.Add(id, ann);
                            }
                            lock (brains)
                            {
                                foreach (var item in dict.Where(j => brains.Any(ii => ii.id.ToString() == j.Key)))
                                {
                                    var rbrain = brains.First(i => i.id.ToString() == item.Key);
                                    var index = brains.IndexOf(rbrain);
                                    rbrain = Brain.Deserialize64(item.Value);
                                    brains[index] = rbrain;
                                    rbrain.pending = false;
                                }
                            }
                            break;
                        }
                }

            } while (command != NetCommands.quit);
            element = slaves.First(i => i.Address == IPAddress.Parse(socket.RemoteEndPoint.ToString().Split(':')[0]) && i.Port == int.Parse(socket.RemoteEndPoint.ToString().Split(':')[1]));
            slaves.Remove(element);
            OnSlaveUnregister?.Invoke(element, new EventArgs() { }); // new c# syntaxis?
            sw.WriteLine("Bye");
            sr.Close();
            sw.Close();
            ns.Close();
        }

        /// <summary>
        /// Sends the verification set to a slave instance over a NetworkStream
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="sr"></param>
        /// <param name="sw"></param>
        private void SendVerificationSet(NetworkStream ns, StreamReader sr, StreamWriter sw)
        {
            sw.WriteLine(NetCommands.RecieveVerificationSet);
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, verification);
            var data = Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
            sw.WriteLine(data.Length.ToString());
            sw.WriteLine(data);
            sw.Flush();
            ns.Flush();
            var count = int.Parse(sr.ReadLine());
            //if (count != verification.TrainingSampleCount)
            //    throw new Exception(ErrorTransmission);
        }

        /// <summary>
        /// Sends the training set to a specific slave
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="sr"></param>
        /// <param name="sw"></param>
        private void SendTrainingSet(NetworkStream ns, StreamReader sr, StreamWriter sw)
        {
            sw.WriteLine(NetCommands.RecieveTrainingSet);
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, training);
            var data = Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
            sw.WriteLine(data.Length.ToString());
            sw.WriteLine(data);
            sw.Flush();
            ns.Flush();
            var count = int.Parse(sr.ReadLine());
            if (count != training.TrainingSampleCount)
                throw new Exception(ErrorTransmission);
        }
    }
}
