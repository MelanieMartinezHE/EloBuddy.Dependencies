namespace EloBuddy.SDK.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class Interrupter
    {
        internal static readonly Dictionary<int, InterruptableSpell> CastingSpell = new Dictionary<int, InterruptableSpell>();
        internal static readonly Dictionary<string, List<InterruptableSpell>> SpellDatabase = new Dictionary<string, List<InterruptableSpell>>();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event InterruptableSpellHandler OnInterruptableSpell;

        static Interrupter()
        {
            Initialize();
        }

        internal static void GameOnOnUpdate(EventArgs args)
        {
            foreach (AIHeroClient client in from h in EntityManager.Heroes.AllHeroes
                where ((CastingSpell.ContainsKey(h.NetworkId) && !h.Spellbook.IsChanneling) && !h.Spellbook.IsCharging) && !h.Spellbook.IsCastingSpell
                select h)
            {
                CastingSpell.Remove(client.NetworkId);
            }
            if (OnInterruptableSpell != null)
            {
                foreach (InterruptableSpellEventArgs args2 in from h in EntityManager.Heroes.AllHeroes.Select<AIHeroClient, InterruptableSpellEventArgs>(new Func<AIHeroClient, InterruptableSpellEventArgs>(Interrupter.GetSpell))
                    where h > null
                    select h)
                {
                    OnInterruptableSpell(args2.Sender, args2);
                }
            }
        }

        internal static InterruptableSpellEventArgs GetSpell(AIHeroClient target)
        {
            if ((!target.IsValid || target.IsDead) || !CastingSpell.ContainsKey(target.NetworkId))
            {
                return null;
            }
            InterruptableSpell spell = CastingSpell[target.NetworkId];
            return new InterruptableSpellEventArgs { 
                Sender = target,
                Slot = spell.SpellSlot,
                DangerLevel = spell.DangerLevel,
                EndTime = target.Spellbook.CastEndTime
            };
        }

        internal static void Initialize()
        {
            RegisterSpell("Urgot", SpellSlot.R, DangerLevel.High);
            RegisterSpell("Velkoz", SpellSlot.R, DangerLevel.High);
            RegisterSpell("Warwick", SpellSlot.R, DangerLevel.High);
            RegisterSpell("Xerath", SpellSlot.R, DangerLevel.High);
            RegisterSpell("Caitlyn", SpellSlot.R, DangerLevel.High);
            RegisterSpell("FiddleSticks", SpellSlot.R, DangerLevel.High);
            RegisterSpell("Galio", SpellSlot.R, DangerLevel.High);
            RegisterSpell("Sion", SpellSlot.Q, DangerLevel.High);
            RegisterSpell("Karthus", SpellSlot.R, DangerLevel.High);
            RegisterSpell("Katarina", SpellSlot.R, DangerLevel.High);
            RegisterSpell("Lucian", SpellSlot.R, DangerLevel.High);
            RegisterSpell("Malzahar", SpellSlot.R, DangerLevel.High);
            RegisterSpell("MissFortune", SpellSlot.R, DangerLevel.High);
            RegisterSpell("Nunu", SpellSlot.R, DangerLevel.High);
            RegisterSpell("FiddleSticks", SpellSlot.W, DangerLevel.Medium);
            RegisterSpell("Varus", SpellSlot.Q, DangerLevel.Medium);
            RegisterSpell("Pantheon", SpellSlot.E, DangerLevel.Medium);
            RegisterSpell("Janna", SpellSlot.R, DangerLevel.Medium);
            RegisterSpell("TahmKench", SpellSlot.R, DangerLevel.Medium);
            RegisterSpell("Quinn", SpellSlot.R, DangerLevel.Medium);
            RegisterSpell("Xerath", SpellSlot.Q, DangerLevel.Medium);
            RegisterSpell("Zac", SpellSlot.E, DangerLevel.Medium);
            RegisterSpell("MasterYi", SpellSlot.W, DangerLevel.Low);
            RegisterSpell("RekSai", SpellSlot.R, DangerLevel.Low);
            RegisterSpell("Shen", SpellSlot.R, DangerLevel.Low);
            RegisterSpell("TwistedFate", SpellSlot.R, DangerLevel.Low);
            Game.OnTick += new GameTick(Interrupter.GameOnOnUpdate);
            Obj_AI_Base.OnProcessSpellCast += new Obj_AI_ProcessSpellCast(Interrupter.OnOnProcessSpellCast);
        }

        internal static void OnOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            AIHeroClient client = sender as AIHeroClient;
            if (((client != null) && !CastingSpell.ContainsKey(client.NetworkId)) && SpellDatabase.ContainsKey(client.ChampionName))
            {
                InterruptableSpell spell = SpellDatabase[client.ChampionName].Find(o => o.SpellSlot == args.Slot);
                if (spell > null)
                {
                    CastingSpell.Add(client.NetworkId, spell);
                }
            }
        }

        internal static void RegisterSpell(string champName, SpellSlot slot, DangerLevel dangerLevel)
        {
            if (!SpellDatabase.ContainsKey(champName))
            {
                SpellDatabase.Add(champName, new List<InterruptableSpell>());
            }
            InterruptableSpell item = new InterruptableSpell {
                SpellSlot = slot,
                DangerLevel = dangerLevel
            };
            SpellDatabase[champName].Add(item);
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Interrupter.<>c <>9 = new Interrupter.<>c();
            public static Func<AIHeroClient, bool> <>9__8_0;
            public static Func<Interrupter.InterruptableSpellEventArgs, bool> <>9__8_1;

            internal bool <GameOnOnUpdate>b__8_0(AIHeroClient h) => 
                (((Interrupter.CastingSpell.ContainsKey(h.NetworkId) && !h.Spellbook.IsChanneling) && !h.Spellbook.IsCharging) && !h.Spellbook.IsCastingSpell);

            internal bool <GameOnOnUpdate>b__8_1(Interrupter.InterruptableSpellEventArgs h) => 
                (h > null);
        }

        internal class InterruptableSpell
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Enumerations.DangerLevel <DangerLevel>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SpellSlot <SpellSlot>k__BackingField;

            public EloBuddy.SDK.Enumerations.DangerLevel DangerLevel { get; internal set; }

            public EloBuddy.SpellSlot SpellSlot { get; internal set; }
        }

        public class InterruptableSpellEventArgs : EventArgs
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Enumerations.DangerLevel <DangerLevel>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float <EndTime>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private AIHeroClient <Sender>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SpellSlot <Slot>k__BackingField;

            public EloBuddy.SDK.Enumerations.DangerLevel DangerLevel { get; internal set; }

            public float EndTime { get; internal set; }

            public AIHeroClient Sender { get; internal set; }

            public SpellSlot Slot { get; internal set; }
        }

        public delegate void InterruptableSpellHandler(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e);
    }
}

