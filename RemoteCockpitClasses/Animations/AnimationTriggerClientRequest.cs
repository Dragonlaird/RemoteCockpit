using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public class AnimationTriggerClientRequest : IAnimationTrigger
    {
        public AnimationTriggerTypeEnum Type { get; set; }
        public ClientRequest Request { get; set; }
        //[JsonConverter(typeof(ConcreteConverter<IAnimationAction>))]
        public IEnumerable<IAnimationAction> Actions { get; set; }
    }
}
