using InstrumentPlugins;
using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AvionicsInstrumentControl;

namespace AvionicsInstrumentsControl.Wrappers
{
    public class FS_Altimeter : ICockpitInstrument
    {
        private readonly AirSpeedIndicatorInstrumentControl instrument = new AirSpeedIndicatorInstrumentControl();
        public IEnumerable<ClientRequest> RequiredValues => new ClientRequest[] { new ClientRequest { Name = "AIRSPEED INDICATED", Unit = "knots" } };

        public string Name => "Guillaume CHOUTEAU - AirSpeedIndicatorInstrumentControl";

        public string Author => "Guillaume CHOUTEAU";

        public string[] Aircraft => new string[] { "Cessna 152 Asobo" };

        public DateTime PluginDate => DateTime.Parse("13-Feb-2021 10:00:00");

        public int UpdateFrequency { get; set; } = 3;

        public InstrumentType Type => InstrumentType.Airspeed_Indicator;

        public Control Control =>  instrument;

        public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<string> LogMessage;
        public event EventHandler Disposed;

        public void Dispose()
        {
            instrument.Dispose();
            if(Disposed != null)
                try
                {
                    Disposed.DynamicInvoke();
                }
                catch { }
        }

        public void SetLayout(int top, int left, int height, int width)
        {
            instrument.Height = height;
            instrument.Width = width;
            instrument.Top = top;
            instrument.Left = left;
        }

        public void ValueUpdate(ClientRequestResult value)
        {
            try
            {
                if (((AirSpeedIndicatorInstrumentControl)Control).InvokeRequired)
                {
                    ((AirSpeedIndicatorInstrumentControl)Control).Invoke(new Action(() =>
                    {
                        ((AirSpeedIndicatorInstrumentControl)Control).SetAirSpeedIndicatorParameters((int)(double)value.Result);
                    }));
                    return;
                }
                ((AirSpeedIndicatorInstrumentControl)Control).SetAirSpeedIndicatorParameters((int)(double)value.Result);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
