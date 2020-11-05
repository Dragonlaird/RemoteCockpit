using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses
{
    public interface IAnimationAction
    {
        AnimationTriggerType Type { get; set; }
    }
}
