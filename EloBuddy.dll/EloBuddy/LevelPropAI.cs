namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class LevelPropAI : EloBuddy.GameObject
    {
        public LevelPropAI()
        {
        }

        public unsafe LevelPropAI(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

