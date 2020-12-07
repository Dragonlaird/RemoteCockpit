using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public class AnimationActionMove : IAnimationAction
    {
        public AnimationActionTypeEnum Type { get; set; } = AnimationActionTypeEnum.MoveX;
        public float MaxValue { get; set; }
        public bool Invert { get; set; }
    }
}
