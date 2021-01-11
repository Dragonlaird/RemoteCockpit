using RemoteCockpitClasses.Animations.Actions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RemoteCockpitClasses.Animations.Triggers
{
    public class AnimationTriggerClientMouseClick : IAnimationTrigger
    {
        public AnimationTriggerTypeEnum Type { get; set; }
        public string Name { get; set; }
        public IAnimationAction[] Actions { get; set; }
        public RectangleF ClickZone { get; set; }
        public float Value { get; set; }
        public float IncrementAmount { get; set; }
        public float MaxValue { get; set; }
        public float MinValue { get;set; }
    }
}
