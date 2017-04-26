namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class DrawFX : EloBuddy.GameObject
    {
        public DrawFX()
        {
        }

        public unsafe DrawFX(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

