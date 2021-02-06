using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Actions;
using RemoteCockpitClasses.Animations.Triggers;

namespace RemoteCockpitClasses.Animations.Items

{
    //[JsonConverter(typeof(ConcreteConverter<AnimationDrawing[]>))]
    [XmlType("Animation")]
    public class AnimationDrawing : IAnimationItem
    {
        public AnimationItemTypeEnum Type { get { return AnimationItemTypeEnum.Drawing; } set { } }
        public string Name { get; set; }
        public AnimationPoint[] PointMap { get; set; }
        public FillTypeEnum FillMethod { get; set; }
        public Color FillColor { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        [JsonConverter(typeof(ConcreteJSONConverter<AnimationTriggerClientRequest[]>))]
        public IAnimationTrigger[] Triggers { get; set; }
        [JsonIgnore]
        public object LastAppliedValue { get; set; }
    }
}
