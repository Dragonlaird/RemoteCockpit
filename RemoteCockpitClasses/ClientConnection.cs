using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses
{
    public class ClientConnection
    {
        public StateObject Client { get; set; }
        public List<SimVarRequest> Requests { get; set; }
    }
}
