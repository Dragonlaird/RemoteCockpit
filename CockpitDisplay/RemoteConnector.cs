using Newtonsoft.Json;
using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CockpitDisplay
{
    public class RemoteConnector
    {
        private ClientConnection Client;
        private IPEndPoint _endPoint;

        public EventHandler<ClientRequestResult> ReceiveData;

        public bool Connected
        {
            get
            {
                return Client?.Connection?.Connected ?? false;
            }
        }

        public RemoteConnector(IPEndPoint endpoint)
        {
            _endPoint = endpoint;
        }

        private void InitializeComponent()
        {
            Client = new ClientConnection();
        }

        public void Connect()
        {
            Disconnect(); // Disconnect if already connected
            Client = Client ?? new ClientConnection();
            Client.Connection = new Socket(_endPoint.Address.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            Client.Connection.BeginConnect(_endPoint,
                new AsyncCallback(ConnectCallback), Client.Connection);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                // Once connected - listen for data asynchronously
                try
                {
                    // Create the state object.  
                    StateObject state = new StateObject();
                    state.workSocket = client;

                    // Begin receiving the data from the remote device.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;
                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    var result = state.sb.ToString().Replace("\n", "");
                    if(result.IndexOf("\r\r")> -1)
                    {
                        if(ReceiveData != null)
                        {
                            try
                            {
                                while (result.IndexOf("\r\r") > -1)
                                {
                                    var firstResult = result.Substring(0, result.IndexOf("\r\r") + 2);
                                    var requestResult = JsonConvert.DeserializeObject<ClientRequestResult>(firstResult);
                                    result = result.Substring(result.IndexOf("\r\r") + 2);
                                    state.sb = new StringBuilder(result);
                                    ReceiveData.DynamicInvoke(state, requestResult);
                                }
                            }
                            catch(Exception ex)
                            {

                            }
                        }
                    }
                    //  Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        if (ReceiveData != null)
                            ReceiveData.DynamicInvoke(this, JsonConvert.DeserializeObject<ClientRequestResult>(state.sb.ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Disconnect()
        {
            if (Connected)
            {
                Client.Connection.Disconnect(true);
            }
        }

        public void RequestVariable(ClientRequest request)
        {
            if (Connected)
            {
                var requestString = JsonConvert.SerializeObject(request) + "\r\r";
                var requestBytes = Encoding.UTF8.GetBytes(requestString.ToArray());
                this.Client.Connection.Send(requestBytes, SocketFlags.None);
            }
        }
    }
}
