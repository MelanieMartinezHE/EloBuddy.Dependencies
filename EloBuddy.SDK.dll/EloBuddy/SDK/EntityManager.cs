namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Constants;
    using EloBuddy.SDK.Utils;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class EntityManager
    {
        internal static void Initialize()
        {
            Heroes.Initialize();
            Turrets.Initialize();
        }

        public static List<Obj_AI_Base> Allies
        {
            get
            {
                List<Obj_AI_Base> list = new List<Obj_AI_Base>();
                list.AddRange(Heroes.Allies);
                list.AddRange(MinionsAndMonsters.AlliedMinions);
                return list;
            }
        }

        public static List<Obj_AI_Base> Enemies
        {
            get
            {
                List<Obj_AI_Base> list = new List<Obj_AI_Base>();
                list.AddRange(Heroes.Enemies);
                list.AddRange(MinionsAndMonsters.EnemyMinions);
                list.AddRange(MinionsAndMonsters.Monsters);
                return list;
            }
        }

        public static class Heroes
        {
            internal static List<AIHeroClient> _allHeroes = new List<AIHeroClient>();
            internal static List<AIHeroClient> _allies = new List<AIHeroClient>();
            internal static List<AIHeroClient> _enemies = new List<AIHeroClient>();
            internal static bool ContainsKalista;
            internal static bool ContainsKayle;
            internal static bool ContainsKindred;
            internal static bool ContainsZilean;

            internal static void Initialize()
            {
                _allHeroes = ObjectManager.Get<AIHeroClient>().ToList<AIHeroClient>();
                _allies = new List<AIHeroClient>();
                _enemies = new List<AIHeroClient>();
                if (!Bootstrap.IsSpectatorMode)
                {
                    _allies = AllHeroes.FindAll(o => o.IsAlly);
                    _enemies = AllHeroes.FindAll(o => o.IsEnemy);
                    ContainsKalista = AllHeroes.Any<AIHeroClient>(client => client.Hero == Champion.Kalista);
                    ContainsKayle = AllHeroes.Any<AIHeroClient>(client => client.Hero == Champion.Kayle);
                    ContainsKindred = AllHeroes.Any<AIHeroClient>(client => client.Hero == Champion.Kindred);
                    ContainsZilean = AllHeroes.Any<AIHeroClient>(client => client.Hero == Champion.Zilean);
                    object[] args = new object[] { _allies.Count, _enemies.Count };
                    Logger.Info("EntityManager.Heroes: Allies ({0}) Enemies ({1})", args);
                    GameObject.OnCreate += delegate (GameObject sender, EventArgs args) {
                        AIHeroClient item = sender as AIHeroClient;
                        if ((item != null) && (item.Hero == Champion.PracticeTool_TargetDummy))
                        {
                            if (!_allHeroes.Contains(item))
                            {
                                _allHeroes.Add(item);
                            }
                            if (item.IsEnemy && !_enemies.Contains(item))
                            {
                                _enemies.Add(item);
                            }
                            if (item.IsAlly && !_allies.Contains(item))
                            {
                                _allies.Add(item);
                            }
                        }
                    };
                    GameObject.OnDelete += delegate (GameObject sender, EventArgs args) {
                        AIHeroClient item = sender as AIHeroClient;
                        if ((item != null) && (item.Hero == Champion.PracticeTool_TargetDummy))
                        {
                            if (_allHeroes.Contains(item))
                            {
                                _allHeroes.Remove(item);
                            }
                            if (item.IsEnemy && _enemies.Contains(item))
                            {
                                _enemies.Remove(item);
                            }
                            if (item.IsAlly && _allies.Contains(item))
                            {
                                _allies.Remove(item);
                            }
                        }
                    };
                }
            }

            public static List<AIHeroClient> AllHeroes =>
                new List<AIHeroClient>(_allHeroes);

            public static List<AIHeroClient> Allies =>
                new List<AIHeroClient>(_allies);

            public static List<AIHeroClient> Enemies =>
                new List<AIHeroClient>(_enemies);

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly EntityManager.Heroes.<>c <>9 = new EntityManager.Heroes.<>c();
                public static Predicate<AIHeroClient> <>9__13_0;
                public static Predicate<AIHeroClient> <>9__13_1;
                public static Func<AIHeroClient, bool> <>9__13_2;
                public static Func<AIHeroClient, bool> <>9__13_3;
                public static Func<AIHeroClient, bool> <>9__13_4;
                public static Func<AIHeroClient, bool> <>9__13_5;
                public static GameObjectCreate <>9__13_6;
                public static GameObjectDelete <>9__13_7;

                internal bool <Initialize>b__13_0(AIHeroClient o) => 
                    o.IsAlly;

                internal bool <Initialize>b__13_1(AIHeroClient o) => 
                    o.IsEnemy;

                internal bool <Initialize>b__13_2(AIHeroClient client) => 
                    (client.Hero == Champion.Kalista);

                internal bool <Initialize>b__13_3(AIHeroClient client) => 
                    (client.Hero == Champion.Kayle);

                internal bool <Initialize>b__13_4(AIHeroClient client) => 
                    (client.Hero == Champion.Kindred);

                internal bool <Initialize>b__13_5(AIHeroClient client) => 
                    (client.Hero == Champion.Zilean);

                internal void <Initialize>b__13_6(GameObject sender, EventArgs args)
                {
                    AIHeroClient item = sender as AIHeroClient;
                    if ((item != null) && (item.Hero == Champion.PracticeTool_TargetDummy))
                    {
                        if (!EntityManager.Heroes._allHeroes.Contains(item))
                        {
                            EntityManager.Heroes._allHeroes.Add(item);
                        }
                        if (item.IsEnemy && !EntityManager.Heroes._enemies.Contains(item))
                        {
                            EntityManager.Heroes._enemies.Add(item);
                        }
                        if (item.IsAlly && !EntityManager.Heroes._allies.Contains(item))
                        {
                            EntityManager.Heroes._allies.Add(item);
                        }
                    }
                }

                internal void <Initialize>b__13_7(GameObject sender, EventArgs args)
                {
                    AIHeroClient item = sender as AIHeroClient;
                    if ((item != null) && (item.Hero == Champion.PracticeTool_TargetDummy))
                    {
                        if (EntityManager.Heroes._allHeroes.Contains(item))
                        {
                            EntityManager.Heroes._allHeroes.Remove(item);
                        }
                        if (item.IsEnemy && EntityManager.Heroes._enemies.Contains(item))
                        {
                            EntityManager.Heroes._enemies.Remove(item);
                        }
                        if (item.IsAlly && EntityManager.Heroes._allies.Contains(item))
                        {
                            EntityManager.Heroes._allies.Remove(item);
                        }
                    }
                }
            }
        }

        public static class MinionsAndMonsters
        {
            public static IEnumerable<Obj_AI_Minion> Get(EntityType type, EntityManager.UnitTeam minionTeam = 2, Vector3? sourcePosition = new Vector3?(), float radius = 3.402823E+38f, bool addBoundingRadius = true)
            {
                IEnumerable<Obj_AI_Minion> minions;
                switch (type)
                {
                    case EntityType.Minion:
                        switch (minionTeam)
                        {
                            case EntityManager.UnitTeam.Ally:
                                minions = AlliedMinions;
                                goto Label_0061;

                            case EntityManager.UnitTeam.Enemy:
                                minions = EnemyMinions;
                                goto Label_0061;
                        }
                        minions = Minions;
                        break;

                    case EntityType.Monster:
                        minions = Monsters;
                        break;

                    default:
                        minions = CombinedAttackable;
                        break;
                }
            Label_0061:
                if (minions == null)
                {
                    return new List<Obj_AI_Minion>();
                }
                return (from o in minions.Where<Obj_AI_Minion>(delegate (Obj_AI_Minion o) {
                    Vector3? nullable1 = sourcePosition;
                    return (nullable1.HasValue ? nullable1.GetValueOrDefault() : Player.Instance.ServerPosition).IsInRange((Obj_AI_Base) o, radius + (addBoundingRadius ? o.BoundingRadius : 0f));
                })
                    orderby o.MaxHealth descending
                    select o).ToList<Obj_AI_Minion>();
            }

            [Obsolete("Use the method in the spell to get position or cast it.")]
            public static FarmLocation GetCircularFarmLocation(IEnumerable<Obj_AI_Minion> entities, float width, int range, Vector2? sourcePosition = new Vector2?())
            {
                Obj_AI_Minion[] minionArray = entities.ToArray<Obj_AI_Minion>();
                switch (minionArray.Length)
                {
                    case 0:
                        return new FarmLocation();

                    case 1:
                        return new FarmLocation { 
                            CastPosition = minionArray[0].ServerPosition,
                            HitNumber = 1
                        };
                }
                Vector2? nullable = sourcePosition;
                Vector2 startPos = nullable.HasValue ? nullable.GetValueOrDefault() : Player.Instance.ServerPosition.To2D();
                int num = 0;
                Vector2 zero = Vector2.Zero;
                Vector2[] source = (from o in minionArray
                    select o.ServerPosition.To2D() into o
                    where o.IsInRange(startPos, (float) range)
                    select o).ToArray<Vector2>();
                Vector2[] vectorArray2 = source;
                for (int i = 0; i < vectorArray2.Length; i++)
                {
                    Vector2 pos = vectorArray2[i];
                    int num4 = source.Count<Vector2>(o => o.IsInRange(pos, width / 2f));
                    if (num4 >= num)
                    {
                        zero = pos;
                        num = num4;
                    }
                }
                return new FarmLocation { 
                    CastPosition = zero.To3DWorld(),
                    HitNumber = num
                };
            }

            [Obsolete("Use the method in the spell to get position or cast it.")]
            public static FarmLocation GetCircularFarmLocation(IEnumerable<Obj_AI_Minion> entities, float width, int range, int delay, float speed, Vector2? sourcePosition = new Vector2?())
            {
                Obj_AI_Base[] baseArray = entities.Cast<Obj_AI_Base>().ToArray<Obj_AI_Base>();
                Vector3? source = null;
                if (sourcePosition.HasValue)
                {
                    source = new Vector3?(sourcePosition.Value.To3DWorld());
                }
                Vector2? nullable = sourcePosition;
                Vector2 startPos = nullable.HasValue ? nullable.GetValueOrDefault() : Player.Instance.ServerPosition.To2D();
                int num = 0;
                Vector2 zero = Vector2.Zero;
                PredictionResult[] resultArray = (from o in baseArray
                    select Prediction.Position.PredictCircularMissile(o, (float) range, (int) (width / 2f), delay, speed, source, false) into o
                    where o.UnitPosition.IsInRange(startPos, range + (width / 2f))
                    select o).ToArray<PredictionResult>();
                PredictionResult[] resultArray2 = resultArray;
                for (int i = 0; i < resultArray2.Length; i++)
                {
                    PredictionResult pos = resultArray2[i];
                    int num3 = resultArray.Count<PredictionResult>(o => o.UnitPosition.IsInRange(pos.UnitPosition, width / 2f));
                    if (num3 >= num)
                    {
                        zero = pos.UnitPosition.To2D();
                        num = num3;
                    }
                }
                return new FarmLocation { 
                    CastPosition = zero.To3DWorld(),
                    HitNumber = num
                };
            }

            public static IEnumerable<Obj_AI_Minion> GetJungleMonsters(Vector3? sourcePosition = new Vector3?(), float radius = 3.402823E+38f, bool addBoundingRadius = true) => 
                Get(EntityType.Monster, EntityManager.UnitTeam.Both, sourcePosition, radius, addBoundingRadius);

            public static IEnumerable<Obj_AI_Minion> GetLaneMinions(EntityManager.UnitTeam minionTeam = 2, Vector3? sourcePosition = new Vector3?(), float radius = 3.402823E+38f, bool addBoundingRadius = true) => 
                Get(EntityType.Minion, minionTeam, sourcePosition, radius, addBoundingRadius);

            [Obsolete("Use the method in the spell to get position or cast it.")]
            public static FarmLocation GetLineFarmLocation(IEnumerable<Obj_AI_Minion> entities, float width, int range, Vector2? sourcePosition = new Vector2?())
            {
                Func<Vector2, bool> <>9__3;
                Obj_AI_Minion[] source = entities.ToArray<Obj_AI_Minion>();
                switch (source.Length)
                {
                    case 0:
                        return new FarmLocation();

                    case 1:
                        return new FarmLocation { 
                            CastPosition = source[0].ServerPosition,
                            HitNumber = 1
                        };
                }
                List<Vector2> list = new List<Vector2>(from o in source select o.ServerPosition.To2D());
                Obj_AI_Minion[] minionArray2 = source;
                for (int i = 0; i < minionArray2.Length; i++)
                {
                    Obj_AI_Minion target = minionArray2[i];
                    list.AddRange(from t in source
                        where t.NetworkId != target.NetworkId
                        select (Vector2) ((t.ServerPosition.To2D() + target.ServerPosition.To2D()) / 2f));
                }
                Vector2? nullable = sourcePosition;
                Vector2 startPos = nullable.HasValue ? nullable.GetValueOrDefault() : Player.Instance.ServerPosition.To2D();
                int num = 0;
                Vector2 zero = Vector2.Zero;
                foreach (Vector2 vector2 in list.Where<Vector2>((Func<Vector2, bool>) (<>9__3 ?? (<>9__3 = o => o.IsInRange(startPos, (float) range)))))
                {
                    Vector2 endPos = startPos + ((Vector2) (range * (vector2 - startPos).Normalized()));
                    int num4 = source.Count<Obj_AI_Minion>(o => o.ServerPosition.To2D().Distance(startPos, endPos, true, true) <= (width * width));
                    if (num4 >= num)
                    {
                        zero = endPos;
                        num = num4;
                    }
                }
                return new FarmLocation { 
                    CastPosition = zero.To3DWorld(),
                    HitNumber = num
                };
            }

            public static IEnumerable<Obj_AI_Minion> AlliedMinions =>
                (from o in Minions
                    where o.IsAlly
                    select o).ToArray<Obj_AI_Minion>();

            public static IEnumerable<Obj_AI_Minion> Combined =>
                Monsters.Concat<Obj_AI_Minion>(Minions);

            public static IEnumerable<Obj_AI_Minion> CombinedAttackable =>
                Monsters.Concat<Obj_AI_Minion>(EnemyMinions);

            public static IEnumerable<Obj_AI_Minion> EnemyMinions =>
                (from o in Minions
                    where o.IsEnemy
                    select o).ToArray<Obj_AI_Minion>();

            public static IEnumerable<Obj_AI_Minion> JunglePlants =>
                (from o in ObjectManager.Get<Obj_AI_Minion>()
                    where (o.IsValidTarget(null, false, null) && o.MaxHealth.Equals((float) 1f)) && o.BaseSkinName.StartsWith("SRU_Plant_")
                    select o).ToArray<Obj_AI_Minion>();

            public static IEnumerable<Obj_AI_Minion> Minions =>
                (from o in ObjectManager.Get<Obj_AI_Minion>()
                    where ((o.IsValidTarget(null, false, null) && o.IsMinion) && ((o.Team == GameObjectTeam.Chaos) || (o.Team == GameObjectTeam.Order))) && (o.MaxHealth > 6f)
                    select o).ToArray<Obj_AI_Minion>();

            public static IEnumerable<Obj_AI_Minion> Monsters =>
                (from o in ObjectManager.Get<Obj_AI_Minion>()
                    where (o.IsValidTarget(null, false, null) && o.IsMonster) && (o.MaxHealth > 3f)
                    select o).ToArray<Obj_AI_Minion>();

            public static IEnumerable<Obj_AI_Minion> OtherAllyMinions =>
                (from o in OtherMinions
                    where o.IsAlly
                    select o).ToArray<Obj_AI_Minion>();

            public static IEnumerable<Obj_AI_Minion> OtherEnemyMinions =>
                (from o in OtherMinions
                    where o.IsEnemy
                    select o).ToArray<Obj_AI_Minion>();

            public static IEnumerable<Obj_AI_Minion> OtherMinions =>
                (from o in ObjectManager.Get<Obj_AI_Minion>()
                    where o.IsValidTarget(null, false, null) && ((((o.IsMinion && !o.IsMonster) && ((o.MaxHealth <= 6f) && !ObjectNames.InvalidTargets.Contains(o.BaseSkinName))) || (!o.IsMinion && !o.IsMonster)) || ((((o.IsMonster && !o.IsMinion) && ((o.MaxHealth <= 3f) && (o.Health > 0f))) && o.HasBuff("GangplankEBarrelActive")) && o.GetBuff("GangplankEBarrelActive").Caster.IsEnemy))
                    select o).ToArray<Obj_AI_Minion>();

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly EntityManager.MinionsAndMonsters.<>c <>9 = new EntityManager.MinionsAndMonsters.<>c();
                public static Func<Obj_AI_Minion, bool> <>9__1_0;
                public static Func<Obj_AI_Minion, bool> <>9__11_0;
                public static Func<Obj_AI_Minion, bool> <>9__13_0;
                public static Func<Obj_AI_Minion, bool> <>9__15_0;
                public static Func<Obj_AI_Minion, bool> <>9__17_0;
                public static Func<Obj_AI_Minion, bool> <>9__19_0;
                public static Func<Obj_AI_Minion, float> <>9__20_1;
                public static Func<Obj_AI_Minion, Vector2> <>9__23_0;
                public static Func<Obj_AI_Minion, Vector2> <>9__25_0;
                public static Func<Obj_AI_Minion, bool> <>9__3_0;
                public static Func<Obj_AI_Minion, bool> <>9__9_0;

                internal bool <get_AlliedMinions>b__9_0(Obj_AI_Minion o) => 
                    o.IsAlly;

                internal bool <get_EnemyMinions>b__11_0(Obj_AI_Minion o) => 
                    o.IsEnemy;

                internal bool <get_JunglePlants>b__15_0(Obj_AI_Minion o) => 
                    ((o.IsValidTarget(null, false, null) && o.MaxHealth.Equals((float) 1f)) && o.BaseSkinName.StartsWith("SRU_Plant_"));

                internal bool <get_Minions>b__3_0(Obj_AI_Minion o) => 
                    (((o.IsValidTarget(null, false, null) && o.IsMinion) && ((o.Team == GameObjectTeam.Chaos) || (o.Team == GameObjectTeam.Order))) && (o.MaxHealth > 6f));

                internal bool <get_Monsters>b__1_0(Obj_AI_Minion o) => 
                    ((o.IsValidTarget(null, false, null) && o.IsMonster) && (o.MaxHealth > 3f));

                internal bool <get_OtherAllyMinions>b__17_0(Obj_AI_Minion o) => 
                    o.IsAlly;

                internal bool <get_OtherEnemyMinions>b__19_0(Obj_AI_Minion o) => 
                    o.IsEnemy;

                internal bool <get_OtherMinions>b__13_0(Obj_AI_Minion o) => 
                    (o.IsValidTarget(null, false, null) && ((((o.IsMinion && !o.IsMonster) && ((o.MaxHealth <= 6f) && !ObjectNames.InvalidTargets.Contains(o.BaseSkinName))) || (!o.IsMinion && !o.IsMonster)) || ((((o.IsMonster && !o.IsMinion) && ((o.MaxHealth <= 3f) && (o.Health > 0f))) && o.HasBuff("GangplankEBarrelActive")) && o.GetBuff("GangplankEBarrelActive").Caster.IsEnemy)));

                internal float <Get>b__20_1(Obj_AI_Minion o) => 
                    o.MaxHealth;

                internal Vector2 <GetCircularFarmLocation>b__23_0(Obj_AI_Minion o) => 
                    o.ServerPosition.To2D();

                internal Vector2 <GetLineFarmLocation>b__25_0(Obj_AI_Minion o) => 
                    o.ServerPosition.To2D();
            }

            public enum EntityType
            {
                Minion,
                Monster,
                Both
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct FarmLocation
            {
                public int HitNumber;
                public Vector3 CastPosition;
            }
        }

        public static class Turrets
        {
            internal static List<Obj_AI_Turret> _allies = new List<Obj_AI_Turret>();
            internal static List<Obj_AI_Turret> _allTurrets = new List<Obj_AI_Turret>();
            internal static List<Obj_AI_Turret> _enemies = new List<Obj_AI_Turret>();

            internal static void Initialize()
            {
                _allTurrets = ObjectManager.Get<Obj_AI_Turret>().ToList<Obj_AI_Turret>();
                _allies = new List<Obj_AI_Turret>();
                _enemies = new List<Obj_AI_Turret>();
                if (!Bootstrap.IsSpectatorMode)
                {
                    _allies = AllTurrets.FindAll(o => o.IsAlly);
                    _enemies = AllTurrets.FindAll(o => o.IsEnemy);
                }
                GameObject.OnDelete += delegate (GameObject sender, EventArgs args) {
                    if (sender is Obj_AI_Turret)
                    {
                        _allTurrets.RemoveAll(o => o.IdEquals(sender));
                        if (!Bootstrap.IsSpectatorMode)
                        {
                            _allies.RemoveAll(o => o.IdEquals(sender));
                            _enemies.RemoveAll(o => o.IdEquals(sender));
                        }
                    }
                };
            }

            public static List<Obj_AI_Turret> Allies =>
                new List<Obj_AI_Turret>(_allies);

            public static List<Obj_AI_Turret> AllTurrets =>
                new List<Obj_AI_Turret>(_allTurrets);

            public static List<Obj_AI_Turret> Enemies =>
                new List<Obj_AI_Turret>(_enemies);

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly EntityManager.Turrets.<>c <>9 = new EntityManager.Turrets.<>c();
                public static Predicate<Obj_AI_Turret> <>9__9_0;
                public static Predicate<Obj_AI_Turret> <>9__9_1;
                public static GameObjectDelete <>9__9_2;

                internal bool <Initialize>b__9_0(Obj_AI_Turret o) => 
                    o.IsAlly;

                internal bool <Initialize>b__9_1(Obj_AI_Turret o) => 
                    o.IsEnemy;

                internal void <Initialize>b__9_2(GameObject sender, EventArgs args)
                {
                    if (sender is Obj_AI_Turret)
                    {
                        EntityManager.Turrets._allTurrets.RemoveAll(o => o.IdEquals(sender));
                        if (!Bootstrap.IsSpectatorMode)
                        {
                            EntityManager.Turrets._allies.RemoveAll(o => o.IdEquals(sender));
                            EntityManager.Turrets._enemies.RemoveAll(o => o.IdEquals(sender));
                        }
                    }
                }
            }
        }

        public enum UnitTeam
        {
            Ally,
            Both,
            Enemy
        }
    }
}

