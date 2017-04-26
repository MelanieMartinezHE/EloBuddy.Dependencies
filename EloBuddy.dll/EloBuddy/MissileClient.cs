namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;

    public class MissileClient : EloBuddy.GameObject
    {
        internal unsafe EloBuddy.Native.MissileClient* self;

        public MissileClient()
        {
        }

        public unsafe MissileClient(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, base.m_networkId, unit)
        {
        }

        internal unsafe EloBuddy.Native.MissileClient* GetPtr() => 
            ((EloBuddy.Native.MissileClient*) base.GetPtrUncached());

        public Vector3 EndPosition
        {
            get
            {
                EloBuddy.Native.MissileClient* ptrUncached = (EloBuddy.Native.MissileClient*) base.GetPtrUncached();
                if (ptrUncached == null)
                {
                    return Vector3.Zero;
                }
                Vector3f* vectorfPtr = EloBuddy.Native.MissileClient.GetDestPos(ptrUncached);
                Vector3 vector = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                if ((vector == Vector3.Zero) && (this.Target != null))
                {
                    return this.Target.Position;
                }
                return vector;
            }
        }

        public EloBuddy.SpellData SData
        {
            get
            {
                EloBuddy.Native.MissileClient* ptrUncached = (EloBuddy.Native.MissileClient*) base.GetPtrUncached();
                if (ptrUncached != null)
                {
                    MissileClientData** dataPtr2 = EloBuddy.Native.MissileClient.GetMissileData(ptrUncached);
                    if (dataPtr2 != null)
                    {
                        EloBuddy.Native.SpellData* spelldata = EloBuddy.Native.MissileClientData.GetSData(*((MissileClientData* modopt(IsConst) modopt(IsConst)*) dataPtr2));
                        if (spelldata != null)
                        {
                            return new EloBuddy.SpellData(spelldata);
                        }
                    }
                }
                return null;
            }
        }

        public EloBuddy.SpellSlot Slot
        {
            get
            {
                EloBuddy.Native.MissileClient* ptrUncached = (EloBuddy.Native.MissileClient*) base.GetPtrUncached();
                if (ptrUncached != null)
                {
                    return *(EloBuddy.Native.MissileClient.GetSlot(ptrUncached));
                }
                return EloBuddy.SpellSlot.Unknown;
            }
        }

        public EloBuddy.Obj_AI_Base SpellCaster
        {
            get
            {
                EloBuddy.Native.MissileClient* ptrUncached = (EloBuddy.Native.MissileClient*) base.GetPtrUncached();
                if (ptrUncached != null)
                {
                    short* numPtr = EloBuddy.Native.MissileClient.GetSpellCaster(ptrUncached);
                    if (numPtr != null)
                    {
                        return (EloBuddy.Obj_AI_Base) ObjectManager.GetUnitByIndex(numPtr[0]);
                    }
                }
                return null;
            }
        }

        public Vector3 StartPosition
        {
            get
            {
                EloBuddy.Native.MissileClient* ptrUncached = (EloBuddy.Native.MissileClient*) base.GetPtrUncached();
                if (ptrUncached != null)
                {
                    Vector3f* vectorfPtr = EloBuddy.Native.MissileClient.GetLaunchPos(ptrUncached);
                    if (vectorfPtr != null)
                    {
                        return new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                    }
                }
                return Vector3.Zero;
            }
        }

        public EloBuddy.GameObject Target
        {
            get
            {
                EloBuddy.Native.MissileClient* ptrUncached = (EloBuddy.Native.MissileClient*) base.GetPtrUncached();
                if (ptrUncached != null)
                {
                    short* numPtr = EloBuddy.Native.MissileClient.GetTarget(ptrUncached);
                    if (numPtr != null)
                    {
                        return ObjectManager.GetUnitByIndex(numPtr[0]);
                    }
                }
                return null;
            }
        }
    }
}

