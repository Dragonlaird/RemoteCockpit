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
using System.Threading.Tasks;
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
        public EventHandler<string> LogMessage;
        private Point ScreenDimensions;
        public frmCockpit()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            this.Show();
            this.Update();
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
                            Task.Run(() =>
                            {
                                if (requestResult.Request.Name == "UPDATE FREQUENCY" && requestResult.Request.Unit == "millisecond")
                                {
                                    try
                                    {
                                        instrument.UpdateFrequency = int.Parse(requestResult.Result?.ToString() ?? "3000");
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
                                        ConsoleLog(string.Format("Instrument Update Failed.\rInstrument: {0}\rSimVar: {1}\rError: {2}", instrument.Name, requestResult.Request.Name, ex.Message));
                                    }
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog(string.Format("ResultUpdate Error: {0}", ex.Message));
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
                //obj.Update();
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
                    this.FindForm().BackColor = Color.Black;
                    var imageFile = File.OpenRead(string.Format(@".\Layouts\Dashboards\{0}", layoutDefinition.Background));
                    var image = Image.FromStream(imageFile);
                    var imageScaleFactor = (double)this.DisplayRectangle.Width / (double)image.Width;
                    aspectRatio = (double)image.Height / image.Width;
                    if (image.Height * imageScaleFactor > this.Height)
                        imageScaleFactor = (double)this.Height / image.Height;
                    var backgroundImage = new Bitmap(image, new Size((int)(image.Width * imageScaleFactor), (int)(image.Height * imageScaleFactor)));
                    ScreenDimensions.X = backgroundImage.Width;
                    ScreenDimensions.Y = backgroundImage.Height;
                    this.BackgroundImage = backgroundImage;
                    this.BackgroundImageLayout = ImageLayout.None;
                }
                catch (Exception ex)
                {
                    ConsoleLog(string.Format("LoadLayout (Backgound Image) Error: {0}", ex.Message));
                }
            }
            // Variable layoutInstruments contains all the plugins we can use for this layout
            // Now we simply add them to the relevant location on the form, suitably resized based on the current form size
            var variables = new List<ClientRequest>();
            try
            {
                foreach (var instrumentPosition in layoutDefinition.Postions)
                {
                    var plugin = instrumentPlugins.Where(x => x.Aircraft != null && (x.Aircraft.Contains(layoutDefinition.Name) || x.Aircraft.Contains("Generic"))).OrderByDescending(x => x.PluginDate).FirstOrDefault(x => x.Type == instrumentPosition.Type);
                    try
                    {
                        if (plugin != null)
                        {
                            string message = string.Format("Adding Instrument: {0} (Type: {1})", plugin.Name, plugin.Type.ToString());
                            ConsoleLog(message);
                            plugin.LogMessage += ConsoleLog;
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
                            //plugin.Control.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog(string.Format("LoadLayout (Add Plugin) Error: {0}\rError: {1}", plugin.Name, ex.Message));
                    }
                }
                if (!variables.Any(x => x.Name == "UPDATE FREQUENCY" && x.Unit == "second"))
                {
                    variables.Add(new ClientRequest { Name = "UPDATE FREQUENCY", Unit = "second" });
                }
            }
            catch (Exception ex)
            {
                ConsoleLog(string.Format("LoadLayout (Add Instruments) Error: {0}", ex.Message));
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
                    catch (Exception ex)
                    {
                        ConsoleLog(string.Format("LoadLayout (Submit Initial Variables) Error: {0}", ex.Message));
                    }
                }
            }
            //this.Invalidate(true);
            //this.Update();
        }

        private LayoutDefinition GetLayout(string name)
        {
            if (layoutDefinitions.Count == 0)
            {
                string message = string.Format("Loading Layout: {0}", name);
                ConsoleLog(message);
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
            {
                result = layoutDefinitions.First();
                string message = string.Format("Layout Not Defined: {0}\rUsing First Layout: {1}", name, result.Name);
                ConsoleLog(message);
            }
            return result;
        }

        // Modified from code: https://www.c-sharpcorner.com/article/introduction-to-building-a-plug-in-architecture-using-C-Sharp/
        private List<Assembly> LoadAvailableAssemblies()
        {
            string message = string.Format("Loading Plugins");
            ConsoleLog(message);

            DirectoryInfo dInfo = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, @"Plugins"));
            if (!dInfo.Exists)
            {
                ConsoleLog(string.Format("LoadAvailableAssemblies (Plugins Folder) Error: Folder missing - {0}", Path.Combine(Environment.CurrentDirectory, @"Plugins")));
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
                        message = string.Format("Loading Assembly: {0}", file.Name);
                        ConsoleLog(message);
                        plugInAssemblyList.Add(Assembly.LoadFile(file.FullName)); // May fail if not a .NET assembly
                    }
                    catch(Exception ex) {
                        ConsoleLog(string.Format("LoadAvailableAssemblies (Add Custom Plugins) Error: {0}", ex.Message));
                    }
                }
            }

            return plugInAssemblyList;
        }

        private List<ICockpitInstrument> GetPlugIns(List<Assembly> assemblies)
        {
            List<Type> availableTypes = new List<Type>();
            string message = string.Format("Fetching Custom plugins");
            ConsoleLog(message);
            foreach (Assembly currentAssembly in assemblies)
                try
                {
                    availableTypes.AddRange(currentAssembly.GetTypes());
                }
                catch(Exception ex) {
                    ConsoleLog(string.Format("GetPlugIns (Assemblies) Error: {0}", ex.Message));
                }

            List<Type> instrumentsList = availableTypes.FindAll(delegate (Type t)
            {
                List<Type> interfaceTypes = new List<Type>(t.GetInterfaces());
                return interfaceTypes.Contains(typeof(ICockpitInstrument));
            });
            var customInstruments = instrumentsList.ConvertAll<ICockpitInstrument>(delegate (Type t) { 
                try { 
                    return Activator.CreateInstance(t) as ICockpitInstrument; 
                } 
                catch(Exception ex) { 
                    ConsoleLog(string.Format("Unable to build Cockpit Instrument: {0}\rError: {1}", t.Name, ex.Message)); 
                    return null; 
                } 
            }).Where(x => x != null).ToList();
            message = string.Format("Fetching Generic plugins");
            ConsoleLog(message);
            foreach (var instrumentDefinition in Directory.GetFiles(".\\GenericInstruments"))
            {
                if (instrumentDefinition?.ToLower().EndsWith(".json") == true)
                    try
                    {
                        var genericInstrument = new Generic_Instrument(100, 100, instrumentDefinition);
                        customInstruments.Add(genericInstrument);
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog(string.Format("GetPlugIns (Generic Instrument).\rInstrument: {0}.\rError: {0}", instrumentDefinition, ex.Message));
                    }
            }
            // convert the list of Objects to an instantiated list of ICalculators
            return customInstruments;
        }

        private void ConsoleLog(object sender, string e)
        {
            ConsoleLog(e);
        }

        private void ConsoleLog(string message)
        {
            if (LogMessage != null)
                try
                {
                    LogMessage.DynamicInvoke(this, message);
                }
                catch//(Exception ex)
                {

                }
        }
    }
}
