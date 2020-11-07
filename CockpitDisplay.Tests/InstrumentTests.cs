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

namespace CockpitDisplay.Tests
{
    [TestClass]
    public class InstrumentTests
    {
        private ICockpitInstrument instrument;

        private Configuration GetConfiguration()
        {
            var config = new Configuration
            {
                Name = "Test Instrument for all aircraft",
                Author = "Dragonlaird",
                Aircraft = new string[] { "Cessna 152 ASOBO" },
                Type = InstrumentType.Airspeed_Indicator,
                BackgroundImagePath = ".\\Backgrounds\\Airspeed_Indicator.png",
                Animations = new AnimationDrawing[]
                {
                    new AnimationDrawing
                    {
                            Name = "Needle",
                            PointMap = new AnimationPoint[]
                            {
                                new AnimationPoint(0, 0),
                                new AnimationPoint(-1.25f, -1.25f),
                                new AnimationPoint(-2.5f, 20.0f),
                                new AnimationPoint(0, 27.0f),
                                new AnimationPoint(2.5f, 20.0f),
                                new AnimationPoint(1.25f, -1.25f)
                            },
                            RelativeX = 50,
                            RelativeY = 50,
                            FillColor = Color.CornflowerBlue,
                            FillMethod = System.Windows.Forms.VisualStyles.FillType.Solid,
                            ScaleMethod = AnimationScaleMethodEnum.Percent,
                            Triggers = new List<AnimationTriggerClientRequest>{
                                new AnimationTriggerClientRequest
                                {
                                    Type = AnimationTriggerTypeEnum.ClientRequest,
                                    Request = new ClientRequest
                                    {
                                        Name = "INDICATED AIRSPEED", Unit = "knots"
                                    },
                                    Actions = new List<IAnimationAction>
                                    {
                                        new AnimationActionRotate
                                        {
                                            CentrePoint = new AnimationPoint(50,50),
                                            MaximumValueExpected = 200,
                                            RotateClockwise = true
                                        }
                                    }
                                }
                            }
                        },
                    }
            };

            return config;
        }

        private Image LoadImage(string imagePath, double scaleFactor)
        {
            var imageFile = File.OpenRead(imagePath);
            var image = Image.FromStream(imageFile);
            var resizedImage = new Bitmap(image, new Size((int)(image.Width * scaleFactor), (int)(image.Height * scaleFactor)));
            return resizedImage;
        }

        //private PointF GetPoint(double length, double angleInDegrees)
        //{
        //    return new PointF((float)(length * Math.Sin(ConvertToRadians(angleInDegrees))), (float)(length * Math.Cos(ConvertToRadians(angleInDegrees))));
        //}

        //private double ConvertToRadians(double angle)
        //{
        //    while (angle < 0)
        //        angle += 360;
        //    while (angle >= 360)
        //        angle -= 360;
        //    return 0.01745 * angle;
        //}

        [TestMethod]
        public void CreateAltimeter()
        {
            instrument = new Generic_Altimeter();
            instrument.SetLayout(0, 0, 100, 100);
        }

        [TestMethod]
        public void CreateGeneriAirspeedcInstrument()
        {
            Form testForm = new Form();
            var scaleFactor = testForm.Width < testForm.Height ? testForm.Width / testForm.Height : testForm.Height / testForm.Width;
            instrument = new Generic_Instrument(GetConfiguration());
            var instrumentJson = JsonConvert.SerializeObject(GetConfiguration());
            instrument.SetLayout(50, 50, 200, 200);
            var clientRequests = instrument.RequiredValues;
            testForm.BackgroundImage = LoadImage(".\\CockpitBackgrounds\\Cockpit_Background.jpg", scaleFactor);
            testForm.Controls.Add(instrument.Control);
            testForm.Invalidate();
            testForm.Show();
            double lastValue = 0;
            for (var i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                lastValue += 10;
                instrument.ValueUpdate(new ClientRequestResult
                {
                    Request = clientRequests.First(),
                    Result = lastValue
                });
            }
        }
    }
}
