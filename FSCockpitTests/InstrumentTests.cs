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
            foreach (var pathToJSON in Directory.GetFiles("..\\..\\..\\InstrumentPlugins\\GenericInstruments", "*.json"))
            {
                //var pathToJSON = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\InstrumentPlugins\\GenericInstruments\\Generic_Attitude_Indicator.json");
                Configuration config = new Configuration();
                config.Load(pathToJSON);
                Assert.IsFalse(string.IsNullOrEmpty(config.Name));
            }
        }
    }
}
