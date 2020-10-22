using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpit
{
    public class RemoteCockpit
    {
        public EventHandler<LogMessage> LogReceived;
        private FSConnector fsConnector;
        private SocketListener listener;
        private bool fsConnected = false;
        public RemoteCockpit()
        {
            StartConnector();
            StartListener();
        }

        private void StartConnector()
        {
            fsConnector = new FSConnector();
            fsConnector.LogReceived += WriteLog;
            fsConnector.DataReceived += MessageReceived;
            fsConnector.ConnectionStateChange += ConnectionStateChanged;
            var tempRequest = new SimVarRequest { Name = "GPS POSITION ALT" };
            fsConnector.RequestVariable(tempRequest);
            tempRequest = new SimVarRequest { Name = "AMBIENT WIND VELOCITY" };
            fsConnector.RequestVariable(tempRequest);
            tempRequest = new SimVarRequest { Name = "AMBIENT WIND DIRECTION" };
            fsConnector.RequestVariable(tempRequest);
            fsConnector.ValueRequestInterval = 3;
            fsConnector.Start();

        }

        private void StartListener()
        {
            var ipAddress = ConfigurationManager.AppSettings.Get(@"ipAddress");
            var ipPort = int.Parse(ConfigurationManager.AppSettings.Get(@"ipPort"));
            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), ipPort);
            listener = new SocketListener(endPoint);
            listener.LogReceived += WriteLog;
            listener.Start();
        }

        /// <summary>
        /// If any variable values are received from FS, send to subscribed clients
        /// </summary>
        /// <param name="sender">FSConnector instance</param>
        /// <param name="e">SimVarRequestResult containing requested valiable, unit and value</param>
        private void MessageReceived(object sender, SimVarRequestResult e)
        {
            WriteLog(this, new LogMessage { Message = string.Format("Value Received: {0} - {1} ({2}) = {3}", e.Request.ID, e.Request.Name, e.Request.Unit, e.Value), Type = System.Diagnostics.EventLogEntryType.Information });
        }

        /// <summary>
        /// Notification of when FS is connected/disconnected
        /// </summary>
        /// <param name="sender">FSConnector instance</param>
        /// <param name="connected">True = Connected; False = Disconnected;</param>
        private void ConnectionStateChanged(object sender, bool connected)
        {
            fsConnected = connected;
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
