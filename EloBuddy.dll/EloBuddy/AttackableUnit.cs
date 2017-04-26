namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using std;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class AttackableUnit : EloBuddy.GameObject
    {
        internal static List<AttackableUnitDamage> AttackableUnitDamageHandlers;
        internal static List<AttackableUnitModifyShield> AttackableUnitModifyShieldHandlers;
        internal static IntPtr m_AttackableUnitDamageNative = new IntPtr();
        internal static OnAttackableUnitDamageNativeDelegate m_AttackableUnitDamageNativeDelegate;
        internal static IntPtr m_AttackableUnitModifyShieldNative = new IntPtr();
        internal static OnAttackableUnitModifyShieldNativeDelegate m_AttackableUnitModifyShieldNativeDelegate;
        private unsafe EloBuddy.Native.AttackableUnit* self;

        public static  event AttackableUnitDamage OnDamage
        {
            add
            {
                AttackableUnitDamageHandlers.Add(handler);
            }
            remove
            {
                AttackableUnitDamageHandlers.Remove(handler);
            }
        }

        public static  event AttackableUnitModifyShield OnModifyShield
        {
            add
            {
                AttackableUnitModifyShieldHandlers.Add(handler);
            }
            remove
            {
                AttackableUnitModifyShieldHandlers.Remove(handler);
            }
        }

        static AttackableUnit()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(EloBuddy.AttackableUnit.DomainUnloadEventHandler);
            AttackableUnitModifyShieldHandlers = new List<AttackableUnitModifyShield>();
            m_AttackableUnitModifyShieldNativeDelegate = new OnAttackableUnitModifyShieldNativeDelegate(EloBuddy.AttackableUnit.OnAttackableUnitModifyShieldNative);
            m_AttackableUnitModifyShieldNative = Marshal.GetFunctionPointerForDelegate(m_AttackableUnitModifyShieldNativeDelegate);
            EloBuddy.Native.EventHandler<29,void __cdecl(EloBuddy::Native::AttackableUnit *,float,float),EloBuddy::Native::AttackableUnit *,float,float>.Add(EloBuddy.Native.EventHandler<29,void __cdecl(EloBuddy::Native::AttackableUnit *,float,float),EloBuddy::Native::AttackableUnit *,float,float>.GetInstance(), m_AttackableUnitModifyShieldNative.ToPointer());
            AttackableUnitDamageHandlers = new List<AttackableUnitDamage>();
            m_AttackableUnitDamageNativeDelegate = new OnAttackableUnitDamageNativeDelegate(EloBuddy.AttackableUnit.OnAttackableUnitDamageNative);
            m_AttackableUnitDamageNative = Marshal.GetFunctionPointerForDelegate(m_AttackableUnitDamageNativeDelegate);
            EloBuddy.Native.EventHandler<30,void __cdecl(EloBuddy::Native::AttackableUnit *,EloBuddy::Native::AttackableUnit *,float,EloBuddy::Native::DamageLayout *),EloBuddy::Native::AttackableUnit *,EloBuddy::Native::AttackableUnit *,float,EloBuddy::Native::DamageLayout *>.Add(EloBuddy.Native.EventHandler<30,void __cdecl(EloBuddy::Native::AttackableUnit *,EloBuddy::Native::AttackableUnit *,float,EloBuddy::Native::DamageLayout *),EloBuddy::Native::AttackableUnit *,EloBuddy::Native::AttackableUnit *,float,EloBuddy::Native::DamageLayout *>.GetInstance(), m_AttackableUnitDamageNative.ToPointer());
        }

        public AttackableUnit()
        {
        }

        public unsafe AttackableUnit(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<29,void __cdecl(EloBuddy::Native::AttackableUnit *,float,float),EloBuddy::Native::AttackableUnit *,float,float>.Remove(EloBuddy.Native.EventHandler<29,void __cdecl(EloBuddy::Native::AttackableUnit *,float,float),EloBuddy::Native::AttackableUnit *,float,float>.GetInstance(), m_AttackableUnitModifyShieldNative.ToPointer());
            EloBuddy.Native.EventHandler<30,void __cdecl(EloBuddy::Native::AttackableUnit *,EloBuddy::Native::AttackableUnit *,float,EloBuddy::Native::DamageLayout *),EloBuddy::Native::AttackableUnit *,EloBuddy::Native::AttackableUnit *,float,EloBuddy::Native::DamageLayout *>.Remove(EloBuddy.Native.EventHandler<30,void __cdecl(EloBuddy::Native::AttackableUnit *,EloBuddy::Native::AttackableUnit *,float,EloBuddy::Native::DamageLayout *),EloBuddy::Native::AttackableUnit *,EloBuddy::Native::AttackableUnit *,float,EloBuddy::Native::DamageLayout *>.GetInstance(), m_AttackableUnitDamageNative.ToPointer());
        }

        internal unsafe EloBuddy.Native.AttackableUnit* GetPtr() => 
            ((EloBuddy.Native.AttackableUnit*) base.GetPtr());

        internal static unsafe void OnAttackableUnitDamageNative(EloBuddy.Native.AttackableUnit* A_0, EloBuddy.Native.AttackableUnit* A_1, float A_2, DamageLayout* A_3)
        {
            Exception innerException = null;
            try
            {
                EloBuddy.AttackableUnit source = (EloBuddy.AttackableUnit) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                EloBuddy.AttackableUnit target = (EloBuddy.AttackableUnit) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_1);
                float* numPtr = EloBuddy.Native.DamageLayout.GetTime(A_3);
                AttackableUnitDamageEventArgs args = new AttackableUnitDamageEventArgs(source, target, DamageHitType.Normal, DamageType.Physical, A_2, numPtr[0]);
                foreach (AttackableUnitDamage damage in AttackableUnitDamageHandlers.ToArray())
                {
                    try
                    {
                        damage(source, args);
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

        internal static unsafe void OnAttackableUnitModifyShieldNative(EloBuddy.Native.AttackableUnit* __unnamed000, float attackShield, float magicShield)
        {
            Exception innerException = null;
            try
            {
                EloBuddy.AttackableUnit sender = (EloBuddy.AttackableUnit) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) __unnamed000);
                AttackableUnitModifyShieldEventArgs args = new AttackableUnitModifyShieldEventArgs(attackShield, magicShield);
                foreach (AttackableUnitModifyShield shield in AttackableUnitModifyShieldHandlers.ToArray())
                {
                    try
                    {
                        shield(sender, args);
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

        public float AllShield
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetAllShield(ptr));
                }
                return 0f;
            }
        }

        public string ArmorMaterial
        {
            get
            {
                if ((base.GetPtr() != null) && (EloBuddy.Native.AttackableUnit.GetArmorMaterial((EloBuddy.Native.AttackableUnit* modopt(IsConst) modopt(IsConst)) base.GetPtr()) != null))
                {
                    basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsConst) modopt(IsConst) localPtr = EloBuddy.Native.AttackableUnit.GetArmorMaterial((EloBuddy.Native.AttackableUnit* modopt(IsConst) modopt(IsConst)) base.GetPtr());
                    return new string((0x10 > localPtr[20]) ? ((sbyte*) localPtr) : ((sbyte*) localPtr[0]));
                }
                return "Unknown";
            }
        }

        public float AttackShield
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetAttackShield(ptr));
                }
                return 0f;
            }
        }

        public Vector3 Direction
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    Vector3f* vectorfPtr = EloBuddy.Native.AttackableUnit.GetDirection(ptr);
                    return new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                }
                return Vector3.Zero;
            }
        }

        public int HasBotAI
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetHasBotAI(ptr));
                }
                return 0;
            }
        }

        public float Health
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetHealth(ptr));
                }
                return 0f;
            }
        }

        public float HealthPercent
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return EloBuddy.Native.AttackableUnit.GetHealthPercent(ptr);
                }
                return 0f;
            }
        }

        public bool IsAttackingPlayer
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                return ((ptr != null) && EloBuddy.Native.AttackableUnit.IsAttackingPlayer(ptr));
            }
        }

        public int IsBot
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetIsBot(ptr));
                }
                return 0;
            }
        }

        public bool IsInvulnerable
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetIsInvulnerable(ptr));
                }
                return false;
            }
        }

        public bool IsLifestealImmune
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetIsLifestealImmune(ptr));
                }
                return false;
            }
        }

        public bool IsMelee
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                return ((ptr != null) && EloBuddy.Native.AttackableUnit.IsMelee(ptr));
            }
        }

        public bool IsPhysicalImmune
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetIsPhysicalImmune(ptr));
                }
                return false;
            }
        }

        public bool IsRanged
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                return ((ptr != null) && EloBuddy.Native.AttackableUnit.IsRanged(ptr));
            }
        }

        public bool IsTargetable
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    GameObjectVTable* tablePtr = EloBuddy.Native.GameObject.GetVirtual((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) ptr);
                    if (tablePtr != null)
                    {
                        return (bool) **(((int*) tablePtr))[0x110](tablePtr);
                    }
                }
                return false;
            }
        }

        public bool IsTargetableToTeam
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    GameObjectVTable* tablePtr = EloBuddy.Native.GameObject.GetVirtual((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) ptr);
                    if (tablePtr != null)
                    {
                        return (bool) **(((int*) tablePtr))[0x114](tablePtr);
                    }
                }
                return false;
            }
        }

        public bool IsZombie
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetIsZombie(ptr));
                }
                return false;
            }
        }

        public float KledSkaarlHP
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetKledSkaarlHP(ptr));
                }
                return 0f;
            }
        }

        public bool MagicImmune
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetMagicImmune(ptr));
                }
                return false;
            }
        }

        public float MagicShield
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetMagicShield(ptr));
                }
                return 0f;
            }
        }

        public float Mana
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetMana(ptr));
                }
                return 0f;
            }
        }

        public float ManaPercent
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return EloBuddy.Native.AttackableUnit.GetManaPercent(ptr);
                }
                return 0f;
            }
        }

        public float MaxHealth
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetMaxHealth(ptr));
                }
                return 0f;
            }
        }

        public float MaxKledSkaarlHP
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetMaxKledSkaarlHP(ptr));
                }
                return 0f;
            }
        }

        public float MaxMana
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetMaxMana(ptr));
                }
                return 0f;
            }
        }

        public float OverrideCollisionHeight
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetOverrideCollisionHeight(ptr));
                }
                return 0f;
            }
        }

        public float OverrideCollisionRadius
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetOverrideCollisionRadius(ptr));
                }
                return 0f;
            }
        }

        public float PathfindingCollisionRadius
        {
            get
            {
                EloBuddy.Native.AttackableUnit* ptr = (EloBuddy.Native.AttackableUnit*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AttackableUnit.GetPathfindingCollisionRadius(ptr));
                }
                return 0f;
            }
        }

        public string WeaponMaterial
        {
            get
            {
                if ((base.GetPtr() != null) && (EloBuddy.Native.AttackableUnit.GetWeaponMaterial((EloBuddy.Native.AttackableUnit* modopt(IsConst) modopt(IsConst)) base.GetPtr()) != null))
                {
                    basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsConst) modopt(IsConst) localPtr = EloBuddy.Native.AttackableUnit.GetWeaponMaterial((EloBuddy.Native.AttackableUnit* modopt(IsConst) modopt(IsConst)) base.GetPtr());
                    return new string((0x10 > localPtr[20]) ? ((sbyte*) localPtr) : ((sbyte*) localPtr[0]));
                }
                return "Unknown";
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnAttackableUnitDamageNativeDelegate(EloBuddy.Native.AttackableUnit* A_0, EloBuddy.Native.AttackableUnit* A_1, float A_2, DamageLayout* A_3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnAttackableUnitModifyShieldNativeDelegate(EloBuddy.Native.AttackableUnit* __unnamed000, float attackShield, float magicShield);
    }
}

