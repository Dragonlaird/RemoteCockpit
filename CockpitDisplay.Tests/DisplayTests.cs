using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CockpitDisplay;

namespace CockpitDisplay.Tests
{
    [TestClass]
    public class DisplayTests
    {
        CockpitDisplay.frmCockpit frm;
        [TestMethod]
        public void CreateCockpitInstance()
        {
            frm = new frmCockpit();
            frm.LoadLayout("Cessna 152 ASOBO");
            frm.Dispose();
        }

        [TestMethod]
        public void DisplayCockpitInstance()
        {
            frm = new frmCockpit();
            frm.LoadLayout("Cessna 152 ASOBO");
            frm.Show();
            frm.Close();
            frm.Dispose();
        }
    }
}
