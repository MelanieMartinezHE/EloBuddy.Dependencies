namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class PlayerDoEmoteEventArgs : EventArgs
    {
        private short m_emoteId;
        private bool m_process;

        public PlayerDoEmoteEventArgs(AIHeroClient sender, short emoteId)
        {
            this.m_emoteId = emoteId;
            this.m_process = true;
        }

        public Emote EmoteId =>
            ((Emote) this.m_emoteId);

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

        public delegate void PlayerDoEmoteEvent(AIHeroClient sender, PlayerDoEmoteEventArgs args);
    }
}

