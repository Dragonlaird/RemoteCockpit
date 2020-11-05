using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses
{
    public class Animation
    {
        public IAnimationTrigger Trigger { get; set; }
        public IAnimationItem Item { get; set; }
        public IEnumerable<IAnimationAction> Actions { get; set; }
    }
}
