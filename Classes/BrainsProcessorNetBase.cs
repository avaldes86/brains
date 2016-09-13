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
    public class BrainsProcessorNetBase : BrainsProcessorBase
    {
        public const string ErrorTransmission = "Error in transmission. Data may be corrupt.";
        protected TcpListener listener;
        public bool listening;
        protected Thread main;
        protected bool locked;
        protected Connection cxn;
        public Queue<string> logs = new Queue<string>();

        protected BrainsProcessorNetBase(BackgroundWorker backgroundWorker)
        {
            bw = backgroundWorker;
        }

        protected BrainsProcessorNetBase() : base()
        {
        }


        /// <summary>
        /// Send a list of ANNs over a Tcp Connection
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="sr"></param>
        /// <param name="sw"></param>
        /// <param name="cbrains">List of brains</param>
        public void SendBrains(NetworkStream ns, StreamReader sr, StreamWriter sw, List<Brain> cbrains)
        {
            sw.WriteLine(NetCommands.RecieveBrains);
            sw.WriteLine(cbrains.Count);
            foreach (var item in cbrains)
            {
                item.pending = true;
                var ms = new MemoryStream();
                item.Serialize(ms);
                var data = Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
                var data1 = ms.ToArray();
                var hash = Convert.ToBase64String(MD5.Create().ComputeHash(data1));
                sw.WriteLine(item.id);
                sw.WriteLine(data.Length.ToString());
                sw.WriteLine(data);
                sw.WriteLine(hash);
                sw.Flush();
                ns.Flush();
                var rhash = sr.ReadLine();
                if (rhash != hash)
                    throw new Exception(ErrorTransmission);
            }
        }

        /// <summary>
        /// Recieve brains from a cluster coworker over a TcpConnection
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="sr"></param>
        /// <param name="sw"></param>
        public void RecieveBrains()
        {
            var q = int.Parse(cxn.sr.ReadLine());
            var rbrains = new List<Brain>();
            for (int i = 0; i < q; i++)
            {
                var id = cxn.sr.ReadLine();
                var size = int.Parse(cxn.sr.ReadLine());
                var data = cxn.sr.ReadLine();
                var hash = cxn.sr.ReadLine();
                var d1 = Convert.FromBase64String(data);
                var ms = new MemoryStream(d1);
                var chash = Convert.ToBase64String(MD5.Create().ComputeHash(d1));
                cxn.sw.WriteLine(chash);
                cxn.sw.Flush();
                cxn.ns.Flush();
                if (chash != hash)
                    throw new Exception(ErrorTransmission);
                rbrains.Add(Brain.Deserialize(ms));
                ms.Dispose();
            }
            brains = rbrains;
            logs.Enqueue($"{DateTime.Now}: {brains.Count} recieved.");
        }

        /// <summary>
        /// Start the TcpListener and the thread to accept and process incoming connections
        /// </summary>
        /// <param name="interf">Interface IP to bind. Ex: 0.0.0.0 (bind all interfaces)</param>
        /// <param name="port">Port to listen</param>
        public void StartListen(IPAddress interf, int port)
        {
            listening = true;
            listener = new TcpListener(interf, port);
            listener.Start();
            main = new Thread(Listener);
            main.Start();
        }

       
        /// <summary>
        /// The thread which wait for incoming connections 
        /// </summary>
        protected void Listener()
        {
            while (listening)
            {
                var ir = listener.BeginAcceptSocket(ServerProcess, listener);
                locked = true;
                while (locked)
                {
                    Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        /// Method to override on child classes to process connections
        /// </summary>
        /// <param name="sock"></param>
        protected virtual void ProcessConnection(Socket sock)
        {

        }
        /// <summary>
        /// Process an incomming connection
        /// </summary>
        /// <param name="ar">Asynchronous result of TcpListener.BeginAcceptSocket</param>
        private void ServerProcess(IAsyncResult ar)
        {
            locked = false;
            if (ar.AsyncState == null) return;
            var socket = ((TcpListener)ar.AsyncState).EndAcceptSocket(ar);
            if (socket == null) return;
            ProcessConnection(socket);
        }


        /// <summary>
        /// Stop the thread listening for incomming connections
        /// </summary>
        public void StopListen()
        {
            listening = false;
            listener.Stop();
        }
        
        internal override void SetData(List<double[]> data)
        {
            this.data = data;
        }
        

    }
}
