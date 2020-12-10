using RemoteCockpitClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpit
{
    public static class AsynchronousSocketExtensions
    {
        public static EventHandler<StateObject> ConnectionDropped;
        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException ex) {
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
