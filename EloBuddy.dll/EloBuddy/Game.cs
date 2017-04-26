namespace EloBuddy
{
    using EloBuddy.Native;
    using EloBuddy.Networking;
    using SharpDX;
    using std;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Game
    {
        internal static List<GameAfk> GameAfkHandlers;
        internal static List<GameDisconnect> GameDisconnectHandlers;
        internal static List<GameEnd> GameEndHandlers;
        internal static List<GameLoad> GameLoadHandlers;
        internal static List<GameNotify> GameNotifyHandlers;
        internal static List<GamePostTick> GamePostTickHandlers;
        internal static List<GamePreTick> GamePreTickHandlers;
        internal static List<GameProcessPacket> GameProcessPacketHandlers;
        internal static List<GameSendPacket> GameSendPacketHandlers;
        internal static List<GameTick> GameTickHandlers;
        internal static List<GameUpdate> GameUpdateHandlers;
        internal static List<GameWndProc> GameWndProcHandlers;
        internal static Dictionary<short, List<PacketCallback>> m_callBackDictionary = new Dictionary<short, List<PacketCallback>>();
        internal static IntPtr m_GameAfkNative = new IntPtr();
        internal static OnGameAfkNativeDelegate m_GameAfkNativeDelegate;
        internal static IntPtr m_GameDisconnectNative = new IntPtr();
        internal static OnGameDisconnectNativeDelegate m_GameDisconnectNativeDelegate;
        internal static IntPtr m_GameEndNative = new IntPtr();
        internal static OnGameEndNativeDelegate m_GameEndNativeDelegate;
        internal static IntPtr m_GameLoadNative = new IntPtr();
        internal static OnGameLoadNativeDelegate m_GameLoadNativeDelegate;
        internal static IntPtr m_GameNotifyNative = new IntPtr();
        internal static OnGameNotifyNativeDelegate m_GameNotifyNativeDelegate;
        internal static IntPtr m_GamePostTickNative = new IntPtr();
        internal static OnGamePostTickNativeDelegate m_GamePostTickNativeDelegate;
        internal static IntPtr m_GamePreTickNative = new IntPtr();
        internal static OnGamePreTickNativeDelegate m_GamePreTickNativeDelegate;
        internal static IntPtr m_GameProcessPacketNative = new IntPtr();
        internal static OnGameProcessPacketNativeDelegate m_GameProcessPacketNativeDelegate;
        internal static IntPtr m_GameSendPacketNative = new IntPtr();
        internal static OnGameSendPacketNativeDelegate m_GameSendPacketNativeDelegate;
        internal static IntPtr m_GameTickNative = new IntPtr();
        internal static OnGameTickNativeDelegate m_GameTickNativeDelegate;
        internal static IntPtr m_GameUpdateNative = new IntPtr();
        internal static OnGameUpdateNativeDelegate m_GameUpdateNativeDelegate;
        internal static IntPtr m_GameWndProcNative = new IntPtr();
        internal static OnGameWndProcNativeDelegate m_GameWndProcNativeDelegate;
        internal static int m_lastTick;

        public static  event GameAfk OnAfk
        {
            add
            {
                GameAfkHandlers.Add(handler);
            }
            remove
            {
                GameAfkHandlers.Remove(handler);
            }
        }

        public static  event GameDisconnect OnDisconnect
        {
            add
            {
                GameDisconnectHandlers.Add(handler);
            }
            remove
            {
                GameDisconnectHandlers.Remove(handler);
            }
        }

        public static  event GameEnd OnEnd
        {
            add
            {
                GameEndHandlers.Add(handler);
            }
            remove
            {
                GameEndHandlers.Remove(handler);
            }
        }

        public static  event GameLoad OnLoad
        {
            add
            {
                GameLoadHandlers.Add(handler);
            }
            remove
            {
                GameLoadHandlers.Remove(handler);
            }
        }

        public static  event GameNotify OnNotify
        {
            add
            {
                GameNotifyHandlers.Add(handler);
            }
            remove
            {
                GameNotifyHandlers.Remove(handler);
            }
        }

        public static  event GamePostTick OnPostTick
        {
            add
            {
                GamePostTickHandlers.Add(handler);
            }
            remove
            {
                GamePostTickHandlers.Remove(handler);
            }
        }

        public static  event GamePreTick OnPreTick
        {
            add
            {
                GamePreTickHandlers.Add(handler);
            }
            remove
            {
                GamePreTickHandlers.Remove(handler);
            }
        }

        public static  event GameProcessPacket OnProcessPacket
        {
            add
            {
                GameProcessPacketHandlers.Add(handler);
            }
            remove
            {
                GameProcessPacketHandlers.Remove(handler);
            }
        }

        public static  event GameSendPacket OnSendPacket
        {
            add
            {
                GameSendPacketHandlers.Add(handler);
            }
            remove
            {
                GameSendPacketHandlers.Remove(handler);
            }
        }

        public static  event GameTick OnTick
        {
            add
            {
                GameTickHandlers.Add(handler);
            }
            remove
            {
                GameTickHandlers.Remove(handler);
            }
        }

        public static  event GameUpdate OnUpdate
        {
            add
            {
                GameUpdateHandlers.Add(handler);
            }
            remove
            {
                GameUpdateHandlers.Remove(handler);
            }
        }

        public static  event GameWndProc OnWndProc
        {
            add
            {
                GameWndProcHandlers.Add(handler);
            }
            remove
            {
                GameWndProcHandlers.Remove(handler);
            }
        }

        static Game()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(EloBuddy.Game.DomainUnloadEventHandler);
            GameWndProcHandlers = new List<GameWndProc>();
            m_GameWndProcNativeDelegate = new OnGameWndProcNativeDelegate(EloBuddy.Game.OnGameWndProcNative);
            m_GameWndProcNative = Marshal.GetFunctionPointerForDelegate(m_GameWndProcNativeDelegate);
            EloBuddy.Native.EventHandler<1,bool __cdecl(HWND__ *,unsigned int,unsigned int,long),HWND__ *,unsigned int,unsigned int,long>.Add(EloBuddy.Native.EventHandler<1,bool __cdecl(HWND__ *,unsigned int,unsigned int,long),HWND__ *,unsigned int,unsigned int,long>.GetInstance(), m_GameWndProcNative.ToPointer());
            GameUpdateHandlers = new List<GameUpdate>();
            m_GameUpdateNativeDelegate = new OnGameUpdateNativeDelegate(EloBuddy.Game.OnGameUpdateNative);
            m_GameUpdateNative = Marshal.GetFunctionPointerForDelegate(m_GameUpdateNativeDelegate);
            EloBuddy.Native.EventHandler<2,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<2,void __cdecl(void)>.GetInstance(), m_GameUpdateNative.ToPointer());
            GameEndHandlers = new List<GameEnd>();
            m_GameEndNativeDelegate = new OnGameEndNativeDelegate(EloBuddy.Game.OnGameEndNative);
            m_GameEndNative = Marshal.GetFunctionPointerForDelegate(m_GameEndNativeDelegate);
            EloBuddy.Native.EventHandler<4,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<4,void __cdecl(void)>.GetInstance(), m_GameEndNative.ToPointer());
            GameLoadHandlers = new List<GameLoad>();
            m_GameLoadNativeDelegate = new OnGameLoadNativeDelegate(EloBuddy.Game.OnGameLoadNative);
            m_GameLoadNative = Marshal.GetFunctionPointerForDelegate(m_GameLoadNativeDelegate);
            EloBuddy.Native.EventHandler<26,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<26,void __cdecl(void)>.GetInstance(), m_GameLoadNative.ToPointer());
            GameSendPacketHandlers = new List<GameSendPacket>();
            m_GameSendPacketNativeDelegate = new OnGameSendPacketNativeDelegate(EloBuddy.Game.OnGameSendPacketNative);
            m_GameSendPacketNative = Marshal.GetFunctionPointerForDelegate(m_GameSendPacketNativeDelegate);
            EloBuddy.Native.EventHandler<19,bool __cdecl(EloBuddy::Native::C2S_ENetPacket *,unsigned int,unsigned int,unsigned long),EloBuddy::Native::C2S_ENetPacket *,unsigned int,unsigned int,unsigned long>.Add(EloBuddy.Native.EventHandler<19,bool __cdecl(EloBuddy::Native::C2S_ENetPacket *,unsigned int,unsigned int,unsigned long),EloBuddy::Native::C2S_ENetPacket *,unsigned int,unsigned int,unsigned long>.GetInstance(), m_GameSendPacketNative.ToPointer());
            GameProcessPacketHandlers = new List<GameProcessPacket>();
            m_GameProcessPacketNativeDelegate = new OnGameProcessPacketNativeDelegate(EloBuddy.Game.OnGameProcessPacketNative);
            m_GameProcessPacketNative = Marshal.GetFunctionPointerForDelegate(m_GameProcessPacketNativeDelegate);
            EloBuddy.Native.EventHandler<20,bool __cdecl(EloBuddy::Native::S2C_ENetPacket *,unsigned int,unsigned int,unsigned long),EloBuddy::Native::S2C_ENetPacket *,unsigned int,unsigned int,unsigned long>.Add(EloBuddy.Native.EventHandler<20,bool __cdecl(EloBuddy::Native::S2C_ENetPacket *,unsigned int,unsigned int,unsigned long),EloBuddy::Native::S2C_ENetPacket *,unsigned int,unsigned int,unsigned long>.GetInstance(), m_GameProcessPacketNative.ToPointer());
            GamePreTickHandlers = new List<GamePreTick>();
            m_GamePreTickNativeDelegate = new OnGamePreTickNativeDelegate(EloBuddy.Game.OnGamePreTickNative);
            m_GamePreTickNative = Marshal.GetFunctionPointerForDelegate(m_GamePreTickNativeDelegate);
            EloBuddy.Native.EventHandler<31,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<31,void __cdecl(void)>.GetInstance(), m_GamePreTickNative.ToPointer());
            GameTickHandlers = new List<GameTick>();
            m_GameTickNativeDelegate = new OnGameTickNativeDelegate(EloBuddy.Game.OnGameTickNative);
            m_GameTickNative = Marshal.GetFunctionPointerForDelegate(m_GameTickNativeDelegate);
            EloBuddy.Native.EventHandler<32,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<32,void __cdecl(void)>.GetInstance(), m_GameTickNative.ToPointer());
            GamePostTickHandlers = new List<GamePostTick>();
            m_GamePostTickNativeDelegate = new OnGamePostTickNativeDelegate(EloBuddy.Game.OnGamePostTickNative);
            m_GamePostTickNative = Marshal.GetFunctionPointerForDelegate(m_GamePostTickNativeDelegate);
            EloBuddy.Native.EventHandler<33,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<33,void __cdecl(void)>.GetInstance(), m_GamePostTickNative.ToPointer());
            GameAfkHandlers = new List<GameAfk>();
            m_GameAfkNativeDelegate = new OnGameAfkNativeDelegate(EloBuddy.Game.OnGameAfkNative);
            m_GameAfkNative = Marshal.GetFunctionPointerForDelegate(m_GameAfkNativeDelegate);
            EloBuddy.Native.EventHandler<34,bool __cdecl(void)>.Add(EloBuddy.Native.EventHandler<34,bool __cdecl(void)>.GetInstance(), m_GameAfkNative.ToPointer());
            GameDisconnectHandlers = new List<GameDisconnect>();
            m_GameDisconnectNativeDelegate = new OnGameDisconnectNativeDelegate(EloBuddy.Game.OnGameDisconnectNative);
            m_GameDisconnectNative = Marshal.GetFunctionPointerForDelegate(m_GameDisconnectNativeDelegate);
            EloBuddy.Native.EventHandler<35,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<35,void __cdecl(void)>.GetInstance(), m_GameDisconnectNative.ToPointer());
            GameNotifyHandlers = new List<GameNotify>();
            m_GameNotifyNativeDelegate = new OnGameNotifyNativeDelegate(EloBuddy.Game.OnGameNotifyNative);
            m_GameNotifyNative = Marshal.GetFunctionPointerForDelegate(m_GameNotifyNativeDelegate);
            EloBuddy.Native.EventHandler<37,void __cdecl(unsigned int,int),unsigned int,int>.Add(EloBuddy.Native.EventHandler<37,void __cdecl(unsigned int,int),unsigned int,int>.GetInstance(), m_GameNotifyNative.ToPointer());
        }

        public static void AddPacketCallback(short opCode, Action<GamePacket> action)
        {
            if (!m_callBackDictionary.ContainsKey(opCode))
            {
                m_callBackDictionary.Add(opCode, new List<PacketCallback>());
            }
            m_callBackDictionary[opCode].Add(new PacketCallback(PacketCallbackType.BothWays, action));
        }

        public static void AddPacketCallback(short opCode, Action<GamePacket> action, PacketCallbackType callbackType)
        {
            if (!m_callBackDictionary.ContainsKey(opCode))
            {
                m_callBackDictionary.Add(opCode, new List<PacketCallback>());
            }
            m_callBackDictionary[opCode].Add(new PacketCallback(callbackType, action));
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<1,bool __cdecl(HWND__ *,unsigned int,unsigned int,long),HWND__ *,unsigned int,unsigned int,long>.Remove(EloBuddy.Native.EventHandler<1,bool __cdecl(HWND__ *,unsigned int,unsigned int,long),HWND__ *,unsigned int,unsigned int,long>.GetInstance(), m_GameWndProcNative.ToPointer());
            EloBuddy.Native.EventHandler<2,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<2,void __cdecl(void)>.GetInstance(), m_GameUpdateNative.ToPointer());
            EloBuddy.Native.EventHandler<4,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<4,void __cdecl(void)>.GetInstance(), m_GameEndNative.ToPointer());
            EloBuddy.Native.EventHandler<26,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<26,void __cdecl(void)>.GetInstance(), m_GameLoadNative.ToPointer());
            EloBuddy.Native.EventHandler<19,bool __cdecl(EloBuddy::Native::C2S_ENetPacket *,unsigned int,unsigned int,unsigned long),EloBuddy::Native::C2S_ENetPacket *,unsigned int,unsigned int,unsigned long>.Remove(EloBuddy.Native.EventHandler<19,bool __cdecl(EloBuddy::Native::C2S_ENetPacket *,unsigned int,unsigned int,unsigned long),EloBuddy::Native::C2S_ENetPacket *,unsigned int,unsigned int,unsigned long>.GetInstance(), m_GameSendPacketNative.ToPointer());
            EloBuddy.Native.EventHandler<20,bool __cdecl(EloBuddy::Native::S2C_ENetPacket *,unsigned int,unsigned int,unsigned long),EloBuddy::Native::S2C_ENetPacket *,unsigned int,unsigned int,unsigned long>.Remove(EloBuddy.Native.EventHandler<20,bool __cdecl(EloBuddy::Native::S2C_ENetPacket *,unsigned int,unsigned int,unsigned long),EloBuddy::Native::S2C_ENetPacket *,unsigned int,unsigned int,unsigned long>.GetInstance(), m_GameProcessPacketNative.ToPointer());
            EloBuddy.Native.EventHandler<31,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<31,void __cdecl(void)>.GetInstance(), m_GamePreTickNative.ToPointer());
            EloBuddy.Native.EventHandler<32,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<32,void __cdecl(void)>.GetInstance(), m_GameTickNative.ToPointer());
            EloBuddy.Native.EventHandler<33,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<33,void __cdecl(void)>.GetInstance(), m_GamePostTickNative.ToPointer());
            EloBuddy.Native.EventHandler<34,bool __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<34,bool __cdecl(void)>.GetInstance(), m_GameAfkNative.ToPointer());
            EloBuddy.Native.EventHandler<35,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<35,void __cdecl(void)>.GetInstance(), m_GameDisconnectNative.ToPointer());
            EloBuddy.Native.EventHandler<37,void __cdecl(unsigned int,int),unsigned int,int>.Remove(EloBuddy.Native.EventHandler<37,void __cdecl(unsigned int,int),unsigned int,int>.GetInstance(), m_GameNotifyNative.ToPointer());
        }

        public static void Drop()
        {
            EloBuddy.Native.Game.FromBehind_LeagueSharp_Sucks();
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static bool LuaDoString(string lua) => 
            false;

        [return: MarshalAs(UnmanagedType.U1)]
        internal static bool OnGameAfkNative()
        {
            Exception innerException = null;
            GameAfk[] afkArray = null;
            GameAfk afk = null;
            bool flag = false;
            try
            {
                GameAfkEventArgs args = new GameAfkEventArgs();
                afkArray = GameAfkHandlers.ToArray();
                int index = 0;
                while (true)
                {
                    if (index >= afkArray.Length)
                    {
                        break;
                    }
                    afk = afkArray[index];
                    try
                    {
                        afk(args);
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
                flag = args.Process || flag;
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

        internal static void OnGameDisconnectNative()
        {
            Exception innerException = null;
            try
            {
                foreach (GameDisconnect disconnect in GameDisconnectHandlers.ToArray())
                {
                    try
                    {
                        disconnect(EventArgs.Empty);
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

        internal static void OnGameEndNative(int A_0)
        {
            Exception innerException = null;
            try
            {
                foreach (GameEnd end in GameEndHandlers.ToArray())
                {
                    try
                    {
                        end(new GameEndEventArgs(A_0));
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

        internal static void OnGameLoadNative()
        {
            Exception innerException = null;
            try
            {
                foreach (GameLoad load in GameLoadHandlers.ToArray())
                {
                    try
                    {
                        load(EventArgs.Empty);
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

        internal static void OnGameNotifyNative(uint A_0, int A_1)
        {
            Exception innerException = null;
            try
            {
                GameNotifyEventArgs args = new GameNotifyEventArgs((GameEventId) A_1, A_0);
                foreach (GameNotify notify in GameNotifyHandlers.ToArray())
                {
                    try
                    {
                        notify(args);
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

        internal static void OnGamePostTickNative()
        {
            Exception innerException = null;
            try
            {
                foreach (GamePostTick tick in GamePostTickHandlers.ToArray())
                {
                    try
                    {
                        tick(EventArgs.Empty);
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

        internal static void OnGamePreTickNative()
        {
            Exception innerException = null;
            try
            {
                foreach (GamePreTick tick in GamePreTickHandlers.ToArray())
                {
                    try
                    {
                        tick(EventArgs.Empty);
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
        internal static unsafe bool OnGameProcessPacketNative(S2C_ENetPacket* A_0, uint A_1, uint A_2, uint modopt(IsLong) A_3)
        {
            Exception innerException = null;
            EloBuddy.GamePacketEventArgs args = null;
            bool flag = true;
            try
            {
                byte num = *(EloBuddy.Native.S2C_ENetPacket.GetSize(A_0));
                if ((num <= 0xfff) && (num >= 10))
                {
                    short* numPtr = EloBuddy.Native.S2C_ENetPacket.GetCommand(A_0);
                    byte[] destination = new byte[num];
                    IntPtr source = new IntPtr((void*) numPtr);
                    IntPtr ptr5 = source;
                    Marshal.Copy(source, destination, 4, num - 4);
                    int length = num - 10;
                    byte[] buffer2 = new byte[length];
                    IntPtr ptr = new IntPtr(EloBuddy.Native.S2C_ENetPacket.GetData(A_0));
                    IntPtr ptr4 = ptr;
                    Marshal.Copy(ptr, buffer2, 0, length);
                    Array.Copy(BitConverter.GetBytes((int) A_3), destination, 4);
                    if (m_callBackDictionary.ContainsKey(numPtr[0]))
                    {
                        foreach (PacketCallback callback in m_callBackDictionary[numPtr[0]].ToArray())
                        {
                            PacketCallbackType callbackType = callback.m_callbackType;
                            if ((*(((int*) &callbackType)) == 2) || (*(((int*) &callbackType)) == 0))
                            {
                                callback.m_action(new GamePacket(destination));
                            }
                        }
                    }
                    IntPtr hashAlgorithm = new IntPtr((int) A_3);
                    args = new EloBuddy.GamePacketEventArgs(*(EloBuddy.Native.S2C_ENetPacket.GetCommand(A_0)), 0, buffer2, destination, hashAlgorithm, (PacketChannel) A_1, (PacketProtocolFlags) A_2);
                    foreach (GameProcessPacket packet in GameProcessPacketHandlers.ToArray())
                    {
                        try
                        {
                            packet(args);
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
                byte num5 = 1;
                return (bool) num5;
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
        internal static unsafe bool OnGameSendPacketNative(C2S_ENetPacket* A_0, uint A_1, uint A_2, uint modopt(IsLong) A_3)
        {
            Exception innerException = null;
            EloBuddy.GamePacketEventArgs args = null;
            bool flag = true;
            try
            {
                byte num = *(EloBuddy.Native.C2S_ENetPacket.GetSize(A_0));
                if ((num <= 0xfff) && (num >= 10))
                {
                    short* numPtr = EloBuddy.Native.C2S_ENetPacket.GetCommand(A_0);
                    byte[] destination = new byte[num];
                    IntPtr source = new IntPtr((void*) numPtr);
                    IntPtr ptr5 = source;
                    Marshal.Copy(source, destination, 4, num - 4);
                    int length = num - 10;
                    byte[] buffer2 = new byte[length];
                    IntPtr ptr = new IntPtr(EloBuddy.Native.C2S_ENetPacket.GetData(A_0));
                    IntPtr ptr4 = ptr;
                    Marshal.Copy(ptr, buffer2, 0, length);
                    Array.Copy(BitConverter.GetBytes((int) A_3), destination, 4);
                    if (m_callBackDictionary.ContainsKey(numPtr[0]))
                    {
                        foreach (PacketCallback callback in m_callBackDictionary[numPtr[0]].ToArray())
                        {
                            PacketCallbackType callbackType = callback.m_callbackType;
                            if ((*(((int*) &callbackType)) == 2) || (*(((int*) &callbackType)) == 0))
                            {
                                callback.m_action(new GamePacket(destination));
                            }
                        }
                    }
                    IntPtr hashAlgorithm = new IntPtr((int) A_3);
                    uint* numPtr2 = EloBuddy.Native.C2S_ENetPacket.GetNetworkId(A_0);
                    args = new EloBuddy.GamePacketEventArgs(*(EloBuddy.Native.C2S_ENetPacket.GetCommand(A_0)), numPtr2[0], buffer2, destination, hashAlgorithm, (PacketChannel) A_1, (PacketProtocolFlags) A_2);
                    foreach (GameSendPacket packet in GameSendPacketHandlers.ToArray())
                    {
                        try
                        {
                            packet(args);
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
                byte num5 = 1;
                return (bool) num5;
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

        internal static void OnGameTickNative()
        {
            Exception innerException = null;
            try
            {
                foreach (GameTick tick in GameTickHandlers.ToArray())
                {
                    try
                    {
                        tick(EventArgs.Empty);
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

        internal static void OnGameUpdateNative()
        {
            Exception innerException = null;
            try
            {
                foreach (GameUpdate update in GameUpdateHandlers.ToArray())
                {
                    try
                    {
                        update(EventArgs.Empty);
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
        internal static unsafe bool OnGameWndProcNative(HWND__* A_0, uint A_1, uint A_2, int modopt(IsLong) A_3)
        {
            Exception innerException = null;
            GameWndProc[] procArray = null;
            GameWndProc proc = null;
            bool flag = true;
            try
            {
                WndEventArgs args = new WndEventArgs(A_0, A_1, A_2, A_3);
                procArray = GameWndProcHandlers.ToArray();
                int index = 0;
                while (true)
                {
                    if (index >= procArray.Length)
                    {
                        break;
                    }
                    proc = procArray[index];
                    try
                    {
                        proc(args);
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
                flag = args.Process && flag;
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

        public static void ProcessPacket(byte[] packetData, PacketChannel channel)
        {
            ProcessPacket(packetData, channel, true);
        }

        public static unsafe void ProcessPacket(byte[] packetData, PacketChannel channel, [MarshalAs(UnmanagedType.U1)] bool triggerEvent)
        {
            if (EloBuddy.Native.ClientNode.GetInstance() != null)
            {
                int length = packetData.Length;
                if (length > 10)
                {
                    byte* numPtr = new[]((uint) length);
                    int index = 0;
                    if (0 < packetData.Length)
                    {
                        do
                        {
                            index[(int) numPtr] = packetData[index];
                            index++;
                        }
                        while (index < packetData.Length);
                    }
                    int num3 = BitConverter.ToInt32(packetData, 0);
                    EloBuddy.Native.ClientNode.ProcessClientPacket(numPtr, (uint modopt(IsLong)) packetData.Length, num3, (ENETCHANNEL) channel, triggerEvent);
                    delete((void*) numPtr);
                }
            }
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool QuitGame()
        {
            EloBuddy.Native.Game* gamePtr = EloBuddy.Native.Game.GetInstance();
            if (gamePtr != null)
            {
                EloBuddy.Native.Game.QuitGame(gamePtr);
            }
            try
            {
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void SendPacket(byte[] packetData, PacketChannel channel, PacketProtocolFlags flag)
        {
            SendPacket(packetData, channel, flag, true);
        }

        public static unsafe void SendPacket(byte[] packetData, PacketChannel channel, PacketProtocolFlags flag, [MarshalAs(UnmanagedType.U1)] bool triggerEvent)
        {
            if (EloBuddy.Native.NetClient.Get() != null)
            {
                int length = packetData.Length;
                if (length > 10)
                {
                    byte* numPtr = new[]((uint) length);
                    int index = 0;
                    if (0 < packetData.Length)
                    {
                        do
                        {
                            index[(int) numPtr] = packetData[index];
                            index++;
                        }
                        while (index < packetData.Length);
                    }
                    int num3 = BitConverter.ToInt32(packetData, 0);
                    EloBuddy.Native.NetClient.SendToServer(numPtr, packetData.Length, (uint modopt(IsLong)) num3, (ENETCHANNEL) channel, (ESENDPROTOCOL) flag, triggerEvent);
                    delete((void*) numPtr);
                }
            }
        }

        public static Vector3 ActiveCursorPos
        {
            get
            {
                pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
                if (hudPtr != null)
                {
                    HudManager* managerPtr = EloBuddy.Native.pwHud.GetHudManager((pwHud modopt(IsConst)* modopt(IsConst) modopt(IsConst)) hudPtr);
                    if (managerPtr != null)
                    {
                        Vector3f* vectorfPtr = EloBuddy.Native.HudManager.GetActiveVirtualCursorPos(managerPtr);
                        return new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                    }
                }
                return Vector3.Zero;
            }
        }

        public static string BuildDate =>
            new string(EloBuddy.Native.BuildInfo.GetBuildDate());

        public static string BuildTime =>
            new string(EloBuddy.Native.BuildInfo.GetBuildTime());

        public static string BuildType =>
            new string(EloBuddy.Native.BuildInfo.GetBuildType());

        public static Vector3 CursorPos
        {
            get
            {
                pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
                if (hudPtr != null)
                {
                    HudManager* managerPtr = EloBuddy.Native.pwHud.GetHudManager((pwHud modopt(IsConst)* modopt(IsConst) modopt(IsConst)) hudPtr);
                    if (managerPtr != null)
                    {
                        Vector3f* vectorfPtr = EloBuddy.Native.HudManager.GetVirtualCursorPos(managerPtr);
                        return new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
                    }
                }
                return Vector3.Zero;
            }
        }

        public static Vector2 CursorPos2D
        {
            get
            {
                pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
                if (hudPtr != null)
                {
                    HudManager* managerPtr = EloBuddy.Native.pwHud.GetHudManager((pwHud modopt(IsConst)* modopt(IsConst) modopt(IsConst)) hudPtr);
                    if (managerPtr != null)
                    {
                        Vector3f vectorf;
                        EloBuddy.Native.HudManager.GetCursorPos2D((HudManager modopt(IsConst)* modopt(IsConst) modopt(IsConst)) managerPtr, &vectorf);
                        float modopt(CallConvThiscall) x = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                        return new Vector2(x, EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf));
                    }
                }
                return Vector2.Zero;
            }
        }

        public static float FPS
        {
            get
            {
                EloBuddy.Native.Game* gamePtr = EloBuddy.Native.Game.GetInstance();
                if (gamePtr != null)
                {
                    return (float) ((1.0 / ((double) EloBuddy.Native.Game.GetFPS((EloBuddy.Native.Game modopt(IsConst)* modopt(IsConst) modopt(IsConst)) gamePtr))) + 0.5);
                }
                return 0f;
            }
        }

        public static uint GameId
        {
            get
            {
                ClientFacade* facadePtr = EloBuddy.Native.ClientFacade.GetInstance();
                if (facadePtr != null)
                {
                    return *(EloBuddy.Native.ClientFacade.GetGameId(facadePtr));
                }
                return 0;
            }
        }

        public static string IP
        {
            get
            {
                ClientFacade* facadePtr = EloBuddy.Native.ClientFacade.GetInstance();
                if (facadePtr != null)
                {
                    basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsConst) modopt(IsConst) localPtr = EloBuddy.Native.ClientFacade.GetIP(facadePtr);
                    return new string((0x10 > localPtr[20]) ? ((sbyte*) localPtr) : ((sbyte*) localPtr[0]));
                }
                return "Unknown";
            }
        }

        public static bool IsCustomGame
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                MissionInfo* infoPtr = EloBuddy.Native.MissionInfo.GetInstance();
                return ((infoPtr != null) && ((bool) ((byte) (*(EloBuddy.Native.MissionInfo.GetGameId(infoPtr)) == 0))));
            }
        }

        public static bool IsWindowFocused
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
                if (hudPtr != null)
                {
                    return *(EloBuddy.Native.pwHud.GetIsWindowFocused(hudPtr));
                }
                return false;
            }
        }

        public static GameMapId MapId
        {
            get
            {
                MissionInfo* infoPtr = EloBuddy.Native.MissionInfo.GetInstance();
                if (infoPtr != null)
                {
                    return *(EloBuddy.Native.MissionInfo.GetMapId(infoPtr));
                }
                return GameMapId.SummonersRift;
            }
        }

        public static EloBuddy.GameMode Mode
        {
            get
            {
                ClientFacade* facadePtr = EloBuddy.Native.ClientFacade.GetInstance();
                if (facadePtr != null)
                {
                    return EloBuddy.Native.ClientFacade.GetGameState((ClientFacade modopt(IsConst)* modopt(IsConst) modopt(IsConst)) facadePtr);
                }
                return EloBuddy.GameMode.Running;
            }
        }

        public static int Ping
        {
            get
            {
                NetClient* clientPtr = EloBuddy.Native.NetClient.Get();
                if (clientPtr != null)
                {
                    NetClient_Virtual* virtualPtr = EloBuddy.Native.NetClient.GetVirtual(clientPtr);
                    if (virtualPtr != null)
                    {
                        return **(((int*) virtualPtr))[0x98](virtualPtr);
                    }
                }
                return 0;
            }
        }

        public static int Port
        {
            get
            {
                ClientFacade* facadePtr = EloBuddy.Native.ClientFacade.GetInstance();
                if (facadePtr != null)
                {
                    return *(EloBuddy.Native.ClientFacade.GetPort(facadePtr));
                }
                return 0;
            }
        }

        public static string Region
        {
            get
            {
                ClientFacade* facadePtr = EloBuddy.Native.ClientFacade.GetInstance();
                if (facadePtr != null)
                {
                    basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsConst) modopt(IsConst) localPtr = EloBuddy.Native.ClientFacade.GetRegion(facadePtr);
                    return new string((0x10 > localPtr[20]) ? ((sbyte*) localPtr) : ((sbyte*) localPtr[0]));
                }
                return "Unknown";
            }
        }

        public static int TicksPerSecond
        {
            get => 
                EloBuddy.Native.Game.GetTPS(EloBuddy.Native.Game.GetInstance());
            set
            {
                EloBuddy.Native.Game.SetTPS(EloBuddy.Native.Game.GetInstance(), value);
            }
        }

        public static float Time
        {
            get
            {
                RiotClock* clockPtr = EloBuddy.Native.RiotClock.GetInstance();
                if (clockPtr != null)
                {
                    return EloBuddy.Native.RiotClock.GetTime(clockPtr);
                }
                return 0f;
            }
        }

        public static GameType Type
        {
            get
            {
                MissionInfo* infoPtr = EloBuddy.Native.MissionInfo.GetInstance();
                if (infoPtr != null)
                {
                    return *(EloBuddy.Native.MissionInfo.GetGameType(infoPtr));
                }
                return GameType.Normal;
            }
        }

        public static string Version =>
            new string(EloBuddy.Native.Core.GetGameVersion());

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool OnGameAfkNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnGameDisconnectNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnGameEndNativeDelegate(int A_0);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnGameLoadNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnGameNotifyNativeDelegate(uint A_0, int A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnGamePostTickNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnGamePreTickNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnGameProcessPacketNativeDelegate(S2C_ENetPacket* A_0, uint A_1, uint A_2, uint modopt(IsLong) A_3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnGameSendPacketNativeDelegate(C2S_ENetPacket* A_0, uint A_1, uint A_2, uint modopt(IsLong) A_3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnGameTickNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnGameUpdateNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnGameWndProcNativeDelegate(HWND__* A_0, uint A_1, uint A_2, int modopt(IsLong) A_3);
    }
}

