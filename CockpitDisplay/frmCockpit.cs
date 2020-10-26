using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using RemoteCockpitClasses;
using Newtonsoft.Json;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;

namespace CockpitDisplay
{
    public partial class frmCockpit : Form
    {
        private delegate void SafeCallDelegate(object obj, string propertyName, object value);
        private delegate void SafeControlAddDelegate(Control ctrl, Control parent);
        private List<ICockpitInstrument> instruments;
        private LayoutDefinition layoutDefinition;
        public frmCockpit()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
        }

        private void AddControl(Control ctrl, Control parent)
        {
            if (parent.InvokeRequired)
            {
                var d = new SafeControlAddDelegate(AddControl);
                parent.Invoke(d, new object[] { ctrl, parent });
                return;
            }
            parent.Controls.Add(ctrl);
        }

        private void RemoveControl(Control ctrl, Control parent)
        {
            if (parent.InvokeRequired)
            {
                var d = new SafeControlAddDelegate(RemoveControl);
                parent.Invoke(d, new object[] { ctrl, parent });
                return;
            }
            parent.Controls.Remove(ctrl);
        }

        private void UpdateProperty(object obj, string propertyName, object value)
        {
            if (obj != null && !string.IsNullOrEmpty(propertyName) && obj.GetType().GetProperty(propertyName) != null)
            {
                if (((Control)obj).InvokeRequired)
                {
                    var d = new SafeCallDelegate(UpdateProperty);
                    ((Control)obj).Invoke(d, new object[] { obj, propertyName, value });
                    return;
                }
                if (obj.GetType().GetProperty(propertyName) != null)
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, value);
                    ((Control)obj).Update();
                }
            }
        }

        public void ResultUpdate(ClientRequestResult requestResult)
        {
            lock (instruments)
                foreach (var instrument in instruments)
                {
                    if (instrument.RequiredValues.Any(x => x.Name == requestResult.Request.Name && x.Unit == requestResult.Request.Unit))
                        try
                        {
                            instrument.ValueUpdate(requestResult);
                        }
                        catch { }
                }
        }

        internal void LoadLayout(string text)
        {
            // Here we would clear the current cockpit layout and load all the plugins for the new layout
            foreach (Control control in this.Controls)
            {
                RemoveControl(control, this);
            }
            // Find which Instruments are used in this Cockpit
            layoutDefinition = GetLayout(text);
            // Load one of every Instrument plugin type for this layout
            var allAssemblies = LoadAvailableAssemblies();
            var instrumentPlugins = GetPlugIns(allAssemblies);
            instruments = new List<ICockpitInstrument>();
            instruments.AddRange(instrumentPlugins
                .Where(x => !string.IsNullOrEmpty(text) && x.Layouts.Contains(layoutDefinition.Name) && layoutDefinition.InstrumentTypes.Contains(x.Type)).GroupBy(
                x => x.Type,
                x => x.PluginDate,
                    (baseType, ages) => new
                    {
                        Key = baseType,
                        Count = ages.Count(),
                        Min = ages.Min(),
                        Max = ages.Max()
                    })
                .Select(x => instrumentPlugins.FirstOrDefault(y => y.Type == x.Key && y.PluginDate == x.Max)));
            instruments.AddRange(instrumentPlugins.Where(x => x.Layouts.Contains("") && layoutDefinition.InstrumentTypes.Contains(x.Type) && !instruments.Any(y => y.Type == x.Type)).GroupBy(
                x => x.Type,
                x => x.PluginDate,
                    (baseType, ages) => new
                    {
                        Key = baseType,
                        Count = ages.Count(),
                        Min = ages.Min(),
                        Max = ages.Max()
                    })
                .Select(x => instrumentPlugins.FirstOrDefault(y => y.Type == x.Key && y.PluginDate == x.Max)));
            // Variable instruments contains all the plugins we can use for this layout
            // Now we simply add them to the relevant location on the form, suitably resized based on the current fom size
            foreach(var instrumentPosition in layoutDefinition.Postions)
            {
                var plugin = instrumentPlugins.FirstOrDefault(x => x.Type == instrumentPosition.Type);
                if(plugin != null)
                {
                    var vScaleFactor = this.Height / 100;
                    var hScaleFactor = this.Width / 100;
                    plugin.SetLayout(
                        instrumentPosition.Top * vScaleFactor, 
                        instrumentPosition.Left * hScaleFactor, 
                        instrumentPosition.Height * vScaleFactor,
                        instrumentPosition.Width * hScaleFactor);
                    AddControl(plugin.Control, this);
                }
            }
        }

        private LayoutDefinition GetLayout(string name)
        {
            LayoutDefinition result = new LayoutDefinition
            {
                Name = string.IsNullOrEmpty(name) ? "Generic Layout" : name,
                Postions = new InstrumentLocation[] {
                    new InstrumentLocation {
                        Type = InstrumentType.Altimeter,
                        Top = 5,
                        Left = 5,
                        Height = 15,
                        Width = 15
                    },
                    new InstrumentLocation {
                        Type = InstrumentType.GPS,
                        Top = 5,
                        Left = 20,
                        Height = 20,
                        Width = 20
                    },
                }
            };
            return result;
        }

        // Modified from code: https://www.c-sharpcorner.com/article/introduction-to-building-a-plug-in-architecture-using-C-Sharp/
        private static List<Assembly> LoadAvailableAssemblies()
        {
            DirectoryInfo dInfo = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, @"Plugins"));
            FileInfo[] files = dInfo.GetFiles("*.dll");
            List<Assembly> plugInAssemblyList = new List<Assembly>();

            if (null != files)
            {
                foreach (FileInfo file in files)
                {
                    try
                    {
                        plugInAssemblyList.Add(Assembly.LoadFile(file.FullName)); // May fail if not a .NET assembly
                    }
                    catch(Exception ex) { }
                }
            }

            return plugInAssemblyList;
        }

        static List<ICockpitInstrument> GetPlugIns(List<Assembly> assemblies)
        {
            List<Type> availableTypes = new List<Type>();

            foreach (Assembly currentAssembly in assemblies)
                try
                {
                    availableTypes.AddRange(currentAssembly.GetTypes());
                }
                catch { }

            // get a list of objects that implement the ICalculator interface AND 
            // have the CalculationPlugInAttribute
            List<Type> instrumentsList = availableTypes.FindAll(delegate (Type t)
            {
                List<Type> interfaceTypes = new List<Type>(t.GetInterfaces());
                return interfaceTypes.Contains(typeof(ICockpitInstrument));
            });

            // convert the list of Objects to an instantiated list of ICalculators
            return instrumentsList.ConvertAll<ICockpitInstrument>(delegate (Type t) { return Activator.CreateInstance(t) as ICockpitInstrument; });

        }
    }
}
