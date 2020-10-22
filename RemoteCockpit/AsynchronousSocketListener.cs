using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteCockpit
{
    // State object for reading client data asynchronously  
    public class StateObject
    {
        public int ConnectionID { get; internal set; }
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received content
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
    }
    public class AsynchronousSocketListener
    {
        public static EventHandler<StateObject> NewConnection;
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
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
                }
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

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
                if (content.IndexOf("\r\r") > -1)
                {
                    // All the data has been read from the client.
                    // Echo the data back to the client.  
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}