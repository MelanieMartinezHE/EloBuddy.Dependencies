namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Utils;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class DamageLibraryManager
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static DamageDatabase <Database>k__BackingField;

        static DamageLibraryManager()
        {
            Database = new DamageDatabase();
        }

        internal static bool ContainsChampion(Champion champion) => 
            Database.ContainsKey(champion);

        internal static bool ContainsSlot(Champion champion, SpellSlot slot) => 
            (ContainsChampion(champion) && Database[champion].ContainsKey(slot));

        internal static bool ContainsStage(Champion champion, SpellSlot slot, DamageLibrary.SpellStages stage) => 
            (ContainsSlot(champion, slot) && Database[champion][slot].ContainsKey(stage));

        internal static string GetSpellDataChampName(Champion champion)
        {
            switch (champion)
            {
                case Champion.AurelionSol:
                    return "Aurelion Sol";

                case Champion.Chogath:
                    return "Cho'Gath";

                case Champion.DrMundo:
                    return "Dr Mundo";

                case Champion.Khazix:
                    return "Kha'Zix";

                case Champion.KogMaw:
                    return "Kog'Maw";

                case Champion.Leblanc:
                    return "LeBlanc";

                case Champion.LeeSin:
                    return "Lee Sin";

                case Champion.MasterYi:
                    return "Master Yi";

                case Champion.FiddleSticks:
                    return "Fiddlesticks";

                case Champion.JarvanIV:
                    return "Jarvan IV";

                case Champion.MissFortune:
                    return "Miss Fortune";

                case Champion.RekSai:
                    return "Rek'Sai";

                case Champion.TahmKench:
                    return "Tahm Kench";

                case Champion.TwistedFate:
                    return "Twisted Fate";

                case Champion.Velkoz:
                    return "Vel'Koz";

                case Champion.MonkeyKing:
                    return "Wukong";

                case Champion.XinZhao:
                    return "Xin Zhao";
            }
            return champion.ToString();
        }

        internal static void Initialize()
        {
            Dictionary<string, Dictionary<SpellSlot, List<StageSpell>>> dictionary = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<SpellSlot, List<StageSpell>>>>(DefaultSettings.DamageLibrary);
            IEnumerable<Champion> enumerable = from x in EntityManager.Heroes.AllHeroes select x.Hero;
            foreach (Champion champion in enumerable)
            {
                string spellDataChampName = GetSpellDataChampName(champion);
                if ((spellDataChampName != null) && dictionary.ContainsKey(spellDataChampName))
                {
                    ChampionDamageDatabase database;
                    if (Database.TryMakeNewChampion(champion, out database))
                    {
                        foreach (KeyValuePair<SpellSlot, List<StageSpell>> pair in dictionary[spellDataChampName])
                        {
                            SpellDamageDatabase database2 = database.NewOrExistingSpell(pair.Key);
                            foreach (StageSpell spell in pair.Value)
                            {
                                if (database2.ContainsKey(spell.Stage))
                                {
                                    object[] args = new object[] { spell.Stage, spellDataChampName, pair.Key };
                                    Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Warn, "The stage '{0}' is already present in '{1}' {2}!", args);
                                }
                                else
                                {
                                    database2.AddStageSpell(spell, pair.Key);
                                }
                            }
                        }
                    }
                }
                else if (!Database.ContainsKey(champion))
                {
                    object[] objArray2 = new object[] { spellDataChampName };
                    Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Warn, "'{0}' was not found in the DamageLibrary!", objArray2);
                    Database.Add(champion, new ChampionDamageDatabase());
                }
            }
        }

        internal static void ReplaceSpell<T>(AIHeroClient hero, SpellSlot slot, DamageLibrary.SpellStages stage) where T: DamageSourceReplacement
        {
            object[] args = new object[] { slot, Database[hero.Hero][slot][stage] };
            Database[hero.Hero][slot][stage] = (T) Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, args, null);
        }

        internal static bool TryGetChampion(Champion champion, out ChampionDamageDatabase database)
        {
            database = null;
            if (!ContainsChampion(champion))
            {
                return false;
            }
            database = Database[champion];
            return true;
        }

        internal static bool TryGetSlot(Champion champion, SpellSlot slot, out SpellDamageDatabase database)
        {
            database = null;
            if (!ContainsSlot(champion, slot))
            {
                return false;
            }
            database = Database[champion][slot];
            return true;
        }

        internal static bool TryGetStage(Champion champion, SpellSlot slot, DamageLibrary.SpellStages stage, out Damage.DamageSourceBase damageSourceBase)
        {
            damageSourceBase = null;
            if (!ContainsStage(champion, slot, stage))
            {
                return false;
            }
            damageSourceBase = Database[champion][slot][stage];
            return true;
        }

        internal static DamageDatabase Database
        {
            [CompilerGenerated]
            get => 
                <Database>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Database>k__BackingField = value;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly DamageLibraryManager.<>c <>9 = new DamageLibraryManager.<>c();
            public static Func<AIHeroClient, Champion> <>9__16_0;

            internal Champion <Initialize>b__16_0(AIHeroClient x) => 
                x.Hero;
        }

        public class ChampionDamageDatabase : DamageLibraryManager.LockedDictionary<SpellSlot, DamageLibraryManager.SpellDamageDatabase>
        {
            public DamageLibraryManager.SpellDamageDatabase NewOrExistingSpell(SpellSlot key) => 
                (base.ContainsKey(key) ? base[key] : this.NewSpell(key));

            public DamageLibraryManager.SpellDamageDatabase NewSpell(SpellSlot key)
            {
                DamageLibraryManager.SpellDamageDatabase database;
                base.Add(key, database = new DamageLibraryManager.SpellDamageDatabase());
                return database;
            }
        }

        internal class ChampionSpell
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private DamageLibraryManager.SpellBonus[] <BonusDamages>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float[] <Damages>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.DamageType <DamageType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private DamageLibraryManager.ExpresionDamage[] <ExpresionDamages>k__BackingField;

            public Damage.DamageSourceBase ToDamageSourceBase(SpellSlot slot)
            {
                Damage.DamageSourceBoundle boundle = new Damage.DamageSourceBoundle();
                Damage.DamageSource damageSource = new Damage.DamageSource(slot, this.DamageType) {
                    Damages = this.Damages
                };
                boundle.Add(damageSource);
                foreach (DamageLibraryManager.SpellBonus bonus in this.BonusDamages)
                {
                    Damage.BonusDamageSource source2 = new Damage.BonusDamageSource(slot, bonus.DamageType) {
                        DamagePercentages = bonus.DamagePercentages,
                        ScalingTarget = bonus.ScalingTarget,
                        ScalingType = bonus.ScalingType
                    };
                    boundle.Add(source2);
                }
                if (this.ExpresionDamages > null)
                {
                    foreach (DamageLibraryManager.ExpresionDamage damage in this.ExpresionDamages)
                    {
                        Damage.ExpresionDamageSource source3 = new Damage.ExpresionDamageSource(damage.Expression, slot, damage.DamageType) {
                            DamageType = damage.DamageType,
                            DamagePercentages = damage.DamagePercentages,
                            Variables = (from x in damage.StaticVariables select new Damage.ExpresionStaticVarible(x.Key, x.ScalingTarget, x.ScalingType)).Cast<Damage.IVariable>().Concat<Damage.IVariable>(((IEnumerable<Damage.IVariable>) (from x in damage.TypeVariables select new Damage.ExpresionTypeVarible(x.Key, x.DamageType, x.Target, x.Name, x.Parameters)))).Concat<Damage.IVariable>((IEnumerable<Damage.IVariable>) (from x in damage.LevelVariables select new Damage.ExpresionLevelVarible(x.Key, x.Slot, x.Damages))),
                            Expression = damage.Expression,
                            Condition = damage.Condition
                        };
                        boundle.AddExpresion(source3);
                    }
                }
                return boundle;
            }

            public DamageLibraryManager.SpellBonus[] BonusDamages { get; set; }

            public float[] Damages { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public EloBuddy.DamageType DamageType { get; set; }

            public DamageLibraryManager.ExpresionDamage[] ExpresionDamages { get; set; }

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly DamageLibraryManager.ChampionSpell.<>c <>9 = new DamageLibraryManager.ChampionSpell.<>c();
                public static Func<DamageLibraryManager.StaticDamageVarible, Damage.ExpresionStaticVarible> <>9__16_0;
                public static Func<DamageLibraryManager.TypeDamageVarible, Damage.ExpresionTypeVarible> <>9__16_1;
                public static Func<DamageLibraryManager.LevelDamageVarible, Damage.ExpresionLevelVarible> <>9__16_2;

                internal Damage.ExpresionStaticVarible <ToDamageSourceBase>b__16_0(DamageLibraryManager.StaticDamageVarible x) => 
                    new Damage.ExpresionStaticVarible(x.Key, x.ScalingTarget, x.ScalingType);

                internal Damage.ExpresionTypeVarible <ToDamageSourceBase>b__16_1(DamageLibraryManager.TypeDamageVarible x) => 
                    new Damage.ExpresionTypeVarible(x.Key, x.DamageType, x.Target, x.Name, x.Parameters);

                internal Damage.ExpresionLevelVarible <ToDamageSourceBase>b__16_2(DamageLibraryManager.LevelDamageVarible x) => 
                    new Damage.ExpresionLevelVarible(x.Key, x.Slot, x.Damages);
            }
        }

        internal class DamageDatabase : Dictionary<Champion, DamageLibraryManager.ChampionDamageDatabase>
        {
            public DamageLibraryManager.ChampionDamageDatabase NewChampion(Champion key)
            {
                DamageLibraryManager.ChampionDamageDatabase database;
                if (!this.TryMakeNewChampion(key, out database))
                {
                    throw new ArgumentException("Key is already present in the Champion Database. Use TryMakeNewChampion() to prevent this in the future.", "key");
                }
                return database;
            }

            public bool TryMakeNewChampion(Champion key, out DamageLibraryManager.ChampionDamageDatabase database)
            {
                database = null;
                if (base.ContainsKey(key))
                {
                    return false;
                }
                database = new DamageLibraryManager.ChampionDamageDatabase();
                base.Add(key, database);
                return true;
            }
        }

        internal abstract class DamageSourceReplacement : Damage.DamageSourceBoundle
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Damage.DamageSourceBoundle <MonsterDamage>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <MonsterMaxDamage>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int[] <MonsterMaxDamages>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Damage.DamageSourceBase <OriginalDamageSource>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SpellSlot <Slot>k__BackingField;

            internal DamageSourceReplacement(SpellSlot slot, Damage.DamageSourceBase originalDamageSource)
            {
                this.Slot = slot;
                this.OriginalDamageSource = originalDamageSource;
                this.MonsterMaxDamage = -1;
                base.Add(originalDamageSource);
            }

            public override float GetDamage(Obj_AI_Base source, Obj_AI_Base target)
            {
                if (target is Obj_AI_Minion)
                {
                    if ((base.Condition == null) ? false : !base.Condition(target))
                    {
                        return 0f;
                    }
                    int num = (this.MonsterMaxDamage > -1) ? this.MonsterMaxDamage : ((this.MonsterMaxDamages != null) ? this.MonsterMaxDamages[source.Spellbook.GetSpell(this.Slot).Level - 1] : -1);
                    if (this.MonsterDamage > null)
                    {
                        return ((num > -1) ? Math.Min((float) num, this.MonsterDamage.GetDamage(source, target)) : this.MonsterDamage.GetDamage(source, target));
                    }
                    if (num > -1)
                    {
                        return Math.Min((float) num, base.GetDamage(source, target));
                    }
                }
                return base.GetDamage(source, target);
            }

            public void SetMonsterDamage(DamageType damageType, float[] damages = null, params Damage.DamageSourceBase[] sources)
            {
                this.MonsterDamage = new Damage.DamageSourceBoundle();
                Damage.DamageSource damageSource = new Damage.DamageSource(this.Slot, damageType) {
                    Damages = damages ?? new float[5]
                };
                this.MonsterDamage.Add(damageSource);
                foreach (Damage.DamageSourceBase base2 in sources)
                {
                    this.MonsterDamage.Add(base2);
                }
            }

            internal Damage.DamageSourceBoundle MonsterDamage { get; set; }

            internal int MonsterMaxDamage { get; set; }

            internal int[] MonsterMaxDamages { get; set; }

            internal Damage.DamageSourceBase OriginalDamageSource { get; set; }

            internal SpellSlot Slot { get; set; }
        }

        internal class ExpresionDamage
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Condition>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float[] <DamagePercentages>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.DamageType <DamageType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Expression>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private DamageLibraryManager.LevelDamageVarible[] <LevelVariables>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private DamageLibraryManager.StaticDamageVarible[] <StaticVariables>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private DamageLibraryManager.TypeDamageVarible[] <TypeVariables>k__BackingField;

            public string Condition { get; set; }

            public float[] DamagePercentages { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public EloBuddy.DamageType DamageType { get; set; }

            public string Expression { get; set; }

            public DamageLibraryManager.LevelDamageVarible[] LevelVariables { get; set; }

            public DamageLibraryManager.StaticDamageVarible[] StaticVariables { get; set; }

            public DamageLibraryManager.TypeDamageVarible[] TypeVariables { get; set; }
        }

        internal class KalistaW : DamageLibraryManager.DamageSourceReplacement
        {
            public KalistaW(SpellSlot slot, Damage.DamageSourceBase originalDamageSource) : base(slot, originalDamageSource)
            {
                base.MonsterMaxDamages = new int[] { 0x4b, 0x7d, 150, 0xaf, 200 };
                base.Condition = target => target.Buffs.Any<BuffInstance>(b => b.IsValid() && b.Name.Contains("kalistacoopstrikemark"));
            }

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly DamageLibraryManager.KalistaW.<>c <>9 = new DamageLibraryManager.KalistaW.<>c();
                public static Func<Obj_AI_Base, bool> <>9__0_0;
                public static Func<BuffInstance, bool> <>9__0_1;

                internal bool <.ctor>b__0_0(Obj_AI_Base target) => 
                    target.Buffs.Any<BuffInstance>(b => (b.IsValid() && b.Name.Contains("kalistacoopstrikemark")));

                internal bool <.ctor>b__0_1(BuffInstance b) => 
                    (b.IsValid() && b.Name.Contains("kalistacoopstrikemark"));
            }
        }

        internal class LevelDamageVarible
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float[] <Damages>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Key>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SpellSlot <Slot>k__BackingField;

            public float[] Damages { get; set; }

            public string Key { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public SpellSlot Slot { get; set; }
        }

        public class LockedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
        {
            private readonly Dictionary<TKey, TValue> _dictionary;

            public LockedDictionary()
            {
                this._dictionary = new Dictionary<TKey, TValue>();
            }

            internal TValue Add(TKey key, TValue value)
            {
                this._dictionary.Add(key, value);
                return value;
            }

            public bool ContainsKey(TKey key) => 
                this._dictionary.ContainsKey(key);

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => 
                this._dictionary.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => 
                this.GetEnumerator();

            public TValue this[TKey key]
            {
                get => 
                    this._dictionary[key];
                internal set
                {
                    this._dictionary[key] = value;
                }
            }
        }

        internal class MiniGnarW : DamageLibraryManager.DamageSourceReplacement
        {
            internal MiniGnarW(SpellSlot slot, Damage.DamageSourceBase originalDamageSource) : base(slot, originalDamageSource)
            {
                Damage.DamageSourceBase[] sources = new Damage.DamageSourceBase[1];
                Damage.BonusDamageSource source = new Damage.BonusDamageSource(SpellSlot.W, DamageType.Magical) {
                    DamagePercentages = new float[] { 
                        1f,
                        1f,
                        1f,
                        1f,
                        1f
                    },
                    ScalingType = Damage.ScalingType.AbilityPoints,
                    ScalingTarget = Damage.ScalingTarget.Source
                };
                sources[0] = source;
                base.SetMonsterDamage(DamageType.Magical, new float[] { 110f, 170f, 230f, 290f, 350f }, sources);
                base.Condition = target => target.GetBuffCount("GnarWProc") == 2;
            }

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly DamageLibraryManager.MiniGnarW.<>c <>9 = new DamageLibraryManager.MiniGnarW.<>c();
                public static Func<Obj_AI_Base, bool> <>9__0_0;

                internal bool <.ctor>b__0_0(Obj_AI_Base target) => 
                    (target.GetBuffCount("GnarWProc") == 2);
            }
        }

        internal class SpellBonus
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float[] <DamagePercentages>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.DamageType <DamageType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Damage.ScalingTarget <ScalingTarget>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Damage.ScalingType <ScalingType>k__BackingField;

            public float[] DamagePercentages { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public EloBuddy.DamageType DamageType { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public EloBuddy.SDK.Damage.ScalingTarget ScalingTarget { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public EloBuddy.SDK.Damage.ScalingType ScalingType { get; set; }
        }

        public class SpellDamageDatabase : DamageLibraryManager.LockedDictionary<DamageLibrary.SpellStages, Damage.DamageSourceBase>
        {
            internal void AddStageSpell(DamageLibraryManager.StageSpell spell, SpellSlot slot)
            {
                base.Add(spell.Stage, spell.SpellData.ToDamageSourceBase(slot));
            }
        }

        internal class StageSpell
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private DamageLibraryManager.ChampionSpell <SpellData>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private DamageLibrary.SpellStages <Stage>k__BackingField;

            public DamageLibraryManager.ChampionSpell SpellData { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public DamageLibrary.SpellStages Stage { get; set; }
        }

        internal class StaticDamageVarible
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Key>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Damage.ScalingTarget <ScalingTarget>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Damage.ScalingType <ScalingType>k__BackingField;

            public string Key { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public EloBuddy.SDK.Damage.ScalingTarget ScalingTarget { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public EloBuddy.SDK.Damage.ScalingType ScalingType { get; set; }
        }

        internal class TypeDamageVarible
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.DamageType <DamageType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Key>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Name>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string[] <Parameters>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Damage.ScalingTarget <Target>k__BackingField;

            [JsonConverter(typeof(StringEnumConverter))]
            public EloBuddy.DamageType DamageType { get; set; }

            public string Key { get; set; }

            public string Name { get; set; }

            public string[] Parameters { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public Damage.ScalingTarget Target { get; set; }
        }

        internal class VayneW : DamageLibraryManager.DamageSourceReplacement
        {
            internal VayneW(SpellSlot slot, Damage.DamageSourceBase originalDamageSource) : base(slot, originalDamageSource)
            {
                base.MonsterMaxDamage = 200;
                base.Condition = target => target.GetBuffCount("VayneSilverDebuff") == 2;
            }

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly DamageLibraryManager.VayneW.<>c <>9 = new DamageLibraryManager.VayneW.<>c();
                public static Func<Obj_AI_Base, bool> <>9__0_0;

                internal bool <.ctor>b__0_0(Obj_AI_Base target) => 
                    (target.GetBuffCount("VayneSilverDebuff") == 2);
            }
        }
    }
}

