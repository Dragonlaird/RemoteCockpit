using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public interface IAnimationItem
    {
        AnimationItemTypeEnum Type { get; set; }
        string Name { get; set; }
        [JsonConverter(typeof(ConcreteConverter<AnimationTriggerClientRequest>))]
        public AnimationScaleMethodEnum ScaleMethod { get; set; }
        IAnimationTrigger[] Triggers { get; set; }
        object LastAppliedValue { get; set; }
    }
}
