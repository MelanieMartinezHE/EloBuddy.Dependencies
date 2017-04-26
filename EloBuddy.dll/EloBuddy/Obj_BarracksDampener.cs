namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Obj_BarracksDampener : Obj_AnimatedBuilding
    {
        public Obj_BarracksDampener()
        {
        }

        public unsafe Obj_BarracksDampener(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }

        internal unsafe EloBuddy.Native.Obj_BarracksDampener* GetPtr() => 
            ((EloBuddy.Native.Obj_BarracksDampener*) base.GetPtr());

        public DampenerState State
        {
            get
            {
                EloBuddy.Native.Obj_BarracksDampener* ptr = (EloBuddy.Native.Obj_BarracksDampener*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_BarracksDampener.GetDampenerState(ptr));
                }
                return DampenerState.Unknown;
            }
        }
    }
}

