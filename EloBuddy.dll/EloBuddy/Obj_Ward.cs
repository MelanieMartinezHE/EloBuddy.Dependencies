namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_Ward : EloBuddy.Obj_AI_Base
    {
        public Obj_Ward()
        {
        }

        public unsafe Obj_Ward(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }

        public WardType Type =>
            WardType.Unknown;
    }
}

