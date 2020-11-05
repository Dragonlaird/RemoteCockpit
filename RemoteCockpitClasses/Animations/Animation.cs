using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Generic_Instrument
{
    public class Animation
    {
        public IAnimationTrigger Trigger { get; set; }
        public IAnimationItem Item { get; set; }
        public IEnumerable<IAnimationAction> Actions { get; set; }
    }
}
