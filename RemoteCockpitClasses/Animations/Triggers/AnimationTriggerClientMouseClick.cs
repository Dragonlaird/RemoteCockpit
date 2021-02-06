using RemoteCockpitClasses.Animations.Actions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Triggers
{
    [XmlType("Trigger")]
    public class AnimationTriggerClientMouseClick : IAnimationTrigger
    {
        [XmlAttribute(AttributeName = "type")]
        public AnimationTriggerTypeEnum Type { get; set; }
        public string Name { get; set; }
        [XmlElement("Actions")]
        public AnimationXMLConverter Actions { get; set; }
        public RectangleF ClickZone { get; set; }
        public float Value { get; set; }
        public float IncrementAmount { get; set; }
        public float MaxValue { get; set; }
        public float MinValue { get;set; }
    }
}
