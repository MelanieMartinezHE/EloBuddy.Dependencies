namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Notifications;
    using EloBuddy.SDK.Properties;
    using EloBuddy.SDK.Rendering;
    using EloBuddy.SDK.Utils;
    using Newtonsoft.Json;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class TargetSelector
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static TargetSelectorMode <ActiveMode>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EloBuddy.SDK.Menu.Menu <Menu>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static AIHeroClient <SelectedTarget>k__BackingField;
        internal static readonly Dictionary<Champion, string[]> BuffStackNames;
        internal static readonly Dictionary<Champion, Func<int>> CurrentPriorities = new Dictionary<Champion, Func<int>>();
        internal static readonly Dictionary<Champion, int> Priorities = new Dictionary<Champion, int>();
        internal static Dictionary<string, int> PriorityData;

        static TargetSelector()
        {
            Dictionary<Champion, string[]> dictionary = new Dictionary<Champion, string[]>();
            string[] textArray1 = new string[] { "BraumMark" };
            dictionary.Add(Champion.Unknown, textArray1);
            string[] textArray2 = new string[] { "DariusHemo" };
            dictionary.Add(Champion.Darius, textArray2);
            string[] textArray3 = new string[] { "EkkoStacks" };
            dictionary.Add(Champion.Ekko, textArray3);
            string[] textArray4 = new string[] { "GnarWProc" };
            dictionary.Add(Champion.Gnar, textArray4);
            string[] textArray5 = new string[] { "KalistaExpungeMarker" };
            dictionary.Add(Champion.Kalista, textArray5);
            string[] textArray6 = new string[] { "kennenmarkofstorm" };
            dictionary.Add(Champion.Kennen, textArray6);
            string[] textArray7 = new string[] { "KindredHitCharge", "kindredecharge" };
            dictionary.Add(Champion.Kindred, textArray7);
            string[] textArray8 = new string[] { "tahmkenchpdebuffcounter" };
            dictionary.Add(Champion.TahmKench, textArray8);
            string[] textArray9 = new string[] { "tristanaecharge" };
            dictionary.Add(Champion.Tristana, textArray9);
            string[] textArray10 = new string[] { "TwitchDeadlyVenom" };
            dictionary.Add(Champion.Twitch, textArray10);
            string[] textArray11 = new string[] { "VarusWDebuff" };
            dictionary.Add(Champion.Varus, textArray11);
            string[] textArray12 = new string[] { "VayneSilverDebuff" };
            dictionary.Add(Champion.Vayne, textArray12);
            string[] textArray13 = new string[] { "VelkozResearchStack" };
            dictionary.Add(Champion.Velkoz, textArray13);
            string[] textArray14 = new string[] { "ViWProc" };
            dictionary.Add(Champion.Vi, textArray14);
            BuffStackNames = dictionary;
        }

        public static int GetPriority(AIHeroClient target)
        {
            if (target == null)
            {
                return 0;
            }
            if (target.IsAlly && PriorityData.ContainsKey(target.ChampionName))
            {
                return PriorityData[target.ChampionName];
            }
            if (!CurrentPriorities.ContainsKey(target.Hero))
            {
                return 1;
            }
            return CurrentPriorities[target.Hero]();
        }

        internal static float GetReducedPriority(AIHeroClient target)
        {
            switch (GetPriority(target))
            {
                case 2:
                    return 1.5f;

                case 3:
                    return 1.75f;

                case 4:
                    return 2f;

                case 5:
                    return 2.5f;
            }
            return 1f;
        }

        public static AIHeroClient GetTarget(IEnumerable<AIHeroClient> possibleTargets, DamageType damageType)
        {
            List<AIHeroClient> source = possibleTargets.ToList<AIHeroClient>();
            List<AIHeroClient> collection = (from h in source
                where !h.HasUndyingBuff(true)
                select h).ToList<AIHeroClient>();
            if (collection.Count > 0)
            {
                source.Clear();
                source.AddRange(collection);
            }
            bool flag = SelectedEnabled && SelectedTarget.IsValidTarget(null, false, null);
            if (flag && OnlySelectedTarget)
            {
                return SelectedTarget;
            }
            switch (source.Count)
            {
                case 0:
                    return null;

                case 1:
                    return source[0];
            }
            if (flag && source.Contains(SelectedTarget))
            {
                return SelectedTarget;
            }
            switch (ActiveMode)
            {
                case TargetSelectorMode.Auto:
                    return (from h in source
                        orderby (GetReducedPriority(h) * Player.Instance.CalculateDamageOnUnit(h, ((damageType == DamageType.Magical) ? DamageType.Magical : DamageType.Physical), 100f, true, false)) / h.Health descending
                        select h).FirstOrDefault<AIHeroClient>();

                case TargetSelectorMode.MostStack:
                    return source.OrderByDescending<AIHeroClient, float>(delegate (AIHeroClient h) {
                        Func<string, int> <>9__10;
                        return ((((BuffStackNames.Sum<KeyValuePair<Champion, string[]>>(((Func<KeyValuePair<Champion, string[]>, int>) (pair => (((((Champion) pair.Key) == Player.Instance.Hero) || (((Champion) pair.Key) == Champion.Unknown)) ? pair.Value.Sum<string>(((Func<string, int>) (<>9__10 ?? (<>9__10 = stack => Math.Max(h.GetBuffCount(stack), 0))))) : 0)))) + 1) * GetReducedPriority(h)) * Player.Instance.CalculateDamageOnUnit(h, ((damageType == DamageType.Magical) ? DamageType.Magical : DamageType.Physical), 100f, true, false)) / h.Health);
                    }).FirstOrDefault<AIHeroClient>();

                case TargetSelectorMode.MostAbilityPower:
                    return (from unit in source
                        orderby unit.TotalMagicalDamage descending
                        select unit).FirstOrDefault<AIHeroClient>();

                case TargetSelectorMode.MostAttackDamage:
                    return (from unit in source
                        orderby unit.TotalAttackDamage descending
                        select unit).FirstOrDefault<AIHeroClient>();

                case TargetSelectorMode.LeastHealth:
                    return (from unit in source
                        orderby unit.Health
                        select unit).FirstOrDefault<AIHeroClient>();

                case TargetSelectorMode.Closest:
                    return (from unit in source
                        orderby ((Obj_AI_Base) unit).Distance((Obj_AI_Base) Player.Instance, true)
                        select unit).FirstOrDefault<AIHeroClient>();

                case TargetSelectorMode.HighestPriority:
                    return source.OrderByDescending<AIHeroClient, int>(new Func<AIHeroClient, int>(TargetSelector.GetPriority)).FirstOrDefault<AIHeroClient>();

                case TargetSelectorMode.LessAttack:
                    return (from h in source
                        orderby (GetReducedPriority(h) * Player.Instance.CalculateDamageOnUnit(h, DamageType.Physical, 100f, true, false)) / h.Health descending
                        select h).FirstOrDefault<AIHeroClient>();

                case TargetSelectorMode.LessCast:
                    return (from h in source
                        orderby (GetReducedPriority(h) * Player.Instance.CalculateDamageOnUnit(h, DamageType.Magical, 100f, true, false)) / h.Health descending
                        select h).FirstOrDefault<AIHeroClient>();

                case TargetSelectorMode.NearMouse:
                    return (from h in source
                        orderby ((Obj_AI_Base) h).Distance(Game.ActiveCursorPos, true)
                        select h).FirstOrDefault<AIHeroClient>();
            }
            return null;
        }

        public static AIHeroClient GetTarget(float range, DamageType damageType, Vector3? source = new Vector3?(), bool addBoundingRadius = false)
        {
            Vector3? nullable = source;
            Vector3 sourcePosition = nullable.HasValue ? nullable.GetValueOrDefault() : Player.Instance.ServerPosition;
            if (SelectedEnabled && SelectedTarget.IsValidTarget(null, false, null))
            {
                if (OnlySelectedTarget)
                {
                    return SelectedTarget;
                }
                if (sourcePosition.IsInRange((Obj_AI_Base) SelectedTarget, range * 1.15f))
                {
                    return SelectedTarget;
                }
            }
            return GetTarget(from h in EntityManager.Heroes.Enemies
                where h.IsValidTarget(null, false, null) && sourcePosition.IsInRange(((Obj_AI_Base) h), (range + (addBoundingRadius ? h.BoundingRadius : 0f)))
                select h, damageType);
        }

        internal static void Initialize()
        {
            ActiveMode = TargetSelectorMode.Auto;
            PriorityData = JsonConvert.DeserializeObject<Dictionary<string, int>>(Resources.Priorities);
            foreach (AIHeroClient client in from enemy in EntityManager.Heroes.Enemies
                where !Priorities.ContainsKey(enemy.Hero)
                select enemy)
            {
                if (PriorityData.ContainsKey(client.ChampionName))
                {
                    Priorities.Add(client.Hero, PriorityData[client.ChampionName]);
                }
                else
                {
                    object[] args = new object[] { client.ChampionName };
                    Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Warn, "[TargetSelector] '{0}' is not present in database! Using priority 1!", args);
                    Priorities.Add(client.Hero, 1);
                }
                string name = client.ChampionName;
                CurrentPriorities.Add(client.Hero, () => Menu[name].Cast<Slider>().CurrentValue);
            }
            Menu = MainMenu.AddMenu("Target Selector", "TargetSelector2.0", null);
            Menu.AddGroupLabel("Target Selector Mode");
            ComboBox modeBox = new ComboBox("Selected Mode:", from o in Enum.GetValues(typeof(TargetSelectorMode)).Cast<TargetSelectorMode>() select o.ToString(), 0);
            Menu.Add<ComboBox>("modeBox", modeBox).OnValueChange += (<sender>, <args>) => (ActiveMode = (TargetSelectorMode) Enum.Parse(typeof(TargetSelectorMode), modeBox.SelectedText));
            if (Priorities.Count > 0)
            {
                Menu.AddGroupLabel("Priorities");
                Menu.AddLabel("(Higher value means higher priority)", 0x19);
                foreach (KeyValuePair<Champion, int> pair in Priorities)
                {
                    string uniqueIdentifier = pair.Key.ToString();
                    string displayName = pair.Key.ToString();
                    Menu.Add<Slider>(uniqueIdentifier, new Slider(displayName, pair.Value, 1, 5));
                }
                Menu.AddSeparator(0x19);
                Menu.Add<CheckBox>("reset", new CheckBox("Reset to default priorities", false)).OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args) {
                    if (args.NewValue)
                    {
                        foreach (KeyValuePair<Champion, int> pair in Priorities)
                        {
                            Slider introduced4 = Menu[pair.Key.ToString()].Cast<Slider>();
                            introduced4.CurrentValue = pair.Value;
                        }
                        sender.CurrentValue = false;
                    }
                };
            }
            Menu.AddGroupLabel("Selected Target Settings");
            Menu.Add<CheckBox>("selectedTargetEnabled", new CheckBox("Enable manual selected target", true));
            Menu.Add<CheckBox>("drawSelectedTarget", new CheckBox("Draw a circle around selected target", true));
            Menu.Add<CheckBox>("drawNotifications", new CheckBox("Draw notifications about selected target", true));
            Menu.AddGroupLabel("Only Attack Selected Target Settings");
            Menu.Add<CheckBox>("onlySelectedTargetEnabled", new CheckBox("Enable only attack selected target", false));
            Menu.Add<KeyBind>("onlySelectedTargetKey", new KeyBind("Only attack selected target toggle", false, KeyBind.BindTypes.PressToggle, 90, 0x1b)).OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args) {
                if (OnlySelectedTargetEnabled && DrawNotifications)
                {
                    EloBuddy.SDK.Notifications.Notifications.Show(args.NewValue ? new SimpleNotification("Target Selector", "Only attack selected target enabled.") : new SimpleNotification("Target Selector", "Only attack selected target disabled."), 0x1388);
                }
            };
            Messages.RegisterEventHandler<Messages.LeftButtonDown>(delegate (Messages.LeftButtonDown args) {
                if ((!MenuGUI.IsChatOpen && !MainMenu.IsMouseInside) && SelectedEnabled)
                {
                    AIHeroClient client = EntityManager.Heroes.Enemies.FirstOrDefault<AIHeroClient>(o => o.IsValidTarget(null, false, null) && ((Obj_AI_Base) o).IsInRange(Game.ActiveCursorPos, 100f));
                    if (DrawNotifications)
                    {
                        if (client > null)
                        {
                            EloBuddy.SDK.Notifications.Notifications.Show(new SimpleNotification("Target Selector", "Selected " + client.ChampionName + " as target."), 0x1388);
                        }
                        else if (SelectedTarget > null)
                        {
                            EloBuddy.SDK.Notifications.Notifications.Show(new SimpleNotification("Target Selector", "Unselected " + SelectedTarget.ChampionName + " as target."), 0x1388);
                        }
                    }
                    SelectedTarget = client;
                }
            });
            Drawing.OnDraw += delegate (EventArgs <args>) {
                if ((SelectedEnabled && DrawCircleAroundSelected) && SelectedTarget.IsValidTarget(null, false, null))
                {
                    Vector3[] positions = new Vector3[] { SelectedTarget.Position };
                    EloBuddy.SDK.Rendering.Circle.Draw(SharpDX.Color.Red, OnlySelectedTarget ? ((float) 120) : ((float) 80), OnlySelectedTarget ? ((float) 15) : ((float) 5), positions);
                }
            };
        }

        public static TargetSelectorMode ActiveMode
        {
            [CompilerGenerated]
            get => 
                <ActiveMode>k__BackingField;
            [CompilerGenerated]
            set
            {
                <ActiveMode>k__BackingField = value;
            }
        }

        public static bool DrawCircleAroundSelected =>
            Menu["drawSelectedTarget"].Cast<CheckBox>().CurrentValue;

        private static bool DrawNotifications =>
            Menu["drawNotifications"].Cast<CheckBox>().CurrentValue;

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

        public static bool OnlySelectedTarget =>
            (OnlySelectedTargetEnabled && OnlySelectedTargetKey);

        private static bool OnlySelectedTargetEnabled =>
            Menu["onlySelectedTargetEnabled"].Cast<CheckBox>().CurrentValue;

        private static bool OnlySelectedTargetKey =>
            Menu["onlySelectedTargetKey"].Cast<KeyBind>().CurrentValue;

        public static bool SelectedEnabled =>
            Menu["selectedTargetEnabled"].Cast<CheckBox>().CurrentValue;

        public static AIHeroClient SelectedTarget
        {
            [CompilerGenerated]
            get => 
                <SelectedTarget>k__BackingField;
            [CompilerGenerated]
            internal set
            {
                <SelectedTarget>k__BackingField = value;
            }
        }

        public static bool SeletedEnabled =>
            SelectedEnabled;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly TargetSelector.<>c <>9 = new TargetSelector.<>c();
            public static Func<AIHeroClient, bool> <>9__30_0;
            public static Func<TargetSelectorMode, string> <>9__30_2;
            public static ValueBase<bool>.ValueChangeHandler <>9__30_4;
            public static ValueBase<bool>.ValueChangeHandler <>9__30_5;
            public static Messages.MessageHandler<Messages.LeftButtonDown> <>9__30_6;
            public static Func<AIHeroClient, bool> <>9__30_7;
            public static DrawingDraw <>9__30_8;
            public static Func<AIHeroClient, bool> <>9__31_0;
            public static Func<AIHeroClient, float> <>9__31_1;
            public static Func<AIHeroClient, float> <>9__31_2;
            public static Func<AIHeroClient, float> <>9__31_3;
            public static Func<AIHeroClient, float> <>9__31_4;
            public static Func<AIHeroClient, float> <>9__31_5;
            public static Func<AIHeroClient, float> <>9__31_6;
            public static Func<AIHeroClient, float> <>9__31_7;

            internal bool <GetTarget>b__31_0(AIHeroClient h) => 
                !h.HasUndyingBuff(true);

            internal float <GetTarget>b__31_1(AIHeroClient h) => 
                ((Obj_AI_Base) h).Distance(Game.ActiveCursorPos, true);

            internal float <GetTarget>b__31_2(AIHeroClient h) => 
                ((TargetSelector.GetReducedPriority(h) * Player.Instance.CalculateDamageOnUnit(h, DamageType.Magical, 100f, true, false)) / h.Health);

            internal float <GetTarget>b__31_3(AIHeroClient h) => 
                ((TargetSelector.GetReducedPriority(h) * Player.Instance.CalculateDamageOnUnit(h, DamageType.Physical, 100f, true, false)) / h.Health);

            internal float <GetTarget>b__31_4(AIHeroClient unit) => 
                unit.TotalMagicalDamage;

            internal float <GetTarget>b__31_5(AIHeroClient unit) => 
                unit.TotalAttackDamage;

            internal float <GetTarget>b__31_6(AIHeroClient unit) => 
                unit.Health;

            internal float <GetTarget>b__31_7(AIHeroClient unit) => 
                ((Obj_AI_Base) unit).Distance(((Obj_AI_Base) Player.Instance), true);

            internal bool <Initialize>b__30_0(AIHeroClient enemy) => 
                !TargetSelector.Priorities.ContainsKey(enemy.Hero);

            internal string <Initialize>b__30_2(TargetSelectorMode o) => 
                o.ToString();

            internal void <Initialize>b__30_4(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (args.NewValue)
                {
                    foreach (KeyValuePair<Champion, int> pair in TargetSelector.Priorities)
                    {
                        Slider introduced4 = TargetSelector.Menu[pair.Key.ToString()].Cast<Slider>();
                        introduced4.CurrentValue = pair.Value;
                    }
                    sender.CurrentValue = false;
                }
            }

            internal void <Initialize>b__30_5(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (TargetSelector.OnlySelectedTargetEnabled && TargetSelector.DrawNotifications)
                {
                    EloBuddy.SDK.Notifications.Notifications.Show(args.NewValue ? new SimpleNotification("Target Selector", "Only attack selected target enabled.") : new SimpleNotification("Target Selector", "Only attack selected target disabled."), 0x1388);
                }
            }

            internal void <Initialize>b__30_6(Messages.LeftButtonDown args)
            {
                if ((!MenuGUI.IsChatOpen && !MainMenu.IsMouseInside) && TargetSelector.SelectedEnabled)
                {
                    AIHeroClient client = EntityManager.Heroes.Enemies.FirstOrDefault<AIHeroClient>(o => o.IsValidTarget(null, false, null) && ((Obj_AI_Base) o).IsInRange(Game.ActiveCursorPos, 100f));
                    if (TargetSelector.DrawNotifications)
                    {
                        if (client > null)
                        {
                            EloBuddy.SDK.Notifications.Notifications.Show(new SimpleNotification("Target Selector", "Selected " + client.ChampionName + " as target."), 0x1388);
                        }
                        else if (TargetSelector.SelectedTarget > null)
                        {
                            EloBuddy.SDK.Notifications.Notifications.Show(new SimpleNotification("Target Selector", "Unselected " + TargetSelector.SelectedTarget.ChampionName + " as target."), 0x1388);
                        }
                    }
                    TargetSelector.SelectedTarget = client;
                }
            }

            internal bool <Initialize>b__30_7(AIHeroClient o) => 
                (o.IsValidTarget(null, false, null) && ((Obj_AI_Base) o).IsInRange(Game.ActiveCursorPos, 100f));

            internal void <Initialize>b__30_8(EventArgs <args>)
            {
                if ((TargetSelector.SelectedEnabled && TargetSelector.DrawCircleAroundSelected) && TargetSelector.SelectedTarget.IsValidTarget(null, false, null))
                {
                    Vector3[] positions = new Vector3[] { TargetSelector.SelectedTarget.Position };
                    EloBuddy.SDK.Rendering.Circle.Draw(SharpDX.Color.Red, TargetSelector.OnlySelectedTarget ? ((float) 120) : ((float) 80), TargetSelector.OnlySelectedTarget ? ((float) 15) : ((float) 5), positions);
                }
            }
        }
    }
}

