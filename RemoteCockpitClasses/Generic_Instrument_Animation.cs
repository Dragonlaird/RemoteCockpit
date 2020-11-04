using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RemoteCockpitClasses.Generic_Instrument
{
    [DebuggerDisplay("\\{Animation\\} {Request.Name}")]
    public class Animation
    {
        public string ImagePath { get; set; }
        public int AnimationPriority { get; set; }
        public ClientRequest Request { get; set; }
        public string AnimationType { get; set; }
    }
}
