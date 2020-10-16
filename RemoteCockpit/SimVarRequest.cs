using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpit
{
    public class SimVarRequest
    {
        private string _name;
        private string _unit;
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

        public TimeSpan Frequency { get; set; }
    }
}
