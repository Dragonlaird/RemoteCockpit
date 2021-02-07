using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Actions
{
    public class AnimationActionRotate : IAnimationAction
    {
        public AnimationActionTypeEnum Type { get; set; } = AnimationActionTypeEnum.Rotate;
        /// <summary>
        /// The relative point to rotate around.
        /// Default: (50, 50), equates to (50% width, 50% height), which is the centre of the image
        /// </summary>
        public AnimationPoint CentrePoint { get; set; } = new AnimationPoint(50, 50);
        /// <summary>
        /// Should animation be clockwise or counter-clockwise for increments in our received value?
        /// </summary>
        public bool RotateClockwise { get; set; } = true;
        /// <summary>
        /// Maximum expected value of the incoming value, used to determine the correct angle.
        /// e.g. If the Dial travels from zero to 200 and the incoming value is expected between this range,
        /// our MaximumValueExpected would be 200.
        /// This would result in our rotation angle being calculated as:
        /// (360 * CurrentValue) / MaximumValueExpected (degrees)
        /// Thus: (360 * 0) / 200 = 0 degrees or (360 * 200) / 200 = 360 degrees
        /// </summary>
        public double MaximumValueExpected { get; set; } = 100;
        public double MinimumValueExpected { get; set; } = 0;
    }
}
