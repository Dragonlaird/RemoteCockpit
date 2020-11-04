using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteCockpitClasses;
using InstrumentPlugins;

namespace CockpitDisplay.Tests
{
    [TestClass]
    class InstrumentTests
    {
        private ICockpitInstrument instrument;
        [TestMethod]
        public void CreateAltimeter()
        {
            instrument = new Generic_Altimeter();
            instrument.SetLayout(0, 0, 100, 100);
        }
    }
}
