using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Actions
{
    [JsonConverter(typeof(AnimationClassConverter))]
    public interface IAnimationAction
    {
        AnimationActionTypeEnum Type { get; set; }
    }
}
