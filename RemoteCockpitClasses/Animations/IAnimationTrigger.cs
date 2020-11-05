using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses
{
    public interface IAnimationTrigger
    {
        AnimationTriggerType Type { get; set; }
    }
}
