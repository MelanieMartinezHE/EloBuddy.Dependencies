namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_NavPoint : EloBuddy.GameObject
    {
        public Obj_NavPoint()
        {
        }

        public unsafe Obj_NavPoint(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

