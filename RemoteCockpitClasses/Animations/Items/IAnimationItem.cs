using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Items
{
    public interface IAnimationItem
    {
        [XmlAttribute(AttributeName = "type")]
        AnimationItemTypeEnum Type { get; set; }
        string Name { get; set; }
        //[JsonConverter(typeof(ConcreteConverter<AnimationTriggerClientRequest>))]
        [XmlElement("Triggers")]
        AnimationXMLConverter Triggers { get; set; }
        object LastAppliedValue { get; set; }
    }
}
