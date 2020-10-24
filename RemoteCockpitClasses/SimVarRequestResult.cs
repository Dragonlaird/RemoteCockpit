﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpitClasses
{
    public class SimVarRequestResult
    {
        private object _value;
        public SimVarRequest Request { get; set; }
        public object Value
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
