namespace EloBuddy
{
    using EloBuddy.Native;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ObjectManager
    {
        internal static bool cacheCreated;
        private static List<EloBuddy.GameObject> cachedObjects = new List<EloBuddy.GameObject>();
        private static EloBuddy.AIHeroClient m_cachedPlayer;
        internal static IntPtr m_ObjectManagerAppendObjectNative = new IntPtr();
        internal static OnObjectManagerAppendObjectNativeDelegate m_ObjectManagerAppendObjectNativeDelegate;
        internal static List<ObjectManagerAppendObject> ObjectManagerAppendObjectHandlers;

        public static  event ObjectManagerAppendObject OnCreate
        {
            add
            {
                ObjectManagerAppendObjectHandlers.Add(handler);
            }
            remove
            {
                ObjectManagerAppendObjectHandlers.Remove(handler);
            }
        }

        static ObjectManager()
        {
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(ObjectManager.DomainUnloadEventHandler);
            ObjectManagerAppendObjectHandlers = new List<ObjectManagerAppendObject>();
            m_ObjectManagerAppendObjectNativeDelegate = new OnObjectManagerAppendObjectNativeDelegate(ObjectManager.OnObjectManagerAppendObjectNative);
            m_ObjectManagerAppendObjectNative = Marshal.GetFunctionPointerForDelegate(m_ObjectManagerAppendObjectNativeDelegate);
            EloBuddy.Native.EventHandler<60,bool __cdecl(EloBuddy::Native::GameObject *,char * *,int *,int *,EloBuddy::Native::Vector3f * *),EloBuddy::Native::GameObject *,char * *,int *,int *,EloBuddy::Native::Vector3f * *>.Add(EloBuddy.Native.EventHandler<60,bool __cdecl(EloBuddy::Native::GameObject *,char * *,int *,int *,EloBuddy::Native::Vector3f * *),EloBuddy::Native::GameObject *,char * *,int *,int *,EloBuddy::Native::Vector3f * *>.GetInstance(), m_ObjectManagerAppendObjectNative.ToPointer());
            EloBuddy.GameObject.OnCreate += new GameObjectCreate(ObjectManager.OnObjectCreate);
            EloBuddy.GameObject.OnDelete += new GameObjectDelete(ObjectManager.OnObjectDelete);
            if (Player != null)
            {
                RefreshCache();
            }
        }

        public static unsafe EloBuddy.GameObject CreateObjectFromPointer(EloBuddy.Native.GameObject* obj)
        {
            EloBuddy.GameObject obj2;
            uint* numPtr;
            if ((obj == null) || ((EloBuddy.Native.GameObject.GetIndex(obj) == null) && (EloBuddy.Native.GameObject.GetNetworkId(obj) == null)))
            {
                return null;
            }
            switch (EloBuddy.Native.GameObject.GetType(obj))
            {
                case 0:
                {
                    IntPtr ptr30 = (IntPtr) obj;
                    EloBuddy.GameObject obj20 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr30.IsMatch));
                    if (obj20 == null)
                    {
                        uint* numPtr30 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new NeutralMinionCamp(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr30[0], obj);
                    }
                    return obj20;
                }
                case 1:
                {
                    IntPtr ptr29 = (IntPtr) obj;
                    EloBuddy.GameObject obj19 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr29.IsMatch));
                    if (obj19 == null)
                    {
                        uint* numPtr29 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new EloBuddy.Obj_AI_Base(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr29[0], obj);
                    }
                    return obj19;
                }
                case 2:
                {
                    IntPtr ptr28 = (IntPtr) obj;
                    EloBuddy.GameObject obj18 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr28.IsMatch));
                    if (obj18 == null)
                    {
                        uint* numPtr28 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new FollowerObject(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr28[0], obj);
                    }
                    return obj18;
                }
                case 3:
                {
                    IntPtr ptr27 = (IntPtr) obj;
                    EloBuddy.GameObject obj17 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr27.IsMatch));
                    if (obj17 == null)
                    {
                        uint* numPtr27 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new FollowerObjectWithLerpMovement(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr27[0], obj);
                    }
                    return obj17;
                }
                case 4:
                {
                    IntPtr ptr26 = (IntPtr) obj;
                    EloBuddy.GameObject obj16 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr26.IsMatch));
                    if (obj16 == null)
                    {
                        uint* numPtr26 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new EloBuddy.AIHeroClient(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr26[0], obj);
                    }
                    return obj16;
                }
                case 5:
                {
                    IntPtr ptr25 = (IntPtr) obj;
                    EloBuddy.GameObject obj15 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr25.IsMatch));
                    if (obj15 == null)
                    {
                        uint* numPtr25 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_AI_Marker(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr25[0], obj);
                    }
                    return obj15;
                }
                case 6:
                {
                    IntPtr ptr24 = (IntPtr) obj;
                    EloBuddy.GameObject obj14 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr24.IsMatch));
                    if (obj14 == null)
                    {
                        uint* numPtr24 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new EloBuddy.Obj_AI_Minion(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr24[0], obj);
                    }
                    return obj14;
                }
                case 7:
                {
                    IntPtr ptr23 = (IntPtr) obj;
                    EloBuddy.GameObject obj13 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr23.IsMatch));
                    if (obj13 == null)
                    {
                        uint* numPtr23 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new LevelPropAI(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr23[0], obj);
                    }
                    return obj13;
                }
                case 8:
                {
                    IntPtr ptr22 = (IntPtr) obj;
                    EloBuddy.GameObject obj12 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr22.IsMatch));
                    if (obj12 == null)
                    {
                        uint* numPtr22 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_AI_Turret(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr22[0], obj);
                    }
                    return obj12;
                }
                case 9:
                {
                    IntPtr ptr21 = (IntPtr) obj;
                    EloBuddy.GameObject obj11 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr21.IsMatch));
                    if (obj11 == null)
                    {
                        uint* numPtr21 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_GeneralParticleEmitter(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr21[0], obj);
                    }
                    return obj11;
                }
                case 10:
                {
                    IntPtr ptr20 = (IntPtr) obj;
                    EloBuddy.GameObject obj10 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr20.IsMatch));
                    if (obj10 == null)
                    {
                        uint* numPtr20 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new EloBuddy.MissileClient(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr20[0], obj);
                    }
                    return obj10;
                }
                case 11:
                {
                    IntPtr ptr19 = (IntPtr) obj;
                    EloBuddy.GameObject obj9 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr19.IsMatch));
                    if (obj9 == null)
                    {
                        uint* numPtr19 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new DrawFX(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr19[0], obj);
                    }
                    return obj9;
                }
                case 12:
                {
                    IntPtr ptr18 = (IntPtr) obj;
                    EloBuddy.GameObject obj8 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr18.IsMatch));
                    if (obj8 == null)
                    {
                        uint* numPtr18 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new UnrevealedTarget(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr18[0], obj);
                    }
                    return obj8;
                }
                case 13:
                {
                    IntPtr ptr17 = (IntPtr) obj;
                    EloBuddy.GameObject obj7 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr17.IsMatch));
                    if (obj7 == null)
                    {
                        uint* numPtr17 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_LampBulb(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr17[0], obj);
                    }
                    return obj7;
                }
                case 14:
                {
                    IntPtr ptr16 = (IntPtr) obj;
                    EloBuddy.GameObject obj6 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr16.IsMatch));
                    if (obj6 == null)
                    {
                        uint* numPtr16 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_Barracks(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr16[0], obj);
                    }
                    return obj6;
                }
                case 15:
                {
                    IntPtr ptr15 = (IntPtr) obj;
                    EloBuddy.GameObject obj5 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr15.IsMatch));
                    if (obj5 == null)
                    {
                        uint* numPtr15 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new EloBuddy.Obj_BarracksDampener(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr15[0], obj);
                    }
                    return obj5;
                }
                case 0x10:
                {
                    IntPtr ptr14 = (IntPtr) obj;
                    EloBuddy.GameObject obj4 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr14.IsMatch));
                    if (obj4 == null)
                    {
                        uint* numPtr14 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_AnimatedBuilding(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr14[0], obj);
                    }
                    return obj4;
                }
                case 0x11:
                {
                    IntPtr ptr13 = (IntPtr) obj;
                    EloBuddy.GameObject obj3 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr13.IsMatch));
                    if (obj3 == null)
                    {
                        uint* numPtr13 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_Building(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr13[0], obj);
                    }
                    return obj3;
                }
                case 0x12:
                {
                    IntPtr ptr12 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr12.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr12 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_Levelsizer(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr12[0], obj);
                    }
                    return obj2;
                }
                case 0x13:
                {
                    IntPtr ptr11 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr11.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr11 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_NavPoint(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr11[0], obj);
                    }
                    return obj2;
                }
                case 20:
                {
                    IntPtr ptr10 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr10.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr10 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_SpawnPoint(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr10[0], obj);
                    }
                    return obj2;
                }
                case 0x15:
                {
                    IntPtr ptr9 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr9.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr9 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_Lake(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr9[0], obj);
                    }
                    return obj2;
                }
                case 0x16:
                {
                    IntPtr ptr8 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr8.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr8 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_HQ(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr8[0], obj);
                    }
                    return obj2;
                }
                case 0x17:
                {
                    IntPtr ptr7 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr7.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr7 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_InfoPoint(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr7[0], obj);
                    }
                    return obj2;
                }
                case 0x18:
                {
                    IntPtr ptr6 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr6.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr6 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new LevelPropGameObject(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr6[0], obj);
                    }
                    return obj2;
                }
                case 0x19:
                {
                    IntPtr ptr5 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr5.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr5 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new LevelPropSpawnerPoint(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr5[0], obj);
                    }
                    return obj2;
                }
                case 0x1a:
                {
                    IntPtr ptr4 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr4.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr4 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_Shop(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr4[0], obj);
                    }
                    return obj2;
                }
                case 0x1b:
                {
                    IntPtr ptr3 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr3.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr3 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_Turret(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr3[0], obj);
                    }
                    return obj2;
                }
                case 0x1c:
                {
                    IntPtr ptr2 = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr2.IsMatch));
                    if (obj2 == null)
                    {
                        uint* numPtr2 = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new GrassObject(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr2[0], obj);
                    }
                    return obj2;
                }
                case 0x1d:
                {
                    IntPtr ptr = (IntPtr) obj;
                    obj2 = cachedObjects.Find(new Predicate<EloBuddy.GameObject>(ptr.IsMatch));
                    if (obj2 == null)
                    {
                        numPtr = EloBuddy.Native.GameObject.GetNetworkId(obj);
                        return new Obj_Ward(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr[0], obj);
                    }
                    return obj2;
                }
            }
            numPtr = EloBuddy.Native.GameObject.GetNetworkId(obj);
            return new EloBuddy.GameObject(*(EloBuddy.Native.GameObject.GetIndex(obj)), numPtr[0], obj);
        }

        public static void DomainUnloadEventHandler(object A_0, EventArgs A_1)
        {
            EloBuddy.Native.EventHandler<60,bool __cdecl(EloBuddy::Native::GameObject *,char * *,int *,int *,EloBuddy::Native::Vector3f * *),EloBuddy::Native::GameObject *,char * *,int *,int *,EloBuddy::Native::Vector3f * *>.Remove(EloBuddy.Native.EventHandler<60,bool __cdecl(EloBuddy::Native::GameObject *,char * *,int *,int *,EloBuddy::Native::Vector3f * *),EloBuddy::Native::GameObject *,char * *,int *,int *,EloBuddy::Native::Vector3f * *>.GetInstance(), m_ObjectManagerAppendObjectNative.ToPointer());
        }

        public static IEnumerable<ObjectType> Get<ObjectType>() where ObjectType: EloBuddy.GameObject, new()
        {
            RefreshCache();
            if (typeof(EloBuddy.GameObject) == typeof(ObjectType))
            {
                return cachedObjects.Cast<ObjectType>();
            }
            return cachedObjects.OfType<ObjectType>();
        }

        public static EloBuddy.GameObject GetUnitByIndex(short index) => 
            CreateObjectFromPointer(EloBuddy.Native.ObjectManager.GetUnitByIndex(index));

        public static EloBuddy.GameObject GetUnitByNetworkId(uint networkId) => 
            CreateObjectFromPointer(EloBuddy.Native.ObjectManager.GetUnitByNetworkId(networkId));

        public static unsafe ObjectType GetUnitByNetworkId<ObjectType>(uint networkId) where ObjectType: EloBuddy.GameObject, new()
        {
            EloBuddy.Native.GameObject* objPtr = EloBuddy.Native.ObjectManager.GetUnitByNetworkId(networkId);
            if (objPtr != null)
            {
                return (ObjectType) CreateObjectFromPointer(objPtr);
            }
            return default(ObjectType);
        }

        public static Type NativeTypeToManagedType(UnitType type)
        {
            switch (type)
            {
                case ((UnitType) 0):
                    return typeof(NeutralMinionCamp);

                case ((UnitType) 1):
                    return typeof(EloBuddy.Obj_AI_Base);

                case ((UnitType) 2):
                    return typeof(FollowerObject);

                case ((UnitType) 3):
                    return typeof(FollowerObjectWithLerpMovement);

                case ((UnitType) 4):
                    return typeof(EloBuddy.AIHeroClient);

                case ((UnitType) 5):
                    return typeof(Obj_AI_Marker);

                case ((UnitType) 6):
                    return typeof(EloBuddy.Obj_AI_Minion);

                case ((UnitType) 7):
                    return typeof(LevelPropAI);

                case ((UnitType) 8):
                    return typeof(Obj_AI_Turret);

                case ((UnitType) 9):
                    return typeof(Obj_GeneralParticleEmitter);

                case ((UnitType) 10):
                    return typeof(EloBuddy.MissileClient);

                case ((UnitType) 11):
                    return typeof(DrawFX);

                case ((UnitType) 12):
                    return typeof(UnrevealedTarget);

                case ((UnitType) 13):
                    return typeof(Obj_LampBulb);

                case ((UnitType) 14):
                    return typeof(Obj_Barracks);

                case ((UnitType) 15):
                    return typeof(EloBuddy.Obj_BarracksDampener);

                case ((UnitType) 0x10):
                    return typeof(Obj_AnimatedBuilding);

                case ((UnitType) 0x11):
                    return typeof(Obj_Building);

                case ((UnitType) 0x12):
                    return typeof(Obj_Levelsizer);

                case ((UnitType) 0x13):
                    return typeof(Obj_NavPoint);

                case ((UnitType) 20):
                    return typeof(Obj_SpawnPoint);

                case ((UnitType) 0x15):
                    return typeof(Obj_Lake);

                case ((UnitType) 0x16):
                    return typeof(Obj_HQ);

                case ((UnitType) 0x17):
                    return typeof(Obj_InfoPoint);

                case ((UnitType) 0x18):
                    return typeof(LevelPropGameObject);

                case ((UnitType) 0x19):
                    return typeof(LevelPropSpawnerPoint);

                case ((UnitType) 0x1a):
                    return typeof(Obj_Shop);

                case ((UnitType) 0x1b):
                    return typeof(Obj_Turret);

                case ((UnitType) 0x1c):
                    return typeof(GrassObject);

                case ((UnitType) 0x1d):
                    return typeof(Obj_Ward);
            }
            return null;
        }

        internal static void OnObjectCreate(EloBuddy.GameObject sender, EventArgs args)
        {
            OnObjectDelete(sender, args);
            cachedObjects.Add(sender);
        }

        internal static void OnObjectDelete(EloBuddy.GameObject sender, EventArgs args)
        {
            EloBuddy.GameObject[] objArray = cachedObjects.ToArray();
            int index = 0;
            if (0 < objArray.Length)
            {
                do
                {
                    EloBuddy.GameObject item = objArray[index];
                    IntPtr memoryAddress = sender.MemoryAddress;
                    if (item.MemoryAddress == memoryAddress)
                    {
                        cachedObjects.Remove(item);
                    }
                    index++;
                }
                while (index < objArray.Length);
            }
        }

        internal static unsafe void OnObjectManagerAppendObjectNative(EloBuddy.Native.GameObject* A_0, sbyte modopt(IsSignUnspecifiedByte)** A_1, int* A_2, int* A_3, Vector3f** A_4)
        {
        }

        internal static unsafe void RefreshCache()
        {
            if (!cacheCreated && (EloBuddy.Game.Mode == EloBuddy.GameMode.Running))
            {
                cacheCreated = true;
                if (Player != null)
                {
                    cachedObjects.Add(Player);
                }
                EloBuddy.Native.GameObject** objPtr2 = EloBuddy.Native.ObjectManager.GetUnitArray();
                uint num2 = 0;
                do
                {
                    EloBuddy.Native.GameObject* objPtr = ((EloBuddy.Native.GameObject**) (num2 * 4))[(int) objPtr2];
                    if (objPtr != null)
                    {
                        EloBuddy.GameObject item = CreateObjectFromPointer(objPtr);
                        if (item != null)
                        {
                            if (cachedObjects.Count == 0)
                            {
                                cachedObjects.Add(item);
                            }
                            else
                            {
                                IntPtr memoryAddress = item.MemoryAddress;
                                int num = 0;
                                do
                                {
                                    IntPtr ptr = cachedObjects[num].MemoryAddress;
                                    if (memoryAddress == ptr)
                                    {
                                        break;
                                    }
                                    num++;
                                    if (num == cachedObjects.Count)
                                    {
                                        cachedObjects.Add(item);
                                    }
                                }
                                while (num < cachedObjects.Count);
                            }
                        }
                    }
                    num2++;
                }
                while (num2 < 0x2710);
            }
        }

        public static EloBuddy.AIHeroClient Player
        {
            get
            {
                if (m_cachedPlayer == null)
                {
                    EloBuddy.Native.AIHeroClient* clientPtr = EloBuddy.Native.ObjectManager.GetPlayer();
                    if (clientPtr != null)
                    {
                        uint* numPtr = EloBuddy.Native.GameObject.GetNetworkId((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) clientPtr);
                        m_cachedPlayer = new EloBuddy.AIHeroClient(*(EloBuddy.Native.GameObject.GetIndex((EloBuddy.Native.GameObject* modopt(IsConst) modopt(IsConst)) clientPtr)), numPtr[0], (EloBuddy.Native.GameObject*) clientPtr);
                    }
                }
                return m_cachedPlayer;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct EntityPredicate
        {
            private IntPtr address;
            public EntityPredicate(IntPtr address)
            {
                this.address = address;
            }

            [return: MarshalAs(UnmanagedType.U1)]
            public bool IsMatch(EloBuddy.GameObject entity) => 
                (entity.MemoryAddress == this.address);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal unsafe delegate void OnObjectManagerAppendObjectNativeDelegate(EloBuddy.Native.GameObject* A_0, sbyte modopt(IsSignUnspecifiedByte)** A_1, int* A_2, int* A_3, Vector3f** A_4);
    }
}

