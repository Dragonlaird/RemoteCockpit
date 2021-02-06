using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace RemoteCockpitClasses.Animations.Items
{
    //[JsonConverter(typeof(ConcreteConverter<AnimationImage[]>))]
    [XmlType("Animation")]
    public class AnimationExternal : IAnimationItem
    {
        [XmlAttribute(AttributeName = "type")]
        public AnimationItemTypeEnum Type { get { return AnimationItemTypeEnum.External; } set { } }
        public string Name { get; set; }
        public string RemoteURL { get; set; }
        public string RemoteUsername { get; set; }
        public string RemoteToken { get; set; }
        public HttpMethod RequestMethod { get; set; } = HttpMethod.Post;
        public string RequestFormat { get; set; }
        [JsonConverter(typeof(ConcreteJSONConverter<AnimationTriggerClientRequest[]>))]
        [XmlElement("Triggers")]
        public AnimationXMLConverter Triggers { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public object LastAppliedValue { get; set; }
    }
}