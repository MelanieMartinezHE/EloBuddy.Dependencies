namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class Obj_AI_BaseLevelUpEventArgs : EventArgs
    {
        private int m_level;
        private Obj_AI_Base m_sender;

        public Obj_AI_BaseLevelUpEventArgs(Obj_AI_Base sender, int level)
        {
            this.m_sender = sender;
            this.m_level = level;
        }

        public int Level =>
            this.m_level;

        public delegate void Obj_AI_BaseLevel(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args);
    }
}

