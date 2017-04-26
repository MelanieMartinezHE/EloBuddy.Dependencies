namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class WndEventArgs : EventArgs
    {
        private unsafe HWND__* m_hwnd;
        private int modopt(IsLong) m_lparam;
        private uint m_msg;
        private bool m_process;
        private uint m_wparam;

        public unsafe WndEventArgs(HWND__* HWnd, uint message, uint WParam, int modopt(IsLong) LParam)
        {
            this.m_hwnd = HWnd;
            this.m_msg = message;
            this.m_lparam = LParam;
            this.m_wparam = WParam;
            this.m_process = true;
        }

        public uint HWnd =>
            ((uint) this.m_hwnd);

        public int modopt(IsLong) LParam =>
            this.m_lparam;

        public uint Msg =>
            this.m_msg;

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

        public uint WParam =>
            this.m_wparam;

        public delegate void WndProcEvent(WndEventArgs args);
    }
}

