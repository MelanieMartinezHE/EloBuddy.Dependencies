namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using std;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Player : EloBuddy.AIHeroClient
    {
        private static EloBuddy.AIHeroClient m_instance;
        internal static IntPtr m_Player_DoEmoteNative = new IntPtr();
        internal static OnPlayer_DoEmoteNativeDelegate m_Player_DoEmoteNativeDelegate;
        internal static IntPtr m_Player_ProcessIssueOrderNative = new IntPtr();
        internal static OnPlayer_ProcessIssueOrderNativeDelegate m_Player_ProcessIssueOrderNativeDelegate;
        internal static IntPtr m_Player_SwapItemNative = new IntPtr();
        internal static OnPlayer_SwapItemNativeDelegate m_Player_SwapItemNativeDelegate;
        internal static IntPtr m_PlayerPostIssueOrderNative = new IntPtr();
        internal static OnPlayerPostIssueOrderNativeDelegate m_PlayerPostIssueOrderNativeDelegate;
        internal static List<Player_DoEmote> Player_DoEmoteHandlers;
        internal static List<Player_ProcessIssueOrder> Player_ProcessIssueOrderHandlers;
        internal static List<Player_SwapItem> Player_SwapItemHandlers;
        internal static List<PlayerPostIssueOrder> PlayerPostIssueOrderHandlers;
        private unsafe static EloBuddy.Native.AIHeroClient* self = EloBuddy.Native.ObjectManager.GetPlayer();

        public static  event Player_DoEmote OnEmote
        {
            add
            {
                Player_DoEmoteHandlers.Add(handler);
            }
            remove
            {
                Player_DoEmoteHandlers.Remove(handler);
            }
        }

        public static  event Player_ProcessIssueOrder OnIssueOrder
        {
            add
            {
                Player_ProcessIssueOrderHandlers.Add(handler);
            }
            remove
            {
                Player_ProcessIssueOrderHandlers.Remove(handler);
            }
        }

        public static  event PlayerPostIssueOrder OnPostIssueOrder
        {
            add
            {
                PlayerPostIssueOrderHandlers.Add(handler);
            }
            remove
            {
                PlayerPostIssueOrderHandlers.Remove(handler);
            }
        }

        public static  event Player_SwapItem OnSwapItem
        {
            add
            {
                Player_SwapItemHandlers.Add(handler);
            }
            remove
            {
                Player_SwapItemHandlers.Remove(handler);
            }
        }

        static unsafe Player()
        {
            uint* numPtr = EloBuddy.Native.GameObject.GetNetworkId((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) self);
            m_instance = new EloBuddy.AIHeroClient(*(EloBuddy.Native.GameObject.GetIndex((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) self)), numPtr[0], (EloBuddy.Native.GameObject*) self);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(Player.DomainUnloadEventHandler);
            Player_ProcessIssueOrderHandlers = new List<Player_ProcessIssueOrder>();
            m_Player_ProcessIssueOrderNativeDelegate = new OnPlayer_ProcessIssueOrderNativeDelegate(Player.OnPlayer_ProcessIssueOrderNative);
            m_Player_ProcessIssueOrderNative = Marshal.GetFunctionPointerForDelegate(m_Player_ProcessIssueOrderNativeDelegate);
            EloBuddy.Native.EventHandler<15,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,unsigned int,EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,bool),EloBuddy::Native::Obj_AI_Base *,unsigned int,EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,bool>.Add(EloBuddy.Native.EventHandler<15,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,unsigned int,EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,bool),EloBuddy::Native::Obj_AI_Base *,unsigned int,EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,bool>.GetInstance(), m_Player_ProcessIssueOrderNative.ToPointer());
            Player_SwapItemHandlers = new List<Player_SwapItem>();
            m_Player_SwapItemNativeDelegate = new OnPlayer_SwapItemNativeDelegate(Player.OnPlayer_SwapItemNative);
            m_Player_SwapItemNative = Marshal.GetFunctionPointerForDelegate(m_Player_SwapItemNativeDelegate);
            EloBuddy.Native.EventHandler<48,bool __cdecl(EloBuddy::Native::AIHeroClient *,unsigned int,unsigned int),EloBuddy::Native::AIHeroClient *,unsigned int,unsigned int>.Add(EloBuddy.Native.EventHandler<48,bool __cdecl(EloBuddy::Native::AIHeroClient *,unsigned int,unsigned int),EloBuddy::Native::AIHeroClient *,unsigned int,unsigned int>.GetInstance(), m_Player_SwapItemNative.ToPointer());
            Player_DoEmoteHandlers = new List<Player_DoEmote>();
            m_Player_DoEmoteNativeDelegate = new OnPlayer_DoEmoteNativeDelegate(Player.OnPlayer_DoEmoteNative);
            m_Player_DoEmoteNative = Marshal.GetFunctionPointerForDelegate(m_Player_DoEmoteNativeDelegate);
            EloBuddy.Native.EventHandler<72,bool __cdecl(EloBuddy::Native::AIHeroClient *,short),EloBuddy::Native::AIHeroClient *,short>.Add(EloBuddy.Native.EventHandler<72,bool __cdecl(EloBuddy::Native::AIHeroClient *,short),EloBuddy::Native::AIHeroClient *,short>.GetInstance(), m_Player_DoEmoteNative.ToPointer());
            PlayerPostIssueOrderHandlers = new List<PlayerPostIssueOrder>();
        }

        public static EloBuddy.SpellState CanUseSpell(EloBuddy.SpellSlot slot) => 
            m_instance.Spellbook.CanUseSpell(slot);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool CastSpell(EloBuddy.SpellSlot slot) => 
            m_instance.Spellbook.CastSpell(slot);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool CastSpell(EloBuddy.SpellSlot slot, EloBuddy.GameObject target) => 
            m_instance.Spellbook.CastSpell(slot, target);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool CastSpell(EloBuddy.SpellSlot slot, Vector3 position) => 
            m_instance.Spellbook.CastSpell(slot, position);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool CastSpell(EloBuddy.SpellSlot slot, [MarshalAs(UnmanagedType.U1)] bool triggerEvent) => 
            m_instance.Spellbook.CastSpell(slot, triggerEvent);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool CastSpell(EloBuddy.SpellSlot slot, EloBuddy.GameObject target, [MarshalAs(UnmanagedType.U1)] bool triggerEvent) => 
            m_instance.Spellbook.CastSpell(slot, target, triggerEvent);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool CastSpell(EloBuddy.SpellSlot slot, Vector3 startPosition, Vector3 endPosition) => 
            m_instance.Spellbook.CastSpell(slot, startPosition, endPosition);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool CastSpell(EloBuddy.SpellSlot slot, Vector3 position, [MarshalAs(UnmanagedType.U1)] bool triggerEvent) => 
            m_instance.Spellbook.CastSpell(slot, position, triggerEvent);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool CastSpell(EloBuddy.SpellSlot slot, Vector3 startPosition, Vector3 endPosition, [MarshalAs(UnmanagedType.U1)] bool triggerEvent) => 
            m_instance.Spellbook.CastSpell(slot, startPosition, endPosition, triggerEvent);

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool DoEmote(Emote emote) => 
            EloBuddy.Native.AIHeroClient.DoEmote((EloBuddy.Native.AIHeroClient modopt(IsConst)* modopt(IsConst) modopt(IsConst)) self, (short) emote);

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<15,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,unsigned int,EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,bool),EloBuddy::Native::Obj_AI_Base *,unsigned int,EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,bool>.Remove(EloBuddy.Native.EventHandler<15,bool __cdecl(EloBuddy::Native::Obj_AI_Base *,unsigned int,EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,bool),EloBuddy::Native::Obj_AI_Base *,unsigned int,EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,bool>.GetInstance(), m_Player_ProcessIssueOrderNative.ToPointer());
            EloBuddy.Native.EventHandler<48,bool __cdecl(EloBuddy::Native::AIHeroClient *,unsigned int,unsigned int),EloBuddy::Native::AIHeroClient *,unsigned int,unsigned int>.Remove(EloBuddy.Native.EventHandler<48,bool __cdecl(EloBuddy::Native::AIHeroClient *,unsigned int,unsigned int),EloBuddy::Native::AIHeroClient *,unsigned int,unsigned int>.GetInstance(), m_Player_SwapItemNative.ToPointer());
            EloBuddy.Native.EventHandler<72,bool __cdecl(EloBuddy::Native::AIHeroClient *,short),EloBuddy::Native::AIHeroClient *,short>.Remove(EloBuddy.Native.EventHandler<72,bool __cdecl(EloBuddy::Native::AIHeroClient *,short),EloBuddy::Native::AIHeroClient *,short>.GetInstance(), m_Player_DoEmoteNative.ToPointer());
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool DoMasteryBadge() => 
            ((EloBuddy.Native.MenuGUI.GetInstance() != null) && EloBuddy.Native.MenuGUI.DoMasteryBadge());

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool Equals(EloBuddy.GameObject o)
        {
            try
            {
                byte num = (byte) (m_instance.m_networkId == o.m_networkId);
                return (bool) num;
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

        public static void EvolveSpell(EloBuddy.SpellSlot slot)
        {
            m_instance.Spellbook.EvolveSpell(slot);
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool ForceIssueOrder(EloBuddy.GameObjectOrder order, Vector3 targetPos, [MarshalAs(UnmanagedType.U1)] bool triggerEvent)
        {
            Vector3f vectorf;
            return EloBuddy.Native.Obj_AI_Base.IssueOrder((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) self, EloBuddy.Native.Vector3f.{ctor}(&vectorf, targetPos.X, targetPos.Z, targetPos.Y), null, (EloBuddy.Native.GameObjectOrder) order, triggerEvent, true);
        }

        public static EloBuddy.BuffInstance GetBuff(string name) => 
            m_instance.GetBuff(name);

        public Vector3[] GetPath(Vector3 end) => 
            m_instance.GetPath(end);

        public Vector3[] GetPath(Vector3 start, Vector3 end) => 
            m_instance.GetPath(start, end);

        public Vector3[] GetPath(Vector3 end, [MarshalAs(UnmanagedType.U1)] bool smoothPath) => 
            m_instance.GetPath(end, smoothPath);

        public Vector3[] GetPath(Vector3 start, Vector3 end, [MarshalAs(UnmanagedType.U1)] bool smoothPath) => 
            m_instance.GetPath(start, end, smoothPath);

        public static EloBuddy.SpellDataInst GetSpell(EloBuddy.SpellSlot slot) => 
            m_instance.Spellbook.GetSpell(slot);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool HasBuff(string name) => 
            m_instance.HasBuff(name);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool HasBuffOfType(BuffType type) => 
            m_instance.HasBuffOfType(type);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool IssueOrder(EloBuddy.GameObjectOrder order, EloBuddy.GameObject targetUnit) => 
            IssueOrder(order, targetUnit, true);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool IssueOrder(EloBuddy.GameObjectOrder order, Vector3 targetPos) => 
            IssueOrder(order, targetPos, true);

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool IssueOrder(EloBuddy.GameObjectOrder order, EloBuddy.GameObject targetUnit, [MarshalAs(UnmanagedType.U1)] bool triggerEvent)
        {
            if (targetUnit != null)
            {
                EloBuddy.Native.GameObject* objPtr = EloBuddy.Native.ObjectManager.GetUnitByNetworkId(targetUnit.m_networkId);
                if (objPtr != null)
                {
                    Vector3f vectorf;
                    Vector3 position = targetUnit.Position;
                    Vector3 vector2 = targetUnit.Position;
                    Vector3 vector = targetUnit.Position;
                    return EloBuddy.Native.Obj_AI_Base.IssueOrder((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) self, EloBuddy.Native.Vector3f.{ctor}(&vectorf, vector.X, vector2.Z, position.Y), objPtr, (EloBuddy.Native.GameObjectOrder) order, triggerEvent, false);
                }
            }
            return false;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool IssueOrder(EloBuddy.GameObjectOrder order, Vector3 targetPos, [MarshalAs(UnmanagedType.U1)] bool triggerEvent)
        {
            Vector3f vectorf;
            if (order == EloBuddy.GameObjectOrder.AttackUnit)
            {
                return false;
            }
            return EloBuddy.Native.Obj_AI_Base.IssueOrder((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) self, EloBuddy.Native.Vector3f.{ctor}(&vectorf, targetPos.X, targetPos.Z, targetPos.Y), null, (EloBuddy.Native.GameObjectOrder) order, triggerEvent, false);
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool LevelSpell(EloBuddy.SpellSlot slot) => 
            m_instance.Spellbook.LevelSpell(slot);

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnPlayer_DoEmoteNative(EloBuddy.Native.AIHeroClient* A_0, short A_1)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                EloBuddy.AIHeroClient sender = (EloBuddy.AIHeroClient) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                PlayerDoEmoteEventArgs args = new PlayerDoEmoteEventArgs(sender, A_1);
                foreach (Player_DoEmote emote in Player_DoEmoteHandlers.ToArray())
                {
                    try
                    {
                        emote(sender, args);
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
        internal static unsafe bool OnPlayer_ProcessIssueOrderNative(EloBuddy.Native.Obj_AI_Base* A_0, uint A_1, Vector3f* A_2, EloBuddy.Native.GameObject* A_3, [MarshalAs(UnmanagedType.U1)] bool A_4)
        {
            Exception innerException = null;
            Player_ProcessIssueOrder[] orderArray2 = null;
            Player_ProcessIssueOrder order2 = null;
            bool flag = true;
            try
            {
                Vector3 targetPosition = new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_2), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_2), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_2));
                EloBuddy.Obj_AI_Base sender = (EloBuddy.Obj_AI_Base) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                EloBuddy.GameObject target = null;
                if (A_3 != null)
                {
                    target = (EloBuddy.AttackableUnit) ObjectManager.CreateObjectFromPointer(A_3);
                }
                PlayerIssueOrderEventArgs args = new PlayerIssueOrderEventArgs((EloBuddy.GameObjectOrder) A_1, targetPosition, target, A_4);
                orderArray2 = Player_ProcessIssueOrderHandlers.ToArray();
                int index = 0;
                while (true)
                {
                    if (index >= orderArray2.Length)
                    {
                        break;
                    }
                    order2 = orderArray2[index];
                    try
                    {
                        order2(sender, args);
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
                    foreach (PlayerPostIssueOrder order in PlayerPostIssueOrderHandlers.ToArray())
                    {
                        order(sender, args);
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

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnPlayer_SwapItemNative(EloBuddy.Native.AIHeroClient* A_0, uint A_1, uint A_2)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                EloBuddy.AIHeroClient sender = (EloBuddy.AIHeroClient) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                PlayerSwapItemEventArgs args = new PlayerSwapItemEventArgs(sender, (int) A_1, (int) A_2);
                foreach (Player_SwapItem item in Player_SwapItemHandlers.ToArray())
                {
                    try
                    {
                        item(sender, args);
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

        internal static void OnPlayerPostIssueOrderNative()
        {
        }

        public static void SetModel(string model)
        {
            m_instance.SetModel(model);
        }

        public static void SetSkin(string model, int skinId)
        {
            m_instance.SetModel(model);
            m_instance.SetSkinId(skinId);
        }

        public static void SetSkinId(int skinId)
        {
            m_instance.SetSkinId(skinId);
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool SwapItem(int sourceSlotId, int targetSlotId) => 
            EloBuddy.Native.HeroInventory.SwapItem(EloBuddy.Native.Obj_AI_Base.GetInventory((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) self), (uint) sourceSlotId, (uint) targetSlotId);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool UpdateChargeableSpell(EloBuddy.SpellSlot slot, Vector3 position, [MarshalAs(UnmanagedType.U1)] bool releaseCast) => 
            m_instance.Spellbook.UpdateChargeableSpell(slot, position, releaseCast);

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool UpdateChargeableSpell(EloBuddy.SpellSlot slot, Vector3 position, [MarshalAs(UnmanagedType.U1)] bool releaseCast, [MarshalAs(UnmanagedType.U1)] bool triggerEvent) => 
            m_instance.Spellbook.UpdateChargeableSpell(slot, position, releaseCast, triggerEvent);

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool UseObject(EloBuddy.GameObject obj)
        {
            uint networkId = obj.m_networkId;
            return EloBuddy.Native.Obj_AI_Base.UseObject((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) self, EloBuddy.Native.ObjectManager.GetUnitByNetworkId(networkId));
        }

        public bool HasTarget =>
            ((this.Target != null) ? ((bool) ((byte) 1)) : ((bool) ((byte) 0)));

        public static EloBuddy.AIHeroClient Instance =>
            m_instance;

        public string Model
        {
            get
            {
                CharacterDataStack* stackPtr = EloBuddy.Native.Obj_AI_Base.GetCharacterDataStack((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) base.GetPtr());
                if (stackPtr != null)
                {
                    basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsConst) modopt(IsConst) localPtr = EloBuddy.Native.CharacterDataStack.GetActiveModel(stackPtr);
                    return new string((0x10 > localPtr[20]) ? ((sbyte*) localPtr) : ((sbyte*) localPtr[0]));
                }
                return "Unknown";
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
                return 0;
            }
        }

        public static List<EloBuddy.SpellDataInst> Spells =>
            m_instance.Spellbook.Spells;

        public EloBuddy.GameObject Target
        {
            get
            {
                pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
                if (hudPtr != null)
                {
                    HudManager* managerPtr = EloBuddy.Native.pwHud.GetHudManager((pwHud modopt(IsConst)* modopt(IsConst) modopt(IsConst)) hudPtr);
                    if (managerPtr != null)
                    {
                        return ObjectManager.GetUnitByIndex((short) *(EloBuddy.Native.HudManager.GetTargetIndexId(managerPtr)));
                    }
                }
                return null;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnPlayer_DoEmoteNativeDelegate(EloBuddy.Native.AIHeroClient* A_0, short A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnPlayer_ProcessIssueOrderNativeDelegate(EloBuddy.Native.Obj_AI_Base* A_0, uint A_1, Vector3f* A_2, EloBuddy.Native.GameObject* A_3, [MarshalAs(UnmanagedType.U1)] bool A_4);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnPlayer_SwapItemNativeDelegate(EloBuddy.Native.AIHeroClient* A_0, uint A_1, uint A_2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnPlayerPostIssueOrderNativeDelegate();
    }
}

