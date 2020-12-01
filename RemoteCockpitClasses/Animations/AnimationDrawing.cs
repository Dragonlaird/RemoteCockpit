using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;

namespace RemoteCockpitClasses.Animations
{
    //[JsonConverter(typeof(ConcreteConverter<AnimationDrawing[]>))]
    public class AnimationDrawing : IAnimationItem
    {
        public AnimationItemTypeEnum Type { get { return AnimationItemTypeEnum.Drawing; } set { } }
        public string Name { get; set; }
        public AnimationPoint[] PointMap { get; set; }
        public FillType FillMethod { get; set; }

        public Color FillColor { get; set; }
        public AnimationScaleMethodEnum ScaleMethod { get; set; }
        public double RelativeX { get; set; }
        public double RelativeY { get; set; }
        [JsonConverter(typeof(ConcreteConverter<AnimationTriggerClientRequest[]>))]
        public IAnimationTrigger[] Triggers { get; set; }
        [JsonIgnore]
        public object LastAppliedValue { get; set; }
    }
}
