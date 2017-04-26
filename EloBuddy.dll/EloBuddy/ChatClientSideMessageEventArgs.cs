namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ChatClientSideMessageEventArgs : EventArgs
    {
        private unsafe sbyte modopt(IsSignUnspecifiedByte)** m_message;
        private bool m_process;

        public unsafe ChatClientSideMessageEventArgs(sbyte modopt(IsSignUnspecifiedByte)** message)
        {
            this.m_message = message;
            this.m_process = true;
        }

        public string Message
        {
            get => 
                new string(*((sbyte**) this.m_message));
            set
            {
                *((int*) this.m_message) = Marshal.StringToHGlobalAnsi(value).ToPointer();
            }
        }

        public bool Process
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                this.m_process;
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                this.m_process = value;
            }
        }
    }
}

