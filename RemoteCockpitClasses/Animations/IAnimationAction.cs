using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Generic_Instrument
{
    public interface IAnimationAction
    {
        AnimationTriggerTypeEnum Type { get; set; }
    }
}
