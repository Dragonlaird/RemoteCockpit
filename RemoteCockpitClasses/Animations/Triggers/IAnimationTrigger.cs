using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Triggers
{
    [JsonConverter(typeof(AnimationClassConverter))]
    public interface IAnimationTrigger
    {
        AnimationTriggerTypeEnum Type { get; set; }
        string Name { get; set; }
        IAnimationAction[] Actions { get; set; }
    }
}
