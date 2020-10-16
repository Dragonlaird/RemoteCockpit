using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpit
{
    public class RemoteCockpit
    {
        public EventHandler<LogMessage> LogReceived;
        private FSConnector fsConnector;
        public RemoteCockpit()
        {
            fsConnector = new FSConnector();
            fsConnector.LogReceived += WriteLog;
            fsConnector.Start();
        }

        public void WriteLog(object sender, LogMessage msg)
        {
            if (LogReceived != null)
            {
                LogReceived.DynamicInvoke(sender, msg);
            }
            else
            {
                var strType = msg.Type.ToString().Substring(0, 3);
                Console.WriteLine("({0}) [{1}] {2}", sender?.GetType().Name, strType, msg.Message);
            }

        }
    }
}
