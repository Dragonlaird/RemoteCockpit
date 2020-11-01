using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RemoteCockpitClasses;

namespace InstrumentPlugins
{
    public class Generic_Attitude_Indicator : ICockpitInstrument
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
        private double CurrentBank = 0;
        private double LastBank = -1;
        private double CurrentPitch = 0;
        private double LastPitch = -1;
        private double needleMoveSpeed = 0;
        System.Timers.Timer animateTimer;

        /// <summary>
        /// Constructor to initialise any private values, set any defaults 
        /// and generally provide a basic (even empty) control for use on the dashboard
        /// </summary>
        public Generic_Attitude_Indicator()
        {
            RedrawControl();
        }

        private void RedrawControl()
        {
            if (control == null)
            {
                control = new PictureBox();
                control.Name = "Generic_Attitude_Indicator";
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
            var img = ImageLibrary.Attitude_Background;
            var aspectRatio = (double)img.Height / img.Width;
            // Need to rescale control to match the aspect ratio of the image
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

        }
        /// <summary>
        /// An array or list of SimVar (FS SDK Variables) this instrument needs.
        /// These values will be requsted and returned every time FS provides a new value for each.
        /// </summary>
        public IEnumerable<ClientRequest> RequiredValues
        {
            get
            {
                return new ClientRequest[]
                {
                    new ClientRequest
                    {
                        Name = "ATTITUDE INDICATOR BANK DEGREES",
                        Unit = "radians"
                    },
                    new ClientRequest
                    {
                        Name = "ATTITUDE INDICATOR PITCH DEGREES",
                        Unit = "radians"
                    }

                };
            }
        }

        /// <summary>
        /// A simple array of FS Aircraft names that this instrument can be used with.
        /// </summary>
        public string[] Layouts
        {
            get
            {
                return new string[] { "" }; // Blank can be used on all layouts
            }
        }

        /// <summary>
        /// The creation date of this plugin (instrument).
        /// This is used to determine the most recent (and therefore most accurate) instrument for each layout.
        /// This instrument may be used for multiple aircraft but others may have produced a newer instrument for a specific aircraft.
        /// </summary>
        public DateTime PluginDate
        {
            get
            {
                return DateTime.MinValue; // Should always use Min Date for a Generic Plugin so it can be overridden by newer plugins
            }
        }

        /// <summary>
        /// The Type of instrument, such as Altimeter, Airspeed Indicator and so on
        /// </summary>
        public InstrumentType Type
        {
            get
            {
                return InstrumentType.Attitude_Indicator;
            }
        }

        /// <summary>
        /// This is the Windows Form Control that will be added to the coskpit dashboard.
        /// It should be resized to fit within the limits that will be supplied via SetLayout.
        /// </summary>
        public Control Control
        {
            get
            {
                return control;
            }
        }

        /// <summary>
        /// Inherited from IComponent - not really used
        /// </summary>
        public ISite Site { get; set; }

        /// <summary>
        /// Notify the cockpit form if the instrument is disposed.
        /// Not currently used by the cockpit form but retained for future use.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Dispose of Cockpit Instrument.
        /// Removes any objects added during the use of this instance before finally disposing itself.
        /// </summary>
        /// <param name="disposing">Indicates if this Cockpit Instrument is already being disposed.</param>
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

        /// <summary>
        /// Dispose of this Cocpkit Instrument instance.
        /// </summary>
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

        /// <summary>
        /// Set the maximum bounds the Control can fill.
        /// Control must be resized to fit within the supplied limits.
        /// Control should maintain it's own aspect ratio, the aspect ratio of these limits
        /// will be based on the size and position of the dashboard.
        /// Recommend using a Timer to animate the control changes
        /// </summary>
        /// <param name="top">Exact Top position of the Control</param>
        /// <param name="left">Exact Left position of the Control</param>
        /// <param name="height">Maximum height of the Control</param>
        /// <param name="width">Maximum width of the Control</param>
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

        /// <summary>
        /// Whenever a value associated with one of our RequiredValues is updated by FS,
        /// it will be passed to this control via this method.
        /// This method should provide a low overhead, such as simply adding or updating a local variable with the new value.
        /// It should NOT be used to redraw the control itself.
        /// </summary>
        /// <param name="value">Required Variable Value</param>
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
            }
            catch
            {
                // Likely something is being disposed or updated - we'll need to add/update the value next time
            }
        }
    }
}
