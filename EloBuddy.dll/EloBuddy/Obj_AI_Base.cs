namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using std;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Obj_AI_Base : EloBuddy.AttackableUnit
    {
        internal static string[] attacks = new string[] { "caitlynheadshotmissile", "frostarrow", "garenslash2", "kennenmegaproc", "masteryidoublestrike", "quinnwenhanced", "renektonexecute", "renektonsuperexecute", "rengarnewpassivebuffdash", "trundleq", "xenzhaothrust", "xenzhaothrust2", "xenzhaothrust3", "viktorqbuff", "lucianpassiveshot" };
        internal static Dictionary<int, List<EloBuddy.BuffInstance>> cachedBuffs = new Dictionary<int, List<EloBuddy.BuffInstance>>();
        internal static IntPtr m_Obj_AI_BaseBuffGainNative = new IntPtr();
        internal static OnObj_AI_BaseBuffGainNativeDelegate m_Obj_AI_BaseBuffGainNativeDelegate;
        internal static IntPtr m_Obj_AI_BaseBuffLoseNative = new IntPtr();
        internal static OnObj_AI_BaseBuffLoseNativeDelegate m_Obj_AI_BaseBuffLoseNativeDelegate;
        internal static IntPtr m_Obj_AI_BaseBuffUpdateNative = new IntPtr();
        internal static OnObj_AI_BaseBuffUpdateNativeDelegate m_Obj_AI_BaseBuffUpdateNativeDelegate;
        internal static IntPtr m_Obj_AI_BaseDoCastSpellNative = new IntPtr();
        internal static OnObj_AI_BaseDoCastSpellNativeDelegate m_Obj_AI_BaseDoCastSpellNativeDelegate;
        internal static IntPtr m_Obj_AI_BaseLevelUpNative = new IntPtr();
        internal static OnObj_AI_BaseLevelUpNativeDelegate m_Obj_AI_BaseLevelUpNativeDelegate;
        internal static IntPtr m_Obj_AI_BaseNewPathNative = new IntPtr();
        internal static OnObj_AI_BaseNewPathNativeDelegate m_Obj_AI_BaseNewPathNativeDelegate;
        internal static IntPtr m_Obj_AI_BaseOnBasicAttackNative = new IntPtr();
        internal static OnObj_AI_BaseOnBasicAttackNativeDelegate m_Obj_AI_BaseOnBasicAttackNativeDelegate;
        internal static IntPtr m_Obj_AI_BaseOnSurrenderVoteNative = new IntPtr();
        internal static OnObj_AI_BaseOnSurrenderVoteNativeDelegate m_Obj_AI_BaseOnSurrenderVoteNativeDelegate;
        internal static IntPtr m_Obj_AI_BasePlayAnimationNative = new IntPtr();
        internal static OnObj_AI_BasePlayAnimationNativeDelegate m_Obj_AI_BasePlayAnimationNativeDelegate;
        internal static IntPtr m_Obj_AI_BaseTeleportNative = new IntPtr();
        internal static OnObj_AI_BaseTeleportNativeDelegate m_Obj_AI_BaseTeleportNativeDelegate;
        internal static IntPtr m_Obj_AI_ProcessSpellCastNative = new IntPtr();
        internal static OnObj_AI_ProcessSpellCastNativeDelegate m_Obj_AI_ProcessSpellCastNativeDelegate;
        internal static IntPtr m_Obj_AI_UpdateModelNative = new IntPtr();
        internal static OnObj_AI_UpdateModelNativeDelegate m_Obj_AI_UpdateModelNativeDelegate;
        internal static IntPtr m_Obj_AI_UpdatePositionNative = new IntPtr();
        internal static OnObj_AI_UpdatePositionNativeDelegate m_Obj_AI_UpdatePositionNativeDelegate;
        internal static string[] noAttacks = new string[] { 
            "volleyattack", "volleyattackwithsound", "jarvanivcataclysmattack", "monkeykingdoubleattack", "shyvanadoubleattack", "shyvanadoubleattackdragon", "zyragraspingplantattack", "zyragraspingplantattack2", "zyragraspingplantattackfire", "zyragraspingplantattack2fire", "viktorpowertransfer", "sivirwattackbounce", "asheqattacknoonhit", "elisespiderlingbasicattack", "heimertyellowbasicattack", "heimertyellowbasicattack2",
            "heimertbluebasicattack", "annietibbersbasicattack", "annietibbersbasicattack2", "yorickdecayedghoulbasicattack", "yorickravenousghoulbasicattack", "yorickspectralghoulbasicattack", "malzaharvoidlingbasicattack", "malzaharvoidlingbasicattack2", "malzaharvoidlingbasicattack3", "kindredwolfbasicattack"
        };
        internal static List<Obj_AI_BaseBuffGain> Obj_AI_BaseBuffGainHandlers;
        internal static List<Obj_AI_BaseBuffLose> Obj_AI_BaseBuffLoseHandlers;
        internal static List<Obj_AI_BaseBuffUpdate> Obj_AI_BaseBuffUpdateHandlers;
        internal static List<Obj_AI_BaseDoCastSpell> Obj_AI_BaseDoCastSpellHandlers;
        internal static List<Obj_AI_BaseLevelUp> Obj_AI_BaseLevelUpHandlers;
        internal static List<Obj_AI_BaseNewPath> Obj_AI_BaseNewPathHandlers;
        internal static List<Obj_AI_BaseOnBasicAttack> Obj_AI_BaseOnBasicAttackHandlers;
        internal static List<Obj_AI_BaseOnSurrenderVote> Obj_AI_BaseOnSurrenderVoteHandlers;
        internal static List<Obj_AI_BasePlayAnimation> Obj_AI_BasePlayAnimationHandlers;
        internal static List<Obj_AI_BaseTeleport> Obj_AI_BaseTeleportHandlers;
        internal static List<Obj_AI_ProcessSpellCast> Obj_AI_ProcessSpellCastHandlers;
        internal static List<Obj_AI_UpdateModel> Obj_AI_UpdateModelHandlers;
        internal static List<EloBuddy.Obj_AI_UpdatePosition> Obj_AI_UpdatePositionHandlers;

        public static  event Obj_AI_BaseOnBasicAttack OnBasicAttack
        {
            add
            {
                Obj_AI_BaseOnBasicAttackHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_BaseOnBasicAttackHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_BaseBuffGain OnBuffGain
        {
            add
            {
                Obj_AI_BaseBuffGainHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_BaseBuffGainHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_BaseBuffLose OnBuffLose
        {
            add
            {
                Obj_AI_BaseBuffLoseHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_BaseBuffLoseHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_BaseBuffUpdate OnBuffUpdate
        {
            add
            {
                Obj_AI_BaseBuffUpdateHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_BaseBuffUpdateHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_BaseLevelUp OnLevelUp
        {
            add
            {
                Obj_AI_BaseLevelUpHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_BaseLevelUpHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_BaseNewPath OnNewPath
        {
            add
            {
                Obj_AI_BaseNewPathHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_BaseNewPathHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_BasePlayAnimation OnPlayAnimation
        {
            add
            {
                Obj_AI_BasePlayAnimationHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_BasePlayAnimationHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_ProcessSpellCast OnProcessSpellCast
        {
            add
            {
                Obj_AI_ProcessSpellCastHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_ProcessSpellCastHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_BaseDoCastSpell OnSpellCast
        {
            add
            {
                Obj_AI_BaseDoCastSpellHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_BaseDoCastSpellHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_BaseOnSurrenderVote OnSurrender
        {
            add
            {
                Obj_AI_BaseOnSurrenderVoteHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_BaseOnSurrenderVoteHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_BaseTeleport OnTeleport
        {
            add
            {
                Obj_AI_BaseTeleportHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_BaseTeleportHandlers.Remove(handler);
            }
        }

        public static  event Obj_AI_UpdateModel OnUpdateModel
        {
            add
            {
                Obj_AI_UpdateModelHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_UpdateModelHandlers.Remove(handler);
            }
        }

        public static  event EloBuddy.Obj_AI_UpdatePosition OnUpdatePosition
        {
            add
            {
                Obj_AI_UpdatePositionHandlers.Add(handler);
            }
            remove
            {
                Obj_AI_UpdatePositionHandlers.Remove(handler);
            }
        }

        static Obj_AI_Base()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(EloBuddy.Obj_AI_Base.DomainUnloadEventHandler);
            Obj_AI_ProcessSpellCastHandlers = new List<Obj_AI_ProcessSpellCast>();
            m_Obj_AI_ProcessSpellCastNativeDelegate = new OnObj_AI_ProcessSpellCastNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_ProcessSpellCastNative);
            m_Obj_AI_ProcessSpellCastNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_ProcessSpellCastNativeDelegate);
            EloBuddy.Native.EventHandler<14,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.Add(EloBuddy.Native.EventHandler<14,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.GetInstance(), m_Obj_AI_ProcessSpellCastNative.ToPointer());
            Obj_AI_BaseTeleportHandlers = new List<Obj_AI_BaseTeleport>();
            m_Obj_AI_BaseTeleportNativeDelegate = new OnObj_AI_BaseTeleportNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_BaseTeleportNative);
            m_Obj_AI_BaseTeleportNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_BaseTeleportNativeDelegate);
            EloBuddy.Native.EventHandler<16,void __cdecl(EloBuddy::Native::Obj_AI_Base *,char *,char *),EloBuddy::Native::Obj_AI_Base *,char *,char *>.Add(EloBuddy.Native.EventHandler<16,void __cdecl(EloBuddy::Native::Obj_AI_Base *,char *,char *),EloBuddy::Native::Obj_AI_Base *,char *,char *>.GetInstance(), m_Obj_AI_BaseTeleportNative.ToPointer());
            Obj_AI_BaseNewPathHandlers = new List<Obj_AI_BaseNewPath>();
            m_Obj_AI_BaseNewPathNativeDelegate = new OnObj_AI_BaseNewPathNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_BaseNewPathNative);
            m_Obj_AI_BaseNewPathNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_BaseNewPathNativeDelegate);
            EloBuddy.Native.EventHandler<17,void __cdecl(EloBuddy::Native::Obj_AI_Base *,std::vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> > *,bool,float),EloBuddy::Native::Obj_AI_Base *,std::vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> > *,bool,float>.Add(EloBuddy.Native.EventHandler<17,void __cdecl(EloBuddy::Native::Obj_AI_Base *,std::vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> > *,bool,float),EloBuddy::Native::Obj_AI_Base *,std::vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> > *,bool,float>.GetInstance(), m_Obj_AI_BaseNewPathNative.ToPointer());
            Obj_AI_BasePlayAnimationHandlers = new List<Obj_AI_BasePlayAnimation>();
            m_Obj_AI_BasePlayAnimationNativeDelegate = new OnObj_AI_BasePlayAnimationNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_BasePlayAnimationNative);
            m_Obj_AI_BasePlayAnimationNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_BasePlayAnimationNativeDelegate);
            EloBuddy.Native.EventHandler<18,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,int),EloBuddy::Native::Obj_AI_Base *,int>.Add(EloBuddy.Native.EventHandler<18,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,int),EloBuddy::Native::Obj_AI_Base *,int>.GetInstance(), m_Obj_AI_BasePlayAnimationNative.ToPointer());
            Obj_AI_BaseBuffGainHandlers = new List<Obj_AI_BaseBuffGain>();
            m_Obj_AI_BaseBuffGainNativeDelegate = new OnObj_AI_BaseBuffGainNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_BaseBuffGainNative);
            m_Obj_AI_BaseBuffGainNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_BaseBuffGainNativeDelegate);
            EloBuddy.Native.EventHandler<37,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.Add(EloBuddy.Native.EventHandler<37,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.GetInstance(), m_Obj_AI_BaseBuffGainNative.ToPointer());
            Obj_AI_BaseBuffLoseHandlers = new List<Obj_AI_BaseBuffLose>();
            m_Obj_AI_BaseBuffLoseNativeDelegate = new OnObj_AI_BaseBuffLoseNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_BaseBuffLoseNative);
            m_Obj_AI_BaseBuffLoseNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_BaseBuffLoseNativeDelegate);
            EloBuddy.Native.EventHandler<38,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.Add(EloBuddy.Native.EventHandler<38,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.GetInstance(), m_Obj_AI_BaseBuffLoseNative.ToPointer());
            Obj_AI_BaseBuffUpdateHandlers = new List<Obj_AI_BaseBuffUpdate>();
            m_Obj_AI_BaseBuffUpdateNativeDelegate = new OnObj_AI_BaseBuffUpdateNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_BaseBuffUpdateNative);
            m_Obj_AI_BaseBuffUpdateNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_BaseBuffUpdateNativeDelegate);
            EloBuddy.Native.EventHandler<39,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.Add(EloBuddy.Native.EventHandler<39,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.GetInstance(), m_Obj_AI_BaseBuffUpdateNative.ToPointer());
            Obj_AI_BaseLevelUpHandlers = new List<Obj_AI_BaseLevelUp>();
            m_Obj_AI_BaseLevelUpNativeDelegate = new OnObj_AI_BaseLevelUpNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_BaseLevelUpNative);
            m_Obj_AI_BaseLevelUpNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_BaseLevelUpNativeDelegate);
            EloBuddy.Native.EventHandler<44,void __cdecl(EloBuddy::Native::Obj_AI_Base *,int),EloBuddy::Native::Obj_AI_Base *,int>.Add(EloBuddy.Native.EventHandler<44,void __cdecl(EloBuddy::Native::Obj_AI_Base *,int),EloBuddy::Native::Obj_AI_Base *,int>.GetInstance(), m_Obj_AI_BaseLevelUpNative.ToPointer());
            Obj_AI_UpdateModelHandlers = new List<Obj_AI_UpdateModel>();
            m_Obj_AI_UpdateModelNativeDelegate = new OnObj_AI_UpdateModelNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_UpdateModelNative);
            m_Obj_AI_UpdateModelNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_UpdateModelNativeDelegate);
            EloBuddy.Native.EventHandler<51,void __cdecl(EloBuddy::Native::Obj_AI_Base *,char *,int),EloBuddy::Native::Obj_AI_Base *,char *,int>.Add(EloBuddy.Native.EventHandler<51,void __cdecl(EloBuddy::Native::Obj_AI_Base *,char *,int),EloBuddy::Native::Obj_AI_Base *,char *,int>.GetInstance(), m_Obj_AI_UpdateModelNative.ToPointer());
            Obj_AI_UpdatePositionHandlers = new List<EloBuddy.Obj_AI_UpdatePosition>();
            m_Obj_AI_UpdatePositionNativeDelegate = new OnObj_AI_UpdatePositionNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_UpdatePositionNative);
            m_Obj_AI_UpdatePositionNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_UpdatePositionNativeDelegate);
            EloBuddy.Native.EventHandler<70,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Vector3f *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Vector3f *>.Add(EloBuddy.Native.EventHandler<70,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Vector3f *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Vector3f *>.GetInstance(), m_Obj_AI_UpdatePositionNative.ToPointer());
            Obj_AI_BaseDoCastSpellHandlers = new List<Obj_AI_BaseDoCastSpell>();
            m_Obj_AI_BaseDoCastSpellNativeDelegate = new OnObj_AI_BaseDoCastSpellNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_BaseDoCastSpellNative);
            m_Obj_AI_BaseDoCastSpellNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_BaseDoCastSpellNativeDelegate);
            EloBuddy.Native.EventHandler<71,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.Add(EloBuddy.Native.EventHandler<71,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.GetInstance(), m_Obj_AI_BaseDoCastSpellNative.ToPointer());
            Obj_AI_BaseOnBasicAttackHandlers = new List<Obj_AI_BaseOnBasicAttack>();
            m_Obj_AI_BaseOnBasicAttackNativeDelegate = new OnObj_AI_BaseOnBasicAttackNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_BaseOnBasicAttackNative);
            m_Obj_AI_BaseOnBasicAttackNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_BaseOnBasicAttackNativeDelegate);
            EloBuddy.Native.EventHandler<73,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.Add(EloBuddy.Native.EventHandler<73,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.GetInstance(), m_Obj_AI_BaseOnBasicAttackNative.ToPointer());
            Obj_AI_BaseOnSurrenderVoteHandlers = new List<Obj_AI_BaseOnSurrenderVote>();
            m_Obj_AI_BaseOnSurrenderVoteNativeDelegate = new OnObj_AI_BaseOnSurrenderVoteNativeDelegate(EloBuddy.Obj_AI_Base.OnObj_AI_BaseOnSurrenderVoteNative);
            m_Obj_AI_BaseOnSurrenderVoteNative = Marshal.GetFunctionPointerForDelegate(m_Obj_AI_BaseOnSurrenderVoteNativeDelegate);
            EloBuddy.Native.EventHandler<74,void __cdecl(EloBuddy::Native::Obj_AI_Base *,unsigned char),EloBuddy::Native::Obj_AI_Base *,unsigned char>.Add(EloBuddy.Native.EventHandler<74,void __cdecl(EloBuddy::Native::Obj_AI_Base *,unsigned char),EloBuddy::Native::Obj_AI_Base *,unsigned char>.GetInstance(), m_Obj_AI_BaseOnSurrenderVoteNative.ToPointer());
        }

        public Obj_AI_Base()
        {
        }

        public unsafe Obj_AI_Base(short index, uint networkId, EloBuddy.Native.GameObject* unit) : base(index, networkId, unit)
        {
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool Capture()
        {
            if (EloBuddy.Native.ObjectManager.GetPlayer() != null)
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                EloBuddy.Native.Obj_AI_Base.Capture(EloBuddy.Native.ObjectManager.GetPlayer(), ptr);
            }
            return false;
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<14,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.Remove(EloBuddy.Native.EventHandler<14,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.GetInstance(), m_Obj_AI_ProcessSpellCastNative.ToPointer());
            EloBuddy.Native.EventHandler<16,void __cdecl(EloBuddy::Native::Obj_AI_Base *,char *,char *),EloBuddy::Native::Obj_AI_Base *,char *,char *>.Remove(EloBuddy.Native.EventHandler<16,void __cdecl(EloBuddy::Native::Obj_AI_Base *,char *,char *),EloBuddy::Native::Obj_AI_Base *,char *,char *>.GetInstance(), m_Obj_AI_BaseTeleportNative.ToPointer());
            EloBuddy.Native.EventHandler<17,void __cdecl(EloBuddy::Native::Obj_AI_Base *,std::vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> > *,bool,float),EloBuddy::Native::Obj_AI_Base *,std::vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> > *,bool,float>.Remove(EloBuddy.Native.EventHandler<17,void __cdecl(EloBuddy::Native::Obj_AI_Base *,std::vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> > *,bool,float),EloBuddy::Native::Obj_AI_Base *,std::vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> > *,bool,float>.GetInstance(), m_Obj_AI_BaseNewPathNative.ToPointer());
            EloBuddy.Native.EventHandler<18,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,int),EloBuddy::Native::Obj_AI_Base *,int>.Remove(EloBuddy.Native.EventHandler<18,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,int),EloBuddy::Native::Obj_AI_Base *,int>.GetInstance(), m_Obj_AI_BasePlayAnimationNative.ToPointer());
            EloBuddy.Native.EventHandler<37,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.Remove(EloBuddy.Native.EventHandler<37,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.GetInstance(), m_Obj_AI_BaseBuffGainNative.ToPointer());
            EloBuddy.Native.EventHandler<38,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.Remove(EloBuddy.Native.EventHandler<38,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.GetInstance(), m_Obj_AI_BaseBuffLoseNative.ToPointer());
            EloBuddy.Native.EventHandler<39,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.Remove(EloBuddy.Native.EventHandler<39,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::BuffInstance *>.GetInstance(), m_Obj_AI_BaseBuffUpdateNative.ToPointer());
            EloBuddy.Native.EventHandler<44,void __cdecl(EloBuddy::Native::Obj_AI_Base *,int),EloBuddy::Native::Obj_AI_Base *,int>.Remove(EloBuddy.Native.EventHandler<44,void __cdecl(EloBuddy::Native::Obj_AI_Base *,int),EloBuddy::Native::Obj_AI_Base *,int>.GetInstance(), m_Obj_AI_BaseLevelUpNative.ToPointer());
            EloBuddy.Native.EventHandler<51,void __cdecl(EloBuddy::Native::Obj_AI_Base *,char *,int),EloBuddy::Native::Obj_AI_Base *,char *,int>.Remove(EloBuddy.Native.EventHandler<51,void __cdecl(EloBuddy::Native::Obj_AI_Base *,char *,int),EloBuddy::Native::Obj_AI_Base *,char *,int>.GetInstance(), m_Obj_AI_UpdateModelNative.ToPointer());
            EloBuddy.Native.EventHandler<70,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Vector3f *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Vector3f *>.Remove(EloBuddy.Native.EventHandler<70,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Vector3f *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::Vector3f *>.GetInstance(), m_Obj_AI_UpdatePositionNative.ToPointer());
            EloBuddy.Native.EventHandler<71,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.Remove(EloBuddy.Native.EventHandler<71,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.GetInstance(), m_Obj_AI_BaseDoCastSpellNative.ToPointer());
            EloBuddy.Native.EventHandler<73,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.Remove(EloBuddy.Native.EventHandler<73,void __cdecl(EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *),EloBuddy::Native::Obj_AI_Base *,EloBuddy::Native::SpellCastInfo *>.GetInstance(), m_Obj_AI_BaseOnBasicAttackNative.ToPointer());
            EloBuddy.Native.EventHandler<74,void __cdecl(EloBuddy::Native::Obj_AI_Base *,unsigned char),EloBuddy::Native::Obj_AI_Base *,unsigned char>.Remove(EloBuddy.Native.EventHandler<74,void __cdecl(EloBuddy::Native::Obj_AI_Base *,unsigned char),EloBuddy::Native::Obj_AI_Base *,unsigned char>.GetInstance(), m_Obj_AI_BaseOnSurrenderVoteNative.ToPointer());
        }

        public unsafe EloBuddy.BuffInstance GetBuff(string name)
        {
            EloBuddy.Native.BuffInstance* instancePtr;
            IntPtr ptr;
            if (!string.IsNullOrEmpty(name))
            {
                BuffManager* managerPtr = EloBuddy.Native.Obj_AI_Base.GetBuffManager((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr());
                ptr = Marshal.StringToHGlobalAnsi(name);
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
                                instancePtr = *((EloBuddy.Native.BuffInstance**) ((num * 8) + nodePtr));
                                if (((instancePtr != null) && EloBuddy.Native.BuffInstance.IsValid(instancePtr)) && EloBuddy.Native.BuffInstance.IsActive(instancePtr))
                                {
                                    ScriptBaseBuff* buffPtr = EloBuddy.Native.BuffInstance.GetScriptBaseBuff(instancePtr);
                                    if (_strcmpi(EloBuddy.Native.ScriptBaseBuff.GetName(buffPtr), (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) ptr.ToPointer()) == 0)
                                    {
                                        goto Label_00BA;
                                    }
                                    VirtualScriptBaseBuff* modopt(CallConvThiscall) local1 = EloBuddy.Native.ScriptBaseBuff.GetVirtual(buffPtr);
                                    if (_strcmpi(*local1[0][0x34](local1), (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) ptr.ToPointer()) == 0)
                                    {
                                        goto Label_00BA;
                                    }
                                }
                                num++;
                            }
                            while (num < num2);
                        }
                    }
                }
                Marshal.FreeHGlobal(ptr);
            }
            return null;
        Label_00BA:
            Marshal.FreeHGlobal(ptr);
            byte* numPtr = EloBuddy.Native.BuffInstance.GetIndex(instancePtr);
            return new EloBuddy.BuffInstance(instancePtr, base.m_networkId, (short) numPtr[0]);
        }

        public unsafe int GetBuffCount(string buffName)
        {
            EloBuddy.Native.BuffInstance* instancePtr;
            IntPtr ptr;
            if (!string.IsNullOrEmpty(buffName))
            {
                BuffManager* managerPtr = EloBuddy.Native.Obj_AI_Base.GetBuffManager((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr());
                ptr = Marshal.StringToHGlobalAnsi(buffName);
                if (managerPtr != null)
                {
                    BuffNode* nodePtr = *(EloBuddy.Native.BuffManager.GetBegin(managerPtr));
                    BuffNode* nodePtr2 = *(EloBuddy.Native.BuffManager.GetEnd(managerPtr));
                    uint num2 = (uint) ((nodePtr2 - nodePtr) >> 3);
                    if (nodePtr != nodePtr2)
                    {
                        uint num = 0;
                        if (0 < num2)
                        {
                            do
                            {
                                instancePtr = *((EloBuddy.Native.BuffInstance**) ((num * 8) + nodePtr));
                                if (((instancePtr != null) && EloBuddy.Native.BuffInstance.IsValid(instancePtr)) && EloBuddy.Native.BuffInstance.IsActive(instancePtr))
                                {
                                    ScriptBaseBuff* buffPtr = EloBuddy.Native.BuffInstance.GetScriptBaseBuff(instancePtr);
                                    if (_strcmpi(EloBuddy.Native.ScriptBaseBuff.GetName(buffPtr), (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) ptr.ToPointer()) == 0)
                                    {
                                        goto Label_00B2;
                                    }
                                    VirtualScriptBaseBuff* modopt(CallConvThiscall) local1 = EloBuddy.Native.ScriptBaseBuff.GetVirtual(buffPtr);
                                    if (_strcmpi(*local1[0][0x34](local1), (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) ptr.ToPointer()) == 0)
                                    {
                                        goto Label_00B2;
                                    }
                                }
                                num++;
                            }
                            while (num < num2);
                        }
                    }
                }
                Marshal.FreeHGlobal(ptr);
            }
            return -1;
        Label_00B2:
            Marshal.FreeHGlobal(ptr);
            return EloBuddy.Native.BuffInstance.GetCount(instancePtr);
        }

        public Vector3[] GetPath(Vector3 end) => 
            this.GetPath(end, false);

        public Vector3[] GetPath(Vector3 start, Vector3 end) => 
            this.GetPath(start, end, false);

        public unsafe Vector3[] GetPath(Vector3 end, [MarshalAs(UnmanagedType.U1)] bool smoothPath)
        {
            List<Vector3> list = new List<Vector3>();
            EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
            if (ptr != null)
            {
                AIManager_Client* clientPtr = *(EloBuddy.Native.Obj_AI_Base.GetAIManager_Client(ptr));
                if (clientPtr != null)
                {
                    Actor_Common* commonPtr = EloBuddy.Native.AIManager_Client.GetActor(clientPtr);
                    if (commonPtr != null)
                    {
                        NavigationPath path;
                        Vector3f vectorf;
                        Vector3f vectorf2;
                        Vector3 position = base.Position;
                        Vector3 vector3 = base.Position;
                        Vector3 vector2 = base.Position;
                        EloBuddy.Native.Vector3f.{ctor}(&vectorf2, vector2.X, vector3.Z, position.Y);
                        EloBuddy.Native.Vector3f.{ctor}(&vectorf, end.X, end.Z, end.Y);
                        EloBuddy.Native.NavigationPath.{ctor}(&path);
                        try
                        {
                            if (EloBuddy.Native.Actor_Common.CreatePath(commonPtr, (Vector3f modopt(IsConst)* modopt(IsImplicitlyDereferenced)) &vectorf2, (Vector3f modopt(IsConst)* modopt(IsImplicitlyDereferenced)) &vectorf, (NavigationPath modopt(IsConst)* modopt(IsImplicitlyDereferenced)) &path))
                            {
                                if (smoothPath)
                                {
                                    EloBuddy.Native.Actor_Common.SmoothPath(commonPtr, &path);
                                }
                                Vector3f* vectorfPtr = *(EloBuddy.Native.NavigationPath.GetBegin((NavigationPath modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &path));
                                Vector3f** vectorfPtr2 = EloBuddy.Native.NavigationPath.GetEnd((NavigationPath modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &path);
                                if (vectorfPtr != *(((int*) vectorfPtr2)))
                                {
                                    do
                                    {
                                        Vector3 item = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                                        list.Add(item);
                                        vectorfPtr += 12;
                                    }
                                    while (vectorfPtr != *(((int*) vectorfPtr2)));
                                }
                            }
                        }
                        fault
                        {
                            ___CxxCallUnwindDtor(EloBuddy.Native.NavigationPath.{dtor}, (void*) &path);
                        }
                        EloBuddy.Native.NavigationPath.{dtor}(&path);
                    }
                }
            }
            return list.ToArray();
        }

        public unsafe Vector3[] GetPath(Vector3 start, Vector3 end, [MarshalAs(UnmanagedType.U1)] bool smoothPath)
        {
            List<Vector3> list = new List<Vector3>();
            EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
            if (ptr != null)
            {
                AIManager_Client* clientPtr = *(EloBuddy.Native.Obj_AI_Base.GetAIManager_Client(ptr));
                if (clientPtr != null)
                {
                    Actor_Common* commonPtr = EloBuddy.Native.AIManager_Client.GetActor(clientPtr);
                    if (commonPtr != null)
                    {
                        NavigationPath path;
                        Vector3f vectorf;
                        Vector3f vectorf2;
                        EloBuddy.Native.Vector3f.{ctor}(&vectorf2, start.X, start.Z, start.Y);
                        EloBuddy.Native.Vector3f.{ctor}(&vectorf, end.X, end.Z, end.Y);
                        EloBuddy.Native.NavigationPath.{ctor}(&path);
                        try
                        {
                            if (EloBuddy.Native.Actor_Common.CreatePath(commonPtr, (Vector3f modopt(IsConst)* modopt(IsImplicitlyDereferenced)) &vectorf2, (Vector3f modopt(IsConst)* modopt(IsImplicitlyDereferenced)) &vectorf, (NavigationPath modopt(IsConst)* modopt(IsImplicitlyDereferenced)) &path))
                            {
                                if (smoothPath)
                                {
                                    EloBuddy.Native.Actor_Common.SmoothPath(commonPtr, &path);
                                }
                                Vector3f* vectorfPtr = *(EloBuddy.Native.NavigationPath.GetBegin((NavigationPath modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &path));
                                Vector3f** vectorfPtr2 = EloBuddy.Native.NavigationPath.GetEnd((NavigationPath modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &path);
                                if (vectorfPtr != *(((int*) vectorfPtr2)))
                                {
                                    do
                                    {
                                        Vector3 item = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                                        list.Add(item);
                                        vectorfPtr += 12;
                                    }
                                    while (vectorfPtr != *(((int*) vectorfPtr2)));
                                }
                            }
                        }
                        fault
                        {
                            ___CxxCallUnwindDtor(EloBuddy.Native.NavigationPath.{dtor}, (void*) &path);
                        }
                        EloBuddy.Native.NavigationPath.{dtor}(&path);
                    }
                }
            }
            return list.ToArray();
        }

        internal unsafe EloBuddy.Native.Obj_AI_Base* GetPtr() => 
            ((EloBuddy.Native.Obj_AI_Base*) base.GetPtr());

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool HasBuff(string name)
        {
            IntPtr ptr;
            if (!string.IsNullOrEmpty(name))
            {
                BuffManager* managerPtr = EloBuddy.Native.Obj_AI_Base.GetBuffManager((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr());
                ptr = Marshal.StringToHGlobalAnsi(name);
                if (managerPtr != null)
                {
                    BuffNode* nodePtr = *(EloBuddy.Native.BuffManager.GetBegin(managerPtr));
                    BuffNode* nodePtr2 = *(EloBuddy.Native.BuffManager.GetEnd(managerPtr));
                    uint num2 = (uint) ((nodePtr2 - nodePtr) >> 3);
                    if (nodePtr != nodePtr2)
                    {
                        uint num = 0;
                        if (0 < num2)
                        {
                            do
                            {
                                EloBuddy.Native.BuffInstance* instancePtr = *((EloBuddy.Native.BuffInstance**) ((num * 8) + nodePtr));
                                if (((instancePtr != null) && EloBuddy.Native.BuffInstance.IsValid(instancePtr)) && EloBuddy.Native.BuffInstance.IsActive(instancePtr))
                                {
                                    ScriptBaseBuff* buffPtr = EloBuddy.Native.BuffInstance.GetScriptBaseBuff(instancePtr);
                                    if (_strcmpi(EloBuddy.Native.ScriptBaseBuff.GetName(buffPtr), (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) ptr.ToPointer()) == 0)
                                    {
                                        goto Label_00B2;
                                    }
                                    VirtualScriptBaseBuff* modopt(CallConvThiscall) local1 = EloBuddy.Native.ScriptBaseBuff.GetVirtual(buffPtr);
                                    if (_strcmpi(*local1[0][0x34](local1), (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) ptr.ToPointer()) == 0)
                                    {
                                        goto Label_00B2;
                                    }
                                }
                                num++;
                            }
                            while (num < num2);
                        }
                    }
                }
                Marshal.FreeHGlobal(ptr);
            }
            return false;
        Label_00B2:
            Marshal.FreeHGlobal(ptr);
            return true;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool HasBuffOfType(BuffType type)
        {
            List<EloBuddy.BuffInstance>.Enumerator enumerator = this.Buffs.GetEnumerator();
            if (enumerator.MoveNext())
            {
                do
                {
                    EloBuddy.BuffInstance current = enumerator.Current;
                    EloBuddy.Native.BuffInstance* buffPtr = current.GetBuffPtr();
                    if ((buffPtr != null) && EloBuddy.Native.BuffInstance.IsActive(buffPtr))
                    {
                        BuffType type2;
                        EloBuddy.Native.BuffInstance* instancePtr = current.GetBuffPtr();
                        if (instancePtr != null)
                        {
                            type2 = *(EloBuddy.Native.BuffInstance.GetType(instancePtr));
                        }
                        else
                        {
                            type2 = BuffType.Internal;
                        }
                        if (*(((int*) &type2)) == type)
                        {
                            return true;
                        }
                    }
                }
                while (enumerator.MoveNext());
            }
            return false;
        }

        internal static unsafe void OnObj_AI_BaseBuffGainNative(EloBuddy.Native.Obj_AI_Base* A_0, EloBuddy.Native.BuffInstance* A_1)
        {
            Obj_AI_BaseBuffGainEventArgs args = null;
            try
            {
                if ((A_0 != null) && (A_1 != null))
                {
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    byte* numPtr2 = EloBuddy.Native.BuffInstance.GetIndex(A_1);
                    uint* numPtr = EloBuddy.Native.GameObject.GetNetworkId((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) A_0);
                    EloBuddy.BuffInstance item = new EloBuddy.BuffInstance(A_1, numPtr[0], (short) numPtr2[0]);
                    uint networkId = sender.m_networkId;
                    if (!cachedBuffs.ContainsKey((int) networkId))
                    {
                        uint num3 = sender.m_networkId;
                        cachedBuffs[(int) num3] = new List<EloBuddy.BuffInstance>();
                    }
                    if (EloBuddy.Native.BuffInstance.IsValid(A_1))
                    {
                        uint num2 = sender.m_networkId;
                        cachedBuffs[(int) num2].Add(item);
                    }
                    if (Obj_AI_BaseBuffGainHandlers.Count > 0)
                    {
                        args = new Obj_AI_BaseBuffGainEventArgs(item);
                        foreach (Obj_AI_BaseBuffGain gain in Obj_AI_BaseBuffGainHandlers.ToArray())
                        {
                            gain(sender, args);
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
        }

        internal static unsafe void OnObj_AI_BaseBuffLoseNative(EloBuddy.Native.Obj_AI_Base* A_0, EloBuddy.Native.BuffInstance* A_1)
        {
            Obj_AI_BaseBuffLoseEventArgs args = null;
            try
            {
                if ((A_0 != null) && (A_1 != null))
                {
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    byte* numPtr2 = EloBuddy.Native.BuffInstance.GetIndex(A_1);
                    uint* numPtr = EloBuddy.Native.GameObject.GetNetworkId((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) A_0);
                    EloBuddy.BuffInstance buff = new EloBuddy.BuffInstance(A_1, numPtr[0], (short) numPtr2[0]);
                    if (Obj_AI_BaseBuffLoseHandlers.Count > 0)
                    {
                        args = new Obj_AI_BaseBuffLoseEventArgs(buff);
                        foreach (Obj_AI_BaseBuffLose lose in Obj_AI_BaseBuffLoseHandlers.ToArray())
                        {
                            lose(sender, args);
                        }
                    }
                    uint networkId = sender.m_networkId;
                    if (cachedBuffs.ContainsKey((int) networkId))
                    {
                        IntPtr memoryAddress = buff.MemoryAddress;
                        IntPtr ptr5 = memoryAddress;
                        IntPtr ptr3 = memoryAddress;
                        uint num4 = sender.m_networkId;
                        foreach (EloBuddy.BuffInstance instance in cachedBuffs[(int) num4].ToArray())
                        {
                            IntPtr ptr = instance.MemoryAddress;
                            IntPtr ptr4 = ptr;
                            if (ptr == ptr3)
                            {
                                uint num3 = sender.m_networkId;
                                cachedBuffs[(int) num3].Remove(instance);
                            }
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
        }

        internal static unsafe void OnObj_AI_BaseBuffUpdateNative(EloBuddy.Native.Obj_AI_Base* A_0, EloBuddy.Native.BuffInstance* A_1)
        {
            Obj_AI_BaseBuffUpdateEventArgs args = null;
            EloBuddy.BuffInstance buff = null;
            EloBuddy.Obj_AI_Base sender = null;
            try
            {
                if (((A_0 != null) && (A_1 != null)) && (Obj_AI_BaseBuffUpdateHandlers.Count > 0))
                {
                    sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    byte* numPtr2 = EloBuddy.Native.BuffInstance.GetIndex(A_1);
                    uint* numPtr = EloBuddy.Native.GameObject.GetNetworkId((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) A_0);
                    buff = new EloBuddy.BuffInstance(A_1, numPtr[0], (short) numPtr2[0]);
                    args = new Obj_AI_BaseBuffUpdateEventArgs(buff);
                    foreach (Obj_AI_BaseBuffUpdate update in Obj_AI_BaseBuffUpdateHandlers.ToArray())
                    {
                        update(sender, args);
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
        }

        internal static unsafe void OnObj_AI_BaseDoCastSpellNative(EloBuddy.Native.Obj_AI_Base* A_0, SpellCastInfo* A_1)
        {
            Exception innerException = null;
            try
            {
                if ((A_0 != null) && (A_1 != null))
                {
                    Vector3f vectorf;
                    Vector3f vectorf2;
                    EloBuddy.Native.Vector3f.SwitchYZ(EloBuddy.Native.SpellCastInfo.GetStart(A_1), &vectorf2);
                    EloBuddy.Native.Vector3f.SwitchYZ(EloBuddy.Native.SpellCastInfo.GetEnd(A_1), &vectorf);
                    float modopt(CallConvThiscall) x = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf2);
                    float modopt(CallConvThiscall) y = EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf2);
                    Vector3 start = new Vector3(x, y, EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf2));
                    float modopt(CallConvThiscall) introduced21 = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                    float modopt(CallConvThiscall) introduced22 = EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                    Vector3 end = new Vector3(introduced21, introduced22, EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf));
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    uint modopt(IsLong)* numPtr = EloBuddy.Native.SpellData.GetSDataArray(EloBuddy.Native.SpellCastInfo.GetSpellData(A_1)) + 0x39e;
                    bool* flagPtr = numPtr;
                    int* numPtr3 = EloBuddy.Native.SpellCastInfo.GetSpellSlot(A_1);
                    int* numPtr2 = EloBuddy.Native.SpellCastInfo.GetCounter(A_1);
                    GameObjectProcessSpellCastEventArgs args = new GameObjectProcessSpellCastEventArgs(new EloBuddy.SpellData(EloBuddy.Native.SpellCastInfo.GetSpellData(A_1)), EloBuddy.Native.SpellCastInfo.GetLevel(A_1), start, end, EloBuddy.Native.SpellCastInfo.GetLocalId(A_1), numPtr2[0], *((EloBuddy.SpellSlot*) numPtr3), *((bool*) numPtr));
                    if (sender != null)
                    {
                        foreach (Obj_AI_BaseDoCastSpell spell in Obj_AI_BaseDoCastSpellHandlers.ToArray())
                        {
                            try
                            {
                                spell(sender, args);
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

        internal static unsafe void OnObj_AI_BaseLevelUpNative(EloBuddy.Native.Obj_AI_Base* A_0, int A_1)
        {
            Exception innerException = null;
            try
            {
                if (A_0 != null)
                {
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    Obj_AI_BaseLevelUpEventArgs args = new Obj_AI_BaseLevelUpEventArgs(sender, A_1);
                    foreach (Obj_AI_BaseLevelUp up in Obj_AI_BaseLevelUpHandlers.ToArray())
                    {
                        try
                        {
                            up(sender, args);
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

        internal static unsafe void OnObj_AI_BaseNewPathNative(EloBuddy.Native.Obj_AI_Base* A_0, vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> >* A_1, [MarshalAs(UnmanagedType.U1)] bool A_2, float A_3)
        {
            Exception innerException = null;
            try
            {
                if ((A_0 != null) && (A_1 != null))
                {
                    vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> > local3;
                    List<Vector3> list = new List<Vector3>();
                    std.vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> >.{ctor}(&local3, (vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> > modopt(IsConst)* modopt(IsImplicitlyDereferenced)) A_1);
                    try
                    {
                        _Vector_iterator<std::_Vector_val<std::_Simple_types<EloBuddy::Native::Vector3f> > > local2;
                        *((int*) &local2) = *((int*) &local3);
                        while (true)
                        {
                            _Vector_iterator<std::_Vector_val<std::_Simple_types<EloBuddy::Native::Vector3f> > > local;
                            Vector3f* vectorfPtr4 = *((Vector3f**) (&local3 + 4));
                            *((int*) &local) = *((int*) (&local3 + 4));
                            if (((byte) (((byte) (*(((int*) &local2)) == *(((int*) (&local3 + 4))))) == 0)) == 0)
                            {
                                break;
                            }
                            Vector3f* modopt(IsImplicitlyDereferenced) vectorfPtr3 = *((Vector3f* modopt(IsImplicitlyDereferenced)*) &local2);
                            Vector3f* modopt(IsImplicitlyDereferenced) vectorfPtr2 = *((Vector3f* modopt(IsImplicitlyDereferenced)*) &local2);
                            Vector3f* modopt(IsImplicitlyDereferenced) vectorfPtr = *((Vector3f* modopt(IsImplicitlyDereferenced)*) &local2);
                            float modopt(CallConvThiscall) x = EloBuddy.Native.Vector3f.GetX(*((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)*) &local2));
                            float modopt(CallConvThiscall) y = EloBuddy.Native.Vector3f.GetZ(*((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)*) &local2));
                            Vector3 item = new Vector3(x, y, EloBuddy.Native.Vector3f.GetY(*((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)*) &local2)));
                            list.Add(item);
                            *((int*) &local2) += 12;
                        }
                        EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                        GameObjectNewPathEventArgs args = new GameObjectNewPathEventArgs(list.ToArray(), A_2, A_3);
                        foreach (Obj_AI_BaseNewPath path in Obj_AI_BaseNewPathHandlers.ToArray())
                        {
                            try
                            {
                                path(sender, args);
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
                    fault
                    {
                        ___CxxCallUnwindDtor(std.vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> >.{dtor}, (void*) &local3);
                    }
                    std.vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> >._Tidy(&local3);
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

        internal static unsafe void OnObj_AI_BaseOnBasicAttackNative(EloBuddy.Native.Obj_AI_Base* A_0, SpellCastInfo* A_1)
        {
            Exception innerException = null;
            GameObjectProcessSpellCastEventArgs args = null;
            try
            {
                if (A_0 != null)
                {
                    Vector3f vectorf;
                    Vector3f vectorf2;
                    EloBuddy.Native.Vector3f.SwitchYZ(EloBuddy.Native.SpellCastInfo.GetStart(A_1), &vectorf2);
                    EloBuddy.Native.Vector3f.SwitchYZ(EloBuddy.Native.SpellCastInfo.GetEnd(A_1), &vectorf);
                    float modopt(CallConvThiscall) x = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf2);
                    float modopt(CallConvThiscall) y = EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf2);
                    Vector3 start = new Vector3(x, y, EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf2));
                    float modopt(CallConvThiscall) introduced23 = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                    float modopt(CallConvThiscall) introduced24 = EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                    Vector3 end = new Vector3(introduced23, introduced24, EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf));
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    EloBuddy.SpellData sdata = new EloBuddy.SpellData(EloBuddy.Native.SpellCastInfo.GetSpellData(A_1));
                    string str = sdata.Name.ToLower();
                    if ((str.Contains("attack") && (Array.IndexOf<string>(noAttacks, str) == -1)) || (Array.IndexOf<string>(attacks, str) != -1))
                    {
                        uint modopt(IsLong)* numPtr = EloBuddy.Native.SpellData.GetSDataArray(EloBuddy.Native.SpellCastInfo.GetSpellData(A_1)) + 0x39e;
                        bool* flagPtr = numPtr;
                        int* numPtr3 = EloBuddy.Native.SpellCastInfo.GetSpellSlot(A_1);
                        int* numPtr2 = EloBuddy.Native.SpellCastInfo.GetCounter(A_1);
                        args = new GameObjectProcessSpellCastEventArgs(sdata, EloBuddy.Native.SpellCastInfo.GetLevel(A_1), start, end, EloBuddy.Native.SpellCastInfo.GetLocalId(A_1), numPtr2[0], *((EloBuddy.SpellSlot*) numPtr3), *((bool*) numPtr));
                        if (sender != null)
                        {
                            foreach (Obj_AI_BaseOnBasicAttack attack in Obj_AI_BaseOnBasicAttackHandlers.ToArray())
                            {
                                try
                                {
                                    attack(sender, args);
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

        internal static unsafe void OnObj_AI_BaseOnSurrenderVoteNative(EloBuddy.Native.Obj_AI_Base* A_0, byte A_1)
        {
            Exception innerException = null;
            try
            {
                if (A_0 != null)
                {
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    Obj_AI_BaseSurrenderVoteEventArgs args = new Obj_AI_BaseSurrenderVoteEventArgs(A_1);
                    foreach (Obj_AI_BaseOnSurrenderVote vote in Obj_AI_BaseOnSurrenderVoteHandlers.ToArray())
                    {
                        try
                        {
                            vote(sender, args);
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

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnObj_AI_BasePlayAnimationNative(EloBuddy.Native.Obj_AI_Base* A_0, int A_1)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                if (A_0 != null)
                {
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    GameObjectPlayAnimationEventArgs args = new GameObjectPlayAnimationEventArgs(A_1);
                    if (sender == null)
                    {
                        return flag;
                    }
                    foreach (Obj_AI_BasePlayAnimation animation in Obj_AI_BasePlayAnimationHandlers.ToArray())
                    {
                        try
                        {
                            animation(sender, args);
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

        internal static unsafe void OnObj_AI_BaseTeleportNative(EloBuddy.Native.Obj_AI_Base* A_0, sbyte modopt(IsSignUnspecifiedByte)* A_1, sbyte modopt(IsSignUnspecifiedByte)* A_2)
        {
            Exception innerException = null;
            try
            {
                if (A_0 != null)
                {
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    GameObjectTeleportEventArgs args = new GameObjectTeleportEventArgs(new string(A_1), new string(A_2));
                    foreach (Obj_AI_BaseTeleport teleport in Obj_AI_BaseTeleportHandlers.ToArray())
                    {
                        try
                        {
                            teleport(sender, args);
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

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnObj_AI_ProcessSpellCastNative(EloBuddy.Native.Obj_AI_Base* __unnamed000, SpellCastInfo* castInfo)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                if ((__unnamed000 != null) && (castInfo != null))
                {
                    Vector3f* vectorfPtr2 = EloBuddy.Native.SpellCastInfo.GetStart(castInfo);
                    Vector3f* vectorfPtr = EloBuddy.Native.SpellCastInfo.GetEnd(castInfo);
                    Vector3 start = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr2), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr2), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr2));
                    Vector3 end = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) __unnamed000);
                    int* numPtr2 = EloBuddy.Native.SpellCastInfo.GetSpellSlot(castInfo);
                    int* numPtr = EloBuddy.Native.SpellCastInfo.GetCounter(castInfo);
                    GameObjectProcessSpellCastEventArgs args = new GameObjectProcessSpellCastEventArgs(new EloBuddy.SpellData(EloBuddy.Native.SpellCastInfo.GetSpellData(castInfo)), EloBuddy.Native.SpellCastInfo.GetLevel(castInfo), start, end, EloBuddy.Native.SpellCastInfo.GetLocalId(castInfo), numPtr[0], *((EloBuddy.SpellSlot*) numPtr2), false);
                    if (sender == null)
                    {
                        byte num2 = 1;
                        return (bool) num2;
                    }
                    foreach (Obj_AI_ProcessSpellCast cast in Obj_AI_ProcessSpellCastHandlers.ToArray())
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
        internal static unsafe bool OnObj_AI_UpdateModelNative(EloBuddy.Native.Obj_AI_Base* A_0, sbyte modopt(IsSignUnspecifiedByte)* A_1, int A_2)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                if (A_0 != null)
                {
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    UpdateModelEventArgs args = new UpdateModelEventArgs(new string(A_1), A_2);
                    foreach (Obj_AI_UpdateModel model in Obj_AI_UpdateModelHandlers.ToArray())
                    {
                        try
                        {
                            model(sender, args);
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

        internal static unsafe void OnObj_AI_UpdatePositionNative(EloBuddy.Native.Obj_AI_Base* A_0, Vector3f* A_1)
        {
            Exception innerException = null;
            try
            {
                if (A_0 != null)
                {
                    EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                    Vector3 vector = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_1), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_1), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_1));
                    Obj_AI_UpdatePositionEventArgs args = new Obj_AI_UpdatePositionEventArgs(sender, vector);
                    foreach (EloBuddy.Obj_AI_UpdatePosition position in Obj_AI_UpdatePositionHandlers.ToArray())
                    {
                        try
                        {
                            position(sender, args);
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

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool SetModel(string model)
        {
            EloBuddy.Native.Obj_AI_Base* basePtr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
            if (basePtr != null)
            {
                CharacterDataStack* stackPtr = EloBuddy.Native.Obj_AI_Base.GetCharacterDataStack(basePtr);
                if (stackPtr != null)
                {
                    IntPtr ptr = Marshal.StringToHGlobalAnsi(model);
                    return EloBuddy.Native.CharacterDataStack.SetModel(stackPtr, (sbyte modopt(IsSignUnspecifiedByte)*) ptr.ToPointer());
                }
            }
            return false;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool SetSkin(string model, int skinId)
        {
            EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
            if (ptr != null)
            {
                CharacterDataStack* stackPtr = EloBuddy.Native.Obj_AI_Base.GetCharacterDataStack(ptr);
                if (stackPtr != null)
                {
                    EloBuddy.Native.CharacterDataStack.SetBaseSkinId(stackPtr, skinId);
                }
            }
            return this.SetModel(model);
        }

        public unsafe void SetSkinId(int skinId)
        {
            EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
            if (ptr != null)
            {
                CharacterDataStack* stackPtr = EloBuddy.Native.Obj_AI_Base.GetCharacterDataStack(ptr);
                if (stackPtr != null)
                {
                    EloBuddy.Native.CharacterDataStack.SetBaseSkinId(stackPtr, skinId);
                }
            }
        }

        public float _FlatArmorModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatArmorModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatArmorPenetrationMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatArmorPenetrationMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatArmorPenetrationModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatArmorPenetrationModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatCritChanceModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatCritChanceModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatCritDamageModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatCritDamageModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatDodgeMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatDodgeMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatDodgeModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatDodgeModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatHPModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatHPModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatHPRegenModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatHPRegenModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatMagicDamageModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatMagicDamageModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatMagicPenetrationMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatMagicPenetrationMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatMagicPenetrationModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatMagicPenetrationModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatMovementSpeedModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatMovementSpeedModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatMPModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatMPModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatMPRegenModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatMPRegenModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatPhysicalDamageModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatPhysicalDamageModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatSpellBlockModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatSpellBlockModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatTimeDeadMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatTimeDeadMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _FlatTimeDeadModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_FlatTimeDeadModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _NonHealingFlatHPPoolMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_NonHealingFlatHPPoolMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _PercentArmorPenetrationMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_PercentArmorPenetrationMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _PercentArmorPenetrationModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_PercentArmorPenetrationModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _PercentAttackSpeedModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_PercentAttackSpeedModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _PercentCooldownMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_PercentCooldownMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _PercentCooldownModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_PercentCooldownModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _PercentMagicPenetrationMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_PercentMagicPenetrationMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _PercentMagicPenetrationModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_PercentMagicPenetrationModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _PercentMovementSpeedModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_PercentMovementSpeedModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _PercentTimeDeadMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_PercentTimeDeadMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float _PercentTimeDeadModPerLevel
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.Get_PercentTimeDeadModPerLevel(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float AcquisitionRangeMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetAcquisitionRangeMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        [Obsolete("This property will be removed as it's not used in the client.")]
        public int AI_LastPetSpawnedId =>
            0;

        public float Armor
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetArmor(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float AttackCastDelay
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return EloBuddy.Native.Obj_AI_Base.GetAttackCastDelay(ptr);
                }
                return 0f;
            }
        }

        public float AttackDelay
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return EloBuddy.Native.Obj_AI_Base.GetAttackDelay(ptr);
                }
                return 0f;
            }
        }

        public float AttackRange
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetAttackRange(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float AttackSpeedMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetAttackSpeedMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public int AutoAttackTargettingFlags
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetAutoAttackTargettingFlags(ptr));
                }
                return 0;
            }
        }

        public float BaseAbilityDamage
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetBaseAbilityDamage(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float BaseAttackDamage
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetBaseAttackDamage(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float BasePARRegenRate
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetBasePARRegenRate(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public string BaseSkinName
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsConst) modopt(IsConst) localPtr = EloBuddy.Native.Obj_AI_Base.GetSkinName(ptr);
                    return new string((0x10 > localPtr[20]) ? ((sbyte*) localPtr) : ((sbyte*) localPtr[0]));
                }
                return "Unknown";
            }
        }

        public EloBuddy.SpellData BasicAttack
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    EloBuddy.Native.SpellData* spelldata = EloBuddy.Native.Obj_AI_Base.GetBasicAttack(ptr);
                    if (spelldata != null)
                    {
                        return new EloBuddy.SpellData(spelldata);
                    }
                }
                return null;
            }
        }

        public List<EloBuddy.BuffInstance> Buffs
        {
            get
            {
                uint networkId = base.m_networkId;
                if (cachedBuffs.ContainsKey((int) networkId))
                {
                    uint num4 = base.m_networkId;
                    return cachedBuffs[(int) num4];
                }
                List<EloBuddy.BuffInstance> list = new List<EloBuddy.BuffInstance>();
                BuffManager* managerPtr = EloBuddy.Native.Obj_AI_Base.GetBuffManager((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr());
                if (managerPtr != null)
                {
                    BuffNode* nodePtr = *(EloBuddy.Native.BuffManager.GetBegin(managerPtr));
                    BuffNode* nodePtr2 = *(EloBuddy.Native.BuffManager.GetEnd(managerPtr));
                    if ((nodePtr != null) && (nodePtr2 != null))
                    {
                        uint num2 = (uint) ((nodePtr2 - nodePtr) >> 3);
                        uint num = 0;
                        if (0 < num2)
                        {
                            do
                            {
                                EloBuddy.Native.BuffInstance* inst = *((EloBuddy.Native.BuffInstance**) ((num * 8) + nodePtr));
                                if (((inst != null) && EloBuddy.Native.BuffInstance.IsValid(inst)) && EloBuddy.Native.BuffInstance.IsActive(inst))
                                {
                                    list.Add(new EloBuddy.BuffInstance(inst, base.m_networkId, (short) num));
                                }
                                num++;
                            }
                            while (num < num2);
                        }
                    }
                }
                uint num3 = base.m_networkId;
                cachedBuffs.Add((int) num3, list);
                return list;
            }
        }

        public bool CanAttack =>
            ((GameObjectCharacterState) *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()))).HasFlag(GameObjectCharacterState.CanAttack);

        public bool CanCast
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                if (state.HasFlag(GameObjectCharacterState.Surpressed) && state.HasFlag(GameObjectCharacterState.CanCast))
                {
                    num = 0;
                }
                else
                {
                    num = 1;
                }
                return (bool) ((byte) num);
            }
        }

        public bool CanMove
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                if (!state.HasFlag(GameObjectCharacterState.CanMove) && !state.HasFlag(GameObjectCharacterState.Immovable))
                {
                    num = 0;
                }
                else
                {
                    num = 1;
                }
                return (bool) ((byte) num);
            }
        }

        public float CastRange
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetCastRange(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public GameObjectCharacterState CharacterState =>
            *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));

        public EloBuddy.CharData CharData
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    EloBuddy.Native.CharData* dataPtr = *(EloBuddy.Native.Obj_AI_Base.GetCharData(ptr));
                    if (dataPtr != null)
                    {
                        CharDataInfo* charInfo = EloBuddy.Native.CharData.GetCharDataInfo(dataPtr);
                        if (charInfo != null)
                        {
                            return new EloBuddy.CharData(charInfo);
                        }
                    }
                }
                return null;
            }
        }

        public GameObjectCombatType CombatType
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetCombatType(ptr));
                }
                return GameObjectCombatType.Melee;
            }
        }

        public float Crit
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetCrit(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float CritDamageMultiplier
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetCritDamageMultiplier(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float DeathDuration =>
            0f;

        public Vector3 Direction
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    Vector3f* vectorfPtr = EloBuddy.Native.AttackableUnit.GetDirection((EloBuddy.Native.AttackableUnit* modopt(IsConst) modopt(IsConst)) ptr);
                    if (vectorfPtr != null)
                    {
                        return new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                    }
                }
                return Vector3.Zero;
            }
        }

        public float Dodge
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetDodge(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public int EvolvePoints
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetEvolvePoints(ptr));
                }
                return 0;
            }
        }

        public float ExpGiveRadius
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetExpGiveRadius(ptr));
                }
                return 0f;
            }
        }

        [Obsolete("This property will be removed as it's not used in the client.")]
        public Vector3 FearLeashPoint =>
            Vector3.Zero;

        public BitVector32 Flags
        {
            get
            {
                int* numPtr = EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr());
                return new BitVector32(numPtr[0]);
            }
        }

        public float FlatArmorMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatArmorMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatArmorPenetrationMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatArmorPenetrationMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatAttackRangeMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatAttackRangeMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatBubbleRadiusMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatBubbleRadiusMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatCastRangeMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatCastRangeMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatCooldownMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatCooldownMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatCritChanceMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatCritChanceMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatCritDamageMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatCritDamageMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatDamageReductionFromBarracksMinionMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetFlatDamageReductionFromBarracksMinionMod(ptr));
                }
                return 0f;
            }
        }

        public float FlatDodgeMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatDodgeMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatExpRewardMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatExpRewardMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatGoldPer10Mod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatGoldPer10Mod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatGoldRewardMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatGoldRewardMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatHPPoolMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatHPPoolMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatHPRegenMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatHPRegenMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatMagicDamageMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatMagicDamageMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatMagicPenetrationMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatMagicPenetrationMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatMagicReduction
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatMagicReduction(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatMissChanceMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatMissChanceMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatMovementSpeedHasteMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatMovementSpeedHasteMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatMovementSpeedSlowMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatMovementSpeedSlowMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatPARPoolMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatPARPoolMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatPARRegenMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatPARRegenMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatPhysicalDamageMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatPhysicalDamageMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatPhysicalReduction
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatPhysicalReduction(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float FlatSpellBlockMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetFlatSpellBlockMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float Gold
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetGold(ptr));
                }
                return 0f;
            }
        }

        public float GoldTotal
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetGoldTotal(ptr));
                }
                return 0f;
            }
        }

        public Vector2 HPBarPosition
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    UnitInfoComponent* componentPtr = *(EloBuddy.Native.Obj_AI_Base.GetInfoComponent(ptr));
                    if (componentPtr != null)
                    {
                        Vector3f vectorf;
                        EloBuddy.Native.UnitInfoComponent.GetHPBarPosition(componentPtr, &vectorf);
                        float modopt(CallConvThiscall) x = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                        return new Vector2(x, EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf));
                    }
                }
                return Vector2.Zero;
            }
        }

        public float HPBarXOffset
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    UnitInfoComponent* componentPtr = *(EloBuddy.Native.Obj_AI_Base.GetInfoComponent(ptr));
                    if (componentPtr != null)
                    {
                        UnitInfoHealthBar* barPtr = *(EloBuddy.Native.UnitInfoComponent.GetHealthbar(componentPtr));
                        if (barPtr != null)
                        {
                            return *(EloBuddy.Native.UnitInfoHealthBar.GetXOffset(barPtr));
                        }
                    }
                }
                return 0f;
            }
            set
            {
                if (base.GetPtr() != null)
                {
                    UnitInfoComponent* componentPtr = *(EloBuddy.Native.Obj_AI_Base.GetInfoComponent((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                    if (componentPtr != null)
                    {
                        UnitInfoHealthBar* barPtr = *(EloBuddy.Native.UnitInfoComponent.GetHealthbar(componentPtr));
                        if (barPtr != null)
                        {
                            EloBuddy.Native.UnitInfoHealthBar.SetXOffset(barPtr, value);
                        }
                    }
                }
            }
        }

        public float HPBarYOffset
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    UnitInfoComponent* componentPtr = *(EloBuddy.Native.Obj_AI_Base.GetInfoComponent(ptr));
                    if (componentPtr != null)
                    {
                        UnitInfoHealthBar* barPtr = *(EloBuddy.Native.UnitInfoComponent.GetHealthbar(componentPtr));
                        if (barPtr != null)
                        {
                            return *(EloBuddy.Native.UnitInfoHealthBar.GetYOffset(barPtr));
                        }
                    }
                }
                return 0f;
            }
            set
            {
                if (base.GetPtr() != null)
                {
                    UnitInfoComponent* componentPtr = *(EloBuddy.Native.Obj_AI_Base.GetInfoComponent((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                    if (componentPtr != null)
                    {
                        UnitInfoHealthBar* barPtr = *(EloBuddy.Native.UnitInfoComponent.GetHealthbar(componentPtr));
                        if (barPtr != null)
                        {
                            EloBuddy.Native.UnitInfoHealthBar.SetYOffset(barPtr, value);
                        }
                    }
                }
            }
        }

        public float HPRegenRate
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetHPRegenRate(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public Vector3 InfoComponentBasePosition
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    UnitInfoComponent* componentPtr = *(EloBuddy.Native.Obj_AI_Base.GetInfoComponent(ptr));
                    if (componentPtr != null)
                    {
                        Vector3f* vectorfPtr = EloBuddy.Native.UnitInfoComponent.GetBaseDrawPosition(componentPtr);
                        return new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                    }
                }
                return Vector3.Zero;
            }
        }

        public EloBuddy.InventorySlot[] InventoryItems
        {
            get
            {
                List<EloBuddy.InventorySlot> list = new List<EloBuddy.InventorySlot>();
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    HeroInventory* inventoryPtr = EloBuddy.Native.Obj_AI_Base.GetInventory(ptr);
                    if (inventoryPtr != null)
                    {
                        int slot = 0;
                        do
                        {
                            EloBuddy.Native.InventorySlot* slotPtr = EloBuddy.Native.HeroInventory.GetInventorySlot(inventoryPtr, slot);
                            if ((slotPtr != null) && (*(((int*) slotPtr)) != 0))
                            {
                                list.Add(new EloBuddy.InventorySlot(base.m_networkId, slot));
                            }
                            slot++;
                        }
                        while (slot < 0x27);
                    }
                }
                return list.ToArray();
            }
        }

        public bool IsAsleep
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) ((*(((int*) &state))[0x10] >> 10) & 1));
            }
        }

        public bool IsCallForHelpSuppresser
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return ((*(((int*) &state))[0x3a] != 0) ? ((bool) ((byte) 1)) : ((bool) ((byte) 0)));
            }
        }

        public bool IsCharmed
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) ((*(((int*) &state))[0x10] >> 15) & 1));
            }
        }

        public bool IsFeared
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) ((*(((int*) &state))[0x10] >> 7) & 1));
            }
        }

        public bool IsFleeing
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) ((*(((int*) &state))[0x10] >> 8) & 1));
            }
        }

        public bool IsForceRenderParticles
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) ((*(((int*) &state))[0x10] >> 0x11) & 1));
            }
        }

        public bool IsGhosted
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) ((*(((int*) &state))[0x10] >> 12) & 1));
            }
        }

        public bool IsHPBarRendered
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    GameObjectVTable* tablePtr = EloBuddy.Native.GameObject.GetVirtual((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) ptr);
                    if (tablePtr != null)
                    {
                        return (bool) **(((int*) tablePtr))[0x1c4](tablePtr);
                    }
                }
                return false;
            }
        }

        public bool IsIgnoreCallForHelp
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return ((*(((int*) &state))[0x38] != 0) ? ((bool) ((byte) 1)) : ((bool) ((byte) 0)));
            }
        }

        public bool IsMinion
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    GameObjectVTable* tablePtr = EloBuddy.Native.GameObject.GetVirtual((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) ptr);
                    if (tablePtr != null)
                    {
                        return (bool) **(((int*) tablePtr))[0x360](tablePtr);
                    }
                }
                return false;
            }
        }

        public bool IsMonster =>
            ((bool) ((byte) (base.Team == EloBuddy.GameObjectTeam.Neutral)));

        public bool IsMoving =>
            ((this.Path.Length >= 2) ? ((bool) ((byte) 1)) : ((bool) ((byte) 0)));

        public bool IsNearSight
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) ((*(((int*) &state))[0x10] >> 11) & 1));
            }
        }

        public bool IsNoRender
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) (*(((int*) &state))[0x22] & 1));
            }
        }

        public bool IsPacified
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                if (state.HasFlag(GameObjectCharacterState.CanCast) && !state.HasFlag(GameObjectCharacterState.CanCast))
                {
                    num = 1;
                }
                else
                {
                    num = 0;
                }
                return (bool) ((byte) num);
            }
        }

        public bool IsRevealSpecificUnit
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) ((*(((int*) &state))[0x10] >> 5) & 1));
            }
        }

        public bool IsRooted
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                if ((state.HasFlag(GameObjectCharacterState.Immovable) && !state.HasFlag(GameObjectCharacterState.CanMove)) && !state.HasFlag(GameObjectCharacterState.CanAttack))
                {
                    num = 1;
                }
                else
                {
                    num = 0;
                }
                return (bool) ((byte) num);
            }
        }

        public bool IsStealthed
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) ((*(((int*) &state))[0x10] >> 4) & 1));
            }
        }

        public bool IsStunned
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int num;
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                if ((state.HasFlag(GameObjectCharacterState.Immovable) && !state.HasFlag(GameObjectCharacterState.CanMove)) && (!state.HasFlag(GameObjectCharacterState.CanAttack) && !state.HasFlag(GameObjectCharacterState.CanCast)))
                {
                    num = 1;
                }
                else
                {
                    num = 0;
                }
                return (bool) ((byte) num);
            }
        }

        public bool IsSuppressCallForHelp
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return ((*(((int*) &state))[0x39] != 0) ? ((bool) ((byte) 1)) : ((bool) ((byte) 0)));
            }
        }

        public bool IsTaunted
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                GameObjectCharacterState state = *(EloBuddy.Native.Obj_AI_Base.GetCharacterActionState((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr()));
                return (bool) ((byte) ((*(((int*) &state))[0x10] >> 6) & 1));
            }
        }

        [Obsolete("This property will be removed as it's not used in the client.")]
        public Vector3 LastPausePosition =>
            Vector3.Zero;

        public float MissChance
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetMissChance(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public string Model
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    CharacterDataStack* stackPtr = EloBuddy.Native.Obj_AI_Base.GetCharacterDataStack(ptr);
                    if (stackPtr != null)
                    {
                        basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsConst) modopt(IsConst) localPtr = EloBuddy.Native.CharacterDataStack.GetActiveModel(stackPtr);
                        return new string((0x10 > localPtr[20]) ? ((sbyte*) localPtr) : ((sbyte*) localPtr[0]));
                    }
                }
                return "Unknown";
            }
        }

        public float MoveSpeed
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetMoveSpeed(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float MoveSpeedFloorMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetMoveSpeedFloorMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PARRegenRate
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPARRegenRate(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PassiveCooldownEndTime
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPassiveCooldownEndTime(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PassiveCooldownTotalTime
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPassiveCooldownTotalTime(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public Vector3[] Path
        {
            get
            {
                List<Vector3> list = new List<Vector3>();
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    AIManager_Client* clientPtr = *(EloBuddy.Native.Obj_AI_Base.GetAIManager_Client(ptr));
                    if ((clientPtr != null) && (EloBuddy.Native.AIManager_Client.GetActor(clientPtr) != null))
                    {
                        NavigationPath* pathPtr = EloBuddy.Native.AIManager_Client.GetNavPath(clientPtr);
                        if (pathPtr != null)
                        {
                            Vector3f* vectorfPtr = *(EloBuddy.Native.NavigationPath.GetBegin((NavigationPath modopt(IsConst)* modopt(IsConst) modopt(IsConst)) pathPtr));
                            Vector3f** vectorfPtr2 = EloBuddy.Native.NavigationPath.GetEnd((NavigationPath modopt(IsConst)* modopt(IsConst) modopt(IsConst)) pathPtr);
                            if (vectorfPtr != *(((int*) vectorfPtr2)))
                            {
                                do
                                {
                                    Vector3 item = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                                    list.Add(item);
                                    vectorfPtr += 12;
                                }
                                while (vectorfPtr != *(((int*) vectorfPtr2)));
                            }
                        }
                    }
                }
                return list.ToArray();
            }
        }

        public float PathfindingRadiusMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPathfindingRadiusMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentArmorMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentArmorMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentArmorPenetrationMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentArmorPenetrationMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentAttackRangeMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentAttackRangeMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentAttackSpeedMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentAttackSpeedMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentBaseHPRegenMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentBaseHPRegenMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentBasePARRegenMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentBasePARRegenMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentBonusArmorPenetrationMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentBonusArmorPenetrationMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentBonusMagicPenetrationMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentBonusMagicPenetrationMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentBubbleRadiusMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentBubbleRadiusMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentCastRangeMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentCastRangeMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentCCReduction
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentCCReduction(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentCooldownMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentCooldownMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentCritDamageMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentCritDamageMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentDamageToBarracksMinionMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetPercentDamageToBarracksMinionMod(ptr));
                }
                return 0f;
            }
        }

        public float PercentEXPBonus
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentEXPBonus(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentGoldLostOnDeathMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentGoldLostOnDeathMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentHealingAmountMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentHealingAmountMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentHPPoolMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentHPPoolMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentHPRegenMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentHPRegenMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentLifeStealMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentLifeStealMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentLocalGoldRewardMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentLocalGoldRewardMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentMagicDamageMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentMagicDamageMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentMagicPenetrationMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentMagicPenetrationMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentMagicReduction
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentMagicReduction(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentMovementSpeedHasteMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentMovementSpeedHasteMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentMovementSpeedSlowMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentMovementSpeedSlowMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentMultiplicativeAttackSpeedMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentMultiplicativeAttackSpeedMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentMultiplicativeMovementSpeedMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentMultiplicativeMovementSpeedMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentPARPoolMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentPARPoolMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentPARRegenMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentPARRegenMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentPhysicalDamageMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentPhysicalDamageMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentPhysicalReduction
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentPhysicalReduction(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentRespawnTimeMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentRespawnTimeMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentSlowResistMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentSlowResistMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentSpellBlockMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentSpellBlockMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentSpellVampMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentSpellVampMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentTenacityCharacterMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentTenacityCharacterMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentTenacityCleanseMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentTenacityCleanseMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentTenacityItemMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentTenacityItemMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentTenacityMasteryMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentTenacityMasteryMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public float PercentTenacityRuneMod
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetPercentTenacityRuneMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public EloBuddy.GameObject Pet =>
            null;

        [Obsolete("This property will be removed as it's not used in the client.")]
        public float PetReturnRadius =>
            0f;

        public bool PlayerControlled
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.Obj_AI_Base.GetPlayerControlled(ptr));
                }
                return false;
            }
        }

        public float ScaleSkinCoef
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetScaleSkinCoef(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public Vector3 ServerPosition
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    Vector3f vectorf;
                    Vector3f vectorf2;
                    Vector3f vectorf3;
                    return new Vector3(EloBuddy.Native.Vector3f.GetX(EloBuddy.Native.GameObject.GetServerPosition((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) ptr, &vectorf3)), EloBuddy.Native.Vector3f.GetY(EloBuddy.Native.GameObject.GetServerPosition((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) ptr, &vectorf2)), EloBuddy.Native.Vector3f.GetZ(EloBuddy.Native.GameObject.GetServerPosition((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) ptr, &vectorf)));
                }
                return Vector3.Zero;
            }
        }

        public int SkinId
        {
            get
            {
                CharacterDataStack* stackPtr = EloBuddy.Native.Obj_AI_Base.GetCharacterDataStack((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr());
                if (stackPtr != null)
                {
                    return *(EloBuddy.Native.CharacterDataStack.GetActiveSkinId(stackPtr));
                }
                return -1;
            }
        }

        public float SpellBlock
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetSpellBlock(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        public EloBuddy.Spellbook Spellbook =>
            new EloBuddy.Spellbook(base.GetPtr());

        [Obsolete("This property will be removed as it's not used in the client.")]
        public float SpellCastBlockingAI =>
            0f;

        public float TotalAttackDamage
        {
            get
            {
                float num;
                float num2;
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    num2 = *(EloBuddy.Native.CharacterIntermediate.GetFlatPhysicalDamageMod(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                else
                {
                    num2 = 0f;
                }
                EloBuddy.Native.Obj_AI_Base* basePtr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (basePtr != null)
                {
                    num = *(EloBuddy.Native.CharacterIntermediate.GetBaseAttackDamage(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(basePtr)));
                }
                else
                {
                    num = 0f;
                }
                return (num + num2);
            }
        }

        public float TotalMagicalDamage
        {
            get
            {
                EloBuddy.Native.Obj_AI_Base* ptr = (EloBuddy.Native.Obj_AI_Base*) base.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.CharacterIntermediate.GetBaseAbilityDamage(EloBuddy.Native.Obj_AI_Base.GetCharacterIntermediate(ptr)));
                }
                return 0f;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObj_AI_BaseBuffGainNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, EloBuddy.Native.BuffInstance* A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObj_AI_BaseBuffLoseNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, EloBuddy.Native.BuffInstance* A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObj_AI_BaseBuffUpdateNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, EloBuddy.Native.BuffInstance* A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObj_AI_BaseDoCastSpellNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, SpellCastInfo* A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObj_AI_BaseLevelUpNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, int A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObj_AI_BaseNewPathNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, vector<EloBuddy::Native::Vector3f,std::allocator<EloBuddy::Native::Vector3f> >* A_1, [MarshalAs(UnmanagedType.U1)] bool A_2, float A_3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObj_AI_BaseOnBasicAttackNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, SpellCastInfo* A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObj_AI_BaseOnSurrenderVoteNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, byte A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnObj_AI_BasePlayAnimationNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, int A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObj_AI_BaseTeleportNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, sbyte modopt(IsSignUnspecifiedByte)* A_1, sbyte modopt(IsSignUnspecifiedByte)* A_2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnObj_AI_ProcessSpellCastNativeDelegate(EloBuddy.Native.Obj_AI_Base* __unnamed000, SpellCastInfo* castInfo);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnObj_AI_UpdateModelNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, sbyte modopt(IsSignUnspecifiedByte)* A_1, int A_2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObj_AI_UpdatePositionNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, Vector3f* A_1);
    }
}

