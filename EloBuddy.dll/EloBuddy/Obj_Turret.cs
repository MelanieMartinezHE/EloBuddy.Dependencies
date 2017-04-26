namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_Turret : Obj_AnimatedBuilding
    {
        public Obj_Turret()
        {
        }

        public unsafe Obj_Turret(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

