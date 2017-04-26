namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Hud
    {
        internal static List<HudChangeTarget> HudChangeTargetHandlers;
        internal static IntPtr m_HudChangeTargetNative = new IntPtr();
        internal static OnHudChangeTargetNativeDelegate m_HudChangeTargetNativeDelegate;

        public static  event HudChangeTarget OnTargetChange
        {
            add
            {
                HudChangeTargetHandlers.Add(handler);
            }
            remove
            {
                HudChangeTargetHandlers.Remove(handler);
            }
        }

        static Hud()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(Hud.DomainUnloadEventHandler);
            HudChangeTargetHandlers = new List<HudChangeTarget>();
            m_HudChangeTargetNativeDelegate = new OnHudChangeTargetNativeDelegate(Hud.OnHudChangeTargetNative);
            m_HudChangeTargetNative = Marshal.GetFunctionPointerForDelegate(m_HudChangeTargetNativeDelegate);
            EloBuddy.Native.EventHandler<49,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.Add(EloBuddy.Native.EventHandler<49,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.GetInstance(), m_HudChangeTargetNative.ToPointer());
        }

        public static unsafe void DisableDrawing(HudDrawingType type)
        {
            pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
            if (hudPtr != null)
            {
                switch (type)
                {
                    case HudDrawingType.Healthbar:
                        EloBuddy.Native.pwHud.DisableHPBar(hudPtr);
                        break;

                    case HudDrawingType.Menu:
                        EloBuddy.Native.pwHud.DisableMenuUI(hudPtr);
                        break;

                    case HudDrawingType.PwHud:
                        EloBuddy.Native.pwHud.DisableUI(hudPtr);
                        break;

                    case HudDrawingType.Minimap:
                        EloBuddy.Native.pwHud.DisableMinimap(hudPtr);
                        break;

                    case HudDrawingType.Ping:
                        EloBuddy.Native.pwHud.DisablePing(hudPtr);
                        break;
                }
            }
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<49,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.Remove(EloBuddy.Native.EventHandler<49,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.GetInstance(), m_HudChangeTargetNative.ToPointer());
        }

        public static unsafe void EnableDrawing(HudDrawingType type)
        {
            pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
            if (hudPtr != null)
            {
                switch (type)
                {
                    case HudDrawingType.Healthbar:
                        EloBuddy.Native.pwHud.EnableHPBar(hudPtr);
                        break;

                    case HudDrawingType.Menu:
                        EloBuddy.Native.pwHud.EnableMenuUI(hudPtr);
                        break;

                    case HudDrawingType.PwHud:
                        EloBuddy.Native.pwHud.EnableUI(hudPtr);
                        break;

                    case HudDrawingType.Minimap:
                        EloBuddy.Native.pwHud.EnableMinimap(hudPtr);
                        break;

                    case HudDrawingType.Ping:
                        EloBuddy.Native.pwHud.EnablePing(hudPtr);
                        break;
                }
            }
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool IsDrawing(HudDrawingType type)
        {
            pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
            if (hudPtr != null)
            {
                return (bool) ((byte) !EloBuddy.Native.pwHud.IsDrawing(hudPtr, (pwHudDrawingType) type));
            }
            return true;
        }

        internal static unsafe void OnHudChangeTargetNative(EloBuddy.Native.GameObject* A_0)
        {
            Exception innerException = null;
            EloBuddy.GameObject obj4 = null;
            try
            {
                EloBuddy.GameObject obj2;
                byte num2;
                HudChangeTargetEventArgs args;
                EloBuddy.GameObject obj3 = null;
                if (A_0 != null)
                {
                    obj4 = ObjectManager.CreateObjectFromPointer(A_0);
                }
                else
                {
                    num2 = 1;
                    goto Label_0029;
                }
                num2 = 0;
                goto Label_002E;
            Label_0029:
                obj2 = null;
                goto Label_0032;
            Label_002E:
                obj2 = obj3;
            Label_0032:
                args = new HudChangeTargetEventArgs(obj2, (bool) num2);
                foreach (HudChangeTarget target in HudChangeTargetHandlers.ToArray())
                {
                    try
                    {
                        target(args);
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

        public static unsafe void ShowClick(ClickType type, Vector3 position)
        {
            pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
            if (hudPtr != null)
            {
                Vector3f vectorf;
                int num = (int) (type == ClickType.Attack);
                EloBuddy.Native.pwHud.ShowClick((pwHud modopt(IsConst)* modopt(IsConst) modopt(IsConst)) hudPtr, EloBuddy.Native.Vector3f.{ctor}(&vectorf, position.X, position.Z, position.Y), (byte) num);
            }
        }

        public static EloBuddy.GameObject SelectedTarget =>
            null;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnHudChangeTargetNativeDelegate(EloBuddy.Native.GameObject* A_0);
    }
}

