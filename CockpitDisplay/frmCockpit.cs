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
using Newtonsoft.Json.Linq;
using System.Timers;

namespace CockpitDisplay
{
    public partial class frmCockpit : Form
    {
        private delegate void SafeCallDelegate(object obj, string propertyName, object value);
        private delegate void SafeControlAddDelegate(Control ctrl, Control parent);
        private delegate void SafeFormUpdateDelegate(Control ctrl);
        private List<ICockpitInstrument> instrumentPlugins;
        private List<ICockpitInstrument> usedInstrumentPlugins;
        private List<LayoutDefinition> layoutDefinitions;
        private LayoutDefinition layoutDefinition;
        public EventHandler<ClientRequest> RequestValue;
        private Point ScreenDimensions;
        private bool redrawingControls = false;
        public frmCockpit()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            layoutDefinitions = new List<LayoutDefinition>();
            instrumentPlugins = new List<ICockpitInstrument>();
            usedInstrumentPlugins = new List<ICockpitInstrument>();
            var allAssemblies = LoadAvailableAssemblies();
            instrumentPlugins = GetPlugIns(allAssemblies);
        }

        private void AddControl(Control ctrl, Control parent)
        {
            if (parent.InvokeRequired)
            {
                var d = new SafeControlAddDelegate(AddControl);
                parent.Invoke(d, new object[] { ctrl, parent });
                return;
            }
            ctrl.BackColor = Color.Transparent;
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
                    UpdateCockpitItem((Control)obj);
                }
            }
        }

        public void ResultUpdate(ClientRequestResult requestResult)
        {
            if (usedInstrumentPlugins != null && !redrawingControls)
                redrawingControls = true;
                lock (usedInstrumentPlugins)
                    foreach (var instrument in usedInstrumentPlugins)
                    {
                        if (instrument.RequiredValues.Any(x => x.Name == requestResult.Request.Name && x.Unit == requestResult.Request.Unit))
                            try
                            {
                                instrument.ValueUpdate(requestResult);
                                UpdateCockpitItem(instrument.Control);
                            }
                            catch (Exception ex)
                            {
                            }
                    }
            UpdateCockpitItem(this);
        }

        private void UpdateCockpitItem(Control obj)
        {
            if (obj.InvokeRequired)
            {
                var d = new SafeFormUpdateDelegate(UpdateCockpitItem);
                this.Invoke(d, new object[] { obj });
                return;
            }
            try
            {
                obj.Update();
            }
            catch { }
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
            double aspectRatio = 0;
            // Load one of every Instrument plugin type for this layout
            if (!string.IsNullOrEmpty(layoutDefinition.Background))
            {
                try
                {
                    var imageFile = File.OpenRead(string.Format(@".\Layouts\Dashboards\{0}", layoutDefinition.Background));
                    var image = Image.FromStream(imageFile);
                    var imageScaleFactor = (double)this.Width / image.Width;
                    aspectRatio = (double)image.Height / image.Width;
                    if (image.Height * imageScaleFactor > this.Height)
                        imageScaleFactor = (double)this.Height / image.Height;
                    var backgroundImage = new Bitmap(image, new Size((int)(image.Width * imageScaleFactor), (int)(image.Height * imageScaleFactor)));
                    ScreenDimensions = new Point(backgroundImage.Width, backgroundImage.Height);
                    var g = this.CreateGraphics();
                    g.DrawImageUnscaled(backgroundImage, 0, 0, this.Width, this.Height);
                    //this.BackgroundImage = backgroundImage;
                }
                catch(Exception ex)
                {

                }
            }
            var layoutInstruments = new List<ICockpitInstrument>();
            layoutInstruments.AddRange(instrumentPlugins
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
            layoutInstruments.AddRange(instrumentPlugins.Where(x => x.Layouts.Contains("") && layoutDefinition.InstrumentTypes.Contains(x.Type) && !layoutInstruments.Any(y => y.Type == x.Type)).GroupBy(
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
            // Variable layoutInstruments contains all the plugins we can use for this layout
            // Now we simply add them to the relevant location on the form, suitably resized based on the current fom size
            var variables = new List<ClientRequest>();
            while (redrawingControls)
            {
                Thread.Sleep(10);
            }
            redrawingControls = true;
            foreach (var instrumentPosition in layoutDefinition.Postions)
            {
                var plugin = instrumentPlugins.FirstOrDefault(x => x.Type == instrumentPosition.Type);
                if(plugin != null)
                {
                    variables.AddRange(plugin.RequiredValues.Distinct().Where(x => !variables.Any(y => y.Name == x.Name && y.Unit == x.Unit)));
                    var vScaleFactor = (double)ScreenDimensions.Y / 100;
                    var hScaleFactor = (double)ScreenDimensions.X / 100;
                    plugin.SetLayout(
                        (int)(instrumentPosition.Top * vScaleFactor),
                        (int)(instrumentPosition.Left * hScaleFactor),
                        (int)(instrumentPosition.Height * vScaleFactor),
                        (int)(instrumentPosition.Width * hScaleFactor));
                    AddControl(plugin.Control, this);
                    usedInstrumentPlugins.Add(plugin);
                    UpdateCockpitItem(plugin.Control);
                }
            }
            redrawingControls = false;
            // Request all variables used by any plugin - even if they've been requested before - duplicate requests are ignored
            if (RequestValue != null)
            {
                foreach(var variable in variables)
                {
                    try
                    {
                        RequestValue.DynamicInvoke(this, variable);
                    }
                    catch(Exception ex) { }
                }
            }

        }

        private LayoutDefinition GetLayout(string name)
        {
            if (layoutDefinitions.Count == 0)
            {
                var layoutsDefinitionsText = File.ReadAllText(@".\Layouts\Layouts.json");
                var layouts = (JObject)JsonConvert.DeserializeObject(layoutsDefinitionsText);
                foreach (var layoutJson in layouts["Layouts"])
                {
                    var layout = layoutJson.ToObject<LayoutDefinition>();
                    if (!layoutDefinitions.Contains(layout))
                        layoutDefinitions.Add(layout);
                }
            }
            LayoutDefinition result = layoutDefinitions.SingleOrDefault(x => x.Name == name);
            if (result == null)
                result = layoutDefinitions.First();
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
