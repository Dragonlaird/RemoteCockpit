﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public interface IAnimationAction
    {
        AnimationTriggerTypeEnum Type { get; set; }
    }
}
