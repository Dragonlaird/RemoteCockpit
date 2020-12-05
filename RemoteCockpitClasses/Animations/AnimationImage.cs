using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;

namespace RemoteCockpitClasses.Animations
{
    //[JsonConverter(typeof(ConcreteConverter<AnimationImage[]>))]
    public class AnimationImage : IAnimationItem
    {
        public AnimationItemTypeEnum Type { get { return AnimationItemTypeEnum.Image; } set { } }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        [JsonConverter(typeof(ConcreteConverter<AnimationTriggerClientRequest[]>))]
        public IAnimationTrigger[] Triggers { get; set; }
        [JsonIgnore]
        public object LastAppliedValue { get; set; }
    }
}