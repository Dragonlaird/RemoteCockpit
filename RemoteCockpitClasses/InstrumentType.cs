using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RemoteCockpitClasses
{
    public enum InstrumentType
    {
        [XmlEnum("0")] Altimeter = 0,
        [XmlEnum("1")] Airspeed_Indicator = 1,
        [XmlEnum("2")] Vertical_Speed_Indicator = 2,
        [XmlEnum("3")] Magnetic_Compass = 3,
        [XmlEnum("4")] Attitude_Indicator = 4,
        [XmlEnum("5")] Heading_Indicator = 5,
        [XmlEnum("6")] Turn_Indicator = 6,
        [XmlEnum("7")] VOR_Indicator = 7,
        [XmlEnum("8")] NDB_Indicator = 8,
        [XmlEnum("9")] GPS = 9,
        [XmlEnum("10")] Clock = 10,
        [XmlEnum("11")] Other = 11
    }
}
