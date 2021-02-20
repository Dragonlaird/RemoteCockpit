using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpitServer
{
    /// <summary>
    /// Extension class for AsyncSocket to provide IsConnected Property and Event Handler for Connection drop
    /// </summary>
    public static class AsynchronousSocketExtensions
    {
        /// <summary>
        /// Parent handler to call when a client connection drops
        /// </summary>
        public static EventHandler<StateObject> ConnectionDropped;

        /// <summary>
        /// Is this socket currently connected?
        /// If dropped, report it to parent class
        /// </summary>
        /// <param name="socket">Socket to check for connected state</param>
        /// <returns>Status of socket connection</returns>
        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch// (SocketException ex)
            {
                if(ConnectionDropped != null)
                {
                    try
                    {
                        StateObject socketState = new StateObject { workSocket = socket, sb = new StringBuilder(), buffer = new byte[0] };
                        ConnectionDropped.DynamicInvoke(socketState);
                    }
                    catch { }
                }
                return false;
            }
        }
    }
}
