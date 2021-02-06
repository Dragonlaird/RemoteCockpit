using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations
{
    public enum AnimationTriggerTypeEnum
    {
        [XmlEnum("0")] Timer = 0,
        [XmlEnum("1")] ClientRequest = 1,
        [XmlEnum("2")] MouseClick = 2
    }
}
