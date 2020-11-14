using InstrumentPlugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace CockpitDisplay
{
    public partial class frmCockpit : Form
    {
        private delegate void SafeCallDelegate(object obj, string propertyName, object value);
        private delegate void SafeControlAddDelegate(Control ctrl);
        private delegate void SafeFormUpdateDelegate(Control ctrl);
        private List<ICockpitInstrument> instrumentPlugins;
        private List<ICockpitInstrument> usedInstrumentPlugins;
        private List<LayoutDefinition> layoutDefinitions;
        private LayoutDefinition layoutDefinition;
        public EventHandler<ClientRequest> RequestValue;
        private Point ScreenDimensions;
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

        private void AddControl(Control ctrl)
        {
            if (this.InvokeRequired)
            {
                var d = new SafeControlAddDelegate(AddControl);
                this.Invoke(d, new object[] { ctrl });
                return;
            }
            ctrl.BackColor = Color.Transparent;
            this.Controls.Add(ctrl);
            ctrl.BringToFront();
        }

        private void RemoveControl(Control ctrl)
        {
            if (this.InvokeRequired)
            {
                var d = new SafeControlAddDelegate(RemoveControl);
                this.Invoke(d, new object[] { ctrl });
                return;
            }
            this.Controls.Remove(ctrl);
            ctrl.Dispose();
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
            try
            {
                if (usedInstrumentPlugins != null)
                {
                    try
                    {
                        foreach (var instrument in usedInstrumentPlugins)
                        {
                            if (requestResult.Request.Name == "UPDATE FREQUENCY" && requestResult.Request.Unit == "second")
                            {
                                try
                                {
                                    instrument.UpdateFrequency = int.Parse(requestResult.Result.ToString());
                                }
                                catch { }
                            }
                            else
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
                    }
                    catch (Exception ex)
                    {

                    }
                    UpdateCockpitItem(this);
                }
            }
            catch { }
        }

        private void UpdateCockpitItem(Control obj)
        {
            if (obj.InvokeRequired)
            {
                try
                {
                    var d = new SafeFormUpdateDelegate(UpdateCockpitItem);
                    this.Invoke(d, new object[] { obj });
                }
                catch { }
                return;
            }
            try
            {
                obj.Update();
            }
            catch { }
        }

        public void LoadLayout(string text)
        {
            // Here we clear the current cockpit layout and load all the plugins for the new layout
            foreach (Control control in this.Controls)
            {
                RemoveControl(control);
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
                    var backgroundImage = new Bitmap(this.Width, this.Height);
                    using (Graphics gr = Graphics.FromImage(backgroundImage))
                    {
                        gr.DrawImage(new Bitmap(image, new Size((int)(image.Width * imageScaleFactor), (int)(image.Height * imageScaleFactor))), new Point(0, 0));
                        ScreenDimensions.X = (int)(image.Width * imageScaleFactor);
                        ScreenDimensions.Y = (int)(image.Height * imageScaleFactor);
                    }
                    this.BackgroundImage = backgroundImage;
                }
                catch (Exception ex)
                {

                }
            }
            // Variable layoutInstruments contains all the plugins we can use for this layout
            // Now we simply add them to the relevant location on the form, suitably resized based on the current form size
            var variables = new List<ClientRequest>();
            try
            {
                foreach (var instrumentPosition in layoutDefinition.Postions)
                {
                    var plugin = instrumentPlugins.OrderByDescending(x=> x.PluginDate).FirstOrDefault(x => x.Type == instrumentPosition.Type);
                    try
                    {
                        if (plugin != null)
                        {
                            variables.AddRange(plugin.RequiredValues.Distinct().Where(x => !variables.Any(y => y.Name == x.Name && y.Unit == x.Unit)));
                            var vScaleFactor = (double)ScreenDimensions.Y / 100;
                            var hScaleFactor = (double)ScreenDimensions.X / 100;
                            plugin.SetLayout(
                                (int)(instrumentPosition.Top * vScaleFactor),
                                (int)(instrumentPosition.Left * hScaleFactor),
                                (int)(instrumentPosition.Height * vScaleFactor),
                                (int)(instrumentPosition.Width * hScaleFactor));
                            AddControl(plugin.Control);
                            usedInstrumentPlugins.Add(plugin);
                            UpdateCockpitItem(plugin.Control);
                            plugin.Control.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Unable to add plugin: {0}\rError: {1}", plugin.Name, ex.Message));
                    }
                }
                if (!variables.Any(x => x.Name == "UPDATE FREQUENCY" && x.Unit == "second"))
                {
                    variables.Add(new ClientRequest { Name = "UPDATE FREQUENCY", Unit = "second" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to add Instruments.\rError: {0}", ex.Message);
            }
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
            if (!dInfo.Exists)
            {
                MessageBox.Show("Plugins folder doesn't exist", "Folder Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Assembly>();
            }
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
            var customInstruments = instrumentsList.ConvertAll<ICockpitInstrument>(delegate (Type t) { return Activator.CreateInstance(t) as ICockpitInstrument; });
            foreach (var instrumentDefinition in Directory.GetFiles(".\\GenericInstruments"))
            {
                try
                {
                    var genericInstrument = new Generic_Instrument(instrumentDefinition);
                    customInstruments.Add(genericInstrument);
                }
                catch (Exception ex)
                {

                }
            }
            // convert the list of Objects to an instantiated list of ICalculators
            return customInstruments;
        }
    }
}
