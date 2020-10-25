using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses
{
    public class ClientRequestResult
    {
        private object _result;
        private ClientRequest _request;
        public ClientRequest Request
        {
            get
            {
                return _request;
            }
            set
            {
                if(_request != value)
                {
                    _request = value;
                    LastUpdated = DateTime.Now;
                }
            }
        }
        public object Result
        {
            get
            {
                return _result;
            }
            set
            {
                if(_result != value)
                {
                    _result = value;
                    LastUpdated = DateTime.Now;
                }
            }
        }
        public DateTime LastUpdated { get; private set; }
    }
}
