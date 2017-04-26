namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Properties;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class Item
    {
        private float _range;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ItemId <Id>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Dictionary<ItemId, EloBuddy.SDK.ItemInfo> <ItemData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EloBuddy.SDK.ItemInfo <ItemInfo>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <RangeSqr>k__BackingField;

        static Item()
        {
            DefaultContractResolver resolver = new DefaultContractResolver();
            resolver.DefaultMembersSearchFlags |= BindingFlags.NonPublic;
            JsonSerializerSettings settings = new JsonSerializerSettings {
                ContractResolver = resolver
            };
            ItemData = JsonConvert.DeserializeObject<Dictionary<ItemId, EloBuddy.SDK.ItemInfo>>(Resources.ItemData, settings);
        }

        public Item(ItemId id, float range = 0f)
        {
            this.Id = id;
            if (ItemData.ContainsKey(this.Id))
            {
                this.ItemInfo = ItemData[this.Id];
            }
            this.Range = range;
        }

        public Item(int id, float range = 0f)
        {
            this.Id = (ItemId) id;
            if (ItemData.ContainsKey(this.Id))
            {
                this.ItemInfo = ItemData[this.Id];
            }
            this.Range = range;
        }

        public void Buy()
        {
            Shop.BuyItem(this.Id);
        }

        public static bool CanUseItem(ItemId id) => 
            (from slot in Player.Instance.InventoryItems
                where slot.Id == id
                select Player.Instance.Spellbook.Spells.FirstOrDefault<SpellDataInst>(spell => spell.Slot == (slot.Slot + 6)) into inst
                select (inst != null) && (inst.State == SpellState.Ready)).FirstOrDefault<bool>();

        public static bool CanUseItem(int id) => 
            CanUseItem((ItemId) id);

        public static bool CanUseItem(string name) => 
            (from slot in Player.Instance.InventoryItems
                where slot.Name == name
                select Player.Instance.Spellbook.Spells.FirstOrDefault<SpellDataInst>(spell => spell.Slot == (slot.Slot + 6)) into inst
                select (inst != null) && (inst.State == SpellState.Ready)).FirstOrDefault<bool>();

        public bool Cast() => 
            UseItem(this.Id, (Obj_AI_Base) null);

        public bool Cast(Obj_AI_Base target) => 
            UseItem(this.Id, target);

        public bool Cast(Vector2 position) => 
            UseItem(this.Id, position);

        public bool Cast(Vector3 position) => 
            UseItem(this.Id, position);

        public Item[] GetComponents()
        {
            if (this.ItemInfo == null)
            {
                return new Item[0];
            }
            return (from id in this.ItemInfo.FromId select new Item(id, 0f)).ToArray<Item>();
        }

        public Item[] GetUpgrades()
        {
            if (this.ItemInfo == null)
            {
                return new Item[0];
            }
            return (from id in this.ItemInfo.IntoId select new Item(id, 0f)).ToArray<Item>();
        }

        public int GoldRequired() => 
            (this.ItemInfo?.Gold.Base + (from it in this.GetComponents()
                where !it.IsOwned(null)
                select it).Sum<Item>(((Func<Item, int>) (it => it.GoldRequired()))));

        public static bool HasItem(ItemId id, AIHeroClient hero = null) => 
            (hero ?? Player.Instance).InventoryItems.Any<InventorySlot>(slot => (slot.Id == id));

        public static bool HasItem(int id, AIHeroClient hero = null) => 
            HasItem((ItemId) id, hero);

        public static bool HasItem(string name, AIHeroClient hero = null) => 
            (hero ?? Player.Instance).InventoryItems.Any<InventorySlot>(slot => (slot.Name == name));

        public static void Initialize()
        {
        }

        public bool IsInRange(Obj_AI_Base target) => 
            this.IsInRange(target.ServerPosition);

        public bool IsInRange(Vector2 target) => 
            this.IsInRange(target.To3D(0));

        public bool IsInRange(Vector3 target) => 
            (Player.Instance.ServerPosition.Distance(target, true) < this.RangeSqr);

        public bool IsOwned(AIHeroClient target = null) => 
            HasItem(this.Id, target);

        public bool IsReady() => 
            CanUseItem(this.Id);

        public static bool UseItem(ItemId id, Obj_AI_Base target = null) => 
            (from slot in Player.Instance.InventoryItems
                where slot.Id == id
                select (target != null) ? ((IEnumerable<bool>) Player.CastSpell(slot.SpellSlot, target)) : ((IEnumerable<bool>) Player.CastSpell(slot.SpellSlot))).FirstOrDefault<bool>();

        public static bool UseItem(ItemId id, Vector2 position) => 
            UseItem(id, position.To3D(0));

        public static bool UseItem(ItemId id, Vector3 position) => 
            (position.IsZero ? false : (from slot in Player.Instance.InventoryItems
                where slot.Id == id
                select Player.CastSpell(slot.SpellSlot, position)).FirstOrDefault<bool>());

        public static bool UseItem(int id, Obj_AI_Base target = null) => 
            UseItem((ItemId) id, target);

        public static bool UseItem(int id, Vector2 position) => 
            UseItem(id, position.To3D(0));

        public static bool UseItem(int id, Vector3 position) => 
            UseItem((ItemId) id, position);

        public static bool UseItem(string name, Obj_AI_Base target = null) => 
            (from slot in Player.Instance.InventoryItems
                where slot.Name == name
                select (target != null) ? ((IEnumerable<bool>) Player.Instance.Spellbook.CastSpell(slot.SpellSlot, target)) : ((IEnumerable<bool>) Player.CastSpell(slot.SpellSlot))).FirstOrDefault<bool>();

        public ItemId Id { get; private set; }

        public static Dictionary<ItemId, EloBuddy.SDK.ItemInfo> ItemData
        {
            [CompilerGenerated]
            get => 
                <ItemData>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <ItemData>k__BackingField = value;
            }
        }

        public EloBuddy.SDK.ItemInfo ItemInfo { get; private set; }

        public float Range
        {
            get => 
                this._range;
            set
            {
                this._range = value;
                this.RangeSqr = value * value;
            }
        }

        public float RangeSqr { get; private set; }

        public List<SpellSlot> Slots =>
            (from slot in Player.Instance.InventoryItems
                where slot.Id == this.Id
                select slot.SpellSlot).ToList<SpellSlot>();

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Item.<>c <>9 = new Item.<>c();
            public static Func<InventorySlot, SpellSlot> <>9__19_1;
            public static Func<ItemId, Item> <>9__36_0;
            public static Func<ItemId, Item> <>9__37_0;
            public static Func<Item, bool> <>9__38_0;
            public static Func<Item, int> <>9__38_1;
            public static Func<InventorySlot, SpellDataInst> <>9__42_1;
            public static Func<SpellDataInst, bool> <>9__42_3;
            public static Func<InventorySlot, SpellDataInst> <>9__44_1;
            public static Func<SpellDataInst, bool> <>9__44_3;

            internal SpellDataInst <CanUseItem>b__42_1(InventorySlot slot) => 
                Player.Instance.Spellbook.Spells.FirstOrDefault<SpellDataInst>(spell => (spell.Slot == (slot.Slot + 6)));

            internal bool <CanUseItem>b__42_3(SpellDataInst inst) => 
                ((inst != null) && (inst.State == SpellState.Ready));

            internal SpellDataInst <CanUseItem>b__44_1(InventorySlot slot) => 
                Player.Instance.Spellbook.Spells.FirstOrDefault<SpellDataInst>(spell => (spell.Slot == (slot.Slot + 6)));

            internal bool <CanUseItem>b__44_3(SpellDataInst inst) => 
                ((inst != null) && (inst.State == SpellState.Ready));

            internal SpellSlot <get_Slots>b__19_1(InventorySlot slot) => 
                slot.SpellSlot;

            internal Item <GetComponents>b__37_0(ItemId id) => 
                new Item(id, 0f);

            internal Item <GetUpgrades>b__36_0(ItemId id) => 
                new Item(id, 0f);

            internal bool <GoldRequired>b__38_0(Item it) => 
                !it.IsOwned(null);

            internal int <GoldRequired>b__38_1(Item it) => 
                it.GoldRequired();
        }
    }
}

