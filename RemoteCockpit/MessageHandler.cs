using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteCockpit
{
    // Adaped from http://stackoverflow.com/questions/2443867/message-pump-in-net-windows-service

    internal class MessageHandler : NativeWindow
    {
        public event EventHandler<Message> MessageReceived;

        public MessageHandler()
        {
        }

        internal void CreateHandle()
        {
            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message msg)
        {
            // filter messages here for your purposes
            if (msg.Msg == 1026 && MessageReceived != null)
                MessageReceived.DynamicInvoke(this, msg);
            else
                base.WndProc(ref msg);
        }
    }

    public class MessagePumpManager
    {
        private readonly Thread messagePump;
        private AutoResetEvent messagePumpRunning = new AutoResetEvent(false);
        private SimConnect simConnect = null;
        public EventHandler<bool> SimConnected;
        public EventHandler<SIMCONNECT_RECV_EXCEPTION> SimError;
        public EventHandler<SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE> SimData;
        private const int WM_USER_SIMCONNECT = 0x0402;
        private const bool bFSXcompatible = false;

        public MessagePumpManager()
        {
            // start message pump in its own thread
            messagePump = new Thread(RunMessagePump) { Name = "ManualMessagePump" };
            messagePump.Start();
            messagePumpRunning.WaitOne();
        }

        public void AddRequest(SimVarRequest request)
        {
            simConnect.AddToDataDefinition(request.DefID, request.Name, request.Unit, SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simConnect.RegisterDataDefineStruct<double>(request.DefID);
        }

        // Message Pump Thread
        private void RunMessagePump()
        {
            // Create control to handle windows messages
            MessageHandler messageHandler = new MessageHandler();
            messageHandler.CreateHandle();
            ConnectFS(messageHandler);
            messagePumpRunning.Set();
            Application.Run();
        }

        private void MessageReceived(object sender, Message msg)
        {
            if (msg.Msg == WM_USER_SIMCONNECT && simConnect != null)
                try
                {
                    simConnect.ReceiveMessage();
                }
                catch(Exception ex)
                {
                    // Seen to happen if FS is shutting down
                }
        }

        public void GetValue(SimVarRequest request)
        {
            try
            {
                simConnect?.RequestDataOnSimObjectType(request.ReqID, request.DefID, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            }
            catch(Exception ex)
            {
                // Likely cause, no request for this variable has been received
            }
        }

        private void ConnectFS(MessageHandler messageHandler)
        {
            // SimConnect must be linked in the same thread as the Application.Run()
            try
            {
                simConnect = new SimConnect("Remote Cockpit", messageHandler.Handle, WM_USER_SIMCONNECT, null, bFSXcompatible ? (uint)1 : 0);

                messageHandler.MessageReceived += MessageReceived;

                /// Listen for Connect
                simConnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);

                /// Listen for Disconnect
                simConnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

                /// Listen for Exceptions
                simConnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);

                /// Listen for SimVar Data
                simConnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);

            }
            catch(Exception ex){

            }
        }

        private void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            if (SimData != null)
                SimData.DynamicInvoke(this, data);
        }

        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            if (SimError != null)
                SimError.DynamicInvoke(this, data);
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            if (SimConnected != null)
                SimConnected.DynamicInvoke(this, true);
        }

        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            if (SimConnected != null)
                SimConnected.DynamicInvoke(this, false);
        }
    }
}
