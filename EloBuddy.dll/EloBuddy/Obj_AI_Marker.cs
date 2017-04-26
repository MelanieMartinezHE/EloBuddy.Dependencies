namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_AI_Marker : EloBuddy.Obj_AI_Base
    {
        public Obj_AI_Marker()
        {
        }

        public unsafe Obj_AI_Marker(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

