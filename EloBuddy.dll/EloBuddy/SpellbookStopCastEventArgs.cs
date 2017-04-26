namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class SpellbookStopCastEventArgs : EventArgs
    {
        private int m_counter;
        private bool m_destroyMissile;
        private bool m_executeCastFrame;
        private bool m_forceStop;
        private uint m_missileNetworkId;
        private bool m_stopAnimation;

        public SpellbookStopCastEventArgs([MarshalAs(UnmanagedType.U1)] bool stopAnimation, [MarshalAs(UnmanagedType.U1)] bool executeCastFrame, [MarshalAs(UnmanagedType.U1)] bool forceStop, [MarshalAs(UnmanagedType.U1)] bool destroyMissile, uint missileNetworkId, int counter)
        {
            this.m_stopAnimation = stopAnimation;
            this.m_executeCastFrame = executeCastFrame;
            this.m_forceStop = forceStop;
            this.m_destroyMissile = destroyMissile;
            this.m_missileNetworkId = missileNetworkId;
            this.m_counter = counter;
        }

        public int Counter =>
            this.m_counter;

        public bool DestroyMissile =>
            this.m_destroyMissile;

        public bool ExecuteCastFrame =>
            this.m_executeCastFrame;

        public bool ForceStop =>
            this.m_forceStop;

        public uint MissileNetworkId =>
            this.m_missileNetworkId;

        public bool StopAnimation =>
            this.m_stopAnimation;

        public delegate void SpellbookStopCastEvent(Obj_AI_Base sender, SpellbookStopCastEventArgs args);
    }
}

