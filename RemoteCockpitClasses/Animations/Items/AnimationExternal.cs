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
    public class AnimationExternal : IAnimationItem
    {
        public AnimationItemTypeEnum Type { get; set; } = AnimationItemTypeEnum.External;
        public string Name { get; set; }
        public string RemoteURL { get; set; }
        public string RemoteUsername { get; set; }
        public string RemoteToken { get; set; }
        public string RequestMethod { get; set; } = "GET"; // HttpMethod.Post;
        public string RequestFormat { get; set; }
        public IAnimationTrigger[] Triggers { get; set; }
        [JsonIgnore]
        public object LastAppliedValue { get; set; }
    }
}