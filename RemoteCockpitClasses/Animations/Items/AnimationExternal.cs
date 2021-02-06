﻿using Newtonsoft.Json;
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
        public AnimationItemTypeEnum Type { get { return AnimationItemTypeEnum.External; } set { } }
        public string Name { get; set; }
        public string RemoteURL { get; set; }
        public string RemoteUsername { get; set; }
        public string RemoteToken { get; set; }
        public HttpMethod RequestMethod { get; set; } = HttpMethod.Post;
        public string RequestFormat { get; set; }
        [JsonConverter(typeof(ConcreteJSONConverter<AnimationTriggerClientRequest[]>))]
        public IAnimationTrigger[] Triggers { get; set; }
        [JsonIgnore]
        public object LastAppliedValue { get; set; }
    }
}