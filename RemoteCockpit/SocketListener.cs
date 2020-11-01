﻿using System;
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

namespace RemoteCockpit
{
    public class SocketListener
    {
        private IPEndPoint _endPoint;
        private AsynchronousSocketListener _listener;
        public EventHandler<LogMessage> LogReceived;
        public EventHandler<StateObject> ClientConnect;
        public EventHandler<SimVarRequest> ClientRequest;
        public EventHandler<Exception> ClientError;

        private List<ClientConnection> clients;
        private List<SimVarRequestResult> latestValues;

        public SocketListener(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
            clients = new List<ClientConnection>();
            latestValues = new List<SimVarRequestResult>();
            latestValues.Add(new SimVarRequestResult { Request = new SimVarRequest { Name = "FS CONNECTION", Unit = "bool" }, Value = false });
        }

        public void Start()
        {
            WriteLog(string.Format("Starting Socket Listener: {0}:{1}", _endPoint.Address, _endPoint.Port));
            _listener = new AsynchronousSocketListener(_endPoint);
            AsynchronousSocketListener.NewConnection += NewConnection;
            AsynchronousSocketListener.SocketError += SocketError;
            AsynchronousSocketListener.RequestReceived += RequestReceived;
            AsynchronousSocketListener.StartListening();
        }

        private void NewConnection(object sender, StateObject connection)
        {
            try
            {
                if (connection != null && connection.workSocket != null && connection.workSocket.Connected && !clients.Any(x => x.Client.ConnectionID == connection.ConnectionID))
                    lock (clients)
                    {
                        clients.Add(new ClientConnection { Client = connection, Requests = new List<SimVarRequest> { new SimVarRequest { Name = "FS CONNECTION", Unit = "bool" } } });
                    }

                if (ClientConnect != null)
                    ClientConnect.DynamicInvoke(this, connection);
            }
            catch (Exception ex)
            {

            }
        }

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
                                ClientError.DynamicInvoke(sender, ex);
                            }
                        }
                    }
                    if(simVarRequest.Name == "FS CONNECTION") // Always allow clients to requst current connection state
                        ClientRequest.DynamicInvoke(sender, simVarRequest);
                }
            }
        }

        public void SendVariable(SimVarRequestResult result, bool forceSend = false)
        {
            // Find any Clients requesting this variable and send latest result to them
            if(latestValues.Any(x=> x.Request.Name == result.Request.Name && x.Request.Unit == result.Request.Unit))
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
                        client.Client.workSocket.Send(Encoding.UTF8.GetBytes(resultString));
                    }
                }
            }
        }

        private void SocketError(object sender, Exception ex)
        {

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