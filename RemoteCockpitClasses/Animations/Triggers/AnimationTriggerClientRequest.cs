using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Triggers
{
    [XmlType("Trigger")]
    public class AnimationTriggerClientRequest : IAnimationTrigger
    {
        [XmlAttribute(AttributeName = "type")]
        public string Name { get; set; }
        public AnimationTriggerTypeEnum Type { get; set; }
        public ClientRequest Request { get; set; }
        [JsonConverter(typeof(ConcreteJSONConverter<AnimationActionRotate[]>))]
        public IAnimationAction[] Actions { get; set; }
    }
}
