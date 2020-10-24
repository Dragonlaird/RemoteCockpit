using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteCockpitClasses
{
    /// <summary>
    /// Class to encapsulate all different types of JSON request from a Client Socket Connection
    /// </summary>
    public class RemoteRequest
    {
        IEnumerable<SimVarRequest> Variables { get; set; }
    }
}
