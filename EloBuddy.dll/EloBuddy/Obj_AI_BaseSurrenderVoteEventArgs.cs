namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class Obj_AI_BaseSurrenderVoteEventArgs : EventArgs
    {
        private byte m_surrenderType;

        public Obj_AI_BaseSurrenderVoteEventArgs(byte surrenderType)
        {
            this.m_surrenderType = surrenderType;
        }

        public SurrenderVoteType Type =>
            ((SurrenderVoteType) this.m_surrenderType);

        public delegate void Obj_AI_BaseSurrenderVote(Obj_AI_Base sender, Obj_AI_BaseSurrenderVoteEventArgs args);
    }
}

