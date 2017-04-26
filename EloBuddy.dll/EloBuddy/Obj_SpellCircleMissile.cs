namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    [Obsolete("This class has been replaced with MissileClient.")]
    public class Obj_SpellCircleMissile : EloBuddy.Obj_SpellMissile
    {
        public Obj_SpellCircleMissile()
        {
        }

        public unsafe Obj_SpellCircleMissile(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

