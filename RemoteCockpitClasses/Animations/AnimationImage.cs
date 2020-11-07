using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;

namespace RemoteCockpitClasses.Animations
{
    public class AnimationImage : IAnimationItem
    {
        public AnimationItemTypeEnum Type => AnimationItemTypeEnum.Drawing;
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public AnimationScaleMethodEnum ScaleMethod { get; set; }
        public float ScaleSize { get; set; }
        public IEnumerable<IAnimationTrigger> Triggers { get; set; }
    }
}