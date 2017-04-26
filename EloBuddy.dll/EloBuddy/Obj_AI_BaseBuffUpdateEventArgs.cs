namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class Obj_AI_BaseBuffUpdateEventArgs : EventArgs
    {
        private BuffInstance <backing_store>m_buff;

        public Obj_AI_BaseBuffUpdateEventArgs(BuffInstance buff)
        {
            this.m_buff = buff;
        }

        public BuffInstance Buff =>
            this.m_buff;

        private BuffInstance m_buff
        {
            get => 
                this.<backing_store>m_buff;
            set
            {
                this.<backing_store>m_buff = value;
            }
        }

        public delegate void Obj_AI_BaseBuffUpdateEvent(Obj_AI_Base sender, Obj_AI_BaseBuffUpdateEventArgs args);
    }
}

