using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace RemoteCockpit
{
    public class WorkerThread
    {
        public int ThreadID { get; set; }
        public string LastConnectedClientIdentity { get; set; }
        public Thread Thread { get; set; }
        public ZSocket Socket { get; set; }
    }
}
