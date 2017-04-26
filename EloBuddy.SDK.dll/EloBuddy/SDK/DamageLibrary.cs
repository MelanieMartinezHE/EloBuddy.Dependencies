namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class DamageLibrary
    {
        internal static readonly List<string> SpecialCases;

        static DamageLibrary()
        {
            List<string> list1 = new List<string> { 
                "All champs with 0 spell base attack damage",
                "Amumu W",
                "Ashe Passive",
                "Ashe Q",
                "Zed Passive",
                "Darius R",
                "Dr Mundo Q",
                "Dr Mundo E",
                "Ekko E",
                "Elise Q bonus",
                "Elise R",
                "Evelynn R",
                "Fiddlesticks E minions",
                "Fizz W bonus",
                "Garen Q",
                "Hecarim Q minions",
                "Heimerdinger empowered",
                "Jayce W second form",
                "Jayce E monster",
                "Jinx R",
                "Kalista E",
                "Karma ULT",
                "Kassadin R stacks",
                "KhaZix Q empowered",
                "KogMaw W",
                "LeBlanc R",
                "LeeSin Q second cast bonus",
                "Malzahar W",
                "Malzahar E damage per half second from default / 8",
                "Maokai W",
                "Malzahar R"
            };
            SpecialCases = list1;
        }

        public static DamageLibraryManager.ChampionDamageDatabase GetChampionDamageDatabase(this AIHeroClient source) => 
            GetChampionDamageDatabase(source.Hero);

        public static DamageLibraryManager.ChampionDamageDatabase GetChampionDamageDatabase(Champion source)
        {
            DamageLibraryManager.ChampionDamageDatabase database;
            return (DamageLibraryManager.TryGetChampion(source, out database) ? database : new DamageLibraryManager.ChampionDamageDatabase());
        }

        public static float GetItemDamage(this AIHeroClient source, Obj_AI_Base target, ItemId item)
        {
            switch (item)
            {
                case ItemId.Ravenous_Hydra:
                    return source.CalculateDamageOnUnit(target, DamageType.Physical, (0.6f * source.TotalAttackDamage), true, false);

                case ItemId.Tiamat:
                    return source.CalculateDamageOnUnit(target, DamageType.Physical, (0.6f * source.TotalAttackDamage), true, false);

                case ItemId.Frost_Queens_Claim:
                    return source.CalculateDamageOnUnit(target, DamageType.Magical, ((float) (50 + (5 * source.Level))), true, false);

                case ItemId.Bilgewater_Cutlass:
                    return source.CalculateDamageOnUnit(target, DamageType.Magical, 100f, true, false);

                case ItemId.Hextech_Gunblade:
                    return source.CalculateDamageOnUnit(target, DamageType.Magical, (150f + (0.4f * source.TotalMagicalDamage)), true, false);

                case ItemId.Liandrys_Torment:
                    return source.CalculateDamageOnUnit(target, DamageType.Magical, (((target.Health * 0.2f) * 3f) * (target.CanMove ? ((float) 1) : ((float) 2))), true, false);

                case ItemId.Blade_of_the_Ruined_King:
                    return Math.Max(100f, source.CalculateDamageOnUnit(target, DamageType.Physical, target.MaxHealth * 0.1f, true, false));
            }
            object[] args = new object[] { item };
            Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Info, "Item id '{0}' not yet added to DamageLibrary.GetItemDamage!", args);
            return 0f;
        }

        public static float GetSpellDamage(this AIHeroClient source, Obj_AI_Base target, SpellSlot slot, SpellStages stage = 0)
        {
            Damage.DamageSourceBase base2;
            if ((source == null) || (target == null))
            {
                return 0f;
            }
            return (DamageLibraryManager.TryGetStage(source.Hero, slot, stage, out base2) ? base2.GetDamage(source, target) : 0f);
        }

        public static DamageLibraryManager.SpellDamageDatabase GetSpellDamageDatabase(this AIHeroClient source, SpellSlot slot) => 
            GetSpellDamageDatabase(source.Hero, slot);

        public static DamageLibraryManager.SpellDamageDatabase GetSpellDamageDatabase(Champion source, SpellSlot slot)
        {
            DamageLibraryManager.SpellDamageDatabase database;
            return (DamageLibraryManager.TryGetSlot(source, slot, out database) ? database : new DamageLibraryManager.SpellDamageDatabase());
        }

        public static float GetSummonerSpellDamage(this AIHeroClient source, Obj_AI_Base target, SummonerSpells summonerSpell)
        {
            switch (summonerSpell)
            {
                case SummonerSpells.Ignite:
                    return ((50 + (20 * source.Level)) - ((target.HPRegenRate / 5f) * 3f));

                case SummonerSpells.Smite:
                    if (!(target is AIHeroClient))
                    {
                        return new float[] { 
                            390f, 410f, 430f, 450f, 480f, 510f, 540f, 570f, 600f, 640f, 680f, 720f, 760f, 800f, 850f, 900f,
                            950f, 1000f
                        }[source.Level - 1];
                    }
                    if (source.Spellbook.Spells.Any<SpellDataInst>(o => o.Name == "s5_summonersmiteplayerganker"))
                    {
                        return (float) (20 + (8 * source.Level));
                    }
                    if (source.Spellbook.Spells.Any<SpellDataInst>(o => o.Name == "s5_summonersmiteduel"))
                    {
                        return (float) (0x36 + (6 * source.Level));
                    }
                    break;
            }
            return 0f;
        }

        internal static void Initialize()
        {
            DamageLibraryManager.Initialize();
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly DamageLibrary.<>c <>9 = new DamageLibrary.<>c();
            public static Func<SpellDataInst, bool> <>9__7_0;
            public static Func<SpellDataInst, bool> <>9__7_1;

            internal bool <GetSummonerSpellDamage>b__7_0(SpellDataInst o) => 
                (o.Name == "s5_summonersmiteplayerganker");

            internal bool <GetSummonerSpellDamage>b__7_1(SpellDataInst o) => 
                (o.Name == "s5_summonersmiteduel");
        }

        public enum SpellStages
        {
            Default,
            SecondCast,
            SecondForm,
            WayBack,
            Detonation,
            DamagePerSecond,
            Empowered,
            EmpoweredSecondCast,
            ToggledState,
            DamagePerStack,
            Passive
        }

        public enum SummonerSpells
        {
            Ignite,
            Smite
        }
    }
}

