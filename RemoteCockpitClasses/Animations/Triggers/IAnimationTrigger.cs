using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Triggers
{
    public interface IAnimationTrigger
    {
        [XmlAttribute(AttributeName = "type")]
        AnimationTriggerTypeEnum Type { get; set; }
        string Name { get; set; }
        [JsonConverter(typeof(ConcreteJSONConverter<AnimationActionRotate[]>))]
        IAnimationAction[] Actions { get; set; }
    }
}
