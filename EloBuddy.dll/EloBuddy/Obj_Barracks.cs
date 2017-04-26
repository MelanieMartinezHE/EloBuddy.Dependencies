namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_Barracks : Obj_Building
    {
        public Obj_Barracks()
        {
        }

        public unsafe Obj_Barracks(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

