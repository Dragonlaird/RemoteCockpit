using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public interface IAnimationTrigger
    {
        AnimationTriggerTypeEnum Type { get; set; }
        [JsonConverter(typeof(ConcreteConverter<AnimationActionRotate[]>))]
        IAnimationAction[] Actions { get; set; }
    }
}
