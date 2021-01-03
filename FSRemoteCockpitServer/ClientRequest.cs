using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace RemoteCockpit
{
    public class ClientRequest
    {
        public int ThreadID { get; set; }
        public string ClientIdentity { get; set; }
        public string ReceivedMessage { get; set; }
    }
}
