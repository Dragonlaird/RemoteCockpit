using RemoteCockpit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using RemoteCockpitClasses;
using System.Configuration;
using System.Net;
using System.IO;
using System.Windows.Forms;
using Serilog;
using Serilog.Core;

namespace RemoteCockpit
{
    public partial class FSCockpitServer : ServiceBase
    {
        public EventHandler<LogMessage> LogReceived;
        private FSConnector fsConnector;
        private SocketListener listener;
        private List<SimVarRequestResult> requestResults;
        private bool AlwaysSendVariable { get; set; } = false;// Should variable always be retransmitted to clients, even if value hasn't changed?
        private int _updateFrequency = 2; // How may seconds between each SimConnect poll?
        private EventLog logger = null;
        private readonly Logger _log;
        public FSCockpitServer(Logger log)
        {
            _log = log;
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                logger = new EventLog("Application", "localhost", "FS Cockpit Server");
            }
            catch { }
            requestResults = new List<SimVarRequestResult>();
            // Add the first Request Variable for Connection State
            requestResults.Add(new SimVarRequestResult { Request = new SimVarRequest { Name = "FS CONNECTION", Unit = "bool" }, Value = false });
            requestResults.Add(new SimVarRequestResult { Request = new SimVarRequest { Name = "UPDATE FREQUENCY", Unit = "second" }, Value = _updateFrequency });
            requestResults.Add(new SimVarRequestResult { Request = new SimVarRequest { Name = "TITLE", Unit = "string" }, Value = "None" });
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            Stop();
        }

        public void Start()
        {
            WriteLog(this, new LogMessage { Message = "FSCockpit Starting", Type = System.Diagnostics.EventLogEntryType.Information });
            StartConnector();
            StartListener();
            WriteLog(this, new LogMessage { Message = "FSCockpit Started", Type = System.Diagnostics.EventLogEntryType.Information });
        }

        public new void Stop()
        {
            WriteLog(this, new LogMessage { Message = "FSCockpit Stopping", Type = System.Diagnostics.EventLogEntryType.Information });
            fsConnector?.Stop();
            listener?.Stop();
            WriteLog(this, new LogMessage { Message = "FSCockpit Stopped", Type = System.Diagnostics.EventLogEntryType.Information });
        }

        private void StartConnector()
        {
            fsConnector = new FSConnector();
            fsConnector.LogReceived += WriteLog;
            fsConnector.DataReceived += MessageReceived;
            fsConnector.ConnectionStateChange += ConnectionStateChanged;
            //var tempRequest = new SimVarRequest { Name = "AMBIENT WIND VELOCITY" };
            //fsConnector.RequestVariable(tempRequest);
            //tempRequest = new SimVarRequest { Name = "AMBIENT WIND DIRECTION" };
            //fsConnector.RequestVariable(tempRequest);
            //fsConnector.ValueRequestInterval = 3;
            //var tempRequest = new SimVarRequest { Name = "Title", Unit = null };
            //fsConnector.RequestVariable(tempRequest);
            fsConnector.Start();
            try
            {
                _updateFrequency = fsConnector.ValueRequestInterval;
                requestResults.First(x => x.Request.Name == "UPDATE FREQUENCY" && x.Request.Unit == "second").Value = _updateFrequency;
            }
            catch { }

        }

        private void StartListener()
        {
            var ipAddress = ConfigurationManager.AppSettings.Get(@"ipAddress");
            var ipPort = int.Parse(ConfigurationManager.AppSettings.Get(@"ipPort"));
            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), ipPort);
            listener = new SocketListener(endPoint);
            listener.LogReceived += WriteLog;
            listener.ClientConnect += ClientConnect;
            listener.ClientRequest += ClientRequest;
            listener.Start();
        }

        private void ClientConnect(object sender, StateObject e)
        {
            // New client connection - always send current FS CONNECTION value
            var currentConnection = requestResults.SingleOrDefault(x => x.Request.Name == "FS CONNECTION" && x.Request.Unit == "bool");
            if (currentConnection != null)
                listener.SendVariable(currentConnection, true);
            // Fetch the current Update Frequency
            currentConnection = new SimVarRequestResult { Request = new SimVarRequest { Name = "UPDATE FREQUENCY", Unit = "second" }, Value = _updateFrequency };
            if (currentConnection != null)
                listener.SendVariable(currentConnection, true);

        }

        /// <summary>
        /// If a Client has requested a SimVar variable and this is the first request from any client - Submit the request to FSConnector.
        /// Always submit requests for FS CONNECTION to receive a response (confirms remote server is connected and responding)
        /// </summary>
        /// <param name="sender">Listener</param>
        /// <param name="request">Variable requested</param>
        private void ClientRequest(object sender, SimVarRequest request)
        {
            // Remote Client has requested a variable - has this vaiable already been requested?
            if (fsConnector != null && (!requestResults.Any(x => x.Request.Name == request.Name && x.Request.Unit == request.Unit) || request.Name == "FS CONNECTION" || request.Name == "UPDATE FREQUENCY"))
            {
                if (!requestResults.Any(x => x.Request.Name == request.Name && x.Request.Unit == request.Unit))
                    // New request, add it to the list of known requests
                    lock (requestResults)
                    {
                        requestResults.Add(new SimVarRequestResult { Request = request, Value = null });
                    }
                // Send the request to FSConnector
                if ((request.Name == "FS CONNECTION" || request.Name == "UPDATE FREQUENCY") && sender is StateObject)
                {
                    ClientConnect(sender, (StateObject)sender);
                }
                else
                {
                    fsConnector.RequestVariable(request);
                }
            }
        }

        /// <summary>
        /// If any variable values are received from FS, send to subscribed clients
        /// </summary>
        /// <param name="sender">FSConnector instance</param>
        /// <param name="e">SimVarRequestResult containing requested valiable, unit and value</param>
        private void MessageReceived(object sender, SimVarRequestResult e)
        {
            var lastRequest = requestResults.SingleOrDefault(x => x.Request.Name == e.Request.Name && x.Request.Unit == e.Request.Unit);
            if (lastRequest != null && ((lastRequest.Value == null && e.Value != null) || !lastRequest.Value.Equals(e.Value) || AlwaysSendVariable))
            {
                // Request has changed value or we are forcing retransmission - send to SockListener, for retransmission to remote clients
                lock (requestResults)
                {
                    // Update our local list to the latest value
                    requestResults[requestResults.IndexOf(lastRequest)].Value = e.Value;
                }
                WriteLog(this, new LogMessage { Message = string.Format("Value Received: {0} - {1} ({2}) = {3}", e.Request.ID, e.Request.Name, e.Request.Unit, e.Value), Type = System.Diagnostics.EventLogEntryType.Information });
                // Send this variable to Socket Listener to retransmit values to Remote Clients
                listener.SendVariable(e);
            }
        }

        /// <summary>
        /// Notification of when FS is connected/disconnected
        /// </summary>
        /// <param name="sender">FSConnector instance</param>
        /// <param name="connected">True = Connected; False = Disconnected;</param>
        private void ConnectionStateChanged(object sender, bool connected)
        {
            var connectionChanged = new SimVarRequestResult { Request = new SimVarRequest { Name = "FS CONNECTION", Unit = "bool" }, Value = connected };
            MessageReceived(this, connectionChanged);
        }

        private void WriteLog(object sender, LogMessage msg)
        {
            if (LogReceived != null)
            {
                LogReceived.DynamicInvoke(sender, msg);
            }
            else
            {
                var strType = msg.Type.ToString().Substring(0, 3);
                var logMsg = string.Format("({0}) [{1}] {2}", sender?.GetType().Name, strType, msg.Message);
                try
                {
                    switch (msg.Type)
                    {
                        case EventLogEntryType.Error:
                            _log.Error(logMsg);
                            break;
                        case EventLogEntryType.Warning:
                            _log.Warning(logMsg);
                            break;
                        default:
                            _log.Information(logMsg);
                            break;
                    }
                }
                catch
                {

                }
            }
        }
    }
}
