using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RemoteCockpitClasses;
using Serilog;
using Serilog.Core;
using Serilog.Sinks;

namespace RemoteCockpitServer
{
    public class Program
    {
        private static FSCockpitServer server;
        private static RemoteServer consoleServer;
        private static Serilog.LoggerConfiguration logConfig;
        private static Logger log;
        public static void Main(string[] args)
        {
            logConfig = new Serilog.LoggerConfiguration();
            logConfig?
                .WriteTo.Console();
            try
            {
                logConfig?
                    .WriteTo.EventLog("FS Remote Cockpit", "Application");
            }
            catch { }
            if (args.Contains("logtofile"))
            {
                try
                {
                    logConfig?.WriteTo.File(Path.Combine(Path.GetTempPath(), string.Format("FSCockpitServer_{0:yyMMdd}", DateTime.Now))).ToString();
                }
                catch { }
            }
            log = logConfig.CreateLogger();
            //if (Environment.UserInteractive)
            //    args = new string[] { "-c" };
            if (args != null && args.Length == 1 && args[0].Length > 1
                    && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    default:
                        break;
                    case "install":
                    case "i":
                        log?.Information("Installing Remote Cockpit as a Windows Service");
                        SelfInstaller.InstallMe(log);
                        break;
                    case "uninstall":
                    case "u":
                        log?.Information("Uninstalling Remote Cockpit Windows Service");
                        SelfInstaller.UninstallMe(log);
                        break;
                    case "console":
                    case "c":
                    case "start":
                        log?.Information("Starting Remote Cockpit Manually");
                        consoleServer = new RemoteServer(log);
                        consoleServer.Start();
                        break;
                    case "stop":
                        log?.Information("Stopping Remote Cockpit Manually");
                        if (consoleServer?.IsRunning == true)
                        {
                            consoleServer.Stop();
                        }
                        consoleServer?.Dispose();
                        break;
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                server = new FSCockpitServer();
                server.LogReceived += WriteLog;
                ServicesToRun = new ServiceBase[]
                {
                    server
                };
                ServiceBase.Run(ServicesToRun);
            }
        }

        private static void WriteLog(object sender, LogMessage e)
        {
            try
            {
                log?.Write(e.Type, e.Message);
            }
            catch { }
        }
    }
}