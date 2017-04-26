namespace EloBuddy
{
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;

    public class Obj_AI_UpdatePositionEventArgs : EventArgs
    {
        private Vector3 m_position;
        private Obj_AI_Base m_sender;

        public Obj_AI_UpdatePositionEventArgs(Obj_AI_Base sender, Vector3 position)
        {
            this.m_sender = sender;
            this.m_position = position;
        }

        public Vector3 Position =>
            this.m_position;

        public Obj_AI_Base Sender =>
            this.m_sender;

        public delegate void Obj_AI_UpdatePosition(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args);
    }
}

