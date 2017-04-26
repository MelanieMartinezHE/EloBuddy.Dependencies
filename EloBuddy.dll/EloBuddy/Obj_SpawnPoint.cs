namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_SpawnPoint : Obj_Building
    {
        public Obj_SpawnPoint()
        {
        }

        public unsafe Obj_SpawnPoint(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

