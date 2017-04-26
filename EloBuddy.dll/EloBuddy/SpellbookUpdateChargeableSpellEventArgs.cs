namespace EloBuddy
{
    using SharpDX;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class SpellbookUpdateChargeableSpellEventArgs : EventArgs
    {
        private Vector3 m_position;
        private bool m_process;
        private bool m_releaseCast;
        private SpellSlot m_slot;

        public SpellbookUpdateChargeableSpellEventArgs(SpellSlot slot, Vector3 position, [MarshalAs(UnmanagedType.U1)] bool releaseCast)
        {
            this.m_slot = slot;
            this.m_position = position;
            this.m_releaseCast = releaseCast;
            this.m_process = true;
        }

        public Vector3 Position =>
            this.m_position;

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

        public bool ReleaseCast =>
            this.m_releaseCast;

        public SpellSlot Slot =>
            this.m_slot;

        public delegate void SpellbookUpdateChargeableSpellEvent(Spellbook sender, SpellbookUpdateChargeableSpellEventArgs args);
    }
}

