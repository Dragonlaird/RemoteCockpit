using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using RemoteCockpitClasses;
using System.Configuration;
using System.Net;
using System.IO;
using System.Windows.Forms;
using Serilog;
using Serilog.Core;

namespace RemoteCockpitServer
{
    public partial class FSCockpitServer : ServiceBase
    {
        public EventHandler<LogMessage> LogReceived;
        private readonly RemoteServer server;

        public FSCockpitServer(Logger log)
        {
            server = new RemoteServer(log);
        }

        protected override void OnStart(string[] args)
        {
            server.Start();
        }

        protected override void OnStop()
        {
            server.Stop();
        }
        public bool IsRunning
        {
            get
            {
                return server?.IsRunning ?? false;
            }
        }
    }
}
