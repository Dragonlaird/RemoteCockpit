using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace InstrumentPlugins
{
    public class Generic_Altimeter : ICockpitInstrument
    {
        public event EventHandler Disposed;

        private bool disposedValue = false;

        private Control control;

        private const int MinimumAltitude = 0;
        private const int MaximumAltitude = 10000;
        private int CurrentAltitude { get; set; } = -1;
        private int controlTop = 0;
        private int controlLeft = 0;
        private int controlHeight = 50;
        private int controlWidth = 50;
        private Point centre = new Point(0, 0);
        private int lastAltitude = 0;

        public Generic_Altimeter()
        {
            // Draw the initial outline once - after that, an overlay will be used to display altitude
            RedrawControl();
        }

        private void DrawControlForeground()
        {
            return;
            var needle = new PictureBox();
            needle.Name = "Needle";
            needle.BackColor = Color.Transparent;
            needle.Height = control.Height < control.Width ? control.Height : control.Width;
            needle.Width = control.Height < control.Width ? control.Height : control.Width;
            var g = needle.CreateGraphics();
            Pen pen = new Pen(Color.White, 1 + needle.Width / 50);
            g.DrawLine(pen, centre, new Point(centre.X, 0));
            CurrentAltitude = lastAltitude;
            needle.Paint += PaintNeedle;
            if (control.Controls["Needle"] != null)
                control.Controls.Remove(control.Controls["Needle"]);
            control.Controls.Add(needle);
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
        }

        private void RedrawControl()
        {
            if (control == null)
            {
                control = new PictureBox();
                control.Name = "Generic_Altimeter";
                control.Top = controlTop;
                control.Left = controlLeft;
                control.BackColor = Color.Transparent;
                control.ForeColor = Color.Transparent;
                control.Paint += Paint;
            }
            DrawControlBackground();
        }

        private Image RotateImage(Image img, float angle)
        {
            Image rotatedImage = (Image)img.Clone();
            //rotatedImage.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center in the matrix
                g.TranslateTransform(img.Width / 2, img.Height / 2);
                // Rotate
                g.RotateTransform(angle);
                // Restore rotation point in the matrix
                g.TranslateTransform(-img.Width / 2, -img.Height / 2);
                // Draw the image on the bitmap
                g.DrawImage(img, new Point(0, 0));
            }

            return rotatedImage;
        }

        public void ValueUpdate(ClientRequestResult value)
        {
            // Update our control to display latest value
            if(value.Request.Name == "INDICATED ALTITUDE")
            {
                CurrentAltitude = (int)(double)value.Result;
            }
            Paint(control, new PaintEventArgs(control.CreateGraphics(), control.DisplayRectangle));
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            if (!control.Controls.ContainsKey("Needle"))
                DrawControlForeground();
        }

        private void PaintNeedle(object sender, PaintEventArgs e)
        {
            if (!control.Controls.ContainsKey("Needle") || lastAltitude != CurrentAltitude)
                DrawControlForeground();
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

        public string[] Layouts
        {
            get
            {
                return new string[] { "" }; // Blank can be used on all layouts
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
                control.Top = controlTop;
                control.Left = controlLeft;
                control.Height = controlHeight;
                control.Width = controlWidth;
            }
            RedrawControl();
        }
    }
}
