namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ChatWhisperEventArgs : EventArgs
    {
        private unsafe sbyte modopt(IsSignUnspecifiedByte)** m_message;
        private bool m_process;
        private unsafe sbyte modopt(IsSignUnspecifiedByte)** m_sender;

        public unsafe ChatWhisperEventArgs(sbyte modopt(IsSignUnspecifiedByte)** sender, sbyte modopt(IsSignUnspecifiedByte)** message)
        {
            this.m_sender = sender;
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

        public string Sender
        {
            get => 
                new string(*((sbyte**) this.m_sender));
            set
            {
                *((int*) this.m_sender) = Marshal.StringToHGlobalAnsi(value).ToPointer();
            }
        }
    }
}

