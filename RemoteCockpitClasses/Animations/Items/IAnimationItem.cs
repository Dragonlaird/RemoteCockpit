using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Items
{
    [JsonConverter(typeof(AnimationClassConverter))]
    public interface IAnimationItem
    {
        AnimationItemTypeEnum Type { get; set; }
        string Name { get; set; }
        IAnimationTrigger[] Triggers { get; set; }
        object LastAppliedValue { get; set; }
    }
}
