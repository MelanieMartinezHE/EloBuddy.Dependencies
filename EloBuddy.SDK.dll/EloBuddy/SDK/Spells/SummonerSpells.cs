namespace EloBuddy.SDK.Spells
{
    using EloBuddy;
    using EloBuddy.SDK;
    using System;

    public static class SummonerSpells
    {
        public static Spell.Active Barrier = new Spell.Active(SpellSlot.Unknown, 0x7fffffff, DamageType.Mixed);
        public static Spell.Active Clarity = new Spell.Active(SpellSlot.Unknown, 0x7fffffff, DamageType.Mixed);
        public static Spell.Active Cleanse = new Spell.Active(SpellSlot.Unknown, 0x7fffffff, DamageType.Mixed);
        public static Spell.Targeted Exhaust = new Spell.Targeted(SpellSlot.Unknown, 650, DamageType.Mixed);
        public static Spell.Skillshot Flash = new Spell.Skillshot(SpellSlot.Unknown, 420, SkillShotType.Circular, 0, 0x7fffffff, null, DamageType.Mixed);
        public static Spell.Active Ghost = new Spell.Active(SpellSlot.Unknown, 0x7fffffff, DamageType.Mixed);
        public static Spell.Active Heal = new Spell.Active(SpellSlot.Unknown, 0x7fffffff, DamageType.Mixed);
        public static Spell.Targeted Ignite = new Spell.Targeted(SpellSlot.Unknown, 600, DamageType.Mixed);
        public static Spell.Skillshot Mark = new Spell.Skillshot(SpellSlot.Unknown, 0x640, SkillShotType.Linear, 0, 0x3e8, 60, DamageType.Mixed);
        public static Spell.Skillshot PoroToss = new Spell.Skillshot(SpellSlot.Unknown, 0x640, SkillShotType.Linear, 0, 0x3e8, 60, DamageType.Mixed);
        public static Spell.Targeted Smite = new Spell.Targeted(SpellSlot.Unknown, 680, DamageType.Mixed);
        public static Spell.Active ToTheKing = new Spell.Active(SpellSlot.Unknown, 0x7fffffff, DamageType.Mixed);

        internal static void Initialize()
        {
            Barrier.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonerbarrier");
            Clarity.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonermana");
            Cleanse.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonerboost");
            Exhaust.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonerexhaust");
            Flash.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonerflash");
            Mark.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonersnowball");
            Ghost.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonerhaste");
            Heal.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonerheal");
            Ignite.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonerdot");
            Smite.Slot = Player.Instance.FindSummonerSpellSlotFromName("smite");
            PoroToss.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonerporothrow");
            ToTheKing.Slot = Player.Instance.FindSummonerSpellSlotFromName("summonerpororecall");
        }

        public static bool PlayerHas(SummonerSpellsEnum sumSpell)
        {
            switch (sumSpell)
            {
                case SummonerSpellsEnum.Barrier:
                    return (Barrier.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.Clarity:
                    return (Clarity.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.Cleanse:
                    return (Cleanse.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.Exhaust:
                    return (Exhaust.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.Flash:
                    return (Flash.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.Mark:
                    return (Mark.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.PoroToss:
                    return (PoroToss.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.Ghost:
                    return (Ghost.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.Heal:
                    return (Heal.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.ToTheKing:
                    return (ToTheKing.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.Ignite:
                    return (Ignite.Slot != SpellSlot.Unknown);

                case SummonerSpellsEnum.Smite:
                    return (Smite.Slot != SpellSlot.Unknown);
            }
            return false;
        }
    }
}

