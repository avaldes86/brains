using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Brains.Classes
{
    /// <summary>
    /// Class used by the cluster master instance
    /// </summary>
    public class RegisteredSlave
    {
        public string DnsName;
        public IPAddress Address;
        public int Port;
        public int Cores;
        public double MaxClockSpeed;
        public string CPU_Name;
        public bool available = true;
        private IPAddress ip;
        public bool PendingResults = false;
        internal bool noTraining;
        internal bool noVerification;
        internal Connection cxn;
        internal Queue<string> commands = new Queue<string>();
        internal List<Brain> brains;
        internal int tcount;

        public void EnqueueCommand(string command)
        {
            commands.Enqueue(command);
        }

        /// <summary>
        /// Creates a new instance of registered slave instance
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public RegisteredSlave(IPAddress ip, int port)
        {
            this.ip = ip;
            Port = port;
        }

        public override string ToString()
        {
            return ip + ":" + Port;
        }
        
    }

}
