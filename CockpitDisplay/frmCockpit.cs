using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using RemoteCockpitClasses;
using Newtonsoft.Json;
using System.Threading;

namespace CockpitDisplay
{
    public partial class frmCockpit : Form
    {
        private ClientConnection connection;
        public frmCockpit()
        {
            InitializeComponent();
            Initialize();
            Connect();
            // Only variable needed for this is "TITLE", to be notified whenever the aircraft type changes
            Thread.Sleep(1000);
            RequestVariable(new ClientRequest
            {
                Name="TITLE"
            });
            // Requesting more variables to test
            RequestVariable(new ClientRequest
            {
                Name = Name = "AMBIENT WIND VELOCITY",
            });
            RequestVariable(new ClientRequest
            {
                Name = Name = "AMBIENT WIND DIRECTION",
            });
        }

        public void Initialize()
        {
            var ipAddress = ConfigurationManager.AppSettings.Get(@"ipAddress");
            var ipPort = int.Parse(ConfigurationManager.AppSettings.Get(@"ipPort"));
            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), ipPort);
            connection = new ClientConnection { };
            Socket socket = new Socket(endPoint.Address.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            connection.Connection = socket;
        }

        private void Connect()
        {
            try
            {
                var ipAddress = ConfigurationManager.AppSettings.Get(@"ipAddress");
                var ipPort = int.Parse(ConfigurationManager.AppSettings.Get(@"ipPort"));
                var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), ipPort);
                connection.Connection.Connect(endPoint);
                connection.Connection.BeginReceive(new byte[1000], 0, 1000, 0,
                    new AsyncCallback(ReadCallback), connection.Connection);
            }
            catch(Exception ex)
            {

            }
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            Socket handler = (Socket)ar.AsyncState;
            StringBuilder sb = new StringBuilder();
            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                var buffer = new byte[bytesRead];
                // There  might be more data, so store the data received so far.  
                sb.Append(Encoding.ASCII.GetString(
                    buffer, 0, bytesRead));
                // Check for end-of-file tag. If it is not there, read
                // more data.  
                content = sb.ToString()?.Replace("\n", "").Replace("\0", "");
                // Get more data/requests
                handler.BeginReceive(new byte[1000], 0, 1000, 0,
                    new AsyncCallback(ReadCallback), handler);
            }
        }


        private void RequestVariable(ClientRequest request)
        {
            if(connection != null && connection?.Connection.Connected == true)
            {
                var requestString = JsonConvert.SerializeObject(request) + "\r\r";
                var requestBytes = Encoding.UTF8.GetBytes(requestString.ToArray());
                connection.Connection.Send(requestBytes,SocketFlags.None);
            }
        }

        private void ReceiveRemoteResponse(object sender, EventArgs e)
        {

        }
    }
}
