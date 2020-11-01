using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RemoteCockpitClasses
{
    [DebuggerDisplay("\\{ClientRequest\\} {Name}")]
    public class ClientRequest
    {
        public string Name { get; set; }
        public string Unit { get; set; }
    }
}
