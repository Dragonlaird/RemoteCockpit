using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoteCockpitServer;
using Serilog;
using Serilog.Core;
using System;
using System.Diagnostics;
using System.IO;
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
            if (server == null)
            {
                var logConfig = new LoggerConfiguration();
                logConfig.WriteTo.Console();
                log = logConfig.CreateLogger();
                server = new RemoteServer(log);
            }
        }

        private void StartServer()
        {
            if(server != null && !server.IsRunning)
            {
                server.Start();
            }
        }

        [TestMethod]
        public void TestCreateImplicitInstance()
        {
            if (server != null && server.IsRunning)
                server.Stop();
            var implicitServer = Activator.CreateInstance(typeof(RemoteCockpitServer.RemoteServer), log);
            Assert.IsNotNull(implicitServer);
        }

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
            server.Stop();
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
            server.Stop();
        }

        /// <summary>
        /// This test will only pass if MSFS2020 is installed on this computer.
        /// Otherwise, SimConnect fails to load and causes server not to shutdown all clients.
        /// This action is intended, no point in the server running if it has nothing to connect to.
        /// </summary>
        [TestMethod]
        public void TestReceiveData()
        {
            int connectionTimeoutMs = 3000;
            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("MSFS_SDK")))
            {
                StartServer();
                Assert.IsTrue(server.IsRunning);
                var result = string.Empty;
                DateTime endTime = DateTime.Now.AddMilliseconds(connectionTimeoutMs);
                using (var client = new TcpClient())
                {
                    client.ReceiveTimeout = connectionTimeoutMs;
                    client.SendTimeout = connectionTimeoutMs;
                    client.NoDelay = true;
                    client.Connect("127.0.0.1", 5555);

                    using (NetworkStream networkStream = client.GetStream())
                    {
                        networkStream.ReadTimeout = connectionTimeoutMs;
                        using (var reader = new StreamReader(networkStream, Encoding.UTF8))
                        {
                            while (reader != null && client.Connected && endTime > DateTime.Now)
                            {
                                if (networkStream.CanRead && networkStream.DataAvailable && reader.Peek() != -1)
                                    result += (char)reader.Read();
                                else
                                    Thread.Sleep(10);
                            }
                        }
                    }
                }
                server.Stop();
                Assert.IsFalse(string.IsNullOrEmpty(result));
                Assert.IsTrue(result.StartsWith("{"));
            }
        }
    }
}