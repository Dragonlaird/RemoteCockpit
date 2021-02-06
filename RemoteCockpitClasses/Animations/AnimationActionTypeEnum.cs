using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations
{
    public enum AnimationActionTypeEnum
    {
        [XmlEnum("0")] Rotate = 0,
        [XmlEnum("1")] MoveX = 1,
        [XmlEnum("2")] MoveY = 2,
        [XmlEnum("3")] Clip = 3
    }
}
