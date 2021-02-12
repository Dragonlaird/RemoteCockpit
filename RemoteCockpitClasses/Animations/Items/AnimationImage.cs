using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Items
{
    public class AnimationImage : IAnimationItem
    {
        public AnimationItemTypeEnum Type { get; set; } = AnimationItemTypeEnum.Image;
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public IAnimationTrigger[] Triggers { get; set; }
        //[JsonIgnore]
        //public object LastAppliedValue { get; set; }
    }
}