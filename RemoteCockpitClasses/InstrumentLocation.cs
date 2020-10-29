using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RemoteCockpitClasses
{
    public class InstrumentLocation
    {
        public InstrumentType Type { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
    }
}
