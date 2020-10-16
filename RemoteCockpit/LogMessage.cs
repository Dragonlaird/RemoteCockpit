using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpit
{
    public class LogMessage
    {
        public string Message { get; set; }
        public EventLogEntryType Type { get; set; }
    }
}
