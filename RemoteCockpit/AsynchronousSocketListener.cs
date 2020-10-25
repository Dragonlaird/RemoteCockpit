using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RemoteCockpitClasses;
using Newtonsoft.Json;

namespace RemoteCockpit
{
    /// <summary>
    /// Original code from: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example
    /// </summary>
    // State object for reading client data asynchronously  


    public class AsynchronousSocketListener
    {
        private const string requestSeparator = "\r\r";
        public static EventHandler<StateObject> NewConnection;
        public static EventHandler<Exception> SocketError;
        public static EventHandler<ClientRequest> RequestReceived;

        private static int lastConnectionId = 0;
        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private static IPEndPoint _endPoint;
        private static Socket listener;

        public static bool IsRunning { get; set; } = false;
        public static int MaxConnections { get; private set; }

        public AsynchronousSocketListener(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
            IsRunning = false;
            MaxConnections = 100;
        }

        public static void StartListening()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
           // IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            //IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            // Create a TCP/IP socket.  
            listener = new Socket(_endPoint.Address.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(_endPoint);
                listener.Listen(MaxConnections);
                IsRunning = true;
                new Task(() =>
                {
                    while (IsRunning)
                    {
                        // Set the event to nonsignaled state.  
                        allDone.Reset();

                        // Start an asynchronous socket to listen for connections.  
                        listener.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            listener);

                        // Wait until a connection is made before continuing.  
                        allDone.WaitOne();
                    }
                }).Start();
            }
            catch (Exception ex)
            {
                ErrorHandler(listener, ex);
            }
        }

        public static void StopListening()
        {
            IsRunning = false;
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            state.ConnectionID = lastConnectionId++;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
            if (NewConnection != null)
                try
                {
                    NewConnection.DynamicInvoke(AsynchronousSocketListener._endPoint.GetType(), state);
                }
                catch(Exception ex)
                {
                    // Stop any issues with the controlling service causing the listener to crash
                    ErrorHandler(handler, ex);
                }
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            try
            {
                // Retrieve the state object and the handler socket  
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));
                    // Check for end-of-file tag. If it is not there, read
                    // more data.  
                    content = state.sb.ToString()?.Replace("\n", "");
                    if (content.IndexOf(requestSeparator) > -1)
                    {
                        // All the data has been read from the client.
                        if (RequestReceived != null)
                        {
                            try
                            {
                                foreach (var request in content.Split(new string[] { requestSeparator }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    try
                                    {
                                        RequestReceived.DynamicInvoke(state, JsonConvert.DeserializeObject<ClientRequest>(request));
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorHandler(state, ex);
                                    }
                                }
                                state.sb = new StringBuilder();
                                state.sb.Append(content.Substring(content.LastIndexOf(requestSeparator)));
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler(state, ex);
                            }
                        }
                    }
                    // Get more data/requests
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception ex)
            {

                ErrorHandler(listener, ex);
            }
        }

        private static void Send(Socket handler, String data)
        {
            if (handler.Connected)
            {
                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.  
                handler.BeginSend(byteData, 0, byteData.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
            else
            {
                // No-longer connected - drop the client connection
                handler?.Disconnect(false);
                handler?.Close();
                handler?.Dispose();
            }
        }

        private static void ErrorHandler(object sender, Exception ex)
        {
            if(SocketError != null)
            {
                try
                {
                    SocketError.DynamicInvoke(sender, ex);
                }
                catch { }
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Socket handler = null;
            try
            {
                // Retrieve the socket from the state object.  
                handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                ErrorHandler(handler, ex);
            }
        }
    }
}