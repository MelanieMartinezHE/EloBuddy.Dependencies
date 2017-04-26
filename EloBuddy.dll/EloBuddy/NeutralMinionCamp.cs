namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class NeutralMinionCamp : EloBuddy.GameObject
    {
        public NeutralMinionCamp()
        {
        }

        public unsafe NeutralMinionCamp(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

