using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpitClasses
{
    public class SimVarRequest
    {
        private string _name;
        private string _unit;
        public int ID { get; set; }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value?.ToUpper();
            }
        }
        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value?.ToLower();
            }
        }
        public Type Type { get
            {
                return SimVarUnits.DefaultUnits[this.Name].UnitType;
            }
        }
        public SIMCONNECT_DATATYPE SimType
        {
            get
            {
                return SimVarUnits.GetSimVarType(this.Type?.ToString() ?? SimVarUnits.DefaultUnits[this.Name].DefaultUnit);
            }
        }

        public REQUEST ReqID
        {
            get
            {
                return (REQUEST)this.ID;
            }
        }
        public DEFINITION DefID
        {
            get
            {
                return (DEFINITION)this.ID;
            }
        }
    }
}
