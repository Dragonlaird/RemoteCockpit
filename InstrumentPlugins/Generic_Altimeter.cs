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
            Update(control, new PaintEventArgs(control.CreateGraphics(), control.DisplayRectangle));
        }

        private void Update(object sender, PaintEventArgs e)
        {
            if (sender is PictureBox && ((Control)sender).Name == "Generic_Altimeter")
            {
                if(((PictureBox)sender).Controls.Count != 0)
                {
                    ((PictureBox)sender).Controls.Clear();
                }
                ((PictureBox)sender).Controls.Add(CreateNeedle((PictureBox)sender));
            }
        }

        private PictureBox CreateNeedle(Control control)
        {
            var needle = new PictureBox();
            needle.BackColor = Color.Transparent;
            var rect = control.DisplayRectangle;
            // // Ensure ectangle is square and find the centre for our line
            if (rect.Height > rect.Width)
                rect.Height = rect.Width;
            else
                rect.Width = rect.Height;
            var centrePoint = new Point { X = rect.Width / 2, Y = rect.Height / 2 };
            // MinimumAltitude starts at 110 degrees
            // MaximumAltitude ends at 70 degrees
            // Ensure our Current Altitude with within our bounds
            var endPointAngle = (float)(110 + 320.0 * ((float)(CurrentAltitude < MinimumAltitude ? 
                MinimumAltitude : 
                CurrentAltitude > MaximumAltitude ? 
                    MaximumAltitude : 
                    CurrentAltitude) / (float)MaximumAltitude));
            var radius = (float)(rect.Height - rect.Height / 10) / 2;
            var endPoint = new Point { X = (int)(Math.Cos(endPointAngle)*radius), Y = (int)(Math.Sin(endPointAngle)*radius) };
            var pen = new Pen(Color.Black, 4);

            var g = needle.CreateGraphics();
            g.DrawLine(
                pen,
                centrePoint,
                endPoint
                );
            return needle;
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
                    control.BackColor = Color.Gainsboro;
                    control.Name = "Generic_Altimeter";
                    control.Top = controlTop;
                    control.Left = controlLeft;
                    control.Height = controlHeight;
                    control.Width = controlWidth;

                    var g = control.CreateGraphics();
                    g.Clear(Color.Transparent);
                    Pen pen = new Pen(Color.Red, 3);
                    Rectangle rect = new Rectangle(
                        control.DisplayRectangle.X,
                        control.DisplayRectangle.Y,
                        control.DisplayRectangle.Width > control.DisplayRectangle.Height ? control.DisplayRectangle.Height : control.DisplayRectangle.Width,
                        control.DisplayRectangle.Height > control.DisplayRectangle.Width ? control.DisplayRectangle.Width : control.DisplayRectangle.Height);
                    rect.Width -= (1 + (int)pen.Width);
                    rect.Height -= (1 + (int)pen.Width);
                    g.DrawArc(pen, rect, 0, 360);
                    Update(control, new PaintEventArgs(g, rect));
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
