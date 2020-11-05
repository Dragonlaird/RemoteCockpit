using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteCockpitClasses;
using InstrumentPlugins;
using RemoteCockpitClasses.Generic_Instrument;

namespace CockpitDisplay.Tests
{
    [TestClass]
    class InstrumentTests
    {
        private ICockpitInstrument instrument;

        private Configuration GetConfiguration()
        {
            var config = new Configuration();

            return config;
        }

        [TestMethod]
        public void CreateAltimeter()
        {
            instrument = new Generic_Altimeter();
            instrument.SetLayout(0, 0, 100, 100);
        }

        [TestMethod]
        public void CreateGenericInstrument()
        {
            instrument = new Generic_Instrument(GetConfiguration());
            
        }
    }
}
