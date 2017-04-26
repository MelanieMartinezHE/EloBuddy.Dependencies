namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_AnimatedBuilding : Obj_Building
    {
        public Obj_AnimatedBuilding()
        {
        }

        public unsafe Obj_AnimatedBuilding(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

