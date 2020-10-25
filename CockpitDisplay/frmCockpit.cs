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
using System.Reflection;

namespace CockpitDisplay
{
    public partial class frmCockpit : Form
    {
        private RemoteConnector connection;
        private List<ClientRequestResult> results;
        private delegate void SafeCallDelegate(object obj, string propertyName, object value);
        private delegate void SafeControlAddDelegate(Control ctrl, Control parent);
        public frmCockpit()
        {
            InitializeComponent();
            Initialize();
            // Only variable needed for this is "TITLE", to be notified whenever the aircraft type changes
            Thread.Sleep(3000);
            // Always ask to be notified when Connection to FlightSim is made or dropped
            RequestVariable(new ClientRequest
            {
                Name = "FS CONNECTION",
                Units = "bool"
            });
            // Also, ask to be notified whenever user starts flying a different aircraft
            RequestVariable(new ClientRequest
            {
                Name = "TITLE"
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
            results = new List<ClientRequestResult>();
            var ipAddress = ConfigurationManager.AppSettings.Get(@"ipAddress");
            var ipPort = int.Parse(ConfigurationManager.AppSettings.Get(@"ipPort"));
            var endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), ipPort);
            connection = new RemoteConnector(endPoint);
            connection.ReceiveData += ReceiveResultFromServer;
            connection.Connect();
        }

        private void ReceiveResultFromServer(object sender, ClientRequestResult requestResult)
        {
            // Received a new value for a request - identify which plugins need this variable and send it
            if(requestResult.Request.Name == "FS CONNECTION")
            {
                // Just informing us the current connection state to the Flight Simulator = display it on screen
                var existingConnectionState = this.Controls.Find("lblConnected", true);
                if(existingConnectionState == null || existingConnectionState.Count() == 0)
                {
                    AddControl(new Label { Name = "lblConnected", Width = 10, Height = 10, Anchor = AnchorStyles.Top | AnchorStyles.Right }, this);
                    existingConnectionState = this.Controls.Find("lblConnected", true);
                }
                foreach(Label connectionStateLabel in existingConnectionState)
                {
                    if ((bool)requestResult.Result)
                        UpdateObject(connectionStateLabel, "BackColor", Color.Green);
                    else
                        UpdateObject(connectionStateLabel, "BackColor", Color.Red);
                }
            }
        }

        private void AddControl(Control ctrl, Control parent)
        {
            if (parent.InvokeRequired)
            {
                var d = new SafeControlAddDelegate(AddControl);
                parent.Invoke(d, new object[] { ctrl, parent });
                return;
            }
            parent.Controls.Add(ctrl);
        }

        private void UpdateObject(object obj, string propertyName, object value)
        {
            if (obj != null && !string.IsNullOrEmpty(propertyName) && obj.GetType().GetProperty(propertyName) != null)
            {
                if (((Control)obj).InvokeRequired)
                {
                    var d = new SafeCallDelegate(UpdateObject);
                    ((Control)obj).Invoke(d, new object[] { obj, propertyName, value });
                    return;
                }
                if (obj.GetType().GetProperty(propertyName) != null)
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, value);
                    ((Control)obj).Update();
                }
            }
        }

        private void RequestVariable(ClientRequest request)
        {
            if(connection?.Connected == true)
            {
                connection.RequestVariable(request);
            }
        }
    }
}
