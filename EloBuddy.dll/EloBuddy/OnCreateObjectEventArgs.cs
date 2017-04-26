namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class OnCreateObjectEventArgs : EventArgs
    {
        private unsafe int* m_chromaId;
        private unsafe sbyte modopt(IsSignUnspecifiedByte)** m_modelName;
        private unsafe Vector3f** m_position;
        private EloBuddy.GameObject m_sender;
        private unsafe int* m_skinId;

        public unsafe OnCreateObjectEventArgs(EloBuddy.GameObject sender, sbyte modopt(IsSignUnspecifiedByte)** modelName, int* skinId, int* chromaId, Vector3f** position)
        {
            this.m_sender = sender;
            this.m_modelName = modelName;
            this.m_skinId = skinId;
            this.m_chromaId = chromaId;
            this.m_position = position;
        }

        public string Model
        {
            get => 
                new string(*((sbyte**) this.m_modelName));
            set
            {
                *((int*) this.m_modelName) = Marshal.StringToHGlobalAnsi(value).ToPointer();
            }
        }

        public Vector3 Position
        {
            get
            {
                Vector3f* vectorfPtr = this.m_position[0];
                return new Vector3(EloBuddy.Native.Vector3f.GetX((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetZ((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr), EloBuddy.Native.Vector3f.GetY((Vector3f modopt(IsConst)* modopt(IsConst) modopt(IsConst)) vectorfPtr));
            }
            set
            {
                Vector3f vectorf;
                *((int*) this.m_position) = EloBuddy.Native.Vector3f.{ctor}(&vectorf, value.X, value.Z, value.Y);
            }
        }

        public int SkinId
        {
            get => 
                this.m_skinId[0];
            set
            {
                this.m_skinId[0] = value;
            }
        }

        public EloBuddy.GameObjectTeam Team
        {
            get => 
                *(((EloBuddy.GameObjectTeam*) this.m_chromaId));
            set
            {
                this.m_chromaId[0] = (int) value;
            }
        }

        public delegate void OnCreateObjectEvent(OnCreateObjectEventArgs args);
    }
}

