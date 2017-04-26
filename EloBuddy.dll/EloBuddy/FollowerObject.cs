namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class FollowerObject : EloBuddy.Obj_AI_Base
    {
        public FollowerObject()
        {
        }

        public unsafe FollowerObject(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

