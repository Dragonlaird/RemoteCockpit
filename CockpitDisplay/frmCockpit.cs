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
                Name="TITLE",
                Units=null
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
            }
            catch(Exception ex)
            {

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
    }
}
