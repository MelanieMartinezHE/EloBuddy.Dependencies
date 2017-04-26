namespace EloBuddy
{
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class SpellbookCastSpellEventArgs : EventArgs
    {
        private Vector3 m_endPosition;
        private bool m_process;
        private SpellSlot m_slot;
        private Vector3 m_startPosition;
        private GameObject m_target;

        public SpellbookCastSpellEventArgs(Vector3 startPos, Vector3 endPos, GameObject target, SpellSlot slot)
        {
            this.m_startPosition = startPos;
            this.m_endPosition = endPos;
            this.m_target = target;
            this.m_slot = slot;
            this.m_process = true;
        }

        public Vector3 EndPosition =>
            this.m_endPosition;

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

        public SpellSlot Slot
        {
            get => 
                this.m_slot;
            set
            {
                this.m_slot = value;
            }
        }

        public Vector3 StartPosition =>
            this.m_startPosition;

        public GameObject Target =>
            this.m_target;

        public delegate void SpellbookCastSpellEvent(Spellbook sender, SpellbookCastSpellEventArgs args);
    }
}

