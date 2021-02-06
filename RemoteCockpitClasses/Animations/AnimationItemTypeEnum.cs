using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations
{
    public enum AnimationItemTypeEnum
    {
        [XmlEnum("0")] Image = 0,
        [XmlEnum("1")] Drawing = 1,
        [XmlEnum("2")] External = 2
    }
}
