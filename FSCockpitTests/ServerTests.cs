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

        public ServerTests()
        {
            var logConfig = new LoggerConfiguration();
            logConfig.WriteTo.Console();
            log = logConfig.CreateLogger();
        }

        [TestMethod]
        public void TestCreateImplicitInstance()
        {
            var server = Activator.CreateInstance(typeof(RemoteCockpitServer.RemoteServer), log);
            Assert.IsNotNull(server);
        }

        [TestMethod]
        public void TestCreateExplicitInstance()
        {
            using (RemoteServer server = new RemoteServer(log))
            {
                Assert.IsNotNull(server);
            }
        }

        [TestMethod]
        public void TestStartInstanceFor5Seconds()
        {
            using (RemoteServer server = new RemoteServer(log))
            {
                Assert.IsNotNull(server);
                server.Start();
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
        }

        [TestMethod]
        public void TestConnectivity()
        {
            using (RemoteServer server = new RemoteServer(log))
            {
                Assert.IsNotNull(server);
                server.Start();
                Assert.IsTrue(server.IsRunning);
                using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555));
                    Assert.IsTrue(socket.Connected);
                    socket.Close();
                }
                server.Stop();
            }
        }

        [TestMethod]
        public void TestReceiveData()
        {
            using (RemoteServer server = new RemoteServer(log))
            {
                Assert.IsNotNull(server);
                server.Start();
                Assert.IsTrue(server.IsRunning);
                var result = string.Empty;
                DateTime endTime = DateTime.Now.AddSeconds(2);
                using (var client = new TcpClient())
                {
                    client.ReceiveTimeout = 2000;
                    client.SendTimeout = 2000;
                    client.NoDelay = true;
                    client.Connect("127.0.0.1", 5555);

                    using (NetworkStream networkStream = client.GetStream())
                    {
                        networkStream.ReadTimeout = 2000;
                        using (var reader = new StreamReader(networkStream, Encoding.UTF8))
                        {
                            while (reader!= null && client.Connected && networkStream.CanRead && endTime > DateTime.Now)
                            {
                                if (reader.Peek() != -1)
                                    result += (char)reader.Read();
                                else
                                    Thread.Sleep(10);
                            }
                        }

                    }
                }
                Assert.IsFalse(string.IsNullOrEmpty(result));
                Assert.IsTrue(result.StartsWith("{"));
                server.Stop();
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
