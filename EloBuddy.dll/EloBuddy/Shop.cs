namespace EloBuddy
{
    using EloBuddy.Native;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Shop
    {
        internal static IntPtr m_ShopBuyItemNative = new IntPtr();
        internal static OnShopBuyItemNativeDelegate m_ShopBuyItemNativeDelegate;
        internal static IntPtr m_ShopCloseNative = new IntPtr();
        internal static OnShopCloseNativeDelegate m_ShopCloseNativeDelegate;
        internal static IntPtr m_ShopOpenNative = new IntPtr();
        internal static OnShopOpenNativeDelegate m_ShopOpenNativeDelegate;
        internal static IntPtr m_ShopSellItemNative = new IntPtr();
        internal static OnShopSellItemNativeDelegate m_ShopSellItemNativeDelegate;
        internal static IntPtr m_ShopUndoNative = new IntPtr();
        internal static OnShopUndoNativeDelegate m_ShopUndoNativeDelegate;
        internal static List<ShopBuyItem> ShopBuyItemHandlers;
        internal static List<ShopClose> ShopCloseHandlers;
        internal static List<ShopOpen> ShopOpenHandlers;
        internal static List<ShopSellItem> ShopSellItemHandlers;
        internal static List<ShopUndo> ShopUndoHandlers;

        public static  event ShopBuyItem OnBuyItem
        {
            add
            {
                ShopBuyItemHandlers.Add(handler);
            }
            remove
            {
                ShopBuyItemHandlers.Remove(handler);
            }
        }

        public static  event ShopClose OnClose
        {
            add
            {
                ShopCloseHandlers.Add(handler);
            }
            remove
            {
                ShopCloseHandlers.Remove(handler);
            }
        }

        public static  event ShopOpen OnOpen
        {
            add
            {
                ShopOpenHandlers.Add(handler);
            }
            remove
            {
                ShopOpenHandlers.Remove(handler);
            }
        }

        public static  event ShopSellItem OnSellItem
        {
            add
            {
                ShopSellItemHandlers.Add(handler);
            }
            remove
            {
                ShopSellItemHandlers.Remove(handler);
            }
        }

        public static  event ShopUndo OnUndo
        {
            add
            {
                ShopUndoHandlers.Add(handler);
            }
            remove
            {
                ShopUndoHandlers.Remove(handler);
            }
        }

        static Shop()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(Shop.DomainUnloadEventHandler);
            ShopBuyItemHandlers = new List<ShopBuyItem>();
            m_ShopBuyItemNativeDelegate = new OnShopBuyItemNativeDelegate(Shop.OnShopBuyItemNative);
            m_ShopBuyItemNative = Marshal.GetFunctionPointerForDelegate(m_ShopBuyItemNativeDelegate);
            EloBuddy.Native.EventHandler<27,bool __cdecl(EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *),EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *>.Add(EloBuddy.Native.EventHandler<27,bool __cdecl(EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *),EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *>.GetInstance(), m_ShopBuyItemNative.ToPointer());
            ShopSellItemHandlers = new List<ShopSellItem>();
            m_ShopSellItemNativeDelegate = new OnShopSellItemNativeDelegate(Shop.OnShopSellItemNative);
            m_ShopSellItemNative = Marshal.GetFunctionPointerForDelegate(m_ShopSellItemNativeDelegate);
            EloBuddy.Native.EventHandler<28,bool __cdecl(EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *),EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *>.Add(EloBuddy.Native.EventHandler<28,bool __cdecl(EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *),EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *>.GetInstance(), m_ShopSellItemNative.ToPointer());
            ShopOpenHandlers = new List<ShopOpen>();
            m_ShopOpenNativeDelegate = new OnShopOpenNativeDelegate(Shop.OnShopOpenNative);
            m_ShopOpenNative = Marshal.GetFunctionPointerForDelegate(m_ShopOpenNativeDelegate);
            EloBuddy.Native.EventHandler<52,bool __cdecl(void)>.Add(EloBuddy.Native.EventHandler<52,bool __cdecl(void)>.GetInstance(), m_ShopOpenNative.ToPointer());
            ShopCloseHandlers = new List<ShopClose>();
            m_ShopCloseNativeDelegate = new OnShopCloseNativeDelegate(Shop.OnShopCloseNative);
            m_ShopCloseNative = Marshal.GetFunctionPointerForDelegate(m_ShopCloseNativeDelegate);
            EloBuddy.Native.EventHandler<53,bool __cdecl(void)>.Add(EloBuddy.Native.EventHandler<53,bool __cdecl(void)>.GetInstance(), m_ShopCloseNative.ToPointer());
            ShopUndoHandlers = new List<ShopUndo>();
            m_ShopUndoNativeDelegate = new OnShopUndoNativeDelegate(Shop.OnShopUndoNative);
            m_ShopUndoNative = Marshal.GetFunctionPointerForDelegate(m_ShopUndoNativeDelegate);
            EloBuddy.Native.EventHandler<54,bool __cdecl(void)>.Add(EloBuddy.Native.EventHandler<54,bool __cdecl(void)>.GetInstance(), m_ShopUndoNative.ToPointer());
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool BuyItem(ItemId item)
        {
            byte num;
            EloBuddy.Native.AIHeroClient* clientPtr = EloBuddy.Native.ObjectManager.GetPlayer();
            if (clientPtr != null)
            {
                num = EloBuddy.Native.HeroInventory.BuyItem(EloBuddy.Native.Obj_AI_Base.GetInventory((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) clientPtr), (int) item);
            }
            else
            {
                num = 0;
            }
            return (bool) num;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool BuyItem(int itemId)
        {
            EloBuddy.Native.AIHeroClient* clientPtr = EloBuddy.Native.ObjectManager.GetPlayer();
            return ((clientPtr != null) && EloBuddy.Native.HeroInventory.BuyItem(EloBuddy.Native.Obj_AI_Base.GetInventory((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) clientPtr), itemId));
        }

        public static void Close()
        {
            EloBuddy.Native.FoundryItemShop.CloseShop();
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<27,bool __cdecl(EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *),EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *>.Remove(EloBuddy.Native.EventHandler<27,bool __cdecl(EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *),EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *>.GetInstance(), m_ShopBuyItemNative.ToPointer());
            EloBuddy.Native.EventHandler<28,bool __cdecl(EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *),EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *>.Remove(EloBuddy.Native.EventHandler<28,bool __cdecl(EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *),EloBuddy::Native::AIHeroClient *,int,EloBuddy::Native::ItemNode *>.GetInstance(), m_ShopSellItemNative.ToPointer());
            EloBuddy.Native.EventHandler<52,bool __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<52,bool __cdecl(void)>.GetInstance(), m_ShopOpenNative.ToPointer());
            EloBuddy.Native.EventHandler<53,bool __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<53,bool __cdecl(void)>.GetInstance(), m_ShopCloseNative.ToPointer());
            EloBuddy.Native.EventHandler<54,bool __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<54,bool __cdecl(void)>.GetInstance(), m_ShopUndoNative.ToPointer());
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnShopBuyItemNative(EloBuddy.Native.AIHeroClient* A_0, int A_1, ItemNode* A_2)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                List<int> list = new List<int>();
                EloBuddy.AIHeroClient sender = (EloBuddy.AIHeroClient) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                int* numPtr2 = EloBuddy.Native.ItemNode.GetMaxStacks(A_2);
                int* numPtr = EloBuddy.Native.ItemNode.GetPrice(A_2);
                ShopActionEventArgs args = new ShopActionEventArgs(sender, A_1, numPtr[0], numPtr2[0], new string(EloBuddy.Native.ItemNode.GetName(A_2)), list.ToArray());
                foreach (ShopBuyItem item in ShopBuyItemHandlers.ToArray())
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

        [return: MarshalAs(UnmanagedType.U1)]
        internal static bool OnShopCloseNative()
        {
            bool flag = true;
            try
            {
                ShopCloseEventArgs args = new ShopCloseEventArgs();
                foreach (ShopClose close in ShopCloseHandlers.ToArray())
                {
                    close(args);
                    flag = args.Process && flag;
                }
                return flag;
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
            return flag;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static bool OnShopOpenNative()
        {
            bool flag = true;
            try
            {
                ShopOpenEventArgs args = new ShopOpenEventArgs();
                foreach (ShopOpen open in ShopOpenHandlers.ToArray())
                {
                    open(args);
                    flag = args.Process && flag;
                }
                return flag;
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
            return flag;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnShopSellItemNative(EloBuddy.Native.AIHeroClient* A_0, int A_1, ItemNode* A_2)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                List<int> list = new List<int>();
                EloBuddy.AIHeroClient sender = (EloBuddy.AIHeroClient) ObjectManager.CreateObjectFromPointer((EloBuddy.Native.GameObject*) A_0);
                int* numPtr2 = EloBuddy.Native.ItemNode.GetMaxStacks(A_2);
                int* numPtr = EloBuddy.Native.ItemNode.GetPrice(A_2);
                ShopActionEventArgs args = new ShopActionEventArgs(sender, A_1, numPtr[0], numPtr2[0], new string(EloBuddy.Native.ItemNode.GetName(A_2)), list.ToArray());
                foreach (ShopSellItem item in ShopSellItemHandlers.ToArray())
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

        [return: MarshalAs(UnmanagedType.U1)]
        internal static bool OnShopUndoNative()
        {
            bool flag = true;
            try
            {
                ShopUndoPurchaseEventArgs args = new ShopUndoPurchaseEventArgs();
                foreach (ShopUndo undo in ShopUndoHandlers.ToArray())
                {
                    undo(args);
                    flag = args.Process && flag;
                }
                return flag;
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
            return flag;
        }

        public static void Open()
        {
            EloBuddy.Native.FoundryItemShop.OpenShop();
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool SellItem(EloBuddy.SpellSlot slot)
        {
            throw new Exception("InvalidSlotException - Items only");
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool SellItem(int slot)
        {
            EloBuddy.Native.AIHeroClient* clientPtr = EloBuddy.Native.ObjectManager.GetPlayer();
            return ((clientPtr != null) && EloBuddy.Native.HeroInventory.SellItem(EloBuddy.Native.Obj_AI_Base.GetInventory((EloBuddy.Native.Obj_AI_Base* modopt(IsConst) modopt(IsConst)) clientPtr), slot));
        }

        public static void UndoPurchase()
        {
            EloBuddy.Native.FoundryItemShop.UndoPurchase();
        }

        public static bool CanShop
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AIHeroClient* clientPtr = EloBuddy.Native.ObjectManager.GetPlayer();
                return ((clientPtr != null) && EloBuddy.Native.AIHeroClient.Virtual_CanShop(clientPtr));
            }
        }

        public static bool IsOpen
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                FoundryItemShop* shopPtr = EloBuddy.Native.FoundryItemShop.GetInstance();
                if (shopPtr != null)
                {
                    return *(EloBuddy.Native.FoundryItemShop.GetIsOpen(shopPtr));
                }
                return false;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnShopBuyItemNativeDelegate(EloBuddy.Native.AIHeroClient* A_0, int A_1, ItemNode* A_2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool OnShopCloseNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool OnShopOpenNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnShopSellItemNativeDelegate(EloBuddy.Native.AIHeroClient* A_0, int A_1, ItemNode* A_2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool OnShopUndoNativeDelegate();
    }
}

