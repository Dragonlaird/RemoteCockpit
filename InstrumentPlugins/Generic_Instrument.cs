using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using RemoteCockpitClasses;
using Newtonsoft;
using Newtonsoft.Json;
using RemoteCockpitClasses.Generic_Instrument;
using System.IO;
using System.Drawing;

namespace InstrumentPlugins
{
    public class Generic_Instrument : ICockpitInstrument
    {
        private Configuration config;
        private System.Timers.Timer animateTimer;
        private double aspectRatio;
        private int controlTop = 0;
        private int controlLeft = 0;
        private int controlHeight = 50;
        private int controlWidth = 50;
        private List<ClientRequestResult> lastResults = new List<ClientRequestResult>();

        public Generic_Instrument()
        {
            config = new Configuration();
        }

        public Generic_Instrument(string filePath)
        {
            var result = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(filePath));
            Initialize();
        }

        private void Initialize()
        {
            var imageFile = File.OpenRead(config.BackgroundImagePath);
            var image = Image.FromStream(imageFile);
            aspectRatio = (double)image.Height / image.Width;
            var imageScaleFactor = controlHeight < controlWidth ? image.Width / controlWidth : controlHeight / image.Height;
            var backgroundImage = new Bitmap(image, new Size((int)(image.Width * imageScaleFactor), (int)(image.Height * imageScaleFactor)));
            Control = new Panel();
            Control.Height = image.Height;
            Control.Width = image.Width;
            Control.Top = controlTop;
            Control.Left = controlLeft;
            Control.BackgroundImage = backgroundImage;
            lastResults = new List<ClientRequestResult>();
            foreach(var clientRequest in config.Animations.Select(x => x.Request))
            {
                lastResults.Add(new ClientRequestResult { Request = clientRequest, Result = null });
            }
        }

        private bool disposedValue;

        public IEnumerable<ClientRequest> RequiredValues { get; private set; }

        public string[] Layouts { get; private set; }

        public DateTime PluginDate { get; private set; }

        public InstrumentType Type { get; private set; }

        public Control Control { get; private set; }

        public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler Disposed;

        public void SetLayout(int top, int left, int height, int width)
        {
            controlTop = top;
            controlLeft = left;
            controlHeight = height;
            controlWidth = width;
            Initialize();
        }

        public void ValueUpdate(ClientRequestResult value)
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    config = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Generic_Instrument()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
