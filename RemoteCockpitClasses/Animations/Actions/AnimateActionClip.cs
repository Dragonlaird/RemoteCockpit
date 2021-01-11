using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations.Actions
{
    public class AnimationActionClip : IAnimationAction
    {
        public AnimationActionTypeEnum Type => AnimationActionTypeEnum.Clip;
        public AnimateActionClipEnum Style { get; set; }
        public AnimationPoint StartPoint { get; set; }
        public AnimationPoint EndPoint { get; set; }
    }
}
