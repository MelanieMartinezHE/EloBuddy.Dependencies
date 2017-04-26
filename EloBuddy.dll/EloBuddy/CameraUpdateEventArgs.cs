namespace EloBuddy
{
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class CameraUpdateEventArgs : EventArgs
    {
        private float m_mouseX;
        private float m_mouseY;
        private bool m_process = true;

        public CameraUpdateEventArgs(float mouseX, float mouseY)
        {
        }

        public Vector2 Mouse2D =>
            new Vector2(this.m_mouseX, this.m_mouseY);

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

        public delegate void CameraUpdateEvent(CameraUpdateEventArgs args);
    }
}

