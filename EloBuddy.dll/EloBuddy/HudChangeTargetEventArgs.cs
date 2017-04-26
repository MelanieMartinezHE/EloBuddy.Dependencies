namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class HudChangeTargetEventArgs : EventArgs
    {
        private bool m_clear;
        private GameObject m_target;

        public HudChangeTargetEventArgs(GameObject target, [MarshalAs(UnmanagedType.U1)] bool clear)
        {
            this.m_target = target;
            this.m_clear = clear;
        }

        public bool Reset =>
            this.m_clear;

        public GameObject Target =>
            this.m_target;

        public delegate void HudChangeTargetEvent(HudChangeTargetEventArgs args);
    }
}

