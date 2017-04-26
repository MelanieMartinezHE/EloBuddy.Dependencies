namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using std;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ManagedTexture
    {
        private System.Drawing.Color m_color;
        private Vector3 m_position;
        internal static IntPtr m_r3dTextureLoadNative = new IntPtr();
        internal static Onr3dTextureLoadNativeDelegate m_r3dTextureLoadNativeDelegate;
        private float m_size;
        internal unsafe r3dTexture* m_texture;
        private string m_textureName;
        private IntPtr m_texturePtr;
        internal static List<r3dTextureLoad> r3dTextureLoadHandlers;

        public static  event r3dTextureLoad OnLoad
        {
            add
            {
                r3dTextureLoadHandlers.Add(handler);
            }
            remove
            {
                r3dTextureLoadHandlers.Remove(handler);
            }
        }

        static ManagedTexture()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(ManagedTexture.DomainUnloadEventHandler);
            r3dTextureLoadHandlers = new List<r3dTextureLoad>();
            m_r3dTextureLoadNativeDelegate = new Onr3dTextureLoadNativeDelegate(ManagedTexture.Onr3dTextureLoadNative);
            m_r3dTextureLoadNative = Marshal.GetFunctionPointerForDelegate(m_r3dTextureLoadNativeDelegate);
            EloBuddy.Native.EventHandler<100,bool __cdecl(EloBuddy::Native::r3dTexture *,std::basic_string<char,std::char_traits<char>,std::allocator<char> > *,unsigned long *),EloBuddy::Native::r3dTexture *,std::basic_string<char,std::char_traits<char>,std::allocator<char> > *,unsigned long *>.Add(EloBuddy.Native.EventHandler<100,bool __cdecl(EloBuddy::Native::r3dTexture *,std::basic_string<char,std::char_traits<char>,std::allocator<char> > *,unsigned long *),EloBuddy::Native::r3dTexture *,std::basic_string<char,std::char_traits<char>,std::allocator<char> > *,unsigned long *>.GetInstance(), m_r3dTextureLoadNative.ToPointer());
        }

        public ManagedTexture(string texture)
        {
            this.m_textureName = texture;
            this.Load();
        }

        public ManagedTexture(string texture, Vector3 position, System.Drawing.Color color, float size)
        {
            this.m_textureName = texture;
            this.m_position = position;
            this.m_color = color;
            this.m_size = size;
            this.Load();
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<100,bool __cdecl(EloBuddy::Native::r3dTexture *,std::basic_string<char,std::char_traits<char>,std::allocator<char> > *,unsigned long *),EloBuddy::Native::r3dTexture *,std::basic_string<char,std::char_traits<char>,std::allocator<char> > *,unsigned long *>.Remove(EloBuddy.Native.EventHandler<100,bool __cdecl(EloBuddy::Native::r3dTexture *,std::basic_string<char,std::char_traits<char>,std::allocator<char> > *,unsigned long *),EloBuddy::Native::r3dTexture *,std::basic_string<char,std::char_traits<char>,std::allocator<char> > *,unsigned long *>.GetInstance(), m_r3dTextureLoadNative.ToPointer());
        }

        public unsafe void Draw()
        {
            if (this.m_texture != null)
            {
                EloBuddy.Drawing.DrawTexture(this, this.m_position, this.m_size, this.m_color);
            }
        }

        public unsafe void Draw(Vector3 position)
        {
            if (this.m_texture != null)
            {
                System.Drawing.Color white = System.Drawing.Color.White;
                EloBuddy.Drawing.DrawTexture(this, position, 250f, white);
            }
        }

        public unsafe void Draw(Vector3 position, float size)
        {
            if (this.m_texture != null)
            {
                System.Drawing.Color white = System.Drawing.Color.White;
                EloBuddy.Drawing.DrawTexture(this, position, size, white);
            }
        }

        public unsafe void Draw(Vector3 __unnamed000, float size, System.Drawing.Color c)
        {
            if (this.m_texture != null)
            {
                EloBuddy.Drawing.DrawTexture(this, __unnamed000, size, c);
            }
        }

        public unsafe void Load()
        {
            basic_string<char,std::char_traits<char>,std::allocator<char> >* modopt(IsConst) modopt(IsConst) localPtr2;
            basic_string<char,std::char_traits<char>,std::allocator<char> >* localPtr = @new(0x18);
            try
            {
                if (localPtr != null)
                {
                    IntPtr ptr2 = Marshal.StringToHGlobalAnsi(this.m_textureName);
                    localPtr2 = std.basic_string<char,std::char_traits<char>,std::allocator<char> >.{ctor}(localPtr, (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) ptr2.ToPointer());
                }
                else
                {
                    localPtr2 = 0;
                }
            }
            fault
            {
                delete((void*) localPtr);
            }
            r3dTexture* texturePtr = EloBuddy.Native.r3dRenderLayer.LoadTexture(localPtr2);
            this.m_texture = texturePtr;
            IntPtr ptr = (IntPtr) texturePtr;
            this.m_texturePtr = ptr;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        internal static unsafe bool Onr3dTextureLoadNative(r3dTexture* __unnamed000, basic_string<char,std::char_traits<char>,std::allocator<char> >* textureName, uint modopt(IsLong)* sharedMemory)
        {
            Exception innerException = null;
            r3dTextureLoad[] loadArray = null;
            r3dTextureLoad load = null;
            bool flag = true;
            try
            {
                OnLoadTextureEventArgs args = new OnLoadTextureEventArgs(new string((0x10 > *(((int*) (textureName + 20)))) ? ((sbyte*) textureName) : ((sbyte*) *(((int*) textureName)))));
                loadArray = r3dTextureLoadHandlers.ToArray();
                int index = 0;
                while (true)
                {
                    if (index >= loadArray.Length)
                    {
                        break;
                    }
                    load = loadArray[index];
                    try
                    {
                        load(args);
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

        public IntPtr ManagedPointer =>
            this.m_texturePtr;

        public IntPtr* NativePointer =>
            ((IntPtr*) ((int) this.m_texture));

        public string TextureName
        {
            get => 
                this.m_textureName;
            set
            {
                this.m_textureName = value;
                this.Load();
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate bool Onr3dTextureLoadNativeDelegate(r3dTexture* __unnamed000, basic_string<char,std::char_traits<char>,std::allocator<char> >* textureName, uint modopt(IsLong)* sharedMemory);
    }
}

