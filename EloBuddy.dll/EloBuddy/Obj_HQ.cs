namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_HQ : Obj_AnimatedBuilding
    {
        public Obj_HQ()
        {
        }

        public unsafe Obj_HQ(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

