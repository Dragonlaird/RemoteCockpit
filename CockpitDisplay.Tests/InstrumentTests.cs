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
                Animations = new IAnimationItem[]
                {
                    new AnimationDrawing
                    {
                            Name = "Needle",
                            PointMap = new PointF[]
                            {
                                GetPoint(0, 0),
                                GetPoint(1.5, 1.5),
                                GetPoint(-24, 4),
                                GetPoint(-30, 0),
                                GetPoint(-24, -4),
                                GetPoint(-1.5, -1.5)
                            },
                            RelativeX = 50,
                            RelativeY = 50,
                            FillColor = Color.SkyBlue,
                            FillMethod = System.Windows.Forms.VisualStyles.FillType.Solid,
                            ScaleMethod = AnimationScaleMethodEnum.Percent,
                            ScaleSize = 0.4,
                            Triggers = new List<IAnimationTrigger>{
                                new AnimationTriggerClientRequest
                                {
                                    Type = AnimationTriggerTypeEnum.ClientRequest,
                                    Request = new ClientRequest
                                    {
                                        Name = "INDICATED AIRSPEED", Unit = "knots"
                                    },
                                    Actions = new List<IAnimationAction>
                                    {

                                    }
                                }
                            }
                        },
                    }
            };

            return config;
        }

        private PointF GetPoint(double length, double angleInDegrees)
        {
            return new PointF((float)(length * Math.Sin(ConvertToRadians(angleInDegrees))), (float)(length * Math.Cos(ConvertToRadians(angleInDegrees))));
        }

        private double ConvertToRadians(double angle)
        {
            while (angle < 0)
                angle += 360;
            while (angle >= 360)
                angle -= 360;
            return 0.01745 * angle;
        }

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
            instrument = new Generic_Instrument(GetConfiguration());
            instrument.SetLayout(50, 50, 200, 200);
            var clientRequest = instrument.RequiredValues;
            testForm.Controls.Add(instrument.Control);
            testForm.Invalidate();
            testForm.Show();
            
        }
    }
}
