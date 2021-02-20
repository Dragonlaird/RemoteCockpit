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
using RemoteCockpitClasses;
using Serilog.Events;

namespace RemoteCockpitServer
{
    /// <summary>
    /// Check if MSFS2020 is running and send any requested valiable values to connected clients
    /// </summary>
    public class FSConnector : IDisposable
    {
        #region Private Variables
        private const int connectionCheckInterval = 10; // Seconds to recheck for FS connection 
        private int _valueRequestInterval = 1000; // Millieconds between each batch of requests for variable updates
        private MessagePumpManager handler;
        private bool disposedValue;
        private List<SimVarRequest> simVarRequests;
        private bool bConnected = false;
        private System.Threading.Timer requestTimer;
        #endregion

        #region Event Handlers
        public EventHandler<SimVarRequestResult> DataReceived;
        public EventHandler<LogMessage> LogReceived;
        public EventHandler<bool> ConnectionStateChange;
        public EventHandler<Exception> ErrorEvent;
        #endregion

        #region Public Properties
        public int ValueRequestInterval
        {
            get
            {
                return _valueRequestInterval;
            }
            set
            {
                if (value != _valueRequestInterval && value > 0)
                {
                    WriteLog(string.Format("Value Request Frequency changed from {0} to {1} milliseconds", _valueRequestInterval, value));
                    _valueRequestInterval = value;
                    if (requestTimer != null)
                    {
                        requestTimer.Dispose();
                        requestTimer = new System.Threading.Timer(RequestAllValues);
                        requestTimer.Change(10, _valueRequestInterval);
                    }
                }
            }
        }

        public bool Connecting { get; private set; }

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
        #endregion

        #region ctor
        public FSConnector()
        {
            Initialize();
        }
        #endregion

        #region Start/Stop/Initialize/Dispose
        /// <summary>
        /// Prepare local classes and variables
        /// </summary>
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
            connectTimer.Change(10, connectionCheckInterval * 1000);
            requestTimer = new System.Threading.Timer(RequestAllValues);
            requestTimer.Change(10, ValueRequestInterval);

            //StartConnector();
        }

        /// <summary>
        /// Start connecting to MSFS2020
        /// </summary>
        /// <param name="state"></param>
        private void StartConnector(object state)
        {
            StartConnector();
        }

        /// <summary>
        /// Start connecting to MSFS2020
        /// </summary>
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
                    WriteLog(string.Format("Unable to Start SimConnect Handler: {0}", ex.Message), LogEventLevel.Error);
                    //Stop();
                }
            }
        }

        /// <summary>
        /// Stop checking for MSFS 2020 running
        /// </summary>
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
        /// <summary>
        /// Client has requested a variable, add it to the ist of variables being checked, if not already requested
        /// </summary>
        /// <param name="request">Variable to listen for</param>
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
                        WriteLog(string.Format("Queing Request for ID: {0} - {1} ({2})", request.ID, request.Name, request.Unit));
                        simVarRequests.Add(request);
                    }
                    try
                    {
                        AddSimVarRequest(request);
                    }
                    catch(Exception ex)
                    {
                        WriteLog(string.Format("Request Error: {0} - {1} ({2}) - {3}", request?.ID, request?.Name, request?.Unit, ex.Message), LogEventLevel.Error);
                    }
                }
                else
                {
                    request = simVarRequests.First(x => x.Name == request.Name && x.Unit == request.Unit);
                    WriteLog(string.Format("Request already submitted: {0} - {1} ({2})", request.ID, request.Name, request.Unit), LogEventLevel.Warning);
                }
            }
            else
            {
                WriteLog(string.Format("Invalid Request for: {0} ({1})", request?.Name, request?.Unit), LogEventLevel.Error);
            }
        }

        /// <summary>
        /// Add new variable request to SimConnect handler
        /// </summary>
        /// <param name="request">SimVar request</param>
        private void AddSimVarRequest(SimVarRequest request)
        {
            if (Connected && handler != null)
                try
                {
                    WriteLog(string.Format("Requesting Variable: {0} - {1} ({2})", request?.ID, request?.Name, request?.Unit));
                    handler.AddRequest(request);
                }
                catch (Exception ex)
                {
                    WriteLog(string.Format("SimVarRequest Error: {0} - {1} ({2}) - {3}", request?.ID, request?.Name, request?.Unit, ex.Message), LogEventLevel.Error);
                }
        }

        /// <summary>
        /// Get latest value for every SimVar variable requested
        /// </summary>
        /// <param name="state"></param>
        private void RequestAllValues(object state)
        {
            if (Connected && handler != null)
            {
                lock (simVarRequests)
                    foreach (var request in simVarRequests)
                    {
                        handler?.GetValue(request);
                    }
            }
        }
        #endregion

        #region Window Message Handler Emulator
        /// <summary>
        /// Use a simulated Windows Message Pump to send/receive windows messages via SimConnect to/from MSFS2020
        /// </summary>
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

        /// <summary>
        /// Dispose of local instances of classes
        /// </summary>
        private void DisposeHandler()
        {
            handler = null;
        }
        #endregion

        #region SimConnect Event Handlers
        /// <summary>
        /// Called by SimConnect for any error
        /// </summary>
        /// <param name="sender">SimConnect</param>
        /// <param name="error">Error supplied by MSFS2020</param>
        void Sim_Error(object sender, SIMCONNECT_RECV_EXCEPTION error)
        {
            var reqId = error.dwSendID;
            var paramId = error.dwIndex; // Which item in the request caused the error
            var errorId = error.dwException;
            var errorString = string.Format("Error: {2}; Req: {0}; Param: {1}; Desc: {3};", reqId, GetSimErrorParameter((int)paramId), errorId, GetSimError((int)errorId));
            WriteLog(string.Format("SimConnect Exception: {0}", errorString), LogEventLevel.Error);
            if (ErrorEvent != null)
            {
                ErrorEvent.DynamicInvoke(this, new Exception(errorString));
            }
        }

        /// <summary>
        /// Which SimCOnnect parameter caused the MSFS2020 error
        /// </summary>
        /// <param name="index">Parameter Index</param>
        /// <returns>Name of parameter</returns>
        private string GetSimErrorParameter(int index)
        {
            var result = "";
            switch (index)
            {
                case 0:
                    result = "DefineID";
                    break;
                case 1:
                    result = "DatumeName";
                    break;
                case 2:
                    result = "UnitsName";
                    break;
                case 3:
                    result = "DatumeType";
                    break;
                case 4:
                    result = "DatumEpsilon";
                    break;
                case 5:
                    result = "DatumID";
                    break;
                default:
                    result = "Unknown";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Fnd which type of error was generated
        /// </summary>
        /// <param name="errorId">Error index value</param>
        /// <returns>Name of error type generated</returns>
        private string GetSimError(int errorId)
        {
            var result = "";
            switch (errorId)
            {
                case 0:
                    result = "SIMCONNECT_EXCEPTION_NONE";
                    break;
                case 2:
                    result = "SIMCONNECT_EXCEPTION_SIZE_MISMATCH";
                    break;
                case 3:
                    result = "SIMCONNECT_EXCEPTION_UNRECOGNIZED_ID";
                    break;
                case 4:
                    result = "SIMCONNECT_EXCEPTION_UNOPENED";
                    break;
                case 5:
                    result = "SIMCONNECT_EXCEPTION_VERSION_MISMATCH";
                    break;
                case 6:
                    result = "SIMCONNECT_EXCEPTION_TOO_MANY_GROUPS";
                    break;
                case 7:
                    result = "SIMCONNECT_EXCEPTION_NAME_UNRECOGNIZED";
                    break;
                case 8:
                    result = "SIMCONNECT_EXCEPTION_TOO_MANY_EVENT_NAMES";
                    break;
                case 9:
                    result = "SIMCONNECT_EXCEPTION_EVENT_ID_DUPLICATE";
                    break;
                case 10:
                    result = "SIMCONNECT_EXCEPTION_TOO_MANY_MAPS";
                    break;
                case 11:
                    result = "SIMCONNECT_EXCEPTION_TOO_MANY_OBJECTS";
                    break;
                case 12:
                    result = "SIMCONNECT_EXCEPTION_TOO_MANY_REQUESTS";
                    break;
                case 13:
                    result = "SIMCONNECT_EXCEPTION_WEATHER_INVALID_PORT";
                    break;
                case 14:
                    result = "SIMCONNECT_EXCEPTION_WEATHER_INVALID_METAR";
                    break;
                case 15:
                    result = "SIMCONNECT_EXCEPTION_WEATHER_UNABLE_TO_GET_OBSERVATION";
                    break;
                case 16:
                    result = "SIMCONNECT_EXCEPTION_WEATHER_UNABLE_TO_CREATE_STATION";
                    break;
                case 17:
                    result = "SIMCONNECT_EXCEPTION_WEATHER_UNABLE_TO_REMOVE_STATION";
                    break;
                case 18:
                    result = "SIMCONNECT_EXCEPTION_INVALID_DATA_TYPE";
                    break;
                case 19:
                    result = "SIMCONNECT_EXCEPTION_INVALID_DATA_SIZE";
                    break;
                case 20:
                    result = "SIMCONNECT_EXCEPTION_DATA_ERROR";
                    break;
                case 21:
                    result = "SIMCONNECT_EXCEPTION_INVALID_ARRAY";
                    break;
                case 22:
                    result = "SIMCONNECT_EXCEPTION_CREATE_OBJECT_FAILED";
                    break;
                case 23:
                    result = "SIMCONNECT_EXCEPTION_LOAD_FLIGHTPLAN_FAILED";
                    break;
                case 24:
                    result = "SIMCONNECT_EXCEPTION_OPERATION_INVALID_FOR_OJBECT_TYPE";
                    break;
                case 25:
                    result = "SIMCONNECT_EXCEPTION_ILLEGAL_OPERATION";
                    break;
                case 26:
                    result = "SIMCONNECT_EXCEPTION_ALREADY_SUBSCRIBED";
                    break;
                case 27:
                    result = "SIMCONNECT_EXCEPTION_INVALID_ENUM";
                    break;
                case 28:
                    result = "SIMCONNECT_EXCEPTION_DEFINITION_ERROR";
                    break;
                case 29:
                    result = "SIMCONNECT_EXCEPTION_DUPLICATE_ID";
                    break;
                case 30:
                    result = "SIMCONNECT_EXCEPTION_DATUM_ID";
                    break;
                case 31:
                    result = "SIMCONNECT_EXCEPTION_OUT_OF_BOUNDS";
                    break;
                case 32:
                    result = "SIMCONNECT_EXCEPTION_ALREADY_CREATED";
                    break;
                case 33:
                    result = "SIMCONNECT_EXCEPTION_OBJECT_OUTSIDE_REALITY_BUBBLE";
                    break;
                case 34:
                    result = "SIMCONNECT_EXCEPTION_OBJECT_CONTAINER";
                    break;
                case 35:
                    result = "SIMCONNECT_EXCEPTION_OBJECT_AI";
                    break;
                case 36:
                    result = "SIMCONNECT_EXCEPTION_OBJECT_ATC";
                    break;
                case 37:
                    result = "SIMCONNECT_EXCEPTION_OBJECT_SCHEDULE";
                    break;
                default:
                    result = "SIMCONNECT_EXCEPTION_ERROR";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Process SinVar result from MSFS2020
        /// </summary>
        /// <param name="sender">SimConnect</param>
        /// <param name="data">SimVariable value</param>
        void Sim_Data(object sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            var request = simVarRequests.SingleOrDefault(x => (int)x.ReqID == (int)data.dwRequestID);
            if (request != null && data?.dwData?.Length > 0 && DataReceived != null)
            {
                var result = new SimVarRequestResult { Request = request, Value = data.dwData[0] is SimVarString ? ((SimVarString)data.dwData[0]).Value : data.dwData[0] };
                DataReceived.DynamicInvoke(this, result);
            }
        }

        /// <summary>
        /// Called when connection to MSFS2020 is established or stopped
        /// </summary>
        /// <param name="sender">SimConnect</param>
        /// <param name="connected">Connection State</param>
        void Sim_Connection(object sender, bool connected)
        {
            if (connected)
            {
                // Can occasionally receive multiple connected notices from SimConnect, particularly when FS is starting
                if (!Connected)
                {
                    Connected = true;
                    WriteLog("Connected");
                    // New connection to FS - resubmit all previous SimVariable requests
                    lock (simVarRequests)
                        foreach (var request in simVarRequests)
                        {
                            AddSimVarRequest(request);
                        }
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
        /// <summary>
        /// Send any log messages to parent class, if logger connected.
        /// If not connected, use Console to write log
        /// </summary>
        /// <param name="sender">Class reporting this log message</param>
        /// <param name="message">Log message, including severity</param>
        private void WriteLog(object sender, LogMessage message)
        {
            if (LogReceived != null)
            {
                // Attempt to send log message to parent class
                try
                {
                    LogReceived.DynamicInvoke(sender, message);
                }
                catch { } // Parent error reporter has a problem - can't do anything about it here
            }
            else
            {
                // No parent logger, write to Console
                var strType = message.Type.ToString().Substring(0, 3);
                Console.WriteLine("[{0}] {1}", strType, message);
            }
        }

        /// <summary>
        /// Default helper for locally generated log messages
        /// </summary>
        /// <param name="message">Log message to report</param>
        /// <param name="type">Severity (default: Information)</param>
        private void WriteLog(string message, LogEventLevel type = LogEventLevel.Information)
        {
            WriteLog(this, new LogMessage { Message = message, Type = type });
        }
        #endregion
    }
}
