namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class PlayerSwapItemEventArgs : EventArgs
    {
        private bool m_process;
        private AIHeroClient m_sender;
        private int m_sourceSlotId;
        private int m_targetSlotId;

        public PlayerSwapItemEventArgs(AIHeroClient sender, int sourceSlotId, int targetSlotId)
        {
            this.m_sender = sender;
            this.m_sourceSlotId = sourceSlotId;
            this.m_targetSlotId = targetSlotId;
            this.m_process = true;
        }

        public int From =>
            this.m_sourceSlotId;

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

        public int To =>
            this.m_targetSlotId;

        public delegate void PlayerSwapItemEvent(AIHeroClient sender, PlayerSwapItemEventArgs args);
    }
}

