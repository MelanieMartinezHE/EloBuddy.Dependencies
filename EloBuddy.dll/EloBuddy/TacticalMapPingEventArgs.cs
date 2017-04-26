namespace EloBuddy
{
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class TacticalMapPingEventArgs : EventArgs
    {
        private PingCategory m_pingType;
        private Vector2 m_position;
        private bool m_process;
        private GameObject m_source;
        private GameObject m_target;

        public TacticalMapPingEventArgs(Vector2 position, GameObject target, GameObject source, PingCategory pingType)
        {
            this.m_position = position;
            this.m_target = target;
            this.m_source = source;
            this.m_pingType = pingType;
            this.m_process = true;
        }

        public PingCategory PingType =>
            this.m_pingType;

        public Vector2 Position =>
            this.m_position;

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

        public GameObject Source =>
            this.m_source;

        public GameObject Target =>
            this.m_target;

        public delegate void TacticalMapPingEvent(TacticalMapPingEventArgs args);
    }
}

