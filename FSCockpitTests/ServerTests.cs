using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoteCockpitClasses;
using RemoteCockpitServer;
using Serilog;
using Serilog.Core;
using Serilog.Sinks;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace FSCockpitTests
{
    [TestClass]
    public class ServerTests
    {
        private Logger log;
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        private static RemoteServer server;

        public ServerTests()
        {
            StartLog();
            if (server == null)
            {
                server = new RemoteServer(log);
            }
        }

        private void StartLog()
        {
            if (log == null)
            {
                var logConfig = new LoggerConfiguration();
                logConfig
                    .WriteTo.Debug();
                log = logConfig.CreateLogger();
            }
        }

        private void StartServer()
        {
            StopServer();
            log.Information("Starting Server");
            server = null;
            server = new RemoteServer(log);
            server.Start();
        }

        private void StopServer()
        {
            if (server != null)
            {
                log.Information("Stopping Server");
                if (server.IsRunning)
                {
                    server.Stop();
                    log.Information("Server Stopped");
                }
                server.Dispose();
                log.Information("Server Disposed");
            }
        }

        //[TestMethod]
        //public void TestCreateImplicitInstance()
        //{
        //    if (server != null && server.IsRunning)
        //        server.Stop();
        //    var implicitServer = Activator.CreateInstance(typeof(RemoteCockpitServer.RemoteServer), log);
        //    Assert.IsNotNull(implicitServer);
        //}

        [TestMethod]
        public void TestCreateExplicitInstance()
        {
            //StartServer();
            Assert.IsNotNull(server);
        }

        [TestMethod]
        public void TestStartInstanceFor5Seconds()
        {
            StartServer();
            Assert.IsTrue(server.IsRunning);
            DateTime endTime = DateTime.Now.AddSeconds(5);
            while (server.IsRunning && endTime > DateTime.Now)
            {
                Thread.Sleep(100);
            }
            DateTime completedTime = DateTime.Now;
            StopServer();
            Assert.IsFalse(server.IsRunning);
            Assert.IsTrue(completedTime > endTime);
        }

        [TestMethod]
        public void TestConnectivity()
        {
            StartServer();
            Assert.IsTrue(server.IsRunning);
            using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555));
                Assert.IsTrue(socket.Connected);
                socket.Close();
            }
            StopServer();
        }

        /// <summary>
        /// This test will only pass if MSFS2020 is installed on this computer.
        /// Otherwise, SimConnect fails to load and causes server not to shutdown all clients.
        /// This action is intended, no point in the server running if it has nothing to connect to.
        /// </summary>
        [TestMethod]
        public void TestReceiveData()
        {
            StartLog();
            StartServer();
            Assert.IsTrue(server.IsRunning);
            int connectionTimeoutMs = 3000;
            string result = string.Empty;
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList.Where(x=> x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5555);

            // Create a TCP/IP  socket.    
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);
            log.Debug("Socket defined: {0}", sender.AddressFamily.ToString());

            // Connect the socket to the remote endpoint. Catch any errors.    
            try
            {
                byte[] bytes = new byte[1024];
                // Connect to Remote EndPoint  
                sender.Connect(remoteEP);
                if (sender.Connected)
                {
                    log.Debug("Socket connected to {0}", sender.RemoteEndPoint.ToString());
                    var endTime = DateTime.Now.AddMilliseconds(connectionTimeoutMs);

                    while (sender.Connected && string.IsNullOrEmpty(result) && endTime > DateTime.Now)
                    {
                        if (sender.Available > 0)
                        {
                            int bytesRec = sender.Receive(bytes);
                            result += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                    sender.Disconnect(false);
                    sender.Dispose();
                }
                else
                {
                    log.Warning("Connection failed");
                }
            }
            catch (Exception ex)
            {
                log.Error("Socket Error: {0}", ex.Message);
            }
            StopServer();
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.IsTrue(result.StartsWith("{"));
        }
    }
}