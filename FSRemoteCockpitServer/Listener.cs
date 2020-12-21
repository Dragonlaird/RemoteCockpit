using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace RemoteCockpit
{
    public class Listener : IDisposable
    {
        private bool disposedValue = false;
        private CancellationToken token;
        private ZContext _context;
        private ZSocket _socket;
        private ZSocket _worker;
        private List<string> _requests = new List<string>();
        private readonly string _endPoint;
        public EventHandler<ClientRequest> MessageReceived;
        public EventHandler<LogMessage> LogReceived;
        private Task _listen;
        private static List<WorkerThread> _workerThreads;
        public Listener(IPEndPoint endPoint)
        {
            _endPoint = string.Format("{0}://{1}:{2}", endPoint.AddressFamily == AddressFamily.InterNetwork ? "tcp" : "udp", endPoint.Address.ToString(), endPoint.Port);
            Initialize();
        }
        public Listener(IPEndPoint endPoint, CancellationToken cancellationToken)
        {
            token = cancellationToken;
            _endPoint = string.Format("{0}://{1}:{2}", endPoint.AddressFamily == AddressFamily.InterNetwork ? "tcp" : "udp", endPoint.Address.ToString(), endPoint.Port);
            Initialize();
        }

        public void Initialize()
        {
            lock (_requests)
                _requests = new List<string>();
            _socket?.Dispose();
            _socket = null;
            _context?.Dispose();
            _context = null;
            _workerThreads = new List<WorkerThread>();
        }

        public void Start()
        {
            _context = new ZContext();
            _socket = new ZSocket(_context, ZSocketType.ROUTER);
            _worker = new ZSocket(_context, ZSocketType.DEALER);

            // Bind
            _socket.Bind(_endPoint); //"tcp://*:5555"
            _worker.Bind("inproc://backend");
            _listen = new Task(() =>
            {
                Listen();
            });
            _listen.Start();

            WriteLog("Started");
        }

        private void Listen()
        {
            while (_context != null && _socket != null && !disposedValue)
            {
                // Launch pool of worker threads, precise number is not critical

                for (int i = 0; i < 5; ++i)
                {
                    int j = i;
                    _workerThreads.Add(new WorkerThread
                    {
                        ThreadID = j,
                        Thread = new Thread(() => AsyncSrv_ServerWorker(this, _context, j))
                    }
                    );
                }
                foreach(var workerThread in _workerThreads)
                {
                    workerThread.Thread.Start();
                }
                // Connect backend to frontend via a proxy
                ZError error;
                if (!ZContext.Proxy(_socket, _worker, out error))
                {
                    if (error == ZError.ETERM)
                        return; // Interrupted
                    throw new ZException(error);
                }

/*                using (ZFrame request = _socket.ReceiveFrame())
                {
                    
                    var reqString = request.ReadString();
                    Console.WriteLine("Listener Received {0}", reqString);
                    MessageReceived.DynamicInvoke(this, reqString);
                }
*/
            }
        }
        static void AsyncSrv_ServerWorker(Listener listener, ZContext context, int i)
        {
            using (var worker = new ZSocket(context, ZSocketType.DEALER))
            {
                worker.Connect("inproc://backend");
                ZError error;
                ZMessage request;
                var rnd = new Random();
                while (listener.token == null || !listener.token.IsCancellationRequested)
                {
                    if (null == (request = worker.ReceiveMessage(out error)))
                    {
                        if (error == ZError.ETERM)
                            return; // Interrupted
                        throw new ZException(error);
                    }
                    using (request)
                    {
                        // The DEALER socket gives us the reply envelope and message
                        
                        string identity = request[0].ReadString();
                        string content = request[1].ReadString();
                        var clientRequest = new ClientRequest
                        {
                            ThreadID = i,
                            ClientIdentity = identity,
                            ReceivedMessage = content
                        };
                        lock (_workerThreads)
                        {
                            // Associate the connected client with this thread/socket, removing any previous associations
                            _workerThreads.ForEach(x => {
                                if (x.LastConnectedClientIdentity == identity && x.ThreadID != i)
                                    x.LastConnectedClientIdentity = null;
                            });
                            var workerThread = _workerThreads.First(x => x.ThreadID == i);
                            workerThread.LastConnectedClientIdentity = identity;
                            workerThread.Socket = worker;
                        }
                        listener.WriteLog(string.Format("Listener Received: [{0}] {1} => {2}", i, identity, content));
                        if (listener.MessageReceived != null)
                            listener.MessageReceived.DynamicInvoke(worker, clientRequest);
                    }
                }
            }
        }

        public static void Send(string identity,string message)
        {
            lock (_workerThreads)
            {
                // Find last worker socket this client was connected to (should still be connected)
                var workerThread = _workerThreads.FirstOrDefault(x => x.LastConnectedClientIdentity == identity);
                if (workerThread != null && workerThread.Thread != null && workerThread.Socket != null)
                {
                    using (var response = new ZMessage())
                    {
                        response.Add(new ZFrame(identity));
                        response.Add(new ZFrame(message));

                        ZError error;
                        if (!workerThread.Socket.Send(response, out error))
                        {
                            if (error == ZError.ETERM)
                                return; // Interrupted
                            throw new ZException(error);
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            WriteLog("Stopping");
            lock (_requests)
                _requests = null;
            _socket?.Dispose();
            _socket = null;
            _context?.Dispose();
            _context = null;
            _listen?.Dispose();
            WriteLog("Stopped");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stop();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void WriteLog(object sender, LogMessage message)
        {
            if (LogReceived != null)
            {
                LogReceived.DynamicInvoke(sender, message);
            }
            else
            {
                var strType = message.Type.ToString().Substring(0, 3);
                Console.WriteLine("[{0}] {1}", strType, message);
            }
        }

        private void WriteLog(string message, EventLogEntryType type = EventLogEntryType.Information)
        {
            WriteLog(this, new LogMessage { Message = message, Type = type });
        }
    }
}
