namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    [Obsolete("This class has been replaced with MissileClient.")]
    public class Obj_SpellChainMissile : EloBuddy.Obj_SpellMissile
    {
        public Obj_SpellChainMissile()
        {
        }

        public unsafe Obj_SpellChainMissile(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

