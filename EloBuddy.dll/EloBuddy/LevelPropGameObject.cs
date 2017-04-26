namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class LevelPropGameObject : EloBuddy.GameObject
    {
        public LevelPropGameObject()
        {
        }

        public unsafe LevelPropGameObject(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

