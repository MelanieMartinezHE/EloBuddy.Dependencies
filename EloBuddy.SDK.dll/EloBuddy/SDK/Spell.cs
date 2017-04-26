namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Rendering;
    using EloBuddy.SDK.Spells;
    using EloBuddy.SDK.Utils;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class Spell
    {
        public class Active : Spell.SpellBase
        {
            public Active(SpellSlot spellSlot, uint spellRange = 0xffffffff, DamageType dmgType = 3) : base(spellSlot, spellRange, dmgType)
            {
            }

            public override bool Cast()
            {
                if (!this.PrecheckCast(null))
                {
                    return false;
                }
                Player.CastSpell(base.Slot);
                return true;
            }

            public override bool Cast(Obj_AI_Base targetEntity) => 
                (base.IsInRange(targetEntity) && this.Cast());

            public override bool Cast(Vector3 targetPosition) => 
                (base.IsInRange(targetPosition) && this.Cast());
        }

        public enum CastFailures
        {
            TargetOutOfRange,
            SpellNotReady,
            MinimumHitChance,
            SpellHumanized
        }

        public class Chargeable : Spell.Skillshot
        {
            internal int _releaseCastSent;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <ChargingStartedTime>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <FullyChargedCastsOnly>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <FullyChargedTime>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsCharging>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private uint <MaximumRange>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private uint <MinimumRange>k__BackingField;

            public Chargeable(SpellSlot spellSlot, uint minimumRange, uint maximumRange, int fullyChargedTime, int castDelay = 250, int? spellSpeed = new int?(), int? spellWidth = new int?(), DamageType dmgType = 3) : base(spellSlot, minimumRange, SkillShotType.Linear, castDelay, spellSpeed, spellWidth, dmgType)
            {
                this.MinimumRange = minimumRange;
                this.MaximumRange = maximumRange;
                this.FullyChargedTime = fullyChargedTime;
                Spellbook.OnCastSpell += new SpellbookCastSpell(this.OnCastSpell);
                Spellbook.OnUpdateChargeableSpell += new SpellbookUpdateChargeableSpell(this.OnUpdateChargeableSpell);
                Spellbook.OnStopCast += new SpellbookStopCast(this.OnStopCast);
                Obj_AI_Base.OnProcessSpellCast += new Obj_AI_ProcessSpellCast(this.OnProcessSpellCast);
            }

            public override bool Cast(Obj_AI_Base targetEntity)
            {
                if (this.IsCharging)
                {
                    PredictionResult prediction = this.GetPrediction(targetEntity);
                    return ((prediction.HitChance >= base.MinimumHitChance) && this.Cast(prediction.CastPosition));
                }
                return false;
            }

            public override bool Cast(Vector3 targetPosition)
            {
                if ((this.IsCharging && ((Core.GameTickCount - this.ChargingStartedTime) > 0)) && (!this.FullyChargedCastsOnly || this.IsFullyCharged))
                {
                    Player.CastSpell(base.Slot, targetPosition);
                    return true;
                }
                return false;
            }

            public override PredictionResult GetPrediction(Obj_AI_Base target) => 
                Prediction.Position.PredictLinearMissile(target, this.IsCharging ? ((float) this.Range) : ((float) this.MaximumRange), base.Width, base.CastDelay, (float) base.Speed, base.AllowedCollisionCount, base.SourcePosition, false);

            private void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
            {
                if (sender.Owner.IsMe && (args.Slot == base.Slot))
                {
                    if (this.IsCharging)
                    {
                        if ((Core.GameTickCount - this.ChargingStartedTime) > 500)
                        {
                            this.IsCharging = false;
                            Player.Instance.Spellbook.UpdateChargeableSpell(base.Slot, args.EndPosition, true, true);
                        }
                        else
                        {
                            args.Process = false;
                        }
                    }
                    else
                    {
                        this.StartCharging();
                    }
                }
            }

            private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
            {
                if (((sender.IsMe && (args.Slot == base.Slot)) && !this.IsCharging) && ((Core.GameTickCount - this._releaseCastSent) > ((Game.Ping / 2) + 100)))
                {
                    this.IsCharging = true;
                    this.ChargingStartedTime = Core.GameTickCount - Game.Ping;
                }
            }

            private void OnStopCast(Obj_AI_Base sender, SpellbookStopCastEventArgs args)
            {
                if (sender.IsMe)
                {
                    this.IsCharging = false;
                }
            }

            private void OnUpdateChargeableSpell(Spellbook sender, SpellbookUpdateChargeableSpellEventArgs args)
            {
                if ((sender.Owner.IsMe && (args.Slot == base.Slot)) && args.ReleaseCast)
                {
                    this._releaseCastSent = Core.GameTickCount;
                    this.IsCharging = false;
                }
            }

            public bool StartCharging()
            {
                if ((!this.IsCharging && base.IsReady(0)) && ((Core.GameTickCount - this._releaseCastSent) > 500))
                {
                    this.IsCharging = true;
                    this.ChargingStartedTime = Core.GameTickCount - Game.Ping;
                    Player.CastSpell(base.Slot, Game.CursorPos, false);
                    return true;
                }
                return this.IsCharging;
            }

            public int ChargingStartedTime { get; protected set; }

            public bool FullyChargedCastsOnly { get; set; }

            public int FullyChargedTime { get; protected set; }

            public bool IsCharging { get; protected set; }

            public bool IsFullyCharged =>
                (((Core.GameTickCount - this.ChargingStartedTime) > this.FullyChargedTime) && this.IsCharging);

            public uint MaximumRange { get; protected set; }

            public uint MinimumRange { get; protected set; }

            public override uint Range
            {
                get => 
                    (!this.IsCharging ? base.Range : Convert.ToUInt32(Math.Min((float) this.MaximumRange, this.MinimumRange + ((this.MaximumRange - this.MinimumRange) * (((float) (Core.GameTickCount - this.ChargingStartedTime)) / ((float) this.FullyChargedTime))))));
                set
                {
                    base.Range = value;
                }
            }
        }

        public interface ISpell
        {
            bool Cast();
            bool Cast(Obj_AI_Base targetEntity);
            bool Cast(Vector3 targetPosition);
        }

        public abstract class Ranged : Spell.SpellBase
        {
            protected Ranged(SpellSlot spellSlot, uint spellRange, DamageType dmgType = 3) : base(spellSlot, spellRange, dmgType)
            {
            }

            public override bool Cast()
            {
                throw new SpellCastException("Can't cast ranged spell without target!");
            }

            protected override bool PrecheckCast(Vector3? position = new Vector3?())
            {
                if (!position.HasValue)
                {
                    throw new ArgumentNullException("position");
                }
                if (base.PrecheckCast(position))
                {
                    if (!base.IsInRange(position.Value))
                    {
                        base.LastCastFailure = Spell.CastFailures.TargetOutOfRange;
                        return false;
                    }
                    return true;
                }
                return false;
            }
        }

        public class SimpleSkillshot : Spell.SpellBase
        {
            public SimpleSkillshot(SpellSlot spellSlot, uint spellRange = 0xffffffff, DamageType dmgType = 3) : base(spellSlot, spellRange, dmgType)
            {
            }

            public override bool Cast()
            {
                throw new SpellCastException("Can't cast a Simple Skillshot spell without a position!");
            }

            public override bool Cast(Obj_AI_Base targetEntity)
            {
                throw new SpellCastException("Can't cast a Simple Skillshot spell on a target!");
            }

            public override bool Cast(Vector3 targetPosition)
            {
                if (!this.PrecheckCast(new Vector3?(targetPosition)))
                {
                    return false;
                }
                Player.CastSpell(base.Slot, targetPosition);
                return true;
            }
        }

        public class Skillshot : Spell.Ranged
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <AllowedCollisionCount>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <ConeAngleDegrees>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private HitChance <MinimumHitChance>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Vector3? <SourcePosition>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Speed>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SkillShotType <Type>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Width>k__BackingField;

            public Skillshot(SpellSlot slot, DamageType dmgType = 3) : base(slot, 0, dmgType)
            {
                List<SpellInfo> spellInfoList = SpellDatabase.GetSpellInfoList(Player.Instance);
                if (spellInfoList == null)
                {
                    Console.WriteLine("Skillshot information for this champion doesnt exist or isnt implemented yet, please contact a developer.");
                }
                else
                {
                    SpellInfo info = spellInfoList.FirstOrDefault<SpellInfo>(i => i.Slot == slot);
                    if (info == null)
                    {
                        Console.WriteLine("The info for the spell " + slot + " doesnt exist or isnt implemented yet, please contact a developer.");
                    }
                    else
                    {
                        this.Range = (uint) info.Range;
                        this.Speed = (int) info.MissileSpeed;
                        this.Width = (int) info.Radius;
                        base.CastDelay = (int) (info.Delay * 1000f);
                        switch (info.Type)
                        {
                            case SpellType.Circle:
                                this.Type = SkillShotType.Circular;
                                break;

                            case SpellType.Line:
                                this.Type = SkillShotType.Linear;
                                break;

                            case SpellType.Cone:
                                this.Type = SkillShotType.Cone;
                                this.ConeAngleDegrees = (int) info.Radius;
                                break;

                            case SpellType.Arc:
                                this.Type = SkillShotType.Circular;
                                break;

                            case SpellType.MissileLine:
                                this.Type = SkillShotType.Linear;
                                break;

                            case SpellType.MissileAoe:
                                this.Type = SkillShotType.Circular;
                                break;
                        }
                        this.MinimumHitChance = HitChance.Medium;
                    }
                }
            }

            public Skillshot(SpellSlot spellSlot, uint spellRange, SkillShotType skillShotType, int castDelay = 250, int? spellSpeed = new int?(), int? spellWidth = new int?(), DamageType dmgType = 3) : base(spellSlot, spellRange, dmgType)
            {
                this.Type = skillShotType;
                base.CastDelay = castDelay;
                int? nullable = spellSpeed;
                this.Speed = nullable.HasValue ? nullable.GetValueOrDefault() : 0;
                nullable = spellWidth;
                this.Width = nullable.HasValue ? nullable.GetValueOrDefault() : 0;
                this.ConeAngleDegrees = 90;
                this.MinimumHitChance = HitChance.Medium;
            }

            public override bool Cast(Obj_AI_Base targetEntity)
            {
                if (this.PrecheckCast(new Vector3?(targetEntity.ServerPosition)))
                {
                    if (this.AllowedCollisionCount < 0x7fffffff)
                    {
                        Vector3? nullable2;
                        Vector3? sourcePosition = this.SourcePosition;
                        Vector3 start = sourcePosition.HasValue ? sourcePosition.GetValueOrDefault() : ((nullable2 = base.RangeCheckSource).HasValue ? nullable2.GetValueOrDefault() : Player.Instance.ServerPosition);
                        if (start.WillCollideWithYasuoWall(targetEntity, true))
                        {
                            return false;
                        }
                    }
                    PredictionResult prediction = this.GetPrediction(targetEntity);
                    if (prediction.HitChance < this.MinimumHitChance)
                    {
                        base.LastCastFailure = Spell.CastFailures.MinimumHitChance;
                        return false;
                    }
                    if (((Obj_AI_Base) Player.Instance).Distance(prediction.UnitPosition, true) < base.RangeSquared)
                    {
                        Player.CastSpell(base.Slot, prediction.CastPosition);
                        return true;
                    }
                }
                return false;
            }

            public override bool Cast(Vector3 targetPosition)
            {
                if (!this.PrecheckCast(new Vector3?(targetPosition)))
                {
                    return false;
                }
                if (this.AllowedCollisionCount < 0x7fffffff)
                {
                    Vector3? nullable2;
                    Vector3? sourcePosition = this.SourcePosition;
                    Vector3 start = sourcePosition.HasValue ? sourcePosition.GetValueOrDefault() : ((nullable2 = base.RangeCheckSource).HasValue ? nullable2.GetValueOrDefault() : Player.Instance.ServerPosition);
                    if (start.WillCollideWithYasuoWall(targetPosition, true))
                    {
                        return false;
                    }
                }
                Player.CastSpell(base.Slot, targetPosition);
                return true;
            }

            public virtual bool CastIfItWillHit(int minTargets = 2, int minHitchancePercent = 0x4b)
            {
                switch (this.Type)
                {
                    case SkillShotType.Linear:
                    {
                        AIHeroClient[] entities = EntityManager.Heroes.Enemies.Where<AIHeroClient>(new Func<AIHeroClient, bool>(this.CanCast)).ToArray<AIHeroClient>();
                        BestPosition position = this.GetBestLinearCastPosition(entities, 0, null);
                        if (((position.CastPosition == Vector3.Zero) || (position.HitNumber <= 0)) || (position.HitNumber < minTargets))
                        {
                            break;
                        }
                        return this.Cast(position.CastPosition);
                    }
                    case SkillShotType.Circular:
                    {
                        AIHeroClient[] clientArray2 = EntityManager.Heroes.Enemies.Where<AIHeroClient>(new Func<AIHeroClient, bool>(this.CanCast)).ToArray<AIHeroClient>();
                        BestPosition position2 = this.GetBestLinearCastPosition(clientArray2, 0, null);
                        if (((position2.CastPosition == Vector3.Zero) || (position2.HitNumber <= 0)) || (position2.HitNumber < minTargets))
                        {
                            break;
                        }
                        return this.Cast(position2.CastPosition);
                    }
                    case SkillShotType.Cone:
                    {
                        AIHeroClient[] clientArray3 = EntityManager.Heroes.Enemies.Where<AIHeroClient>(new Func<AIHeroClient, bool>(this.CanCast)).ToArray<AIHeroClient>();
                        BestPosition position3 = this.GetBestLinearCastPosition(clientArray3, 0, null);
                        if (((position3.CastPosition == Vector3.Zero) || (position3.HitNumber <= 0)) || (position3.HitNumber < minTargets))
                        {
                            break;
                        }
                        return this.Cast(position3.CastPosition);
                    }
                }
                return false;
            }

            public virtual bool CastMinimumHitchance(Obj_AI_Base target, HitChance hitChance = 5)
            {
                if (target == null)
                {
                    return false;
                }
                PredictionResult prediction = this.GetPrediction(target);
                return ((prediction.HitChance >= hitChance) && this.Cast(prediction.CastPosition));
            }

            public virtual bool CastMinimumHitchance(Obj_AI_Base target, int hitChancePercent = 60)
            {
                if (target == null)
                {
                    return false;
                }
                PredictionResult prediction = this.GetPrediction(target);
                return ((prediction.HitChancePercent >= hitChancePercent) && this.Cast(prediction.CastPosition));
            }

            public virtual bool CastOnBestFarmPosition(int minMinion = 3, int hitChance = 50)
            {
                switch (this.Type)
                {
                    case SkillShotType.Linear:
                    {
                        IOrderedEnumerable<Obj_AI_Minion> enumerable = from m in EntityManager.MinionsAndMonsters.EnemyMinions.Where<Obj_AI_Minion>(new Func<Obj_AI_Minion, bool>(this.CanCast))
                            orderby m.Health
                            select m;
                        BestPosition position = this.GetBestLinearCastPosition((IEnumerable<Obj_AI_Base>) enumerable, 0, null);
                        if (position.HitNumber >= minMinion)
                        {
                            this.Cast(position.CastPosition);
                        }
                        break;
                    }
                    case SkillShotType.Circular:
                    {
                        Obj_AI_Minion[] entities = (from m in EntityManager.MinionsAndMonsters.EnemyMinions.Where<Obj_AI_Minion>(new Func<Obj_AI_Minion, bool>(this.CanCast))
                            orderby m.Health
                            select m).ToArray<Obj_AI_Minion>();
                        BestPosition position2 = this.GetBestCircularCastPosition(entities, hitChance, 0);
                        if (position2.HitNumber >= minMinion)
                        {
                            this.Cast(position2.CastPosition);
                        }
                        break;
                    }
                    case SkillShotType.Cone:
                    {
                        Obj_AI_Minion[] minionArray2 = (from m in EntityManager.MinionsAndMonsters.EnemyMinions.Where<Obj_AI_Minion>(new Func<Obj_AI_Minion, bool>(this.CanCast))
                            orderby m.Health
                            select m).ToArray<Obj_AI_Minion>();
                        BestPosition position3 = this.GetBestConeCastPosition(minionArray2, hitChance, 0);
                        if (position3.HitNumber >= minMinion)
                        {
                            this.Cast(position3.CastPosition);
                        }
                        break;
                    }
                }
                return false;
            }

            public virtual bool CastStartToEnd(Vector3 start, Vector3 end)
            {
                if (!this.PrecheckCast(new Vector3?(end)))
                {
                    return false;
                }
                if ((this.AllowedCollisionCount < 0x7fffffff) && start.WillCollideWithYasuoWall(end, true))
                {
                    return false;
                }
                Player.CastSpell(base.Slot, start, end);
                return true;
            }

            public virtual bool CastStartToEndAoe(Obj_AI_Base[] Entities, int HitCount, int StartToEndDistance, float HitChance = 60f)
            {
                if (Entities == null)
                {
                    return false;
                }
                Vector3? sourcePosition = this.SourcePosition;
                Vector3 source = sourcePosition.HasValue ? sourcePosition.GetValueOrDefault() : Player.Instance.ServerPosition;
                IOrderedEnumerable<Obj_AI_Base> enumerable = from o in Entities
                    where o.IsValidTarget(null, false, null) && (this.GetPrediction(o).HitChancePercent >= 60f)
                    orderby o.Distance(source, false) descending
                    select o;
                IOrderedEnumerable<Obj_AI_Base> enumerable2 = from o in Entities
                    where o.IsValidTarget(null, false, null) && (this.GetPrediction(o).HitChancePercent >= 60f)
                    orderby o.Distance(source, false)
                    select o;
                Dictionary<int, Geometry.Polygon.Rectangle> dictionary = new Dictionary<int, Geometry.Polygon.Rectangle>();
                foreach (Obj_AI_Base base2 in enumerable)
                {
                    Vector3 castPosition = this.GetPrediction(base2).CastPosition;
                    foreach (Obj_AI_Base base3 in enumerable2)
                    {
                        Vector3 target = this.GetPrediction(base3).CastPosition;
                        Vector3 vector5 = castPosition.Extend(target, ((float) StartToEndDistance)).To3D(0);
                        if (this.PrecheckCast(new Vector3?(vector5)))
                        {
                            Geometry.Polygon.Rectangle rectangle = new Geometry.Polygon.Rectangle(castPosition, vector5, (float) this.Width);
                            int key = Entities.Count<Obj_AI_Base>(new Func<Obj_AI_Base, bool>(rectangle.IsInside));
                            dictionary.Add(key, rectangle);
                        }
                    }
                }
                if (!dictionary.Any<KeyValuePair<int, Geometry.Polygon.Rectangle>>())
                {
                    return false;
                }
                KeyValuePair<int, Geometry.Polygon.Rectangle> pair = (from r in dictionary
                    orderby r.Key descending
                    select r).FirstOrDefault<KeyValuePair<int, Geometry.Polygon.Rectangle>>();
                if (pair.Key < HitCount)
                {
                    return false;
                }
                Vector3 start = pair.Value.Start.To3D(0);
                Vector3 vector2 = pair.Value.End.To3D(0);
                if (!this.PrecheckCast(new Vector3?(vector2)))
                {
                    return false;
                }
                if ((this.AllowedCollisionCount < 0x7fffffff) && start.WillCollideWithYasuoWall(vector2, true))
                {
                    return false;
                }
                Player.CastSpell(base.Slot, start, vector2);
                return true;
            }

            public virtual BestPosition GetBestCircularCastPosition(IEnumerable<Obj_AI_Base> entities, int hitChance = 60, int moreDelay = 0)
            {
                PredictionResult result = (from r in Prediction.Position.PredictCircularMissileAoe(entities.ToArray<Obj_AI_Base>(), (float) this.Range, this.Width, base.CastDelay + moreDelay, (float) this.Speed, null)
                    orderby r.GetCollisionObjects<AIHeroClient>().Length descending, r.GetCollisionObjects<Obj_AI_Base>().Length descending
                    select r).FirstOrDefault<PredictionResult>();
                if ((result != null) && (result.HitChancePercent >= hitChance))
                {
                    return new BestPosition { 
                        CastPosition = result.CastPosition,
                        HitNumber = result.CollisionObjects.Length
                    };
                }
                return new BestPosition { 
                    CastPosition = Vector3.Zero,
                    HitNumber = 0
                };
            }

            public virtual BestPosition GetBestConeCastPosition(IEnumerable<Obj_AI_Base> entities, int hitChance = 60, int moreDelay = 0)
            {
                PredictionResult result = (from r in Prediction.Position.PredictConeSpellAoe(entities.ToArray<Obj_AI_Base>(), (float) this.Range, this.Width, base.CastDelay + moreDelay, (float) this.Speed, null)
                    orderby r.GetCollisionObjects<AIHeroClient>().Length descending, r.GetCollisionObjects<Obj_AI_Base>().Length descending
                    select r).FirstOrDefault<PredictionResult>();
                if ((result != null) && (result.HitChancePercent >= hitChance))
                {
                    return new BestPosition { 
                        CastPosition = result.CastPosition,
                        HitNumber = result.CollisionObjects.Length
                    };
                }
                return new BestPosition { 
                    CastPosition = Vector3.Zero,
                    HitNumber = 0
                };
            }

            public virtual BestPosition GetBestLinearCastPosition(IEnumerable<Obj_AI_Base> entities, int moreDelay = 0, Vector2? sourcePosition = new Vector2?())
            {
                Func<Vector2, bool> <>9__5;
                Obj_AI_Base[] baseArray = entities.ToArray<Obj_AI_Base>();
                switch (baseArray.Length)
                {
                    case 0:
                        return new BestPosition();

                    case 1:
                        return new BestPosition { 
                            CastPosition = this.GetPrediction(baseArray[0]).CastPosition,
                            HitNumber = 1
                        };
                }
                int extradelay = base.CastDelay + moreDelay;
                List<Vector2> source = new List<Vector2>(from o in baseArray
                    orderby o.Health
                    select Prediction.Position.PredictUnitPosition(o, extradelay));
                Obj_AI_Base[] baseArray2 = baseArray;
                for (int i = 0; i < baseArray2.Length; i++)
                {
                    Obj_AI_Base target = baseArray2[i];
                    Vector2 predictedPos = Prediction.Position.PredictUnitPosition(target, extradelay);
                    source.AddRange(from t in baseArray
                        orderby t.Health
                        where t.NetworkId != target.NetworkId
                        select (Vector2) ((predictedPos + predictedPos) / 2f));
                }
                Vector2? nullable = sourcePosition;
                Vector2 startPos = nullable.HasValue ? nullable.GetValueOrDefault() : Player.Instance.ServerPosition.To2D();
                int num = 0;
                Vector2 zero = Vector2.Zero;
                foreach (Vector2 vector2 in source.Where<Vector2>((Func<Vector2, bool>) (<>9__5 ?? (<>9__5 = o => o.IsInRange(startPos, (float) this.Range)))))
                {
                    Vector2 endPos = startPos + ((Vector2) (this.Range * (vector2 - startPos).Normalized()));
                    int num4 = (from o in baseArray
                        where o.IsValidTarget(null, false, null)
                        orderby o.Health
                        select o).Count<Obj_AI_Base>(o => Prediction.Position.PredictUnitPosition(o, extradelay).Distance(startPos, endPos, true, true) <= (this.Width * this.Width));
                    if (num4 >= num)
                    {
                        zero = endPos;
                        num = num4;
                    }
                }
                return new BestPosition { 
                    CastPosition = zero.To3DWorld(),
                    HitNumber = num
                };
            }

            public override float GetHealthPrediction(Obj_AI_Base target)
            {
                int castDelay = base.CastDelay;
                if (Math.Abs((float) (this.Speed - float.MaxValue)) > float.Epsilon)
                {
                    Vector3? sourcePosition = this.SourcePosition;
                    castDelay += (int) ((1000f * Math.Max((float) (target.Position.Distance((sourcePosition.HasValue ? sourcePosition.GetValueOrDefault() : Player.Instance.ServerPosition), false) - Player.Instance.BoundingRadius), (float) 0f)) / ((float) this.Speed));
                }
                return Prediction.Health.GetPrediction(target, castDelay);
            }

            public virtual PredictionResult GetPrediction(Obj_AI_Base target)
            {
                switch (this.Type)
                {
                    case SkillShotType.Linear:
                        return Prediction.Position.PredictLinearMissile(target, (float) this.Range, this.Width, base.CastDelay, (float) this.Speed, this.AllowedCollisionCount, this.SourcePosition, false);

                    case SkillShotType.Circular:
                        return Prediction.Position.PredictCircularMissile(target, (float) this.Range, this.Radius, base.CastDelay, (float) this.Speed, this.SourcePosition, false);

                    case SkillShotType.Cone:
                        return Prediction.Position.PredictConeSpell(target, (float) this.Range, this.ConeAngleDegrees, base.CastDelay, (float) this.Speed, this.SourcePosition, false);
                }
                object[] args = new object[] { this.Type };
                Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Warn, "Skillshot type '{0}' not implemented yet!", args);
                return null;
            }

            public int AllowedCollisionCount { get; set; }

            public int ConeAngleDegrees { get; set; }

            public bool HasCollision =>
                (this.AllowedCollisionCount > 0);

            public HitChance MinimumHitChance { get; set; }

            public int Radius =>
                (this.Width / 2);

            public Vector3? SourcePosition { get; set; }

            public int Speed { get; set; }

            public SkillShotType Type { get; protected set; }

            public int Width { get; set; }

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly Spell.Skillshot.<>c <>9 = new Spell.Skillshot.<>c();
                public static Func<KeyValuePair<int, Geometry.Polygon.Rectangle>, int> <>9__39_4;
                public static Func<Obj_AI_Base, float> <>9__41_0;
                public static Func<Obj_AI_Base, float> <>9__41_2;
                public static Func<Obj_AI_Base, bool> <>9__41_6;
                public static Func<Obj_AI_Base, float> <>9__41_7;
                public static Func<PredictionResult, int> <>9__42_0;
                public static Func<PredictionResult, int> <>9__42_1;
                public static Func<PredictionResult, int> <>9__43_0;
                public static Func<PredictionResult, int> <>9__43_1;
                public static Func<Obj_AI_Minion, float> <>9__45_0;
                public static Func<Obj_AI_Minion, float> <>9__45_1;
                public static Func<Obj_AI_Minion, float> <>9__45_2;

                internal float <CastOnBestFarmPosition>b__45_0(Obj_AI_Minion m) => 
                    m.Health;

                internal float <CastOnBestFarmPosition>b__45_1(Obj_AI_Minion m) => 
                    m.Health;

                internal float <CastOnBestFarmPosition>b__45_2(Obj_AI_Minion m) => 
                    m.Health;

                internal int <CastStartToEndAoe>b__39_4(KeyValuePair<int, Geometry.Polygon.Rectangle> r) => 
                    r.Key;

                internal int <GetBestCircularCastPosition>b__42_0(PredictionResult r) => 
                    r.GetCollisionObjects<AIHeroClient>().Length;

                internal int <GetBestCircularCastPosition>b__42_1(PredictionResult r) => 
                    r.GetCollisionObjects<Obj_AI_Base>().Length;

                internal int <GetBestConeCastPosition>b__43_0(PredictionResult r) => 
                    r.GetCollisionObjects<AIHeroClient>().Length;

                internal int <GetBestConeCastPosition>b__43_1(PredictionResult r) => 
                    r.GetCollisionObjects<Obj_AI_Base>().Length;

                internal float <GetBestLinearCastPosition>b__41_0(Obj_AI_Base o) => 
                    o.Health;

                internal float <GetBestLinearCastPosition>b__41_2(Obj_AI_Base t) => 
                    t.Health;

                internal bool <GetBestLinearCastPosition>b__41_6(Obj_AI_Base t) => 
                    t.IsValidTarget(null, false, null);

                internal float <GetBestLinearCastPosition>b__41_7(Obj_AI_Base o) => 
                    o.Health;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct BestPosition
            {
                public int HitNumber;
                public Vector3 CastPosition;
            }
        }

        public abstract class SpellBase : Spell.ISpell
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <CastDelay>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.DamageType <DamageType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Spell.CastFailures <LastCastFailure>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private uint <Range>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Vector3? <RangeCheckSource>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SpellSlot <Slot>k__BackingField;

            [field: CompilerGenerated, DebuggerBrowsable(0)]
            public event SpellCastedHandler OnSpellCasted;

            protected SpellBase(SpellSlot spellSlot, uint spellRange = 0xffffffff, EloBuddy.DamageType dmgType = 3)
            {
                this.Slot = spellSlot;
                this.Range = spellRange;
                this.DamageType = dmgType;
                if (this.Slot != SpellSlot.Unknown)
                {
                    Obj_AI_Base.OnSpellCast += new Obj_AI_BaseDoCastSpell(this.OnSpellCast);
                }
            }

            public bool CanCast(Obj_AI_Base target) => 
                (target.IsValidTarget(new float?((float) this.Range), false, null) && this.IsReady(0));

            public virtual bool Cast() => 
                false;

            public virtual bool Cast(Obj_AI_Base targetEntity) => 
                false;

            public virtual bool Cast(Vector3 targetPosition) => 
                false;

            public virtual void DrawRange(SharpDX.Color color, float lineWidth = 3f)
            {
                Vector3[] positions = new Vector3[1];
                Vector3? rangeCheckSource = this.RangeCheckSource;
                positions[0] = rangeCheckSource.HasValue ? rangeCheckSource.GetValueOrDefault() : Player.Instance.Position;
                EloBuddy.SDK.Rendering.Circle.Draw(color, (float) this.Range, lineWidth, positions);
            }

            public virtual void DrawRange(System.Drawing.Color color, float lineWidth = 3f)
            {
                Vector3[] positions = new Vector3[1];
                Vector3? rangeCheckSource = this.RangeCheckSource;
                positions[0] = rangeCheckSource.HasValue ? rangeCheckSource.GetValueOrDefault() : Player.Instance.Position;
                EloBuddy.SDK.Rendering.Circle.Draw(color.ToSharpDX(), (float) this.Range, lineWidth, positions);
            }

            public virtual float GetHealthPrediction(Obj_AI_Base target) => 
                Prediction.Health.GetPrediction(target, this.CastDelay);

            public virtual float GetSpellDamage(Obj_AI_Base target) => 
                Player.Instance.GetSpellDamage(target, this.Slot, DamageLibrary.SpellStages.Default);

            public virtual AIHeroClient GetTarget() => 
                TargetSelector.GetTarget((float) this.Range, this.DamageType, null, false);

            public bool IsInRange(Obj_AI_Base targetEntity)
            {
                Vector3? rangeCheckSource = this.RangeCheckSource;
                return ((rangeCheckSource.HasValue ? rangeCheckSource.GetValueOrDefault() : Player.Instance.ServerPosition).Distance(targetEntity, true) < this.RangeSquared);
            }

            public bool IsInRange(Vector3 targetPosition)
            {
                Vector3? rangeCheckSource = this.RangeCheckSource;
                return ((rangeCheckSource.HasValue ? rangeCheckSource.GetValueOrDefault() : Player.Instance.ServerPosition).Distance(targetPosition, true) < this.RangeSquared);
            }

            public bool IsReady(uint extraTime = 0)
            {
                if (this.Slot == SpellSlot.Unknown)
                {
                    return false;
                }
                return ((extraTime == 0) ? Player.GetSpell(this.Slot).IsReady : (((Player.GetSpell(this.Slot).CooldownExpires + (((float) extraTime) / 1000f)) - Game.Time) < 0f));
            }

            internal void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
            {
                if ((sender.IsMe && (this.OnSpellCasted != null)) && (args.Slot == this.Slot))
                {
                    this.OnSpellCasted(this, args);
                }
            }

            protected virtual bool PrecheckCast(Vector3? position = new Vector3?())
            {
                if (!this.IsReady(0))
                {
                    this.LastCastFailure = Spell.CastFailures.SpellNotReady;
                    return false;
                }
                if (Chat.IsOpen)
                {
                    this.LastCastFailure = Spell.CastFailures.SpellHumanized;
                    return false;
                }
                return true;
            }

            public int AmmoQuantity =>
                this.Handle.Ammo;

            public int CastDelay { get; set; }

            public EloBuddy.DamageType DamageType { get; set; }

            public SpellDataInst Handle =>
                Player.GetSpell(this.Slot);

            public bool IsLearned =>
                ((this.Slot != SpellSlot.Unknown) && this.Handle.IsLearned);

            public bool IsOnCooldown =>
                ((this.Slot != SpellSlot.Unknown) && this.Handle.IsOnCooldown);

            public Spell.CastFailures LastCastFailure { get; protected set; }

            public int Level =>
                ((this.Slot != SpellSlot.Unknown) ? this.Handle.Level : 0);

            public int ManaCost
            {
                get
                {
                    if (!this.IsLearned)
                    {
                        return 0;
                    }
                    return (int) this.Handle.SData.ManaCostArray[this.Level - 1];
                }
            }

            public string Name =>
                ((this.Slot != SpellSlot.Unknown) ? this.Handle.Name : "");

            public virtual uint Range { get; set; }

            public Vector3? RangeCheckSource { get; set; }

            public uint RangeSquared =>
                (this.Range * this.Range);

            public SpellSlot Slot { get; set; }

            public SpellState State =>
                ((this.Slot != SpellSlot.Unknown) ? this.Handle.State : SpellState.Unknown);

            public int ToggleState =>
                this.Handle.ToggleState;

            public delegate void SpellCastedHandler(Spell.SpellBase spell, GameObjectProcessSpellCastEventArgs args);
        }

        public class Targeted : Spell.Ranged
        {
            public Targeted(SpellSlot spellSlot, uint spellRange, DamageType dmgType = 3) : base(spellSlot, spellRange, dmgType)
            {
            }

            public override bool Cast()
            {
                throw new SpellCastException("Can't cast targeted spell without target!");
            }

            public override bool Cast(Obj_AI_Base targetEntity)
            {
                if (!this.PrecheckCast(new Vector3?(targetEntity.ServerPosition)))
                {
                    return false;
                }
                Player.CastSpell(base.Slot, targetEntity);
                return true;
            }

            public override bool Cast(Vector3 targetPosition)
            {
                if (!this.PrecheckCast(new Vector3?(targetPosition)))
                {
                    return false;
                }
                Player.CastSpell(base.Slot, targetPosition);
                return true;
            }
        }
    }
}

