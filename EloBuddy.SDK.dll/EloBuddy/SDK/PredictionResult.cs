namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Enumerations;
    using SharpDX;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class PredictionResult
    {
        private readonly int _allowedCollisionCount;
        private readonly EloBuddy.SDK.Enumerations.HitChance _hitChanceOverride;
        public readonly Vector3 CastPosition;
        public readonly Obj_AI_Base[] CollisionObjects;
        public readonly float HitChancePercent;
        public readonly Vector3 UnitPosition;

        public PredictionResult(Vector3 castPosition, Vector3 unitPosition, float hitChancePercent, Obj_AI_Base[] collisionMinions, int allowedCollisionCount, EloBuddy.SDK.Enumerations.HitChance hitChanceOverride = 0)
        {
            if (hitChancePercent < 0f)
            {
                hitChancePercent = 0f;
            }
            if (hitChancePercent > 100f)
            {
                hitChancePercent = 100f;
            }
            this.CastPosition = castPosition;
            this.UnitPosition = unitPosition;
            this.HitChancePercent = hitChancePercent;
            this.CollisionObjects = collisionMinions ?? new Obj_AI_Base[0];
            this._allowedCollisionCount = allowedCollisionCount;
            this._hitChanceOverride = hitChanceOverride;
            if (this.Collision)
            {
                this.HitChancePercent = 0f;
            }
        }

        public T[] GetCollisionObjects<T>() => 
            (from unit in this.CollisionObjects
                where unit.GetType() == typeof(T)
                select unit).Cast<T>().ToArray<T>();

        internal static PredictionResult ResultImpossible(Prediction.Position.PredictionData data, Vector3? castPos = new Vector3?(), Vector3? unitPos = new Vector3?())
        {
            Vector3? nullable = castPos;
            nullable = unitPos;
            return new PredictionResult(nullable.HasValue ? nullable.GetValueOrDefault() : Vector3.Zero, nullable.HasValue ? nullable.GetValueOrDefault() : Vector3.Zero, 0f, null, data.AllowCollisionCount, EloBuddy.SDK.Enumerations.HitChance.Impossible);
        }

        public bool Collision =>
            ((this.CollisionObjects.Length > this._allowedCollisionCount) && (this._allowedCollisionCount >= 0));

        public EloBuddy.SDK.Enumerations.HitChance HitChance
        {
            get
            {
                if (this.Collision)
                {
                    return EloBuddy.SDK.Enumerations.HitChance.Collision;
                }
                if (this._hitChanceOverride > EloBuddy.SDK.Enumerations.HitChance.Unknown)
                {
                    return this._hitChanceOverride;
                }
                if (this.HitChancePercent == 0f)
                {
                    return EloBuddy.SDK.Enumerations.HitChance.Impossible;
                }
                if (this.HitChancePercent >= 70f)
                {
                    return EloBuddy.SDK.Enumerations.HitChance.High;
                }
                if (this.HitChancePercent >= 50f)
                {
                    return EloBuddy.SDK.Enumerations.HitChance.Medium;
                }
                if (this.HitChancePercent >= 25f)
                {
                    return EloBuddy.SDK.Enumerations.HitChance.AveragePoint;
                }
                if (this.HitChancePercent > 0f)
                {
                    return EloBuddy.SDK.Enumerations.HitChance.Low;
                }
                return EloBuddy.SDK.Enumerations.HitChance.Unknown;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c__10<T>
        {
            public static readonly PredictionResult.<>c__10<T> <>9;
            public static Func<Obj_AI_Base, bool> <>9__10_0;

            static <>c__10()
            {
                PredictionResult.<>c__10<T>.<>9 = new PredictionResult.<>c__10<T>();
            }

            internal bool <GetCollisionObjects>b__10_0(Obj_AI_Base unit) => 
                (unit.GetType() == typeof(T));
        }
    }
}

