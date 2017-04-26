namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameObjectIntegerPropertyChangeEventArgs : EventArgs
    {
        private unsafe sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)* m_prop;
        private int m_value;

        public unsafe GameObjectIntegerPropertyChangeEventArgs(sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)* prop, int value)
        {
            this.m_prop = prop;
            this.m_value = value;
        }

        public string Property =>
            new string(this.m_prop);

        public int Value =>
            this.m_value;

        public delegate void GameObjectIntegerPropertyChangeEvent(GameObject sender, EventArgs args);
    }
}

