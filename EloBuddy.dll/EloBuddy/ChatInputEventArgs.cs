namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ChatInputEventArgs : EventArgs
    {
        private unsafe sbyte modopt(IsSignUnspecifiedByte)** m_input;
        private bool m_process;

        public unsafe ChatInputEventArgs(sbyte modopt(IsSignUnspecifiedByte)** input)
        {
            this.m_input = input;
            this.m_process = true;
        }

        public string Input
        {
            get => 
                new string(*((sbyte**) this.m_input));
            set
            {
                *((int*) this.m_input) = Marshal.StringToHGlobalAnsi(value).ToPointer();
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

        public delegate void ChatInputEvent(ChatInputEventArgs args);
    }
}

