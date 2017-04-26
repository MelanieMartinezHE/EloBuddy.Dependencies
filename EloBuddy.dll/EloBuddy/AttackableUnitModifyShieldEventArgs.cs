namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class AttackableUnitModifyShieldEventArgs : EventArgs
    {
        private float m_attackShield;
        private float m_magicShield;

        public AttackableUnitModifyShieldEventArgs(float magicShield, float attackShield)
        {
            this.m_attackShield = attackShield;
            this.m_magicShield = magicShield;
        }

        public float AttackShield =>
            this.m_attackShield;

        public float MagicShield =>
            this.m_magicShield;

        public delegate void AttackableUnitModifyShieldEvent(AttackableUnit sender, AttackableUnitModifyShieldEventArgs args);
    }
}

