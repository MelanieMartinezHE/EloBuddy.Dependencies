namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_AI_Turret : EloBuddy.Obj_AI_Base
    {
        public Obj_AI_Turret()
        {
        }

        public unsafe Obj_AI_Turret(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

