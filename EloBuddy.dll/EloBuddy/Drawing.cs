namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using SharpDX.Direct3D9;
    using std;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Drawing
    {
        internal static List<DrawingBeginScene> DrawingBeginSceneHandlers;
        internal static List<DrawingDraw> DrawingDrawHandlers;
        internal static List<DrawingEndScene> DrawingEndSceneHandlers;
        internal static List<DrawingFlushEndScene> DrawingFlushEndSceneHandlers;
        internal static List<DrawingPostReset> DrawingPostResetHandlers;
        internal static List<DrawingPreReset> DrawingPreResetHandlers;
        internal static List<DrawingPresent> DrawingPresentHandlers;
        internal static List<DrawingSetRenderTarget> DrawingSetRenderTargetHandlers;
        internal static Device m_device;
        internal static IntPtr m_DrawingBeginSceneNative = new IntPtr();
        internal static OnDrawingBeginSceneNativeDelegate m_DrawingBeginSceneNativeDelegate;
        internal static IntPtr m_DrawingDrawNative = new IntPtr();
        internal static OnDrawingDrawNativeDelegate m_DrawingDrawNativeDelegate;
        internal static IntPtr m_DrawingEndSceneNative = new IntPtr();
        internal static OnDrawingEndSceneNativeDelegate m_DrawingEndSceneNativeDelegate;
        internal static IntPtr m_DrawingFlushEndSceneNative = new IntPtr();
        internal static OnDrawingFlushEndSceneNativeDelegate m_DrawingFlushEndSceneNativeDelegate;
        internal static IntPtr m_DrawingPostResetNative = new IntPtr();
        internal static OnDrawingPostResetNativeDelegate m_DrawingPostResetNativeDelegate;
        internal static IntPtr m_DrawingPreResetNative = new IntPtr();
        internal static OnDrawingPreResetNativeDelegate m_DrawingPreResetNativeDelegate;
        internal static IntPtr m_DrawingPresentNative = new IntPtr();
        internal static OnDrawingPresentNativeDelegate m_DrawingPresentNativeDelegate;
        internal static IntPtr m_DrawingSetRenderTargetNative = new IntPtr();
        internal static OnDrawingSetRenderTargetNativeDelegate m_DrawingSetRenderTargetNativeDelegate;
        internal unsafe static void* oldDX;

        public static  event DrawingBeginScene OnBeginScene
        {
            add
            {
                DrawingBeginSceneHandlers.Add(handler);
            }
            remove
            {
                DrawingBeginSceneHandlers.Remove(handler);
            }
        }

        public static  event DrawingDraw OnDraw
        {
            add
            {
                DrawingDrawHandlers.Add(handler);
            }
            remove
            {
                DrawingDrawHandlers.Remove(handler);
            }
        }

        public static  event DrawingEndScene OnEndScene
        {
            add
            {
                DrawingEndSceneHandlers.Add(handler);
            }
            remove
            {
                DrawingEndSceneHandlers.Remove(handler);
            }
        }

        public static  event DrawingFlushEndScene OnFlushEndScene
        {
            add
            {
                DrawingFlushEndSceneHandlers.Add(handler);
            }
            remove
            {
                DrawingFlushEndSceneHandlers.Remove(handler);
            }
        }

        public static  event DrawingPostReset OnPostReset
        {
            add
            {
                DrawingPostResetHandlers.Add(handler);
            }
            remove
            {
                DrawingPostResetHandlers.Remove(handler);
            }
        }

        public static  event DrawingPreReset OnPreReset
        {
            add
            {
                DrawingPreResetHandlers.Add(handler);
            }
            remove
            {
                DrawingPreResetHandlers.Remove(handler);
            }
        }

        public static  event DrawingPresent OnPresent
        {
            add
            {
                DrawingPresentHandlers.Add(handler);
            }
            remove
            {
                DrawingPresentHandlers.Remove(handler);
            }
        }

        public static  event DrawingSetRenderTarget OnSetRenderTarget
        {
            add
            {
                DrawingSetRenderTargetHandlers.Add(handler);
            }
            remove
            {
                DrawingSetRenderTargetHandlers.Remove(handler);
            }
        }

        static Drawing()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(EloBuddy.Drawing.DomainUnloadEventHandler);
            DrawingBeginSceneHandlers = new List<DrawingBeginScene>();
            m_DrawingBeginSceneNativeDelegate = new OnDrawingBeginSceneNativeDelegate(EloBuddy.Drawing.OnDrawingBeginSceneNative);
            m_DrawingBeginSceneNative = Marshal.GetFunctionPointerForDelegate(m_DrawingBeginSceneNativeDelegate);
            EloBuddy.Native.EventHandler<5,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<5,void __cdecl(void)>.GetInstance(), m_DrawingBeginSceneNative.ToPointer());
            DrawingDrawHandlers = new List<DrawingDraw>();
            m_DrawingDrawNativeDelegate = new OnDrawingDrawNativeDelegate(EloBuddy.Drawing.OnDrawingDrawNative);
            m_DrawingDrawNative = Marshal.GetFunctionPointerForDelegate(m_DrawingDrawNativeDelegate);
            EloBuddy.Native.EventHandler<6,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<6,void __cdecl(void)>.GetInstance(), m_DrawingDrawNative.ToPointer());
            DrawingEndSceneHandlers = new List<DrawingEndScene>();
            m_DrawingEndSceneNativeDelegate = new OnDrawingEndSceneNativeDelegate(EloBuddy.Drawing.OnDrawingEndSceneNative);
            m_DrawingEndSceneNative = Marshal.GetFunctionPointerForDelegate(m_DrawingEndSceneNativeDelegate);
            EloBuddy.Native.EventHandler<7,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<7,void __cdecl(void)>.GetInstance(), m_DrawingEndSceneNative.ToPointer());
            DrawingPostResetHandlers = new List<DrawingPostReset>();
            m_DrawingPostResetNativeDelegate = new OnDrawingPostResetNativeDelegate(EloBuddy.Drawing.OnDrawingPostResetNative);
            m_DrawingPostResetNative = Marshal.GetFunctionPointerForDelegate(m_DrawingPostResetNativeDelegate);
            EloBuddy.Native.EventHandler<8,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<8,void __cdecl(void)>.GetInstance(), m_DrawingPostResetNative.ToPointer());
            DrawingPreResetHandlers = new List<DrawingPreReset>();
            m_DrawingPreResetNativeDelegate = new OnDrawingPreResetNativeDelegate(EloBuddy.Drawing.OnDrawingPreResetNative);
            m_DrawingPreResetNative = Marshal.GetFunctionPointerForDelegate(m_DrawingPreResetNativeDelegate);
            EloBuddy.Native.EventHandler<9,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<9,void __cdecl(void)>.GetInstance(), m_DrawingPreResetNative.ToPointer());
            DrawingPresentHandlers = new List<DrawingPresent>();
            m_DrawingPresentNativeDelegate = new OnDrawingPresentNativeDelegate(EloBuddy.Drawing.OnDrawingPresentNative);
            m_DrawingPresentNative = Marshal.GetFunctionPointerForDelegate(m_DrawingPresentNativeDelegate);
            EloBuddy.Native.EventHandler<10,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<10,void __cdecl(void)>.GetInstance(), m_DrawingPresentNative.ToPointer());
            DrawingSetRenderTargetHandlers = new List<DrawingSetRenderTarget>();
            m_DrawingSetRenderTargetNativeDelegate = new OnDrawingSetRenderTargetNativeDelegate(EloBuddy.Drawing.OnDrawingSetRenderTargetNative);
            m_DrawingSetRenderTargetNative = Marshal.GetFunctionPointerForDelegate(m_DrawingSetRenderTargetNativeDelegate);
            EloBuddy.Native.EventHandler<11,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<11,void __cdecl(void)>.GetInstance(), m_DrawingSetRenderTargetNative.ToPointer());
            DrawingFlushEndSceneHandlers = new List<DrawingFlushEndScene>();
            m_DrawingFlushEndSceneNativeDelegate = new OnDrawingFlushEndSceneNativeDelegate(EloBuddy.Drawing.OnDrawingFlushEndSceneNative);
            m_DrawingFlushEndSceneNative = Marshal.GetFunctionPointerForDelegate(m_DrawingFlushEndSceneNativeDelegate);
            EloBuddy.Native.EventHandler<45,void __cdecl(void)>.Add(EloBuddy.Native.EventHandler<45,void __cdecl(void)>.GetInstance(), m_DrawingFlushEndSceneNative.ToPointer());
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<5,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<5,void __cdecl(void)>.GetInstance(), m_DrawingBeginSceneNative.ToPointer());
            EloBuddy.Native.EventHandler<6,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<6,void __cdecl(void)>.GetInstance(), m_DrawingDrawNative.ToPointer());
            EloBuddy.Native.EventHandler<7,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<7,void __cdecl(void)>.GetInstance(), m_DrawingEndSceneNative.ToPointer());
            EloBuddy.Native.EventHandler<8,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<8,void __cdecl(void)>.GetInstance(), m_DrawingPostResetNative.ToPointer());
            EloBuddy.Native.EventHandler<9,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<9,void __cdecl(void)>.GetInstance(), m_DrawingPreResetNative.ToPointer());
            EloBuddy.Native.EventHandler<10,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<10,void __cdecl(void)>.GetInstance(), m_DrawingPresentNative.ToPointer());
            EloBuddy.Native.EventHandler<11,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<11,void __cdecl(void)>.GetInstance(), m_DrawingSetRenderTargetNative.ToPointer());
            EloBuddy.Native.EventHandler<45,void __cdecl(void)>.Remove(EloBuddy.Native.EventHandler<45,void __cdecl(void)>.GetInstance(), m_DrawingFlushEndSceneNative.ToPointer());
        }

        public static unsafe void DrawCircle(Vector3 pos, float radius, System.Drawing.Color color)
        {
            Vector3f vectorf;
            EloBuddy.Native.r3dRenderer.DrawCircularRangeIndicator(EloBuddy.Native.Vector3f.{ctor}(&vectorf, pos.X, pos.Y, pos.Z), radius, color.ToArgb(), null);
        }

        public static void DrawLine(Vector2 start, Vector2 end, float thickness, System.Drawing.Color color)
        {
            EloBuddy.Native.Drawing.DrawLine(start.X, start.Y, end.X, end.Y, thickness, (uint modopt(IsLong)) color.ToArgb());
        }

        public static void DrawLine(float x, float y, float x2, float y2, float thickness, System.Drawing.Color color)
        {
            EloBuddy.Native.Drawing.DrawLine(x, y, x2, y2, thickness, (uint modopt(IsLong)) color.ToArgb());
        }

        public static unsafe void DrawText(Vector2 position, System.Drawing.Color color, string text, int size)
        {
            IntPtr hglobal = Marshal.StringToHGlobalAnsi(text);
            EloBuddy.Native.Drawing.GetInstance();
            EloBuddy.Native.Drawing.DrawFontText(position.X, position.Y, (uint modopt(IsLong)) color.ToArgb(), (sbyte modopt(IsSignUnspecifiedByte)*) hglobal.ToPointer(), size);
            Marshal.FreeHGlobal(hglobal);
        }

        public static unsafe void DrawText(float x, float y, System.Drawing.Color color, string text)
        {
            IntPtr hglobal = Marshal.StringToHGlobalAnsi(text);
            EloBuddy.Native.Drawing.GetInstance();
            EloBuddy.Native.Drawing.DrawFontText(x, y, (uint modopt(IsLong)) color.ToArgb(), (sbyte modopt(IsSignUnspecifiedByte)*) hglobal.ToPointer(), 14);
            Marshal.FreeHGlobal(hglobal);
        }

        public static unsafe void DrawText(float x, float y, System.Drawing.Color color, string text, int size)
        {
            IntPtr hglobal = Marshal.StringToHGlobalAnsi(text);
            EloBuddy.Native.Drawing.GetInstance();
            EloBuddy.Native.Drawing.DrawFontText(x, y, (uint modopt(IsLong)) color.ToArgb(), (sbyte modopt(IsSignUnspecifiedByte)*) hglobal.ToPointer(), size);
            Marshal.FreeHGlobal(hglobal);
        }

        public static unsafe void DrawTexture(IntPtr* texture, Vector3 worldCoord, float radius, System.Drawing.Color color)
        {
            if (EloBuddy.Native.r3dRenderer.GetInstance() != null)
            {
                Vector3f vectorf;
                EloBuddy.Native.r3dRenderer.DrawTexture((r3dTexture*) ((int) texture), EloBuddy.Native.Vector3f.{ctor}(&vectorf, worldCoord.X, worldCoord.Y, worldCoord.Z), radius, color.ToArgb());
            }
        }

        public static unsafe void DrawTexture(ManagedTexture texture, Vector3 worldCoord, float radius, System.Drawing.Color color)
        {
            if (EloBuddy.Native.r3dRenderer.GetInstance() != null)
            {
                Vector3f vectorf;
                EloBuddy.Native.r3dRenderer.DrawTexture(texture.m_texture, EloBuddy.Native.Vector3f.{ctor}(&vectorf, worldCoord.X, worldCoord.Y, worldCoord.Z), radius, color.ToArgb());
            }
        }

        public static unsafe void DrawTexture(string texture, Vector3 worldCoord, float radius, System.Drawing.Color color)
        {
            if (EloBuddy.Native.r3dRenderer.GetInstance() != null)
            {
                int num;
                Vector3f* modopt(IsConst) modopt(IsConst) vectorfPtr;
                basic_string<char,std::char_traits<char>,std::allocator<char> > local;
                IntPtr ptr = Marshal.StringToHGlobalAnsi(texture);
                basic_string<char,std::char_traits<char>,std::allocator<char> >* localPtr2 = &local;
                basic_string<char,std::char_traits<char>,std::allocator<char> >* modopt(IsConst) modopt(IsConst) localPtr = std.basic_string<char,std::char_traits<char>,std::allocator<char> >.{ctor}(&local, (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) ptr.ToPointer());
                try
                {
                    Vector3f vectorf;
                    vectorfPtr = EloBuddy.Native.Vector3f.{ctor}(&vectorf, worldCoord.X, worldCoord.Y, worldCoord.Z);
                    num = color.ToArgb();
                }
                fault
                {
                    ___CxxCallUnwindDtor(std.basic_string<char,std::char_traits<char>,std::allocator<char> >.{dtor}, (void*) localPtr2);
                }
                EloBuddy.Native.r3dRenderer.DrawTexture(localPtr, vectorfPtr, radius, num);
            }
        }

        public static unsafe Size GetTextEntent(string text, int size)
        {
            Size size2 = new Size();
            IntPtr hglobal = Marshal.StringToHGlobalAnsi(text);
            tagSIZE gsize = EloBuddy.Native.Drawing.GetTextEntent((sbyte modopt(IsSignUnspecifiedByte)*) hglobal.ToPointer(), size);
            size2.Width = *((int*) &gsize);
            size2.Height = *((int*) (&gsize + 4));
            Marshal.FreeHGlobal(hglobal);
            return size2;
        }

        internal static void OnDrawingBeginSceneNative()
        {
            Exception innerException = null;
            try
            {
                foreach (DrawingBeginScene scene in DrawingBeginSceneHandlers.ToArray())
                {
                    try
                    {
                        scene(EventArgs.Empty);
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

        internal static void OnDrawingDrawNative()
        {
            Exception innerException = null;
            try
            {
                foreach (DrawingDraw draw in DrawingDrawHandlers.ToArray())
                {
                    try
                    {
                        draw(EventArgs.Empty);
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

        internal static void OnDrawingEndSceneNative()
        {
            Exception innerException = null;
            try
            {
                foreach (DrawingEndScene scene in DrawingEndSceneHandlers.ToArray())
                {
                    try
                    {
                        scene(EventArgs.Empty);
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

        internal static void OnDrawingFlushEndSceneNative()
        {
            Exception innerException = null;
            try
            {
                foreach (DrawingFlushEndScene scene in DrawingFlushEndSceneHandlers.ToArray())
                {
                    try
                    {
                        scene(EventArgs.Empty);
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

        internal static void OnDrawingPostResetNative()
        {
            Exception innerException = null;
            try
            {
                foreach (DrawingPostReset reset in DrawingPostResetHandlers.ToArray())
                {
                    try
                    {
                        reset(EventArgs.Empty);
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

        internal static void OnDrawingPreResetNative()
        {
            Exception innerException = null;
            try
            {
                foreach (DrawingPreReset reset in DrawingPreResetHandlers.ToArray())
                {
                    try
                    {
                        reset(EventArgs.Empty);
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

        internal static void OnDrawingPresentNative()
        {
            Exception innerException = null;
            try
            {
                foreach (DrawingPresent present in DrawingPresentHandlers.ToArray())
                {
                    try
                    {
                        present(EventArgs.Empty);
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

        internal static void OnDrawingSetRenderTargetNative()
        {
            Exception innerException = null;
            try
            {
                foreach (DrawingSetRenderTarget target in DrawingSetRenderTargetHandlers.ToArray())
                {
                    try
                    {
                        target(EventArgs.Empty);
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

        public static unsafe Vector3 ScreenToWorld(Vector2 pos)
        {
            Vector3f vectorf;
            Vector3f vectorf2;
            EloBuddy.Native.Vector3f.{ctor}(&vectorf2, 0f, 0f, 0f);
            EloBuddy.Native.Vector3f.{ctor}(&vectorf, 0f, 0f, 0f);
            EloBuddy.Native.r3dRenderer.r3dScreenTo3D(pos.X, pos.Y, &vectorf2, &vectorf);
            float num3 = (float) (EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf) * 1000.0);
            float num2 = (float) (EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf) * 1000.0);
            float num = (float) (EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf) * 1000.0);
            return new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf2) + num, EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf2) + num3, EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf2) + num2);
        }

        public static Vector3 ScreenToWorld(float x, float y)
        {
            Vector2 pos = new Vector2(x, y);
            return ScreenToWorld(pos);
        }

        public static Vector2 WorldToMinimap(Vector3 worldCoord) => 
            EloBuddy.TacticalMap.WorldToMinimap(worldCoord);

        public static unsafe Vector2 WorldToScreen(Vector3 worldCoord)
        {
            Vector3f vectorf;
            Vector3f vectorf2;
            EloBuddy.Native.Vector3f.{ctor}(&vectorf2, worldCoord.X, worldCoord.Y, worldCoord.Z);
            EloBuddy.Native.Vector3f.{ctor}(&vectorf);
            EloBuddy.Native.r3dRenderer.r3dProjectToScreen(&vectorf2, &vectorf);
            float modopt(CallConvThiscall) x = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
            return new Vector2(x, EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf));
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool WorldToScreen(Vector3 worldCoord, out Vector2 screenCoord)
        {
            Vector3f vectorf;
            Vector3f vectorf2;
            EloBuddy.Native.Vector3f.{ctor}(&vectorf2, worldCoord.X, worldCoord.Y, worldCoord.Z);
            EloBuddy.Native.Vector3f.{ctor}(&vectorf);
            bool flag = EloBuddy.Native.r3dRenderer.r3dProjectToScreen(&vectorf2, &vectorf);
            float modopt(CallConvThiscall) x = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
            Vector2 vector = new Vector2(x, EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf));
            screenCoord = vector;
            return flag;
        }

        public static Device Direct3DDevice
        {
            get
            {
                IDirect3DDevice9* devicePtr = EloBuddy.Native.RiotX3D.GetDirect3DDevice();
                if ((devicePtr != null) && (devicePtr != oldDX))
                {
                    oldDX = (void*) devicePtr;
                    m_device = new Device((IntPtr) devicePtr);
                }
                return m_device;
            }
        }

        public static int Height =>
            *(EloBuddy.Native.r3dRenderer.GetClientHeight(EloBuddy.Native.r3dRenderer.GetInstance()));

        public static Matrix Projection
        {
            get
            {
                _D3DMATRIX _ddmatrix;
                D3DXMATRIX* ddxmatrixPtr = EloBuddy.Native.r3dRenderer.GetProjection(EloBuddy.Native.r3dRenderer.GetInstance());
                memcpy(&_ddmatrix, ddxmatrixPtr, 0x40);
                return new Matrix(*((float*) &_ddmatrix), *((float*) (&_ddmatrix + 4)), *((float*) (&_ddmatrix + 8)), *((float*) (&_ddmatrix + 12)), *((float*) (&_ddmatrix + 0x10)), *((float*) (&_ddmatrix + 20)), *((float*) (&_ddmatrix + 0x18)), *((float*) (&_ddmatrix + 0x1c)), *((float*) (&_ddmatrix + 0x20)), *((float*) (&_ddmatrix + 0x24)), *((float*) (&_ddmatrix + 40)), *((float*) (&_ddmatrix + 0x2c)), *((float*) (&_ddmatrix + 0x30)), *((float*) (&_ddmatrix + 0x34)), *((float*) (&_ddmatrix + 0x38)), *((float*) (&_ddmatrix + 60)));
            }
        }

        public static Matrix View
        {
            get
            {
                _D3DMATRIX _ddmatrix;
                D3DXMATRIX* ddxmatrixPtr = EloBuddy.Native.r3dRenderer.GetView(EloBuddy.Native.r3dRenderer.GetInstance());
                memcpy(&_ddmatrix, ddxmatrixPtr, 0x40);
                return new Matrix(*((float*) &_ddmatrix), *((float*) (&_ddmatrix + 4)), *((float*) (&_ddmatrix + 8)), *((float*) (&_ddmatrix + 12)), *((float*) (&_ddmatrix + 0x10)), *((float*) (&_ddmatrix + 20)), *((float*) (&_ddmatrix + 0x18)), *((float*) (&_ddmatrix + 0x1c)), *((float*) (&_ddmatrix + 0x20)), *((float*) (&_ddmatrix + 0x24)), *((float*) (&_ddmatrix + 40)), *((float*) (&_ddmatrix + 0x2c)), *((float*) (&_ddmatrix + 0x30)), *((float*) (&_ddmatrix + 0x34)), *((float*) (&_ddmatrix + 0x38)), *((float*) (&_ddmatrix + 60)));
            }
        }

        public static int Width =>
            *(EloBuddy.Native.r3dRenderer.GetClientWidth(EloBuddy.Native.r3dRenderer.GetInstance()));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnDrawingBeginSceneNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnDrawingDrawNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnDrawingEndSceneNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnDrawingFlushEndSceneNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnDrawingPostResetNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnDrawingPreResetNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnDrawingPresentNativeDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void OnDrawingSetRenderTargetNativeDelegate();
    }
}

