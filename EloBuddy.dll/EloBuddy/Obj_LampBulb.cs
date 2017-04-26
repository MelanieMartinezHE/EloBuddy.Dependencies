namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_LampBulb : EloBuddy.GameObject
    {
        public Obj_LampBulb()
        {
        }

        public unsafe Obj_LampBulb(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

