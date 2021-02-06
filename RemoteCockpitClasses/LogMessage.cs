using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;
using Serilog.Events;

namespace RemoteCockpitClasses
{
    public class LogMessage
    {
        public string Message { get; set; }
        public LogEventLevel Type { get; set; }
    }
}
