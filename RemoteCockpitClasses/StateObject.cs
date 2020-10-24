using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace RemoteCockpitClasses
{
    public class StateObject
    {
        public int ConnectionID { get; set; }
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received content
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
    }
}
