using Newtonsoft.Json;
using RemoteCockpitClasses.Animations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstrumentDesigner
{
    public partial class frmInstrumentDesigner : Form
    {
        private Configuration config;
        public frmInstrumentDesigner()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            config = new Configuration();
            openFileDialog.FileName = "";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();

        }

        private void LoadConfig(object sender, EventArgs e)
        {
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var fileName = openFileDialog.FileName;
                try
                {
                    var configJson = File.ReadAllText(fileName);
                    config = JsonConvert.DeserializeObject<Configuration>(configJson);
                    if (config == null || config.Name == null)
                    {
                        throw new Exception("Configuration Load Failed");
                    }
                }
                catch(IOException ex)
                {
                    config = new Configuration();
                    MessageBox.Show(string.Format("Error opening Configuration file:\r\r{0}", fileName), "File Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch(Exception ex)
                {
                    config = new Configuration();
                    MessageBox.Show(string.Format("Error reading Configuration file:\r\r{0}", fileName), "Invalid File Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
