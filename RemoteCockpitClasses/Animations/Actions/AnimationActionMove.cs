using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Actions
{
    [XmlType("Action")]
    public class AnimationActionMove : IAnimationAction
    {
        [XmlAttribute(AttributeName = "type")]
        public AnimationActionTypeEnum Type { get; set; } = AnimationActionTypeEnum.MoveX;
        public float MaxValue { get; set; } = 100;
        public float MinValue { get; set; } = 0;
        public bool Invert { get; set; } = false;
    }
}
