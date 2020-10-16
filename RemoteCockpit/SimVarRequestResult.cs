using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpit
{
    public class SimVarRequestResult
    {
        private string _name;
        private string _unit;
        private double _value;
        private REQUEST _id;

        public SimVarRequestResult()
        {
            LastUpdated = DateTime.UtcNow;
        }
        public SimVarRequestResult(string Name, string Unit)
        {
            this.Name = Name;
            this.Unit = Unit;
        }
        public REQUEST Id
        {
            get
            {
                return (REQUEST)_id;
            }
            set
            {
                if ((REQUEST)_id == 0)
                {
                    _id = (REQUEST)value;
                    LastUpdated = DateTime.UtcNow;
                }
            }
        } // Request ID

        public DEFINITION DefId
        {
            get
            {
                return (DEFINITION)Id;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == null)
                {
                    _name = value?.ToUpper();
                    LastUpdated = DateTime.UtcNow;
                }
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
                if (_unit == null)
                {
                    _unit = value?.ToLower();
                    LastUpdated = DateTime.UtcNow;
                }
            }
        }
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    LastUpdated = DateTime.UtcNow;
                }
            }
        }

        public DateTime LastUpdated { get; private set; }
    }
}
