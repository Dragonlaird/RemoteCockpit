using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations.Triggers
{
    public interface IAnimationTrigger
    {
        AnimationTriggerTypeEnum Type { get; set; }
        string Name { get; set; }
        [JsonConverter(typeof(ConcreteConverter<AnimationActionRotate[]>))]
        IAnimationAction[] Actions { get; set; }
    }
}
