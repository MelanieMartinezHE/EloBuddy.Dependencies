namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class LevelPropSpawnerPoint : EloBuddy.GameObject
    {
        public LevelPropSpawnerPoint()
        {
        }

        public unsafe LevelPropSpawnerPoint(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

