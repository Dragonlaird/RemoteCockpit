using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CockpitDisplay
{
    public partial class frmCockpit : Form
    {
        public frmCockpit()
        {
            InitializeComponent();
            // Only variable needed for this is "TITLE", to be notified whenever the aircraft type changes
        }
    }
}
