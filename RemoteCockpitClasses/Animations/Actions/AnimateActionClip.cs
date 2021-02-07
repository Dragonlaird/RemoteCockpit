using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Actions
{
    public class AnimationActionClip : IAnimationAction
    {
        public AnimationActionTypeEnum Type { get; set; } = AnimationActionTypeEnum.Clip;
        public AnimateActionClipEnum Style { get; set; }
        public AnimationPoint StartPoint { get; set; }
        public AnimationPoint EndPoint { get; set; }
    }
}
