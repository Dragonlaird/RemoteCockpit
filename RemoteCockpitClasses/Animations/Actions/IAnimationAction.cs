using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Actions
{
    public interface IAnimationAction
    {
        [JsonConverter(typeof(ConcreteJSONConverter<AnimationActionRotate>))]
        [XmlAttribute(AttributeName = "type")]
        AnimationActionTypeEnum Type { get; }
    }
}
