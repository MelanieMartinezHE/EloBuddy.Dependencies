namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Spellbook
    {
        private uint m_networkId;
        private unsafe EloBuddy.Native.Spellbook* m_spellbook;
        internal static IntPtr m_SpellbookCastSpellNative = new IntPtr();
        internal static OnSpellbookCastSpellNativeDelegate m_SpellbookCastSpellNativeDelegate;
        internal static IntPtr m_SpellbookPostCastSpellNative = new IntPtr();
        internal static OnSpellbookPostCastSpellNativeDelegate m_SpellbookPostCastSpellNativeDelegate;
        internal static IntPtr m_SpellbookStopCastNative = new IntPtr();
        internal static OnSpellbookStopCastNativeDelegate m_SpellbookStopCastNativeDelegate;
        internal static IntPtr m_SpellbookUpdateChargeableSpellNative = new IntPtr();
        internal static OnSpellbookUpdateChargeableSpellNativeDelegate m_SpellbookUpdateChargeableSpellNativeDelegate;
        private unsafe EloBuddy.Native.Obj_AI_Base* self;
        internal static List<SpellbookCastSpell> SpellbookCastSpellHandlers;
        internal static List<SpellbookPostCastSpell> SpellbookPostCastSpellHandlers;
        internal static List<SpellbookStopCast> SpellbookStopCastHandlers;
        internal static List<SpellbookUpdateChargeableSpell> SpellbookUpdateChargeableSpellHandlers;

        public static  event SpellbookCastSpell OnCastSpell
        {
            add
            {
                SpellbookCastSpellHandlers.Add(handler);
            }
            remove
            {
                SpellbookCastSpellHandlers.Remove(handler);
            }
        }

        public static  event SpellbookPostCastSpell OnPostCastSpell
        {
            add
            {
                SpellbookPostCastSpellHandlers.Add(handler);
            }
            remove
            {
                SpellbookPostCastSpellHandlers.Remove(handler);
            }
        }

        public static  event SpellbookStopCast OnStopCast
        {
            add
            {
                SpellbookStopCastHandlers.Add(handler);
            }
            remove
            {
                SpellbookStopCastHandlers.Remove(handler);
            }
        }

        public static  event SpellbookUpdateChargeableSpell OnUpdateChargeableSpell
        {
            add
            {
                SpellbookUpdateChargeableSpellHandlers.Add(handler);
            }
            remove
            {
                SpellbookUpdateChargeableSpellHandlers.Remove(handler);
            }
        }

        static Spellbook()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(EloBuddy.Spellbook.DomainUnloadEventHandler);
            SpellbookCastSpellHandlers = new List<SpellbookCastSpell>();
            m_SpellbookCastSpellNativeDelegate = new OnSpellbookCastSpellNativeDelegate(EloBuddy.Spellbook.OnSpellbookCastSpellNative);
            m_SpellbookCastSpellNative = Marshal.GetFunctionPointerForDelegate(m_SpellbookCastSpellNativeDelegate);
            EloBuddy.Native.EventHandler<21,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Spellbook *,EloBuddy::Native::Vector3f *,EloBuddy::Native::Vector3f *,unsigned int,int),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Spellbook *,EloBuddy::Native::Vector3f *,EloBuddy::Native::Vector3f *,unsigned int,int>.Add(EloBuddy.Native.EventHandler<21,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Spellbook *,EloBuddy::Native::Vector3f *,EloBuddy::Native::Vector3f *,unsigned int,int),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Spellbook *,EloBuddy::Native::Vector3f *,EloBuddy::Native::Vector3f *,unsigned int,int>.GetInstance(), m_SpellbookCastSpellNative.ToPointer());
            SpellbookStopCastHandlers = new List<SpellbookStopCast>();
            m_SpellbookStopCastNativeDelegate = new OnSpellbookStopCastNativeDelegate(EloBuddy.Spellbook.OnSpellbookStopCastNative);
            m_SpellbookStopCastNative = Marshal.GetFunctionPointerForDelegate(m_SpellbookStopCastNativeDelegate);
            EloBuddy.Native.EventHandler<22,void __cdecl(EloBuddy::Native::Obj_AI_Base *,bool,bool,bool,bool,int,int),EloBuddy::Native::Obj_AI_Base *,bool,bool,bool,bool,int,int>.Add(EloBuddy.Native.EventHandler<22,void __cdecl(EloBuddy::Native::Obj_AI_Base *,bool,bool,bool,bool,int,int),EloBuddy::Native::Obj_AI_Base *,bool,bool,bool,bool,int,int>.GetInstance(), m_SpellbookStopCastNative.ToPointer());
            SpellbookUpdateChargeableSpellHandlers = new List<SpellbookUpdateChargeableSpell>();
            m_SpellbookUpdateChargeableSpellNativeDelegate = new OnSpellbookUpdateChargeableSpellNativeDelegate(EloBuddy.Spellbook.OnSpellbookUpdateChargeableSpellNative);
            m_SpellbookUpdateChargeableSpellNative = Marshal.GetFunctionPointerForDelegate(m_SpellbookUpdateChargeableSpellNativeDelegate);
            EloBuddy.Native.EventHandler<23,bool __cdecl(EloBuddy::Native::Spellbook *,int,EloBuddy::Native::Vector3f *,bool),EloBuddy::Native::Spellbook *,int,EloBuddy::Native::Vector3f *,bool>.Add(EloBuddy.Native.EventHandler<23,bool __cdecl(EloBuddy::Native::Spellbook *,int,EloBuddy::Native::Vector3f *,bool),EloBuddy::Native::Spellbook *,int,EloBuddy::Native::Vector3f *,bool>.GetInstance(), m_SpellbookUpdateChargeableSpellNative.ToPointer());
            SpellbookPostCastSpellHandlers = new List<SpellbookPostCastSpell>();
        }

        public unsafe Spellbook(EloBuddy.Native.GameObject* @object)
        {
            this.self = (EloBuddy.Native.Obj_AI_Base*) @object;
            this.m_spellbook = this.GetSpellbook();
            uint* numPtr = EloBuddy.Native.GameObject.GetNetworkId(@object);
            this.m_networkId = numPtr[0];
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool CanSpellBeUpgraded(EloBuddy.SpellSlot slot)
        {
            EloBuddy.Native.Spellbook* spellbook = this.GetSpellbook();
            return ((spellbook != null) && EloBuddy.Native.Spellbook.SpellSlotCanBeUpgraded((EloBuddy.Native.Spellbook modopt(IsConst)* modopt(IsConst) modopt(IsConst)) spellbook, (EloBuddy.Native.SpellSlot) slot));
        }

        public unsafe EloBuddy.SpellState CanUseSpell(EloBuddy.SpellSlot slot)
        {
            EloBuddy.Native.Spellbook* spellbook = this.GetSpellbook();
            if ((spellbook != null) && (slot < ((EloBuddy.SpellSlot) 0x40)))
            {
                return EloBuddy.Native.Spellbook.CanUseSpell(spellbook, (EloBuddy.Native.SpellSlot) slot);
            }
            return EloBuddy.SpellState.Unknown;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public bool CastSpell(EloBuddy.SpellSlot slot) => 
            this.CastSpell(slot, true);

        [return: MarshalAs(UnmanagedType.U1)]
        public bool CastSpell(EloBuddy.SpellSlot slot, EloBuddy.GameObject target)
        {
            byte num;
            if ((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40)))
            {
                num = EloBuddy.Native.Spellbook.CastSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, target.GetPtr(), true, false);
            }
            else
            {
                num = 0;
            }
            return (bool) num;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public bool CastSpell(EloBuddy.SpellSlot slot, Vector3 position) => 
            this.CastSpell(slot, position, true);

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool CastSpell(EloBuddy.SpellSlot slot, [MarshalAs(UnmanagedType.U1)] bool triggerEvent)
        {
            Vector3f vectorf;
            return (((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40))) && EloBuddy.Native.Spellbook.CastSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, *(EloBuddy.Native.GameObject.GetPosition((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) this.self, &vectorf)), triggerEvent, false));
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public bool CastSpell(EloBuddy.SpellSlot slot, EloBuddy.GameObject target, [MarshalAs(UnmanagedType.U1)] bool triggerEvent) => 
            (((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40))) && EloBuddy.Native.Spellbook.CastSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, target.GetPtr(), triggerEvent, false));

        [return: MarshalAs(UnmanagedType.U1)]
        public bool CastSpell(EloBuddy.SpellSlot slot, Vector3 startPosition, Vector3 endPosition) => 
            this.CastSpell(slot, startPosition, endPosition, true);

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool CastSpell(EloBuddy.SpellSlot slot, Vector3 position, [MarshalAs(UnmanagedType.U1)] bool triggerEvent)
        {
            Vector3f vectorf;
            return (((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40))) && EloBuddy.Native.Spellbook.CastSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, *(EloBuddy.Native.Vector3f.{ctor}(&vectorf, position.X, position.Y, position.Z)), triggerEvent, false));
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool CastSpell(EloBuddy.SpellSlot slot, [MarshalAs(UnmanagedType.U1)] bool triggerEvent, [MarshalAs(UnmanagedType.U1)] bool forceCast)
        {
            Vector3f vectorf;
            return (((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40))) && EloBuddy.Native.Spellbook.CastSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, *(EloBuddy.Native.GameObject.GetPosition((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) this.self, &vectorf)), triggerEvent, forceCast));
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public bool CastSpell(EloBuddy.SpellSlot slot, EloBuddy.GameObject target, [MarshalAs(UnmanagedType.U1)] bool triggerEvent, [MarshalAs(UnmanagedType.U1)] bool forceCast) => 
            (((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40))) && EloBuddy.Native.Spellbook.CastSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, target.GetPtr(), triggerEvent, forceCast));

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool CastSpell(EloBuddy.SpellSlot slot, Vector3 startPosition, Vector3 endPosition, [MarshalAs(UnmanagedType.U1)] bool triggerEvent)
        {
            if ((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40)))
            {
                Vector3f vectorf;
                Vector3f vectorf2;
                EloBuddy.Native.Vector3f.{ctor}(&vectorf2, startPosition.X, startPosition.Y, startPosition.Z);
                EloBuddy.Native.Vector3f.{ctor}(&vectorf, endPosition.X, endPosition.Y, endPosition.Z);
                return EloBuddy.Native.Spellbook.CastSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, vectorf2, vectorf, triggerEvent, false);
            }
            return false;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool CastSpell(EloBuddy.SpellSlot slot, Vector3 position, [MarshalAs(UnmanagedType.U1)] bool triggerEvent, [MarshalAs(UnmanagedType.U1)] bool forceCast)
        {
            Vector3f vectorf;
            return (((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40))) && EloBuddy.Native.Spellbook.CastSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, *(EloBuddy.Native.Vector3f.{ctor}(&vectorf, position.X, position.Y, position.Z)), triggerEvent, forceCast));
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool CastSpell(EloBuddy.SpellSlot slot, Vector3 startPosition, Vector3 endPosition, [MarshalAs(UnmanagedType.U1)] bool triggerEvent, [MarshalAs(UnmanagedType.U1)] bool forceCast)
        {
            if ((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40)))
            {
                Vector3f vectorf;
                Vector3f vectorf2;
                EloBuddy.Native.Vector3f.{ctor}(&vectorf2, startPosition.X, startPosition.Y, startPosition.Z);
                EloBuddy.Native.Vector3f.{ctor}(&vectorf, endPosition.X, endPosition.Y, endPosition.Z);
                return EloBuddy.Native.Spellbook.CastSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, vectorf2, vectorf, triggerEvent, forceCast);
            }
            return false;
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<21,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Spellbook *,EloBuddy::Native::Vector3f *,EloBuddy::Native::Vector3f *,unsigned int,int),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Spellbook *,EloBuddy::Native::Vector3f *,EloBuddy::Native::Vector3f *,unsigned int,int>.Remove(EloBuddy.Native.EventHandler<21,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Spellbook *,EloBuddy::Native::Vector3f *,EloBuddy::Native::Vector3f *,unsigned int,int),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Spellbook *,EloBuddy::Native::Vector3f *,EloBuddy::Native::Vector3f *,unsigned int,int>.GetInstance(), m_SpellbookCastSpellNative.ToPointer());
            EloBuddy.Native.EventHandler<22,void __cdecl(EloBuddy::Native::Obj_AI_Base *,bool,bool,bool,bool,int,int),EloBuddy::Native::Obj_AI_Base *,bool,bool,bool,bool,int,int>.Remove(EloBuddy.Native.EventHandler<22,void __cdecl(EloBuddy::Native::Obj_AI_Base *,bool,bool,bool,bool,int,int),EloBuddy::Native::Obj_AI_Base *,bool,bool,bool,bool,int,int>.GetInstance(), m_SpellbookStopCastNative.ToPointer());
            EloBuddy.Native.EventHandler<23,bool __cdecl(EloBuddy::Native::Spellbook *,int,EloBuddy::Native::Vector3f *,bool),EloBuddy::Native::Spellbook *,int,EloBuddy::Native::Vector3f *,bool>.Remove(EloBuddy.Native.EventHandler<23,bool __cdecl(EloBuddy::Native::Spellbook *,int,EloBuddy::Native::Vector3f *,bool),EloBuddy::Native::Spellbook *,int,EloBuddy::Native::Vector3f *,bool>.GetInstance(), m_SpellbookUpdateChargeableSpellNative.ToPointer());
        }

        public void EvolveSpell(EloBuddy.SpellSlot slot)
        {
            if ((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40)))
            {
                EloBuddy.Native.Spellbook.EvolveSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot);
            }
        }

        public unsafe EloBuddy.SpellDataInst GetSpell(EloBuddy.SpellSlot slot)
        {
            if ((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40)))
            {
                EloBuddy.Native.SpellDataInst* instPtr = EloBuddy.Native.Spellbook.GetSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot);
                if (instPtr != null)
                {
                    if (EloBuddy.Native.SpellDataInst.GetSData(instPtr) == null)
                    {
                        return null;
                    }
                    return new EloBuddy.SpellDataInst(EloBuddy.Native.Spellbook.GetSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot), slot, this.GetSpellbook());
                }
            }
            return null;
        }

        public unsafe EloBuddy.Native.Spellbook* GetSpellbook()
        {
            if (this.m_spellbook == null)
            {
                this.m_spellbook = EloBuddy.Native.Obj_AI_Base.GetSpellbook(this.self);
            }
            EloBuddy.Native.Spellbook* spellbook = this.m_spellbook;
            if (spellbook == null)
            {
                throw new SpellbookNotFoundException();
            }
            return spellbook;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public bool LevelSpell(EloBuddy.SpellSlot slot) => 
            (((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40))) && EloBuddy.Native.Spellbook.LevelSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot));

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnSpellbookCastSpellNative(EloBuddy.Native.Obj_AI_Base* A_0, EloBuddy.Native.Spellbook* A_1, Vector3f* A_2, Vector3f* A_3, uint A_4, int A_5)
        {
            Exception innerException = null;
            SpellbookCastSpell[] spellArray2 = null;
            SpellbookCastSpell spell2 = null;
            EloBuddy.GameObject target = null;
            bool flag = true;
            try
            {
                Vector3 startPos = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_2), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_2), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_2));
                Vector3 endPos = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_3), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_3), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_3));
                EloBuddy.Native.GameObject* objPtr = EloBuddy.Native.ObjectManager.GetUnitByNetworkId(A_4);
                if (objPtr != null)
                {
                    target = ObjectManager.CreateObjectFromPointer(objPtr);
                }
                EloBuddy.Spellbook sender = new EloBuddy.Spellbook((EloBuddy.Native.GameObject*) A_0);
                SpellbookCastSpellEventArgs args = new SpellbookCastSpellEventArgs(startPos, endPos, target, (EloBuddy.SpellSlot) A_5);
                spellArray2 = SpellbookCastSpellHandlers.ToArray();
                int index = 0;
                while (true)
                {
                    if (index >= spellArray2.Length)
                    {
                        break;
                    }
                    spell2 = spellArray2[index];
                    try
                    {
                        spell2(sender, args);
                        flag = args.Process && flag;
                    }
                    catch (Exception exception4)
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("========================================");
                        System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                        System.Console.WriteLine();
                        System.Console.WriteLine("Type: {0}", exception4.GetType().FullName);
                        System.Console.WriteLine("Message: {0}", exception4.Message);
                        System.Console.WriteLine();
                        System.Console.WriteLine("Stracktrace:");
                        System.Console.WriteLine(exception4.StackTrace);
                        innerException = exception4.InnerException;
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
                    index++;
                }
                if (args.Process)
                {
                    foreach (SpellbookPostCastSpell spell in SpellbookPostCastSpellHandlers.ToArray())
                    {
                        spell(sender, args);
                    }
                    return flag;
                }
            }
            catch (Exception exception3)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("========================================");
                System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                System.Console.WriteLine();
                System.Console.WriteLine("Type: {0}", exception3.GetType().FullName);
                System.Console.WriteLine("Message: {0}", exception3.Message);
                System.Console.WriteLine();
                System.Console.WriteLine("Stracktrace:");
                System.Console.WriteLine(exception3.StackTrace);
                Exception exception2 = exception3.InnerException;
                if (exception2 != null)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("InnerException(s):");
                    do
                    {
                        System.Console.WriteLine("----------------------------------------");
                        System.Console.WriteLine("Type: {0}", exception2.GetType().FullName);
                        System.Console.WriteLine("Message: {0}", exception2.Message);
                        System.Console.WriteLine();
                        System.Console.WriteLine("Stracktrace:");
                        System.Console.WriteLine(exception2.StackTrace);
                        exception2 = exception2.InnerException;
                    }
                    while (exception2 != null);
                    System.Console.WriteLine("----------------------------------------");
                }
                System.Console.WriteLine("========================================");
                System.Console.WriteLine();
            }
            return flag;
        }

        internal static void OnSpellbookPostCastSpellNative()
        {
        }

        internal static unsafe void OnSpellbookStopCastNative(EloBuddy.Native.Obj_AI_Base* A_0, [MarshalAs(UnmanagedType.U1)] bool A_1, [MarshalAs(UnmanagedType.U1)] bool A_2, [MarshalAs(UnmanagedType.U1)] bool A_3, [MarshalAs(UnmanagedType.U1)] bool A_4, int A_5, int A_6)
        {
            Exception innerException = null;
            EloBuddy.Obj_AI_Base sender = null;
            try
            {
                A_0 = *((EloBuddy.Native.Obj_AI_Base**) (A_0 + 0x694));
                if (A_0 != null)
                {
                    sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                }
                SpellbookStopCastEventArgs args = new SpellbookStopCastEventArgs(A_1, A_2, A_3, A_4, (uint) A_5, A_6);
                foreach (SpellbookStopCast cast in SpellbookStopCastHandlers.ToArray())
                {
                    try
                    {
                        cast(sender, args);
                    }
                    catch (Exception exception4)
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("========================================");
                        System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                        System.Console.WriteLine();
                        System.Console.WriteLine("Type: {0}", exception4.GetType().FullName);
                        System.Console.WriteLine("Message: {0}", exception4.Message);
                        System.Console.WriteLine();
                        System.Console.WriteLine("Stracktrace:");
                        System.Console.WriteLine(exception4.StackTrace);
                        innerException = exception4.InnerException;
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
                }
            }
            catch (Exception exception3)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("========================================");
                System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                System.Console.WriteLine();
                System.Console.WriteLine("Type: {0}", exception3.GetType().FullName);
                System.Console.WriteLine("Message: {0}", exception3.Message);
                System.Console.WriteLine();
                System.Console.WriteLine("Stracktrace:");
                System.Console.WriteLine(exception3.StackTrace);
                Exception exception2 = exception3.InnerException;
                if (exception2 != null)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("InnerException(s):");
                    do
                    {
                        System.Console.WriteLine("----------------------------------------");
                        System.Console.WriteLine("Type: {0}", exception2.GetType().FullName);
                        System.Console.WriteLine("Message: {0}", exception2.Message);
                        System.Console.WriteLine();
                        System.Console.WriteLine("Stracktrace:");
                        System.Console.WriteLine(exception2.StackTrace);
                        exception2 = exception2.InnerException;
                    }
                    while (exception2 != null);
                    System.Console.WriteLine("----------------------------------------");
                }
                System.Console.WriteLine("========================================");
                System.Console.WriteLine();
            }
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnSpellbookUpdateChargeableSpellNative(EloBuddy.Native.Spellbook* A_0, int A_1, Vector3f* A_2, [MarshalAs(UnmanagedType.U1)] bool A_3)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                EloBuddy.Spellbook sender = new EloBuddy.Spellbook(EloBuddy.Native.Spellbook.GetOwner(A_0));
                Vector3 position = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_2), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_2), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_2));
                SpellbookUpdateChargeableSpellEventArgs args = new SpellbookUpdateChargeableSpellEventArgs((EloBuddy.SpellSlot) A_1, position, A_3);
                foreach (SpellbookUpdateChargeableSpell spell in SpellbookUpdateChargeableSpellHandlers.ToArray())
                {
                    try
                    {
                        spell(sender, args);
                        flag = args.Process && flag;
                    }
                    catch (Exception exception4)
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine("========================================");
                        System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                        System.Console.WriteLine();
                        System.Console.WriteLine("Type: {0}", exception4.GetType().FullName);
                        System.Console.WriteLine("Message: {0}", exception4.Message);
                        System.Console.WriteLine();
                        System.Console.WriteLine("Stracktrace:");
                        System.Console.WriteLine(exception4.StackTrace);
                        innerException = exception4.InnerException;
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
                }
                return flag;
            }
            catch (Exception exception3)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("========================================");
                System.Console.WriteLine("Exception occured! EloBuddy might crash!");
                System.Console.WriteLine();
                System.Console.WriteLine("Type: {0}", exception3.GetType().FullName);
                System.Console.WriteLine("Message: {0}", exception3.Message);
                System.Console.WriteLine();
                System.Console.WriteLine("Stracktrace:");
                System.Console.WriteLine(exception3.StackTrace);
                Exception exception2 = exception3.InnerException;
                if (exception2 != null)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("InnerException(s):");
                    do
                    {
                        System.Console.WriteLine("----------------------------------------");
                        System.Console.WriteLine("Type: {0}", exception2.GetType().FullName);
                        System.Console.WriteLine("Message: {0}", exception2.Message);
                        System.Console.WriteLine();
                        System.Console.WriteLine("Stracktrace:");
                        System.Console.WriteLine(exception2.StackTrace);
                        exception2 = exception2.InnerException;
                    }
                    while (exception2 != null);
                    System.Console.WriteLine("----------------------------------------");
                }
                System.Console.WriteLine("========================================");
                System.Console.WriteLine();
            }
            return flag;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool UpdateChargeableSpell(EloBuddy.SpellSlot slot, Vector3 position, [MarshalAs(UnmanagedType.U1)] bool releaseCast)
        {
            Vector3f vectorf;
            return (((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40))) && EloBuddy.Native.Spellbook.UpdateChargeableSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, *(EloBuddy.Native.Vector3f.{ctor}(&vectorf, position.X, position.Z, position.Y)), releaseCast, false));
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool UpdateChargeableSpell(EloBuddy.SpellSlot slot, Vector3 position, [MarshalAs(UnmanagedType.U1)] bool releaseCast, [MarshalAs(UnmanagedType.U1)] bool triggerEvent)
        {
            Vector3f vectorf;
            return (((this.GetSpellbook() != null) && (slot < ((EloBuddy.SpellSlot) 0x40))) && EloBuddy.Native.Spellbook.UpdateChargeableSpell(this.GetSpellbook(), (EloBuddy.Native.SpellSlot) slot, *(EloBuddy.Native.Vector3f.{ctor}(&vectorf, position.X, position.Z, position.Y)), releaseCast, triggerEvent));
        }

        public EloBuddy.SpellSlot ActiveSpellSlot
        {
            get
            {
                if (this.GetSpellbook() == null)
                {
                    return EloBuddy.SpellSlot.Unknown;
                }
                return *(EloBuddy.Native.Spellbook.GetActiveSpellSlot(this.GetSpellbook()));
            }
        }

        public float CastEndTime
        {
            get
            {
                EloBuddy.Native.Spellbook* spellbook = this.GetSpellbook();
                if (spellbook != null)
                {
                    SpellCaster_Client* clientPtr = *(EloBuddy.Native.Spellbook.GetSpellCaster(spellbook));
                    if (clientPtr != null)
                    {
                        return **(((int*) clientPtr))[0x38](clientPtr);
                    }
                }
                return 0f;
            }
        }

        public float CastTime
        {
            get
            {
                EloBuddy.Native.Spellbook* spellbook = this.GetSpellbook();
                if (spellbook != null)
                {
                    SpellCaster_Client* clientPtr = *(EloBuddy.Native.Spellbook.GetSpellCaster(spellbook));
                    if (clientPtr != null)
                    {
                        return **(((int*) clientPtr))[0x38](clientPtr);
                    }
                }
                return 0f;
            }
        }

        public bool HasSpellCaster
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.Spellbook* spellbook = this.GetSpellbook();
                return ((spellbook != null) && ((*(EloBuddy.Native.Spellbook.GetSpellCaster(spellbook)) != 0) ? ((bool) ((byte) 1)) : ((bool) ((byte) 0))));
            }
        }

        public bool IsAutoAttacking
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.Spellbook* spellbook = this.GetSpellbook();
                if (spellbook != null)
                {
                    SpellCaster_Client* clientPtr = *(EloBuddy.Native.Spellbook.GetSpellCaster(spellbook));
                    if (clientPtr != null)
                    {
                        return (bool) **(((int*) clientPtr))[0x24](clientPtr);
                    }
                }
                return false;
            }
        }

        public bool IsCastingSpell =>
            ((this.CastEndTime == 0f) ? ((bool) ((byte) 0)) : ((bool) ((byte) 1)));

        public bool IsChanneling
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.Spellbook* spellbook = this.GetSpellbook();
                if (spellbook != null)
                {
                    SpellCaster_Client* clientPtr = *(EloBuddy.Native.Spellbook.GetSpellCaster(spellbook));
                    if (clientPtr != null)
                    {
                        return (bool) **(((int*) clientPtr))[0x2c](clientPtr);
                    }
                }
                return false;
            }
        }

        public bool IsCharging
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.Spellbook* spellbook = this.GetSpellbook();
                if (spellbook != null)
                {
                    SpellCaster_Client* clientPtr = *(EloBuddy.Native.Spellbook.GetSpellCaster(spellbook));
                    if (clientPtr != null)
                    {
                        return (bool) **(((int*) clientPtr))[40](clientPtr);
                    }
                }
                return false;
            }
        }

        public bool IsStopped
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.Spellbook* spellbook = this.GetSpellbook();
                if (spellbook != null)
                {
                    SpellCaster_Client* clientPtr = *(EloBuddy.Native.Spellbook.GetSpellCaster(spellbook));
                    if (clientPtr != null)
                    {
                        return (bool) **(((int*) clientPtr))[0x40](clientPtr);
                    }
                }
                return false;
            }
        }

        public EloBuddy.Obj_AI_Base Owner =>
            ((EloBuddy.Obj_AI_Base) ObjectManager.Player);

        public EloBuddy.SpellSlot SelectedSpellSlot
        {
            get
            {
                if (this.GetSpellbook() == null)
                {
                    return EloBuddy.SpellSlot.Unknown;
                }
                return *(EloBuddy.Native.Spellbook.GetSelectedSpellSlot(this.GetSpellbook()));
            }
        }

        public List<EloBuddy.SpellDataInst> Spells
        {
            get
            {
                List<EloBuddy.SpellDataInst> list = new List<EloBuddy.SpellDataInst>();
                int num = 0;
                do
                {
                    uint num2 = (uint) (num >> 2);
                    EloBuddy.Native.SpellDataInst* spellDataInst = *((EloBuddy.Native.SpellDataInst**) ((num2 * 4) + EloBuddy.Native.Spellbook.GetSpells(this.GetSpellbook())));
                    if (spellDataInst != null)
                    {
                        list.Add(new EloBuddy.SpellDataInst(spellDataInst, (EloBuddy.SpellSlot) num2, this.GetSpellbook()));
                    }
                    num += 4;
                }
                while (num < 0x40);
                return list;
            }
        }

        public bool SpellWasCast
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.Spellbook* spellbook = this.GetSpellbook();
                if (spellbook != null)
                {
                    SpellCaster_Client* clientPtr = *(EloBuddy.Native.Spellbook.GetSpellCaster(spellbook));
                    if (clientPtr != null)
                    {
                        return (bool) **(((int*) clientPtr))[0x20](clientPtr);
                    }
                }
                return false;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnSpellbookCastSpellNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, EloBuddy.Native.Spellbook* A_1, Vector3f* A_2, Vector3f* A_3, uint A_4, int A_5);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnSpellbookPostCastSpellNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnSpellbookStopCastNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, [MarshalAs(UnmanagedType.U1)] bool A_1, [MarshalAs(UnmanagedType.U1)] bool A_2, [MarshalAs(UnmanagedType.U1)] bool A_3, [MarshalAs(UnmanagedType.U1)] bool A_4, int A_5, int A_6);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnSpellbookUpdateChargeableSpellNativeDelegate(EloBuddy.Native.Spellbook* A_0, int A_1, Vector3f* A_2, [MarshalAs(UnmanagedType.U1)] bool A_3);
    }
}

