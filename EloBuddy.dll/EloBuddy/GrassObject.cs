namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class GrassObject : EloBuddy.GameObject
    {
        public GrassObject()
        {
        }

        public unsafe GrassObject(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

