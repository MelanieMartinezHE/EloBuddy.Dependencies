namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    [Obsolete("This class has been replaced with MissileClient.")]
    public class Obj_SpellLineMissile : EloBuddy.Obj_SpellMissile
    {
        public Obj_SpellLineMissile()
        {
        }

        public unsafe Obj_SpellLineMissile(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

