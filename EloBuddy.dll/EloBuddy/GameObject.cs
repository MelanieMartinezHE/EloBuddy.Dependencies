namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using std;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class GameObject
    {
        internal static List<GameObjectCreate> GameObjectCreateHandlers;
        internal static List<GameObjectDelete> GameObjectDeleteHandlers;
        internal static List<GameObjectFloatPropertyChange> GameObjectFloatPropertyChangeHandlers;
        internal static List<GameObjectIntegerPropertyChange> GameObjectIntegerPropertyChangeHandlers;
        internal static IntPtr m_GameObjectCreateNative = new IntPtr();
        internal static OnGameObjectCreateNativeDelegate m_GameObjectCreateNativeDelegate;
        internal static IntPtr m_GameObjectDeleteNative = new IntPtr();
        internal static OnGameObjectDeleteNativeDelegate m_GameObjectDeleteNativeDelegate;
        internal static IntPtr m_GameObjectFloatPropertyChangeNative = new IntPtr();
        internal static OnGameObjectFloatPropertyChangeNativeDelegate m_GameObjectFloatPropertyChangeNativeDelegate;
        internal static IntPtr m_GameObjectIntegerPropertyChangeNative = new IntPtr();
        internal static OnGameObjectIntegerPropertyChangeNativeDelegate m_GameObjectIntegerPropertyChangeNativeDelegate;
        protected short m_index;
        protected uint m_networkId;
        protected unsafe EloBuddy.Native.GameObject* self;

        public static  event GameObjectCreate OnCreate
        {
            add
            {
                GameObjectCreateHandlers.Add(handler);
            }
            remove
            {
                GameObjectCreateHandlers.Remove(handler);
            }
        }

        public static  event GameObjectDelete OnDelete
        {
            add
            {
                GameObjectDeleteHandlers.Add(handler);
            }
            remove
            {
                GameObjectDeleteHandlers.Remove(handler);
            }
        }

        public static  event GameObjectFloatPropertyChange OnFloatPropertyChange
        {
            add
            {
                GameObjectFloatPropertyChangeHandlers.Add(handler);
            }
            remove
            {
                GameObjectFloatPropertyChangeHandlers.Remove(handler);
            }
        }

        public static  event GameObjectIntegerPropertyChange OnIntegerPropertyChange
        {
            add
            {
                GameObjectIntegerPropertyChangeHandlers.Add(handler);
            }
            remove
            {
                GameObjectIntegerPropertyChangeHandlers.Remove(handler);
            }
        }

        static GameObject()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(EloBuddy.GameObject.DomainUnloadEventHandler);
            GameObjectCreateHandlers = new List<GameObjectCreate>();
            m_GameObjectCreateNativeDelegate = new OnGameObjectCreateNativeDelegate(EloBuddy.GameObject.OnGameObjectCreateNative);
            m_GameObjectCreateNative = Marshal.GetFunctionPointerForDelegate(m_GameObjectCreateNativeDelegate);
            EloBuddy.Native.EventHandler<13,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.Add(EloBuddy.Native.EventHandler<13,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.GetInstance(), m_GameObjectCreateNative.ToPointer());
            GameObjectDeleteHandlers = new List<GameObjectDelete>();
            m_GameObjectDeleteNativeDelegate = new OnGameObjectDeleteNativeDelegate(EloBuddy.GameObject.OnGameObjectDeleteNative);
            m_GameObjectDeleteNative = Marshal.GetFunctionPointerForDelegate(m_GameObjectDeleteNativeDelegate);
            EloBuddy.Native.EventHandler<24,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.Add(EloBuddy.Native.EventHandler<24,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.GetInstance(), m_GameObjectDeleteNative.ToPointer());
            GameObjectFloatPropertyChangeHandlers = new List<GameObjectFloatPropertyChange>();
            m_GameObjectFloatPropertyChangeNativeDelegate = new OnGameObjectFloatPropertyChangeNativeDelegate(EloBuddy.GameObject.OnGameObjectFloatPropertyChangeNative);
            m_GameObjectFloatPropertyChangeNative = Marshal.GetFunctionPointerForDelegate(m_GameObjectFloatPropertyChangeNativeDelegate);
            EloBuddy.Native.EventHandler<61,void __cdecl(EloBuddy::Native::GameObject *,char const *,float),EloBuddy::Native::GameObject *,char const *,float>.Add(EloBuddy.Native.EventHandler<61,void __cdecl(EloBuddy::Native::GameObject *,char const *,float),EloBuddy::Native::GameObject *,char const *,float>.GetInstance(), m_GameObjectFloatPropertyChangeNative.ToPointer());
            GameObjectIntegerPropertyChangeHandlers = new List<GameObjectIntegerPropertyChange>();
            m_GameObjectIntegerPropertyChangeNativeDelegate = new OnGameObjectIntegerPropertyChangeNativeDelegate(EloBuddy.GameObject.OnGameObjectIntegerPropertyChangeNative);
            m_GameObjectIntegerPropertyChangeNative = Marshal.GetFunctionPointerForDelegate(m_GameObjectIntegerPropertyChangeNativeDelegate);
            EloBuddy.Native.EventHandler<62,void __cdecl(EloBuddy::Native::GameObject *,char const *,float),EloBuddy::Native::GameObject *,char const *,int>.Add(EloBuddy.Native.EventHandler<62,void __cdecl(EloBuddy::Native::GameObject *,char const *,float),EloBuddy::Native::GameObject *,char const *,int>.GetInstance(), m_GameObjectIntegerPropertyChangeNative.ToPointer());
        }

        public GameObject()
        {
        }

        public unsafe GameObject(short index, uint networkId, EloBuddy.Native.GameObject* __unnamed002)
        {
            this.m_index = index;
            this.m_networkId = networkId;
            this.self = __unnamed002;
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<13,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.Remove(EloBuddy.Native.EventHandler<13,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.GetInstance(), m_GameObjectCreateNative.ToPointer());
            EloBuddy.Native.EventHandler<24,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.Remove(EloBuddy.Native.EventHandler<24,void __cdecl(EloBuddy::Native::GameObject *),EloBuddy::Native::GameObject *>.GetInstance(), m_GameObjectDeleteNative.ToPointer());
            EloBuddy.Native.EventHandler<61,void __cdecl(EloBuddy::Native::GameObject *,char const *,float),EloBuddy::Native::GameObject *,char const *,float>.Remove(EloBuddy.Native.EventHandler<61,void __cdecl(EloBuddy::Native::GameObject *,char const *,float),EloBuddy::Native::GameObject *,char const *,float>.GetInstance(), m_GameObjectFloatPropertyChangeNative.ToPointer());
            EloBuddy.Native.EventHandler<62,void __cdecl(EloBuddy::Native::GameObject *,char const *,float),EloBuddy::Native::GameObject *,char const *,int>.Remove(EloBuddy.Native.EventHandler<62,void __cdecl(EloBuddy::Native::GameObject *,char const *,float),EloBuddy::Native::GameObject *,char const *,int>.GetInstance(), m_GameObjectIntegerPropertyChangeNative.ToPointer());
        }

        internal unsafe EloBuddy.Native.GameObject* GetPtr()
        {
            EloBuddy.Native.GameObject* self = this.self;
            if (self == null)
            {
                EloBuddy.Native.GameObject* objPtr3 = EloBuddy.Native.ObjectManager.GetUnitByIndex(this.m_index);
                if (objPtr3 != null)
                {
                    this.self = objPtr3;
                }
                else
                {
                    EloBuddy.Native.GameObject* objPtr2 = EloBuddy.Native.ObjectManager.GetUnitByNetworkId(this.m_networkId);
                    if (objPtr2 != null)
                    {
                        this.self = objPtr2;
                    }
                }
                self = this.self;
                if (self == null)
                {
                    System.Console.WriteLine("GameObjectNotFoundException(): Index: {0} - NetworkId: {1}", this.m_index, this.m_networkId);
                    throw new GameObjectNotFoundException();
                }
            }
            return self;
        }

        internal unsafe EloBuddy.Native.GameObject* GetPtrUncached()
        {
            EloBuddy.Native.GameObject* objPtr = EloBuddy.Native.ObjectManager.GetUnitByIndex(this.m_index);
            if (objPtr != null)
            {
                return objPtr;
            }
            return EloBuddy.Native.ObjectManager.GetUnitByNetworkId(this.m_networkId);
        }

        internal static unsafe void OnGameObjectCreateNative(EloBuddy.Native.GameObject* unit)
        {
            Exception innerException = null;
            EloBuddy.GameObject sender = null;
            try
            {
                if ((unit != null) && (GameObjectCreateHandlers.Count > 0))
                {
                    sender = ObjectManager.CreateObjectFromPointer(unit);
                    if (sender != null)
                    {
                        foreach (GameObjectCreate create in GameObjectCreateHandlers.ToArray())
                        {
                            try
                            {
                                create(sender, EventArgs.Empty);
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

        internal static unsafe void OnGameObjectDeleteNative(EloBuddy.Native.GameObject* unit)
        {
            Exception innerException = null;
            try
            {
                if (unit != null)
                {
                    EloBuddy.GameObject sender = ObjectManager.CreateObjectFromPointer(unit);
                    if (sender != null)
                    {
                        uint networkId = sender.m_networkId;
                        EloBuddy.Obj_AI_Base.cachedBuffs.Remove((int) networkId);
                        foreach (GameObjectDelete delete in GameObjectDeleteHandlers.ToArray())
                        {
                            try
                            {
                                delete(sender, EventArgs.Empty);
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

        internal static unsafe void OnGameObjectFloatPropertyChangeNative(EloBuddy.Native.GameObject* A_0, sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)* A_1, float A_2)
        {
            Exception innerException = null;
            try
            {
                if ((A_0 != null) && (A_1 != null))
                {
                    EloBuddy.GameObject sender = ObjectManager.CreateObjectFromPointer(A_0);
                    GameObjectFloatPropertyChangeEventArgs args = new GameObjectFloatPropertyChangeEventArgs(A_1, A_2);
                    foreach (GameObjectFloatPropertyChange change in GameObjectFloatPropertyChangeHandlers.ToArray())
                    {
                        try
                        {
                            change(sender, args);
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

        internal static unsafe void OnGameObjectIntegerPropertyChangeNative(EloBuddy.Native.GameObject* A_0, sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)* A_1, int A_2)
        {
            Exception innerException = null;
            try
            {
                if ((A_0 != null) && (A_1 != null))
                {
                    EloBuddy.GameObject sender = ObjectManager.CreateObjectFromPointer(A_0);
                    GameObjectIntegerPropertyChangeEventArgs args = new GameObjectIntegerPropertyChangeEventArgs(A_1, A_2);
                    foreach (GameObjectIntegerPropertyChange change in GameObjectIntegerPropertyChangeHandlers.ToArray())
                    {
                        try
                        {
                            change(sender, args);
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

        public BoundingBox BBox
        {
            get
            {
                EloBuddy.Native.GameObject* ptr = this.GetPtr();
                if (ptr != null)
                {
                    EloBuddy.Native.BBox* boxPtr = EloBuddy.Native.GameObject.GetBBox(ptr);
                    Vector3 minimum = new Vector3(*((float*) boxPtr), *((float*) (boxPtr + 8)), *((float*) (boxPtr + 4)));
                    Vector3 maximum = new Vector3(*((float*) (boxPtr + 12)), *((float*) (boxPtr + 20)), *((float*) (boxPtr + 0x10)));
                    return new BoundingBox(minimum, maximum);
                }
                return new BoundingBox(Vector3.Zero, Vector3.Zero);
            }
        }

        public float BoundingRadius
        {
            get
            {
                EloBuddy.Native.GameObject* ptr = this.GetPtr();
                if (ptr != null)
                {
                    GameObjectVTable* tablePtr = EloBuddy.Native.GameObject.GetVirtual(ptr);
                    if (tablePtr != null)
                    {
                        return **(((int*) tablePtr))[0x90](tablePtr);
                    }
                }
                return 0f;
            }
        }

        public short Index =>
            this.m_index;

        public bool IsAlly
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AIHeroClient* clientPtr = EloBuddy.Native.ObjectManager.GetPlayer();
                if (clientPtr == null)
                {
                    return false;
                }
                EloBuddy.GameObjectTeam team = *(EloBuddy.Native.GameObject.GetTeam(this.GetPtr()));
                uint* numPtr = EloBuddy.Native.GameObject.GetTeam((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) clientPtr);
                return (bool) ((byte) (*(((int*) &team)) == numPtr[0]));
            }
        }

        public bool IsDead
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.GameObject* ptr = this.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.GameObject.GetIsDead(ptr));
                }
                return false;
            }
        }

        public bool IsEnemy =>
            ((bool) ((byte) !this.IsAlly));

        public bool IsMe
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.AIHeroClient* clientPtr = EloBuddy.Native.ObjectManager.GetPlayer();
                if (clientPtr == null)
                {
                    return false;
                }
                uint* numPtr = EloBuddy.Native.GameObject.GetNetworkId((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) clientPtr);
                return (bool) ((byte) (this.m_networkId == numPtr[0]));
            }
        }

        public bool IsValid
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                if (this == null)
                {
                    return false;
                }
                EloBuddy.Native.GameObject* ptr = null;
                try
                {
                    ptr = this.GetPtr();
                }
                catch (Exception)
                {
                    return false;
                }
                return (ptr != null);
            }
        }

        public bool IsVisible
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                if (!base.GetType().IsAssignableFrom(typeof(EloBuddy.Obj_AI_Base)))
                {
                    return true;
                }
                EloBuddy.Native.GameObject* ptr = this.GetPtr();
                if (ptr != null)
                {
                    GameObjectVTable* tablePtr = EloBuddy.Native.GameObject.GetVirtual(ptr);
                    if (tablePtr != null)
                    {
                        return (bool) **(((int*) tablePtr))[0x1c4](tablePtr);
                    }
                }
                return false;
            }
        }

        public IntPtr MemoryAddress =>
            ((IntPtr) this.GetPtr());

        public string Name
        {
            get
            {
                string str;
                basic_string<char,std::char_traits<char>,std::allocator<char> > local;
                EloBuddy.Native.GameObject* ptr = this.GetPtr();
                if (ptr == null)
                {
                    return "Unknown";
                }
                EloBuddy.Native.GameObject.GetName(ptr, &local);
                try
                {
                    str = msclr.interop.marshal_as<class System::String ^,class std::basic_string<char,struct std::char_traits<char>,class std::allocator<char> > >((basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsImplicitlyDereferenced)) &local);
                }
                fault
                {
                    ___CxxCallUnwindDtor(std.basic_string<char,std::char_traits<char>,std::allocator<char> >.{dtor}, (void*) &local);
                }
                std.basic_string<char,std::char_traits<char>,std::allocator<char> >._Tidy(&local, true, 0);
                return str;
            }
            set
            {
                EloBuddy.Native.GameObject* objPtr = this.GetPtr();
                if (objPtr != null)
                {
                    basic_string<char,std::char_traits<char>,std::allocator<char> > local4;
                    IntPtr hglobal = Marshal.StringToHGlobalAnsi(value);
                    std.basic_string<char,std::char_traits<char>,std::allocator<char> >.{ctor}(&local4, (sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)*) hglobal.ToPointer());
                    try
                    {
                        _String_iterator<std::_String_val<std::_Simple_types<char> > > local;
                        _String_iterator<std::_String_val<std::_Simple_types<char> > > local2;
                        basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> > local3;
                        basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >* localPtr2 = &local3;
                        sbyte modopt(IsSignUnspecifiedByte)* numPtr2 = (0x10 <= *(((int*) (&local4 + 20)))) ? ((sbyte modopt(IsSignUnspecifiedByte)*) *(((int*) &local4))) : ((sbyte modopt(IsSignUnspecifiedByte)*) &local4);
                        *((int*) &local2) = *(((int*) (&local4 + 0x10))) + numPtr2;
                        sbyte modopt(IsSignUnspecifiedByte)* numPtr = (0x10 <= *(((int*) (&local4 + 20)))) ? ((sbyte modopt(IsSignUnspecifiedByte)*) *(((int*) &local4))) : ((sbyte modopt(IsSignUnspecifiedByte)*) &local4);
                        *((int*) &local) = numPtr;
                        basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >* modopt(IsConst) modopt(IsConst) localPtr = std.basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >.{ctor}<class std::_String_iterator<class std::_String_val<struct std::_Simple_types<char> > >,void>(&local3, local, local2);
                        EloBuddy.Native.GameObject.SetName(objPtr, localPtr);
                        Marshal.FreeHGlobal(hglobal);
                    }
                    fault
                    {
                        ___CxxCallUnwindDtor(std.basic_string<char,std::char_traits<char>,std::allocator<char> >.{dtor}, (void*) &local4);
                    }
                    std.basic_string<char,std::char_traits<char>,std::allocator<char> >._Tidy(&local4, true, 0);
                }
            }
        }

        public int NetworkId =>
            ((int) this.m_networkId);

        public Vector3 Position
        {
            get
            {
                EloBuddy.Native.GameObject* ptr = this.GetPtr();
                if (ptr != null)
                {
                    Vector3f vectorf;
                    EloBuddy.Native.GameObject.GetPosition(ptr, &vectorf);
                    float modopt(CallConvThiscall) x = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                    float modopt(CallConvThiscall) y = EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                    return new Vector3(x, y, EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf));
                }
                return Vector3.Zero;
            }
        }

        public EloBuddy.GameObjectTeam Team =>
            *(EloBuddy.Native.GameObject.GetTeam(this.GetPtr()));

        public GameObjectType Type =>
            EloBuddy.Native.GameObject.GetType(this.GetPtr());

        public bool VisibleOnScreen
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.GameObject* ptr = this.GetPtr();
                if (ptr != null)
                {
                    return *(EloBuddy.Native.GameObject.GetVisibleOnScreen(ptr));
                }
                return false;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnGameObjectCreateNativeDelegate(EloBuddy.Native.GameObject* unit);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnGameObjectDeleteNativeDelegate(EloBuddy.Native.GameObject* unit);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnGameObjectFloatPropertyChangeNativeDelegate(EloBuddy.Native.GameObject* A_0, sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)* A_1, float A_2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnGameObjectIntegerPropertyChangeNativeDelegate(EloBuddy.Native.GameObject* A_0, sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)* A_1, int A_2);
    }
}

