using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;

namespace RemoteCockpitClasses.Animations
{
    //[JsonConverter(typeof(ConcreteConverter<AnimationImage[]>))]
    public class AnimationExternal : IAnimationItem
    {
        public AnimationItemTypeEnum Type { get { return AnimationItemTypeEnum.External; } set { } }
        public string Name { get; set; }
        public string BackgroundImage { get; set; }
        public string RemoteURL { get; set; }
        public string RemoteUsername { get; set; } = "Dragonlaird";
        public string RemoteToken { get; set; } = "pk.eyJ1IjoiZHJhZ29ubGFpcmQiLCJhIjoiY2tpbGg3dDI3MGhwMDJzbGI5NDJxdm1pNiJ9.jtAEPKfPm5S1Be_ol6tODw";
        public HttpMethod RequestMethod { get; set; } = HttpMethod.Post;
        public string RequestFormat { get; set; }
        [JsonConverter(typeof(ConcreteConverter<AnimationTriggerClientRequest[]>))]
        public IAnimationTrigger[] Triggers { get; set; }
        [JsonIgnore]
        public object LastAppliedValue { get; set; }
    }
}