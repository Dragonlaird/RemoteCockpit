using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public interface IAnimationItem
    {
        AnimationItemTypeEnum Type { get; }
        string Name { get; set; }
    }
}
