namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class FollowerObjectWithLerpMovement : FollowerObject
    {
        public FollowerObjectWithLerpMovement()
        {
        }

        public unsafe FollowerObjectWithLerpMovement(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

