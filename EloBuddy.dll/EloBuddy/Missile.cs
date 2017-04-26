namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Missile : EloBuddy.GameObject
    {
        public Missile()
        {
        }

        public unsafe Missile(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

