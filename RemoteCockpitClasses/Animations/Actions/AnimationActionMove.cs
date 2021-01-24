using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Animations.Actions
{
    public class AnimationActionMove : IAnimationAction
    {
        public AnimationActionTypeEnum Type { get; set; } = AnimationActionTypeEnum.MoveX;
        public float MaxValue { get; set; } = 100;
        public float MinValue { get; set; } = 0;
        public bool Invert { get; set; } = false;
    }
}
