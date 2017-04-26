namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_Shop : Obj_Building
    {
        public Obj_Shop()
        {
        }

        public unsafe Obj_Shop(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

