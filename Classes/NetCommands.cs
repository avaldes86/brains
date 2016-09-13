using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brains.Classes
{
    public class NetCommands
    {
        public const string UnregisterSlave = "UnregisterSlave";
        public const string RegisterSlave = "RegisterSlave";
        public const string RecieveBrains = "RecieveBrains";
        public const string RecieveTrainingSet = "RecieveTrainingSet";
        public const string RecieveVerificationSet = "RecieveVerificationSet";
        public const string RunTraining = "RunTraining";
        public const string SendTrainingSet = "SendTrainingSet";
        public const string SendVerificationSet = "SendVerificationSet";
        public const string SendBrains = "SendBrains";
        public const string Train = "Train";
        public const string quit = "quit";
    }
}
