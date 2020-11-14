using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RemoteCockpitClasses;

namespace InstrumentPlugins
{
    public class Inital_Instrument_Template : ICockpitInstrument
    {
        private Control control = null;
        private bool disposedValue = false;
        private int controlTop = 0;
        private int controlLeft = 0;
        private int controlHeight = 50;
        private int controlWidth = 50;
        private int animationTimeInMs = 3000;
        private List<ClientRequestResult> values = new List<ClientRequestResult>();

        /// <summary>
        /// Constructor to initialise any private values, set any defaults 
        /// and generally provide a basic (even empty) control for use on the dashboard
        /// </summary>
        public Inital_Instrument_Template()
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
                        Name = "AIRSPEED INDICATED",
                        Unit = "knots"
                    }
                };
            }
        }

        /// <summary>
        /// A simple array of FS Aircraft names that this instrument can be used with.
        /// </summary>
        public string[] Aircraft
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
                return InstrumentType.Other;
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
        /// Name of the instrument this plugin generates
        /// Typically the type of instrument this control creates and aircraft it can be used on
        /// e.g. Airspeed Indicator for Cessna 152
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the organisation or person who created this instrument plugin
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// How often we expect to receive updates from the server (in seconds)
        /// Useful for producing smooth animations from last to current value.
        /// Can be used to determine how often the animation should be updated
        /// and by how much to update it before the next update is expected
        /// </summary>
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
