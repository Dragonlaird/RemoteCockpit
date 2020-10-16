using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FlightSimulator.SimConnect;

namespace RemoteCockpit
{
    public class FSConnector : IDisposable
    {
        private MessageHandler handler;
        private const int WM_USER_SIMCONNECT = 0x0402;
        private bool bFSXcompatible = false;

        private SimConnect simConnect = null;

        private bool disposedValue;

        public bool Connecting { get; private set; }
        public bool Connected { get; private set; }

        public EventHandler MessageReceived;
        public EventHandler<LogMessage> LogReceived;

        private IntPtr m_hWnd;

        private IEnumerable<SimVarRequestResult> simvarRequests { get; set; }

        public FSConnector()
        {
            Initialize();
        }


        #region Start/Stop/Initialize/Dispose
        private void Initialize()
        {
            DestroyHandle();
            Connecting = false;
            Connected = false;
        }

        public void Start()
        {
            WriteLog("Connecting");
            Connecting = true;
            try
            {
                if (handler == null)
                    CreateHandle();

                simConnect = new SimConnect("Remote Cockpit", m_hWnd, WM_USER_SIMCONNECT, null, bFSXcompatible ? (uint)1 : 0);

                /// Listen for Connect
                simConnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);

                /// Listen for Disconnect
                simConnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

                /// Listen for Exceptions
                simConnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);

                /// Listen for SimVar Data
                simConnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);
            }
            catch(Exception ex)
            {
                WriteLog(ex.Message, EventLogEntryType.Error);
                Stop();
            }
        }

        public void Stop()
        {
            WriteLog("Disconnecting");
            Connecting = false;
            Connected = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();
                    simConnect?.Dispose();
                    handler?.DestroyHandle();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Window Handle Emulator
        private void CreateHandle()
        {
            if(handler != null)
            {
                DestroyHandle();
            }

            handler = new MessageHandler();
            handler.MessageReceived += ReceiveMessage;
            handler.LogReceived += WriteLog;
            handler.CreateHandle();
            m_hWnd = handler.Handle;
        }

        private void DestroyHandle()
        {
            if (handler != null)
            {
                handler.DestroyHandle();
                WriteLog("Destroying Handle");
            }
            handler = null;
        }
        #endregion

        #region SimConnect Event Handlers
        void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            // Find Request in SimVarRequestModel. If value has changed, update it and invoke MessageReceived
            uint iObject = data.dwObjectID;
            DEFINITION simVarId = (DEFINITION)data.dwDefineID;
            var simVarRequest = simvarRequests.FirstOrDefault(x => x.DefId == simVarId);
            if (simVarRequest != null)
            {
                if (simVarRequest.Value == (double)data.dwData[0])
                    return;

                simVarRequest.Value = (double)data.dwData[0];
            }

            if (MessageReceived != null)
                MessageReceived.DynamicInvoke(sender, simVarRequest);
        }

        void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Connected = true;
            Connecting = false;
            WriteLog("Connected");
        }

        void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            if (simConnect != null && (Connected | Connecting))
            {
                Stop();
                try
                {
                    simConnect.Dispose();
                    simConnect = null;
                    DestroyHandle();
                }
                catch (Exception ex) {
                    WriteLog(ex.Message, EventLogEntryType.Error);
                }
            }
            else
            {
                Console.WriteLine("Already disconnected");
            }
        }

        void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            SIMCONNECT_EXCEPTION eException = (SIMCONNECT_EXCEPTION)data.dwException;
            var errorType = Enum.GetName(typeof(SIMCONNECT_EXCEPTION), data.dwException);
            Console.WriteLine(string.Format("SimConnect Error: {0}", errorType));
            if (simConnect != null)
            {
                try
                {
                    SimConnect_OnRecvQuit(sender, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("SimConnect Dispose Error: {0}", ex.Message));
                }
            }
        }
        #endregion

        #region Message and Log Handlers
        private void ReceiveMessage(object sender, EventArgs e)
        {
            simConnect?.ReceiveMessage();
        }

        private void WriteLog(object sender, LogMessage message)
        {
            WriteLog(message.Message, message.Type);
        }

        private void WriteLog(string message, EventLogEntryType type = EventLogEntryType.Information)
        {
            if (LogReceived != null)
            {
                LogReceived.DynamicInvoke(this, new LogMessage { Message = message, Type = type });
            }
            else
            {
                var strType = type.ToString().Substring(0, 3);
                Console.WriteLine("[{0}] {1}", strType, message);
            }
        }
        #endregion
    }
}
