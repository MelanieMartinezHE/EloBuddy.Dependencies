namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Constants;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Spells;
    using EloBuddy.SDK.Utils;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;

    public static class Prediction
    {
        internal static void Initialize()
        {
            Health.Initialize();
            Position.Initialize();
            Position.Collision.Initialize();
            Manager.Initialize();
            Position.PredictionEx.UnitTracker.Init();
        }

        public static class Health
        {
            internal static readonly Dictionary<int, List<IncomingAttack>> IncomingAttacks = new Dictionary<int, List<IncomingAttack>>();

            private static float Angle(Vector2 u, Vector2 v)
            {
                double d = ((double) ((u.X * v.X) + (u.Y * v.Y))) / (Math.Sqrt((double) (u.X.Pow() + u.Y.Pow())) * Math.Sqrt((double) (v.X.Pow() + v.Y.Pow())));
                return (float) Math.Acos(d);
            }

            internal static float GetPredictedMissileTravelTime(Obj_AI_Base minion, Vector2 sourcePosition, float missileDelay, float missileSpeed)
            {
                if ((missileSpeed <= 0f) || (missileSpeed >= float.MaxValue))
                {
                    return 0f;
                }
                Vector3 serverPosition = minion.ServerPosition;
                if (minion.IsMoving && (minion.Path.Length == 2))
                {
                    Vector3 vector2 = minion.Path.LastOrDefault<Vector3>();
                    Vector3 vector = (vector2 - serverPosition).Normalized();
                    float num = vector2.Distance(serverPosition, false);
                    float num2 = missileDelay * minion.MoveSpeed;
                    if (num2 <= num)
                    {
                        Vector3 vector4 = serverPosition + ((Vector3) (vector * num2));
                        float num3 = Angle((vector4.To2D() - sourcePosition).Normalized(), vector.To2D());
                        double num4 = Math.Sin((double) num3);
                        double d = Math.Sin((minion.MoveSpeed / missileSpeed) * num4);
                        double num6 = Math.Asin(d);
                        double a = (3.1415926535897931 - num3) - num6;
                        if (a >= 0.0)
                        {
                            float num8 = vector4.Distance(sourcePosition, false);
                            double num9 = Math.Sin(a);
                            float num10 = (float) ((d / num9) * num8);
                            Vector3 vector5 = vector4 + ((Vector3) (vector * num10));
                            serverPosition = ((num10 + num2) <= num) ? vector5 : vector2;
                        }
                        else
                        {
                            serverPosition = vector2;
                        }
                    }
                    else
                    {
                        serverPosition = vector2;
                    }
                }
                return (sourcePosition.Distance(serverPosition, false) / missileSpeed);
            }

            internal static float GetPredictedMissileTravelTime(Obj_AI_Base minion, Vector3 sourcePosition, float missileDelay, float missileSpeed) => 
                GetPredictedMissileTravelTime(minion, sourcePosition.To2D(), missileDelay, missileSpeed);

            public static Dictionary<Obj_AI_Base, float> GetPrediction(Dictionary<Obj_AI_Base, int> minionsTime)
            {
                var <>9__4;
                Dictionary<Obj_AI_Base, float> dictionary = minionsTime.Keys.ToDictionary<Obj_AI_Base, Obj_AI_Base, float>(minion => minion, minion => minion.Health);
                foreach (IncomingAttack attack in from <>h__TransparentIdentifier0 in (from entry in IncomingAttacks
                    from attack in entry.Value
                    select new { 
                        entry = entry,
                        attack = attack
                    }).Where((Func<<>f__AnonymousType4<KeyValuePair<int, List<IncomingAttack>>, IncomingAttack>, bool>) (<>9__4 ?? (<>9__4 = <>h__TransparentIdentifier0 => minionsTime.ContainsKey(<>h__TransparentIdentifier0.attack.Target)))) select <>h__TransparentIdentifier0.attack)
                {
                    Dictionary<Obj_AI_Base, float> dictionary2 = dictionary;
                    Obj_AI_Base target = attack.Target;
                    dictionary2[target] -= attack.GetDamage(minionsTime[attack.Target]);
                }
                return dictionary;
            }

            public static float GetPrediction(Obj_AI_Base target, int time)
            {
                Func<IncomingAttack, bool> <>9__1;
                Func<IncomingAttack, float> <>9__2;
                return (((target.Health + target.AllShield) + target.AttackShield) - IncomingAttacks.Sum<KeyValuePair<int, List<IncomingAttack>>>(((Func<KeyValuePair<int, List<IncomingAttack>>, float>) (entry => entry.Value.Where<IncomingAttack>(((Func<IncomingAttack, bool>) (<>9__1 ?? (<>9__1 = o => o.EqualsTarget(target))))).Sum<IncomingAttack>(((Func<IncomingAttack, float>) (<>9__2 ?? (<>9__2 = attack => attack.GetDamage(time)))))))));
            }

            internal static void Initialize()
            {
                Obj_AI_Base.OnBasicAttack += new Obj_AI_BaseOnBasicAttack(Prediction.Health.OnBasicAttack);
                Spellbook.OnStopCast += new SpellbookStopCast(Prediction.Health.OnStopCast);
                Game.OnTick += new GameTick(Prediction.Health.OnGameUpdate);
                GameObject.OnDelete += new GameObjectDelete(Prediction.Health.OnDelete);
            }

            internal static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
            {
                if (!sender.IsMe && args.IsAutoAttack())
                {
                    Obj_AI_Minion target = args.Target as Obj_AI_Minion;
                    if ((target != null) && (sender.Team.IsAlly() && ((Obj_AI_Base) Player.Instance).IsInRange(sender, 2000f)))
                    {
                        switch (sender.Type)
                        {
                            case GameObjectType.AIHeroClient:
                            case GameObjectType.obj_AI_Minion:
                            case GameObjectType.obj_AI_Turret:
                                if (!IncomingAttacks.ContainsKey(sender.NetworkId))
                                {
                                    IncomingAttacks.Add(sender.NetworkId, new List<IncomingAttack>());
                                }
                                else
                                {
                                    foreach (IncomingAttack attack in IncomingAttacks[sender.NetworkId])
                                    {
                                        attack.IsActiveAttack = false;
                                    }
                                }
                                IncomingAttacks[sender.NetworkId].Add(new IncomingAttack(sender, target, (int) args.SData.MissileSpeed));
                                break;
                        }
                    }
                }
            }

            private static void OnDelete(GameObject sender, EventArgs args)
            {
                Obj_AI_Base target = sender as Obj_AI_Base;
                if (target > null)
                {
                    foreach (IncomingAttack attack in from entry in IncomingAttacks.ToArray<KeyValuePair<int, List<IncomingAttack>>>() select entry.Value)
                    {
                        if (attack.Target.IdEquals(target))
                        {
                            attack.Target = null;
                        }
                        if (attack.Source.IdEquals(target))
                        {
                            attack.Source = null;
                        }
                    }
                }
            }

            internal static void OnGameUpdate(EventArgs args)
            {
                VerifyAttacks();
            }

            internal static void OnStopCast(Obj_AI_Base sender, SpellbookStopCastEventArgs args)
            {
                if (args.DestroyMissile && args.StopAnimation)
                {
                    if (sender.IsMelee)
                    {
                        IncomingAttacks.Remove(sender.NetworkId);
                    }
                    else if (IncomingAttacks.ContainsKey(sender.NetworkId) && (IncomingAttacks[sender.NetworkId].Count > 0))
                    {
                        IncomingAttacks[sender.NetworkId].RemoveAt(IncomingAttacks[sender.NetworkId].Count - 1);
                    }
                }
            }

            internal static void VerifyAttacks()
            {
                foreach (KeyValuePair<int, List<IncomingAttack>> pair in IncomingAttacks.ToArray<KeyValuePair<int, List<IncomingAttack>>>())
                {
                    float? range = null;
                    Vector3? rangeCheckFrom = null;
                    bool flag = ObjectManager.GetUnitByNetworkId<Obj_AI_Base>(pair.Key).IsValidTarget(range, false, rangeCheckFrom);
                    foreach (IncomingAttack attack in pair.Value.ToArray())
                    {
                        attack._sourceIsValid = flag;
                        if (attack.ShouldRemove)
                        {
                            IncomingAttacks[pair.Key].Remove(attack);
                        }
                    }
                    if (IncomingAttacks[pair.Key].Count == 0)
                    {
                        IncomingAttacks.Remove(pair.Key);
                    }
                }
            }

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly Prediction.Health.<>c <>9 = new Prediction.Health.<>c();
                public static Func<Obj_AI_Base, Obj_AI_Base> <>9__10_0;
                public static Func<Obj_AI_Base, float> <>9__10_1;
                public static Func<KeyValuePair<int, List<Prediction.Health.IncomingAttack>>, IEnumerable<Prediction.Health.IncomingAttack>> <>9__10_2;
                public static Func<KeyValuePair<int, List<Prediction.Health.IncomingAttack>>, Prediction.Health.IncomingAttack, <>f__AnonymousType4<KeyValuePair<int, List<Prediction.Health.IncomingAttack>>, Prediction.Health.IncomingAttack>> <>9__10_3;
                public static Func<<>f__AnonymousType4<KeyValuePair<int, List<Prediction.Health.IncomingAttack>>, Prediction.Health.IncomingAttack>, Prediction.Health.IncomingAttack> <>9__10_5;
                public static Func<KeyValuePair<int, List<Prediction.Health.IncomingAttack>>, IEnumerable<Prediction.Health.IncomingAttack>> <>9__2_0;

                internal Obj_AI_Base <GetPrediction>b__10_0(Obj_AI_Base minion) => 
                    minion;

                internal float <GetPrediction>b__10_1(Obj_AI_Base minion) => 
                    minion.Health;

                internal IEnumerable<Prediction.Health.IncomingAttack> <GetPrediction>b__10_2(KeyValuePair<int, List<Prediction.Health.IncomingAttack>> entry) => 
                    entry.Value;

                internal <>f__AnonymousType4<KeyValuePair<int, List<Prediction.Health.IncomingAttack>>, Prediction.Health.IncomingAttack> <GetPrediction>b__10_3(KeyValuePair<int, List<Prediction.Health.IncomingAttack>> entry, Prediction.Health.IncomingAttack attack) => 
                    new { 
                        entry = entry,
                        attack = attack
                    };

                internal Prediction.Health.IncomingAttack <GetPrediction>b__10_5(<>f__AnonymousType4<KeyValuePair<int, List<Prediction.Health.IncomingAttack>>, Prediction.Health.IncomingAttack> <>h__TransparentIdentifier0) => 
                    <>h__TransparentIdentifier0.attack;

                internal IEnumerable<Prediction.Health.IncomingAttack> <OnDelete>b__2_0(KeyValuePair<int, List<Prediction.Health.IncomingAttack>> entry) => 
                    entry.Value;
            }

            internal class IncomingAttack
            {
                internal bool _arrived;
                private float? _cachedDamage;
                internal bool _sourceIsRanged;
                internal bool _sourceIsValid = true;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int <AttackCastDelay>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int <AttackDelay>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private bool <IsActiveAttack>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int <MissileSpeed>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private Obj_AI_Base <Source>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private Vector2 <SourcePosition>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int <StartTick>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private Obj_AI_Base <Target>k__BackingField;

                public IncomingAttack(Obj_AI_Base source, Obj_AI_Base target, int missileSpeed = 0x7fffffff)
                {
                    this.Source = source;
                    this.Target = target;
                    this.SourcePosition = source.ServerPosition.To2D();
                    this.AttackCastDelay = (int) (this.Source.AttackCastDelay * 1000f);
                    this.AttackDelay = (int) (this.Source.AttackDelay * 1000f);
                    this.MissileSpeed = missileSpeed;
                    this.StartTick = Core.GameTickCount;
                    this.IsActiveAttack = true;
                    this._sourceIsRanged = this.Source.IsRanged;
                }

                internal bool EqualsTarget(Obj_AI_Base target) => 
                    (this.Target.NetworkId == target.NetworkId);

                public float GetDamage(int delay)
                {
                    float num = 0f;
                    if (!this.ShouldRemove)
                    {
                        delay += (Game.Ping - 100) + Orbwalker.ExtraFarmDelay;
                        int num2 = ((this.StartTick + this.AttackCastDelay) + this.MissileFlightTime) - Core.GameTickCount;
                        if (num2 <= -250)
                        {
                            this._arrived = true;
                        }
                        if (this.IsActiveAttack && this._sourceIsValid)
                        {
                            int num3 = 0;
                            while (num2 < delay)
                            {
                                if (num2 > 0)
                                {
                                    num3++;
                                }
                                num2 += this.AttackDelay;
                            }
                            return (num + (this.Damage * num3));
                        }
                        if ((num2 >= delay) || (num2 <= 0))
                        {
                            return num;
                        }
                        if (this._sourceIsRanged || this._sourceIsValid)
                        {
                            num += this.Damage;
                        }
                    }
                    return num;
                }

                internal int AttackCastDelay { get; set; }

                internal int AttackDelay { get; set; }

                internal float Damage
                {
                    get
                    {
                        if (!this._cachedDamage.HasValue)
                        {
                            this._cachedDamage = new float?(this.Source.GetAutoAttackDamage(this.Target, true));
                        }
                        return this._cachedDamage.Value;
                    }
                }

                internal bool IsActiveAttack { get; set; }

                internal int MissileFlightTime
                {
                    get
                    {
                        if (this._sourceIsRanged)
                        {
                            return (int) (1000f * Prediction.Health.GetPredictedMissileTravelTime(this.Target, this.SourcePosition, Math.Max(0f, (float) (this.AttackCastDelay - (Core.GameTickCount - this.StartTick))) / 1000f, (float) this.MissileSpeed));
                        }
                        return 0;
                    }
                }

                internal int MissileSpeed { get; set; }

                internal bool ShouldRemove =>
                    ((((this.Target == null) || this.Target.IsDead) || (0xbb8 < (Core.GameTickCount - this.StartTick))) || this._arrived);

                internal Obj_AI_Base Source { get; set; }

                internal Vector2 SourcePosition { get; set; }

                internal int StartTick { get; set; }

                internal Obj_AI_Base Target { get; set; }
            }
        }

        public static class Manager
        {
            internal static EloBuddy.SDK.Menu.Menu Menu;
            private static readonly Dictionary<int, string> Strings = new Dictionary<int, string>();
            private static readonly Dictionary<int, Func<PredictionInput, PredictionOutput>> Suscribers = new Dictionary<int, Func<PredictionInput, PredictionOutput>>();

            public static PredictionOutput GetPrediction(PredictionInput input)
            {
                int currentValue = Menu["PredictionSelected"].Cast<ComboBox>().CurrentValue;
                return (Suscribers.ContainsKey(currentValue) ? Suscribers[currentValue](input) : new PredictionOutput(input));
            }

            internal static void Initialize()
            {
            }

            public static void Suscribe(string addonName, Func<PredictionInput, PredictionOutput> func)
            {
                if (SelectedComboBox.Overlay.Children.All<Button>(i => i.TextValue != addonName))
                {
                    SelectedComboBox.Add(addonName);
                }
                if (Strings.All<KeyValuePair<int, string>>(i => i.Value != addonName))
                {
                    int count = Strings.Count;
                    Suscribers[count] = func;
                    Strings[count] = addonName;
                }
            }

            public static string PredictionSelected
            {
                get => 
                    (Strings.ContainsKey(SelectedComboBox.CurrentValue) ? Strings[SelectedComboBox.CurrentValue] : "SDK Prediction");
                set
                {
                    for (int i = 0; i < Strings.Count; i++)
                    {
                        string str = Strings[i];
                        if (str == value)
                        {
                            SelectedComboBox.CurrentValue = i;
                        }
                    }
                }
            }

            private static ComboBox SelectedComboBox =>
                Menu["PredictionSelected"].Cast<ComboBox>();

            public class PredictionInput
            {
                private Vector3? _from;
                private Vector3? _rangeCheckFrom;
                public HashSet<CollisionType> CollisionTypes = new HashSet<CollisionType>();
                public float Delay;
                public float Radius = 1f;
                public float Range = float.MaxValue;
                public float Speed = float.MaxValue;
                public Obj_AI_Base Target;
                public SkillShotType Type = SkillShotType.Circular;

                public Vector3 From
                {
                    get
                    {
                        Vector3? nullable = this._from;
                        return (nullable.HasValue ? nullable.GetValueOrDefault() : Player.Instance.ServerPosition);
                    }
                    set
                    {
                        this._from = new Vector3?(value);
                    }
                }

                public Vector3 RangeCheckFrom
                {
                    get
                    {
                        Vector3? nullable = this._rangeCheckFrom;
                        return (nullable.HasValue ? nullable.GetValueOrDefault() : this.From);
                    }
                    set
                    {
                        this._rangeCheckFrom = new Vector3?(value);
                    }
                }
            }

            public class PredictionOutput
            {
                private float _hitChancePercent;
                public Vector3 CastPosition;
                public List<GameObject> CollisionGameObjects = new List<GameObject>();
                public EloBuddy.SDK.Enumerations.HitChance HitChance = EloBuddy.SDK.Enumerations.HitChance.Impossible;
                public Prediction.Manager.PredictionInput Input;
                public Vector3 PredictedPosition;
                public float RealHitChancePercent;

                public PredictionOutput(Prediction.Manager.PredictionInput input)
                {
                    this.Input = input;
                    this.CastPosition = input.Target.ServerPosition;
                    this.PredictedPosition = input.Target.ServerPosition;
                }

                public T[] GetCollisionObjects<T>() => 
                    (from unit in this.CollisionGameObjects
                        where unit.GetType() == typeof(T)
                        select unit).Cast<T>().ToArray<T>();

                public GameObject[] GetCollisionObjects(CollisionType type)
                {
                    switch (type)
                    {
                        case CollisionType.AiHeroClient:
                            return (from unit in this.CollisionGameObjects
                                where unit is AIHeroClient
                                select unit).ToArray<GameObject>();

                        case CollisionType.ObjAiMinion:
                            return (from unit in this.CollisionGameObjects
                                where unit is Obj_AI_Minion
                                select unit).ToArray<GameObject>();

                        case CollisionType.YasuoWall:
                            return (from unit in this.CollisionGameObjects
                                where !(unit is Obj_AI_Minion) && !(unit is AIHeroClient)
                                select unit).ToArray<GameObject>();
                    }
                    return new GameObject[0];
                }

                public bool Collides =>
                    (this.CollisionGameObjects.Count > 0);

                public Obj_AI_Base[] CollisionObjects =>
                    this.GetCollisionObjects<Obj_AI_Base>();

                public float HitChancePercent
                {
                    get => 
                        (!this.Collides ? this._hitChancePercent : 0f);
                    set
                    {
                        this._hitChancePercent = value;
                        this.RealHitChancePercent = value;
                    }
                }

                [Serializable, CompilerGenerated]
                private sealed class <>c
                {
                    public static readonly Prediction.Manager.PredictionOutput.<>c <>9 = new Prediction.Manager.PredictionOutput.<>c();
                    public static Func<GameObject, bool> <>9__16_0;
                    public static Func<GameObject, bool> <>9__16_1;
                    public static Func<GameObject, bool> <>9__16_2;

                    internal bool <GetCollisionObjects>b__16_0(GameObject unit) => 
                        (unit is AIHeroClient);

                    internal bool <GetCollisionObjects>b__16_1(GameObject unit) => 
                        (unit is Obj_AI_Minion);

                    internal bool <GetCollisionObjects>b__16_2(GameObject unit) => 
                        (!(unit is Obj_AI_Minion) && !(unit is AIHeroClient));
                }

                [Serializable, CompilerGenerated]
                private sealed class <>c__15<T>
                {
                    public static readonly Prediction.Manager.PredictionOutput.<>c__15<T> <>9;
                    public static Func<GameObject, bool> <>9__15_0;

                    static <>c__15()
                    {
                        Prediction.Manager.PredictionOutput.<>c__15<T>.<>9 = new Prediction.Manager.PredictionOutput.<>c__15<T>();
                    }

                    internal bool <GetCollisionObjects>b__15_0(GameObject unit) => 
                        (unit.GetType() == typeof(T));
                }
            }
        }

        public static class Position
        {
            private static Dictionary<int, Tuple<float, float>> _heroActionDuration = new Dictionary<int, Tuple<float, float>>();
            private static Dictionary<int, float> _moveTime = new Dictionary<int, float>();
            private static Dictionary<int, List<Vector2>> _pathGroup = new Dictionary<int, List<Vector2>>();
            private static Dictionary<int, Vector2> _tendencyDestination = new Dictionary<int, Vector2>();
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private static EloBuddy.SDK.Menu.Menu <Menu>k__BackingField;

            private static float ActionDuration(Obj_AI_Base unit)
            {
                Tuple<float, float> tuple;
                if ((unit.Type == GameObjectType.AIHeroClient) && _heroActionDuration.TryGetValue(unit.NetworkId, out tuple))
                {
                    float num = tuple.Item2 - ((Game.Time - tuple.Item1) * 1000f);
                    return Math.Max(0f, num);
                }
                return 0f;
            }

            private static bool CollidesWithTerrain(Geometry.Polygon polygon) => 
                (TerrainCollisionPoints(polygon).Length > 0);

            private static bool CollidesWithTerrain(PredictionData data, Vector2 castPos) => 
                CollidesWithTerrain(data.GetSkillshotPolygon(castPos));

            private static Obj_AI_Base[] GatherCollisionObjects(Vector2 center, float range, Obj_AI_Base[] ignoreUnits = null)
            {
                ignoreUnits = ignoreUnits ?? new Obj_AI_Base[0];
                return (from unit in ObjectManager.Get<Obj_AI_Base>()
                    where (((unit.IsValidTarget(null, false, null) && unit.IsEnemy) && (!unit.IsStructure() && (unit.MaxHealth > 6f))) && unit.IsInRange(center, range)) && ignoreUnits.All<Obj_AI_Base>(ignore => (ignore.Index != unit.Index))
                    select unit).ToArray<Obj_AI_Base>();
            }

            public static Prediction.Manager.PredictionOutput GetPrediction(Prediction.Manager.PredictionInput input)
            {
                if ((input == null) || (input.Target == null))
                {
                    throw new ArgumentNullException("input");
                }
                return Prediction.Manager.GetPrediction(input);
            }

            public static PredictionResult GetPrediction(Obj_AI_Base target, PredictionData data, bool skipCollision = false)
            {
                Vector3 vector;
                Vector3 startPos;
                Vector3 endPos;
                float moveSpeed;
                if (target == null)
                {
                    throw new ArgumentNullException("target");
                }
                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }
                if (!Prediction.Manager.PredictionSelected.Equals("SDK Prediction"))
                {
                    return TransformToPredictionResult(Prediction.Manager.GetPrediction(TransformPredictionData(target, data)));
                }
                int time = data.Delay + Ping;
                Dash.DashEventArgs dashInfo = target.GetDashInfo();
                if (dashInfo > null)
                {
                    moveSpeed = 3000f;
                    startPos = dashInfo.StartPos;
                    endPos = dashInfo.EndPos;
                    vector = endPos;
                }
                else if (!target.IsMoving)
                {
                    moveSpeed = 0f;
                    startPos = target.ServerPosition;
                    endPos = target.ServerPosition;
                    vector = endPos;
                }
                else
                {
                    Vector3[] realPath = GetRealPath(target);
                    moveSpeed = target.MoveSpeed;
                    startPos = PredictUnitPosition(target, time).To3DWorld();
                    endPos = realPath.Last<Vector3>();
                    vector = startPos;
                    for (int i = 0; i < (realPath.Length - 1); i++)
                    {
                        Vector3 vector7 = realPath[i];
                        Vector3 vector8 = realPath[i + 1];
                        if (Geometry.PointInLineSegment(vector7.To2D(), vector8.To2D(), startPos.To2D()))
                        {
                            endPos = vector8;
                            break;
                        }
                    }
                    Vector2 vector6 = Collision.GetCollisionPoint(startPos.To2D(), endPos.To2D(), data.SourcePosition.To2D(), moveSpeed, data.Speed);
                    if (!Geometry.PointInLineSegment(startPos.To2D(), endPos.To2D(), vector6) && (endPos != realPath.Last<Vector3>()))
                    {
                        Vector3? castPos = null;
                        return PredictionResult.ResultImpossible(data, castPos, null);
                    }
                }
                Vector2 point = Collision.GetCollisionPoint(startPos.To2D(), endPos.To2D(), data.SourcePosition.To2D(), moveSpeed, data.Speed);
                Vector2 vector5 = TendencyDestination(target);
                float boundingRadius = target.BoundingRadius;
                if (!Geometry.PointInLineSegment(startPos.To2D(), endPos.To2D(), point))
                {
                    Vector2 vector9 = point;
                    point = endPos.To2D();
                    if (vector5 != endPos.To2D())
                    {
                        float num13 = point.Distance(vector9, true);
                        float num14 = point.Distance(vector5, true);
                        if (num13 > num14)
                        {
                            point = point.Extend(vector5, point.Distance(vector5, false));
                        }
                        else
                        {
                            point = point.Extend(vector5, data.Radius + (boundingRadius / 4f));
                        }
                    }
                }
                else
                {
                    point = point.Extend(startPos, (float) (data.Radius / 2));
                }
                if (CollidesWithTerrain(data, point))
                {
                    point = point.Extend(startPos, ((float) data.Radius)).Extend(vector5, (float) (data.Radius / 2));
                }
                float range = data.Range * 2f;
                List<Obj_AI_Base> list = new List<Obj_AI_Base>();
                if (!skipCollision)
                {
                    PredictionData.CollisionCheck collisionCalculator = data.GetCollisionCalculator(point);
                    Obj_AI_Base[] ignoreUnits = new Obj_AI_Base[] { target };
                    foreach (Obj_AI_Base base2 in GatherCollisionObjects(data.SourcePosition.To2D(), range, ignoreUnits))
                    {
                        if (collisionCalculator(target, base2))
                        {
                            list.Add(base2);
                        }
                    }
                }
                if (!data.GetSkillshotPolygon(point).IsInside(vector))
                {
                    return PredictionResult.ResultImpossible(data, new Vector3?(point.To3DWorld()), new Vector3?(vector));
                }
                float num5 = ActionDuration(target);
                float num6 = data.SourcePosition.Distance(point, false) / data.Speed;
                float num7 = boundingRadius / target.MoveSpeed;
                float num8 = Math.Max((float) 0f, (float) ((num6 - num7) - num5));
                double num9 = Math.Max(0.001, (double) (((float) data.Range) / data.Speed));
                float num10 = (float) Math.Max((double) 0.05, (double) ((1.0 - (((double) num8) / num9)) * 100.0));
                float hitChancePercent = 10f + (num10 * TendencyFactor(target));
                if (target.Type != GameObjectType.AIHeroClient)
                {
                    hitChancePercent = 80f;
                }
                HitChance unknown = HitChance.Unknown;
                if (target.GetMovementBlockedDebuffDuration() > num6)
                {
                    unknown = HitChance.Immobile;
                    hitChancePercent = 100f;
                }
                if (dashInfo > null)
                {
                    unknown = HitChance.Dashing;
                    hitChancePercent = 90f;
                }
                return new PredictionResult(point.To3DWorld(), vector, hitChancePercent, list.ToArray(), data.AllowCollisionCount, unknown);
            }

            public static PredictionResult[] GetPredictionAoe(Obj_AI_Base[] targets, PredictionData data)
            {
                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }
                targets = targets ?? EntityManager.Heroes.Enemies.Cast<Obj_AI_Base>().ToArray<Obj_AI_Base>();
                Dictionary<Obj_AI_Base, PredictionResult> results = new Dictionary<Obj_AI_Base, PredictionResult>();
                foreach (Obj_AI_Base base2 in targets)
                {
                    PredictionResult result = GetPrediction(base2, data, true);
                    if (result.HitChance > HitChance.Collision)
                    {
                        results.Add(base2, result);
                    }
                }
                if (results.Count == 0)
                {
                    return new PredictionResult[0];
                }
                List<PredictionResult> list = new List<PredictionResult>();
                foreach (List<Vector2> list2 in data.GetAoeGroups(results))
                {
                    Vector2 castPos = list2.ToArray().CenterPoint();
                    PredictionData.CollisionCheck collisionCalculator = data.GetCollisionCalculator(castPos);
                    List<Obj_AI_Base> list3 = new List<Obj_AI_Base>();
                    foreach (Obj_AI_Base base3 in GatherCollisionObjects(data.SourcePosition.To2D(), (float) (data.Range * 2), null))
                    {
                        if (collisionCalculator(base3, base3))
                        {
                            list3.Add(base3);
                        }
                    }
                    list.Add(new PredictionResult(castPos.To3DWorld(), Vector3.Zero, 100f, list3.ToArray(), -1, HitChance.Unknown));
                }
                return list.ToArray();
            }

            public static Vector3[] GetRealPath(Obj_AI_Base unit)
            {
                List<Vector3> second = unit.Path.ToList<Vector3>();
                for (int i = second.Count - 1; i > 0; i--)
                {
                    Vector2 segmentStart = second[i].To2D();
                    Vector2 segmentEnd = second[i - 1].To2D();
                    if (unit.ServerPosition.Distance(segmentStart, segmentEnd, true) <= 50.Pow())
                    {
                        second.RemoveRange(0, i);
                        break;
                    }
                }
                Vector3[] first = new Vector3[] { unit.Position };
                return first.Concat<Vector3>(second).ToArray<Vector3>();
            }

            private static Prediction.Manager.PredictionOutput GetSdkBetaPrediction(Prediction.Manager.PredictionInput input)
            {
                PredictionInput input2 = new PredictionInput {
                    Collision = input.CollisionTypes.Count > 0,
                    Delay = input.Delay,
                    From = input.From,
                    Radius = input.Radius,
                    Range = input.Range,
                    Speed = input.Speed,
                    Type = TransformToSkillshotTypeEx(input.Type),
                    Unit = input.Target
                };
                PredictionOutput prediction = PredictionEx.GetPrediction(input2);
                int hitchance = (int) prediction.Hitchance;
                Prediction.Manager.PredictionOutput output2 = new Prediction.Manager.PredictionOutput(input) {
                    CastPosition = prediction.CastPosition,
                    PredictedPosition = prediction.UnitPosition,
                    HitChancePercent = (float) (((prediction.Hitchance - 1) * ((HitChanceEx) 0x19)) - 10),
                    HitChance = (hitchance < 3) ? HitChance.Impossible : ((HitChance) hitchance)
                };
                output2.CollisionGameObjects.AddRange(prediction.CollisionObjects.ToArray());
                return output2;
            }

            private static Prediction.Manager.PredictionOutput GetSdkPrediction(Prediction.Manager.PredictionInput input)
            {
                if ((input == null) || (input.Target == null))
                {
                    throw new ArgumentNullException("input");
                }
                PredictionData data = new PredictionData(TransformToPredictionType(input.Type), (int) input.Range, (int) input.Radius, (int) input.Radius, (int) (1000f * input.Delay), (int) input.Speed, TransformToCollisionCount(input.CollisionTypes), new Vector3?(input.From));
                PredictionResult result = GetPrediction(input.Target, data, false);
                Prediction.Manager.PredictionOutput output = new Prediction.Manager.PredictionOutput(input) {
                    CastPosition = result.CastPosition,
                    HitChancePercent = result.HitChancePercent,
                    HitChance = result.HitChance,
                    PredictedPosition = result.UnitPosition
                };
                output.CollisionGameObjects.AddRange(result.CollisionObjects);
                return output;
            }

            internal static void Initialize()
            {
                Menu = MainMenu.AddMenu("Prediction", "Prediction", null);
                Prediction.Manager.Menu = Menu;
                List<string> textValues = new List<string> { "SDK Prediction" };
                Prediction.Manager.Menu.Add<ComboBox>("PredictionSelected", new ComboBox("Prediction Selected:", textValues, 0));
                Prediction.Manager.Suscribe("SDK Prediction", new Func<Prediction.Manager.PredictionInput, Prediction.Manager.PredictionOutput>(Prediction.Position.GetSdkPrediction));
                Prediction.Manager.Suscribe("SDK Beta Prediction", new Func<Prediction.Manager.PredictionInput, Prediction.Manager.PredictionOutput>(Prediction.Position.GetSdkBetaPrediction));
                Menu.AddGroupLabel("General");
                Menu.Add<Slider>("skillshotRangeAdjustment", new Slider("Skillshot range scale {0}%", 100, 50, 120));
                Menu.AddLabel("It allows you to adjust the skillshot range.", 0x19);
                Menu.AddGroupLabel("Collision");
                Menu.Add<Slider>("extraHitboxRadius", new Slider("Extra Hitbox Radius", 0, 0, 80));
                Menu.AddLabel("Add more hitbox to objects when calculating collision.", 0x19);
                Obj_AI_Base.OnBasicAttack += new Obj_AI_BaseOnBasicAttack(Prediction.Position.OnBasicAttack);
                Obj_AI_Base.OnProcessSpellCast += new Obj_AI_ProcessSpellCast(Prediction.Position.OnProcessSpellCast);
                Obj_AI_Base.OnNewPath += new Obj_AI_BaseNewPath(Prediction.Position.OnNewPath);
            }

            private static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
            {
                if (sender.Type == GameObjectType.AIHeroClient)
                {
                    float num = sender.AttackCastDelay * 1000f;
                    _heroActionDuration[sender.NetworkId] = new Tuple<float, float>(Game.Time, num);
                }
            }

            private static void OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
            {
                if (!args.IsDash && (sender.Type == GameObjectType.AIHeroClient))
                {
                    List<Vector2> list;
                    if (!_pathGroup.TryGetValue(sender.NetworkId, out list))
                    {
                        list = new List<Vector2>();
                        _pathGroup[sender.NetworkId] = list;
                    }
                    Vector2 vector = args.Path.Last<Vector3>().To2D();
                    Vector2 vector2 = list.Any<Vector2>() ? list.ToArray().CenterPoint() : vector;
                    if (vector.Distance(vector2, true) <= 200.Pow())
                    {
                        list.Add(vector);
                        _tendencyDestination[sender.NetworkId] = list.ToArray().CenterPoint();
                    }
                    else
                    {
                        list.Clear();
                        _tendencyDestination[sender.NetworkId] = vector;
                    }
                }
                if (!args.IsDash && (sender.Type == GameObjectType.AIHeroClient))
                {
                    _moveTime[sender.NetworkId] = Game.Time;
                }
            }

            private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
            {
                if (sender.Type == GameObjectType.AIHeroClient)
                {
                    float num = args.SData.CastTime * 1000f;
                    if ((num == 0f) && !sender.IsMoving)
                    {
                        num = 150f;
                    }
                    _heroActionDuration[sender.NetworkId] = new Tuple<float, float>(Game.Time, num);
                }
            }

            public static PredictionResult PredictCircularMissile(Obj_AI_Base target, float range, int radius, int delay, float speed, Vector3? sourcePosition = new Vector3?(), bool ignoreCollision = false)
            {
                PredictionData data = new PredictionData(PredictionData.PredictionType.Circular, (int) range, radius, 0, delay, (int) speed, -1, sourcePosition);
                return GetPrediction(target, data, ignoreCollision);
            }

            public static PredictionResult[] PredictCircularMissileAoe(Obj_AI_Base[] targets, float range, int radius, int delay, float speed, Vector3? sourcePosition = new Vector3?())
            {
                PredictionData data = new PredictionData(PredictionData.PredictionType.Circular, (int) range, radius, 0, delay, (int) speed, -1, sourcePosition);
                return GetPredictionAoe(targets, data);
            }

            public static PredictionResult PredictConeSpell(Obj_AI_Base target, float range, int angle, int delay, float speed, Vector3? sourcePosition = new Vector3?(), bool ignoreCollision = false)
            {
                PredictionData data = new PredictionData(PredictionData.PredictionType.Cone, (int) range, 0, angle, delay, (int) speed, -1, sourcePosition);
                return GetPrediction(target, data, ignoreCollision);
            }

            public static PredictionResult[] PredictConeSpellAoe(Obj_AI_Base[] targets, float range, int angle, int delay, float speed, Vector3? sourcePosition = new Vector3?())
            {
                PredictionData data = new PredictionData(PredictionData.PredictionType.Circular, (int) range, 0, angle, delay, (int) speed, -1, sourcePosition);
                return GetPredictionAoe(targets, data);
            }

            public static PredictionResult PredictLinearMissile(Obj_AI_Base target, float range, int radius, int delay, float speed, int allowedCollisionCount, Vector3? sourcePosition = new Vector3?(), bool ignoreCollision = false)
            {
                PredictionData data = new PredictionData(PredictionData.PredictionType.Linear, (int) range, radius, 0, delay, (int) speed, allowedCollisionCount, sourcePosition);
                return GetPrediction(target, data, ignoreCollision);
            }

            public static Vector2 PredictUnitPosition(Obj_AI_Base unit, int time)
            {
                float range = (((float) time) / 1000f) * unit.MoveSpeed;
                Vector3[] realPath = GetRealPath(unit);
                for (int i = 0; i < (realPath.Length - 1); i++)
                {
                    Vector2 vector = realPath[i].To2D();
                    Vector2 vector2 = realPath[i + 1].To2D();
                    float num3 = vector.Distance(vector2, false);
                    if (num3 > range)
                    {
                        return vector.Extend(vector2, range);
                    }
                    range -= num3;
                }
                return ((realPath.Length == 0) ? unit.ServerPosition : realPath.Last<Vector3>()).To2D();
            }

            private static Vector2 TendencyDestination(Obj_AI_Base unit)
            {
                Vector2 vector2;
                Dash.DashEventArgs dashInfo = unit.GetDashInfo();
                if (dashInfo > null)
                {
                    return dashInfo.EndPos.To2D();
                }
                if ((unit.Type == GameObjectType.AIHeroClient) && _tendencyDestination.TryGetValue(unit.NetworkId, out vector2))
                {
                    return vector2;
                }
                return unit.Path.Last<Vector3>().To2D();
            }

            private static float TendencyFactor(Obj_AI_Base unit)
            {
                if (unit.Type == GameObjectType.AIHeroClient)
                {
                    float num;
                    List<Vector2> list;
                    _moveTime.TryGetValue(unit.NetworkId, out num);
                    _pathGroup.TryGetValue(unit.NetworkId, out list);
                    float num2 = 1f;
                    if ((Game.Time - num) >= 0.15)
                    {
                        num2 += 0.5f;
                    }
                    if ((list != null) && (list.Count >= 3))
                    {
                        num2 += 0.4f;
                    }
                    return num2;
                }
                return 2f;
            }

            private static Vector2[] TerrainCollisionPoints(Geometry.Polygon polygon)
            {
                List<Vector2> list = new List<Vector2>();
                List<Vector2> points = polygon.Points;
                for (int i = 0; i < points.Count; i++)
                {
                    Vector2 vector = points[i];
                }
                return list.ToArray();
            }

            private static Prediction.Manager.PredictionInput TransformPredictionData(Obj_AI_Base target, PredictionData data) => 
                new Prediction.Manager.PredictionInput { 
                    Target = target,
                    Delay = ((float) data.Delay) / 1000f,
                    Speed = data.Speed,
                    From = data.SourcePosition,
                    RangeCheckFrom = data.SourcePosition,
                    Radius = (data.Type == PredictionData.PredictionType.Cone) ? ((float) data.Angle) : ((float) data.Radius),
                    Range = data.Range,
                    CollisionTypes = TransformToCollisionTypes(data.AllowCollisionCount),
                    Type = TransformToSkillShotType(data.Type)
                };

            private static int TransformToCollisionCount(ICollection<CollisionType> collisionTypes)
            {
                int num = 0x7fffffff;
                if (collisionTypes.Contains(CollisionType.AiHeroClient))
                {
                    return 0;
                }
                if (collisionTypes.Contains(CollisionType.ObjAiMinion))
                {
                    num = -1;
                }
                return num;
            }

            private static HashSet<CollisionType> TransformToCollisionTypes(int allowedCollisionCount)
            {
                HashSet<CollisionType> set = new HashSet<CollisionType>();
                switch (allowedCollisionCount)
                {
                    case -1:
                        set.Add(CollisionType.ObjAiMinion);
                        return set;

                    case 0:
                        set.Add(CollisionType.AiHeroClient);
                        set.Add(CollisionType.ObjAiMinion);
                        return set;

                    case 1:
                        set.Add(CollisionType.ObjAiMinion);
                        return set;
                }
                return set;
            }

            private static PredictionResult TransformToPredictionResult(Prediction.Manager.PredictionOutput output) => 
                new PredictionResult(output.CastPosition, output.PredictedPosition, output.HitChancePercent, output.CollisionObjects, TransformToCollisionCount(output.Input.CollisionTypes), output.HitChance);

            private static PredictionData.PredictionType TransformToPredictionType(SkillShotType type)
            {
                switch (type)
                {
                    case SkillShotType.Linear:
                        return PredictionData.PredictionType.Linear;

                    case SkillShotType.Circular:
                        return PredictionData.PredictionType.Circular;

                    case SkillShotType.Cone:
                        return PredictionData.PredictionType.Cone;
                }
                return PredictionData.PredictionType.Circular;
            }

            private static SkillShotType TransformToSkillShotType(PredictionData.PredictionType type)
            {
                switch (type)
                {
                    case PredictionData.PredictionType.Linear:
                        return SkillShotType.Linear;

                    case PredictionData.PredictionType.Circular:
                        return SkillShotType.Circular;

                    case PredictionData.PredictionType.Cone:
                        return SkillShotType.Cone;
                }
                return SkillShotType.Circular;
            }

            private static SkillshotTypeEx TransformToSkillshotTypeEx(SkillShotType type)
            {
                switch (type)
                {
                    case SkillShotType.Linear:
                        return SkillshotTypeEx.SkillshotLine;

                    case SkillShotType.Circular:
                        return SkillshotTypeEx.SkillshotCircle;

                    case SkillShotType.Cone:
                        return SkillshotTypeEx.SkillshotCone;
                }
                return SkillshotTypeEx.SkillshotCircle;
            }

            internal static int ExtraHitbox =>
                (Menu["extraHitboxRadius"].Cast<Slider>().CurrentValue + 10);

            internal static EloBuddy.SDK.Menu.Menu Menu
            {
                [CompilerGenerated]
                get => 
                    <Menu>k__BackingField;
                [CompilerGenerated]
                set
                {
                    <Menu>k__BackingField = value;
                }
            }

            internal static int Ping =>
                Game.Ping;

            internal static int RangeAdjustment =>
                Menu["skillshotRangeAdjustment"].Cast<Slider>().CurrentValue;

            public static class Collision
            {
                private static List<YasuoWall> yasuoWalls = new List<YasuoWall>();

                public static bool CircularMissileCollision(Obj_AI_Base unit, Vector2 missileStartPos, Vector2 missileEndPos, float missileSpeed, int missileRadius, int delay, int extraRadius = 0)
                {
                    float num = (delay + Prediction.Position.Ping) + ((missileSpeed > 0f) ? (missileStartPos.Distance(missileEndPos, false) / missileSpeed) : 0f);
                    Vector2 vector = Prediction.Position.PredictUnitPosition(unit, (int) num);
                    return (missileEndPos.Distance(vector, true) <= ((float) ((missileRadius + unit.BoundingRadius) + extraRadius)).Pow());
                }

                public static Vector2 GetCollisionPoint(Vector2 start, Vector2 end, Vector2 position, float speed, float speed2)
                {
                    if (start == end)
                    {
                        return start;
                    }
                    Vector2 vector = start;
                    Vector2 target = end;
                    Vector2 vector3 = position;
                    Vector2 vector4 = new Vector2(vector3.X - vector.X, vector3.Y - vector.Y);
                    Vector2 vector5 = new Vector2(target.X - vector.X, target.Y - vector.Y);
                    double number = ((double) ((vector4.X * vector5.X) + (vector4.Y * vector5.Y))) / (Math.Sqrt(Math.Pow((double) vector4.X, 2.0) + Math.Pow((double) vector4.Y, 2.0)) * Math.Sqrt(Math.Pow((double) vector5.X, 2.0) + Math.Pow((double) vector5.Y, 2.0)));
                    float num2 = vector.Distance(vector3, false);
                    float range = (float) Math.Abs((double) ((((speed.Pow() * num2) * number) - Math.Sqrt((speed.Pow() * num2.Pow()) * ((speed2.Pow() + (speed.Pow() * number.Pow())) - speed.Pow()))) / ((double) (speed2.Pow() - speed.Pow()))));
                    return vector.Extend(target, range);
                }

                public static Vector3 GetYasuoWallCollision(Vector3 start, Vector3 end, bool onlyEnemy = true)
                {
                    Func<YasuoWall, bool> <>9__0;
                    foreach (YasuoWall wall in yasuoWalls.Where<YasuoWall>((Func<YasuoWall, bool>) (<>9__0 ?? (<>9__0 = w => ((w.Caster != null) && (w.Polygon != null)) && (!onlyEnemy || w.Caster.IsEnemy)))))
                    {
                        List<Vector2> points = wall.Polygon.Points;
                        for (int i = 0; i < points.Count; i++)
                        {
                            Geometry.IntersectionResult result = points[i].Intersection(points[(i >= 3) ? 0 : (i + 1)], start.To2D(), end.To2D());
                            if (result.Intersects)
                            {
                                return result.Point.To3DWorld();
                            }
                        }
                    }
                    return Vector3.Zero;
                }

                internal static void Initialize()
                {
                    if (EntityManager.Heroes.AllHeroes.Any<AIHeroClient>(i => i.Hero == Champion.Yasuo))
                    {
                        GameObject.OnCreate += delegate (GameObject sender, EventArgs args) {
                            MissileClient client = sender as MissileClient;
                            if (client != null)
                            {
                                AIHeroClient caster = client.SpellCaster as AIHeroClient;
                                if ((caster != null) && caster.BaseSkinName.Equals("Yasuo"))
                                {
                                    YasuoWall wall = yasuoWalls.FirstOrDefault<YasuoWall>(w => (w.Caster != null) && w.Caster.IdEquals(caster));
                                    string name = client.SData.Name;
                                    bool flag = name.Equals("YasuoWMovingWallMisL");
                                    bool flag2 = name.Equals("YasuoWMovingWallMisVis");
                                    bool flag3 = name.Equals("YasuoWMovingWallMisR");
                                    if (wall > null)
                                    {
                                        if (flag)
                                        {
                                            wall.LeftEdge = client;
                                        }
                                        if (flag2)
                                        {
                                            wall.Middle = client;
                                        }
                                        if (flag3)
                                        {
                                            wall.RightEdge = client;
                                        }
                                    }
                                    else
                                    {
                                        YasuoWall item = new YasuoWall {
                                            Caster = caster
                                        };
                                        if (flag)
                                        {
                                            item.LeftEdge = client;
                                        }
                                        if (flag2)
                                        {
                                            item.Middle = client;
                                        }
                                        if (flag3)
                                        {
                                            item.RightEdge = client;
                                        }
                                        yasuoWalls.Add(item);
                                    }
                                }
                            }
                        };
                        Game.OnTick += <args> => yasuoWalls.RemoveAll(w => (Core.GameTickCount - w.StartTick) > 4000f);
                    }
                }

                public static bool LinearMissileCollision(Obj_AI_Base unit, Vector2 missileStartPos, Vector2 missileEndPos, float missileSpeed, int missileWidth, int delay, int extraRadius = 0)
                {
                    float num = unit.BoundingRadius + extraRadius;
                    if (!unit.IsMoving)
                    {
                        return (Geometry.SegmentCircleIntersectionPriority(missileStartPos, missileEndPos, unit.ServerPosition.To2D(), num, unit.ServerPosition.To2D(), num + (missileWidth / 2)) > 0);
                    }
                    int time = delay + Prediction.Position.Ping;
                    Vector2 point = Prediction.Position.PredictUnitPosition(unit, time);
                    Vector3[] realPath = Prediction.Position.GetRealPath(unit);
                    float num3 = 0f;
                    for (int i = 0; i < (realPath.Length - 1); i++)
                    {
                        Vector2 segmentStart = realPath[i].To2D();
                        Vector2 segmentEnd = realPath[i + 1].To2D();
                        if (point != Vector2.Zero)
                        {
                            if (!Geometry.PointInLineSegment(segmentStart, segmentEnd, point))
                            {
                                continue;
                            }
                            segmentStart = point;
                            point = Vector2.Zero;
                        }
                        Vector2 vector4 = missileStartPos.Extend(missileEndPos, num3 * missileSpeed);
                        if (!Geometry.PointInLineSegment(missileStartPos, missileEndPos, vector4))
                        {
                            break;
                        }
                        if (MovingObjectsCollision(vector4, missileEndPos, (float) (missileWidth / 2), missileSpeed, false, segmentStart, segmentEnd, num, unit.MoveSpeed, true))
                        {
                            return true;
                        }
                        num3 += segmentStart.Distance(segmentEnd, false) / unit.MoveSpeed;
                    }
                    return false;
                }

                public static bool MovingObjectsCollision(Vector2 start, Vector2 destination, float hitbox, float speed, bool isUnit, Vector2 start2, Vector2 destination2, float hitbox2, float speed2, bool isUnit2)
                {
                    float num = speed2;
                    float num2 = start2.Distance(destination2, false);
                    float x = destination2.X;
                    float num4 = start2.X;
                    float num5 = start.Distance(destination, false);
                    float num6 = destination.X;
                    float num7 = speed;
                    float num8 = start.X;
                    float y = destination2.Y;
                    float num10 = start2.Y;
                    float num11 = destination.Y;
                    float num12 = start.Y;
                    float num13 = ((float) (hitbox + hitbox2)).Pow();
                    float num14 = -1f / (((num2 * num2) * num5) * num5);
                    float num15 = ((((((((((((((((((((((((-num * num) * x) * x) * num5) * num5) + ((((((2f * num) * num) * x) * num4) * num5) * num5)) - (((((num * num) * num4) * num4) * num5) * num5)) - (((((num * num) * num5) * num5) * y) * y)) + ((((((2f * num) * num) * num5) * num5) * y) * num10)) - (((((num * num) * num5) * num5) * num10) * num10)) + ((((((2f * num) * num2) * x) * num5) * num6) * num7)) - ((((((2f * num) * num2) * x) * num5) * num7) * num8)) - ((((((2f * num) * num2) * num4) * num5) * num6) * num7)) + ((((((2f * num) * num2) * num4) * num5) * num7) * num8)) + ((((((2f * num) * num2) * num5) * num7) * y) * num11)) - ((((((2f * num) * num2) * num5) * num7) * y) * num12)) - ((((((2f * num) * num2) * num5) * num7) * num10) * num11)) + ((((((2f * num) * num2) * num5) * num7) * num10) * num12)) - (((((num2 * num2) * num6) * num6) * num7) * num7)) + ((((((2f * num2) * num2) * num6) * num7) * num7) * num8)) - (((((num2 * num2) * num7) * num7) * num8) * num8)) - (((((num2 * num2) * num7) * num7) * num11) * num11)) + ((((((2f * num2) * num2) * num7) * num7) * num11) * num12)) - (((((num2 * num2) * num7) * num7) * num12) * num12)) * num14;
                    float num16 = (((((((((((((((((((((-2f * num) * num2) * x) * num4) * num5) * num5) + ((((((2f * num) * num2) * x) * num5) * num5) * num8)) + ((((((2f * num) * num2) * num4) * num4) * num5) * num5)) - ((((((2f * num) * num2) * num4) * num5) * num5) * num8)) - ((((((2f * num) * num2) * num5) * num5) * y) * num10)) + ((((((2f * num) * num2) * num5) * num5) * y) * num12)) + ((((((2f * num) * num2) * num5) * num5) * num10) * num10)) - ((((((2f * num) * num2) * num5) * num5) * num10) * num12)) + ((((((2f * num2) * num2) * num4) * num5) * num6) * num7)) - ((((((2f * num2) * num2) * num4) * num5) * num7) * num8)) - ((((((2f * num2) * num2) * num5) * num6) * num7) * num8)) + ((((((2f * num2) * num2) * num5) * num7) * num8) * num8)) + ((((((2f * num2) * num2) * num5) * num7) * num10) * num11)) - ((((((2f * num2) * num2) * num5) * num7) * num10) * num12)) - ((((((2f * num2) * num2) * num5) * num7) * num11) * num12)) + ((((((2f * num2) * num2) * num5) * num7) * num12) * num12)) * num14;
                    float num17 = ((((((((((num2 * num2) * (-num4 * num4)) * num5) * num5) + ((((((2f * num2) * num2) * num4) * num5) * num5) * num8)) + ((((num2 * num2) * num5) * num5) * num13)) - (((((num2 * num2) * num5) * num5) * num8) * num8)) - (((((num2 * num2) * num5) * num5) * num10) * num10)) + ((((((2f * num2) * num2) * num5) * num5) * num10) * num12)) - (((((num2 * num2) * num5) * num5) * num12) * num12)) * num14;
                    float num18 = (num16 * num16) - ((4f * num15) * num17);
                    if ((num2 == 0f) || (num5 == 0f))
                    {
                        return true;
                    }
                    if ((num15 != 0f) || (num16 != 0f))
                    {
                        if (num18 < 0f)
                        {
                            return (num17 <= 0f);
                        }
                        double num19 = (-num16 + Math.Sqrt((double) num18)) / ((double) (2f * num15));
                        double num20 = (-num16 - Math.Sqrt((double) num18)) / ((double) (2f * num15));
                        float num21 = start.Distance(destination, false) / speed;
                        float num22 = start2.Distance(destination2, false) / speed2;
                        float num23 = 0f;
                        float num24 = (num21 > num22) ? num22 : num21;
                        if ((num19 >= num23) && (num19 <= num24))
                        {
                            return true;
                        }
                        if ((num20 >= num23) && (num20 <= num24))
                        {
                            return true;
                        }
                        if (((num21 > num22) || (num22 > num21)) && (num17 > 0f))
                        {
                            if (((num22 > num21) && !isUnit) || ((num21 > num22) && !isUnit2))
                            {
                                return false;
                            }
                            float num25 = (num21 > num22) ? num22 : num21;
                            Vector2 source = (num21 > num22) ? start : start2;
                            Vector2 segmentEnd = (num21 > num22) ? destination : destination2;
                            float num26 = (num21 > num22) ? speed : speed2;
                            Vector2 vector3 = (num21 > num22) ? destination2 : destination;
                            float num27 = hitbox + hitbox2;
                            return (Geometry.SegmentCircleIntersectionPriority(source.Extend(segmentEnd, num26 * num25), segmentEnd, vector3, num27, vector3, num27) > 0);
                        }
                    }
                    return (num17 <= 0f);
                }

                [Serializable, CompilerGenerated]
                private sealed class <>c
                {
                    public static readonly Prediction.Position.Collision.<>c <>9 = new Prediction.Position.Collision.<>c();
                    public static Func<AIHeroClient, bool> <>9__2_0;
                    public static GameObjectCreate <>9__2_1;
                    public static GameTick <>9__2_3;
                    public static Predicate<Prediction.Position.Collision.YasuoWall> <>9__2_4;

                    internal bool <Initialize>b__2_0(AIHeroClient i) => 
                        (i.Hero == Champion.Yasuo);

                    internal void <Initialize>b__2_1(GameObject sender, EventArgs args)
                    {
                        AIHeroClient caster;
                        MissileClient client = sender as MissileClient;
                        if (client != null)
                        {
                            caster = client.SpellCaster as AIHeroClient;
                            if ((caster != null) && caster.BaseSkinName.Equals("Yasuo"))
                            {
                                Prediction.Position.Collision.YasuoWall wall = Prediction.Position.Collision.yasuoWalls.FirstOrDefault<Prediction.Position.Collision.YasuoWall>(w => (w.Caster != null) && w.Caster.IdEquals(caster));
                                string name = client.SData.Name;
                                bool flag = name.Equals("YasuoWMovingWallMisL");
                                bool flag2 = name.Equals("YasuoWMovingWallMisVis");
                                bool flag3 = name.Equals("YasuoWMovingWallMisR");
                                if (wall > null)
                                {
                                    if (flag)
                                    {
                                        wall.LeftEdge = client;
                                    }
                                    if (flag2)
                                    {
                                        wall.Middle = client;
                                    }
                                    if (flag3)
                                    {
                                        wall.RightEdge = client;
                                    }
                                }
                                else
                                {
                                    Prediction.Position.Collision.YasuoWall item = new Prediction.Position.Collision.YasuoWall {
                                        Caster = caster
                                    };
                                    if (flag)
                                    {
                                        item.LeftEdge = client;
                                    }
                                    if (flag2)
                                    {
                                        item.Middle = client;
                                    }
                                    if (flag3)
                                    {
                                        item.RightEdge = client;
                                    }
                                    Prediction.Position.Collision.yasuoWalls.Add(item);
                                }
                            }
                        }
                    }

                    internal void <Initialize>b__2_3(EventArgs <args>)
                    {
                        Prediction.Position.Collision.yasuoWalls.RemoveAll(w => (Core.GameTickCount - w.StartTick) > 4000f);
                    }

                    internal bool <Initialize>b__2_4(Prediction.Position.Collision.YasuoWall w) => 
                        ((Core.GameTickCount - w.StartTick) > 4000f);
                }

                internal class YasuoWall
                {
                    internal AIHeroClient Caster;
                    internal MissileClient LeftEdge;
                    internal MissileClient Middle;
                    internal MissileClient RightEdge;
                    internal float StartTick = Core.GameTickCount;

                    internal EloBuddy.SDK.Geometry.Polygon.Rectangle Polygon
                    {
                        get
                        {
                            int num = 120;
                            int num2 = 30;
                            if ((this.LeftEdge != null) && (this.RightEdge > null))
                            {
                                return new EloBuddy.SDK.Geometry.Polygon.Rectangle(this.LeftEdge.Position.Extend(this.RightEdge.Position, (float) -num2), this.RightEdge.Position.Extend(this.LeftEdge.Position, (float) -num2), (float) num);
                            }
                            if ((this.LeftEdge != null) && (this.Middle > null))
                            {
                                float num3 = ((GameObject) this.LeftEdge).Distance(((GameObject) this.Middle), false) * 2f;
                                return new EloBuddy.SDK.Geometry.Polygon.Rectangle(this.LeftEdge.Position.Extend(this.Middle.Position, (float) -num2), this.LeftEdge.Position.Extend(this.Middle.Position, num3 + num2), (float) num);
                            }
                            if ((this.RightEdge != null) && (this.Middle > null))
                            {
                                float num4 = ((GameObject) this.RightEdge).Distance(((GameObject) this.Middle), false) * 2f;
                                return new EloBuddy.SDK.Geometry.Polygon.Rectangle(this.RightEdge.Position.Extend(this.Middle.Position, (float) -num2), this.RightEdge.Position.Extend(this.Middle.Position, num4 + num2), (float) num);
                            }
                            return null;
                        }
                    }
                }
            }

            internal enum CollisionableObjectsEx
            {
                Minions,
                Heroes,
                YasuoWall,
                Walls,
                Allies
            }

            internal static class CollisionEx
            {
                private static int _wallCastT;
                private static Vector2 _yasuoWallCastedPos;

                static CollisionEx()
                {
                    Obj_AI_Base.OnProcessSpellCast += new Obj_AI_ProcessSpellCast(Prediction.Position.CollisionEx.Obj_AI_Hero_OnProcessSpellCast);
                }

                internal static List<Obj_AI_Base> GetCollision(List<Vector3> positions, Prediction.Position.PredictionInput input)
                {
                    Func<Obj_AI_Minion, bool> <>9__0;
                    Predicate<AIHeroClient> <>9__1;
                    Predicate<AIHeroClient> <>9__2;
                    List<Obj_AI_Base> source = new List<Obj_AI_Base>();
                    foreach (Vector3 vector in positions)
                    {
                        Prediction.Position.CollisionableObjectsEx[] collisionObjects = input.CollisionObjects;
                        for (int i = 0; i < collisionObjects.Length; i++)
                        {
                            float num2;
                            GameObject obj2;
                            int num4;
                            Vector2 vector5;
                            switch (collisionObjects[i])
                            {
                                case Prediction.Position.CollisionableObjectsEx.Minions:
                                {
                                    foreach (Obj_AI_Minion minion in ObjectManager.Get<Obj_AI_Minion>().Where<Obj_AI_Minion>((Func<Obj_AI_Minion, bool>) (<>9__0 ?? (<>9__0 = minion => minion.IsValidTarget(new float?(Math.Min((float) ((input.Range + input.Radius) + 100f), (float) 2000f)), true, new Vector3?(input.RangeCheckFrom))))))
                                    {
                                        input.Unit = minion;
                                        if (Prediction.Position.PredictionEx.GetPrediction(input, false, false).UnitPosition.To2D().Distance(input.From.To2D(), vector.To2D(), true, true) <= Math.Pow((double) ((input.Radius + 15f) + minion.BoundingRadius), 2.0))
                                        {
                                            source.Add(minion);
                                        }
                                    }
                                    continue;
                                }
                                case Prediction.Position.CollisionableObjectsEx.Heroes:
                                {
                                    foreach (AIHeroClient client in EntityManager.Heroes.Enemies.FindAll(<>9__1 ?? (<>9__1 = hero => hero.IsValidTarget(new float?(Math.Min((float) ((input.Range + input.Radius) + 100f), (float) 2000f)), true, new Vector3?(input.RangeCheckFrom)))))
                                    {
                                        input.Unit = client;
                                        if (Prediction.Position.PredictionEx.GetPrediction(input, false, false).UnitPosition.To2D().Distance(input.From.To2D(), vector.To2D(), true, true) <= Math.Pow((double) ((input.Radius + 50f) + client.BoundingRadius), 2.0))
                                        {
                                            source.Add(client);
                                        }
                                    }
                                    continue;
                                }
                                case Prediction.Position.CollisionableObjectsEx.YasuoWall:
                                {
                                    if ((Core.GameTickCount - _wallCastT) <= 0xfa0)
                                    {
                                        goto Label_039E;
                                    }
                                    continue;
                                }
                                case Prediction.Position.CollisionableObjectsEx.Walls:
                                    num2 = vector.Distance(input.From, false) / 20f;
                                    num4 = 0;
                                    goto Label_036F;

                                case Prediction.Position.CollisionableObjectsEx.Allies:
                                {
                                    foreach (AIHeroClient client2 in EntityManager.Heroes.Allies.FindAll(<>9__2 ?? (<>9__2 = hero => Vector3.Distance(ObjectManager.Player.ServerPosition, hero.ServerPosition) <= Math.Min((float) ((input.Range + input.Radius) + 100f), (float) 2000f))))
                                    {
                                        input.Unit = client2;
                                        if (Prediction.Position.PredictionEx.GetPrediction(input, false, false).UnitPosition.To2D().Distance(input.From.To2D(), vector.To2D(), true, true) <= Math.Pow((double) ((input.Radius + 50f) + client2.BoundingRadius), 2.0))
                                        {
                                            source.Add(client2);
                                        }
                                    }
                                    continue;
                                }
                                default:
                                {
                                    continue;
                                }
                            }
                        Label_0344:
                            vector5 = input.From.To2D().Extend(vector.To2D(), num2 * num4);
                            num4++;
                        Label_036F:
                            if (num4 < 20)
                            {
                                goto Label_0344;
                            }
                            continue;
                        Label_039E:
                            obj2 = null;
                            foreach (GameObject obj3 in from gameObject in ObjectManager.Get<GameObject>()
                                where gameObject.IsValid && Regex.IsMatch(gameObject.Name, @"_w_windwall_enemy_0.\.troy", RegexOptions.IgnoreCase)
                                select gameObject)
                            {
                                obj2 = obj3;
                            }
                            if (obj2 != null)
                            {
                                string str = obj2.Name.Substring(obj2.Name.Length - 6, 1);
                                int num3 = 300 + (50 * Convert.ToInt32(str));
                                Vector2 vector2 = (obj2.Position.To2D() - _yasuoWallCastedPos).Normalized().Perpendicular();
                                Vector2 vector3 = obj2.Position.To2D() + ((Vector2) ((((float) num3) / 2f) * vector2));
                                Vector2 vector4 = vector3 - ((Vector2) (num3 * vector2));
                                if (vector3.Intersection(vector4, vector.To2D(), input.From.To2D()).Intersects)
                                {
                                    float num5 = Core.GameTickCount + (((vector3.Intersection(vector4, vector.To2D(), input.From.To2D()).Point.Distance(input.From, false) / input.Speed) + input.Delay) * 1000f);
                                    if (num5 < (_wallCastT + 0xfa0))
                                    {
                                        source.Add(ObjectManager.Player);
                                    }
                                }
                            }
                        }
                    }
                    return source.Distinct<Obj_AI_Base>().ToList<Obj_AI_Base>();
                }

                private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
                {
                    if ((sender.IsValid && (sender.Team != ObjectManager.Player.Team)) && (args.SData.Name == "YasuoWMovingWall"))
                    {
                        _wallCastT = Core.GameTickCount;
                        _yasuoWallCastedPos = sender.ServerPosition.To2D();
                    }
                }

                [Serializable, CompilerGenerated]
                private sealed class <>c
                {
                    public static readonly Prediction.Position.CollisionEx.<>c <>9 = new Prediction.Position.CollisionEx.<>c();
                    public static Func<GameObject, bool> <>9__4_3;

                    internal bool <GetCollision>b__4_3(GameObject gameObject) => 
                        (gameObject.IsValid && Regex.IsMatch(gameObject.Name, @"_w_windwall_enemy_0.\.troy", RegexOptions.IgnoreCase));
                }
            }

            internal enum HitChanceEx
            {
                Collision,
                OutOfRange,
                Impossible,
                Low,
                Medium,
                High,
                VeryHigh,
                Dashing,
                Immobile
            }

            internal static class PathTracker
            {
                private const double MaxTime = 1.5;
                private static readonly Dictionary<int, List<Prediction.Position.StoredPath>> StoredPaths = new Dictionary<int, List<Prediction.Position.StoredPath>>();

                static PathTracker()
                {
                    Obj_AI_Base.OnNewPath += new Obj_AI_BaseNewPath(Prediction.Position.PathTracker.Obj_AI_Base_OnNewPath);
                }

                public static Prediction.Position.StoredPath GetCurrentPath(Obj_AI_Base unit) => 
                    (StoredPaths.ContainsKey(unit.NetworkId) ? StoredPaths[unit.NetworkId].LastOrDefault<Prediction.Position.StoredPath>() : new Prediction.Position.StoredPath());

                public static double GetMeanSpeed(Obj_AI_Base unit, double maxT)
                {
                    List<Prediction.Position.StoredPath> storedPaths = GetStoredPaths(unit, 1.5);
                    double num = 0.0;
                    if (storedPaths.Count > 0)
                    {
                        num += (maxT - storedPaths[0].Time) * unit.MoveSpeed;
                        for (int i = 0; i < (storedPaths.Count - 1); i++)
                        {
                            Prediction.Position.StoredPath path2 = storedPaths[i];
                            Prediction.Position.StoredPath path3 = storedPaths[i + 1];
                            if (path2.WaypointCount > 0)
                            {
                                num += Math.Min((path2.Time - path3.Time) * unit.MoveSpeed, (double) path2.Path.PathLength());
                            }
                        }
                        Prediction.Position.StoredPath path = storedPaths.Last<Prediction.Position.StoredPath>();
                        if (path.WaypointCount > 0)
                        {
                            num += Math.Min(path.Time * unit.MoveSpeed, (double) path.Path.PathLength());
                        }
                    }
                    else
                    {
                        return (double) unit.MoveSpeed;
                    }
                    return (num / maxT);
                }

                internal static List<Prediction.Position.StoredPath> GetStoredPaths(Obj_AI_Base unit, double maxT) => 
                    (StoredPaths.ContainsKey(unit.NetworkId) ? (from p in StoredPaths[unit.NetworkId]
                        where p.Time < maxT
                        select p).ToList<Prediction.Position.StoredPath>() : new List<Prediction.Position.StoredPath>());

                public static Vector3 GetTendency(Obj_AI_Base unit)
                {
                    List<Prediction.Position.StoredPath> storedPaths = GetStoredPaths(unit, 1.5);
                    Vector2 vector = new Vector2();
                    foreach (Prediction.Position.StoredPath path in storedPaths)
                    {
                        int num = 1;
                        vector += (Vector2) (num * (path.EndPoint - unit.ServerPosition.To2D()).Normalized());
                    }
                    vector = (Vector2) (vector / ((float) storedPaths.Count));
                    return vector.To3D(0);
                }

                private static void Obj_AI_Base_OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
                {
                    if (sender is AIHeroClient)
                    {
                        if (!StoredPaths.ContainsKey(sender.NetworkId))
                        {
                            StoredPaths.Add(sender.NetworkId, new List<Prediction.Position.StoredPath>());
                        }
                        Prediction.Position.StoredPath item = new Prediction.Position.StoredPath {
                            Tick = Core.GameTickCount,
                            Path = args.Path.ToList<Vector3>().To2D()
                        };
                        StoredPaths[sender.NetworkId].Add(item);
                        if (StoredPaths[sender.NetworkId].Count > 50)
                        {
                            StoredPaths[sender.NetworkId].RemoveRange(0, 40);
                        }
                    }
                }
            }

            public class PredictionData
            {
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int <AllowCollisionCount>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int <Angle>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int <Delay>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int <Radius>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private int <Range>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private Vector3 <SourcePosition>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private float <Speed>k__BackingField;
                [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private PredictionType <Type>k__BackingField;

                public PredictionData(PredictionType type, int range, int radius, int angle, int delay, int speed, int allowCollisionCount = 0, Vector3? sourcePosition = new Vector3?())
                {
                    this.Type = type;
                    this.Range = (int) (((float) (range * Prediction.Position.RangeAdjustment)) / 100f);
                    this.Radius = radius;
                    this.Angle = angle;
                    this.Delay = delay;
                    this.AllowCollisionCount = allowCollisionCount;
                    this.Speed = (speed <= 0) ? ((float) 0x7fffffff) : ((float) speed);
                    Vector3? nullable = sourcePosition;
                    this.SourcePosition = nullable.HasValue ? nullable.GetValueOrDefault() : Player.Instance.ServerPosition;
                }

                internal List<List<Vector2>> GetAoeGroups(Dictionary<Obj_AI_Base, PredictionResult> results)
                {
                    List<List<Vector2>> source = new List<List<Vector2>>();
                    int groupRange;
                    switch (this.Type)
                    {
                        case PredictionType.Linear:
                            Logger.Warn("AoE prediction for linear skillshots is not available yet", new object[0]);
                            return new List<List<Vector2>>();

                        case PredictionType.Circular:
                            groupRange = this.Radius;
                            foreach (Obj_AI_Base base2 in results.Keys)
                            {
                                Func<Vector2, bool> <>9__3;
                                PredictionResult result = results[base2];
                                List<Vector2> newGroup = new List<Vector2> {
                                    result.UnitPosition.To2D()
                                };
                                newGroup.AddRange(from r in results
                                    where r.Value.UnitPosition.Distance(result.UnitPosition, true) <= ((float) (groupRange + (r.Key.BoundingRadius / 2f))).Pow()
                                    select r.Value.UnitPosition.To2D());
                                if (source.All<List<Vector2>>(g => g.Count<Vector2>((<>9__3 ?? (<>9__3 = p => newGroup.Contains(p)))) != newGroup.Count))
                                {
                                    source.Add(newGroup);
                                }
                            }
                            return source;

                        case PredictionType.Cone:
                        {
                            float num = ((float) (((2 * this.Angle) * 3.1415926535897931) / 360.0)) * this.Range;
                            Vector2 center = this.SourcePosition.To2D();
                            foreach (Obj_AI_Base base3 in results.Keys)
                            {
                                Func<Vector2, bool> <>9__5;
                                PredictionResult result = results[base3];
                                List<Vector2> newGroup = new List<Vector2> {
                                    result.UnitPosition.To2D()
                                };
                                foreach (KeyValuePair<Obj_AI_Base, PredictionResult> pair in results)
                                {
                                    Geometry.Polygon.Sector sector = new Geometry.Polygon.Sector(center, pair.Value.UnitPosition.To2D().Extend(result.UnitPosition, num / 2f), (float) ((this.Angle * 3.1415926535897931) / 180.0), (float) this.Range, 20);
                                    if (sector.IsInside(result.UnitPosition))
                                    {
                                        newGroup.Add(pair.Value.UnitPosition.To2D());
                                    }
                                }
                                if (source.All<List<Vector2>>(g => g.Count<Vector2>((<>9__5 ?? (<>9__5 = p => newGroup.Contains(p)))) != newGroup.Count))
                                {
                                    source.Add(newGroup);
                                }
                            }
                            return source;
                        }
                    }
                    return source;
                }

                internal CollisionCheck GetCollisionCalculator(Vector2 castPos)
                {
                    <>c__DisplayClass36_0 class_;
                    Geometry.Polygon sector;
                    switch (this.Type)
                    {
                        case PredictionType.Linear:
                            return new CollisionCheck(class_.<GetCollisionCalculator>b__0);

                        case PredictionType.Circular:
                            return new CollisionCheck(class_.<GetCollisionCalculator>b__1);

                        case PredictionType.Cone:
                            sector = this.GetSkillshotPolygon(castPos);
                            return delegate (Obj_AI_Base target, Obj_AI_Base unit) {
                                int time = this.Delay + Prediction.Position.Ping;
                                Vector2 point = Prediction.Position.PredictUnitPosition(unit, time);
                                return sector.IsInside(point);
                            };
                    }
                    return null;
                }

                internal Geometry.Polygon GetSkillshotPolygon(Vector2 castPos)
                {
                    switch (this.Type)
                    {
                        case PredictionType.Linear:
                            return new Geometry.Polygon.Rectangle(this.SourcePosition, this.SourcePosition.Extend(castPos, ((float) this.Range)).To3DWorld(), (float) (this.Radius * 2));

                        case PredictionType.Circular:
                            return new Geometry.Polygon.Circle(castPos, (float) this.Radius, 20);

                        case PredictionType.Cone:
                            return new Geometry.Polygon.Sector(this.SourcePosition, castPos.To3DWorld(), (float) ((this.Angle * 3.1415926535897931) / 180.0), (float) this.Range, 20);
                    }
                    return null;
                }

                public int AllowCollisionCount { get; internal set; }

                public int Angle { get; internal set; }

                public int Delay { get; internal set; }

                public int Radius { get; internal set; }

                public int Range { get; internal set; }

                public Vector3 SourcePosition { get; internal set; }

                public float Speed { get; internal set; }

                public PredictionType Type { get; internal set; }

                [Serializable, CompilerGenerated]
                private sealed class <>c
                {
                    public static readonly Prediction.Position.PredictionData.<>c <>9 = new Prediction.Position.PredictionData.<>c();
                    public static Func<KeyValuePair<Obj_AI_Base, PredictionResult>, Vector2> <>9__37_1;

                    internal Vector2 <GetAoeGroups>b__37_1(KeyValuePair<Obj_AI_Base, PredictionResult> r) => 
                        r.Value.UnitPosition.To2D();
                }

                internal delegate bool CollisionCheck(Obj_AI_Base target, Obj_AI_Base unit);

                public enum PredictionType
                {
                    Linear,
                    Circular,
                    Cone
                }
            }

            internal static class PredictionEx
            {
                public static List<Vector3> CirclePoints(float CircleLineSegmentN, float radius, Vector3 position)
                {
                    List<Vector3> list = new List<Vector3>();
                    for (int i = 1; i <= CircleLineSegmentN; i++)
                    {
                        double d = ((i * 2) * 3.1415926535897931) / ((double) CircleLineSegmentN);
                        Vector3 item = new Vector3(position.X + (radius * ((float) Math.Cos(d))), position.Y + (radius * ((float) Math.Sin(d))), position.Z);
                        list.Add(item);
                    }
                    return list;
                }

                internal static Prediction.Position.PredictionOutput GetDashingPrediction(Prediction.Position.PredictionInput input)
                {
                    Dash.DashEventArgs dashInfo = input.Unit.GetDashInfo();
                    Prediction.Position.PredictionOutput output = new Prediction.Position.PredictionOutput {
                        Input = input
                    };
                    if (input.Unit.IsDashing())
                    {
                        Vector2 segmentEnd = dashInfo.Path.LastOrDefault<Vector2>();
                        float speed = dashInfo.Speed;
                        List<Vector2> path = new List<Vector2> {
                            input.Unit.ServerPosition.To2D(),
                            segmentEnd
                        };
                        Prediction.Position.PredictionOutput output2 = GetPositionOnPath(input, path, speed);
                        if ((output2.Hitchance >= Prediction.Position.HitChanceEx.High) && (output2.UnitPosition.To2D().Distance(input.Unit.Position.To2D(), segmentEnd, true) < 200f))
                        {
                            output2.CastPosition = output2.CastPosition;
                            output2.Hitchance = Prediction.Position.HitChanceEx.Dashing;
                            return output2;
                        }
                        if (dashInfo.Path.PathLength() > 200f)
                        {
                            float num2 = ((input.Delay / 2f) + (input.From.To2D().Distance(segmentEnd, false) / input.Speed)) - 0.25f;
                            if (num2 <= ((input.Unit.Distance(segmentEnd, false) / speed) + (input.RealRadius / input.Unit.MoveSpeed)))
                            {
                                return new Prediction.Position.PredictionOutput { 
                                    CastPosition = segmentEnd.To3D(0),
                                    UnitPosition = segmentEnd.To3D(0),
                                    Hitchance = Prediction.Position.HitChanceEx.Dashing
                                };
                            }
                        }
                        output.CastPosition = dashInfo.Path.LastOrDefault<Vector2>().To3D(0);
                        output.UnitPosition = output.CastPosition;
                    }
                    return output;
                }

                internal static Prediction.Position.PredictionOutput GetImmobilePrediction(Prediction.Position.PredictionInput input, double remainingImmobileT)
                {
                    float num = input.Delay + (input.Unit.Distance(input.From, false) / input.Speed);
                    if (num <= (remainingImmobileT + (input.RealRadius / input.Unit.MoveSpeed)))
                    {
                        return new Prediction.Position.PredictionOutput { 
                            CastPosition = input.Unit.ServerPosition,
                            UnitPosition = input.Unit.Position,
                            Hitchance = Prediction.Position.HitChanceEx.Immobile
                        };
                    }
                    return new Prediction.Position.PredictionOutput { 
                        Input = input,
                        CastPosition = input.Unit.ServerPosition,
                        UnitPosition = input.Unit.ServerPosition,
                        Hitchance = Prediction.Position.HitChanceEx.High
                    };
                }

                internal static Prediction.Position.PredictionOutput GetPositionOnPath(Prediction.Position.PredictionInput input, List<Vector2> path, float speed = -1f)
                {
                    if (input.Unit.Distance(input.From, true) < 62500f)
                    {
                        speed /= 1.5f;
                    }
                    speed = (Math.Abs((float) (speed - -1f)) < float.Epsilon) ? input.Unit.MoveSpeed : speed;
                    if ((path.Count <= 1) || (input.Unit.Spellbook.IsAutoAttacking && !input.Unit.IsDashing()))
                    {
                        return new Prediction.Position.PredictionOutput { 
                            Input = input,
                            UnitPosition = input.Unit.ServerPosition,
                            CastPosition = input.Unit.ServerPosition,
                            Hitchance = Prediction.Position.HitChanceEx.High
                        };
                    }
                    float num = path.PathLength();
                    if ((num >= ((input.Delay * speed) - input.RealRadius)) && (Math.Abs((float) (input.Speed - float.MaxValue)) < float.Epsilon))
                    {
                        float num2 = (input.Delay * speed) - input.RealRadius;
                        for (int i = 0; i < (path.Count - 1); i++)
                        {
                            Vector2 vector2 = path[i];
                            Vector2 vector3 = path[i + 1];
                            float num4 = vector2.Distance(vector3, false);
                            if (num4 >= num2)
                            {
                                Vector2 vector4 = (vector3 - vector2).Normalized();
                                Vector2 vector5 = vector2 + ((Vector2) (vector4 * num2));
                                Vector2 vector6 = vector2 + ((Vector2) (vector4 * ((i == (path.Count - 2)) ? Math.Min(num2 + input.RealRadius, num4) : (num2 + input.RealRadius))));
                                return new Prediction.Position.PredictionOutput { 
                                    Input = input,
                                    CastPosition = vector5.To3D(0),
                                    UnitPosition = vector6.To3D(0),
                                    Hitchance = Prediction.Position.HitChanceEx.High
                                };
                            }
                            num2 -= num4;
                        }
                    }
                    if ((num >= ((input.Delay * speed) - input.RealRadius)) && (Math.Abs((float) (input.Speed - float.MaxValue)) > float.Epsilon))
                    {
                        float distance = (input.Delay * speed) - input.RealRadius;
                        if (((input.Type == Prediction.Position.SkillshotTypeEx.SkillshotLine) || (input.Type == Prediction.Position.SkillshotTypeEx.SkillshotCone)) && (input.From.Distance(input.Unit.ServerPosition, true) < 40000f))
                        {
                            distance = input.Delay * speed;
                        }
                        path = path.CutPath(distance);
                        float delay = 0f;
                        for (int j = 0; j < (path.Count - 1); j++)
                        {
                            Vector2 vector7 = path[j];
                            Vector2 vector8 = path[j + 1];
                            float num8 = vector7.Distance(vector8, false) / speed;
                            Vector2 vector9 = (vector8 - vector7).Normalized();
                            vector7 -= (Vector2) ((speed * delay) * vector9);
                            object[] objArray = Geometry.VectorMovementCollision(vector7, vector8, speed, input.From.To2D(), input.Speed, delay);
                            float num9 = (float) objArray[0];
                            Vector2 vector10 = (Vector2) objArray[1];
                            if ((vector10.IsValid(false) && (num9 >= delay)) && (num9 <= (delay + num8)))
                            {
                                if (vector10.Distance(vector8, true) >= 20f)
                                {
                                    Vector2 vector11 = vector10 + ((Vector2) (input.RealRadius * vector9));
                                    if (input.Type != Prediction.Position.SkillshotTypeEx.SkillshotLine)
                                    {
                                        float num10 = (input.From.To2D() - vector11).AngleBetween(vector7 - vector8);
                                        if ((num10 > 30f) && (num10 < 150f))
                                        {
                                            float angle = (float) Math.Asin((double) (input.RealRadius / vector11.Distance(input.From, false)));
                                            Vector2 vector12 = input.From.To2D() + (vector11 - input.From.To2D()).Rotated(angle);
                                            Vector2 vector13 = input.From.To2D() + (vector11 - input.From.To2D()).Rotated(-angle);
                                            vector10 = (vector12.Distance(vector10, true) < vector13.Distance(vector10, true)) ? vector12 : vector13;
                                        }
                                    }
                                    return new Prediction.Position.PredictionOutput { 
                                        Input = input,
                                        CastPosition = vector10.To3D(0),
                                        UnitPosition = vector11.To3D(0),
                                        Hitchance = Prediction.Position.HitChanceEx.High
                                    };
                                }
                                break;
                            }
                            delay += num8;
                        }
                    }
                    Vector2 vector = path.Last<Vector2>();
                    return new Prediction.Position.PredictionOutput { 
                        Input = input,
                        CastPosition = vector.To3D(0),
                        UnitPosition = vector.To3D(0),
                        Hitchance = Prediction.Position.HitChanceEx.Medium
                    };
                }

                public static Prediction.Position.PredictionOutput GetPrediction(Prediction.Position.PredictionInput input) => 
                    GetPrediction(input, true, true);

                internal static Prediction.Position.PredictionOutput GetPrediction(Obj_AI_Base unit, float delay)
                {
                    Prediction.Position.PredictionInput input = new Prediction.Position.PredictionInput {
                        Unit = unit,
                        Delay = delay
                    };
                    return GetPrediction(input);
                }

                internal static Prediction.Position.PredictionOutput GetPrediction(Obj_AI_Base unit, float delay, float radius)
                {
                    Prediction.Position.PredictionInput input = new Prediction.Position.PredictionInput {
                        Unit = unit,
                        Delay = delay,
                        Radius = radius
                    };
                    return GetPrediction(input);
                }

                internal static Prediction.Position.PredictionOutput GetPrediction(Prediction.Position.PredictionInput input, bool ft, bool checkCollision)
                {
                    Prediction.Position.PredictionOutput result = null;
                    if (!input.Unit.IsValidTarget(float.MaxValue, false, null))
                    {
                        return new Prediction.Position.PredictionOutput();
                    }
                    if (ft)
                    {
                        input.Delay += (((float) Game.Ping) / 2000f) + 0.06f;
                        if (input.Aoe)
                        {
                        }
                    }
                    if ((Math.Abs((float) (input.Range - float.MaxValue)) > float.Epsilon) && (input.Unit.Distance(input.RangeCheckFrom, true) > Math.Pow(input.Range * 1.5, 2.0)))
                    {
                        return new Prediction.Position.PredictionOutput { Input = input };
                    }
                    if (input.Unit.IsDashing())
                    {
                        result = GetDashingPrediction(input);
                    }
                    else
                    {
                        double remainingImmobileT = UnitIsImmobileUntil(input.Unit);
                        if (remainingImmobileT >= 0.0)
                        {
                            result = GetImmobilePrediction(input, remainingImmobileT);
                        }
                    }
                    if (result == null)
                    {
                        result = GetPositionOnPath(input, input.Unit.GetWaypoints(), input.Unit.MoveSpeed);
                    }
                    if (Math.Abs((float) (input.Range - float.MaxValue)) > float.Epsilon)
                    {
                        if ((result.Hitchance >= Prediction.Position.HitChanceEx.High) && (input.RangeCheckFrom.Distance(input.Unit.Position, true) > Math.Pow((double) (input.Range + ((input.RealRadius * 3f) / 4f)), 2.0)))
                        {
                            result.Hitchance = Prediction.Position.HitChanceEx.Medium;
                        }
                        if (input.RangeCheckFrom.Distance(result.UnitPosition, true) > Math.Pow((double) (input.Range + ((input.Type == Prediction.Position.SkillshotTypeEx.SkillshotCircle) ? input.RealRadius : 0f)), 2.0))
                        {
                            result.Hitchance = Prediction.Position.HitChanceEx.OutOfRange;
                        }
                        if (input.RangeCheckFrom.Distance(result.CastPosition, true) > Math.Pow((double) input.Range, 2.0))
                        {
                            if (result.Hitchance != Prediction.Position.HitChanceEx.OutOfRange)
                            {
                                result.CastPosition = input.RangeCheckFrom + ((Vector3) (input.Range * (result.UnitPosition - input.RangeCheckFrom).To2D().Normalized().To3D(0)));
                            }
                            else
                            {
                                result.Hitchance = Prediction.Position.HitChanceEx.OutOfRange;
                            }
                        }
                    }
                    if (checkCollision && input.Collision)
                    {
                        List<Vector3> positions = new List<Vector3> {
                            result.UnitPosition,
                            result.CastPosition,
                            input.Unit.Position
                        };
                        Obj_AI_Base originalUnit = input.Unit;
                        result.CollisionObjects = Prediction.Position.CollisionEx.GetCollision(positions, input);
                        result.CollisionObjects.RemoveAll(x => x.NetworkId == originalUnit.NetworkId);
                        result.Hitchance = (result.CollisionObjects.Count > 0) ? Prediction.Position.HitChanceEx.Collision : result.Hitchance;
                    }
                    if ((result.Hitchance == Prediction.Position.HitChanceEx.High) || (result.Hitchance == Prediction.Position.HitChanceEx.VeryHigh))
                    {
                        result = WayPointAnalysis(result, input);
                    }
                    return result;
                }

                internal static Prediction.Position.PredictionOutput GetPrediction(Obj_AI_Base unit, float delay, float radius, float speed)
                {
                    Prediction.Position.PredictionInput input = new Prediction.Position.PredictionInput {
                        Unit = unit,
                        Delay = delay,
                        Radius = radius,
                        Speed = speed
                    };
                    return GetPrediction(input);
                }

                internal static Prediction.Position.PredictionOutput GetPrediction(Obj_AI_Base unit, float delay, float radius, float speed, Prediction.Position.CollisionableObjectsEx[] collisionable)
                {
                    Prediction.Position.PredictionInput input = new Prediction.Position.PredictionInput {
                        Unit = unit,
                        Delay = delay,
                        Radius = radius,
                        Speed = speed,
                        CollisionObjects = collisionable
                    };
                    return GetPrediction(input);
                }

                internal static Prediction.Position.PredictionOutput GetStandardPrediction(Prediction.Position.PredictionInput input)
                {
                    float moveSpeed = input.Unit.MoveSpeed;
                    if (input.Unit.Distance(input.From, true) < 40000f)
                    {
                        moveSpeed /= 1.5f;
                    }
                    return GetPositionOnPath(input, input.Unit.GetWaypoints(), moveSpeed);
                }

                internal static void Initialize()
                {
                }

                internal static double UnitIsImmobileUntil(Obj_AI_Base unit) => 
                    ((from buff in unit.Buffs
                        where (buff.IsActive && (Game.Time <= buff.EndTime)) && ((((buff.Type == BuffType.Charm) || (buff.Type == BuffType.Knockup)) || ((buff.Type == BuffType.Stun) || (buff.Type == BuffType.Suppression))) || (buff.Type == BuffType.Snare))
                        select buff).Aggregate<BuffInstance, double>(0.0, (current, buff) => Math.Max(current, (double) buff.EndTime)) - Game.Time);

                internal static Prediction.Position.PredictionOutput WayPointAnalysis(Prediction.Position.PredictionOutput result, Prediction.Position.PredictionInput input)
                {
                    if (!(input.Unit is AIHeroClient) || (input.Radius == 1f))
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                        return result;
                    }
                    if (((UnitTracker.GetSpecialSpellEndTime(input.Unit) > 100.0) || input.Unit.HasBuff("Recall")) || ((UnitTracker.GetLastStopMoveTime(input.Unit) < 100.0) && input.Unit.HasBuffOfType(BuffType.Snare)))
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                        result.CastPosition = input.Unit.Position;
                        return result;
                    }
                    if (UnitTracker.GetLastVisableTime(input.Unit) < 100.0)
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.Medium;
                        return result;
                    }
                    Vector3 vector = input.Unit.GetWaypoints().Last<Vector2>().To3D(0);
                    float num = vector.Distance(input.Unit.ServerPosition, false);
                    float num2 = input.From.Distance(input.Unit.ServerPosition, false);
                    float num3 = vector.Distance(input.From, false);
                    Vector2 vector2 = vector.To2D() - input.Unit.Position.To2D();
                    Vector2 vector3 = input.From.To2D() - input.Unit.Position.To2D();
                    float num4 = vector2.AngleBetween(vector3);
                    float num5 = num2 / input.Speed;
                    if (Math.Abs((float) (input.Speed - float.MaxValue)) < float.Epsilon)
                    {
                        num5 = 0f;
                    }
                    float num6 = num5 + input.Delay;
                    float num7 = input.Unit.MoveSpeed * num6;
                    float num8 = num7 * 0.35f;
                    float num9 = 1000f;
                    if (input.Type == Prediction.Position.SkillshotTypeEx.SkillshotCircle)
                    {
                        num8 -= input.Radius / 2f;
                    }
                    if ((num3 <= num2) && (num2 > (input.Range - num8)))
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.Medium;
                        return result;
                    }
                    if (num > 0f)
                    {
                        if (((num4 < 20f) || (num4 > 160f)) || ((num4 > 130f) && (num > 400f)))
                        {
                            result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                            return result;
                        }
                        List<Vector2> list2 = new List<Vector2>();
                        if (list2.Count > 2)
                        {
                            bool flag10 = true;
                            foreach (Vector2 vector4 in list2)
                            {
                                if (input.Unit.Position.Distance(vector4, false) > vector.Distance(vector4, false))
                                {
                                    flag10 = false;
                                }
                            }
                            if (flag10)
                            {
                                result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                                return result;
                            }
                        }
                        else if ((UnitTracker.GetLastNewPathTime(input.Unit) > 250.0) && (input.Delay < 0.3))
                        {
                            result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                            return result;
                        }
                    }
                    if ((num > 0f) && (num < 100f))
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.Medium;
                        return result;
                    }
                    if (input.Unit.GetWaypoints().Count == 1)
                    {
                        if ((UnitTracker.GetLastAutoAttackTime(input.Unit) < 0.1) && (num6 < 0.7))
                        {
                            result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                            return result;
                        }
                        if (input.Unit.Spellbook.IsAutoAttacking)
                        {
                            result.Hitchance = Prediction.Position.HitChanceEx.High;
                            return result;
                        }
                        if (UnitTracker.GetLastStopMoveTime(input.Unit) < 800.0)
                        {
                            result.Hitchance = Prediction.Position.HitChanceEx.High;
                            return result;
                        }
                        result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                        return result;
                    }
                    if (UnitTracker.SpamSamePlace(input.Unit))
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                        return result;
                    }
                    if (num2 < 250f)
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                        return result;
                    }
                    if (input.Unit.MoveSpeed < 250f)
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                        return result;
                    }
                    if (num3 < 250f)
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                        return result;
                    }
                    if (num > num9)
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                        return result;
                    }
                    if ((input.Unit.HealthPercent < 20f) || (ObjectManager.Player.HealthPercent < 20f))
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                        return result;
                    }
                    if ((input.Type == Prediction.Position.SkillshotTypeEx.SkillshotCircle) && ((UnitTracker.GetLastNewPathTime(input.Unit) < 100.0) && (num > num8)))
                    {
                        result.Hitchance = Prediction.Position.HitChanceEx.VeryHigh;
                        return result;
                    }
                    return result;
                }

                [Serializable, CompilerGenerated]
                private sealed class <>c
                {
                    public static readonly Prediction.Position.PredictionEx.<>c <>9 = new Prediction.Position.PredictionEx.<>c();
                    public static Func<BuffInstance, bool> <>9__16_0;
                    public static Func<double, BuffInstance, double> <>9__16_1;

                    internal bool <UnitIsImmobileUntil>b__16_0(BuffInstance buff) => 
                        ((buff.IsActive && (Game.Time <= buff.EndTime)) && ((((buff.Type == BuffType.Charm) || (buff.Type == BuffType.Knockup)) || ((buff.Type == BuffType.Stun) || (buff.Type == BuffType.Suppression))) || (buff.Type == BuffType.Snare)));

                    internal double <UnitIsImmobileUntil>b__16_1(double current, BuffInstance buff) => 
                        Math.Max(current, (double) buff.EndTime);
                }

                internal class PathInfo
                {
                    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private Vector2 <Position>k__BackingField;
                    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private float <Time>k__BackingField;

                    public Vector2 Position { get; set; }

                    public float Time { get; set; }
                }

                internal class Spells
                {
                    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private double <duration>k__BackingField;
                    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private string <name>k__BackingField;

                    public double duration { get; set; }

                    public string name { get; set; }
                }

                internal static class UnitTracker
                {
                    private static readonly List<AIHeroClient> Champion = new List<AIHeroClient>();
                    private static readonly List<Prediction.Position.PredictionEx.Spells> spells = new List<Prediction.Position.PredictionEx.Spells>();
                    public static List<Prediction.Position.PredictionEx.UnitTrackerInfo> UnitTrackerInfoList = new List<Prediction.Position.PredictionEx.UnitTrackerInfo>();

                    public static double GetLastAutoAttackTime(Obj_AI_Base unit)
                    {
                        Prediction.Position.PredictionEx.UnitTrackerInfo info = UnitTrackerInfoList.Find(x => x.NetworkId == unit.NetworkId);
                        return (double) (Core.GameTickCount - info.AaTick);
                    }

                    public static double GetLastNewPathTime(Obj_AI_Base unit)
                    {
                        Prediction.Position.PredictionEx.UnitTrackerInfo info = UnitTrackerInfoList.Find(x => x.NetworkId == unit.NetworkId);
                        return (double) (Core.GameTickCount - info.NewPathTick);
                    }

                    public static double GetLastStopMoveTime(Obj_AI_Base unit)
                    {
                        Prediction.Position.PredictionEx.UnitTrackerInfo info = UnitTrackerInfoList.Find(x => x.NetworkId == unit.NetworkId);
                        return (double) (Core.GameTickCount - info.StopMoveTick);
                    }

                    public static double GetLastVisableTime(Obj_AI_Base unit)
                    {
                        Prediction.Position.PredictionEx.UnitTrackerInfo info = UnitTrackerInfoList.Find(x => x.NetworkId == unit.NetworkId);
                        return (double) (Core.GameTickCount - info.LastInvisableTick);
                    }

                    public static List<Vector2> GetPathWayCalc(Obj_AI_Base unit) => 
                        new List<Vector2> { unit.ServerPosition.To2D() };

                    public static double GetSpecialSpellEndTime(Obj_AI_Base unit) => 
                        ((double) (UnitTrackerInfoList.Find(x => x.NetworkId == unit.NetworkId).SpecialSpellFinishTick - Core.GameTickCount));

                    internal static void Init()
                    {
                        Prediction.Position.PredictionEx.Spells item = new Prediction.Position.PredictionEx.Spells {
                            name = "katarinar",
                            duration = 1.0
                        };
                        spells.Add(item);
                        Prediction.Position.PredictionEx.Spells spells2 = new Prediction.Position.PredictionEx.Spells {
                            name = "drain",
                            duration = 1.0
                        };
                        spells.Add(spells2);
                        Prediction.Position.PredictionEx.Spells spells3 = new Prediction.Position.PredictionEx.Spells {
                            name = "crowstorm",
                            duration = 1.0
                        };
                        spells.Add(spells3);
                        Prediction.Position.PredictionEx.Spells spells4 = new Prediction.Position.PredictionEx.Spells {
                            name = "consume",
                            duration = 0.5
                        };
                        spells.Add(spells4);
                        Prediction.Position.PredictionEx.Spells spells5 = new Prediction.Position.PredictionEx.Spells {
                            name = "absolutezero",
                            duration = 1.0
                        };
                        spells.Add(spells5);
                        Prediction.Position.PredictionEx.Spells spells6 = new Prediction.Position.PredictionEx.Spells {
                            name = "staticfield",
                            duration = 0.5
                        };
                        spells.Add(spells6);
                        Prediction.Position.PredictionEx.Spells spells7 = new Prediction.Position.PredictionEx.Spells {
                            name = "cassiopeiapetrifyinggaze",
                            duration = 0.5
                        };
                        spells.Add(spells7);
                        Prediction.Position.PredictionEx.Spells spells8 = new Prediction.Position.PredictionEx.Spells {
                            name = "ezrealtrueshotbarrage",
                            duration = 1.0
                        };
                        spells.Add(spells8);
                        Prediction.Position.PredictionEx.Spells spells9 = new Prediction.Position.PredictionEx.Spells {
                            name = "galioidolofdurand",
                            duration = 1.0
                        };
                        spells.Add(spells9);
                        Prediction.Position.PredictionEx.Spells spells10 = new Prediction.Position.PredictionEx.Spells {
                            name = "luxmalicecannon",
                            duration = 1.0
                        };
                        spells.Add(spells10);
                        Prediction.Position.PredictionEx.Spells spells11 = new Prediction.Position.PredictionEx.Spells {
                            name = "reapthewhirlwind",
                            duration = 1.0
                        };
                        spells.Add(spells11);
                        Prediction.Position.PredictionEx.Spells spells12 = new Prediction.Position.PredictionEx.Spells {
                            name = "jinxw",
                            duration = 0.6
                        };
                        spells.Add(spells12);
                        Prediction.Position.PredictionEx.Spells spells13 = new Prediction.Position.PredictionEx.Spells {
                            name = "jinxr",
                            duration = 0.6
                        };
                        spells.Add(spells13);
                        Prediction.Position.PredictionEx.Spells spells14 = new Prediction.Position.PredictionEx.Spells {
                            name = "missfortunebullettime",
                            duration = 1.0
                        };
                        spells.Add(spells14);
                        Prediction.Position.PredictionEx.Spells spells15 = new Prediction.Position.PredictionEx.Spells {
                            name = "shenstandunited",
                            duration = 1.0
                        };
                        spells.Add(spells15);
                        Prediction.Position.PredictionEx.Spells spells16 = new Prediction.Position.PredictionEx.Spells {
                            name = "threshe",
                            duration = 0.4
                        };
                        spells.Add(spells16);
                        Prediction.Position.PredictionEx.Spells spells17 = new Prediction.Position.PredictionEx.Spells {
                            name = "threshrpenta",
                            duration = 0.75
                        };
                        spells.Add(spells17);
                        Prediction.Position.PredictionEx.Spells spells18 = new Prediction.Position.PredictionEx.Spells {
                            name = "threshq",
                            duration = 0.75
                        };
                        spells.Add(spells18);
                        Prediction.Position.PredictionEx.Spells spells19 = new Prediction.Position.PredictionEx.Spells {
                            name = "infiniteduress",
                            duration = 1.0
                        };
                        spells.Add(spells19);
                        Prediction.Position.PredictionEx.Spells spells20 = new Prediction.Position.PredictionEx.Spells {
                            name = "meditate",
                            duration = 1.0
                        };
                        spells.Add(spells20);
                        Prediction.Position.PredictionEx.Spells spells21 = new Prediction.Position.PredictionEx.Spells {
                            name = "alzaharnethergrasp",
                            duration = 1.0
                        };
                        spells.Add(spells21);
                        Prediction.Position.PredictionEx.Spells spells22 = new Prediction.Position.PredictionEx.Spells {
                            name = "lucianq",
                            duration = 0.5
                        };
                        spells.Add(spells22);
                        Prediction.Position.PredictionEx.Spells spells23 = new Prediction.Position.PredictionEx.Spells {
                            name = "caitlynpiltoverpeacemaker",
                            duration = 0.5
                        };
                        spells.Add(spells23);
                        Prediction.Position.PredictionEx.Spells spells24 = new Prediction.Position.PredictionEx.Spells {
                            name = "velkozr",
                            duration = 0.5
                        };
                        spells.Add(spells24);
                        Prediction.Position.PredictionEx.Spells spells25 = new Prediction.Position.PredictionEx.Spells {
                            name = "jhinr",
                            duration = 2.0
                        };
                        spells.Add(spells25);
                        foreach (AIHeroClient client in EntityManager.Heroes.AllHeroes)
                        {
                            Champion.Add(client);
                            Prediction.Position.PredictionEx.UnitTrackerInfo info1 = new Prediction.Position.PredictionEx.UnitTrackerInfo {
                                NetworkId = client.NetworkId,
                                AaTick = Core.GameTickCount,
                                StopMoveTick = Core.GameTickCount,
                                NewPathTick = Core.GameTickCount,
                                SpecialSpellFinishTick = Core.GameTickCount,
                                LastInvisableTick = Core.GameTickCount
                            };
                            UnitTrackerInfoList.Add(info1);
                        }
                        Obj_AI_Base.OnProcessSpellCast += new Obj_AI_ProcessSpellCast(Prediction.Position.PredictionEx.UnitTracker.Obj_AI_Base_OnProcessSpellCast);
                        Obj_AI_Base.OnNewPath += new Obj_AI_BaseNewPath(Prediction.Position.PredictionEx.UnitTracker.Obj_AI_Base_OnNewPath);
                    }

                    private static void Obj_AI_Base_OnNewPath(Obj_AI_Base sender, GameObjectNewPathEventArgs args)
                    {
                        if (sender is AIHeroClient)
                        {
                            Prediction.Position.PredictionEx.UnitTrackerInfo info = UnitTrackerInfoList.Find(x => x.NetworkId == sender.NetworkId);
                            if (args.Path.Count<Vector3>() == 1)
                            {
                                info.StopMoveTick = Core.GameTickCount;
                            }
                            info.NewPathTick = Core.GameTickCount;
                            Prediction.Position.PredictionEx.PathInfo item = new Prediction.Position.PredictionEx.PathInfo {
                                Position = args.Path.Last<Vector3>().To2D(),
                                Time = Core.GameTickCount
                            };
                            info.PathBank.Add(item);
                            if (info.PathBank.Count > 3)
                            {
                                info.PathBank.RemoveAt(0);
                            }
                        }
                    }

                    private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
                    {
                        if (sender is AIHeroClient)
                        {
                            if (args.SData.IsAutoAttack())
                            {
                                UnitTrackerInfoList.Find(x => x.NetworkId == sender.NetworkId).AaTick = Core.GameTickCount;
                            }
                            else
                            {
                                Prediction.Position.PredictionEx.Spells spells = Prediction.Position.PredictionEx.UnitTracker.spells.Find(x => args.SData.Name.ToLower() == x.name.ToLower());
                                if (spells > null)
                                {
                                    UnitTrackerInfoList.Find(x => x.NetworkId == sender.NetworkId).SpecialSpellFinishTick = Core.GameTickCount + ((int) (spells.duration * 1000.0));
                                }
                                else if ((sender.Spellbook.IsAutoAttacking || sender.HasBuffOfType(BuffType.Snare)) || !sender.CanMove)
                                {
                                    UnitTrackerInfoList.Find(x => x.NetworkId == sender.NetworkId).SpecialSpellFinishTick = Core.GameTickCount + 100;
                                }
                            }
                        }
                    }

                    public static bool SpamSamePlace(Obj_AI_Base unit)
                    {
                        Prediction.Position.PredictionEx.UnitTrackerInfo info = UnitTrackerInfoList.Find(x => x.NetworkId == unit.NetworkId);
                        if ((info.PathBank.Count >= 3) && (((info.PathBank[2].Time - info.PathBank[1].Time) < 180f) && ((Core.GameTickCount - info.PathBank[2].Time) < 90f)))
                        {
                            Vector2 position = info.PathBank[1].Position;
                            Vector2 vector2 = info.PathBank[2].Position;
                            Vector2 vector3 = unit.Position.To2D();
                            double d = Math.Pow((double) (vector2.X - vector3.X), 2.0) + Math.Pow((double) (vector2.Y - vector3.Y), 2.0);
                            double num2 = Math.Pow((double) (vector3.X - position.X), 2.0) + Math.Pow((double) (vector3.Y - position.Y), 2.0);
                            double num3 = Math.Pow((double) (vector2.X - position.X), 2.0) + Math.Pow((double) (vector2.Y - position.Y), 2.0);
                            return ((info.PathBank[1].Position.Distance(info.PathBank[2].Position, false) < 50f) || (((Math.Cos(((d + num2) - num3) / ((2.0 * Math.Sqrt(d)) * Math.Sqrt(num2))) * 180.0) / 3.1415926535897931) < 31.0));
                        }
                        return false;
                    }
                }

                internal class UnitTrackerInfo
                {
                    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private int <AaTick>k__BackingField;
                    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private int <LastInvisableTick>k__BackingField;
                    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private int <NetworkId>k__BackingField;
                    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private int <NewPathTick>k__BackingField;
                    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private int <SpecialSpellFinishTick>k__BackingField;
                    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    private int <StopMoveTick>k__BackingField;
                    public List<Prediction.Position.PredictionEx.PathInfo> PathBank = new List<Prediction.Position.PredictionEx.PathInfo>();

                    public int AaTick { get; set; }

                    public int LastInvisableTick { get; set; }

                    public int NetworkId { get; set; }

                    public int NewPathTick { get; set; }

                    public int SpecialSpellFinishTick { get; set; }

                    public int StopMoveTick { get; set; }
                }
            }

            internal class PredictionInput
            {
                private Vector3 _from;
                private Vector3 _rangeCheckFrom;
                internal bool Aoe = false;
                internal bool Collision = false;
                internal Prediction.Position.CollisionableObjectsEx[] CollisionObjects;
                internal float Delay;
                internal float Radius;
                internal float Range;
                internal float Speed;
                internal Prediction.Position.SkillshotTypeEx Type;
                internal Obj_AI_Base Unit;
                internal bool UseBoundingRadius;

                public PredictionInput()
                {
                    Prediction.Position.CollisionableObjectsEx[] exArray1 = new Prediction.Position.CollisionableObjectsEx[2];
                    exArray1[1] = Prediction.Position.CollisionableObjectsEx.YasuoWall;
                    this.CollisionObjects = exArray1;
                    this.Radius = 1f;
                    this.Range = float.MaxValue;
                    this.Speed = float.MaxValue;
                    this.Type = Prediction.Position.SkillshotTypeEx.SkillshotLine;
                    this.Unit = ObjectManager.Player;
                    this.UseBoundingRadius = true;
                }

                internal Vector3 From
                {
                    get => 
                        (this._from.To2D().IsValid(false) ? this._from : ObjectManager.Player.ServerPosition);
                    set
                    {
                        this._from = value;
                    }
                }

                internal Vector3 RangeCheckFrom
                {
                    get => 
                        (this._rangeCheckFrom.To2D().IsValid(false) ? this._rangeCheckFrom : (this.From.To2D().IsValid(false) ? this.From : ObjectManager.Player.ServerPosition));
                    set
                    {
                        this._rangeCheckFrom = value;
                    }
                }

                internal float RealRadius =>
                    (this.UseBoundingRadius ? (this.Radius + this.Unit.BoundingRadius) : this.Radius);
            }

            internal class PredictionOutput
            {
                internal int _aoeTargetsHitCount;
                private Vector3 _castPosition;
                private Vector3 _unitPosition;
                internal List<AIHeroClient> AoeTargetsHit = new List<AIHeroClient>();
                internal List<Obj_AI_Base> CollisionObjects = new List<Obj_AI_Base>();
                internal Prediction.Position.HitChanceEx Hitchance = Prediction.Position.HitChanceEx.Impossible;
                internal Prediction.Position.PredictionInput Input;

                internal int AoeTargetsHitCount =>
                    Math.Max(this._aoeTargetsHitCount, this.AoeTargetsHit.Count);

                internal Vector3 CastPosition
                {
                    get => 
                        ((this._castPosition.IsValid(false) && this._castPosition.To2D().IsValid(false)) ? this._castPosition.SetZ(null) : this.Input.Unit.ServerPosition);
                    set
                    {
                        this._castPosition = value;
                    }
                }

                internal Vector3 UnitPosition
                {
                    get => 
                        (this._unitPosition.To2D().IsValid(false) ? this._unitPosition.SetZ(null) : this.Input.Unit.ServerPosition);
                    set
                    {
                        this._unitPosition = value;
                    }
                }
            }

            internal enum SkillshotTypeEx
            {
                SkillshotLine,
                SkillshotCircle,
                SkillshotCone
            }

            internal class StoredPath
            {
                internal List<Vector2> Path;
                internal int Tick;

                internal Vector2 EndPoint =>
                    this.Path.LastOrDefault<Vector2>();

                internal Vector2 StartPoint =>
                    this.Path.FirstOrDefault<Vector2>();

                internal double Time =>
                    (((double) (Core.GameTickCount - this.Tick)) / 1000.0);

                internal int WaypointCount =>
                    this.Path.Count;
            }
        }
    }
}

