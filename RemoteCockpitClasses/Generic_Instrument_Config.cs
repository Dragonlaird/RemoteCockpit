using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    [DebuggerDisplay("\\{Configuration\\} {Name}")]
    public class Configuration
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public InstrumentType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public string BackgroundImagePath { get; set; }
        public string [] Aircraft { get; set; }
        public Animation[] Animations { get; set; }
        public ClientRequest[] ClientRequests
        {
            get
            {
                return Animations?
                    .Where(x => x.Trigger.Type == AnimationTriggerTypeEnum.ClientRequest)
                    .Select(x => ((AnimationTriggerClientRequest)x.Trigger).Request).Distinct().ToArray();
            }
        }

    }
}
