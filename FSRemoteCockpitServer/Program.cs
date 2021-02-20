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

namespace RemoteCockpitServer
{
    /// <summary>
    /// Execute Remote Server as a Console App or Windows Service, with ability to Install/Uninstall Service
    /// </summary>
    public class Program
    {
        private static FSCockpitServer server;
        private static RemoteServer consoleServer;
        /// <summary>
        /// If Command Line Arguments are supplied, Remote Server runs as a Console App
        /// </summary>
        /// <param name="args">No Aarguments - Run as Windows Service
        /// -install [-i] Install Windows Service
        /// -uninstall [-u] Uninstall Windows Service
        /// -console [-c] [-start] Start Console App
        /// -stop Stop Console App</param>
        public static void Main(string[] args)
        {

            if (Environment.UserInteractive)
                args = new string[] { "-c" };
            if (args != null && args.Length == 1 && args[0].Length > 1
                    && (args[0][0] == '-' || args[0][0] == '/'))
            {
                switch (args[0].Substring(1).ToLower())
                {
                    default:
                        break;
                    case "install":
                    case "i":
                        SelfInstaller.InstallMe();
                        break;
                    case "uninstall":
                    case "u":
                        SelfInstaller.UninstallMe();
                        break;
                    case "console":
                    case "c":
                    case "start":
                        consoleServer = new RemoteServer();
                        consoleServer.Start();
                        break;
                    case "stop":
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
                Console.WriteLine(string.Format("{0:HH:mm:ss} [{2}] ({1}) {3}", DateTime.Now, sender.GetType().Name, (e.Type.ToString() + "    ").Substring(0, 4), e.Message));
            }
            catch { }
        }
    }
}