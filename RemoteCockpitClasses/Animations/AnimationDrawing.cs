using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;

namespace RemoteCockpitClasses.Animations
{
    public class AnimationDrawing : IAnimationItem
    {
        public AnimationItemTypeEnum Type => AnimationItemTypeEnum.Drawing;
        public string Name { get; set; }
        public AnimationPoint[] PointMap { get; set; }
        public FillType FillMethod { get; set; }

        public Color FillColor { get; set; }
        public AnimationScaleMethodEnum ScaleMethod { get; set; }
        public double RelativeX { get; set; }
        public double RelativeY { get; set; }
        [JsonConverter(typeof(ConcreteConverter<List<AnimationTriggerClientRequest>>))]
        public IEnumerable<IAnimationTrigger> Triggers { get; set; }

        public object LastAppliedValue { get; set; }
    }
}
