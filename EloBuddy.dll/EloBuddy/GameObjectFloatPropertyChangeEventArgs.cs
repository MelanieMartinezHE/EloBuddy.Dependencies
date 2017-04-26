namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameObjectFloatPropertyChangeEventArgs : EventArgs
    {
        private unsafe sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)* m_prop;
        private float m_value;

        public unsafe GameObjectFloatPropertyChangeEventArgs(sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)* prop, float value)
        {
            this.m_prop = prop;
            this.m_value = value;
        }

        public string Property =>
            new string(this.m_prop);

        public float Value =>
            this.m_value;

        public delegate void GameObjectFloatPropertyChangeEvent(GameObject sender, EventArgs args);
    }
}

