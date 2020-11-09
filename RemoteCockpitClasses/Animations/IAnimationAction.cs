using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public interface IAnimationAction
    {
        [JsonConverter(typeof(ConcreteConverter<List<AnimationActionRotate>>))]
        AnimationActionTypeEnum Type { get; }
    }
}
