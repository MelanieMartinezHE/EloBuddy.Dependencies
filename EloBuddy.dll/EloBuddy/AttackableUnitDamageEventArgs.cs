namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class AttackableUnitDamageEventArgs : EventArgs
    {
        private float m_damage;
        private DamageType m_damageType;
        private float m_gameTime;
        private DamageHitType m_hitType;
        private AttackableUnit m_source;
        private AttackableUnit m_target;

        public AttackableUnitDamageEventArgs(AttackableUnit source, AttackableUnit target, DamageHitType hitType, DamageType damageType, float damage, float gameTime)
        {
            this.m_source = source;
            this.m_target = target;
            this.m_hitType = hitType;
            this.m_damageType = damageType;
            this.m_damage = damage;
        }

        public float Damage =>
            this.m_damage;

        public DamageHitType HitType =>
            this.m_hitType;

        public AttackableUnit Source =>
            this.m_source;

        public AttackableUnit Target =>
            this.m_target;

        public DamageType Type =>
            this.m_damageType;

        public delegate void AttackableUnitDamageEvent(AttackableUnit sender, AttackableUnitDamageEventArgs args);
    }
}

