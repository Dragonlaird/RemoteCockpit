using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.FlightSimulator.SimConnect;

namespace RemoteCockpit
{
    public class FSConnector : IDisposable
    {
        private const int connectionCheckInterval = 10000;
        private MessagePumpManager handler;

        private bool disposedValue;

        public bool Connecting { get; private set; }
        private bool bConnected = false;

        // Call Callback to advise parent if Connection is dropped or successful
        public bool Connected
        {
            get
            {
                return bConnected;
            }
            private set
            {
                if (bConnected != value)
                {
                    bConnected = value;
                    if (ConnectionStateChange != null)
                    {
                        ConnectionStateChange.Invoke(this, value);
                    }
                }
            }
        }

        public EventHandler<SimVarRequestResult> MessageReceived;
        public EventHandler<LogMessage> LogReceived;
        public EventHandler<bool> ConnectionStateChange;
        public EventHandler<Exception> ErrorEvent;
        public EventHandler<SimVarRequestResult> DataReceived;

        private List<SimVarRequest> simVarRequests;

        public FSConnector()
        {
            Initialize();
            var tempRequest = new SimVarRequest { Name = "GPS POSITION ALT" };
            RequestVariable(tempRequest);
        }

        #region Start/Stop/Initialize/Dispose
        private void Initialize()
        {
            DisposeHandler();
            simVarRequests = new List<SimVarRequest>();
            Connecting = false;
            Connected = false;
        }

        public void Start()
        {
            System.Threading.Timer connectTimer = new System.Threading.Timer(StartConnector);
            connectTimer.Change(10, connectionCheckInterval);
            //StartConnector();
        }

        private void StartConnector(object state)
        {
            StartConnector();
        }

        private void StartConnector()
        {
            if (!Connected)
            {
                Stop();
                WriteLog("Connecting");
                Connecting = true;

                try
                {
                    CreateHandler();

                }
                catch (Exception ex)
                {
                    WriteLog(ex.Message, EventLogEntryType.Error);
                    //Stop();
                }
            }
        }

        public void Stop()
        {
            WriteLog("Disconnecting");
            DisposeHandler();
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
                }
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

        #region SimVar Request Handlers
        public void RequestVariable(SimVarRequest request)
        {
            if (request != null && !string.IsNullOrWhiteSpace(request.Name))
            {
                request.Name = request.Name.ToUpper();
                if (string.IsNullOrWhiteSpace(request.Unit))
                {
                    request.Unit = SimVarUnits.DefaultUnits[request.Name].DefaultUnit;
                }
                if (!simVarRequests.Any(x => x.Name == request.Name && request.Unit == request.Unit))
                {
                    if (simVarRequests.Count() == 0)
                    {
                        request.ID = 1;
                    }
                    else
                    {
                        request.ID = simVarRequests.Max(x => x.ID) + 1;
                    }
                    lock (simVarRequests)
                    {
                        WriteLog(string.Format("Adding SimVarRequest for ID: {0} - {1} ({2})", request.ID, request.Name, request.Unit));
                        simVarRequests.Add(request);
                    }
                    try
                    {
                        AddSimVarRequest(request);
                    }
                    catch(Exception ex)
                    {
                        WriteLog(string.Format("SimVarRequest Error: {0} - {1} ({2}) - {3}", request?.ID, request?.Name, request?.Unit, ex.Message), EventLogEntryType.Error);
                    }
                }
                else
                {
                    request = simVarRequests.First(x => x.Name == request.Name && x.Unit == request.Unit);
                    WriteLog(string.Format("SimVarRequest already requested: {0} - {1} ({2})", request.ID, request.Name, request.Unit));
                }
            }
            else
            {
                WriteLog(string.Format("Invalid SimVarRequest for: {0} ({1})", request?.Name, request?.Unit), EventLogEntryType.Error);
            }
        }

        private void AddSimVarRequest(SimVarRequest request)
        {
            if (Connected && handler != null)
                try
                {
                    handler.AddRequest(request);
                }
                catch (Exception ex)
                {
                    WriteLog(string.Format("SimVarRequest Error: {0} - {1} ({2}) - {3}", request?.ID, request?.Name, request?.Unit, ex.Message), EventLogEntryType.Error);
                }

        }
        #endregion

        #region Window Message Handler Emulator
        private void CreateHandler()
        {
            if (handler == null)
            {
                handler = new MessagePumpManager();               
                handler.SimConnected += Sim_Connection;
                handler.SimError += Sim_Error;
                handler.SimData += Sim_Data;
            }
        }

        private void DisposeHandler()
        {
            handler = null;
        }
        #endregion

        #region SimConnect Event Handlers
        void Sim_Error(object sender, SIMCONNECT_RECV_EXCEPTION error)
        {
            WriteLog(string.Format("SimConnect Exception: {0}", error.dwException), EventLogEntryType.Error);
            if (ErrorEvent != null)
                ErrorEvent.DynamicInvoke(this, new Exception(string.Format("SimConnect Exception: {0}", error.dwException)));
        }

        void Sim_Data(object sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            WriteLog(string.Format("SimConnect Data: {0}={1}", data.dwRequestID, data.dwData));

        }

        void Sim_Connection(object sender, bool connected)
        {
            if (connected)
            {
                Connected = true;
                WriteLog("Connected");
                // New connection to FS - resubmit all previous SimVariable requests
                foreach(var request in simVarRequests)
                {
                    AddSimVarRequest(request);
                }
            }
            else
            {
                Connected = false;
                WriteLog("Disconnected");
            }
            Connecting = false;
        }
        #endregion

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
