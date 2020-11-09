using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public class AnimationTriggerClientRequest : IAnimationTrigger
    {
        public AnimationTriggerTypeEnum Type { get; set; }
        public ClientRequest Request { get; set; }
        [JsonConverter(typeof(ConcreteConverter<List<AnimationActionRotate>>))]
        public IEnumerable<IAnimationAction> Actions { get; set; }
    }
}
