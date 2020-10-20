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

        private List<SimVarRequest> simVarRequests;

        private IEnumerable<SimVarRequestResult> simvarRequests { get; set; }

        public FSConnector()
        {
            Initialize();
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
                if (!simVarRequests.Any(x => x.Name == request.Name.ToUpper() && request.Unit == request.Unit.ToLower()))
                {
                    lock (simVarRequests)
                    {
                        WriteLog(string.Format("Adding SimVarRequest for: {0} ({1})", request.Name, request.Unit));
                        simVarRequests.Add(new SimVarRequest { Name = request.Name.ToUpper(), Unit = request.Unit.ToLower() });
                    }
                }
                else
                {
                    WriteLog(string.Format("SimVarRequest already requested: {0} ({1})", request.Name.ToUpper(), request.Unit.ToLower()));
                }
            }
            else
            {
                WriteLog(string.Format("Invalid SimVarRequest for: {0} ({1})", request?.Name?.ToUpper(), request?.Unit?.ToLower()), EventLogEntryType.Error);
            }
        }
        #endregion

        #region Window Handle Emulator
        private void CreateHandler()
        {
            if (handler == null)
            {
                handler = new MessagePumpManager();               
                handler.SimConnect += Sim_Connection;
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

        }

        void Sim_Data(object sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {

        }

        void Sim_Connection(object sender, bool connected)
        {
            if (connected)
            {
                Connected = true;
                WriteLog("Connected");
            }
            else
            {
                Connected = false;
                WriteLog("Disconnected");
            }
            Connecting = false;
        }
/*
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
        }

        void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            if (simConnect != null && (Connected | Connecting))
            {
                Stop();
                try
                {
                    simConnect?.Dispose();
                    simConnect = null;
                    DisposeHandler();
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
*/
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
