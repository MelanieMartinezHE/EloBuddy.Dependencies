namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;

    public class Obj_AI_Minion : EloBuddy.Obj_AI_Base
    {
        public Obj_AI_Minion()
        {
        }

        public unsafe Obj_AI_Minion(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }

        internal unsafe EloBuddy.Native.Obj_AI_Minion* GetPtr() => 
            ((EloBuddy.Native.Obj_AI_Minion*) base.GetPtr());

        public int CampNumber
        {
            get
            {
                EloBuddy.Native.Obj_AI_Minion* ptr = (EloBuddy.Native.Obj_AI_Minion*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Minion.GetCampNumber(ptr));
                }
                return 0;
            }
        }

        public Vector3 LeashedPosition
        {
            get
            {
                EloBuddy.Native.Obj_AI_Minion* ptr = (EloBuddy.Native.Obj_AI_Minion*) base.GetPtr();
                if (ptr != null)
                {
                    Vector3f* vectorfPtr = EloBuddy.Native.Obj_AI_Minion.GetLeashedPosition(ptr);
                    if (vectorfPtr != null)
                    {
                        return new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                    }
                }
                return Vector3.Zero;
            }
        }

        public int MinionLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Minion* ptr = (EloBuddy.Native.Obj_AI_Minion*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Minion.GetMinionLevel(ptr));
                }
                return 0;
            }
        }

        public int OriginalState
        {
            get
            {
                EloBuddy.Native.Obj_AI_Minion* ptr = (EloBuddy.Native.Obj_AI_Minion*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Minion.GetOriginalState(ptr));
                }
                return 0;
            }
        }

        public int RoamState
        {
            get
            {
                EloBuddy.Native.Obj_AI_Minion* ptr = (EloBuddy.Native.Obj_AI_Minion*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Minion.GetRoamState(ptr));
                }
                return 0;
            }
        }
    }
}

