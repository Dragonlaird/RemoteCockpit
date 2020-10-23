using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpit
{
    interface ICockpitInstrument
    {
        IEnumerable<SimVarRequest> RequiredValues { get; }

        void ValueUpdate(SimVarRequestResult value);
    }
}
