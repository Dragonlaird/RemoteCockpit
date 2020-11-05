using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses
{
    public class AnimationTriggerClientRequest : IAnimationTrigger
    {
        public AnimationTriggerType Type { get { return AnimationTriggerType.ClientRequest; } set { } }
        public ClientRequest Request { get; set; }
    }
}
