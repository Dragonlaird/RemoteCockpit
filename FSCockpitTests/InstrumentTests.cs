using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemoteCockpitClasses.Animations;
using System;
using System.IO;

namespace FSCockpitTests
{
    [TestClass]
    public class InstrumentTests
    {
        [TestMethod]
        public void LoadJSONConfigTest()
        {
            var pathToJSON = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\InstrumentPlugins\\GenericInstruments\\Generic_Attitude_Indicator.json");
            Configuration config = new Configuration();
            config.Load(pathToJSON);
        }

        [TestMethod]
        public void LoadXMLConfigTest()
        {
            var pathToXML= Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\InstrumentPlugins\\GenericInstruments\\Generic_Bearing_Indicator.xml");
            Configuration config = new Configuration();
            config.Load(pathToXML);
        }
    }
}
