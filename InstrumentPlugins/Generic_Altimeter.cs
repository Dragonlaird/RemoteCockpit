using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Timers;
using System.Windows.Forms;

namespace InstrumentPlugins
{
    public class Generic_Altimeter : ICockpitInstrument
    {
        public event EventHandler Disposed;
        private delegate void SafeControlUpdateDelegate(Control ctrl);

        private bool disposedValue = false;

        private Control control;

        private const int MinimumAltitude = 0;
        private const int MaximumAltitude = 10000;
        private double CurrentAltitude { get; set; } = 0;
        private int controlTop = 0;
        private int controlLeft = 0;
        private int controlHeight = 50;
        private int controlWidth = 50;
        private Point centre = new Point(0, 0);
        private double lastAltitude = -1;
        private double needleMoveSpeed = 0;
        private int animationTimeInMs = 3000;
        System.Timers.Timer animateTimer;
        public event EventHandler<string> LogMessage;

        public Generic_Altimeter()
        {
            // Draw the initial outline once - after that, an overlay will be used to display altitude
            RedrawControl();
        }

        private void DrawControlBackground()
        {
            //var dial = new PictureBox();
            //dial.Name = "Dial";
            //dial.BackColor = Color.Transparent;
            var img = ImageLibrary.Background;
            var aspectRatio = (double)img.Height / img.Width;
            // Need to rescale dial to match the aspect ratio of the image
            var x = (int)(aspectRatio > 1 ? controlHeight : Math.Floor((double)controlWidth / aspectRatio));
            var y = (int)(aspectRatio > 1 ? Math.Floor((double)controlHeight / aspectRatio) : controlWidth);
            var resizedImage = new Bitmap(img, new Size(y, x));
            control.BackgroundImage = resizedImage;
            control.BackgroundImageLayout = ImageLayout.Center;
            centre = new Point(control.Width / 2, control.Height / 2);
            control.BackColor = Color.Transparent;
            control.Height = resizedImage.Height;
            control.Width = resizedImage.Width;
            centre = new Point(control.Width / 2, control.Height / 2);
            control.Controls.Clear();
        }

        private void RedrawControl()
        {
            if (control == null)
            {
                control = new Panel();
                control.Name = "Generic_Altimeter";
                control.Top = controlTop;
                control.Left = controlLeft;
                control.BackColor = Color.Transparent;
                control.ForeColor = Color.Transparent;
                control.Paint += Paint;
            }
            DrawControlBackground();
        }


        public void ValueUpdate(ClientRequestResult value)
        {
            // Update our control to display latest value
            if (value.Request.Name == "INDICATED ALTITUDE")
            {
                if ((int)(double)value.Result != CurrentAltitude)
                {
                    CurrentAltitude = (int)(double)value.Result;
                    needleMoveSpeed = ((CurrentAltitude > lastAltitude ? ((double)CurrentAltitude - (double)lastAltitude) : ((double)lastAltitude - (double)CurrentAltitude)) / 2000);
                    if (control?.Controls?.ContainsKey("Needle") == true)
                    {
                        //UpdateNeedle(control.Controls["Needle"]);
                        // Only need to start the timer if it isn't already running
                        if (animateTimer == null || !animateTimer.Enabled)
                        {
                            animateTimer = new System.Timers.Timer(50);
                            animateTimer.Elapsed += MoveNeedle;
                            animateTimer.AutoReset = true;
                            animateTimer.Enabled = true;
                            animateTimer.Start();
                        }
                    }
                }
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
            // Only update the needle if it should move
            if (lastAltitude != CurrentAltitude && sender is PictureBox && ((PictureBox)sender).Name == "Needle")
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

                // Determine new needle step poisition by moving 5 feet
                var nextAltitude = lastAltitude;
                if(nextAltitude < CurrentAltitude)
                {
                    nextAltitude += needleMoveSpeed;
                    if (nextAltitude > CurrentAltitude)
                        nextAltitude = CurrentAltitude;
                }
                else
                {
                    nextAltitude -= needleMoveSpeed;
                    if(nextAltitude < CurrentAltitude)
                        nextAltitude = CurrentAltitude;
                }

                // Reached our target position, no need for any more drawing
                if (nextAltitude == CurrentAltitude && animateTimer != null)
                {
                    animateTimer.Stop();
                    animateTimer?.Dispose();
                    animateTimer = null;
                }

                // Update digital readout
                var digitalReadoutPen = new Pen(Color.Red, 1);
                var digitalReadoutTop = (double)control.Height * 0.28;
                var digitalReadoutLeft = (double)control.Width * 0.355;
                var digitalReadoutHeight = (double)control.Height * 0.175;
                var digitalReadoutWidth = (double)control.Width * 0.325;
                Rectangle textRect = new Rectangle((int)digitalReadoutLeft, (int)digitalReadoutTop, (int)digitalReadoutWidth, (int)digitalReadoutHeight);
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graph.PixelOffsetMode = PixelOffsetMode.HighQuality;
                var altText = string.Format("{0:0#,##0}", nextAltitude < 0 ? nextAltitude * -1 : nextAltitude);
                var fontSize = (float)(digitalReadoutWidth / 5);
                var digitalReadoutFont = new Font("Courier", fontSize, FontStyle.Bold);
                graph.DrawString(altText, digitalReadoutFont, nextAltitude < 0 ? Brushes.RosyBrown : Brushes.Gainsboro, textRect);

                // Short Needle
                GraphicsPath gp = new GraphicsPath();
                var altimeterNeedlePosition = nextAltitude / 10000.0;
                var altimeterNeedleLength = (double)(needle.Height / 2) * 3 / 5;
                var altimeterNeedleAngle = 360.0 * altimeterNeedlePosition;
                var points = GetPoints(altimeterNeedleLength, altimeterNeedleAngle);
                gp.AddLines(points);
                graph.FillPath(pen.Brush, gp);

                // Long Needle
                gp = new GraphicsPath();
                altimeterNeedlePosition = nextAltitude % 1000 / 1000.0;
                altimeterNeedleLength = (double)(needle.Height / 2) * 4 / 5;
                altimeterNeedleAngle = 360.0 * altimeterNeedlePosition;
                points = GetPoints(altimeterNeedleLength, altimeterNeedleAngle);
                gp.AddLines(points);
                graph.FillPath(pen.Brush, gp);

                needle.Image = bitmap;

                needle.BringToFront();
                lastAltitude = nextAltitude;
            }
        }

        private PointF[] GetPoints(double length, double angleInDegrees)
        {
            List<PointF> results = new List<PointF>();
            PointF[] points = {
                        GetPoint(length / 20, angleInDegrees - 110),
                        GetPoint(length * 0.8, angleInDegrees - 4),
                        GetPoint(length, angleInDegrees),
                        GetPoint(length * 0.8, angleInDegrees + 4),
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

        public InstrumentType Type
        {
            get
            {
                return InstrumentType.Altimeter;
            }
        }

        public IEnumerable<ClientRequest> RequiredValues
        {
            get
            {
                return new List<ClientRequest> { new ClientRequest { Name = "INDICATED ALTITUDE", Unit = "feet" } };
            }
        }

        public string[] Aircraft
        {
            get
            {
                return new string[] { "Generic" }; // Blank can be used on all layouts
            }
        }

        public DateTime PluginDate
        {
            get
            {
                return DateTime.MinValue; // Should always use Min Date for a Generic Plugin so it can be overridden by newer plugins
            }
        }

        public ISite Site { get; set; }

        public Control Control {
            get
            {
                return control;
            }
        }

        public string Name => "Generic Altimeter";

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
            lastAltitude = -1; // Force redraw of the needle
            RedrawControl();
        }
    }
}
