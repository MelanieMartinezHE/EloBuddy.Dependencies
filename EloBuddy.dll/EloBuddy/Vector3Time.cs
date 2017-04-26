namespace EloBuddy
{
    using SharpDX;
    using System;

    public class Vector3Time
    {
        private Vector3 m_pos;
        private float m_time;

        public Vector3Time(Vector3 pos, float time)
        {
            this.m_pos = pos;
            this.m_time = time;
        }

        public Vector3 Position =>
            this.m_pos;

        public float Time =>
            this.m_time;
    }
}

