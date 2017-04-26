namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Collections.Generic;

    [Obsolete("This class has been replaced with MissileClient.")]
    public class Obj_SpellMissile : EloBuddy.GameObject
    {
        private unsafe EloBuddy.Native.Obj_SpellMissile* self;

        public Obj_SpellMissile()
        {
        }

        public unsafe Obj_SpellMissile(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }

        public Vector3Time[] GetPath(float precision) => 
            new List<Vector3Time>().ToArray();

        public Vector3 GetPositionAfterTime(float timeElapsed) => 
            Vector3.Zero;

        public Vector3 EndPosition =>
            Vector3.Zero;

        public EloBuddy.SpellData SData =>
            null;

        public EloBuddy.Obj_AI_Base SpellCaster =>
            null;

        public Vector3 StartPosition =>
            Vector3.Zero;

        public EloBuddy.GameObject Target =>
            null;
    }
}

