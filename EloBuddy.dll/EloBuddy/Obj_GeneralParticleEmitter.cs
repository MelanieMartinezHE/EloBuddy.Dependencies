namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_GeneralParticleEmitter : EloBuddy.GameObject
    {
        public Obj_GeneralParticleEmitter()
        {
        }

        public unsafe Obj_GeneralParticleEmitter(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

