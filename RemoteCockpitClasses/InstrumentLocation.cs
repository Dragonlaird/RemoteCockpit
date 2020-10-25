using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RemoteCockpitClasses
{
    public class InstrumentLocation
    {
        public InstrumentType Type { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
