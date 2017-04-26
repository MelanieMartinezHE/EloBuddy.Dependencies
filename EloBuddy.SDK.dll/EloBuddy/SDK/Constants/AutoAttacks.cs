namespace EloBuddy.SDK.Constants
{
    using EloBuddy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class AutoAttacks
    {
        internal static readonly HashSet<string> AutoAttackDatabase;
        internal static readonly Dictionary<Champion, HashSet<string>> AutoAttackResetAnimationsName;
        internal static readonly HashSet<string> AutoAttackResetNamesDatabase;
        private static readonly Dictionary<Champion, SpellSlot> AutoAttackResetSlotsDatabase;
        internal static readonly Dictionary<Champion, SpellSlot> DashAutoAttackResetSlotsDatabase;
        internal static readonly HashSet<string> NoneAutoAttackDatabase;
        internal static readonly Dictionary<Champion, HashSet<string>> SpecialAutoAttacksAnimationName;
        internal static readonly HashSet<Champion> UnabortableAutoDatabase;

        static AutoAttacks()
        {
            HashSet<string> set1 = new HashSet<string> { 
                "caitlynheadshotmissile",
                "frostarrow",
                "garenslash2",
                "kennenmegaproc",
                "masteryidoublestrike",
                "quinnwenhanced",
                "renektonexecute",
                "renektonsuperexecute",
                "rengarnewpassivebuffdash",
                "trundleq",
                "viktorqbuff",
                "xenzhaothrust",
                "xenzhaothrust2",
                "xenzhaothrust3",
                "lucianpassiveshot"
            };
            AutoAttackDatabase = set1;
            HashSet<string> set2 = new HashSet<string> { 
                "volleyattack",
                "volleyattackwithsound",
                "jarvanivcataclysmattack",
                "monkeykingdoubleattack",
                "shyvanadoubleattack",
                "shyvanadoubleattackdragon",
                "zyragraspingplantattack",
                "zyragraspingplantattack2",
                "zyragraspingplantattackfire",
                "zyragraspingplantattack2fire",
                "viktorpowertransfer",
                "sivirwattackbounce",
                "asheqattacknoonhit",
                "elisespiderlingbasicattack",
                "heimertyellowbasicattack",
                "heimertyellowbasicattack2",
                "heimertbluebasicattack",
                "annietibbersbasicattack",
                "annietibbersbasicattack2",
                "yorickdecayedghoulbasicattack",
                "yorickravenousghoulbasicattack",
                "yorickspectralghoulbasicattack",
                "malzaharvoidlingbasicattack",
                "malzaharvoidlingbasicattack2",
                "malzaharvoidlingbasicattack3",
                "kindredwolfbasicattack",
                "gravesautoattackrecoil"
            };
            NoneAutoAttackDatabase = set2;
            HashSet<string> set3 = new HashSet<string> { 
                "jaycehypercharge",
                "shyvanadoubleattack",
                "takedown",
                "itemtitanichydracleave"
            };
            AutoAttackResetNamesDatabase = set3;
            Dictionary<Champion, HashSet<string>> dictionary1 = new Dictionary<Champion, HashSet<string>>();
            HashSet<string> set4 = new HashSet<string> { "Spell5" };
            dictionary1.Add(Champion.Rengar, set4);
            SpecialAutoAttacksAnimationName = dictionary1;
            Dictionary<Champion, HashSet<string>> dictionary2 = new Dictionary<Champion, HashSet<string>>();
            HashSet<string> set5 = new HashSet<string> { 
                "Spell3",
                "Spell3withReload"
            };
            dictionary2.Add(Champion.Graves, set5);
            AutoAttackResetAnimationsName = dictionary2;
            Dictionary<Champion, SpellSlot> dictionary3 = new Dictionary<Champion, SpellSlot> {
                { 
                    Champion.Graves,
                    SpellSlot.E
                },
                { 
                    Champion.Lucian,
                    SpellSlot.E
                },
                { 
                    Champion.Riven,
                    SpellSlot.Q
                },
                { 
                    Champion.Vayne,
                    SpellSlot.Q
                }
            };
            DashAutoAttackResetSlotsDatabase = dictionary3;
            Dictionary<Champion, SpellSlot> dictionary4 = new Dictionary<Champion, SpellSlot> {
                { 
                    Champion.Ashe,
                    SpellSlot.Q
                },
                { 
                    Champion.Blitzcrank,
                    SpellSlot.E
                },
                { 
                    Champion.Camille,
                    SpellSlot.Q
                },
                { 
                    Champion.Darius,
                    SpellSlot.W
                },
                { 
                    Champion.DrMundo,
                    SpellSlot.E
                },
                { 
                    Champion.Fiora,
                    SpellSlot.E
                },
                { 
                    Champion.Gangplank,
                    SpellSlot.Q
                },
                { 
                    Champion.Garen,
                    SpellSlot.Q
                },
                { 
                    Champion.Kassadin,
                    SpellSlot.W
                },
                { 
                    Champion.Illaoi,
                    SpellSlot.W
                },
                { 
                    Champion.Jax,
                    SpellSlot.W
                },
                { 
                    Champion.Katarina,
                    SpellSlot.E
                },
                { 
                    Champion.Leona,
                    SpellSlot.Q
                },
                { 
                    Champion.MasterYi,
                    SpellSlot.W
                },
                { 
                    Champion.Mordekaiser,
                    SpellSlot.Q
                },
                { 
                    Champion.Nautilus,
                    SpellSlot.W
                },
                { 
                    Champion.Nasus,
                    SpellSlot.Q
                },
                { 
                    Champion.RekSai,
                    SpellSlot.Q
                },
                { 
                    Champion.Renekton,
                    SpellSlot.W
                },
                { 
                    Champion.Rengar,
                    SpellSlot.Q
                },
                { 
                    Champion.Sejuani,
                    SpellSlot.W
                },
                { 
                    Champion.Sivir,
                    SpellSlot.W
                },
                { 
                    Champion.Talon,
                    SpellSlot.Q
                },
                { 
                    Champion.Teemo,
                    SpellSlot.Q
                },
                { 
                    Champion.Trundle,
                    SpellSlot.Q
                },
                { 
                    Champion.Vi,
                    SpellSlot.E
                },
                { 
                    Champion.Volibear,
                    SpellSlot.Q
                },
                { 
                    Champion.MonkeyKing,
                    SpellSlot.Q
                },
                { 
                    Champion.XinZhao,
                    SpellSlot.Q
                },
                { 
                    Champion.Yorick,
                    SpellSlot.Q
                }
            };
            AutoAttackResetSlotsDatabase = dictionary4;
            HashSet<Champion> set6 = new HashSet<Champion> {
                Champion.Kalista
            };
            UnabortableAutoDatabase = set6;
        }

        public static bool IsAutoAttack(this GameObjectProcessSpellCastEventArgs args) => 
            ((args.Target != null) && args.SData.IsAutoAttack());

        public static bool IsAutoAttack(this MissileClient missile) => 
            ((missile.Target != null) && missile.SData.IsAutoAttack());

        public static bool IsAutoAttack(this SpellData spellData) => 
            IsAutoAttack(spellData.Name);

        public static bool IsAutoAttack(this SpellDataInst spellDataInst) => 
            IsAutoAttack(spellDataInst.Name);

        public static bool IsAutoAttack(string spellName)
        {
            string item = spellName.ToLower();
            return (AutoAttackDatabase.Contains(item) || (!NoneAutoAttackDatabase.Contains(item) && item.Contains("attack")));
        }

        public static bool IsAutoAttackReset(string spellName) => 
            AutoAttackResetNamesDatabase.Contains(spellName.ToLower());

        public static bool IsAutoAttackReset(AIHeroClient hero, GameObjectProcessSpellCastEventArgs args)
        {
            if (AutoAttackResetSlotsDatabase.ContainsKey(hero.Hero))
            {
                return (((SpellSlot) AutoAttackResetSlotsDatabase[hero.Hero]) == args.Slot);
            }
            return IsAutoAttackReset(args.SData.Name);
        }

        public static bool IsDashAutoAttackReset(AIHeroClient hero, GameObjectPlayAnimationEventArgs args) => 
            (AutoAttackResetAnimationsName.ContainsKey(hero.Hero) && AutoAttackResetAnimationsName[hero.Hero].Contains(args.Animation));

        public static bool IsDashAutoAttackReset(AIHeroClient hero, GameObjectProcessSpellCastEventArgs args) => 
            (DashAutoAttackResetSlotsDatabase.ContainsKey(hero.Hero) && (((SpellSlot) DashAutoAttackResetSlotsDatabase[hero.Hero]) == args.Slot));

        public static string[] AutoAttackResetSpells =>
            AutoAttackResetNamesDatabase.ToArray<string>();

        public static string[] AutoAttackSpells =>
            AutoAttackDatabase.ToArray<string>();

        public static string[] NoneAutoAttackSpells =>
            NoneAutoAttackDatabase.ToArray<string>();

        public static Champion[] UnabortableAutoChamps =>
            UnabortableAutoDatabase.ToArray<Champion>();
    }
}

