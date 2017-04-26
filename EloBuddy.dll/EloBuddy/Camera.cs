namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Camera
    {
        internal static List<CameraSnap> CameraSnapHandlers;
        internal static List<CameraToggleLock> CameraToggleLockHandlers;
        internal static List<CameraUpdate> CameraUpdateHandlers;
        internal static List<CameraZoom> CameraZoomHandlers;
        internal static IntPtr m_CameraSnapNative = new IntPtr();
        internal static OnCameraSnapNativeDelegate m_CameraSnapNativeDelegate;
        internal static IntPtr m_CameraToggleLockNative = new IntPtr();
        internal static OnCameraToggleLockNativeDelegate m_CameraToggleLockNativeDelegate;
        internal static IntPtr m_CameraUpdateNative = new IntPtr();
        internal static OnCameraUpdateNativeDelegate m_CameraUpdateNativeDelegate;
        internal static IntPtr m_CameraZoomNative = new IntPtr();
        internal static OnCameraZoomNativeDelegate m_CameraZoomNativeDelegate;

        public static  event CameraSnap OnSnap
        {
            add
            {
                CameraSnapHandlers.Add(handler);
            }
            remove
            {
                CameraSnapHandlers.Remove(handler);
            }
        }

        public static  event CameraToggleLock OnToggleLock
        {
            add
            {
                CameraToggleLockHandlers.Add(handler);
            }
            remove
            {
                CameraToggleLockHandlers.Remove(handler);
            }
        }

        public static  event CameraUpdate OnUpdate
        {
            add
            {
                CameraUpdateHandlers.Add(handler);
            }
            remove
            {
                CameraUpdateHandlers.Remove(handler);
            }
        }

        public static  event CameraZoom OnZoom
        {
            add
            {
                CameraZoomHandlers.Add(handler);
            }
            remove
            {
                CameraZoomHandlers.Remove(handler);
            }
        }

        static Camera()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(Camera.DomainUnloadEventHandler);
            CameraSnapHandlers = new List<CameraSnap>();
            m_CameraSnapNativeDelegate = new OnCameraSnapNativeDelegate(Camera.OnCameraSnapNative);
            m_CameraSnapNative = Marshal.GetFunctionPointerForDelegate(m_CameraSnapNativeDelegate);
            EloBuddy.Native.EventHandler<55,bool __cdecl(void)>.Add(EloBuddy.Native.EventHandler<55,bool __cdecl(void)>.GetInstance(), m_CameraSnapNative.ToPointer());
            CameraToggleLockHandlers = new List<CameraToggleLock>();
            m_CameraToggleLockNativeDelegate = new OnCameraToggleLockNativeDelegate(Camera.OnCameraToggleLockNative);
            m_CameraToggleLockNative = Marshal.GetFunctionPointerForDelegate(m_CameraToggleLockNativeDelegate);
            EloBuddy.Native.EventHandler<56,bool __cdecl(void)>.Add(EloBuddy.Native.EventHandler<56,bool __cdecl(void)>.GetInstance(), m_CameraToggleLockNative.ToPointer());
            CameraUpdateHandlers = new List<CameraUpdate>();
            m_CameraUpdateNativeDelegate = new OnCameraUpdateNativeDelegate(Camera.OnCameraUpdateNative);
            m_CameraUpdateNative = Marshal.GetFunctionPointerForDelegate(m_CameraUpdateNativeDelegate);
            EloBuddy.Native.EventHandler<57,bool __cdecl(float,float),float,float>.Add(EloBuddy.Native.EventHandler<57,bool __cdecl(float,float),float,float>.GetInstance(), m_CameraUpdateNative.ToPointer());
            CameraZoomHandlers = new List<CameraZoom>();
            m_CameraZoomNativeDelegate = new OnCameraZoomNativeDelegate(Camera.OnCameraZoomNative);
            m_CameraZoomNative = Marshal.GetFunctionPointerForDelegate(m_CameraZoomNativeDelegate);
            EloBuddy.Native.EventHandler<58,bool __cdecl(void)>.Add(EloBuddy.Native.EventHandler<58,bool __cdecl(void)>.GetInstance(), m_CameraZoomNative.ToPointer());
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<55,bool __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<55,bool __cdecl(void)>.GetInstance(), m_CameraSnapNative.ToPointer());
            EloBuddy.Native.EventHandler<56,bool __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<56,bool __cdecl(void)>.GetInstance(), m_CameraToggleLockNative.ToPointer());
            EloBuddy.Native.EventHandler<57,bool __cdecl(float,float),float,float>.Remove(EloBuddy.Native.EventHandler<57,bool __cdecl(float,float),float,float>.GetInstance(), m_CameraUpdateNative.ToPointer());
            EloBuddy.Native.EventHandler<58,bool __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<58,bool __cdecl(void)>.GetInstance(), m_CameraZoomNative.ToPointer());
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static bool OnCameraSnapNative()
        {
            bool flag = true;
            try
            {
                CameraSnapEventArgs args = new CameraSnapEventArgs();
                foreach (CameraSnap snap in CameraSnapHandlers.ToArray())
                {
                    snap(args);
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
        internal static bool OnCameraToggleLockNative()
        {
            bool flag = true;
            try
            {
                CameraLockToggleEventArgs args = new CameraLockToggleEventArgs();
                foreach (CameraToggleLock @lock in CameraToggleLockHandlers.ToArray())
                {
                    @lock(args);
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
        internal static bool OnCameraUpdateNative(float A_0, float A_1)
        {
            bool flag = true;
            try
            {
                CameraUpdateEventArgs args = new CameraUpdateEventArgs(A_0, A_1);
                foreach (CameraUpdate update in CameraUpdateHandlers.ToArray())
                {
                    update(args);
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
        internal static bool OnCameraZoomNative()
        {
            bool flag = true;
            try
            {
                CameraZoomEventArgs args = new CameraZoomEventArgs();
                foreach (CameraZoom zoom in CameraZoomHandlers.ToArray())
                {
                    zoom(args);
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

        public static unsafe void SetZoomDistance(float value)
        {
            byte[] buffer = new byte[] { 0x51, 0x13, 0x11, 0x9f, 0xff, 0x41 };
            r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
            if (cameraPtr != null)
            {
                EloBuddy.Native.r3dCamera.SetSafeZoomDistance(cameraPtr, value);
            }
        }

        public static float CameraX
        {
            get
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    return *(EloBuddy.Native.r3dCamera.GetCameraX(cameraPtr));
                }
                return 0f;
            }
            set
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    EloBuddy.Native.r3dCamera.SetCameraX(cameraPtr, value);
                }
            }
        }

        public static float CameraY
        {
            get
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    return *(EloBuddy.Native.r3dCamera.GetCameraY(cameraPtr));
                }
                return 0f;
            }
            set
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    EloBuddy.Native.r3dCamera.SetCameraY(cameraPtr, value);
                }
            }
        }

        public static bool Locked
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
                return ((hudPtr != null) && EloBuddy.Native.pwHud.IsLocked(hudPtr));
            }
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                pwHud* hudPtr = EloBuddy.Native.pwHud.GetInstance();
                if (hudPtr == null)
                {
                    throw new Exception("Tried to lock Camera without a valid pointer to pwHud Interface!");
                }
                EloBuddy.Native.pwHud.SetIsLocked(hudPtr, value);
            }
        }

        public static float Pitch
        {
            get
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    return *(EloBuddy.Native.r3dCamera.GetPitch(cameraPtr));
                }
                return 0f;
            }
            set
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    EloBuddy.Native.r3dCamera.SetPitch(cameraPtr, value);
                }
            }
        }

        public static Vector2 ScreenPosition
        {
            get
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    float* numPtr2 = EloBuddy.Native.r3dCamera.GetCameraY(cameraPtr);
                    float* numPtr = EloBuddy.Native.r3dCamera.GetCameraX(cameraPtr);
                    return new Vector2(numPtr[0], numPtr2[0]);
                }
                return Vector2.Zero;
            }
            set
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    EloBuddy.Native.r3dCamera.SetCameraX(cameraPtr, value.X);
                    EloBuddy.Native.r3dCamera.SetCameraY(cameraPtr, value.Y);
                }
            }
        }

        public static float ViewportDistance
        {
            get
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    return *(EloBuddy.Native.r3dCamera.GetViewportDistance(cameraPtr));
                }
                return 0f;
            }
            set
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    EloBuddy.Native.r3dCamera.SetViewportDistance(cameraPtr, value);
                }
            }
        }

        public static Vector2 Yaw
        {
            get
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    float* numPtr2 = EloBuddy.Native.r3dCamera.GetYawY(cameraPtr);
                    float* numPtr = EloBuddy.Native.r3dCamera.GetYawX(cameraPtr);
                    return new Vector2(numPtr[0], numPtr2[0]);
                }
                return Vector2.Zero;
            }
            set
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    EloBuddy.Native.r3dCamera.SetYawX(cameraPtr, value.X);
                    EloBuddy.Native.r3dCamera.SetYawY(cameraPtr, value.Y);
                }
            }
        }

        public static float YawX
        {
            get
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    return *(EloBuddy.Native.r3dCamera.GetYawX(cameraPtr));
                }
                return 0f;
            }
            set
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    EloBuddy.Native.r3dCamera.SetYawX(cameraPtr, value);
                }
            }
        }

        public static float YawY
        {
            get
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    return *(EloBuddy.Native.r3dCamera.GetYawY(cameraPtr));
                }
                return 0f;
            }
            set
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    EloBuddy.Native.r3dCamera.SetYawY(cameraPtr, value);
                }
            }
        }

        public static float ZoomDistance
        {
            get
            {
                r3dCamera* cameraPtr = EloBuddy.Native.r3dCamera.GetInstance();
                if (cameraPtr != null)
                {
                    return *(EloBuddy.Native.r3dCamera.GetZoomDistance(cameraPtr));
                }
                return 0f;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool OnCameraSnapNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool OnCameraToggleLockNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool OnCameraUpdateNativeDelegate(float A_0, float A_1);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool OnCameraZoomNativeDelegate();
    }
}

