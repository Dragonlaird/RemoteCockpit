using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteCockpitClasses.Generic_Instrument
{
    public class Configuration
    {
        public string Name { get; set; }
        public InstrumentType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public string BackgroundImagePath { get; set; }
        public string [] Aircraft { get; set; }
        public Animation[] Animations { get; set; }
        public ClientRequest[] ClientRequests
        {
            get
            {
                return Animations?.Select(x => x.Request).Distinct().ToArray();
            }
        }

    }
}
