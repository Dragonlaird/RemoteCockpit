using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations
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
