using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Generic_Instrument
{
    public interface IAnimationItem
    {
        AnimationItemTypeEnum Type { get; set; }
    }
}
