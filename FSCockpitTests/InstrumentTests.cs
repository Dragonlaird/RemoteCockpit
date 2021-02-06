using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
            Assert.IsFalse(string.IsNullOrEmpty(config.Name));
        }

        }
    }
}
