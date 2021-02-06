using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Actions
{
    [XmlType("Action")]
    public class AnimationActionClip : IAnimationAction
    {
        [XmlAttribute(AttributeName = "type")]
        public AnimationActionTypeEnum Type => AnimationActionTypeEnum.Clip;
        public AnimateActionClipEnum Style { get; set; }
        public AnimationPoint StartPoint { get; set; }
        public AnimationPoint EndPoint { get; set; }
    }
}
