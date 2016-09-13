using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Brains
{
    public class Connection
    {
        public NetworkStream ns;
        public StreamReader sr;
        public StreamWriter sw;
        private IPAddress address;
        private int port;
        private Socket socket;

        public Connection(Socket socket)
        {
            this.socket = socket;
            ns = new NetworkStream(socket, true);
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns);
            sw.AutoFlush = true;
        }

        public Connection(IPAddress address, int port)
        {
            this.address = address;
            this.port = port;
            TcpClient tcp = new TcpClient();
            tcp.Connect(address, port);
            ns = new NetworkStream(tcp.Client,true);
            sr = new StreamReader(ns);
            sw = new StreamWriter(ns);
            sw.AutoFlush = true;
        }

        public Connection(NetworkStream ns, StreamReader sr, StreamWriter sw)
        {
            this.ns = ns;
            this.sr = sr;
            this.sw = sw;
        }

        internal void Close()
        {
            sr.Close();
            sw.Close();
            ns.Close();
        }
    }

}
