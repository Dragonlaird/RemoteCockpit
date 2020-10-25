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

namespace CockpitDisplay
{
    public partial class frmCockpit : Form
    {
        private delegate void SafeCallDelegate(object obj, string propertyName, object value);
        private delegate void SafeControlAddDelegate(Control ctrl, Control parent);
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

        private void UpdateObject(object obj, string propertyName, object value)
        {
            if (obj != null && !string.IsNullOrEmpty(propertyName) && obj.GetType().GetProperty(propertyName) != null)
            {
                if (((Control)obj).InvokeRequired)
                {
                    var d = new SafeCallDelegate(UpdateObject);
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

        internal void LoadLayout(string text)
        {
            // Here we would clear the current cockpit layout and load all the plugins for the new layout
            foreach(Control control in this.Controls)
            {
                RemoveControl(control, this);
            }
        }
    }
}
