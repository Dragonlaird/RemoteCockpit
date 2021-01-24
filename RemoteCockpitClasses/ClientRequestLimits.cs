using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses
{
    public class ClientRequestLimits
    {
        public ClientRequest Request { get; set; }
        public double Max { get; set; } = double.MaxValue;
        public double Min { get; set; } = double.MinValue;
    }
}
