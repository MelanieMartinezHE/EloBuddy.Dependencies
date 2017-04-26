namespace EloBuddy
{
    using EloBuddy.Native;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class BuffInstance
    {
        private short m_index;
        private uint m_networkId;
        private unsafe EloBuddy.Native.BuffInstance* self;

        public unsafe BuffInstance(EloBuddy.Native.BuffInstance* inst, uint networkId, short index)
        {
            this.self = inst;
            this.m_networkId = networkId;
            this.m_index = index;
        }

        internal unsafe EloBuddy.Native.BuffInstance* GetBuffPtr()
        {
            EloBuddy.Native.BuffInstance* self = this.self;
            if (self == null)
            {
                EloBuddy.Native.Obj_AI_Base* basePtr = EloBuddy.Native.ObjectManager.GetUnitByNetworkId(this.m_networkId);
                if (basePtr != null)
                {
                    BuffManager* managerPtr = EloBuddy.Native.Obj_AI_Base.GetBuffManager(basePtr);
                    if (managerPtr != null)
                    {
                        BuffNode* nodePtr = *(EloBuddy.Native.BuffManager.GetBegin(managerPtr));
                        BuffNode* nodePtr2 = *(EloBuddy.Native.BuffManager.GetEnd(managerPtr));
                        if ((nodePtr != null) && (nodePtr2 != null))
                        {
                            uint num = 0;
                            int num2 = (int) ((nodePtr2 - nodePtr) >> 3);
                            if (0 < num2)
                            {
                                do
                                {
                                    EloBuddy.Native.BuffInstance* instancePtr2 = *((EloBuddy.Native.BuffInstance**) ((num * 8) + nodePtr));
                                    if ((instancePtr2 != null) && (instancePtr2 == this.self))
                                    {
                                        return instancePtr2;
                                    }
                                    num++;
                                }
                                while (num < num2);
                            }
                        }
                    }
                }
                self = this.self;
                if (self == null)
                {
                    throw new BuffInstanceNotFoundException();
                }
            }
            return self;
        }

        internal unsafe EloBuddy.Native.BuffInstance* GetPtr() => 
            this.GetBuffPtr();

        public EloBuddy.GameObject Caster
        {
            get
            {
                try
                {
                    EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                    if (buffPtr != null)
                    {
                        BuffScriptInstance* instancePtr = EloBuddy.Native.BuffInstance.GetBuffScriptInstance(buffPtr);
                        if (instancePtr != null)
                        {
                            EloBuddy.Native.GameObject* objPtr = EloBuddy.Native.BuffScriptInstance.GetCaster(instancePtr);
                            if (objPtr != null)
                            {
                                return ObjectManager.CreateObjectFromPointer(objPtr);
                            }
                        }
                    }
                }
                catch (Exception exception2)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("========================================");
                    System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                    System.Console.WriteLine();
                    System.Console.WriteLine("Type: {0}", exception2.GetType().FullName);
                    System.Console.WriteLine("Message: {0}", exception2.Message);
                    System.Console.WriteLine();
                    System.Console.WriteLine("Stracktrace:");
                    System.Console.WriteLine(exception2.StackTrace);
                    Exception innerException = exception2.InnerException;
                    if (innerException != null)
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("InnerException(s):");
                        do
                        {
                            System.Console.WriteLine("----------------------------------------");
                            System.Console.WriteLine("Type: {0}", innerException.GetType().FullName);
                            System.Console.WriteLine("Message: {0}", innerException.Message);
                            System.Console.WriteLine();
                            System.Console.WriteLine("Stracktrace:");
                            System.Console.WriteLine(innerException.StackTrace);
                            innerException = innerException.InnerException;
                        }
                        while (innerException != null);
                        System.Console.WriteLine("----------------------------------------");
                    }
                    System.Console.WriteLine("========================================");
                    System.Console.WriteLine();
                }
                return (EloBuddy.GameObject) ObjectManager.Player;
            }
        }

        public int Count
        {
            get
            {
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    return EloBuddy.Native.BuffInstance.GetCount(buffPtr);
                }
                return 0;
            }
        }

        public int CountAlt
        {
            get
            {
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    return EloBuddy.Native.BuffInstance.GetCountAlt(buffPtr);
                }
                return 0;
            }
        }

        public string DisplayName
        {
            get
            {
                EloBuddy.Native.BuffInstance* instancePtr = this.GetBuffPtr();
                if (instancePtr != null)
                {
                    ScriptBaseBuff* buffPtr = EloBuddy.Native.BuffInstance.GetScriptBaseBuff(instancePtr);
                    if (buffPtr != null)
                    {
                        VirtualScriptBaseBuff* modopt(CallConvThiscall) local1 = EloBuddy.Native.ScriptBaseBuff.GetVirtual(buffPtr);
                        return new string(*local1[0][0x34](local1));
                    }
                }
                return "Unknown";
            }
        }

        public float EndTime
        {
            get
            {
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    return *(EloBuddy.Native.BuffInstance.GetEndTime(buffPtr));
                }
                return 0f;
            }
        }

        public int Index =>
            this.m_index;

        public bool IsActive
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr == null)
                {
                    return false;
                }
                return EloBuddy.Native.BuffInstance.IsActive(buffPtr);
            }
        }

        public bool IsBlind
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type)) == 0x18)
                    {
                        num = 1;
                        goto Label_001F;
                    }
                }
                num = 0;
            Label_001F:
                return (bool) ((byte) num);
            }
        }

        public bool IsDisarm
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type)) == 0x1f)
                    {
                        num = 1;
                        goto Label_001F;
                    }
                }
                num = 0;
            Label_001F:
                return (bool) ((byte) num);
            }
        }

        public bool IsFear
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type)) == 20)
                    {
                        num = 1;
                        goto Label_001F;
                    }
                }
                num = 0;
            Label_001F:
                return (bool) ((byte) num);
            }
        }

        public bool IsInternal
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type)) != 0)
                    {
                        num = 0;
                        goto Label_001D;
                    }
                }
                num = 1;
            Label_001D:
                return (bool) ((byte) num);
            }
        }

        public bool IsKnockback
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type)) == 30)
                    {
                        num = 1;
                        goto Label_001F;
                    }
                }
                num = 0;
            Label_001F:
                return (bool) ((byte) num);
            }
        }

        public bool IsKnockup
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type)) == 0x1d)
                    {
                        num = 1;
                        goto Label_001F;
                    }
                }
                num = 0;
            Label_001F:
                return (bool) ((byte) num);
            }
        }

        public bool IsPermanent
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr == null)
                {
                    return false;
                }
                return EloBuddy.Native.BuffInstance.IsPermanent(buffPtr);
            }
        }

        public bool IsPositive
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr == null)
                {
                    return false;
                }
                return EloBuddy.Native.BuffInstance.IsPositive(buffPtr);
            }
        }

        public bool IsRoot
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type)) == 11)
                    {
                        num = 1;
                        goto Label_001F;
                    }
                }
                num = 0;
            Label_001F:
                return (bool) ((byte) num);
            }
        }

        public bool IsSilence
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type)) == 7)
                    {
                        num = 1;
                        goto Label_001E;
                    }
                }
                num = 0;
            Label_001E:
                return (bool) ((byte) num);
            }
        }

        public bool IsSlow
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type)) == 10)
                    {
                        num = 1;
                        goto Label_001F;
                    }
                }
                num = 0;
            Label_001F:
                return (bool) ((byte) num);
            }
        }

        public bool IsStunOrSuppressed
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type2 = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type2)) == 5)
                    {
                        goto Label_0036;
                    }
                }
                EloBuddy.Native.BuffInstance* instancePtr = this.GetBuffPtr();
                if (instancePtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(instancePtr));
                    if (*(((int*) &type)) == 0x17)
                    {
                        goto Label_0036;
                    }
                }
                int num = 0;
                goto Label_0038;
            Label_0036:
                num = 1;
            Label_0038:
                return (bool) ((byte) num);
            }
        }

        public bool IsSuppression
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    BuffType type = *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                    if (*(((int*) &type)) == 0x17)
                    {
                        num = 1;
                        goto Label_001F;
                    }
                }
                num = 0;
            Label_001F:
                return (bool) ((byte) num);
            }
        }

        public bool IsValid
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                try
                {
                    int num;
                    byte num4;
                    if (this.GetBuffPtr() != null)
                    {
                        EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                        if ((buffPtr != null) && EloBuddy.Native.BuffInstance.IsPositive(buffPtr))
                        {
                            EloBuddy.Native.BuffInstance* instancePtr = this.GetBuffPtr();
                            if ((instancePtr != null) && EloBuddy.Native.BuffInstance.IsActive(instancePtr))
                            {
                                num = 1;
                                goto Label_0047;
                            }
                        }
                    }
                    num = 0;
                Label_0047:
                    num4 = (byte) num;
                    return (bool) num4;
                }
                catch (Exception exception2)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("========================================");
                    System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                    System.Console.WriteLine();
                    System.Console.WriteLine("Type: {0}", exception2.GetType().FullName);
                    System.Console.WriteLine("Message: {0}", exception2.Message);
                    System.Console.WriteLine();
                    System.Console.WriteLine("Stracktrace:");
                    System.Console.WriteLine(exception2.StackTrace);
                    Exception innerException = exception2.InnerException;
                    if (innerException != null)
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("InnerException(s):");
                        do
                        {
                            System.Console.WriteLine("----------------------------------------");
                            System.Console.WriteLine("Type: {0}", innerException.GetType().FullName);
                            System.Console.WriteLine("Message: {0}", innerException.Message);
                            System.Console.WriteLine();
                            System.Console.WriteLine("Stracktrace:");
                            System.Console.WriteLine(innerException.StackTrace);
                            innerException = innerException.InnerException;
                        }
                        while (innerException != null);
                        System.Console.WriteLine("----------------------------------------");
                    }
                    System.Console.WriteLine("========================================");
                    System.Console.WriteLine();
                }
                return false;
            }
        }

        public bool IsVisible
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    return *(EloBuddy.Native.BuffInstance.GetIsVisible(buffPtr));
                }
                return false;
            }
        }

        public IntPtr MemoryAddress =>
            ((IntPtr) this.GetBuffPtr());

        public string Name
        {
            get
            {
                EloBuddy.Native.BuffInstance* instancePtr = this.GetBuffPtr();
                if (instancePtr != null)
                {
                    ScriptBaseBuff* buffPtr = EloBuddy.Native.BuffInstance.GetScriptBaseBuff(instancePtr);
                    if ((buffPtr != null) && (EloBuddy.Native.ScriptBaseBuff.GetName(buffPtr) != null))
                    {
                        return new string(EloBuddy.Native.ScriptBaseBuff.GetName(buffPtr));
                    }
                }
                return "Unknown";
            }
        }

        public string SourceName
        {
            get
            {
                EloBuddy.Native.BuffInstance* instancePtr = this.GetBuffPtr();
                if (instancePtr != null)
                {
                    ScriptBaseBuff* buffPtr2 = EloBuddy.Native.BuffInstance.GetScriptBaseBuff(instancePtr);
                    if (buffPtr2 != null)
                    {
                        ChildScriptBuff* buffPtr = EloBuddy.Native.ScriptBaseBuff.GetChildScriptBuff(buffPtr2);
                        if (buffPtr != null)
                        {
                            return new string(EloBuddy.Native.ChildScriptBuff.GetSourceName(buffPtr));
                        }
                    }
                }
                return "Unknown";
            }
        }

        public float StartTime
        {
            get
            {
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    return *(EloBuddy.Native.BuffInstance.GetStartTime(buffPtr));
                }
                return 0f;
            }
        }

        public BuffType Type
        {
            get
            {
                EloBuddy.Native.BuffInstance* buffPtr = this.GetBuffPtr();
                if (buffPtr != null)
                {
                    return *(EloBuddy.Native.BuffInstance.GetType(buffPtr));
                }
                return BuffType.Internal;
            }
        }
    }
}

