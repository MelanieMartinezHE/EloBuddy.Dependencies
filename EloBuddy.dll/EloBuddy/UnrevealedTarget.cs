namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class UnrevealedTarget : EloBuddy.GameObject
    {
        public UnrevealedTarget()
        {
        }

        public unsafe UnrevealedTarget(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

