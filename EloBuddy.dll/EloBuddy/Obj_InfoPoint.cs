namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_InfoPoint : EloBuddy.GameObject
    {
        public Obj_InfoPoint()
        {
        }

        public unsafe Obj_InfoPoint(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

