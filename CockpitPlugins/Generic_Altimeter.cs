using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace CockpitPlugins
{
    public class Generic_Altimeter : ICockpitInstrument
    {
        public event EventHandler Disposed;

        private bool disposedValue = false;

        private PictureBox control;


        public Generic_Altimeter()
        {
            control = new PictureBox();
            control.BorderStyle = BorderStyle.FixedSingle;
        }


        public void ValueUpdate(ClientRequestResult value)
        {
            // Update our control to display latest value
            throw new NotImplementedException();
        }

        private void control_Update(object sender, PaintEventArgs e)
        {
            // Fetching the property value as accessing the propery directly can cause compiler errors, claiming the Graphics property doesn't exist in System.Drawing
            var g = (System.Drawing.Graphics)e.GetType().GetProperty("Graphics").GetValue(e);
            if (g != null)
            {            
                // Draw a string on the PictureBox.
                g.DrawString("This is a diagonal line drawn on the control", 
                    new Font(FontFamily.GenericSansSerif, 1, FontStyle.Regular, GraphicsUnit.Point, new byte(), false),
                    System.Drawing.Brushes.Blue,
                    new Point(30, 30));
                // Draw a line in the PictureBox.
                g.DrawLine(System.Drawing.Pens.Red, control.Left, control.Top,
                    control.Right, control.Bottom);
            }
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
                var args = Activator.CreateInstance(typeof(System.Windows.Forms.PaintEventArgs), new object[] { 
                    GetControlGraphics(control),
                    control.DisplayRectangle 
                });
                control_Update(control, (System.Windows.Forms.PaintEventArgs)args);
                return control;
            }
        }

        public object GetControlGraphics(Control ctrl)
        {
            return (System.Drawing.Graphics)ctrl.GetType().GetMethod("CreateGraphics").Invoke(ctrl, null);
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
            if (control != null)
            {
                control.Top = top;
                control.Left = left;
                control.Height = height;
                control.Width = width;
            }
        }
    }
}
