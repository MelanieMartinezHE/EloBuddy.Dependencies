namespace EloBuddy
{
    using EloBuddy.Native;
    using std;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class AIHeroClient : EloBuddy.Obj_AI_Base
    {
        internal static List<AIHeroApplyCooldown> AIHeroApplyCooldownHandlers;
        internal static List<AIHeroClientDeath> AIHeroClientDeathHandlers;
        internal static List<AIHeroClientSpawn> AIHeroClientSpawnHandlers;
        internal static IntPtr m_AIHeroApplyCooldownNative = new IntPtr();
        internal static OnAIHeroApplyCooldownNativeDelegate m_AIHeroApplyCooldownNativeDelegate;
        internal static IntPtr m_AIHeroClientDeathNative = new IntPtr();
        internal static OnAIHeroClientDeathNativeDelegate m_AIHeroClientDeathNativeDelegate;
        internal static IntPtr m_AIHeroClientSpawnNative = new IntPtr();
        internal static OnAIHeroClientSpawnNativeDelegate m_AIHeroClientSpawnNativeDelegate;

        public static  event AIHeroClientDeath OnDeath
        {
            add
            {
                AIHeroClientDeathHandlers.Add(handler);
            }
            remove
            {
                AIHeroClientDeathHandlers.Remove(handler);
            }
        }

        public static  event AIHeroClientSpawn OnSpawn
        {
            add
            {
                AIHeroClientSpawnHandlers.Add(handler);
            }
            remove
            {
                AIHeroClientSpawnHandlers.Remove(handler);
            }
        }

        static AIHeroClient()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(EloBuddy.AIHeroClient.DomainUnloadEventHandler);
            AIHeroClientDeathHandlers = new List<AIHeroClientDeath>();
            m_AIHeroClientDeathNativeDelegate = new OnAIHeroClientDeathNativeDelegate(EloBuddy.AIHeroClient.OnAIHeroClientDeathNative);
            m_AIHeroClientDeathNative = Marshal.GetFunctionPointerForDelegate(m_AIHeroClientDeathNativeDelegate);
            EloBuddy.Native.EventHandler<46,void __cdecl(EloBuddy::Native::Obj_AI_Base *,float),EloBuddy::Native::Obj_AI_Base *,float>.Add(EloBuddy.Native.EventHandler<46,void __cdecl(EloBuddy::Native::Obj_AI_Base *,float),EloBuddy::Native::Obj_AI_Base *,float>.GetInstance(), m_AIHeroClientDeathNative.ToPointer());
            AIHeroClientSpawnHandlers = new List<AIHeroClientSpawn>();
            m_AIHeroClientSpawnNativeDelegate = new OnAIHeroClientSpawnNativeDelegate(EloBuddy.AIHeroClient.OnAIHeroClientSpawnNative);
            m_AIHeroClientSpawnNative = Marshal.GetFunctionPointerForDelegate(m_AIHeroClientSpawnNativeDelegate);
            EloBuddy.Native.EventHandler<47,void __cdecl(EloBuddy::Native::AIHeroClient *),EloBuddy::Native::AIHeroClient *>.Add(EloBuddy.Native.EventHandler<47,void __cdecl(EloBuddy::Native::AIHeroClient *),EloBuddy::Native::AIHeroClient *>.GetInstance(), m_AIHeroClientSpawnNative.ToPointer());
        }

        public AIHeroClient()
        {
        }

        public unsafe AIHeroClient(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<46,void __cdecl(EloBuddy::Native::Obj_AI_Base *,float),EloBuddy::Native::Obj_AI_Base *,float>.Remove(EloBuddy.Native.EventHandler<46,void __cdecl(EloBuddy::Native::Obj_AI_Base *,float),EloBuddy::Native::Obj_AI_Base *,float>.GetInstance(), m_AIHeroClientDeathNative.ToPointer());
            EloBuddy.Native.EventHandler<47,void __cdecl(EloBuddy::Native::AIHeroClient *),EloBuddy::Native::AIHeroClient *>.Remove(EloBuddy.Native.EventHandler<47,void __cdecl(EloBuddy::Native::AIHeroClient *),EloBuddy::Native::AIHeroClient *>.GetInstance(), m_AIHeroClientSpawnNative.ToPointer());
        }

        internal unsafe EloBuddy.Native.AIHeroClient* GetPtr() => 
            ((EloBuddy.Native.AIHeroClient*) base.GetPtr());

        internal static unsafe void OnAIHeroApplyCooldownNative(EloBuddy.Native.AIHeroClient* A_0, EloBuddy.Native.SpellDataInst* A_1, uint A_2)
        {
        }

        internal static unsafe void OnAIHeroClientDeathNative(EloBuddy.Native.Obj_AI_Base* A_0, float A_1)
        {
            Exception innerException = null;
            try
            {
                if (A_0 != null)
                {
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    OnHeroDeathEventArgs args = new OnHeroDeathEventArgs(sender, A_1);
                    foreach (AIHeroClientDeath death in AIHeroClientDeathHandlers.ToArray())
                    {
                        try
                        {
                            death(sender, args);
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

        internal static unsafe void OnAIHeroClientSpawnNative(EloBuddy.Native.AIHeroClient* A_0)
        {
            Exception innerException = null;
            try
            {
                if (A_0 != null)
                {
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    foreach (AIHeroClientSpawn spawn in AIHeroClientSpawnHandlers.ToArray())
                    {
                        try
                        {
                            spawn(sender);
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

        public int Assists
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-0);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int BarracksKilled
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-1);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public string ChampionName
        {
            get
            {
                if ((base.GetPtr() != null) && (EloBuddy.Native.AIHeroClient.GetChampionName((EloBuddy.Native.AIHeroClient* modopt(IsConst) modopt(IsConst)) base.GetPtr()) != null))
                {
                    basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsConst) modopt(IsConst) localPtr = EloBuddy.Native.AIHeroClient.GetChampionName((EloBuddy.Native.AIHeroClient* modopt(IsConst) modopt(IsConst)) base.GetPtr());
                    return new string((0x10 > localPtr[20]) ? ((sbyte*) localPtr) : ((sbyte*) localPtr[0]));
                }
                return "Unknown";
            }
        }

        public int ChampionsKilled
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-2);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int CombatPlayerScore
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-3);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int Deaths
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-4);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int DoubleKills
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-5);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public EloBuddy.Experience Experience =>
            new EloBuddy.Experience((EloBuddy.Native.AIHeroClient*) base.GetPtr());

        public float Gold
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetGold((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) ptr));
                }
                return 0f;
            }
        }

        public float GoldTotal
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetGoldTotal((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) ptr));
                }
                return 0f;
            }
        }

        public Champion Hero
        {
            get
            {
                try
                {
                    return (Champion) Enum.Parse(typeof(Champion), this.ChampionName);
                }
                catch (Exception exception)
                {
                    System.Console.WriteLine("[EB-Core] Exception at Champion::Hero. Hero not found: {0}, please report to finndev", this.ChampionName);
                    System.Console.WriteLine("[EB-Core] Exception: {0}", exception.Message);
                }
                return Champion.Unknown;
            }
        }

        public int HQKilled
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-6);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public float LargestCriticalStrike
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-7);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[40](statsPtr);
                        }
                    }
                }
                return -1f;
            }
        }

        public int LargestKillingSpree
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-8);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[40](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int Level =>
            new EloBuddy.Experience((EloBuddy.Native.AIHeroClient*) base.GetPtr()).Level;

        public float LongestTimeSpentLiving
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-9);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[40](statsPtr);
                        }
                    }
                }
                return -1f;
            }
        }

        public float MagicDamageDealtPlayer
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-10);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[40](statsPtr);
                        }
                    }
                }
                return -1f;
            }
        }

        public float MagicDamageTaken
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-11);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[40](statsPtr);
                        }
                    }
                }
                return -1f;
            }
        }

        public Mastery[] Masteries =>
            new List<Mastery>().ToArray();

        public int MinionsKilled
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-12);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int NeutralMinionsKilled
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.AIHeroClient.GetNeutralMinionsKilled(ptr));
                }
                return 0;
            }
        }

        public int NodesCaptured
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-13);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int NodesNeutralized
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-14);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public float ObjectivePlayerScore
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-15);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[40](statsPtr);
                        }
                    }
                }
                return -1f;
            }
        }

        public int PentaKills
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-16);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public float PhysicalDamageDealtPlayer
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-17);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[40](statsPtr);
                        }
                    }
                }
                return -1f;
            }
        }

        public float PhysicalDamageTaken
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-18);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[40](statsPtr);
                        }
                    }
                }
                return -1f;
            }
        }

        public int QuadraKills
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-19);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public List<int> Runes =>
            new List<int>();

        public int SpellTrainingPoints =>
            new EloBuddy.Experience((EloBuddy.Native.AIHeroClient*) base.GetPtr()).SpellTrainingPoints;

        public int SuperMonsterKilled
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-20);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public float TotalHeal
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-21);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[40](statsPtr);
                        }
                    }
                }
                return -1f;
            }
        }

        public float TotalTimeCrowdControlDealt
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-22);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[40](statsPtr);
                        }
                    }
                }
                return -1f;
            }
        }

        public int TripleKills
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-23);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int TurretsKilled
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-24);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int UnrealKills
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-25);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int WardsKilled
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-26);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        public int WardsPlaced
        {
            get
            {
                EloBuddy.Native.AIHeroClient* ptr = (EloBuddy.Native.AIHeroClient*) base.GetPtr();
                if (ptr != null)
                {
                    HeroStatsCollection* statssPtr = EloBuddy.Native.AIHeroClient.GetHeroStatsCollection(ptr);
                    if (statssPtr != null)
                    {
                        HeroStats* statsPtr = EloBuddy.Native.HeroStatsCollection.GetHeroStat(statssPtr, &?A0x2bda5be4.unnamed-global-27);
                        if (statsPtr != null)
                        {
                            return **(((int*) statsPtr))[0x2c](statsPtr);
                        }
                    }
                }
                return -1;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnAIHeroApplyCooldownNativeDelegate(EloBuddy.Native.AIHeroClient* A_0, EloBuddy.Native.SpellDataInst* A_1, uint A_2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnAIHeroClientDeathNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, float A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnAIHeroClientSpawnNativeDelegate(EloBuddy.Native.AIHeroClient* A_0);
    }
}

