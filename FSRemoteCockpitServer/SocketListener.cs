using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RemoteCockpitClasses;
using Serilog.Events;

namespace RemoteCockpitServer
{
    /// <summary>
    /// Listen for client connection requests, send and receive SimVar variable requests
    /// </summary>
    public class SocketListener:IDisposable
    {
        #region Private Variables
        private IPEndPoint _endPoint;
        private List<ClientConnection> clients;
        private List<SimVarRequestResult> latestValues;
        #endregion

        #region Event Handlers
        public EventHandler<LogMessage> LogReceived;
        public EventHandler<StateObject> ClientConnect;
        public EventHandler<StateObject> ClientDisconnect;
        public EventHandler<SimVarRequest> ClientRequest;
        public EventHandler<Exception> ClientError;
        #endregion

        #region ctor
        public SocketListener(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
            clients = new List<ClientConnection>();
            latestValues = new List<SimVarRequestResult>();
            latestValues.Add(new SimVarRequestResult { Request = new SimVarRequest { Name = "FS CONNECTION", Unit = "bool" }, Value = false });
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Start listening for new Client Connections
        /// </summary>
        public void Start()
        {
            WriteLog(string.Format("Starting Socket Listener: {0}:{1}", _endPoint.Address, _endPoint.Port));
            AsynchronousSocketListener.NewConnection += NewConnection;
            AsynchronousSocketListener.SocketError += SocketError;
            AsynchronousSocketListener.RequestReceived += RequestReceived;
            AsynchronousSocketListener.StartListening(_endPoint);
        }

        /// <summary>
        /// SimVar result updated, find any subscribed clients and send, if value has changed or send is forced
        /// </summary>
        /// <param name="result">Latest value for a SimVar Request</param>
        /// <param name="forceSend">Should value always sent, even if value is unchanged?</param>
        public void SendVariable(SimVarRequestResult result, bool forceSend = false)
        {
            // Find any Clients requesting this variable and send latest result to them
            if (latestValues.Any(x => x.Request.Name == result.Request.Name && x.Request.Unit == result.Request.Unit))
            {
                if (forceSend || latestValues.Any(x => x.Request.Name == result.Request.Name && x.Request.Unit == result.Request.Unit && x.Value != result.Value))
                {
                    // Value has been changed - rmember latest value and send to all subscribed clients
                    latestValues.Single(x => x.Request.Name == result.Request.Name && x.Request.Unit == result.Request.Unit).Value = result.Value;
                    var clientResponse = new ClientRequestResult { Request = new RemoteCockpitClasses.ClientRequest { Name = result.Request.Name, Unit = result.Request.Unit }, Result = result.Value };
                    var subscribedClients = clients.Where(x => x.Requests.Any(y => y.Name == result.Request.Name && y.Unit == result.Request.Unit));
                    var resultString = JsonConvert.SerializeObject(clientResponse) + "\r\r";
                    foreach (var client in subscribedClients)
                    {
                        WriteLog(string.Format("Sending Value: Client: {0}; Name: {1}; Value: {2}", client.Client.ConnectionID, result.Request.Name, result.Value));
                        try
                        {
                            client.Client.workSocket.Send(Encoding.UTF8.GetBytes(resultString));
                        }
                        catch//(Exception ex)
                        {
                            // Often happens if the connection is dropped - should be removed from subscribedClients list
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Stop listening for new Client connections
        /// </summary>
        public void Stop()
        {
            WriteLog("Stopping SocketListener", LogEventLevel.Information);
            AsynchronousSocketListener.StopListening();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// New Client connection attempt. Add to list of connected clients
        /// </summary>
        /// <param name="sender">Socket Handler</param>
        /// <param name="connection">Connection details</param>
        private void NewConnection(object sender, StateObject connection)
        {
            try
            {
                if (connection != null && connection.workSocket != null && connection.workSocket.Connected && !clients.Any(x => x.Client.ConnectionID == connection.ConnectionID))
                {
                    lock (clients)
                    {
                        clients.Add(new ClientConnection { Client = connection, Requests = new List<SimVarRequest> { new SimVarRequest { Name = "FS CONNECTION", Unit = "bool" } } });
                    }
                }
                if (ClientConnect != null)
                    ClientConnect.DynamicInvoke(this, connection);
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("NewConnection Error: {0}", ex.Message), LogEventLevel.Error);
            }
        }

        /// <summary>
        /// New SimVar request by client
        /// </summary>
        /// <param name="sender">Client requesting SimVar</param>
        /// <param name="request">SimVar requested</param>
        private void RequestReceived(object sender, ClientRequest request)
        {
            if (sender is StateObject)
            {
                var client = clients.SingleOrDefault(x => x.Client?.ConnectionID == ((StateObject)sender)?.ConnectionID);
                if (client != null)
                {
                    var simVarRequest = new SimVarRequest
                    {
                        Name = request.Name,
                        Unit = request.Unit
                    };
                    var previousRequest = latestValues.Where(x => x.Request.Name == simVarRequest.Name && x.Request.Unit == simVarRequest.Unit).Select(x => x.Request).SingleOrDefault();
                    if (previousRequest != null)
                        simVarRequest = previousRequest;
                    else
                        latestValues.Add(new SimVarRequestResult { Request = simVarRequest });
                    if (!client.Requests.Any(x => x.Name.ToUpper() == simVarRequest.Name.ToUpper() && x.Unit?.ToLower() == simVarRequest.Unit?.ToLower()))
                    {
                        // This client has not requested this variable before - add it
                        client.Requests.Add(simVarRequest);
                        // Submit request back to parent
                        if(ClientRequest != null && simVarRequest.ReqID == REQUEST.RQID) // No client has requested this variable before - Add it to FSConnector requests
                        {
                            try
                            {
                                ClientRequest.DynamicInvoke(sender, simVarRequest);
                            }
                            catch(Exception ex)
                            {
                                WriteLog(string.Format("Request Received Error: {0}", ex.Message), LogEventLevel.Error);
                                ClientError.DynamicInvoke(sender, ex);
                            }
                        }
                    }
                    if(simVarRequest.Name == "FS CONNECTION") // Always allow clients to requst current connection state
                        ClientRequest.DynamicInvoke(sender, simVarRequest);
                }
            }
        }

        /// <summary>
        /// Socket handler generated an error, report it
        /// </summary>
        /// <param name="sender">Socket Handler</param>
        /// <param name="ex">Exception raised</param>
        private void SocketError(object sender, Exception ex)
        {
            WriteLog(string.Format("Socket Error: {0}", ex.Message), LogEventLevel.Error);
        }
        #endregion

        #region Message and Log Handlers
        /// <summary>
        /// Send log message to parent class, or write to console if parent log handler not connected
        /// </summary>
        /// <param name="sender">Class submitting log message</param>
        /// <param name="message">Log message, including severity</param>
        private void WriteLog(object sender, LogMessage message)
        {
            if (LogReceived != null)
            {
                try
                {
                    LogReceived.DynamicInvoke(sender, message);
                }
                catch { } // Parent class log handler has an error - Nothing we can do about it here
            }
            else
            {
                var strType = message.Type.ToString().Substring(0, 3);
                Console.WriteLine("[{0}] {1}", strType, message);
            }
        }

        /// <summary>
        /// Default log message handler for local messages, calls WriteLog(sender, message)
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="type">Severity (default: Information)</param>
        private void WriteLog(string message, LogEventLevel type = LogEventLevel.Information)
        {
            WriteLog(this, new LogMessage { Message = message, Type = type });
        }

        /// <summary>
        /// Disconnect any clients, clear local variables
        /// </summary>
        public void Dispose()
        {
            lock (clients)
            {
                foreach (var client in clients)
                {
                    client?.Client?.workSocket?.Disconnect(false);
                }
                clients.Clear();
            }
            this.Stop();
        }
        #endregion
    }
}
