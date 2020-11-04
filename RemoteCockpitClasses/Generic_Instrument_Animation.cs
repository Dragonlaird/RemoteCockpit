using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Generic_Instrument
{
    public class Animation
    {
        public string ImagePath { get; set; }
        public int AnimationPriority { get; set; }
        public ClientRequest Request { get; set; }
        public string AnimationType { get; set; }
    }
}
