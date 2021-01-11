using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations.Triggers
{
    public class AnimationTriggerClientRequest : IAnimationTrigger
    {
        public string Name { get; set; }
        public AnimationTriggerTypeEnum Type { get; set; }
        public ClientRequest Request { get; set; }
        [JsonConverter(typeof(ConcreteConverter<AnimationActionRotate[]>))]
        public IAnimationAction[] Actions { get; set; }
    }
}
