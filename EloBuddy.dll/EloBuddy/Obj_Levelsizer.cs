namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_Levelsizer : EloBuddy.GameObject
    {
        public Obj_Levelsizer()
        {
        }

        public unsafe Obj_Levelsizer(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }
    }
}

