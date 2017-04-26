namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_Lake : Obj_Building
    {
        public Obj_Lake()
        {
        }

        public unsafe Obj_Lake(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

