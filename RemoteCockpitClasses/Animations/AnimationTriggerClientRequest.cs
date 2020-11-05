using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses.Generic_Instrument
{
    public class AnimationTriggerClientRequest : IAnimationTrigger
    {
        public AnimationTriggerTypeEnum Type { get { return AnimationTriggerTypeEnum.ClientRequest; } set { } }
        public ClientRequest Request { get; set; }
    }
}
