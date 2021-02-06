using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Items
{
    [XmlType("Animation")]
    public class AnimationImage : IAnimationItem
    {
        [XmlAttribute(AttributeName = "type")]
        public AnimationItemTypeEnum Type { get { return AnimationItemTypeEnum.Image; } set { } }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        [JsonConverter(typeof(ConcreteJSONConverter<AnimationTriggerClientRequest[]>))]
        [XmlElement("Triggers")]
        public AnimationXMLConverter Triggers { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public object LastAppliedValue { get; set; }
    }
}