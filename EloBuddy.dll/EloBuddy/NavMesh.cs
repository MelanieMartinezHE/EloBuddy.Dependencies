namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class NavMesh
    {
        public static List<Vector3> CreatePath(Vector3 end)
        {
            List<Vector3> list = new List<Vector3>();
            EloBuddy.AIHeroClient player = ObjectManager.Player;
            if (player != null)
            {
                Vector3[] path = player.GetPath(end);
                int index = 0;
                if (0 < path.Length)
                {
                    do
                    {
                        Vector3 item = path[index];
                        list.Add(item);
                        index++;
                    }
                    while (index < path.Length);
                }
            }
            return list;
        }

        public static List<Vector3> CreatePath(Vector3 start, Vector3 end)
        {
            List<Vector3> list = new List<Vector3>();
            EloBuddy.AIHeroClient player = ObjectManager.Player;
            if (player != null)
            {
                Vector3[] path = player.GetPath(start, end);
                int index = 0;
                if (0 < path.Length)
                {
                    do
                    {
                        Vector3 item = path[index];
                        list.Add(item);
                        index++;
                    }
                    while (index < path.Length);
                }
            }
            return list;
        }

        public static List<Vector3> CreatePath(Vector3 end, [MarshalAs(UnmanagedType.U1)] bool smoothPath)
        {
            List<Vector3> list = new List<Vector3>();
            EloBuddy.AIHeroClient player = ObjectManager.Player;
            if (player != null)
            {
                Vector3[] path = player.GetPath(end, smoothPath);
                int index = 0;
                if (0 < path.Length)
                {
                    do
                    {
                        Vector3 item = path[index];
                        list.Add(item);
                        index++;
                    }
                    while (index < path.Length);
                }
            }
            return list;
        }

        public static List<Vector3> CreatePath(Vector3 start, Vector3 end, [MarshalAs(UnmanagedType.U1)] bool smoothPath)
        {
            List<Vector3> list = new List<Vector3>();
            EloBuddy.AIHeroClient player = ObjectManager.Player;
            if (player != null)
            {
                Vector3[] vectorArray = player.GetPath(start, end, smoothPath);
                int index = 0;
                if (0 < vectorArray.Length)
                {
                    do
                    {
                        Vector3 item = vectorArray[index];
                        list.Add(item);
                        index++;
                    }
                    while (index < vectorArray.Length);
                }
            }
            return list;
        }

        public static EloBuddy.NavMeshCell GetCell(short x, short y) => 
            new EloBuddy.NavMeshCell(x, y);

        public static EloBuddy.NavMeshCell GetCell(int x, int y) => 
            new EloBuddy.NavMeshCell(x, y);

        public static unsafe EloBuddy.CollisionFlags GetCollisionFlags(Vector2 position)
        {
            float x = position.X;
            float y = position.Y;
            EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
            if (meshPtr != null)
            {
                return EloBuddy.Native.NavMesh.GetCollisionFlags(meshPtr, x, y);
            }
            return EloBuddy.CollisionFlags.None;
        }

        public static unsafe EloBuddy.CollisionFlags GetCollisionFlags(Vector3 position)
        {
            float x = position.X;
            float y = position.Y;
            EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
            if (meshPtr != null)
            {
                return EloBuddy.Native.NavMesh.GetCollisionFlags(meshPtr, x, y);
            }
            return EloBuddy.CollisionFlags.None;
        }

        public static unsafe EloBuddy.CollisionFlags GetCollisionFlags(float x, float y)
        {
            EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
            if (meshPtr != null)
            {
                return (EloBuddy.CollisionFlags) ((short) EloBuddy.Native.NavMesh.GetCollisionFlags(meshPtr, x, y));
            }
            return EloBuddy.CollisionFlags.None;
        }

        public static unsafe float GetHeightForPosition(float x, float y)
        {
            EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
            if (meshPtr != null)
            {
                return EloBuddy.Native.NavMesh.GetHeightForPosition(meshPtr, x, y);
            }
            return 0f;
        }

        public static unsafe Vector3 GridToWorld(short x, short y)
        {
            if (EloBuddy.Native.NavMesh.GetInstance() != null)
            {
                Vector3f vectorf;
                EloBuddy.Native.NavMesh.CellToWorld(&vectorf, x, y);
                float modopt(CallConvThiscall) introduced2 = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                float modopt(CallConvThiscall) introduced3 = EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                return new Vector3(introduced2, introduced3, EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf));
            }
            return Vector3.Zero;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool IsWallOfGrass(Vector3 pos, float radius)
        {
            EloBuddy.CollisionFlags none;
            if (EloBuddy.Native.NavMesh.GetInstance() == null)
            {
                return false;
            }
            float x = pos.X;
            float y = pos.Y;
            EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
            if (meshPtr != null)
            {
                none = EloBuddy.Native.NavMesh.GetCollisionFlags(meshPtr, x, y);
            }
            else
            {
                none = EloBuddy.CollisionFlags.None;
            }
            return ((EloBuddy.CollisionFlags) *(((short*) &none))).HasFlag(EloBuddy.CollisionFlags.Grass);
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool IsWallOfGrass(float x, float y, float radius)
        {
            EloBuddy.CollisionFlags none;
            if (EloBuddy.Native.NavMesh.GetInstance() == null)
            {
                return false;
            }
            EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
            if (meshPtr != null)
            {
                none = EloBuddy.Native.NavMesh.GetCollisionFlags(meshPtr, x, y);
            }
            else
            {
                none = EloBuddy.CollisionFlags.None;
            }
            return ((EloBuddy.CollisionFlags) *(((short*) &none))).HasFlag(EloBuddy.CollisionFlags.Grass);
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool LineOfSightTest(Vector3 begin, Vector3 end)
        {
            EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
            if (meshPtr != null)
            {
                Vector3f vectorf;
                Vector3f vectorf2;
                EloBuddy.Native.Vector3f.{ctor}(&vectorf2, 0f, 0f, 0f);
                EloBuddy.Native.Vector3f.{ctor}(&vectorf, 0f, 0f, 0f);
                return EloBuddy.Native.NavMesh.LineOfSightTest(meshPtr, &vectorf2, &vectorf);
            }
            return false;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool SetCollisionFlags(EloBuddy.CollisionFlags flags, Vector2 position)
        {
            byte num;
            float x = position.X;
            float y = position.Y;
            EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
            if (meshPtr != null)
            {
                EloBuddy.Native.NavMesh.SetCollisionFlags(meshPtr, x, y, (EloBuddy.Native.CollisionFlags) flags);
                num = 1;
            }
            else
            {
                num = 0;
            }
            return (bool) num;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool SetCollisionFlags(EloBuddy.CollisionFlags flags, Vector3 position)
        {
            byte num;
            float x = position.X;
            float y = position.Y;
            EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
            if (meshPtr != null)
            {
                EloBuddy.Native.NavMesh.SetCollisionFlags(meshPtr, x, y, (EloBuddy.Native.CollisionFlags) flags);
                num = 1;
            }
            else
            {
                num = 0;
            }
            return (bool) num;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool SetCollisionFlags(EloBuddy.CollisionFlags flags, float x, float y)
        {
            EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
            if (meshPtr != null)
            {
                EloBuddy.Native.NavMesh.SetCollisionFlags(meshPtr, x, y, (EloBuddy.Native.CollisionFlags) flags);
                return true;
            }
            return false;
        }

        public static unsafe Vector2 WorldToGrid(float x, float y)
        {
            if (EloBuddy.Native.NavMesh.GetInstance() != null)
            {
                Vector3f vectorf;
                EloBuddy.Native.NavMesh.WorldToCell(&vectorf, x, y);
                float modopt(CallConvThiscall) introduced2 = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                return new Vector2(introduced2, EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf));
            }
            return Vector2.Zero;
        }

        public static float CellHeight
        {
            get
            {
                EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
                if (meshPtr != null)
                {
                    return *(EloBuddy.Native.NavMesh.GetCellHeight(meshPtr));
                }
                return 0f;
            }
        }

        public static float CellWidth
        {
            get
            {
                EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
                if (meshPtr != null)
                {
                    return *(EloBuddy.Native.NavMesh.GetCellWidth(meshPtr));
                }
                return 0f;
            }
        }

        public static short Height
        {
            get
            {
                EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
                if (meshPtr != null)
                {
                    return *(EloBuddy.Native.NavMesh.GetHeightDelta(meshPtr));
                }
                return 0;
            }
        }

        public static short Width
        {
            get
            {
                EloBuddy.Native.NavMesh* meshPtr = EloBuddy.Native.NavMesh.GetInstance();
                if (meshPtr != null)
                {
                    return *(EloBuddy.Native.NavMesh.GetWidthDelta(meshPtr));
                }
                return 0;
            }
        }
    }
}

