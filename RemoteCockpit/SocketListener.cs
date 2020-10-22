using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpit
{
    public class SocketListener
    {
        private IPEndPoint _endPoint;
        private AsynchronousSocketListener _listener;
        public EventHandler<LogMessage> LogReceived;

        public SocketListener(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
        }

        public void Start()
        {
            WriteLog(string.Format("Starting Socket Listener: {0}:{1}", _endPoint.Address, _endPoint.Port));
            _listener = new AsynchronousSocketListener(_endPoint);
            AsynchronousSocketListener.NewConnection += NewConnection;
            AsynchronousSocketListener.StartListening();
        }

        private void NewConnection(object sender, StateObject connection)
        {
            //throw new NotImplementedException();
        }

        public void Stop()
        {
            AsynchronousSocketListener.StopListening();
        }

        #region Message and Log Handlers
        private void WriteLog(object sender, LogMessage message)
        {
            if (LogReceived != null)
            {
                LogReceived.DynamicInvoke(sender, message);
            }
            else
            {
                var strType = message.Type.ToString().Substring(0, 3);
                Console.WriteLine("[{0}] {1}", strType, message);
            }
        }

        private void WriteLog(string message, EventLogEntryType type = EventLogEntryType.Information)
        {
            WriteLog(this, new LogMessage { Message = message, Type = type });
        }
        #endregion
    }
}
