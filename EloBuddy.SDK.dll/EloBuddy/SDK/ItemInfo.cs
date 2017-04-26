namespace EloBuddy.SDK
{
    using EloBuddy;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public sealed class ItemInfo
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Consumed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <ConsumeOnFull>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? <depth>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Description>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <From>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ItemGold <Gold>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Group>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <HideFromAll>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ItemImage <Image>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool? <inStore>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int[] <Into>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<int, bool> <Maps>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Plaintext>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <RequiredChampion>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <SpecialRecipe>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float? <stacks>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ItemStats <Stats>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Tags>k__BackingField;

        private ItemInfo()
        {
        }

        public bool AvailableForMap =>
            (!this.Maps.ContainsKey((int) Game.MapId) ? false : this.Maps[(int) Game.MapId]);

        public bool Consumed { get; private set; }

        public bool ConsumeOnFull { get; private set; }

        private int? depth { get; set; }

        public int Depth
        {
            get
            {
                int? depth = this.depth;
                return (depth.HasValue ? depth.GetValueOrDefault() : 1);
            }
        }

        public string Description { get; private set; }

        public int[] From { get; private set; }

        public ItemId[] FromId =>
            ((this.From != null) ? (from i in this.From select (ItemId) i).ToArray<ItemId>() : new ItemId[0]);

        public ItemGold Gold { get; private set; }

        public string Group { get; private set; }

        public bool HideFromAll { get; private set; }

        public ItemImage Image { get; private set; }

        private bool? inStore { get; set; }

        public bool InStore
        {
            get
            {
                bool? inStore = this.inStore;
                return (inStore.HasValue ? inStore.GetValueOrDefault() : true);
            }
        }

        public int[] Into { get; private set; }

        public ItemId[] IntoId =>
            ((this.Into != null) ? (from i in this.Into select (ItemId) i).ToArray<ItemId>() : new ItemId[0]);

        public Dictionary<int, bool> Maps { get; private set; }

        public string Name { get; private set; }

        public string Plaintext { get; private set; }

        public string RequiredChampion { get; private set; }

        public float SpecialRecipe { get; private set; }

        private float? stacks { get; set; }

        public float Stacks
        {
            get
            {
                float? stacks = this.stacks;
                return (stacks.HasValue ? stacks.GetValueOrDefault() : 1f);
            }
        }

        public ItemStats Stats { get; private set; }

        public string[] Tags { get; private set; }

        public bool ValidForPlayer =>
            (string.IsNullOrEmpty(this.RequiredChampion) || (Player.Instance.ChampionName == this.RequiredChampion));

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly EloBuddy.SDK.ItemInfo.<>c <>9 = new EloBuddy.SDK.ItemInfo.<>c();
            public static Func<int, ItemId> <>9__84_0;
            public static Func<int, ItemId> <>9__86_0;

            internal ItemId <get_FromId>b__84_0(int i) => 
                ((ItemId) i);

            internal ItemId <get_IntoId>b__86_0(int i) => 
                ((ItemId) i);
        }
    }
}

