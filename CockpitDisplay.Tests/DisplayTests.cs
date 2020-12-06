using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CockpitDisplay;
using System.Threading;
using RemoteCockpitClasses;

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
            Thread.Sleep(1000);
            var result = GetRequest("INDCATED AIRSPEED", "knots");
            frm.ResultUpdate(result);
            Thread.Sleep(3000);
            result.Result = 100;
            frm.ResultUpdate(result);
            Thread.Sleep(3000);
            result.Result = 200;
            Thread.Sleep(3000);
            frm.Close();
            frm.Dispose();
        }

        private ClientRequestResult GetRequest(string name, string unit)
        {
            return new ClientRequestResult { Request = new ClientRequest { Name = name?.ToUpper(), Unit = unit?.ToLower() }, Result = 0 };
        }
    }
}
