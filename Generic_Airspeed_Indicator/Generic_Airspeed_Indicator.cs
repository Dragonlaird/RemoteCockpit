using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Generic_Airspeed_Indicator;
using RemoteCockpitClasses;

namespace InstrumentPlugins
{
    public class Generic_Airspeed_Indicator : ICockpitInstrument
    {
        private Control control = null;
        private bool disposedValue = false;
        private int controlTop = 0;
        private int controlLeft = 0;
        private int controlHeight = 50;
        private int controlWidth = 50;
        private List<ClientRequestResult> values = new List<ClientRequestResult>();
        private Point centre;
        private delegate void SafeControlUpdateDelegate(Control ctrl);
        private double CurrentAirspeed = 0;
        private double LastAirspeed = -1;
        private double needleMoveSpeed = 0;
        private int animationTimeInMs = 3000;
        public event EventHandler<string> LogMessage;
        System.Timers.Timer animateTimer;

        public Generic_Airspeed_Indicator()
        {
            RedrawControl();
        }

        private void RedrawControl()
        {
            if (control == null)
            {
                control = new Panel();
                control.Name = "Custom_Airspeed_Indicator";
                control.Top = controlTop;
                control.Left = controlLeft;
                control.BackColor = Color.Transparent;
                control.ForeColor = Color.Transparent;
                control.Paint += Paint;
            }
            DrawControlBackground();
        }

        private void DrawControlBackground()
        {
            //var dial = new PictureBox();
            //dial.Name = "Dial";
            //dial.BackColor = Color.Transparent;
            var img = ImageLibrary.Airspeed_Background;
            var aspectRatio = (double)img.Height / img.Width;
            // Need to rescale dial to match the aspect ratio of the image
            var x = (int)(aspectRatio > 1 ? controlHeight : Math.Floor((double)controlWidth / aspectRatio));
            var y = (int)(aspectRatio > 1 ? Math.Floor((double)controlHeight / aspectRatio) : controlWidth);
            var resizedImage = new Bitmap(img, new Size(y, x));
            control.Height = resizedImage.Height;
            control.Width = resizedImage.Width;
            control.BackgroundImage = resizedImage;
            control.BackgroundImageLayout = ImageLayout.Center;
            centre = new Point(control.Width / 2, control.Height / 2);
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            // If we haven't already created the needle, do it now
            if (!control.Controls.ContainsKey("Needle"))
            {
                var needle = new PictureBox();
                needle.Name = "Needle";
                needle.BackColor = Color.Transparent;
                needle.Paint += PaintNeedle;
                control.Controls.Add(needle);
            }
        }

        private void PaintNeedle(object sender, PaintEventArgs e)
        {
            if(LastAirspeed != CurrentAirspeed)
            {
                var needle = (PictureBox)sender;
                needle.Top = 0;
                needle.Left = 0;
                needle.Height = control.Height;
                needle.Width = control.Width;
                needle.Enabled = false;

                // Draw the needles
                var pen = new Pen(Color.White, 1);
                //var g = e.Graphics;
                Bitmap bitmap = new Bitmap(needle.Width, needle.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                bitmap.MakeTransparent();
                Graphics graph = Graphics.FromImage(bitmap);
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graph.PixelOffsetMode = PixelOffsetMode.HighQuality;

                var nextAirspeed = LastAirspeed;
                if (nextAirspeed < CurrentAirspeed)
                {
                    nextAirspeed += needleMoveSpeed;
                    if (nextAirspeed > CurrentAirspeed)
                        nextAirspeed= CurrentAirspeed;
                }
                else
                {
                    nextAirspeed -= needleMoveSpeed;
                    if (nextAirspeed < CurrentAirspeed)
                        nextAirspeed = CurrentAirspeed;
                }

                // Reached our target position, no need for any more drawing
                if (nextAirspeed == CurrentAirspeed && animateTimer != null)
                {
                    animateTimer.Stop();
                    animateTimer?.Dispose();
                    animateTimer = null;
                }

                GraphicsPath gp = new GraphicsPath();
                var needlePosition = nextAirspeed / 200.0;
                var needleLength = (double)(needle.Height / 2) * 0.6;
                var needleAngle = 360.0 * needlePosition;
                var points = GetPoints(needleLength, needleAngle);
                gp.AddLines(points);
                graph.FillPath(pen.Brush, gp);

                needle.Image = bitmap;

                needle.BringToFront();

                LastAirspeed = nextAirspeed;
            }
        }

        private PointF[] GetPoints(double length, double angleInDegrees)
        {
            List<PointF> results = new List<PointF>();
            PointF[] points = {
                        GetPoint(length / 20, angleInDegrees - 110),
                        GetPoint(length * 0.8, angleInDegrees - 5),
                        GetPoint(length, angleInDegrees),
                        GetPoint(length * 0.8, angleInDegrees + 5),
                        GetPoint(length / 20, angleInDegrees + 110)
            };
            results.AddRange(points);

            return results.ToArray();
        }

        private PointF GetPoint(double length, double angleInDegrees)
        {
            return new PointF((float)(centre.X + length * Math.Sin(ConvertToRadians(angleInDegrees))), (float)(centre.Y - length * Math.Cos(ConvertToRadians(angleInDegrees))));
        }

        private double ConvertToRadians(double angle)
        {
            while (angle < 0)
                angle += 360;
            while (angle >= 360)
                angle -= 360;
            return 0.01745 * angle;
        }

        private void MoveNeedle(object sender, ElapsedEventArgs e)
        {
            if (control?.Controls.ContainsKey("Needle") == true)
            {
                var needle = control.Controls["Needle"];
                UpdateNeedle(control.Controls["Needle"]);
            }
            else
            {
                animateTimer?.Stop();
                animateTimer?.Dispose();
                animateTimer = null;
            }
        }

        private void UpdateNeedle(Control obj)
        {
            if (obj.InvokeRequired)
            {
                try
                {
                    var d = new SafeControlUpdateDelegate(UpdateNeedle);
                    obj.Invoke(d, new object[] { obj });
                }
                catch { }
                return;
            }
            try
            {
                PaintNeedle(obj, new PaintEventArgs(obj.CreateGraphics(), obj.DisplayRectangle));
            }
            catch { }
        }

        public IEnumerable<ClientRequest> RequiredValues
        {
            get
            {
                return new ClientRequest[]
                {
                    new ClientRequest
                    {
                        Name = "AIRSPEED INDICATED",
                        Unit = "knots"
                    }
                };
            }
        }

        public string[] Aircraft
        {
            get
            {
                return new string[] { "Generic" }; // Generic can be used on all layouts
            }
        }

        public DateTime PluginDate
        {
            get
            {
                return DateTime.MinValue; // Should always use Min Date for a Generic Plugin so it can be overridden by newer plugins
            }
        }

        public InstrumentType Type
        {
            get
            {
                return InstrumentType.Airspeed_Indicator;
            }
        }

        public Control Control
        {
            get
            {
                return control;
            }
        }

        public ISite Site { get; set; }

        public string Name => "Genereic Airspeed Indicator";

        public string Author => "Dragonlaird";

        public int UpdateFrequency
        {
            get
            {
                return animationTimeInMs / 1000;
            }
            set
            {
                animationTimeInMs = value * 1000;
            }
        }

        public event EventHandler Disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Any items that should be disposed of, remove them here
                }
                disposedValue = true;
                if (this.Disposed != null)
                    try
                    {
                        Disposed.DynamicInvoke();
                    }
                    catch { } // Don't care if it failed, it's being removed anyway
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
            if(this.Disposed != null)
                try
                {
                    this.Disposed.DynamicInvoke();
                }
                catch { }
        }

        public void SetLayout(int top, int left, int height, int width)
        {
            controlTop = top;
            controlLeft = left;
            controlHeight = height;
            controlWidth = width;
            if (control != null)
            {
                control.Top = top;
                control.Left = left;
            }
            RedrawControl();
        }

        public void ValueUpdate(ClientRequestResult value)
        {
            try
            {
                if (!values.Any(x => x.Request.Name == value.Request.Name && x.Request.Unit == value.Request.Unit))
                    lock (values)
                        values.Add(value);
                else
                    lock (values)
                        values.First(x => x.Request.Name == value.Request.Name && x.Request.Unit == value.Request.Unit).Result = value.Result;
                if (value.Request.Name == "AIRSPEED INDICATED")
                {
                    if ((double)value.Result != CurrentAirspeed)
                    {
                        CurrentAirspeed = (double)value.Result;
                        needleMoveSpeed = ((CurrentAirspeed > LastAirspeed ? (CurrentAirspeed - LastAirspeed) : (LastAirspeed - CurrentAirspeed)) / 200);
                        if (control?.Controls?.ContainsKey("Needle") == true)
                        {
                            //UpdateNeedle(control.Controls["Needle"]);
                            // Only need to start the timer if it isn't already running
                            if (animateTimer == null || !animateTimer.Enabled)
                            {
                                animateTimer = new System.Timers.Timer(30);
                                animateTimer.Elapsed += MoveNeedle;
                                animateTimer.AutoReset = true;
                                animateTimer.Enabled = true;
                                animateTimer.Start();
                            }
                        }
                    }
                }

            }
            catch
            {
                // Likely something is being disposed or updated - we'll need to add/update the value next time
            }
        }
    }
}
