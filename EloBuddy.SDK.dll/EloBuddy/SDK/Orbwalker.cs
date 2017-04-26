namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Constants;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Notifications;
    using EloBuddy.SDK.Rendering;
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

    public static class Orbwalker
    {
        private static bool _autoAttackCompleted;
        private static bool _autoAttackStarted;
        internal static readonly Dictionary<int, Obj_AI_Minion> _azirSoldiers;
        private static int? _customMovementDelay;
        private static bool? _customSupportMode;
        internal static bool _disableAttacking;
        internal static bool _disableMovement;
        private static int _lastAutoAttackSent;
        private static float _lastCastEndTime;
        private static Vector3? _lastIssueOrderEndVector;
        private static Vector3? _lastIssueOrderStartVector;
        private static int? _lastIssueOrderTargetId;
        private static GameObjectOrder? _lastIssueOrderType;
        private static int _lastShouldWait;
        private static bool _onlyLastHit;
        private static PrecalculatedAutoAttackDamage _precalculatedDamage;
        internal static readonly Dictionary<int, Obj_AI_Minion> _validAzirSoldiers;
        internal static bool _waitingForAutoAttackReset;
        internal static bool _waitingPostAttackEvent;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static ActiveModes <ActiveModesFlags>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EloBuddy.SDK.Menu.Menu <AdvancedMenu>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Dictionary<int, bool> <AzirSoldierPreDashStatus>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EloBuddy.SDK.Menu.Menu <DrawingsMenu>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EloBuddy.SDK.Menu.Menu <FarmingMenu>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static AttackableUnit <ForcedTarget>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <GotAutoAttackReset>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IllaoiGhost>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static int <LastAutoAttack>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static int <LastMovementSent>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static AttackableUnit <LastTarget>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EloBuddy.SDK.Menu.Menu <Menu>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static OrbwalkPositionDelegate <OverrideOrbwalkPosition>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static System.Random <Random>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static int <RandomOffset>k__BackingField;
        public static readonly Dictionary<Champion, string> AllowedMovementBuffs;
        public const float AzirSoldierAutoAttackRange = 275f;
        internal static readonly Dictionary<TargetMinionType, Obj_AI_Minion> CurrentMinions;
        internal static readonly Dictionary<TargetMinionType, List<Obj_AI_Minion>> CurrentMinionsLists;
        internal static readonly Dictionary<int, CalculatedMinionValue> CurrentMinionValues;
        private static readonly Dictionary<int, float> DamageOnMinions;
        internal static readonly ColorBGRA EnemyRangeColorInRange;
        internal static readonly ColorBGRA EnemyRangeColorNotInRange;
        internal static readonly List<AttackableUnit> EnemyStructures;
        internal static readonly Dictionary<int, Obj_AI_Base> LastTargetTurrets;
        private const int ShouldWaitTime = 400;
        internal static bool SupportModeNotificationShown;
        internal static readonly List<Obj_AI_Minion> TickCachedMinions;
        internal static readonly List<Obj_AI_Minion> TickCachedMonsters;
        internal const int TurretRangeSqr = 0xa8304;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event AttackHandler OnAttack;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event OnAutoAttackResetHandler OnAutoAttackReset;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event PostAttackHandler OnPostAttack;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event PreAttackHandler OnPreAttack;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event PreMoveHandler OnPreMove;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event UnkillableMinionHandler OnUnkillableMinion;

        static Orbwalker()
        {
            Dictionary<Champion, string> dictionary1 = new Dictionary<Champion, string> {
                { 
                    Champion.Lucian,
                    "LucianR"
                },
                { 
                    Champion.Varus,
                    "VarusQ"
                },
                { 
                    Champion.Galio,
                    "GalioW"
                },
                { 
                    Champion.Vi,
                    "ViQ"
                },
                { 
                    Champion.Vladimir,
                    "VladimirE"
                },
                { 
                    Champion.Xerath,
                    "XerathArcanopulseChargeUp"
                }
            };
            AllowedMovementBuffs = dictionary1;
            LastTargetTurrets = new Dictionary<int, Obj_AI_Base>();
            EnemyRangeColorNotInRange = new ColorBGRA(0x90, 0xee, 0x90, 100);
            EnemyRangeColorInRange = new ColorBGRA(0xff, 0, 0, 100);
            _azirSoldiers = new Dictionary<int, Obj_AI_Minion>();
            _validAzirSoldiers = new Dictionary<int, Obj_AI_Minion>();
            EnemyStructures = new List<AttackableUnit>();
            TickCachedMinions = new List<Obj_AI_Minion>();
            TickCachedMonsters = new List<Obj_AI_Minion>();
            DamageOnMinions = new Dictionary<int, float>();
            CurrentMinionsLists = new Dictionary<TargetMinionType, List<Obj_AI_Minion>>();
            Dictionary<TargetMinionType, Obj_AI_Minion> dictionary2 = new Dictionary<TargetMinionType, Obj_AI_Minion> {
                { 
                    TargetMinionType.LaneClear,
                    null
                },
                { 
                    TargetMinionType.LastHit,
                    null
                },
                { 
                    TargetMinionType.PriorityLastHitWaiting,
                    null
                }
            };
            CurrentMinions = dictionary2;
            CurrentMinionValues = new Dictionary<int, CalculatedMinionValue>();
        }

        internal static void _OnAttack(GameObjectProcessSpellCastEventArgs args)
        {
            CanAutoAttack = false;
            AttackableUnit target = (args != null) ? (args.Target as AttackableUnit) : LastTarget;
            if (target > null)
            {
                LastTarget = target;
                TriggerAttackEvent(target, EventArgs.Empty);
            }
        }

        internal static void Clear()
        {
            if ((LastTarget != null) && !LastTarget.IsValidTarget(null, false, null))
            {
                LastTarget = null;
            }
            if (!CanMove && (LastTarget == null))
            {
                _autoAttackCompleted = true;
            }
            foreach (TargetMinionType type in Enum.GetValues(typeof(TargetMinionType)).Cast<TargetMinionType>())
            {
                CurrentMinionsLists[type] = new List<Obj_AI_Minion>();
            }
            foreach (KeyValuePair<TargetMinionType, Obj_AI_Minion> pair in (from entry in CurrentMinions
                where !entry.Value.IsValidTarget(null, false, null) || !Player.Instance.IsInAutoAttackRange(entry.Value)
                select entry).ToArray<KeyValuePair<TargetMinionType, Obj_AI_Minion>>())
            {
                CurrentMinions[pair.Key] = null;
            }
            _precalculatedDamage = null;
            TickCachedMonsters.Clear();
            TickCachedMinions.Clear();
            CurrentMinionValues.Clear();
            DamageOnMinions.Clear();
        }

        internal static void CreateMenu()
        {
            Menu = MainMenu.AddMenu("Orbwalker", "Orbwalker", null);
            Menu.AddGroupLabel("Hotkeys");
            RegisterKeyBind(Menu.Add<KeyBind>("combo", new KeyBind("Combo", false, KeyBind.BindTypes.HoldActive, 0x20, 0x1b)), ActiveModes.Combo);
            RegisterKeyBind(Menu.Add<KeyBind>("harass", new KeyBind("Harass", false, KeyBind.BindTypes.HoldActive, 0x43, 0x1b)), ActiveModes.Harass);
            RegisterKeyBind(Menu.Add<KeyBind>("laneclear", new KeyBind("LaneClear", false, KeyBind.BindTypes.HoldActive, 0x56, 0x1b)), ActiveModes.LaneClear);
            RegisterKeyBind(Menu.Add<KeyBind>("jungleclear", new KeyBind("JungleClear", false, KeyBind.BindTypes.HoldActive, 0x56, 0x1b)), ActiveModes.JungleClear);
            RegisterKeyBind(Menu.Add<KeyBind>("lasthit", new KeyBind("LastHit", false, KeyBind.BindTypes.HoldActive, 0x58, 0x1b)), ActiveModes.LastHit);
            RegisterKeyBind(Menu.Add<KeyBind>("flee", new KeyBind("Flee", false, KeyBind.BindTypes.HoldActive, 0x54, 0x1b)), ActiveModes.Flee);
            RegisterKeyBind(Menu.Add<KeyBind>("jungleplantsclear", new KeyBind("Attack Jungle Plants", false, KeyBind.BindTypes.HoldActive, 90, 0x1b)), ActiveModes.JunglePlantsClear);
            Menu.AddGroupLabel("Extra settings");
            Menu.Add<CheckBox>("attackObjects", new CheckBox("Attack other objects", true));
            Menu.Add<CheckBox>("lasthitbarrel", new CheckBox("Last Hit Gangplank Barrels", true));
            Menu.Add<CheckBox>("laneClearChamps", new CheckBox("Attack champions in LaneClear mode", true));
            Menu.Add<CheckBox>("stickToTarget", new CheckBox("Stick to target (only melee)", false));
            Menu.Add<CheckBox>("fastKiting", new CheckBox("Fast kiting", true));
            HashSet<Champion> set = new HashSet<Champion> {
                Champion.Alistar,
                Champion.Bard,
                Champion.Braum,
                Champion.Janna,
                Champion.Karma,
                Champion.Leona,
                Champion.Lulu,
                Champion.Morgana,
                Champion.Nami,
                Champion.Sona,
                Champion.Soraka,
                Champion.TahmKench,
                Champion.Taric,
                Champion.Thresh,
                Champion.Zilean,
                Champion.Zyra
            };
            Menu.Add<CheckBox>("supportMode" + Player.Instance.ChampionName, new CheckBox("Support Mode", set.Contains(Player.Instance.Hero)));
            Menu.Add<CheckBox>("checkYasuoWall", new CheckBox("Don't attack Yasuo's WindWall", true));
            Menu.Add<Slider>("holdRadius" + Player.Instance.ChampionName, new Slider("Hold radius", 100, 0, Math.Max(100, (int) (Player.Instance.GetAutoAttackRange(null) / 2f))));
            Menu.Add<Slider>("delayMove", new Slider("Delay between movements in milliseconds", 220 + Random.Next(40), 0, 0x3e8));
            Menu.Add<Slider>("extraWindUpTime", new Slider("Extra windup time", 0x23, 0, 200));
            Menu.AddLabel("Tip: If your autoattack is getting cancelled too much,", 0x19);
            Menu.AddLabel("you can fix it by adding more extra windup time.", 0x19);
            FarmingMenu = Menu.AddSubMenu("Farming", "Farming " + Player.Instance.ChampionName, null);
            FarmingMenu.AddGroupLabel("Misc Settings");
            FarmingMenu.Add<CheckBox>("lastHitPriority", new CheckBox("Priorize LastHit over Harass", true));
            FarmingMenu.Add<CheckBox>("_freezePriority", new CheckBox("Priorize Freeze over Push", false));
            FarmingMenu.Add<Slider>("extraFarmDelay", new Slider("Extra farm delay", 0, -80, 80));
            FarmingMenu.AddGroupLabel("Masteries Settings");
            FarmingMenu.Add<CheckBox>("doubleEdgedSword", new CheckBox("Double-Edged Sword (Ferocity Tree)", false));
            FarmingMenu.Add<CheckBox>("assassin", new CheckBox("Assassin (Cunning Tree)", false));
            FarmingMenu.Add<Slider>("savagery", new Slider("Savagery (Cunning Tree)", 0, 0, 5));
            FarmingMenu.Add<Slider>("merciless", new Slider("Merciless (Cunning Tree)", 0, 0, 5));
            FarmingMenu.AddGroupLabel("Item Settings");
            FarmingMenu.Add<CheckBox>("useTiamat", new CheckBox("Use Tiamat/Hydra on unkillable minions", true));
            DrawingsMenu = Menu.AddSubMenu("Drawings", null, null);
            DrawingsMenu.AddGroupLabel("Drawings");
            DrawingsMenu.Add<CheckBox>("drawrange", new CheckBox("Auto attack range", true));
            if (Player.Instance.Hero == Champion.Azir)
            {
                DrawingsMenu.Add<CheckBox>("drawAzirRange", new CheckBox("Azir soldier attack range", true));
            }
            DrawingsMenu.Add<CheckBox>("_drawEnemyRange", new CheckBox("Enemy auto attack range", true));
            DrawingsMenu.Add<CheckBox>("drawHoldRadius", new CheckBox("Hold radius (see main menu)", false));
            DrawingsMenu.Add<CheckBox>("drawLasthit", new CheckBox("Lasthittable minions", true));
            DrawingsMenu.Add<CheckBox>("drawTarget", new CheckBox("Orbwalker Target", true));
            DrawingsMenu.Add<CheckBox>("drawDamage", new CheckBox("Damage on minions", true));
            AdvancedMenu = Menu.AddSubMenu("Advanced", null, null);
            AdvancedMenu.AddGroupLabel("Orbwalker control");
            AdvancedMenu.Add<CheckBox>("disableAttacking", new CheckBox("Disable auto attacking", false));
            AdvancedMenu.Add<CheckBox>("disableMovement", new CheckBox("Disable moving to mouse", false));
            AdvancedMenu.AddGroupLabel("Update event listening");
            CheckBox useTick = new CheckBox("Use Game.OnTick (more fps)", true);
            CheckBox useUpdate = new CheckBox("Update Game.OnUpdate (faster reaction)", false);
            useTick.OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args) {
                if (args.NewValue)
                {
                    useUpdate.CurrentValue = false;
                }
                else if (!useUpdate.CurrentValue)
                {
                    useTick.CurrentValue = true;
                }
            };
            useUpdate.OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args) {
                if (args.NewValue)
                {
                    useTick.CurrentValue = false;
                }
                else if (!useTick.CurrentValue)
                {
                    useUpdate.CurrentValue = true;
                }
            };
            AdvancedMenu.Add<CheckBox>("useTick", useTick);
            AdvancedMenu.Add<CheckBox>("useUpdate", useUpdate);
        }

        private static int GetAttackCastDelay(AttackableUnit target)
        {
            if (Player.Instance.Hero == Champion.Azir)
            {
                Obj_AI_Minion minion = AzirSoldiers.FirstOrDefault<Obj_AI_Minion>(i => i.IsInAutoAttackRange(target));
                if (minion > null)
                {
                    return (int) (minion.AttackCastDelay * 1000f);
                }
            }
            return (int) (AttackCastDelay * 1000f);
        }

        private static int GetAttackDelay(AttackableUnit target)
        {
            if (Player.Instance.Hero == Champion.Azir)
            {
                Obj_AI_Minion minion = AzirSoldiers.FirstOrDefault<Obj_AI_Minion>(i => i.IsInAutoAttackRange(target));
                if (minion > null)
                {
                    return (int) (minion.AttackDelay * 1000f);
                }
            }
            return (int) (AttackDelay * 1000f);
        }

        private static float GetAutoAttackDamage(Obj_AI_Minion minion)
        {
            if (_precalculatedDamage == null)
            {
                _precalculatedDamage = Player.Instance.GetStaticAutoAttackDamage(true);
            }
            if (!DamageOnMinions.ContainsKey(minion.NetworkId))
            {
                DamageOnMinions[minion.NetworkId] = Player.Instance.GetAutoAttackDamage(minion, _precalculatedDamage);
            }
            return DamageOnMinions[minion.NetworkId];
        }

        internal static AttackableUnit GetIllaoiGhost()
        {
            if (!IllaoiGhost)
            {
                return null;
            }
            return ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault<Obj_AI_Minion>(o => (((o.IsValidTarget(null, false, null) && o.IsEnemy) && o.HasBuff("illaoiespirit")) && Player.Instance.IsInAutoAttackRange(o)));
        }

        private static int GetMissileTravelTime(Obj_AI_Base minion) => 
            (IsMelee ? 0 : ((int) Math.Max((float) 0f, (float) (1000f * Prediction.Health.GetPredictedMissileTravelTime(minion, Player.Instance.ServerPosition, AttackCastDelay, Player.Instance.BasicAttack.MissileSpeed)))));

        public static AttackableUnit GetTarget()
        {
            if ((ForcedTarget != null) && ForcedTarget.IsValidTarget(null, false, null))
            {
                return (Player.Instance.IsInAutoAttackRange(ForcedTarget) ? ForcedTarget : null);
            }
            if (ActiveModesFlags == ActiveModes.None)
            {
                return null;
            }
            List<AttackableUnit> source = new List<AttackableUnit>();
            foreach (ActiveModes modes in from mode in Enum.GetValues(typeof(ActiveModes)).Cast<ActiveModes>()
                where (mode != ActiveModes.None) && ActiveModesFlags.HasFlag(mode)
                select mode)
            {
                AttackableUnit target;
                AttackableUnit unit3;
                switch (modes)
                {
                    case ActiveModes.LaneClear:
                    {
                        target = GetTarget(TargetTypes.Structure);
                        unit3 = GetTarget(TargetTypes.LaneMinion);
                        if (target <= null)
                        {
                            goto Label_0288;
                        }
                        if (!LastHitPriority)
                        {
                            source.Add(target);
                        }
                        if (unit3.IdEquals(LastHitMinion))
                        {
                            source.Add(unit3);
                        }
                        if (LastHitPriority && !ShouldWait)
                        {
                            source.Add(target);
                        }
                        continue;
                    }
                    case ActiveModes.JunglePlantsClear:
                    {
                        source.Add(GetTarget(TargetTypes.JunglePlant));
                        continue;
                    }
                    case ActiveModes.Combo:
                    {
                        source.Add(GetTarget(TargetTypes.Hero));
                        continue;
                    }
                    case ActiveModes.Harass:
                    {
                        target = GetTarget(TargetTypes.Structure);
                        if (target <= null)
                        {
                            break;
                        }
                        if (!LastHitPriority)
                        {
                            source.Add(target);
                        }
                        source.Add(GetTarget(TargetTypes.LaneMinion));
                        if (LastHitPriority && !ShouldWait)
                        {
                            source.Add(target);
                        }
                        continue;
                    }
                    case (ActiveModes.Harass | ActiveModes.Combo):
                    {
                        continue;
                    }
                    case ActiveModes.LastHit:
                    {
                        source.Add(GetTarget(TargetTypes.LaneMinion));
                        continue;
                    }
                    case ActiveModes.JungleClear:
                    {
                        source.Add(GetTarget(TargetTypes.JungleMob));
                        continue;
                    }
                    default:
                    {
                        continue;
                    }
                }
                if (!LastHitPriority)
                {
                    source.Add(GetTarget(TargetTypes.Hero));
                }
                source.Add(GetTarget(TargetTypes.JungleMob) ?? GetTarget(TargetTypes.LaneMinion));
                if (LastHitPriority && !ShouldWait)
                {
                    source.Add(GetTarget(TargetTypes.Hero));
                }
                continue;
            Label_0288:
                if (!LastHitPriority && LaneClearAttackChamps)
                {
                    source.Add(GetTarget(TargetTypes.Hero));
                }
                if (unit3.IdEquals(LastHitMinion))
                {
                    source.Add(unit3);
                }
                if ((LastHitPriority && LaneClearAttackChamps) && !ShouldWait)
                {
                    source.Add(GetTarget(TargetTypes.Hero));
                }
                if (unit3.IdEquals(LaneClearMinion))
                {
                    source.Add(unit3);
                }
            }
            source.RemoveAll(o => o == null);
            return source.FirstOrDefault<AttackableUnit>();
        }

        internal static AttackableUnit GetTarget(TargetTypes targetType)
        {
            switch (targetType)
            {
                case TargetTypes.Hero:
                {
                    if (Player.Instance.Hero != Champion.Azir)
                    {
                        break;
                    }
                    AIHeroClient target = TargetSelector.GetTarget(from h in EntityManager.Heroes.Enemies
                        where h.IsValidTarget(null, false, null) && ValidAzirSoldiers.Any<Obj_AI_Minion>(i => i.IsInAutoAttackRange(h))
                        select h, DamageType.Magical);
                    if (target <= null)
                    {
                        break;
                    }
                    return target;
                }
                case TargetTypes.JungleMob:
                    if (!_onlyLastHit)
                    {
                        return TickCachedMonsters.FirstOrDefault<Obj_AI_Minion>(m => Player.Instance.IsInAutoAttackRange(m));
                    }
                    return TickCachedMonsters.FirstOrDefault<Obj_AI_Minion>(m => ((GetAutoAttackDamage(m) >= Prediction.Health.GetPrediction(m, GetMissileTravelTime(m))) && Player.Instance.IsInAutoAttackRange(m)));

                case TargetTypes.LaneMinion:
                {
                    bool flag = SupportMode && EntityManager.Heroes.Allies.Any<AIHeroClient>(i => (!i.IsMe && i.IsValidTarget(new float?((float) 0x41a), false, null)));
                    if (flag && !SupportModeNotificationShown)
                    {
                        EloBuddy.SDK.Notifications.Notifications.Show(new SimpleNotification("Orbwalker", "Support mode is enabled"), 0x1388);
                        SupportModeNotificationShown = true;
                    }
                    bool flag2 = !flag || (Player.Instance.GetBuffCount("TalentReaper") > 0);
                    if (!flag2 || (LastHitMinion <= null))
                    {
                        if (ShouldWait || _onlyLastHit)
                        {
                            return null;
                        }
                        return (!flag ? LaneClearMinion : null);
                    }
                    if (((PriorityLastHitWaitingMinion != null) && !PriorityLastHitWaitingMinion.IdEquals(LastHitMinion)) && PriorityLastHitWaitingMinion.IsSiegeMinion())
                    {
                        return null;
                    }
                    return LastHitMinion;
                }
                case TargetTypes.Structure:
                    return (from o in EnemyStructures
                        where ((o.IsValid && !o.IsDead) && o.IsTargetable) && (((Obj_AI_Base) Player.Instance).Distance(((GameObject) o), true) <= Player.Instance.GetAutoAttackRange(o).Pow())
                        orderby o.MaxHealth descending
                        select o).FirstOrDefault<AttackableUnit>();

                case TargetTypes.JunglePlant:
                    return (from o in EntityManager.MinionsAndMonsters.JunglePlants
                        orderby ((Obj_AI_Base) o).Distance(Game.CursorPos, false)
                        select o).FirstOrDefault<Obj_AI_Minion>(p => (p.IsValidTarget(null, false, null) && (((Obj_AI_Base) Player.Instance).Distance(((Obj_AI_Base) p), true) <= Player.Instance.GetAutoAttackRange(p).Pow())));

                default:
                    return null;
            }
            return (TargetSelector.GetTarget(from h in EntityManager.Heroes.Enemies
                where (h.IsValidTarget(null, false, null) && Player.Instance.IsInAutoAttackRange(h)) && ((!IsRanged || !CheckYasuoWall) || (Prediction.Position.Collision.GetYasuoWallCollision(Player.Instance.ServerPosition, h.ServerPosition, true) == Vector3.Zero))
                select h, DamageType.Physical) ?? GetIllaoiGhost());
        }

        internal static bool HasTurretTargetting(this Obj_AI_Minion minion) => 
            LastTargetTurrets.Any<KeyValuePair<int, Obj_AI_Base>>(o => o.Value.IdEquals(minion));

        internal static void Initialize()
        {
            Random = new System.Random(DateTime.Now.Millisecond);
            if (Player.Instance.Hero == Champion.Azir)
            {
                foreach (Obj_AI_Minion minion in from o in ObjectManager.Get<Obj_AI_Minion>()
                    where ((o.IsValid && o.IsAlly) && (o.Name == "AzirSoldier")) && o.Buffs.Any<BuffInstance>(b => (((b.IsValid() && b.Caster.IsMe) && (b.Count == 1)) && (b.DisplayName == "azirwspawnsound")))
                    select o)
                {
                    _azirSoldiers[minion.NetworkId] = minion;
                    if (((Obj_AI_Base) Player.Instance).IsInRange((Obj_AI_Base) minion, 950f))
                    {
                        _validAzirSoldiers[minion.NetworkId] = minion;
                    }
                }
                AzirSoldierPreDashStatus = new Dictionary<int, bool>();
                Obj_AI_Base.OnPlayAnimation += delegate (Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args) {
                    Obj_AI_Minion soldier = sender as Obj_AI_Minion;
                    if (((soldier != null) && soldier.IsAlly) && (soldier.Name == "AzirSoldier"))
                    {
                        switch (args.Animation)
                        {
                            case "Inactive":
                                _validAzirSoldiers.Remove(soldier.NetworkId);
                                if (AzirSoldierPreDashStatus.ContainsKey(soldier.NetworkId))
                                {
                                    AzirSoldierPreDashStatus[soldier.NetworkId] = false;
                                }
                                break;

                            case "Reactivate":
                                _validAzirSoldiers[soldier.NetworkId] = soldier;
                                if (AzirSoldierPreDashStatus.ContainsKey(soldier.NetworkId))
                                {
                                    AzirSoldierPreDashStatus[soldier.NetworkId] = true;
                                }
                                break;

                            case "Run":
                                if (!AzirSoldierPreDashStatus.ContainsKey(soldier.NetworkId))
                                {
                                    AzirSoldierPreDashStatus.Add(soldier.NetworkId, _validAzirSoldiers.Any<KeyValuePair<int, Obj_AI_Minion>>(o => o.Value.IdEquals(soldier)));
                                }
                                _validAzirSoldiers.Remove(soldier.NetworkId);
                                break;

                            case "Run_Exit":
                                if (!AzirSoldierPreDashStatus.ContainsKey(soldier.NetworkId) ? false : AzirSoldierPreDashStatus[soldier.NetworkId])
                                {
                                    _validAzirSoldiers[soldier.NetworkId] = soldier;
                                    AzirSoldierPreDashStatus.Remove(soldier.NetworkId);
                                }
                                break;

                            case "Death":
                                _azirSoldiers.Remove(soldier.NetworkId);
                                _validAzirSoldiers.Remove(soldier.NetworkId);
                                AzirSoldierPreDashStatus.Remove(soldier.NetworkId);
                                break;
                        }
                    }
                };
                Obj_AI_Base.OnBuffGain += delegate (Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args) {
                    Obj_AI_Minion minion = sender as Obj_AI_Minion;
                    if ((((minion != null) && minion.IsAlly) && ((minion.Name == "AzirSoldier") && args.Buff.Caster.IsMe)) && (args.Buff.DisplayName == "azirwspawnsound"))
                    {
                        _azirSoldiers[minion.NetworkId] = minion;
                        _validAzirSoldiers[minion.NetworkId] = minion;
                    }
                };
                Obj_AI_Base.OnBuffLose += delegate (Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args) {
                    Obj_AI_Minion minion = sender as Obj_AI_Minion;
                    if (((minion != null) && minion.IsAlly) && (minion.Name == "AzirSoldier"))
                    {
                        _azirSoldiers.Remove(minion.NetworkId);
                        _validAzirSoldiers.Remove(minion.NetworkId);
                        AzirSoldierPreDashStatus.Remove(minion.NetworkId);
                    }
                };
            }
            IllaoiGhost = EntityManager.Heroes.Allies.Any<AIHeroClient>(h => h.Hero == Champion.Illaoi);
            CreateMenu();
            EnemyStructures.AddRange(from o in ObjectManager.Get<AttackableUnit>()
                where o.IsEnemy && o.IsStructure()
                select o);
            Game.OnTick += delegate (EventArgs <args>) {
                if (UseOnTick)
                {
                    OnTick();
                }
            };
            Game.OnUpdate += delegate (EventArgs <args>) {
                if (UseOnUpdate)
                {
                    OnTick();
                }
                OnUpdate();
            };
            GameObject.OnCreate += new GameObjectCreate(Orbwalker.OnCreate);
            Obj_AI_Base.OnBasicAttack += new Obj_AI_BaseOnBasicAttack(Orbwalker.OnBasicAttack);
            Obj_AI_Base.OnProcessSpellCast += new Obj_AI_ProcessSpellCast(Orbwalker.OnProcessSpellCast);
            Obj_AI_Base.OnSpellCast += new Obj_AI_BaseDoCastSpell(Orbwalker.OnSpellCast);
            Spellbook.OnStopCast += new SpellbookStopCast(Orbwalker.OnStopCast);
            Drawing.OnDraw += new DrawingDraw(Orbwalker.OnDraw);
            if (AutoAttacks.SpecialAutoAttacksAnimationName.ContainsKey(Player.Instance.Hero))
            {
                Obj_AI_Base.OnPlayAnimation += delegate (Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args) {
                    if (sender.IsMe && AutoAttacks.SpecialAutoAttacksAnimationName[Player.Instance.Hero].Contains(args.Animation))
                    {
                        _OnAttack(null);
                    }
                };
            }
            if (AutoAttacks.DashAutoAttackResetSlotsDatabase.ContainsKey(Player.Instance.Hero))
            {
                Vector3 dashEndPosition = new Vector3();
                if (AutoAttacks.AutoAttackResetAnimationsName.ContainsKey(Player.Instance.Hero))
                {
                    Obj_AI_Base.OnPlayAnimation += delegate (Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args) {
                        if (sender.IsMe && AutoAttacks.IsDashAutoAttackReset(Player.Instance, args))
                        {
                            GotAutoAttackReset = true;
                            _waitingForAutoAttackReset = true;
                        }
                    };
                }
                Obj_AI_Base.OnProcessSpellCast += delegate (Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args) {
                    if (sender.IsMe && AutoAttacks.IsDashAutoAttackReset(Player.Instance, args))
                    {
                        GotAutoAttackReset = true;
                        _waitingForAutoAttackReset = true;
                    }
                };
                Game.OnUpdate += delegate (EventArgs <args>) {
                    if (((dashEndPosition != new Vector3()) && _waitingForAutoAttackReset) && (((Vector3.Distance(Player.Instance.Position, dashEndPosition) <= Player.Instance.BoundingRadius) || dashEndPosition.IsWall()) || dashEndPosition.IsBuilding()))
                    {
                        _waitingForAutoAttackReset = false;
                        Core.DelayAction(delegate {
                            ResetAutoAttack();
                            dashEndPosition = new Vector3();
                        }, 30);
                    }
                };
                Obj_AI_Base.OnNewPath += delegate (Obj_AI_Base sender, GameObjectNewPathEventArgs args) {
                    if (sender.IsMe)
                    {
                        if (args.IsDash)
                        {
                            if (_waitingForAutoAttackReset)
                            {
                                dashEndPosition = args.Path.LastOrDefault<Vector3>();
                            }
                        }
                        else if (_waitingForAutoAttackReset)
                        {
                            ResetAutoAttack();
                            _waitingForAutoAttackReset = false;
                            dashEndPosition = new Vector3();
                        }
                    }
                };
            }
            Vector2 minionBarOffset = new Vector2(36f, 3f);
            int barHeight = 6;
            int barWidth = 0x3e;
            Drawing.OnEndScene += delegate (EventArgs <args>) {
                if (DrawDamageMarker)
                {
                    foreach (Obj_AI_Minion minion in from o in TickCachedMinions
                        where DamageOnMinions.ContainsKey(o.NetworkId) && o.VisibleOnScreen
                        select o)
                    {
                        Vector2 vector = (minionBarOffset + minion.HPBarPosition) + new Vector2(barWidth * Math.Min((float) (GetAutoAttackDamage(minion) / minion.MaxHealth), (float) 1f), 0f);
                        Vector2[] screenVertices = new Vector2[] { vector, vector + new Vector2(0f, (float) barHeight) };
                        EloBuddy.SDK.Rendering.Line.DrawLine(System.Drawing.Color.Black, 2f, screenVertices);
                    }
                }
            };
            if (Player.Instance.IsMelee)
            {
                OnUnkillableMinion += delegate (Obj_AI_Base target, UnkillableMinionArgs args) {
                    if ((ActiveModesFlags.HasFlag(ActiveModes.LastHit) || ActiveModesFlags.HasFlag(ActiveModes.LaneClear)) && (UseTiamat && (((Obj_AI_Base) Player.Instance).Distance(target, true) <= 160000f)))
                    {
                        float num = Player.Instance.GetItemDamage(target, ItemId.Tiamat);
                        if (Prediction.Health.GetPrediction(target, 200) <= num)
                        {
                            ItemId[] source = new ItemId[] { ItemId.Tiamat, ItemId.Ravenous_Hydra };
                            foreach (ItemId id in source.Where<ItemId>(new Func<ItemId, bool>(Item.CanUseItem)))
                            {
                                Item.UseItem(id, (Obj_AI_Base) null);
                            }
                        }
                    }
                };
            }
            GameObject.OnDelete += delegate (GameObject sender, EventArgs args) {
                if (sender.IsStructure())
                {
                    EnemyStructures.RemoveAll(o => o.IdEquals(sender));
                }
                if (sender.IdEquals(LastHitMinion))
                {
                    LastHitMinion = null;
                }
                if (sender.IdEquals(PriorityLastHitWaitingMinion))
                {
                    PriorityLastHitWaitingMinion = null;
                }
                if (sender.IdEquals(LaneClearMinion))
                {
                    LaneClearMinion = null;
                }
                if (LastTarget.IdEquals(sender))
                {
                    LastTarget = null;
                }
                if (ForcedTarget.IdEquals(sender))
                {
                    ForcedTarget = null;
                }
            };
            Player.OnPostIssueOrder += delegate (Obj_AI_Base sender, PlayerIssueOrderEventArgs args) {
                if (sender.IsMe)
                {
                    _lastIssueOrderStartVector = new Vector3?(sender.Position);
                    _lastIssueOrderEndVector = new Vector3?(args.TargetPosition);
                    _lastIssueOrderType = new GameObjectOrder?(args.Order);
                    _lastIssueOrderTargetId = new int?((args.Target != null) ? args.Target.NetworkId : 0);
                }
            };
        }

        public static bool ModeIsActive(params ActiveModes[] modes) => 
            modes.Any<ActiveModes>(m => ActiveModesFlags.HasFlag(m));

        public static void MoveTo(Vector3 position)
        {
            if ((CanMove && (((Core.GameTickCount - LastMovementSent) + RandomOffset) > MovementDelay)) && (FastKiting || (((Core.GameTickCount - _lastAutoAttackSent) + RandomOffset) > MovementDelay)))
            {
                GameObjectOrder moveTo;
                GameObjectOrder? nullable;
                Vector3 vector = (((Obj_AI_Base) Player.Instance).Distance(position, true) < 100.Pow()) ? Player.Instance.Position.Extend(position, 100f).To3DWorld() : position;
                if (HoldRadius > 0)
                {
                    if (position.Distance(((Obj_AI_Base) Player.Instance), true) > HoldRadius.Pow())
                    {
                        moveTo = GameObjectOrder.MoveTo;
                    }
                    else
                    {
                        moveTo = GameObjectOrder.Stop;
                    }
                }
                else
                {
                    moveTo = GameObjectOrder.MoveTo;
                }
                if (_lastIssueOrderType.HasValue && ((((GameObjectOrder) (nullable = _lastIssueOrderType).GetValueOrDefault()) == moveTo) ? nullable.HasValue : false))
                {
                    switch (moveTo)
                    {
                        case GameObjectOrder.MoveTo:
                            if ((_lastIssueOrderEndVector.HasValue && (_lastIssueOrderEndVector.Value == vector)) && Player.Instance.IsMoving)
                            {
                                return;
                            }
                            break;

                        case GameObjectOrder.Stop:
                            if (_lastIssueOrderStartVector.HasValue && (_lastIssueOrderStartVector.Value == Player.Instance.Position))
                            {
                            }
                            return;
                    }
                }
                PreMoveArgs args = new PreMoveArgs(vector);
                if (args.Process && Player.IssueOrder(moveTo, vector))
                {
                    LastMovementSent = Core.GameTickCount;
                    RandomOffset = Random.Next(30) - 15;
                }
            }
        }

        internal static void NotifyEventListeners(string eventName, Delegate[] invocationList, params object[] args)
        {
            foreach (Delegate delegate2 in invocationList)
            {
                try
                {
                    delegate2.DynamicInvoke(args);
                }
                catch (Exception exception)
                {
                    object[] objArray1 = new object[] { eventName };
                    Logger.Exception("Failed to notify Orbwalker.{0} event listener!", exception, objArray1);
                }
            }
        }

        internal static void OnBasicAttack(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                _OnAttack(args);
            }
            else if ((sender is Obj_AI_Turret) && (args.Target is Obj_AI_Base))
            {
                LastTargetTurrets[sender.NetworkId] = (Obj_AI_Base) args.Target;
            }
        }

        internal static void OnCreate(GameObject sender, EventArgs args)
        {
            MissileClient missile = sender as MissileClient;
            if (missile > null)
            {
                Obj_AI_Base spellCaster = missile.SpellCaster;
                if (((spellCaster != null) && spellCaster.IsMe) && missile.IsAutoAttack())
                {
                    _autoAttackCompleted = true;
                    TriggerOnPostAttack();
                }
            }
        }

        internal static void OnDraw(EventArgs args)
        {
            if (DrawRange)
            {
                GameObject[] objects = new GameObject[] { Player.Instance };
                EloBuddy.SDK.Rendering.Circle.Draw(SharpDX.Color.LightGreen, Player.Instance.GetAutoAttackRange(null), objects);
            }
            if ((Player.Instance.Hero == Champion.Azir) && DrawAzirRange)
            {
                foreach (Obj_AI_Minion minion in _validAzirSoldiers.Values)
                {
                    GameObject[] objArray2 = new GameObject[] { minion };
                    EloBuddy.SDK.Rendering.Circle.Draw(SharpDX.Color.LightGreen, minion.GetAutoAttackRange(null), objArray2);
                }
            }
            if (DrawHoldRadius)
            {
                GameObject[] objArray3 = new GameObject[] { Player.Instance };
                EloBuddy.SDK.Rendering.Circle.Draw(SharpDX.Color.LightGreen, (float) HoldRadius, objArray3);
            }
            if (DrawEnemyRange)
            {
                foreach (AIHeroClient client in from o in EntityManager.Heroes.Enemies
                    where o.IsValidTarget(null, false, null) && o.ServerPosition.IsOnScreen()
                    select o)
                {
                    float autoAttackRange = client.GetAutoAttackRange(Player.Instance);
                    GameObject[] objArray4 = new GameObject[] { client };
                    EloBuddy.SDK.Rendering.Circle.Draw(((Obj_AI_Base) client).IsInRange(((Obj_AI_Base) Player.Instance), autoAttackRange) ? EnemyRangeColorInRange : EnemyRangeColorNotInRange, autoAttackRange, objArray4);
                }
            }
            if (DrawLastHitMarker)
            {
                if ((LastHitMinion != null) && LastHitMinion.ServerPosition.IsOnScreen())
                {
                    GameObject[] objArray5 = new GameObject[] { LastHitMinion };
                    EloBuddy.SDK.Rendering.Circle.Draw(SharpDX.Color.White, Math.Max(LastHitMinion.BoundingRadius, 65f), 2f, objArray5);
                }
                if (((PriorityLastHitWaitingMinion != null) && !PriorityLastHitWaitingMinion.IdEquals(LastHitMinion)) && PriorityLastHitWaitingMinion.ServerPosition.IsOnScreen())
                {
                    GameObject[] objArray6 = new GameObject[] { PriorityLastHitWaitingMinion };
                    EloBuddy.SDK.Rendering.Circle.Draw(SharpDX.Color.Orange, Math.Max(PriorityLastHitWaitingMinion.BoundingRadius, 65f), 2f, objArray6);
                }
            }
            if ((DrawTargetMarker && LastTarget.IsValidTarget(new float?(Player.Instance.GetAutoAttackRange(LastTarget)), false, null)) && (ActiveModesFlags > ActiveModes.None))
            {
                GameObject[] objArray7 = new GameObject[] { LastTarget };
                EloBuddy.SDK.Rendering.Circle.Draw(SharpDX.Color.LimeGreen, Math.Max((float) (LastTarget.BoundingRadius * 1.25f), (float) 130f), 1f, objArray7);
            }
        }

        internal static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                _lastIssueOrderStartVector = null;
                _lastIssueOrderEndVector = null;
                _lastIssueOrderTargetId = null;
                _lastIssueOrderType = null;
                if (args.IsAutoAttack())
                {
                    _OnAttack(args);
                }
                else if (AutoAttacks.IsAutoAttackReset(Player.Instance, args) && ((Math.Abs(args.SData.CastTime) < float.Epsilon) || (args.SData.CastTime > 0.2f)))
                {
                    Core.DelayAction(new Action(Orbwalker.ResetAutoAttack), 30);
                }
            }
        }

        internal static void OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                if (args.IsAutoAttack())
                {
                    if (Game.Ping < 50)
                    {
                        Core.DelayAction(delegate {
                            _autoAttackCompleted = true;
                            TriggerOnPostAttack();
                        }, 50 - Game.Ping);
                    }
                    else
                    {
                        _autoAttackCompleted = true;
                        TriggerOnPostAttack();
                    }
                }
                else if (AutoAttacks.IsAutoAttackReset(Player.Instance, args))
                {
                    Core.DelayAction(new Action(Orbwalker.ResetAutoAttack), 30);
                }
            }
        }

        internal static void OnStopCast(Obj_AI_Base sender, SpellbookStopCastEventArgs args)
        {
            if (((sender.IsMe && (args.DestroyMissile || args.StopAnimation)) && (((_lastCastEndTime - Game.Time) > 0f) || (IsRanged && !_autoAttackCompleted))) && !args.ForceStop)
            {
                ResetAutoAttack();
            }
        }

        internal static void OnTick()
        {
            Clear();
            if (((ActiveModesFlags != ActiveModes.None) || DrawLastHitMarker) || DrawDamageMarker)
            {
                TickCachedMinions.AddRange(from o in EntityManager.MinionsAndMonsters.EnemyMinions
                    where ((Obj_AI_Base) Player.Instance).IsInRange((Obj_AI_Base) o, 1500f)
                    select o);
                _onlyLastHit = !ActiveModesFlags.HasFlag(ActiveModes.LaneClear) && !ActiveModesFlags.HasFlag(ActiveModes.JungleClear);
                if ((ActiveModesFlags != ActiveModes.None) || DrawLastHitMarker)
                {
                    RecalculateLasthittableMinions();
                }
                else if (DrawDamageMarker)
                {
                    foreach (Obj_AI_Minion minion in TickCachedMinions)
                    {
                        GetAutoAttackDamage(minion);
                    }
                }
            }
            if ((ActiveModesFlags > ActiveModes.None) && (((LastHitMinion == null) && !ShouldWait) && (LaneClearMinion == null)))
            {
                TickCachedMonsters.AddRange(from o in EntityManager.MinionsAndMonsters.Monsters.Where<Obj_AI_Minion>(new Func<Obj_AI_Minion, bool>(Extensions.IsInAutoAttackRange))
                    orderby o.MaxHealth descending
                    select o);
            }
        }

        internal static void OnUpdate()
        {
            if (Player.Instance.Spellbook.IsAutoAttacking)
            {
                float castEndTime = Player.Instance.Spellbook.CastEndTime;
                if (castEndTime > 0f)
                {
                    if (CanIssueOrder)
                    {
                    }
                    _lastCastEndTime = castEndTime;
                }
            }
            TriggerOnPostAttack();
            if (ActiveModesFlags > ActiveModes.None)
            {
                OrbwalkTo(OrbwalkPosition);
            }
        }

        public static void OrbwalkTo(Vector3 position)
        {
            if (!Chat.IsOpen)
            {
                if (!DisableAttacking && CanAutoAttack)
                {
                    AttackableUnit target = GetTarget();
                    if (target > null)
                    {
                        PreAttackArgs args = new PreAttackArgs(target);
                        if (args.Process)
                        {
                            GameObjectOrder? nullable;
                            GameObjectOrder attackUnit;
                            if (_lastIssueOrderType.HasValue)
                            {
                                nullable = _lastIssueOrderType;
                                attackUnit = GameObjectOrder.AttackUnit;
                            }
                            if ((((((((GameObjectOrder) nullable.GetValueOrDefault()) == attackUnit) ? nullable.HasValue : false) && (_lastAutoAttackSent > 0)) && (_lastIssueOrderTargetId.HasValue && (_lastIssueOrderTargetId.Value == args.Target.NetworkId))) && _lastIssueOrderStartVector.HasValue) && (_lastIssueOrderStartVector.Value == Player.Instance.Position))
                            {
                                _autoAttackStarted = false;
                                _lastAutoAttackSent = Core.GameTickCount;
                                LastTarget = args.Target;
                                return;
                            }
                            if (CanIssueAttack && Player.IssueOrder(GameObjectOrder.AttackUnit, args.Target))
                            {
                                _autoAttackStarted = false;
                                _lastAutoAttackSent = Core.GameTickCount;
                                LastTarget = args.Target;
                                return;
                            }
                        }
                    }
                }
                if (!DisableMovement)
                {
                    MoveTo(position);
                }
            }
        }

        internal static void RecalculateLasthittableMinions()
        {
            int num = !CanIssueOrder ? Math.Max(0, ((int) (AttackDelay * 1000f)) - (Core.GameTickCount - LastAutoAttack)) : 0;
            bool canMove = CanMove;
            int num2 = IsMelee ? ((int) 0f) : ((int) ((1000f * Player.Instance.GetAutoAttackRange(null)) / Player.Instance.BasicAttack.MissileSpeed));
            foreach (Obj_AI_Minion minion in TickCachedMinions)
            {
                int attackCastDelay = GetAttackCastDelay(minion);
                int missileTravelTime = GetMissileTravelTime(minion);
                CalculatedMinionValue value2 = new CalculatedMinionValue(minion) {
                    LastHitTime = ((attackCastDelay + missileTravelTime) + num) + Math.Max(0, (int) ((2000f * (((Obj_AI_Base) Player.Instance).Distance(((Obj_AI_Base) minion), false) - Player.Instance.GetAutoAttackRange(minion))) / Player.Instance.MoveSpeed)),
                    LaneClearTime = (GetAttackDelay(minion) + attackCastDelay) + num2
                };
                CurrentMinionValues[minion.NetworkId] = value2;
            }
            foreach (Prediction.Health.IncomingAttack attack in from i in Prediction.Health.IncomingAttacks select i.Value)
            {
                int networkId = attack.Target.NetworkId;
                if (CurrentMinionValues.ContainsKey(networkId))
                {
                    CalculatedMinionValue local1 = CurrentMinionValues[networkId];
                    local1.LastHitHealth -= attack.GetDamage(CurrentMinionValues[networkId].LastHitTime);
                    CalculatedMinionValue local2 = CurrentMinionValues[networkId];
                    local2.LaneClearHealth -= attack.GetDamage(CurrentMinionValues[networkId].LaneClearTime);
                }
            }
            foreach (KeyValuePair<int, CalculatedMinionValue> pair in CurrentMinionValues)
            {
                CalculatedMinionValue value3 = pair.Value;
                Obj_AI_Minion handle = value3.Handle;
                if (value3.IsUnkillable)
                {
                    if (!handle.IdEquals(LastTarget))
                    {
                        if ((OnUnkillableMinion > null) & canMove)
                        {
                            UnkillableMinionArgs args = new UnkillableMinionArgs {
                                RemainingHealth = value3.LastHitHealth
                            };
                            OnUnkillableMinion(handle, args);
                        }
                        CurrentMinionsLists[TargetMinionType.UnKillable].Add(handle);
                    }
                }
                else if (value3.IsLastHittable)
                {
                    CurrentMinionsLists[TargetMinionType.LastHit].Add(handle);
                }
                else if (value3.IsAlmostLastHittable)
                {
                    CurrentMinionsLists[TargetMinionType.PriorityLastHitWaiting].Add(handle);
                }
                else if (value3.IsLaneClearMinion)
                {
                    CurrentMinionsLists[TargetMinionType.LaneClear].Add(handle);
                }
            }
            SortMinionsAndDefineTargets();
            if ((AttackObjects && (LastHitMinion == null)) && !ShouldWait)
            {
                foreach (Obj_AI_Minion minion3 in from minion in EntityManager.MinionsAndMonsters.OtherEnemyMinions.Where<Obj_AI_Minion>(new Func<Obj_AI_Minion, bool>(Extensions.IsInAutoAttackRange))
                    orderby minion.MaxHealth descending, minion.Health
                    where minion.Health > 0f
                    select minion)
                {
                    if (LastHitBarrels)
                    {
                        BuffInstance instance = minion3.Buffs.FirstOrDefault<BuffInstance>(b => b.DisplayName.Equals("GangplankEBarrelActive"));
                        if (instance > null)
                        {
                            int num6 = GetMissileTravelTime(minion3);
                            int time = (num6 == 0) ? GetAttackCastDelay(minion3) : num6;
                            if (Prediction.Health.GetPrediction(minion3, time).Equals((float) 1f))
                            {
                                LastHitMinion = minion3;
                            }
                            else
                            {
                                AIHeroClient caster = instance.Caster as AIHeroClient;
                                if (caster > null)
                                {
                                    float num9 = instance.StartTime * 1000f;
                                    int num10 = (caster.Level < 7) ? 0xfa0 : ((caster.Level < 13) ? 0x7d0 : 0x3e8);
                                    float num11 = num9 + num10;
                                    if ((num11 - Core.GameTickCount) <= time)
                                    {
                                        LastHitMinion = minion3;
                                    }
                                }
                            }
                            break;
                        }
                    }
                    LastHitMinion = minion3;
                    break;
                }
            }
        }

        public static void RegisterKeyBind(KeyBind key, ActiveModes mode)
        {
            key.OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args) {
                if (args.NewValue)
                {
                    if (!ActiveModesFlags.HasFlag(mode))
                    {
                        ActiveModesFlags |= mode;
                    }
                }
                else if (ActiveModesFlags.HasFlag(mode))
                {
                    ActiveModesFlags ^= mode;
                }
            };
        }

        public static void ResetAutoAttack()
        {
            CanAutoAttack = true;
            GotAutoAttackReset = true;
            TriggerAutoAttackReset(null);
        }

        internal static void SortMinionsAndDefineTargets()
        {
            foreach (KeyValuePair<TargetMinionType, List<Obj_AI_Minion>> pair in CurrentMinionsLists.ToArray<KeyValuePair<TargetMinionType, List<Obj_AI_Minion>>>())
            {
                switch (pair.Key)
                {
                    case TargetMinionType.LastHit:
                        CurrentMinionsLists[pair.Key] = (from o in pair.Value
                            orderby o.MaxHealth descending, CurrentMinionValues[o.NetworkId].LastHitHealth
                            select o).ToList<Obj_AI_Minion>();
                        LastHitMinion = pair.Value.FirstOrDefault<Obj_AI_Minion>(new Func<Obj_AI_Minion, bool>(Extensions.IsInAutoAttackRange));
                        break;

                    case TargetMinionType.PriorityLastHitWaiting:
                        CurrentMinionsLists[pair.Key] = (from o in pair.Value
                            orderby o.MaxHealth descending, CurrentMinionValues[o.NetworkId].LaneClearHealth
                            select o).ToList<Obj_AI_Minion>();
                        PriorityLastHitWaitingMinion = pair.Value.FirstOrDefault<Obj_AI_Minion>(m => Player.Instance.IsInAutoAttackRange(m) && ((LastHitMinion == null) || !m.IdEquals(LastHitMinion)));
                        break;

                    case TargetMinionType.LaneClear:
                        if (!_onlyLastHit)
                        {
                            CurrentMinionsLists[pair.Key] = (from minion in pair.Value
                                orderby FreezePriority ? ((IEnumerable<Obj_AI_Minion>) CurrentMinionValues[minion.NetworkId].LaneClearHealth) : ((IEnumerable<Obj_AI_Minion>) (1f / CurrentMinionValues[minion.NetworkId].LaneClearHealth)) descending
                                select minion).ToList<Obj_AI_Minion>();
                            LaneClearMinion = pair.Value.FirstOrDefault<Obj_AI_Minion>(new Func<Obj_AI_Minion, bool>(Extensions.IsInAutoAttackRange));
                        }
                        break;

                    case TargetMinionType.UnKillable:
                        CurrentMinionsLists[pair.Key] = (from o in pair.Value
                            orderby CurrentMinionValues[o.NetworkId].LastHitHealth
                            select o).ToList<Obj_AI_Minion>();
                        break;
                }
            }
        }

        internal static void TriggerAttackEvent(AttackableUnit target, EventArgs args = null)
        {
            if (OnAttack > null)
            {
                object[] objArray1 = new object[] { target, args ?? EventArgs.Empty };
                NotifyEventListeners("OnAttack", OnAttack.GetInvocationList(), objArray1);
            }
        }

        internal static void TriggerAutoAttackReset(EventArgs args = null)
        {
            if (OnAutoAttackReset > null)
            {
                object[] objArray1 = new object[] { args };
                NotifyEventListeners("OnAutoAttackReset", OnAutoAttackReset.GetInvocationList(), objArray1);
            }
        }

        internal static void TriggerOnPostAttack()
        {
            if (_waitingPostAttackEvent && CanBeAborted)
            {
                TriggerPostAttackEvent(LastTarget, EventArgs.Empty);
                GotAutoAttackReset = false;
                _waitingPostAttackEvent = false;
            }
        }

        internal static void TriggerPostAttackEvent(AttackableUnit target, EventArgs args = null)
        {
            if (OnPostAttack > null)
            {
                object[] objArray1 = new object[] { target, args ?? EventArgs.Empty };
                NotifyEventListeners("OnPostAttack", OnPostAttack.GetInvocationList(), objArray1);
            }
        }

        internal static void TriggerPreAttackEvent(AttackableUnit target, PreAttackArgs args)
        {
            if (OnPreAttack > null)
            {
                object[] objArray1 = new object[] { target, args };
                NotifyEventListeners("OnPreAttack", OnPreAttack.GetInvocationList(), objArray1);
            }
        }

        internal static void TriggerPreMoveEvent(EventArgs args = null)
        {
            if (OnPreMove > null)
            {
                object[] objArray1 = new object[] { args };
                NotifyEventListeners("OnPreMove", OnPreMove.GetInvocationList(), objArray1);
            }
        }

        internal static void TriggerUnkillableMinionEvent(Obj_AI_Base target, UnkillableMinionArgs args)
        {
            if (OnUnkillableMinion > null)
            {
                object[] objArray1 = new object[] { target, args };
                NotifyEventListeners("OnUnkillableMinion", OnUnkillableMinion.GetInvocationList(), objArray1);
            }
        }

        public static ActiveModes ActiveModesFlags
        {
            [CompilerGenerated]
            get => 
                <ActiveModesFlags>k__BackingField;
            [CompilerGenerated]
            set
            {
                <ActiveModesFlags>k__BackingField = value;
            }
        }

        internal static EloBuddy.SDK.Menu.Menu AdvancedMenu
        {
            [CompilerGenerated]
            get => 
                <AdvancedMenu>k__BackingField;
            [CompilerGenerated]
            set
            {
                <AdvancedMenu>k__BackingField = value;
            }
        }

        public static float AttackCastDelay
        {
            get
            {
                if ((Player.Instance.Hero == Champion.TwistedFate) && ((Player.Instance.HasBuff("BlueCardPreAttack") || Player.Instance.HasBuff("RedCardPreAttack")) || Player.Instance.HasBuff("GoldCardPreAttack")))
                {
                    return 0.13f;
                }
                return Player.Instance.AttackCastDelay;
            }
        }

        public static float AttackDelay
        {
            get
            {
                if ((Player.Instance.Hero == Champion.Graves) && Player.Instance.HasBuff("GravesBasicAttackAmmo1"))
                {
                    return ((1.07403f * Player.Instance.AttackDelay) - 0.7162381f);
                }
                return Player.Instance.AttackDelay;
            }
        }

        internal static bool AttackObjects =>
            Menu["attackObjects"].Cast<CheckBox>().CurrentValue;

        internal static Dictionary<int, bool> AzirSoldierPreDashStatus
        {
            [CompilerGenerated]
            get => 
                <AzirSoldierPreDashStatus>k__BackingField;
            [CompilerGenerated]
            set
            {
                <AzirSoldierPreDashStatus>k__BackingField = value;
            }
        }

        public static List<Obj_AI_Minion> AzirSoldiers =>
            _azirSoldiers.Values.ToList<Obj_AI_Minion>();

        public static bool CanAutoAttack
        {
            get
            {
                if (!Player.Instance.CanAttack && !_waitingForAutoAttackReset)
                {
                    return false;
                }
                if (Player.Instance.Spellbook.IsChanneling)
                {
                    return false;
                }
                switch (Player.Instance.Hero)
                {
                    case Champion.Darius:
                        if (!Player.Instance.HasBuff("dariusqcast"))
                        {
                            break;
                        }
                        return false;

                    case Champion.Jhin:
                        if (!Player.Instance.HasBuff("JhinPassiveReload"))
                        {
                            break;
                        }
                        return false;

                    case Champion.Kalista:
                        if (Player.Instance.IsDashing())
                        {
                            return false;
                        }
                        break;
                }
                if ((Core.GameTickCount - _lastAutoAttackSent) <= (100 + Game.Ping))
                {
                    return false;
                }
                return CanIssueOrder;
            }
            internal set
            {
                if (value)
                {
                    _autoAttackStarted = false;
                    _autoAttackCompleted = true;
                    LastAutoAttack = 0;
                    LastMovementSent = 0;
                    _lastCastEndTime = 0f;
                    _lastAutoAttackSent = 0;
                }
                else
                {
                    _autoAttackStarted = true;
                    _autoAttackCompleted = false;
                    _waitingPostAttackEvent = true;
                    LastAutoAttack = Core.GameTickCount;
                    if (FastKiting)
                    {
                        LastMovementSent -= MovementDelay - RandomOffset;
                        _lastAutoAttackSent -= 100 + Game.Ping;
                    }
                }
            }
        }

        public static bool CanBeAborted
        {
            get
            {
                int extraWindUpTime = ExtraWindUpTime;
                if ((Player.Instance.Hero == Champion.Vayne) && Player.Instance.HasBuff("vaynetumblebonus"))
                {
                    extraWindUpTime += 150;
                }
                return (_autoAttackCompleted || (((Core.GameTickCount - LastAutoAttack) >= (((AttackCastDelay * 1000f) + extraWindUpTime) + (FastKiting ? -(Game.Ping * 0.65f) : (((float) Game.Ping) / 10f)))) || (IsMelee && ((_lastCastEndTime - Game.Time) < 0f))));
            }
        }

        public static bool CanIssueAttack =>
            ((Core.GameTickCount - _lastAutoAttackSent) > ((AttackCastDelay * 1000f) - (Game.Ping + 70)));

        private static bool CanIssueOrder =>
            ((((Core.GameTickCount - LastAutoAttack) + Game.Ping) + 70) >= (AttackDelay * 1000f));

        public static bool CanMove
        {
            get
            {
                if (AutoAttacks.UnabortableAutoDatabase.Contains(Player.Instance.Hero) && ((Core.GameTickCount - LastAutoAttack) >= Math.Min(MovementDelay, 100)))
                {
                    return true;
                }
                if ((Core.GameTickCount - _lastAutoAttackSent) <= (100 + Game.Ping))
                {
                    return false;
                }
                if (Player.Instance.Spellbook.IsChanneling && (!AllowedMovementBuffs.ContainsKey(Player.Instance.Hero) || !Player.Instance.HasBuff(AllowedMovementBuffs[Player.Instance.Hero])))
                {
                    return false;
                }
                return CanBeAborted;
            }
        }

        internal static bool CheckYasuoWall =>
            Menu["checkYasuoWall"].Cast<CheckBox>().CurrentValue;

        public static bool DisableAttacking
        {
            get => 
                (_disableAttacking ? true : AdvancedMenu["disableAttacking"].Cast<CheckBox>().CurrentValue);
            set
            {
                _disableAttacking = value;
            }
        }

        public static bool DisableMovement
        {
            get => 
                (_disableMovement ? true : AdvancedMenu["disableMovement"].Cast<CheckBox>().CurrentValue);
            set
            {
                _disableMovement = value;
            }
        }

        public static bool DrawAzirRange =>
            DrawingsMenu["drawAzirRange"].Cast<CheckBox>().CurrentValue;

        public static bool DrawDamageMarker =>
            DrawingsMenu["drawDamage"].Cast<CheckBox>().CurrentValue;

        public static bool DrawEnemyRange =>
            DrawingsMenu["_drawEnemyRange"].Cast<CheckBox>().CurrentValue;

        public static bool DrawHoldRadius =>
            DrawingsMenu["drawHoldRadius"].Cast<CheckBox>().CurrentValue;

        internal static EloBuddy.SDK.Menu.Menu DrawingsMenu
        {
            [CompilerGenerated]
            get => 
                <DrawingsMenu>k__BackingField;
            [CompilerGenerated]
            set
            {
                <DrawingsMenu>k__BackingField = value;
            }
        }

        public static bool DrawLastHitMarker =>
            DrawingsMenu["drawLasthit"].Cast<CheckBox>().CurrentValue;

        public static bool DrawRange =>
            DrawingsMenu["drawrange"].Cast<CheckBox>().CurrentValue;

        public static bool DrawTargetMarker =>
            DrawingsMenu["drawTarget"].Cast<CheckBox>().CurrentValue;

        public static int ExtraFarmDelay =>
            FarmingMenu["extraFarmDelay"].Cast<Slider>().CurrentValue;

        public static int ExtraWindUpTime =>
            Menu["extraWindUpTime"].Cast<Slider>().CurrentValue;

        internal static EloBuddy.SDK.Menu.Menu FarmingMenu
        {
            [CompilerGenerated]
            get => 
                <FarmingMenu>k__BackingField;
            [CompilerGenerated]
            set
            {
                <FarmingMenu>k__BackingField = value;
            }
        }

        internal static bool FastKiting =>
            Menu["fastKiting"].Cast<CheckBox>().CurrentValue;

        public static AttackableUnit ForcedTarget
        {
            [CompilerGenerated]
            get => 
                <ForcedTarget>k__BackingField;
            [CompilerGenerated]
            set
            {
                <ForcedTarget>k__BackingField = value;
            }
        }

        internal static bool FreezePriority =>
            FarmingMenu["_freezePriority"].Cast<CheckBox>().CurrentValue;

        public static bool GotAutoAttackReset
        {
            [CompilerGenerated]
            get => 
                <GotAutoAttackReset>k__BackingField;
            [CompilerGenerated]
            internal set
            {
                <GotAutoAttackReset>k__BackingField = value;
            }
        }

        public static int HoldRadius =>
            Menu["holdRadius" + Player.Instance.ChampionName].Cast<Slider>().CurrentValue;

        internal static bool IllaoiGhost
        {
            [CompilerGenerated]
            get => 
                <IllaoiGhost>k__BackingField;
            [CompilerGenerated]
            set
            {
                <IllaoiGhost>k__BackingField = value;
            }
        }

        public static bool IsAutoAttacking =>
            Player.Instance.Spellbook.IsAutoAttacking;

        public static bool IsMelee =>
            (((Player.Instance.IsMelee || (Player.Instance.Hero == Champion.Azir)) || ((Player.Instance.Hero == Champion.Thresh) || (Player.Instance.Hero == Champion.Velkoz))) || ((Player.Instance.Hero == Champion.Viktor) && Player.Instance.HasBuff("viktorpowertransferreturn")));

        public static bool IsRanged =>
            !IsMelee;

        public static bool LaneClearAttackChamps =>
            Menu["laneClearChamps"].Cast<CheckBox>().CurrentValue;

        public static Obj_AI_Minion LaneClearMinion
        {
            get => 
                CurrentMinions[TargetMinionType.LaneClear];
            internal set
            {
                CurrentMinions[TargetMinionType.LaneClear] = value;
            }
        }

        public static List<Obj_AI_Minion> LaneClearMinionsList =>
            (CurrentMinionsLists.ContainsKey(TargetMinionType.LaneClear) ? new List<Obj_AI_Minion>(CurrentMinionsLists[TargetMinionType.LaneClear]) : new List<Obj_AI_Minion>());

        public static int LastAutoAttack
        {
            [CompilerGenerated]
            get => 
                <LastAutoAttack>k__BackingField;
            [CompilerGenerated]
            internal set
            {
                <LastAutoAttack>k__BackingField = value;
            }
        }

        internal static bool LastHitBarrels =>
            Menu["lasthitbarrel"].Cast<CheckBox>().CurrentValue;

        public static Obj_AI_Minion LastHitMinion
        {
            get => 
                CurrentMinions[TargetMinionType.LastHit];
            internal set
            {
                CurrentMinions[TargetMinionType.LastHit] = value;
            }
        }

        public static List<Obj_AI_Minion> LastHitMinionsList =>
            (CurrentMinionsLists.ContainsKey(TargetMinionType.LastHit) ? new List<Obj_AI_Minion>(CurrentMinionsLists[TargetMinionType.LastHit]) : new List<Obj_AI_Minion>());

        public static bool LastHitPriority =>
            FarmingMenu["lastHitPriority"].Cast<CheckBox>().CurrentValue;

        public static int LastMovementSent
        {
            [CompilerGenerated]
            get => 
                <LastMovementSent>k__BackingField;
            [CompilerGenerated]
            internal set
            {
                <LastMovementSent>k__BackingField = value;
            }
        }

        public static AttackableUnit LastTarget
        {
            [CompilerGenerated]
            get => 
                <LastTarget>k__BackingField;
            [CompilerGenerated]
            internal set
            {
                <LastTarget>k__BackingField = value;
            }
        }

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

        public static int MovementDelay
        {
            get
            {
                int? nullable = _customMovementDelay;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : Menu["delayMove"].Cast<Slider>().CurrentValue);
            }
            set
            {
                _customMovementDelay = new int?(value);
            }
        }

        public static Vector3 OrbwalkPosition
        {
            get
            {
                if (OverrideOrbwalkPosition > null)
                {
                    Vector3? nullable = OverrideOrbwalkPosition();
                    if (nullable.HasValue)
                    {
                        return nullable.Value;
                    }
                }
                if ((StickToTarget && (LastTarget != null)) && !ActiveModesFlags.HasFlag(ActiveModes.Flee))
                {
                    Obj_AI_Base lastTarget = LastTarget as Obj_AI_Base;
                    if ((((lastTarget != null) && (lastTarget.IsMonster || (lastTarget.Type == GameObjectType.AIHeroClient))) && (((Obj_AI_Base) Player.Instance).IsInRange(lastTarget, (Player.Instance.GetAutoAttackRange(lastTarget) + 150f)) && (Game.CursorPos.Distance(lastTarget, true) < Game.CursorPos.Distance(((Obj_AI_Base) Player.Instance), true)))) && (lastTarget.Path.Length > 0))
                    {
                        return lastTarget.Path.Last<Vector3>();
                    }
                }
                return Game.CursorPos;
            }
        }

        public static OrbwalkPositionDelegate OverrideOrbwalkPosition
        {
            [CompilerGenerated]
            get => 
                <OverrideOrbwalkPosition>k__BackingField;
            [CompilerGenerated]
            set
            {
                <OverrideOrbwalkPosition>k__BackingField = value;
            }
        }

        public static Obj_AI_Minion PriorityLastHitWaitingMinion
        {
            get => 
                CurrentMinions[TargetMinionType.PriorityLastHitWaiting];
            internal set
            {
                if (value > null)
                {
                    _lastShouldWait = Core.GameTickCount;
                }
                CurrentMinions[TargetMinionType.PriorityLastHitWaiting] = value;
            }
        }

        public static List<Obj_AI_Minion> PriorityLastHitWaitingMinionsList =>
            (CurrentMinionsLists.ContainsKey(TargetMinionType.PriorityLastHitWaiting) ? new List<Obj_AI_Minion>(CurrentMinionsLists[TargetMinionType.PriorityLastHitWaiting]) : new List<Obj_AI_Minion>());

        internal static System.Random Random
        {
            [CompilerGenerated]
            get => 
                <Random>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Random>k__BackingField = value;
            }
        }

        internal static int RandomOffset
        {
            [CompilerGenerated]
            get => 
                <RandomOffset>k__BackingField;
            [CompilerGenerated]
            set
            {
                <RandomOffset>k__BackingField = value;
            }
        }

        public static bool ShouldWait =>
            (((Core.GameTickCount - _lastShouldWait) <= 400) || (PriorityLastHitWaitingMinion > null));

        internal static bool StickToTarget =>
            (!Player.Instance.IsMelee ? false : Menu["stickToTarget"].Cast<CheckBox>().CurrentValue);

        public static bool SupportMode
        {
            get
            {
                bool? nullable = _customSupportMode;
                return (nullable.HasValue ? nullable.GetValueOrDefault() : Menu["supportMode" + Player.Instance.ChampionName].Cast<CheckBox>().CurrentValue);
            }
            set
            {
                _customSupportMode = new bool?(value);
            }
        }

        public static List<Obj_AI_Minion> UnKillableMinionsList =>
            (CurrentMinionsLists.ContainsKey(TargetMinionType.UnKillable) ? new List<Obj_AI_Minion>(CurrentMinionsLists[TargetMinionType.UnKillable]) : new List<Obj_AI_Minion>());

        public static bool UseOnTick =>
            AdvancedMenu["useTick"].Cast<CheckBox>().CurrentValue;

        public static bool UseOnUpdate =>
            AdvancedMenu["useUpdate"].Cast<CheckBox>().CurrentValue;

        public static bool UseTiamat =>
            ((Player.Instance.IsMelee && FarmingMenu["useTiamat"].Cast<CheckBox>().CurrentValue) && Player.Instance.InventoryItems.HasItem(new ItemId[] { ItemId.Tiamat, ItemId.Ravenous_Hydra }));

        public static List<Obj_AI_Minion> ValidAzirSoldiers =>
            _validAzirSoldiers.Values.ToList<Obj_AI_Minion>();

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Orbwalker.<>c <>9 = new Orbwalker.<>c();
            public static Func<Obj_AI_Minion, bool> <>9__204_0;
            public static Func<BuffInstance, bool> <>9__204_1;
            public static Obj_AI_BasePlayAnimation <>9__204_10;
            public static Obj_AI_BasePlayAnimation <>9__204_11;
            public static Obj_AI_ProcessSpellCast <>9__204_12;
            public static Func<Obj_AI_Minion, bool> <>9__204_17;
            public static Orbwalker.UnkillableMinionHandler <>9__204_18;
            public static GameObjectDelete <>9__204_19;
            public static Obj_AI_BasePlayAnimation <>9__204_2;
            public static PlayerPostIssueOrder <>9__204_21;
            public static Obj_AI_BaseBuffGain <>9__204_4;
            public static Obj_AI_BaseBuffLose <>9__204_5;
            public static Func<AIHeroClient, bool> <>9__204_6;
            public static Func<AttackableUnit, bool> <>9__204_7;
            public static GameTick <>9__204_8;
            public static GameUpdate <>9__204_9;
            public static Func<KeyValuePair<Orbwalker.TargetMinionType, Obj_AI_Minion>, bool> <>9__207_0;
            public static Func<Obj_AI_Minion, bool> <>9__208_0;
            public static Func<Obj_AI_Minion, float> <>9__208_1;
            public static Action <>9__216_0;
            public static Func<Orbwalker.ActiveModes, bool> <>9__219_0;
            public static Predicate<AttackableUnit> <>9__219_1;
            public static Func<AIHeroClient, bool> <>9__221_0;
            public static Func<AIHeroClient, bool> <>9__221_2;
            public static Func<Obj_AI_Minion, bool> <>9__221_3;
            public static Func<Obj_AI_Minion, bool> <>9__221_4;
            public static Func<AIHeroClient, bool> <>9__221_5;
            public static Func<AttackableUnit, bool> <>9__221_6;
            public static Func<AttackableUnit, float> <>9__221_7;
            public static Func<Obj_AI_Minion, float> <>9__221_8;
            public static Func<Obj_AI_Minion, bool> <>9__221_9;
            public static Func<Obj_AI_Minion, bool> <>9__222_0;
            public static Func<KeyValuePair<int, List<Prediction.Health.IncomingAttack>>, IEnumerable<Prediction.Health.IncomingAttack>> <>9__256_0;
            public static Func<Obj_AI_Minion, float> <>9__256_1;
            public static Func<Obj_AI_Minion, float> <>9__256_2;
            public static Func<Obj_AI_Minion, bool> <>9__256_3;
            public static Func<BuffInstance, bool> <>9__256_4;
            public static Func<Obj_AI_Minion, float> <>9__257_0;
            public static Func<Obj_AI_Minion, float> <>9__257_1;
            public static Func<Obj_AI_Minion, float> <>9__257_2;
            public static Func<Obj_AI_Minion, float> <>9__257_3;
            public static Func<Obj_AI_Minion, float> <>9__257_4;
            public static Func<Obj_AI_Minion, bool> <>9__257_5;
            public static Func<Obj_AI_Minion, float> <>9__257_6;
            public static Func<AIHeroClient, bool> <>9__260_0;
            public static Func<Orbwalker.ActiveModes, bool> <>9__52_0;

            internal bool <Clear>b__207_0(KeyValuePair<Orbwalker.TargetMinionType, Obj_AI_Minion> entry) => 
                (!entry.Value.IsValidTarget(null, false, null) || !Player.Instance.IsInAutoAttackRange(entry.Value));

            internal bool <GetIllaoiGhost>b__222_0(Obj_AI_Minion o) => 
                (((o.IsValidTarget(null, false, null) && o.IsEnemy) && o.HasBuff("illaoiespirit")) && Player.Instance.IsInAutoAttackRange(o));

            internal bool <GetTarget>b__219_0(Orbwalker.ActiveModes mode) => 
                ((mode != Orbwalker.ActiveModes.None) && Orbwalker.ActiveModesFlags.HasFlag(mode));

            internal bool <GetTarget>b__219_1(AttackableUnit o) => 
                (o == null);

            internal bool <GetTarget>b__221_0(AIHeroClient h) => 
                (h.IsValidTarget(null, false, null) && Orbwalker.ValidAzirSoldiers.Any<Obj_AI_Minion>(i => i.IsInAutoAttackRange(h)));

            internal bool <GetTarget>b__221_2(AIHeroClient h) => 
                ((h.IsValidTarget(null, false, null) && Player.Instance.IsInAutoAttackRange(h)) && ((!Orbwalker.IsRanged || !Orbwalker.CheckYasuoWall) || (Prediction.Position.Collision.GetYasuoWallCollision(Player.Instance.ServerPosition, h.ServerPosition, true) == Vector3.Zero)));

            internal bool <GetTarget>b__221_3(Obj_AI_Minion m) => 
                ((Orbwalker.GetAutoAttackDamage(m) >= Prediction.Health.GetPrediction(m, Orbwalker.GetMissileTravelTime(m))) && Player.Instance.IsInAutoAttackRange(m));

            internal bool <GetTarget>b__221_4(Obj_AI_Minion m) => 
                Player.Instance.IsInAutoAttackRange(m);

            internal bool <GetTarget>b__221_5(AIHeroClient i) => 
                (!i.IsMe && i.IsValidTarget(new float?((float) 0x41a), false, null));

            internal bool <GetTarget>b__221_6(AttackableUnit o) => 
                (((o.IsValid && !o.IsDead) && o.IsTargetable) && (((Obj_AI_Base) Player.Instance).Distance(((GameObject) o), true) <= Player.Instance.GetAutoAttackRange(o).Pow()));

            internal float <GetTarget>b__221_7(AttackableUnit o) => 
                o.MaxHealth;

            internal float <GetTarget>b__221_8(Obj_AI_Minion o) => 
                ((Obj_AI_Base) o).Distance(Game.CursorPos, false);

            internal bool <GetTarget>b__221_9(Obj_AI_Minion p) => 
                (p.IsValidTarget(null, false, null) && (((Obj_AI_Base) Player.Instance).Distance(((Obj_AI_Base) p), true) <= Player.Instance.GetAutoAttackRange(p).Pow()));

            internal bool <Initialize>b__204_0(Obj_AI_Minion o) => 
                (((o.IsValid && o.IsAlly) && (o.Name == "AzirSoldier")) && o.Buffs.Any<BuffInstance>(b => (((b.IsValid() && b.Caster.IsMe) && (b.Count == 1)) && (b.DisplayName == "azirwspawnsound"))));

            internal bool <Initialize>b__204_1(BuffInstance b) => 
                (((b.IsValid() && b.Caster.IsMe) && (b.Count == 1)) && (b.DisplayName == "azirwspawnsound"));

            internal void <Initialize>b__204_10(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
            {
                if (sender.IsMe && AutoAttacks.SpecialAutoAttacksAnimationName[Player.Instance.Hero].Contains(args.Animation))
                {
                    Orbwalker._OnAttack(null);
                }
            }

            internal void <Initialize>b__204_11(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
            {
                if (sender.IsMe && AutoAttacks.IsDashAutoAttackReset(Player.Instance, args))
                {
                    Orbwalker.GotAutoAttackReset = true;
                    Orbwalker._waitingForAutoAttackReset = true;
                }
            }

            internal void <Initialize>b__204_12(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
            {
                if (sender.IsMe && AutoAttacks.IsDashAutoAttackReset(Player.Instance, args))
                {
                    Orbwalker.GotAutoAttackReset = true;
                    Orbwalker._waitingForAutoAttackReset = true;
                }
            }

            internal bool <Initialize>b__204_17(Obj_AI_Minion o) => 
                (Orbwalker.DamageOnMinions.ContainsKey(o.NetworkId) && o.VisibleOnScreen);

            internal void <Initialize>b__204_18(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
            {
                if ((Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) && (Orbwalker.UseTiamat && (((Obj_AI_Base) Player.Instance).Distance(target, true) <= 160000f)))
                {
                    float num = Player.Instance.GetItemDamage(target, ItemId.Tiamat);
                    if (Prediction.Health.GetPrediction(target, 200) <= num)
                    {
                        ItemId[] source = new ItemId[] { ItemId.Tiamat, ItemId.Ravenous_Hydra };
                        foreach (ItemId id in source.Where<ItemId>(new Func<ItemId, bool>(Item.CanUseItem)))
                        {
                            Item.UseItem(id, (Obj_AI_Base) null);
                        }
                    }
                }
            }

            internal void <Initialize>b__204_19(GameObject sender, EventArgs args)
            {
                if (sender.IsStructure())
                {
                    Orbwalker.EnemyStructures.RemoveAll(o => o.IdEquals(sender));
                }
                if (sender.IdEquals(Orbwalker.LastHitMinion))
                {
                    Orbwalker.LastHitMinion = null;
                }
                if (sender.IdEquals(Orbwalker.PriorityLastHitWaitingMinion))
                {
                    Orbwalker.PriorityLastHitWaitingMinion = null;
                }
                if (sender.IdEquals(Orbwalker.LaneClearMinion))
                {
                    Orbwalker.LaneClearMinion = null;
                }
                if (Orbwalker.LastTarget.IdEquals(sender))
                {
                    Orbwalker.LastTarget = null;
                }
                if (Orbwalker.ForcedTarget.IdEquals(sender))
                {
                    Orbwalker.ForcedTarget = null;
                }
            }

            internal void <Initialize>b__204_2(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
            {
                Obj_AI_Minion soldier = sender as Obj_AI_Minion;
                if (((soldier != null) && soldier.IsAlly) && (soldier.Name == "AzirSoldier"))
                {
                    switch (args.Animation)
                    {
                        case "Inactive":
                            Orbwalker._validAzirSoldiers.Remove(soldier.NetworkId);
                            if (Orbwalker.AzirSoldierPreDashStatus.ContainsKey(soldier.NetworkId))
                            {
                                Orbwalker.AzirSoldierPreDashStatus[soldier.NetworkId] = false;
                            }
                            break;

                        case "Reactivate":
                            Orbwalker._validAzirSoldiers[soldier.NetworkId] = soldier;
                            if (Orbwalker.AzirSoldierPreDashStatus.ContainsKey(soldier.NetworkId))
                            {
                                Orbwalker.AzirSoldierPreDashStatus[soldier.NetworkId] = true;
                            }
                            break;

                        case "Run":
                            if (!Orbwalker.AzirSoldierPreDashStatus.ContainsKey(soldier.NetworkId))
                            {
                                Orbwalker.AzirSoldierPreDashStatus.Add(soldier.NetworkId, Orbwalker._validAzirSoldiers.Any<KeyValuePair<int, Obj_AI_Minion>>(o => o.Value.IdEquals(soldier)));
                            }
                            Orbwalker._validAzirSoldiers.Remove(soldier.NetworkId);
                            break;

                        case "Run_Exit":
                            if (!Orbwalker.AzirSoldierPreDashStatus.ContainsKey(soldier.NetworkId) ? false : Orbwalker.AzirSoldierPreDashStatus[soldier.NetworkId])
                            {
                                Orbwalker._validAzirSoldiers[soldier.NetworkId] = soldier;
                                Orbwalker.AzirSoldierPreDashStatus.Remove(soldier.NetworkId);
                            }
                            break;

                        case "Death":
                            Orbwalker._azirSoldiers.Remove(soldier.NetworkId);
                            Orbwalker._validAzirSoldiers.Remove(soldier.NetworkId);
                            Orbwalker.AzirSoldierPreDashStatus.Remove(soldier.NetworkId);
                            break;
                    }
                }
            }

            internal void <Initialize>b__204_21(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
            {
                if (sender.IsMe)
                {
                    Orbwalker._lastIssueOrderStartVector = new Vector3?(sender.Position);
                    Orbwalker._lastIssueOrderEndVector = new Vector3?(args.TargetPosition);
                    Orbwalker._lastIssueOrderType = new GameObjectOrder?(args.Order);
                    Orbwalker._lastIssueOrderTargetId = new int?((args.Target != null) ? args.Target.NetworkId : 0);
                }
            }

            internal void <Initialize>b__204_4(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
            {
                Obj_AI_Minion minion = sender as Obj_AI_Minion;
                if ((((minion != null) && minion.IsAlly) && ((minion.Name == "AzirSoldier") && args.Buff.Caster.IsMe)) && (args.Buff.DisplayName == "azirwspawnsound"))
                {
                    Orbwalker._azirSoldiers[minion.NetworkId] = minion;
                    Orbwalker._validAzirSoldiers[minion.NetworkId] = minion;
                }
            }

            internal void <Initialize>b__204_5(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
            {
                Obj_AI_Minion minion = sender as Obj_AI_Minion;
                if (((minion != null) && minion.IsAlly) && (minion.Name == "AzirSoldier"))
                {
                    Orbwalker._azirSoldiers.Remove(minion.NetworkId);
                    Orbwalker._validAzirSoldiers.Remove(minion.NetworkId);
                    Orbwalker.AzirSoldierPreDashStatus.Remove(minion.NetworkId);
                }
            }

            internal bool <Initialize>b__204_6(AIHeroClient h) => 
                (h.Hero == Champion.Illaoi);

            internal bool <Initialize>b__204_7(AttackableUnit o) => 
                (o.IsEnemy && o.IsStructure());

            internal void <Initialize>b__204_8(EventArgs <args>)
            {
                if (Orbwalker.UseOnTick)
                {
                    Orbwalker.OnTick();
                }
            }

            internal void <Initialize>b__204_9(EventArgs <args>)
            {
                if (Orbwalker.UseOnUpdate)
                {
                    Orbwalker.OnTick();
                }
                Orbwalker.OnUpdate();
            }

            internal bool <ModeIsActive>b__52_0(Orbwalker.ActiveModes m) => 
                Orbwalker.ActiveModesFlags.HasFlag(m);

            internal bool <OnDraw>b__260_0(AIHeroClient o) => 
                (o.IsValidTarget(null, false, null) && o.ServerPosition.IsOnScreen());

            internal void <OnSpellCast>b__216_0()
            {
                Orbwalker._autoAttackCompleted = true;
                Orbwalker.TriggerOnPostAttack();
            }

            internal bool <OnTick>b__208_0(Obj_AI_Minion o) => 
                ((Obj_AI_Base) Player.Instance).IsInRange(((Obj_AI_Base) o), 1500f);

            internal float <OnTick>b__208_1(Obj_AI_Minion o) => 
                o.MaxHealth;

            internal IEnumerable<Prediction.Health.IncomingAttack> <RecalculateLasthittableMinions>b__256_0(KeyValuePair<int, List<Prediction.Health.IncomingAttack>> i) => 
                i.Value;

            internal float <RecalculateLasthittableMinions>b__256_1(Obj_AI_Minion minion) => 
                minion.MaxHealth;

            internal float <RecalculateLasthittableMinions>b__256_2(Obj_AI_Minion minion) => 
                minion.Health;

            internal bool <RecalculateLasthittableMinions>b__256_3(Obj_AI_Minion minion) => 
                (minion.Health > 0f);

            internal bool <RecalculateLasthittableMinions>b__256_4(BuffInstance b) => 
                b.DisplayName.Equals("GangplankEBarrelActive");

            internal float <SortMinionsAndDefineTargets>b__257_0(Obj_AI_Minion minion) => 
                (Orbwalker.FreezePriority ? Orbwalker.CurrentMinionValues[minion.NetworkId].LaneClearHealth : (1f / Orbwalker.CurrentMinionValues[minion.NetworkId].LaneClearHealth));

            internal float <SortMinionsAndDefineTargets>b__257_1(Obj_AI_Minion o) => 
                o.MaxHealth;

            internal float <SortMinionsAndDefineTargets>b__257_2(Obj_AI_Minion o) => 
                Orbwalker.CurrentMinionValues[o.NetworkId].LastHitHealth;

            internal float <SortMinionsAndDefineTargets>b__257_3(Obj_AI_Minion o) => 
                o.MaxHealth;

            internal float <SortMinionsAndDefineTargets>b__257_4(Obj_AI_Minion o) => 
                Orbwalker.CurrentMinionValues[o.NetworkId].LaneClearHealth;

            internal bool <SortMinionsAndDefineTargets>b__257_5(Obj_AI_Minion m) => 
                (Player.Instance.IsInAutoAttackRange(m) && ((Orbwalker.LastHitMinion == null) || !m.IdEquals(Orbwalker.LastHitMinion)));

            internal float <SortMinionsAndDefineTargets>b__257_6(Obj_AI_Minion o) => 
                Orbwalker.CurrentMinionValues[o.NetworkId].LastHitHealth;
        }

        [Flags]
        public enum ActiveModes
        {
            Combo = 1,
            Flee = 0x20,
            Harass = 2,
            JungleClear = 8,
            JunglePlantsClear = 0x40,
            LaneClear = 0x10,
            LastHit = 4,
            None = 0
        }

        public delegate void AttackHandler(AttackableUnit target, EventArgs args);

        internal class CalculatedMinionValue
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Obj_AI_Minion <Handle>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float <LaneClearHealth>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <LaneClearTime>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float <LastHitHealth>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <LastHitTime>k__BackingField;

            internal CalculatedMinionValue(Obj_AI_Minion minion)
            {
                this.Handle = minion;
                this.LastHitHealth = this.Handle.Health;
                this.LaneClearHealth = this.Handle.Health;
            }

            internal Obj_AI_Minion Handle { get; set; }

            internal bool IsAlmostLastHittable
            {
                get
                {
                    float num = this.Handle.HasTurretTargetting() ? this.LastHitHealth : this.LaneClearHealth;
                    float num2 = this.Handle.IsSiegeMinion() ? 1.5f : 1f;
                    return ((num <= (num2 * Orbwalker.GetAutoAttackDamage(this.Handle))) && (num < this.Handle.Health));
                }
            }

            internal bool IsLaneClearMinion
            {
                get
                {
                    if (Orbwalker._onlyLastHit)
                    {
                        return false;
                    }
                    Obj_AI_Turret from = EntityManager.Turrets.Allies.FirstOrDefault<Obj_AI_Turret>(t => ((Obj_AI_Base) t).Distance(((Obj_AI_Base) this.Handle), true) <= 688900f);
                    if (from > null)
                    {
                        if (Math.Abs((float) (this.LaneClearHealth - this.Handle.Health)) < float.Epsilon)
                        {
                            float num2 = from.GetAutoAttackDamage(this.Handle, false);
                            float autoAttackDamage = Orbwalker.GetAutoAttackDamage(this.Handle);
                            for (float i = this.Handle.Health; (i > 0f) && (num2 > 0f); i -= num2)
                            {
                                if (i <= autoAttackDamage)
                                {
                                    return false;
                                }
                            }
                            return true;
                        }
                        return false;
                    }
                    float num = 2f * (((Player.Instance.FlatCritChanceMod >= 0.5f) && (Player.Instance.FlatCritChanceMod < 1f)) ? Player.Instance.GetCriticalStrikePercentMod() : 1f);
                    return ((this.LaneClearHealth > (num * Orbwalker.GetAutoAttackDamage(this.Handle))) || (Math.Abs((float) (this.LaneClearHealth - this.Handle.Health)) < float.Epsilon));
                }
            }

            internal bool IsLastHittable =>
                (this.LastHitHealth <= Orbwalker.GetAutoAttackDamage(this.Handle));

            internal bool IsUnkillable =>
                (this.LastHitHealth < 0f);

            internal float LaneClearHealth { get; set; }

            internal int LaneClearTime { get; set; }

            internal float LastHitHealth { get; set; }

            internal int LastHitTime { get; set; }
        }

        public delegate void OnAutoAttackResetHandler(EventArgs args);

        public delegate Vector3? OrbwalkPositionDelegate();

        public delegate void PostAttackHandler(AttackableUnit target, EventArgs args);

        public class PreAttackArgs : EventArgs
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <Process>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private AttackableUnit <Target>k__BackingField;

            public PreAttackArgs(AttackableUnit target)
            {
                this.Process = true;
                this.Target = target;
                Orbwalker.TriggerPreAttackEvent(target, this);
            }

            public bool Process { get; set; }

            public AttackableUnit Target { get; private set; }
        }

        public delegate void PreAttackHandler(AttackableUnit target, Orbwalker.PreAttackArgs args);

        public class PreMoveArgs : EventArgs
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsOverridden>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Vector3 <Position>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <Process>k__BackingField;

            public PreMoveArgs(Vector3 position)
            {
                this.Process = true;
                this.Position = position;
                this.IsOverridden = (Orbwalker.OverrideOrbwalkPosition != null) && Orbwalker.OverrideOrbwalkPosition().HasValue;
                Orbwalker.TriggerPreMoveEvent(this);
            }

            public bool IsOverridden { get; private set; }

            public Vector3 Position { get; private set; }

            public bool Process { get; set; }
        }

        public delegate void PreMoveHandler(EventArgs args);

        internal enum TargetMinionType
        {
            LastHit,
            PriorityLastHitWaiting,
            LaneClear,
            UnKillable
        }

        internal enum TargetTypes
        {
            Hero,
            JungleMob,
            LaneMinion,
            Structure,
            JunglePlant
        }

        public class UnkillableMinionArgs : EventArgs
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float <RemainingHealth>k__BackingField;

            public float RemainingHealth { get; internal set; }
        }

        public delegate void UnkillableMinionHandler(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args);
    }
}

