using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RemoteCockpitClasses;

namespace RemoteCockpit
{
    public class RemoteCockpit
    {
        public EventHandler<LogMessage> LogReceived;
        private FSConnector fsConnector;
        private SocketListener listener;
        private List<SimVarRequestResult> requestResults;
        private bool AlwaysSendVariable { get; set; } = false;// Should variable always be retransmitted to clients, even if value hasn't changed?
        public RemoteCockpit()
        {
            InitializeComponent();
            StartConnector();
            StartListener();
        }

        private void InitializeComponent()
        {
            requestResults = new List<SimVarRequestResult>();
            // Add the first Request Variable for Connection State
            requestResults.Add(new SimVarRequestResult { Request = new SimVarRequest { Name = "FS CONNECTION", Unit = "bool" }, Value = false });
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

        }

        /// <summary>
        /// If a Client has requested a SimVar variable and this is the first request from any client - Submit the request to FSConnector
        /// </summary>
        /// <param name="sender">Listener</param>
        /// <param name="request">Variable requested</param>
        private void ClientRequest(object sender, SimVarRequest request)
        {
            // Remote Client has requested a variable - has this vaiable already been requested?
            if (fsConnector != null && (!requestResults.Any(x => x.Request.Name == request.Name && x.Request.Unit == request.Unit) || request.Name == "FS CONNECTION"))
            {
                if (!requestResults.Any(x => x.Request.Name == request.Name && x.Request.Unit == request.Unit))
                    // New request, add it to the list of known requests
                    lock (requestResults)
                    {
                        requestResults.Add(new SimVarRequestResult { Request = request, Value = null });
                    }
                // Send the request to FSConnector
                if(request.Name == "FS CONNECTION" && sender is StateObject)
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
