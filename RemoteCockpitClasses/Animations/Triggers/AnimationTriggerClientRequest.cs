using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations.Triggers
{
    public class AnimationTriggerClientRequest : IAnimationTrigger
    {
        public string Name { get; set; }
        public AnimationTriggerTypeEnum Type { get; set; } = AnimationTriggerTypeEnum.ClientRequest;
        public ClientRequest Request { get; set; }
        public IAnimationAction[] Actions { get; set; }
    }
}
