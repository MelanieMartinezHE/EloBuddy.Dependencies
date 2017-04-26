namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class TacticalMap
    {
        internal static IntPtr m_TacticalMapPingNative = new IntPtr();
        internal static OnTacticalMapPingNativeDelegate m_TacticalMapPingNativeDelegate;
        internal static List<TacticalMapPing> TacticalMapPingHandlers;

        public static  event TacticalMapPing OnPing
        {
            add
            {
                TacticalMapPingHandlers.Add(handler);
            }
            remove
            {
                TacticalMapPingHandlers.Remove(handler);
            }
        }

        static TacticalMap()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(EloBuddy.TacticalMap.DomainUnloadEventHandler);
            TacticalMapPingHandlers = new List<TacticalMapPing>();
            m_TacticalMapPingNativeDelegate = new OnTacticalMapPingNativeDelegate(EloBuddy.TacticalMap.OnTacticalMapPingNative);
            m_TacticalMapPingNative = Marshal.GetFunctionPointerForDelegate(m_TacticalMapPingNativeDelegate);
            EloBuddy.Native.EventHandler<36,bool __cdecl(EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,EloBuddy::Native::GameObject *,unsigned int),EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,EloBuddy::Native::GameObject *,unsigned int>.Add(EloBuddy.Native.EventHandler<36,bool __cdecl(EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,EloBuddy::Native::GameObject *,unsigned int),EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,EloBuddy::Native::GameObject *,unsigned int>.GetInstance(), m_TacticalMapPingNative.ToPointer());
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<36,bool __cdecl(EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,EloBuddy::Native::GameObject *,unsigned int),EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,EloBuddy::Native::GameObject *,unsigned int>.Remove(EloBuddy.Native.EventHandler<36,bool __cdecl(EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,EloBuddy::Native::GameObject *,unsigned int),EloBuddy::Native::Vector3f *,EloBuddy::Native::GameObject *,EloBuddy::Native::GameObject *,unsigned int>.GetInstance(), m_TacticalMapPingNative.ToPointer());
        }

        public static unsafe Vector3 MinimapToWorld(float x, float y)
        {
            EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
            if (mapPtr != null)
            {
                Vector3f* vectorfPtr = EloBuddy.Native.TacticalMap.ToWorldCoord((EloBuddy.Native.TacticalMap modopt(IsConst)* modopt(IsConst) modopt(IsConst)) mapPtr, x, y);
                return new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
            }
            return Vector3.Zero;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool OnTacticalMapPingNative(Vector3f* A_0, EloBuddy.Native.GameObject* A_1, EloBuddy.Native.GameObject* A_2, uint A_3)
        {
            Exception innerException = null;
            bool flag = true;
            try
            {
                EloBuddy.GameObject target = null;
                EloBuddy.GameObject source = null;
                if (A_1 != null)
                {
                    target = ObjectManager.CreateObjectFromPointer(A_1);
                }
                if (A_2 != null)
                {
                    source = ObjectManager.CreateObjectFromPointer(A_2);
                }
                Vector2 position = new Vector2(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_0), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) A_0));
                TacticalMapPingEventArgs args = new TacticalMapPingEventArgs(position, target, source, (PingCategory) A_3);
                foreach (TacticalMapPing ping in TacticalMapPingHandlers.ToArray())
                {
                    try
                    {
                        ping(args);
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

        public static unsafe void SendPing(PingCategory type, EloBuddy.GameObject target)
        {
            if (target != null)
            {
                Vector3f vectorf;
                Vector3 position = target.Position;
                Vector3 vector = target.Position;
                EloBuddy.Native.MenuGUI.CallCurrentPing(EloBuddy.Native.Vector3f.{ctor}(&vectorf, vector.X, position.Y, 0f), target.NetworkId, (int) type);
            }
        }

        public static unsafe void SendPing(PingCategory type, Vector2 position)
        {
            Vector3f vectorf;
            EloBuddy.Native.MenuGUI.CallCurrentPing(EloBuddy.Native.Vector3f.{ctor}(&vectorf, position.X, position.Y, 0f), 0, (int) type);
        }

        public static unsafe void SendPing(PingCategory type, Vector3 position)
        {
            Vector3f vectorf;
            EloBuddy.Native.MenuGUI.CallCurrentPing(EloBuddy.Native.Vector3f.{ctor}(&vectorf, position.X, position.Y, 0f), 0, (int) type);
        }

        public static void ShowPing(PingCategory type, EloBuddy.GameObject target)
        {
            ShowPing(type, target, false);
        }

        public static void ShowPing(PingCategory type, Vector2 position)
        {
            ShowPing(type, position, false);
        }

        public static void ShowPing(PingCategory type, Vector3 position)
        {
            ShowPing(type, position, false);
        }

        public static void ShowPing(PingCategory type, EloBuddy.GameObject source, EloBuddy.GameObject target)
        {
            ShowPing(type, source, target, false);
        }

        public static unsafe void ShowPing(PingCategory type, EloBuddy.GameObject target, [MarshalAs(UnmanagedType.U1)] bool playSound)
        {
            EloBuddy.Native.GameObject* objPtr2 = EloBuddy.Native.ObjectManager.GetPlayer();
            if (objPtr2 != null)
            {
                EloBuddy.Native.GameObject* ptr = target.GetPtr();
                if (ptr != null)
                {
                    Vector3f vectorf;
                    int* numPtr2 = EloBuddy.Native.GameObject.GetLocalIndex(objPtr2);
                    int* numPtr = EloBuddy.Native.GameObject.GetLocalIndex(ptr);
                    Vector3 position = target.Position;
                    Vector3 vector = target.Position;
                    EloBuddy.Native.MenuGUI.PingMiniMap(EloBuddy.Native.Vector3f.{ctor}(&vectorf, vector.X, position.Y, 0f), numPtr2[0], numPtr[0], (int) type, playSound);
                }
            }
        }

        public static unsafe void ShowPing(PingCategory type, Vector2 position, [MarshalAs(UnmanagedType.U1)] bool playSound)
        {
            EloBuddy.Native.GameObject* objPtr = EloBuddy.Native.ObjectManager.GetPlayer();
            if (objPtr != null)
            {
                Vector3f vectorf;
                int num = *(EloBuddy.Native.GameObject.GetLocalIndex(objPtr));
                EloBuddy.Native.MenuGUI.PingMiniMap(EloBuddy.Native.Vector3f.{ctor}(&vectorf, position.X, position.Y, 0f), num, num, (int) type, playSound);
            }
        }

        public static unsafe void ShowPing(PingCategory type, Vector3 position, [MarshalAs(UnmanagedType.U1)] bool playSound)
        {
            EloBuddy.Native.GameObject* objPtr = EloBuddy.Native.ObjectManager.GetPlayer();
            if (objPtr != null)
            {
                Vector3f vectorf;
                int num = *(EloBuddy.Native.GameObject.GetLocalIndex(objPtr));
                EloBuddy.Native.MenuGUI.PingMiniMap(EloBuddy.Native.Vector3f.{ctor}(&vectorf, position.X, position.Y, 0f), num, num, (int) type, playSound);
            }
        }

        public static unsafe void ShowPing(PingCategory type, EloBuddy.GameObject source, EloBuddy.GameObject target, [MarshalAs(UnmanagedType.U1)] bool playSound)
        {
            if ((source != null) && (target != null))
            {
                EloBuddy.Native.GameObject* ptr = source.GetPtr();
                EloBuddy.Native.GameObject* objPtr = target.GetPtr();
                if (objPtr != null)
                {
                    Vector3f vectorf;
                    int* numPtr2 = EloBuddy.Native.GameObject.GetLocalIndex(ptr);
                    int* numPtr = EloBuddy.Native.GameObject.GetLocalIndex(objPtr);
                    Vector3 position = target.Position;
                    Vector3 vector = target.Position;
                    EloBuddy.Native.MenuGUI.PingMiniMap(EloBuddy.Native.Vector3f.{ctor}(&vectorf, vector.X, position.Y, 0f), numPtr2[0], numPtr[0], (int) type, playSound);
                }
            }
        }

        public static unsafe Vector2 WorldToMinimap(Vector3 worldCoord)
        {
            Vector3f vectorf;
            EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
            float* numPtr2 = @new(4);
            float* numPtr = @new(4);
            if ((mapPtr != null) && EloBuddy.Native.TacticalMap.ToMapCoord((EloBuddy.Native.TacticalMap modopt(IsConst)* modopt(IsConst) modopt(IsConst)) mapPtr, EloBuddy.Native.Vector3f.{ctor}(&vectorf, worldCoord.X, worldCoord.Z, worldCoord.Y), numPtr2, numPtr))
            {
                return new Vector2(numPtr2[0], numPtr[0]);
            }
            return Vector2.Zero;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool WorldToMinimap(Vector3 worldCoord, out Vector2 mapCoord)
        {
            Vector3f vectorf;
            EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
            float* numPtr2 = @new(4);
            float* numPtr = @new(4);
            if ((mapPtr != null) && EloBuddy.Native.TacticalMap.ToMapCoord((EloBuddy.Native.TacticalMap modopt(IsConst)* modopt(IsConst) modopt(IsConst)) mapPtr, EloBuddy.Native.Vector3f.{ctor}(&vectorf, worldCoord.X, worldCoord.Z, worldCoord.Y), numPtr2, numPtr))
            {
                Vector2 vector = new Vector2(numPtr2[0], numPtr[0]);
                mapCoord = vector;
                return true;
            }
            return false;
        }

        public static float Height
        {
            get
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    return *(EloBuddy.Native.TacticalMap.GetHeight(mapPtr));
                }
                return 0f;
            }
            set
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    EloBuddy.Native.TacticalMap.SetHeight(mapPtr, value);
                }
            }
        }

        public static Vector2 Position
        {
            get
            {
                float num;
                float num2;
                EloBuddy.Native.TacticalMap* mapPtr2 = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr2 != null)
                {
                    num2 = *(EloBuddy.Native.TacticalMap.GetMinimapY(mapPtr2));
                }
                else
                {
                    num2 = 0f;
                }
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    num = *(EloBuddy.Native.TacticalMap.GetMinimapX(mapPtr));
                }
                else
                {
                    num = 0f;
                }
                return new Vector2(num, num2);
            }
        }

        public static float ScaleX
        {
            get
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    return *(EloBuddy.Native.TacticalMap.GetScaleX(mapPtr));
                }
                return 0f;
            }
            set
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    EloBuddy.Native.TacticalMap.SetScaleX(mapPtr, value);
                }
            }
        }

        public static float ScaleY
        {
            get
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    return *(EloBuddy.Native.TacticalMap.GetScaleY(mapPtr));
                }
                return 0f;
            }
            set
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    EloBuddy.Native.TacticalMap.SetScaleY(mapPtr, value);
                }
            }
        }

        public static float Width
        {
            get
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    return *(EloBuddy.Native.TacticalMap.GetWidth(mapPtr));
                }
                return 0f;
            }
            set
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    EloBuddy.Native.TacticalMap.SetWidth(mapPtr, value);
                }
            }
        }

        public static float X
        {
            get
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    return *(EloBuddy.Native.TacticalMap.GetMinimapX(mapPtr));
                }
                return 0f;
            }
            set
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    EloBuddy.Native.TacticalMap.SetMinimapX(mapPtr, value);
                }
            }
        }

        public static float Y
        {
            get
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    return *(EloBuddy.Native.TacticalMap.GetMinimapY(mapPtr));
                }
                return 0f;
            }
            set
            {
                EloBuddy.Native.TacticalMap* mapPtr = EloBuddy.Native.TacticalMap.GetInstance();
                if (mapPtr != null)
                {
                    EloBuddy.Native.TacticalMap.SetMinimapY(mapPtr, value);
                }
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool OnTacticalMapPingNativeDelegate(Vector3f* A_0, EloBuddy.Native.GameObject* A_1, EloBuddy.Native.GameObject* A_2, uint A_3);
    }
}

