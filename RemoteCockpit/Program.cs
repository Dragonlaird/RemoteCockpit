using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteCockpit
{
    class Program
    {
        static void Main(string[] args)
        {
            var fsConnector = new FSConnector();
            fsConnector.LogReceived += WriteLog;
            fsConnector.Start();
            while(fsConnector.Connecting || fsConnector.Connected)
            {
                Thread.Sleep(10);
            }
        }

        static void WriteLog(object sender, LogMessage message)
        {
            var strType = message.Type.ToString().Substring(0, 3).ToUpper();
            Console.WriteLine("({1}) [{0}] {2}", strType, sender.GetType().Name, message.Message);
        }
    }
}
