﻿using RemoteCockpitClasses;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteCockpitServer
{
    /// <summary>
    /// Remove Cockpit Server - Runs as a Windows Service.
    /// Will check for MSFS20 running on local computer
    /// and listen for client connections via the network
    /// </summary>
    public class RemoteServer : IDisposable
    {
        #region Event Handlers
        public EventHandler<LogMessage> LogReceived;
        #endregion

        #region Private variables
        private FSConnector fsConnector;
        private SocketListener listener;
        private List<SimVarRequestResult> requestResults;
        private bool AlwaysSendVariable = false;// Should variable always be retransmitted to clients, even if value hasn't changed?
        private int _updateFrequency = 1000; // How may milliseconds between each SimConnect poll?
        private readonly Logger _log;
        private IPEndPoint endPoint = null;
        #endregion

        #region Public IsRunning flag
        public bool IsRunning = false;
        #endregion

        #region ctor
        public RemoteServer()
        {
            try
            {
                Initialize();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        public RemoteServer(Logger log)
        {
            try
            {
                _log = log;
                Initialize();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Prepare local values and classes for use
        /// </summary>
        private void Initialize()
        {
            try
            {
                // Read App.config settings
                if (ConfigurationManager.AppSettings.AllKeys.Any(x => x == "AlwaysSendVariables"))
                    bool.TryParse(ConfigurationManager.AppSettings["AlwaysSendVariables"], out AlwaysSendVariable);
                var ipAddress = ConfigurationManager.AppSettings.Get(@"ipAddress");
                var ipPort = int.Parse(ConfigurationManager.AppSettings.Get(@"ipPort"));
                IPAddress address = null;
                // Supplied an actual address?
                if (!IPAddress.TryParse(ipAddress, out address))
                {
                    // Not an IP Address - Check if machine name
                    var addresses = Dns.GetHostAddresses(ipAddress);
                    foreach (var addr in addresses)
                    {
                        if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && addr != IPAddress.Loopback)
                        {
                            address = addr;
                            break;
                        }
                    }
                }
                if (address == null)
                    address = IPAddress.Loopback;
                endPoint = new IPEndPoint(address, ipPort);
                WriteLog(this, new LogMessage { Message = string.Format("Listen on IP/Port: {0}/{1}", endPoint.Address.ToString(), endPoint.Port), Type = LogEventLevel.Information });
                // Add the default Request Variables for a new Connection State
                requestResults = new List<SimVarRequestResult>();
                requestResults.Add(new SimVarRequestResult { Request = new SimVarRequest { Name = "FS CONNECTION", Unit = "bool" }, Value = false });
                requestResults.Add(new SimVarRequestResult { Request = new SimVarRequest { Name = "UPDATE FREQUENCY", Unit = "millisecond" }, Value = _updateFrequency });
                requestResults.Add(new SimVarRequestResult { Request = new SimVarRequest { Name = "TITLE", Unit = "string" }, Value = "None" });
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        /// <summary>
        /// Check to see if MSFS2020 is running, if running, instantiate SimConnect communication
        /// </summary>
        private void StartConnector()
        {
            try
            {
                fsConnector = new FSConnector();
                fsConnector.LogReceived += WriteLog;
                fsConnector.DataReceived += MessageReceived;
                fsConnector.ConnectionStateChange += ConnectionStateChanged;
                fsConnector.Start();
                _updateFrequency = fsConnector.ValueRequestInterval;
                requestResults.First(x => x.Request.Name == "UPDATE FREQUENCY" && x.Request.Unit == "millisecond").Value = _updateFrequency;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }

        }

        /// <summary>
        /// Start listening for client connection requests on configured IP Address/Port
        /// </summary>
        private void StartListener()
        {
            try
            {
                listener = new SocketListener(endPoint);
                listener.LogReceived += WriteLog;
                listener.ClientConnect += ClientConnect;
                listener.ClientRequest += ClientRequest;
                listener.Start();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        /// <summary>
        /// Client connection request received
        /// </summary>
        /// <param name="sender">Connection Handler</param>
        /// <param name="e">Connection Request</param>
        private void ClientConnect(object sender, StateObject e)
        {
            try
            {
                // New client connection - always send current FS CONNECTION value
                var currentConnection = requestResults.SingleOrDefault(x => x.Request.Name == "FS CONNECTION" && x.Request.Unit == "bool");
                if (currentConnection != null)
                    listener.SendVariable(currentConnection, true);
                // Fetch the current Update Frequency
                currentConnection = new SimVarRequestResult { Request = new SimVarRequest { Name = "UPDATE FREQUENCY", Unit = "millisecond" }, Value = _updateFrequency };
                if (currentConnection != null)
                    listener.SendVariable(currentConnection, true);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        /// <summary>
        /// If a Client has requested a SimVar variable and this is the first request from any client - Submit the request to FSConnector.
        /// Always submit requests for FS CONNECTION to receive a response (confirms remote server is connected and responding)
        /// </summary>
        /// <param name="sender">Listener</param>
        /// <param name="request">Variable requested</param>
        private void ClientRequest(object sender, SimVarRequest request)
        {
            try
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
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        /// <summary>
        /// If any variable values are received from FS, send to subscribed clients
        /// </summary>
        /// <param name="sender">FSConnector instance</param>
        /// <param name="e">SimVarRequestResult containing requested valiable, unit and value</param>
        private void MessageReceived(object sender, SimVarRequestResult e)
        {
            try
            {
                var lastRequest = requestResults.SingleOrDefault(x => x.Request.Name == e.Request.Name && x.Request.Unit == e.Request.Unit);
                // Only send value to client if values is different from last value sent, or we configured to alsways send value
                if (lastRequest != null && ((lastRequest.Value == null && e.Value != null) || !lastRequest.Value.Equals(e.Value) || AlwaysSendVariable))
                {
                    // Request has changed value or we are forcing retransmission - send to SockListener, for retransmission to remote clients
                    lock (requestResults)
                    {
                        // Update our local list to the latest value
                        requestResults[requestResults.IndexOf(lastRequest)].Value = e.Value;
                    }
                    WriteLog(this, new LogMessage { Message = string.Format("Value Received: {0} - {1} ({2}) = {3}", e.Request.ID, e.Request.Name, e.Request.Unit, e.Value), Type = LogEventLevel.Information });
                    // Send this variable to Socket Listener to retransmit values to Remote Clients
                    listener.SendVariable(e);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        /// <summary>
        /// Notification of when FS is connected/disconnected
        /// </summary>
        /// <param name="sender">FSConnector instance</param>
        /// <param name="connected">True = Connected; False = Disconnected;</param>
        private void ConnectionStateChanged(object sender, bool connected)
        {
            try
            {
                var connectionChanged = new SimVarRequestResult { Request = new SimVarRequest { Name = "FS CONNECTION", Unit = "bool" }, Value = connected };
                MessageReceived(this, connectionChanged);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        /// <summary>
        /// Convert a supplied exception to a LogMessage and call WriteLog(sender, msg)
        /// </summary>
        /// <param name="ex">Exception to be logged</param>
        private void WriteLog(Exception ex)
        {
            var logMsg = string.Empty;
            while(ex != null)
            {
                logMsg += string.Format("\rError: {0}", ex.Message);
                ex = ex.InnerException;
            }
            logMsg = "An error occurred:" + logMsg;
            WriteLog(this, new LogMessage { Type = LogEventLevel.Error, Message = logMsg });
        }

        /// <summary>
        /// If parent has a log writer, send log message
        /// Also try writing to any supplied logger
        /// If no logger was supplied, rite to Console
        /// </summary>
        /// <param name="sender">Class generating this log entry</param>
        /// <param name="msg">Log message to write, including log severity</param>
        private void WriteLog(object sender, LogMessage msg)
        {
            if (LogReceived != null)
            {
                try
                {
                    LogReceived.DynamicInvoke(sender, msg);
                }
                catch
                {
                    // Parent class has an error - nothing we can do about it here
                }
            }
            var strType = msg.Type.ToString().Substring(0, 3);
            var logMsg = string.Format("({0}) [{1}] {2}", sender?.GetType().Name, strType, msg.Message);
            try
            {
                if (_log != null)
                    switch (msg.Type)
                    {
                        case LogEventLevel.Fatal:
                            _log.Fatal(logMsg);
                            break;
                        case LogEventLevel.Error:
                            _log.Error(logMsg);
                            break;
                        case LogEventLevel.Warning:
                            _log.Warning(logMsg);
                            break;
                        case LogEventLevel.Verbose:
                            _log.Verbose(logMsg);
                            break;
                        case LogEventLevel.Debug:
                            _log.Debug(logMsg);
                            break;
                        default:
                            _log.Information(logMsg);
                            break;
                    }
                else
                {
                    Console.WriteLine(msg);
                }
            }
            catch
            {
                // Logger has an error - nothing we can do about it now
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Start RemoteCockpit as a Windows Service
        /// </summary>
        public void Start()
        {
            IsRunning = true;
            try
            {
                WriteLog(this, new LogMessage { Message = "FSCockpit Starting", Type = LogEventLevel.Information });
                StartConnector();
                StartListener();
                WriteLog(this, new LogMessage { Message = "FSCockpit Started", Type = LogEventLevel.Information });
                if (Environment.UserInteractive)
                    while (this.IsRunning)
                    {
                        Thread.Sleep(10);
                    }
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        /// <summary>
        /// Stop Windows Service, if running
        /// </summary>
        public void Stop()
        {
            try
            {
                WriteLog(this, new LogMessage { Message = "FSCockpit Stopping", Type = LogEventLevel.Information });
                fsConnector?.Stop();
                listener?.Stop();
                WriteLog(this, new LogMessage { Message = "FSCockpit Stopped", Type = LogEventLevel.Information });
                IsRunning = false;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        /// <summary>
        /// Dispose any instanciated classes and clear any provate variables used
        /// </summary>
        public void Dispose()
        {
            if (IsRunning)
                Stop();
            fsConnector?.Dispose();
            listener?.Dispose();
            listener = null;
            endPoint = null;
            requestResults?.Clear();
            requestResults = null;
            fsConnector = null;
        }
        #endregion
    }
}
