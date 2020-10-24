using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CockpitDisplay
{
    internal class ClientConnection
    {
        public int? ID { get; set; }
        public Socket Connection { get; set; }
        public IEnumerable<SimVarRequest> Requests { get; set; }
    }
}
