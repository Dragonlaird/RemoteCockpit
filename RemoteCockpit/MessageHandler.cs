using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteCockpit
{
    public class MessageHandler : NativeWindow
    {
        private const int WM_USER_SIMCONNECT = 0x0402;
        public EventHandler<LogMessage> LogReceived;

        public EventHandler MessageReceived;
        public MessageHandler()
        {
        }

        public void CreateHandle()
        {
            try
            {
                WriteLog("Creating Handle");
                CreateHandle(new CreateParams());
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message, EventLogEntryType.Error);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_USER_SIMCONNECT)
            {
                try
                {
                    if (MessageReceived != null)
                        MessageReceived.DynamicInvoke(null);
                }
                catch (COMException ex)
                {
                    WriteLog(ex.Message, EventLogEntryType.Error);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void WriteLog(string message, EventLogEntryType type = EventLogEntryType.Information)
        {
            if (LogReceived != null)
            {
                LogReceived.DynamicInvoke(this, new LogMessage { Message = message, Type = type });
            }
            else
            {
                var strType = type.ToString().Substring(0, 3);
                Console.WriteLine("[{0}] {1}", strType, message);
            }
        }
    }
}
