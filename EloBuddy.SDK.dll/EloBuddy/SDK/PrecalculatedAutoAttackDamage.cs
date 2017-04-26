namespace EloBuddy.SDK
{
    using EloBuddy;
    using System;

    internal class PrecalculatedAutoAttackDamage
    {
        internal DamageType _autoAttackDamageType = DamageType.Physical;
        internal float _calculatedMagical;
        internal float _calculatedPhysical;
        internal float _calculatedTrue;
        internal float _rawMagical;
        internal float _rawPhysical;
        internal float _rawTotal;
    }
}

