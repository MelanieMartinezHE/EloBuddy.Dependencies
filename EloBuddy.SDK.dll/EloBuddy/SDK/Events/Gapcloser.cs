namespace EloBuddy.SDK.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Properties;
    using EloBuddy.SDK.Utils;
    using Newtonsoft.Json;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class Gapcloser
    {
        internal static List<GapcloserEventArgs> _activeGapClosers = new List<GapcloserEventArgs>();
        internal static List<GapCloser> _gapCloserList = new List<GapCloser>();
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EloBuddy.SDK.Menu.Menu <Menu>k__BackingField;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event GapcloserHandler OnGapcloser;

        static Gapcloser()
        {
            try
            {
                _gapCloserList = JsonConvert.DeserializeObject<List<GapCloser>>(Resources.Gapclosers);
            }
            catch (Exception exception)
            {
                object[] args = new object[] { exception };
                Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Error, "Failed to load gapclosers:\n{}", args);
            }
            Obj_AI_Base.OnProcessSpellCast += new Obj_AI_ProcessSpellCast(Gapcloser.OnProcessSpellCast);
            Game.OnTick += new GameTick(Gapcloser.OnTick);
        }

        internal static void AddMenu()
        {
            List<GapCloser> source = new List<GapCloser>();
            using (IEnumerator<GapCloser> enumerator = (from h in _gapCloserList
                where EntityManager.Heroes.AllHeroes.Any<AIHeroClient>(h => h.ChampionName == h.ChampName)
                orderby h.ChampName, h.SpellSlot
                select h).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    GapCloser gapCloser = enumerator.Current;
                    if (Menu == null)
                    {
                        Menu = Core.Menu.AddSubMenu("Gapcloser", null, null);
                        Menu.AddGroupLabel("Spells to be detected:");
                    }
                    if (source.All<GapCloser>(o => o.ChampName != gapCloser.ChampName))
                    {
                        Menu.AddLabel(gapCloser.ChampName, 0x19);
                    }
                    Menu.Add<CheckBox>(gapCloser.ToString(), new CheckBox($"{gapCloser.SpellSlot} - {gapCloser.SpellName}", true));
                    source.Add(gapCloser);
                }
            }
        }

        internal static void Initialize()
        {
        }

        internal static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            AIHeroClient heroSender = sender as AIHeroClient;
            if (heroSender != null)
            {
                GapCloser closer = _gapCloserList.FirstOrDefault<GapCloser>(o => (o.ChampName == heroSender.ChampionName) && (o.SpellName == (args.SData.Name ?? args.SData.AlternateName).ToLower()));
                if (closer.IsValid)
                {
                    Vector3 end = args.End;
                    Obj_AI_Base target = null;
                    switch (closer.SkillType)
                    {
                        case GapcloserType.Skillshot:
                        {
                            float castRangeDisplayOverride = args.SData.CastRangeDisplayOverride;
                            if (castRangeDisplayOverride == 0f)
                            {
                                castRangeDisplayOverride = args.SData.CastRange;
                            }
                            SpellDataTargetType targettingType = args.SData.TargettingType;
                            bool flag3 = (((targettingType == SpellDataTargetType.Location) || (targettingType == SpellDataTargetType.Location2)) || (targettingType == SpellDataTargetType.Location3)) || (targettingType == SpellDataTargetType.LocationAoe);
                            if (!end.IsInRange(((Obj_AI_Base) heroSender), castRangeDisplayOverride) | flag3)
                            {
                                end = heroSender.ServerPosition.Extend(end, castRangeDisplayOverride).To3DWorld();
                            }
                            break;
                        }
                        case GapcloserType.Targeted:
                            target = args.Target as Obj_AI_Base;
                            break;
                    }
                    string champName = closer.ChampName;
                    switch (<PrivateImplementationDetails>.ComputeStringHash(champName))
                    {
                        case 0x4e0475ff:
                            if (champName == "Caitlyn")
                            {
                                end = heroSender.ServerPosition.Extend(args.End, -375f).To3D(0);
                                break;
                            }
                            break;

                        case 0x6cbde4c6:
                            if (champName == "Leona")
                            {
                                AIHeroClient client = (from h in EntityManager.Heroes.Enemies
                                    orderby args.Start.Distance((Obj_AI_Base) h, true) descending
                                    select h).FirstOrDefault<AIHeroClient>(h => h.IsValidTarget(null, false, null) && (h.ServerPosition.To2D().Distance(args.Start.To2D(), args.End.To2D(), true, true) <= ((float) ((args.SData.LineWidth + h.BoundingRadius) * 1.8f)).Pow()));
                                if (client > null)
                                {
                                    end = client.ServerPosition;
                                    target = client;
                                    break;
                                }
                                return;
                            }
                            break;

                        case 0xb18a1139:
                            if (champName == "Azir")
                            {
                                Obj_AI_Minion minion = (from s in Orbwalker.AzirSoldiers
                                    orderby ((Obj_AI_Base) s).Distance(args.End, false)
                                    select s).FirstOrDefault<Obj_AI_Minion>(s => s.IsValid && (s.Team == heroSender.Team));
                                if (minion > null)
                                {
                                    end = minion.ServerPosition;
                                    target = minion;
                                    break;
                                }
                                return;
                            }
                            break;

                        case 0xf9552cf5:
                            if (champName == "LeeSin")
                            {
                                if (args.Slot != SpellSlot.Q)
                                {
                                    break;
                                }
                                Obj_AI_Base base4 = ObjectManager.Get<Obj_AI_Base>().FirstOrDefault<Obj_AI_Base>(o => (o.IsValidTarget(null, false, null) && (o.Team != heroSender.Team)) && (o.HasBuff("BlindMonkQOne") || o.HasBuff("BlindMonkQOneChaos")));
                                if (base4 > null)
                                {
                                    end = base4.ServerPosition;
                                    target = base4;
                                    break;
                                }
                                return;
                            }
                            break;

                        case 0xfa96c495:
                            if (champName == "Riven")
                            {
                                if (((heroSender.GetBuffCount("RivenTriCleave") >= 2) && (args.Slot == SpellSlot.Q)) || (args.Slot == SpellSlot.E))
                                {
                                    end = heroSender.Path.LastOrDefault<Vector3>();
                                }
                                else
                                {
                                    return;
                                }
                                break;
                            }
                            break;

                        case 0xd3333f20:
                            if (champName == "JarvanIV")
                            {
                                Obj_AI_Minion minion2 = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault<Obj_AI_Minion>(o => ((o.IsEnemy && o.IsMinion) && (!o.IsMonster && (o.MaxHealth <= 6f))) && o.BaseSkinName.Equals("JarvanIVStandard"));
                                if ((minion2 != null) && (minion2.ServerPosition.To2D().Distance(args.Start.To2D(), end.To2D(), true, true) <= ((float) (args.SData.LineWidth + minion2.BoundingRadius)).Pow().Pow()))
                                {
                                    break;
                                }
                                return;
                            }
                            break;

                        case 0xe26daab7:
                            if (champName == "Thresh")
                            {
                                Obj_AI_Base base3 = ObjectManager.Get<Obj_AI_Base>().FirstOrDefault<Obj_AI_Base>(o => (o.IsValidTarget(null, false, null) && (o.Team != heroSender.Team)) && o.Buffs.Any<BuffInstance>(b => b.Name.ToLower().Contains("threshq")));
                                if (base3 > null)
                                {
                                    end = base3.ServerPosition;
                                    target = base3;
                                    break;
                                }
                                return;
                            }
                            break;
                    }
                    GapcloserEventArgs item = new GapcloserEventArgs {
                        Handle = closer,
                        Sender = heroSender,
                        SpellName = args.SData.Name ?? args.SData.AlternateName,
                        Slot = closer.SpellSlot,
                        Type = closer.SkillType,
                        Start = args.Start,
                        SenderMousePos = args.End,
                        End = end,
                        Target = target,
                        TickCount = Core.GameTickCount,
                        GameTime = Game.Time
                    };
                    _activeGapClosers.Add(item);
                }
            }
        }

        internal static void OnTick(EventArgs args)
        {
            _activeGapClosers.RemoveAll(entry => (Core.GameTickCount > (entry.TickCount + 0x5dc)) || entry.Sender.IsDead);
            if ((OnGapcloser != null) && (_activeGapClosers.Count != 0))
            {
                foreach (GapcloserEventArgs args2 in from o in _activeGapClosers
                    where o.Handle.IsEnabled
                    select o)
                {
                    if ((args2.Target != null) && args2.Target.IsValidTarget(null, false, null))
                    {
                        args2.End = args2.Target.ServerPosition;
                    }
                    OnGapcloser(args2.Sender, args2);
                }
            }
        }

        public static List<GapcloserEventArgs> ActiveGapClosers =>
            new List<GapcloserEventArgs>(_activeGapClosers);

        public static List<GapCloser> GapCloserList =>
            new List<GapCloser>(_gapCloserList);

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

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Gapcloser.<>c <>9 = new Gapcloser.<>c();
            public static Func<Gapcloser.GapCloser, bool> <>9__16_0;
            public static Func<Gapcloser.GapCloser, string> <>9__16_2;
            public static Func<Gapcloser.GapCloser, SpellSlot> <>9__16_3;
            public static Predicate<Gapcloser.GapcloserEventArgs> <>9__17_0;
            public static Func<Gapcloser.GapcloserEventArgs, bool> <>9__17_1;
            public static Func<Obj_AI_Minion, bool> <>9__18_6;
            public static Func<BuffInstance, bool> <>9__18_8;

            internal bool <AddMenu>b__16_0(Gapcloser.GapCloser gapCloser) => 
                EntityManager.Heroes.AllHeroes.Any<AIHeroClient>(h => (h.ChampionName == gapCloser.ChampName));

            internal string <AddMenu>b__16_2(Gapcloser.GapCloser h) => 
                h.ChampName;

            internal SpellSlot <AddMenu>b__16_3(Gapcloser.GapCloser h) => 
                h.SpellSlot;

            internal bool <OnProcessSpellCast>b__18_6(Obj_AI_Minion o) => 
                (((o.IsEnemy && o.IsMinion) && (!o.IsMonster && (o.MaxHealth <= 6f))) && o.BaseSkinName.Equals("JarvanIVStandard"));

            internal bool <OnProcessSpellCast>b__18_8(BuffInstance b) => 
                b.Name.ToLower().Contains("threshq");

            internal bool <OnTick>b__17_0(Gapcloser.GapcloserEventArgs entry) => 
                ((Core.GameTickCount > (entry.TickCount + 0x5dc)) || entry.Sender.IsDead);

            internal bool <OnTick>b__17_1(Gapcloser.GapcloserEventArgs o) => 
                o.Handle.IsEnabled;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GapCloser
        {
            public string ChampName;
            public EloBuddy.SpellSlot SpellSlot;
            public string SpellName;
            public Gapcloser.GapcloserType SkillType;
            internal bool IsValid =>
                ((this.ChampName != null) && (this.SpellName > null));
            internal bool IsEnabled =>
                Gapcloser.Menu[this.ToString()].Cast<CheckBox>().CurrentValue;
            public override string ToString() => 
                $"{this.ChampName}: ({this.SpellSlot}) - {this.SpellName} ({this.SkillType})";
        }

        public class GapcloserEventArgs : EventArgs
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Vector3 <End>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float <GameTime>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Gapcloser.GapCloser <Handle>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private AIHeroClient <Sender>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Vector3 <SenderMousePos>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SpellSlot <Slot>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <SpellName>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Vector3 <Start>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Obj_AI_Base <Target>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <TickCount>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Gapcloser.GapcloserType <Type>k__BackingField;

            public Vector3 End { get; set; }

            public float GameTime { get; set; }

            internal Gapcloser.GapCloser Handle { get; set; }

            public AIHeroClient Sender { get; set; }

            public Vector3 SenderMousePos { get; set; }

            public SpellSlot Slot { get; set; }

            public string SpellName { get; set; }

            public Vector3 Start { get; set; }

            public Obj_AI_Base Target { get; set; }

            public int TickCount { get; set; }

            public Gapcloser.GapcloserType Type { get; set; }
        }

        public delegate void GapcloserHandler(AIHeroClient sender, Gapcloser.GapcloserEventArgs e);

        public enum GapcloserType
        {
            Skillshot,
            Targeted
        }
    }
}

