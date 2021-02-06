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

namespace RemoteCockpitServer
{
    public class Program
    {
        private static FSCockpitServer server;
        private static RemoteServer consoleServer;
        public static void Main(string[] args)
        {
            var logConfig = new LoggerConfiguration();
            logConfig
                .WriteTo.ColoredConsole()
                .WriteTo.EventLog("FS Remote Cockpit", "Application");
            if (args.Contains("logtofile"))
            {
                logConfig.WriteTo.File(Path.Combine(Path.GetTempPath(), string.Format("FSCockpitServer_{0:yyMMdd}", DateTime.Now))).ToString();
            }
            var log = logConfig.CreateLogger();
            if (args != null && args.Length == 1 && args[0].Length > 1
                    && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    default:
                        break;
                    case "install":
                    case "i":
                        log.Information("Installing Remote Cockpit as a Windows Service");
                        SelfInstaller.InstallMe();
                        break;
                    case "uninstall":
                    case "u":
                        log.Information("Uninstalling Remote Cockpit Windows Service");
                        SelfInstaller.UninstallMe();
                        break;
                    case "console":
                    case "c":
                    case "start":
                        log.Information("Starting Remote Cockpit Manually");
                        if (server?.IsRunning != true)
                        {
                            consoleServer = new RemoteServer(logConfig.CreateLogger());
                            consoleServer.Start();
                        }
                        break;
                    case "stop":
                        log.Information("Stopping Remote Cockpit Manually");
                        if (server?.IsRunning == true)
                        {
                            server.Stop();
                            while (server?.IsRunning == true)
                            {
                                Thread.Sleep(10);
                            }
                            server = null;
                        }
                        if(consoleServer?.IsRunning == true)
                        {
                            consoleServer.Stop();
                        }
                        break;
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new FSCockpitServer(log)
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}