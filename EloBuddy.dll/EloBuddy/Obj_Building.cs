namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_Building : EloBuddy.AttackableUnit
    {
        public Obj_Building()
        {
        }

        public unsafe Obj_Building(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

