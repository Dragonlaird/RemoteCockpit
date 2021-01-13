﻿using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations.Items
{
    public interface IAnimationItem
    {
        AnimationItemTypeEnum Type { get; set; }
        string Name { get; set; }
        //[JsonConverter(typeof(ConcreteConverter<AnimationTriggerClientRequest>))]
        IAnimationTrigger[] Triggers { get; set; }
        object LastAppliedValue { get; set; }
    }
}