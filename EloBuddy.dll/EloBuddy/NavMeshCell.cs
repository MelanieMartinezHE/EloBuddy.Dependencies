namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;

    public class NavMeshCell
    {
        private short m_x;
        private short m_y;

        public NavMeshCell(short x, short y)
        {
            this.m_x = x;
            this.m_y = y;
        }

        public NavMeshCell(int x, int y)
        {
            this.m_x = (short) x;
            this.m_y = (short) y;
        }

        public EloBuddy.CollisionFlags CollFlags
        {
            get
            {
                if (EloBuddy.Native.NavMesh.GetInstance() != null)
                {
                    Vector3 worldPosition = this.WorldPosition;
                    return (EloBuddy.CollisionFlags) ((short) EloBuddy.Native.NavMesh.GetCollisionFlags(EloBuddy.Native.NavMesh.GetInstance(), worldPosition.X, worldPosition.Y));
                }
                return EloBuddy.CollisionFlags.None;
            }
            set
            {
                if (EloBuddy.Native.NavMesh.GetInstance() != null)
                {
                    Vector3 worldPosition = this.WorldPosition;
                    EloBuddy.Native.NavMesh.SetCollisionFlags(EloBuddy.Native.NavMesh.GetInstance(), worldPosition.X, worldPosition.Y, (EloBuddy.Native.CollisionFlags) value);
                }
            }
        }

        public short GridX =>
            this.m_x;

        public short GridY =>
            this.m_y;

        public Vector3 WorldPosition
        {
            get
            {
                if (EloBuddy.Native.NavMesh.GetInstance() != null)
                {
                    Vector3f vectorf;
                    EloBuddy.Native.NavMesh.CellToWorld(&vectorf, this.m_x, this.m_y);
                    float modopt(CallConvThiscall) x = EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                    float modopt(CallConvThiscall) y = EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf);
                    return new Vector3(x, y, EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) &vectorf));
                }
                return Vector3.Zero;
            }
        }
    }
}

