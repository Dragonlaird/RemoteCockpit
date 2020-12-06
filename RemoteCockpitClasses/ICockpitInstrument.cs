using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Windows.Forms;

namespace RemoteCockpitClasses
{
    public interface ICockpitInstrument : IComponent
    {
        /// <summary>
        /// Values required by this Cockpit Instrument
        /// </summary>
        IEnumerable<ClientRequest> RequiredValues { get; }

        /// <summary>
        /// Name of this plugin
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Author of this plugin
        /// </summary>
        string Author { get; }
        /// <summary>
        /// Called whenever one of the RequiredValues recieves an update
        /// </summary>
        /// <param name="value">SimVarRequestResult, containing SimVarRequest and Value</param>
        void ValueUpdate(ClientRequestResult value);
        /// <summary>
        /// Cockpit Layouts this can be used on.
        /// An empty string means it can be used on any Cockpit Layout (not too useful)
        /// </summary>
        string [] Aircraft { get; }
        /// <summary>
        /// Date this CockpitInstrument was created (not published or updated).
        /// Newest date is used in preference over generic Cockpit Instruments, for specified Cockpit Layouts
        /// </summary>
        DateTime PluginDate { get; }
        /// <summary>
        /// Expected frequence of updates from FS, useful for controlling animations in a timely manner
        /// </summary>
        int UpdateFrequency { get; set; }

        InstrumentType Type { get; }

        Control Control { get; }

        void SetLayout(int top, int left, int height, int width);

        event EventHandler<string> LogMessage;
    }
}
