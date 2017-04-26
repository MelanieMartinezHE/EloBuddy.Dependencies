namespace EloBuddy
{
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class GameObjectNewPathEventArgs : EventArgs
    {
        private bool m_isDash;
        private Vector3[] m_newPath;
        private float m_speed;

        public GameObjectNewPathEventArgs(Vector3[] newPath, [MarshalAs(UnmanagedType.U1)] bool isDash, float speed)
        {
            this.m_newPath = newPath;
            this.m_isDash = isDash;
            this.m_speed = speed;
        }

        public bool IsDash =>
            this.m_isDash;

        public Vector3[] Path =>
            this.m_newPath;

        public float Speed =>
            this.m_speed;

        public delegate void GameObjectNewPathEvent(Obj_AI_Base sender, GameObjectNewPathEventArgs args);
    }
}

