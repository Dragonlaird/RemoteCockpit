using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CockpitPlugins
{
    public class Generic_Altimeter : ICockpitInstrument
    {
        public event EventHandler Disposed;

        private bool disposedValue = false;

        private PictureBox control;

        private const int MinimumAltitude = 0;
        private const int MaximumAltitude = 10000;
        private int CurrentAltitude { get; set; } = 0;
        private int controlTop = 0;
        private int controlLeft = 0;
        private int controlHeight = 50;
        private int controlWidth = 50;

        public Generic_Altimeter()
        {
        }


        public void ValueUpdate(ClientRequestResult value)
        {
            // Update our control to display latest value
            if(value.Request.Name == "INDICATED ALTITUDE")
            {
                CurrentAltitude = (int)value.Result;
            }
            Paint(control, new PaintEventArgs(control.CreateGraphics(), control.DisplayRectangle));
        }

        private void Paint(object sender, PaintEventArgs e)
        {
            var ctrl = (Control)sender;
            if (ctrl.Controls.Count == 0)
            {
                var needle = new PictureBox();
                needle.Paint += PaintNeedle;
                ctrl.Controls.Add(needle);
            }
            //return;
            Graphics g = e.Graphics;
            var pen = new Pen(Color.Black, 5);
            Rectangle rect = new Rectangle(
                ctrl.DisplayRectangle.X + (int)pen.Width,
                ctrl.DisplayRectangle.Y + (int)pen.Width,
                ctrl.DisplayRectangle.Width > ctrl.DisplayRectangle.Height ? ctrl.DisplayRectangle.Height : ctrl.DisplayRectangle.Width,
                ctrl.DisplayRectangle.Height > ctrl.DisplayRectangle.Width ? ctrl.DisplayRectangle.Width : ctrl.DisplayRectangle.Height);
            // reduce the rect dimensions so it isn't clipped when added to the DisplayRectangle
            rect.Width -= 2 * (int)pen.Width;
            rect.Height -= 2 * (int)pen.Width;
            var brush = new SolidBrush(Color.Gainsboro);
            g.FillEllipse(brush, rect.X, rect.Y, rect.Width, rect.Height);
            g.DrawArc(pen, rect, 135, 270);
        }

        private void PaintNeedle(object sender, PaintEventArgs e)
        {
            PictureBox needle = (PictureBox)sender;
            Graphics g = e.Graphics;
            //g.Clear(Color.Transparent);
            needle.BackColor = Color.Transparent;
            var rect = needle.Parent.DisplayRectangle;
            // // Ensure ectangle is square and find the centre for our line
            if (rect.Height > rect.Width)
                rect.Height = rect.Width;
            else
                rect.Width = rect.Height;
            var centrePoint = new Point { X = rect.Width / 2, Y = rect.Height / 2 };
            // MinimumAltitude starts at 110 degrees
            // MaximumAltitude ends at 70 degrees
            // Ensure our Current Altitude with within our bounds
            var angle = 270 * (CurrentAltitude < MinimumAltitude ? MinimumAltitude : (CurrentAltitude > MaximumAltitude ? MaximumAltitude : CurrentAltitude)) / (MaximumAltitude - MinimumAltitude);
            var radius = (double)(rect.Height / 2 - rect.Height / 10);
            var x = centrePoint.X + Math.Sin(angle - 45) * radius;
            var y = centrePoint.Y + Math.Cos(angle - 45) * radius;
            var endPoint = new Point { X = (int)x, Y = (int)y };
            var pen = new Pen(Color.Sienna, 3);

            g.DrawLine(
                pen,
                centrePoint,
                endPoint
                );
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
                if (control == null)
                {
                    // Draw the initial outline once - after that, an overlay will be used to display altitude
                    control = new PictureBox();
                    control.Visible = true;
                    control.BackColor = Color.Transparent;
                    control.Name = "Generic_Altimeter";
                    control.Top = controlTop;
                    control.Left = controlLeft;
                    control.Height = controlHeight;
                    control.Width = controlWidth;
                    control.Paint += this.Paint;
                }
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
        }
    }
}
