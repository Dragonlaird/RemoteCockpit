using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteCockpitClasses
{
    public class LayoutDefinition
    {
        public string Name { get; set; }
        public InstrumentType[] InstrumentTypes
        {
            get
            {
                return Postions?.Select(x => x.Type).ToArray();
            }
        }
        public string Background { get; set; }
        public InstrumentLocation[] Postions { get; set; }
    }
}