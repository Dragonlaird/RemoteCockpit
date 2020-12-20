using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteCockpitClasses;
using InstrumentPlugins;
using System.Windows.Forms;
using RemoteCockpitClasses.Animations;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using Generic_Altitude_Indicator;

namespace CockpitDisplay.Tests
{
    [TestClass]
    public class InstrumentTests
    {
        private ICockpitInstrument instrument;

        private string GetConfiguration(string instrumentFilename = "Generic_Airspeed_Indicator.json")
        {
            var strPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..", "InstrumentPlugins", "GenericInstruments", instrumentFilename);
            return strPath;

        }

        private Image LoadImage(string imagePath, double scaleFactor)
        {
            var imageFile = File.OpenRead(imagePath);
            var image = Image.FromStream(imageFile);
            var resizedImage = new Bitmap(image, new Size((int)(image.Width * scaleFactor), (int)(image.Height * scaleFactor)));
            return resizedImage;
        }

        [TestMethod]
        public void CreateAltimeter()
        {
            instrument = new InstrumentPlugins.Generic_Altitude_Indicator();
            instrument.SetLayout(0, 0, 100, 100);
        }

        [TestMethod]
        public void CreateGeneriAirspeedcInstrument()
        {
            Form testForm = new Form();
            var scaleFactor = testForm.Width < testForm.Height ? testForm.Width / testForm.Height : testForm.Height / testForm.Width;
            instrument = new Generic_Instrument(testForm.Height, testForm.Width, GetConfiguration("Generic_Airspeed_Indicator.json"));
            var instrumentJson = JsonConvert.SerializeObject(GetConfiguration());
            instrument.SetLayout(50, 50, 200, 200);
            var clientRequests = instrument.RequiredValues;
            testForm.BackgroundImage = LoadImage(Path.Combine(Directory.GetCurrentDirectory(), "Layouts\\Dashboards", "Generic_Dashboard.png"), scaleFactor);
            testForm.Controls.Add(instrument.Control);
            testForm.Invalidate();
            testForm.Show();
            double lastValue = 40;
            for (var i = 0; i < 3; i++)
            {
                Thread.Sleep(2900);
                lastValue += new Random().NextDouble() * 70.0;
                instrument.ValueUpdate(new ClientRequestResult
                {
                    Request = clientRequests.First(),
                    Result = lastValue
                });
                testForm.Update();
            }
        }

        [TestMethod]
        public void CreateGPSInstrument()
        {
            Form testForm = new Form();
            var scaleFactor = testForm.Width < testForm.Height ? testForm.Width / testForm.Height : testForm.Height / testForm.Width;
            testForm.BackgroundImage = LoadImage(Path.Combine(Directory.GetCurrentDirectory(), "Layouts\\Dashboards", "Generic_Dashboard.png"), scaleFactor);
            instrument = new Generic_Instrument(150, 350, GetConfiguration("Generic_GPS.json"));
            testForm.Controls.Add(instrument.Control);
            testForm.Invalidate();
            testForm.Show();
            double lastValue = 40;
            var clientRequests = instrument.RequiredValues;

            for (var i = 0; i < 3; i++)
            {
                Thread.Sleep(1000);
                lastValue += new Random().NextDouble() * 70.0;
                instrument.ValueUpdate(new ClientRequestResult
                {
                    Request = clientRequests.First(),
                    Result = lastValue
                });
                testForm.Update();
            }
        }
    }
}
