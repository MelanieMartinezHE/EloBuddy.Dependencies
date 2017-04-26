namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class GameObjectProcessSpellCastEventArgs : EventArgs
    {
        private int m_castedSpellCount;
        private Vector3 m_end;
        private bool m_isToggle;
        private int m_level;
        private bool m_process;
        private EloBuddy.SpellData m_sdata;
        private EloBuddy.SpellSlot m_slot;
        private Vector3 m_start;
        private uint m_targetLocalId;
        private float m_time;

        public GameObjectProcessSpellCastEventArgs(EloBuddy.SpellData sdata, int level, Vector3 start, Vector3 end, uint targetLocalId, int counter, EloBuddy.SpellSlot slot, [MarshalAs(UnmanagedType.U1)] bool isToggle)
        {
            this.m_sdata = sdata;
            this.m_level = level;
            this.m_start = start;
            this.m_end = end;
            this.m_targetLocalId = targetLocalId;
            this.m_castedSpellCount = counter;
            this.m_time = EloBuddy.Game.Time;
            this.m_isToggle = isToggle;
            this.m_process = true;
            this.m_slot = slot;
        }

        public int CastedSpellCount =>
            this.m_castedSpellCount;

        public Vector3 End =>
            this.m_end;

        public bool IsToggle
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                byte num;
                EloBuddy.Native.SpellData* ptr = this.SData.GetPtr();
                if (ptr != null)
                {
                    num = EloBuddy.Native.SpellData.GetSDataArray(ptr)[0x39e];
                }
                else
                {
                    num = 0;
                }
                return (bool) num;
            }
        }

        public int Level =>
            this.m_level;

        public bool Process
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                this.m_process;
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                this.m_process = value;
            }
        }

        public EloBuddy.SpellData SData =>
            this.m_sdata;

        public EloBuddy.SpellSlot Slot =>
            this.m_slot;

        public Vector3 Start =>
            this.m_start;

        public EloBuddy.GameObject Target
        {
            get
            {
                uint targetLocalId = this.m_targetLocalId;
                if (targetLocalId == 0)
                {
                    return null;
                }
                EloBuddy.GameObject unitByIndex = ObjectManager.GetUnitByIndex((short) targetLocalId);
                if (unitByIndex == null)
                {
                    return null;
                }
                return unitByIndex;
            }
        }

        public float Time =>
            this.m_time;

        public delegate void GameObjectProcessSpellCastEvent(EloBuddy.Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args);
    }
}

