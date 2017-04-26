namespace EloBuddy.SDK.Constants
{
    using EloBuddy;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class Dashes
    {
        internal static List<DashSpell> DashSpells = new List<DashSpell>();

        static Dashes()
        {
            if (DashSpells <= null)
            {
                List<DashSpell> list1 = new List<DashSpell> {
                    new DashSpell("Aatrox", SpellSlot.Q, 650, 0x7d0, 600),
                    new DashSpell("Ahri", SpellSlot.R, 450, 0x6d6, 0x7d),
                    new DashSpell("Akali", SpellSlot.R, 700, 0x7d0, 100),
                    new DashSpell("Alistar", SpellSlot.W, 650, 0x76c, 100)
                };
                DashSpell item = new DashSpell("Azir", SpellSlot.E, 0x9c4, 0x4e2, 100) {
                    RequireTargetBaseSkinName = "AzirSoldier"
                };
                list1.Add(item);
                list1.Add(new DashSpell("Braum", SpellSlot.W, 650, 0x5dc, 250));
                list1.Add(new DashSpell("Diana", SpellSlot.R, 0x339, 0x7d0, 100));
                DashSpell spell2 = new DashSpell("Caitlyn", SpellSlot.E, 390, 0x3e8, 250) {
                    FixedRange = true,
                    Inverted = true
                };
                list1.Add(spell2);
                DashSpell spell3 = new DashSpell("Camille", SpellSlot.E, 800, 900, 0) {
                    Name = "CamilleEDash2"
                };
                list1.Add(spell3);
                list1.Add(new DashSpell("Corki", SpellSlot.W, 0x339, 0x41a, 100));
                DashSpell spell4 = new DashSpell("Ekko", SpellSlot.E, 300, 910, 100) {
                    FixedRange = true
                };
                list1.Add(spell4);
                DashSpell spell5 = new DashSpell("Fizz", SpellSlot.Q, 550, 0x578, 100) {
                    FixedRange = true
                };
                list1.Add(spell5);
                DashSpell spell6 = new DashSpell("Gragas", SpellSlot.E, 600, 0x3e8, 250) {
                    FixedRange = true
                };
                list1.Add(spell6);
                list1.Add(new DashSpell("Gnar", SpellSlot.E, 0x1db, 900, 0));
                list1.Add(new DashSpell("Galio", SpellSlot.E, 800, 0x672, 500));
                DashSpell spell7 = new DashSpell("Graves", SpellSlot.E, 0x1a9, 0x4e2, 100) {
                    FixedRange = true
                };
                list1.Add(spell7);
                list1.Add(new DashSpell("Irelia", SpellSlot.Q, 650, 0x898, 100));
                DashSpell spell8 = new DashSpell("Sejuani", SpellSlot.Q, 600, 0x3e8, 50) {
                    FixedRange = true
                };
                list1.Add(spell8);
                list1.Add(new DashSpell("Shen", SpellSlot.E, 600, 0x3e8, 50));
                list1.Add(new DashSpell("Jax", SpellSlot.Q, 700, 0x578, 50));
                list1.Add(new DashSpell("Kindred", SpellSlot.Q, 340, 0x546, 50));
                list1.Add(new DashSpell("Kled", SpellSlot.E, 550, 0x5dc, 50));
                DashSpell spell9 = new DashSpell("Khazix", SpellSlot.E, 700, 0x4e2, 0) {
                    Name = "KhazixE"
                };
                list1.Add(spell9);
                DashSpell spell10 = new DashSpell("Khazix", SpellSlot.E, 900, 0x4e2, 0) {
                    Name = "KhazixELong"
                };
                list1.Add(spell10);
                DashSpell spell11 = new DashSpell("Leblanc", SpellSlot.W, 550, 0x640, 100) {
                    Name = "LeblancSlide"
                };
                list1.Add(spell11);
                DashSpell spell12 = new DashSpell("LeeSin", SpellSlot.Q, 0x9c4, 0x7d0, 0) {
                    Name = "BlindMonkQTwo",
                    RequireTargetBuff = "BlindMonkQOne"
                };
                list1.Add(spell12);
                DashSpell spell13 = new DashSpell("LeeSin", SpellSlot.W, 700, 0x7d0, 100) {
                    Name = "BlindMonkWOne"
                };
                list1.Add(spell13);
                list1.Add(new DashSpell("Lucian", SpellSlot.E, 0x1a9, 0x546, 100));
                list1.Add(new DashSpell("Maokai", SpellSlot.W, 0x20d, 0x7d0, 250));
                DashSpell spell14 = new DashSpell("Nidalee", SpellSlot.W, 0x177, 950, 250) {
                    Name = "Pounce",
                    FixedRange = true
                };
                list1.Add(spell14);
                list1.Add(new DashSpell("Pantheon", SpellSlot.W, 600, 0x3e8, 250));
                list1.Add(new DashSpell("Quinn", SpellSlot.E, 700, 0x4e2, 0));
                list1.Add(new DashSpell("Fiora", SpellSlot.Q, 400, 0xbb8, 100));
                DashSpell spell15 = new DashSpell("RekSai", SpellSlot.E, 750, 600, 150) {
                    Name = "RekSaiEBurrowed",
                    FixedRange = true
                };
                list1.Add(spell15);
                DashSpell spell16 = new DashSpell("RekSai", (SpellSlot) 0x3e, 750, 600, 150) {
                    Name = "RekSaiTunnelTime",
                    RequireTargetBaseSkinName = "RekSaiTunnelEnd"
                };
                list1.Add(spell16);
                DashSpell spell17 = new DashSpell("Riven", SpellSlot.Q, 250, 560, 250) {
                    FixedRange = true
                };
                list1.Add(spell17);
                list1.Add(new DashSpell("Riven", SpellSlot.E, 300, 0x4b0, 250));
                list1.Add(new DashSpell("Renekton", SpellSlot.E, 450, 0x4e2, 250));
                list1.Add(new DashSpell("Tristana", SpellSlot.W, 900, 0x44c, 300));
                list1.Add(new DashSpell("Tryndamere", SpellSlot.E, 650, 900, 250));
                list1.Add(new DashSpell("Talon", SpellSlot.Q, 650, 0x7d0, 100));
                list1.Add(new DashSpell("Vayne", SpellSlot.Q, 300, 910, 100));
                list1.Add(new DashSpell("MonkeyKing", SpellSlot.E, 650, 0x578, 100));
                list1.Add(new DashSpell("Yasuo", SpellSlot.E, 0x1db, 0x7d0, 50));
                DashSpells = list1;
            }
        }

        public static List<DashSpell> DashData(this GameObjectProcessSpellCastEventArgs args, Obj_AI_Base caster)
        {
            Obj_AI_Base targetBase = args.Target as Obj_AI_Base;
            AIHeroClient heroCaster = caster as AIHeroClient;
            return DashSpells.FindAll(d => (((((heroCaster == null) || string.IsNullOrEmpty(d.Champion)) || heroCaster.ChampionName.Equals(d.Champion, StringComparison.CurrentCultureIgnoreCase)) && (string.IsNullOrEmpty(d.Name) || d.Name.Equals(args.SData.Name, StringComparison.CurrentCultureIgnoreCase))) && (args.Slot.Equals(d.Slot) && (((targetBase == null) || string.IsNullOrEmpty(d.RequireTargetBuff)) || targetBase.HasBuff(d.RequireTargetBuff)))) && (((targetBase == null) || string.IsNullOrEmpty(d.RequireTargetBaseSkinName)) || targetBase.BaseSkinName.Equals(d.RequireTargetBaseSkinName, StringComparison.CurrentCultureIgnoreCase)));
        }

        internal static List<DashSpell> GetDashData(this AIHeroClient target) => 
            DashSpells.FindAll(d => d.Champion.Equals(target.ChampionName));

        public static bool IsDash(this GameObjectProcessSpellCastEventArgs args, Obj_AI_Base caster) => 
            args.DashData(caster).Any<DashSpell>();

        public class DashSpell
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <CastDelay>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Champion>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <FixedRange>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <Inverted>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Name>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Range>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <RequireTargetBaseSkinName>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <RequireTargetBuff>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SpellSlot <Slot>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Speed>k__BackingField;

            internal DashSpell(string champ, SpellSlot slot, int range, int speed = 0x7fffffff, int delay = 0)
            {
                this.Champion = champ;
                this.Slot = slot;
                this.Range = range;
                this.CastDelay = delay;
                this.Speed = speed;
            }

            public int CastDelay { get; internal set; }

            internal string Champion { get; set; }

            internal bool FixedRange { get; set; }

            internal bool Inverted { get; set; }

            internal string Name { get; set; }

            internal int Range { get; set; }

            internal string RequireTargetBaseSkinName { get; set; }

            internal string RequireTargetBuff { get; set; }

            public SpellSlot Slot { get; internal set; }

            public int Speed { get; internal set; }

            public bool Targted =>
                (!string.IsNullOrEmpty(this.RequireTargetBaseSkinName) || !string.IsNullOrEmpty(this.RequireTargetBuff));
        }
    }
}

