using AvionicsInstrumentControlDemo;
using InstrumentPlugins;
using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AvionicsInstrumentsControls.Wrappers
{
    public class FS_Altimeter : ICockpitInstrument
    {
        public IEnumerable<ClientRequest> RequiredValues => throw new NotImplementedException();

        public string Name => "Guillaume CHOUTEAU - AirSpeedIndicatorInstrumentControl";

        public string Author => "Guillaume CHOUTEAU";

        public string[] Aircraft => new string[] { "Generic" };

        public DateTime PluginDate => DateTime.Parse("13-Feb-2021 10:00:00");

        public int UpdateFrequency { get; set; } = 3;

        public InstrumentType Type => InstrumentType.Airspeed_Indicator;

        public Control Control =>  new AirSpeedIndicatorInstrumentControl();

        public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<string> LogMessage;
        public event EventHandler Disposed;

        public void Dispose()
        {
            Control.Dispose();
            if(Disposed!= null)
            {
                try
                {
                    Disposed.DynamicInvoke();
                }
                catch { }
            }
        }

        public void SetLayout(int top, int left, int height, int width)
        {
            Control.Height = height;
            Control.Width = width;
            Control.Top = top;
            Control.Left = left;
        }

        public void ValueUpdate(ClientRequestResult value)
        {
            try
            {
                ((AirSpeedIndicatorInstrumentControl)Control).SetAirSpeedIndicatorParameters((int)value.Result);
            }
            catch { }
        }
    }
}
