namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class InventorySlot
    {
        internal unsafe ItemNode* m_itemNode;
        private uint m_networkId;
        private int m_slot;

        public InventorySlot(uint networkId, int slot)
        {
            this.m_networkId = networkId;
            this.m_slot = slot;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool CanUseItem()
        {
            List<EloBuddy.SpellDataInst>.Enumerator enumerator = Player.Spells.GetEnumerator();
            if (enumerator.MoveNext())
            {
                do
                {
                    EloBuddy.SpellDataInst current = enumerator.Current;
                    EloBuddy.SpellSlot slot = (EloBuddy.SpellSlot) (this.m_slot + 6);
                    if (current.Slot == *(((int*) &slot)))
                    {
                        return (bool) ((byte) (current.State == EloBuddy.SpellState.Ready));
                    }
                }
                while (enumerator.MoveNext());
            }
            return false;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public bool Cast()
        {
            EloBuddy.SpellSlot slot = (EloBuddy.SpellSlot) (this.m_slot + 6);
            return Player.CastSpell(slot);
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public bool Cast(EloBuddy.Obj_AI_Base target)
        {
            EloBuddy.SpellSlot slot = (EloBuddy.SpellSlot) (this.m_slot + 6);
            return Player.CastSpell(slot, target);
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public bool Cast(Vector3 position)
        {
            EloBuddy.SpellSlot slot = (EloBuddy.SpellSlot) (this.m_slot + 6);
            return Player.CastSpell(slot, position);
        }

        internal unsafe ItemNode* GetItemNode()
        {
            ItemNode* itemNode = this.m_itemNode;
            if (itemNode != null)
            {
                return itemNode;
            }
            EloBuddy.Native.InventorySlot* ptr = this.GetPtr();
            if (ptr != null)
            {
                InventorySlotNode* nodePtr2 = *((InventorySlotNode**) ptr);
                if (nodePtr2 != null)
                {
                    ItemNode* nodePtr = *((ItemNode**) (nodePtr2 + 12));
                    if (nodePtr != null)
                    {
                        this.m_itemNode = nodePtr;
                    }
                }
            }
            return this.m_itemNode;
        }

        internal unsafe EloBuddy.Native.InventorySlot* GetPtr()
        {
            EloBuddy.Native.Obj_AI_Base* basePtr = EloBuddy.Native.ObjectManager.GetUnitByNetworkId(this.m_networkId);
            if (basePtr != null)
            {
                HeroInventory* inventoryPtr = EloBuddy.Native.Obj_AI_Base.GetInventory(basePtr);
                if (inventoryPtr != null)
                {
                    return EloBuddy.Native.HeroInventory.GetInventorySlot(inventoryPtr, this.m_slot);
                }
            }
            throw new InventorySlotNotFoundException();
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public unsafe bool Sell()
        {
            ItemId unknown;
            ItemNode* itemNode = this.GetItemNode();
            if (itemNode != null)
            {
                unknown = *(EloBuddy.Native.ItemNode.GetItemId(itemNode));
            }
            else
            {
                unknown = ItemId.Unknown;
            }
            return Shop.SellItem(*((int*) &unknown));
        }

        public int Charges
        {
            get
            {
                if (this.GetPtr() != null)
                {
                    return *(EloBuddy.Native.InventorySlot.GetCharges(this.GetPtr()));
                }
                return 0;
            }
        }

        public string Description
        {
            get
            {
                ItemNode* itemNode = this.GetItemNode();
                if (itemNode != null)
                {
                    ItemScript* scriptPtr = *(EloBuddy.Native.ItemNode.GetItemScript(itemNode));
                    if (scriptPtr != null)
                    {
                        ItemId unknown;
                        ItemNode* nodePtr = this.GetItemNode();
                        if (nodePtr != null)
                        {
                            unknown = *(EloBuddy.Native.ItemNode.GetItemId(nodePtr));
                        }
                        else
                        {
                            unknown = ItemId.Unknown;
                        }
                        sbyte modopt(IsSignUnspecifiedByte)* numPtr2 = EloBuddy.Native.ItemScript.GetDescription(scriptPtr, *((int*) &unknown));
                        if (numPtr2 != null)
                        {
                            sbyte modopt(IsSignUnspecifiedByte)* numPtr = EloBuddy.Native.RiotString.TranslateString(numPtr2);
                            if (numPtr != null)
                            {
                                return new string(numPtr);
                            }
                        }
                    }
                }
                return "Unknown";
            }
        }

        public string DisplayName
        {
            get
            {
                ItemNode* itemNode = this.GetItemNode();
                if (itemNode != null)
                {
                    ItemScript* scriptPtr = *(EloBuddy.Native.ItemNode.GetItemScript(itemNode));
                    if (scriptPtr != null)
                    {
                        ItemId unknown;
                        ItemNode* nodePtr = this.GetItemNode();
                        if (nodePtr != null)
                        {
                            unknown = *(EloBuddy.Native.ItemNode.GetItemId(nodePtr));
                        }
                        else
                        {
                            unknown = ItemId.Unknown;
                        }
                        sbyte modopt(IsSignUnspecifiedByte)* numPtr2 = EloBuddy.Native.ItemScript.GetDisplayName(scriptPtr, *((int*) &unknown));
                        if (numPtr2 != null)
                        {
                            sbyte modopt(IsSignUnspecifiedByte)* numPtr = EloBuddy.Native.RiotString.TranslateString(numPtr2);
                            if (numPtr != null)
                            {
                                return new string(numPtr);
                            }
                        }
                    }
                }
                return "Unknown";
            }
        }

        public ItemId Id
        {
            get
            {
                ItemNode* itemNode = this.GetItemNode();
                if (itemNode != null)
                {
                    return *(EloBuddy.Native.ItemNode.GetItemId(itemNode));
                }
                return ItemId.Unknown;
            }
        }

        public bool IsWard
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                int[] numArray = new int[] { 0x7fc, 0x7fd, 0x801, 0x802, 0xc52, 0xd0c, 0xd16, 0xd17 };
                int index = 0;
                if (0 < numArray.Length)
                {
                    do
                    {
                        if (numArray[index] == this.Id)
                        {
                            return true;
                        }
                        index++;
                    }
                    while (index < numArray.Length);
                }
                return false;
            }
        }

        public string Name
        {
            get
            {
                ItemId unknown;
                ItemNode* itemNode = this.GetItemNode();
                string str = "Unknown";
                if (itemNode == null)
                {
                    return str;
                }
                str = new string(EloBuddy.Native.ItemNode.GetName(itemNode));
                if (!string.IsNullOrEmpty(str) && (str != "Unknown"))
                {
                    return str;
                }
                ItemNode* nodePtr = this.GetItemNode();
                if (nodePtr != null)
                {
                    unknown = *(EloBuddy.Native.ItemNode.GetItemId(nodePtr));
                }
                else
                {
                    unknown = ItemId.Unknown;
                }
                return Enum.GetName(typeof(ItemId), (ItemId) *(((int*) &unknown)));
            }
        }

        public int Price
        {
            get
            {
                ItemNode* itemNode = this.GetItemNode();
                if (itemNode != null)
                {
                    return *(EloBuddy.Native.ItemNode.GetPrice(itemNode));
                }
                return 0;
            }
        }

        public int Slot =>
            this.m_slot;

        public EloBuddy.SpellSlot SpellSlot =>
            ((EloBuddy.SpellSlot) (this.m_slot + 6));

        public int Stacks
        {
            get
            {
                if (this.GetPtr() != null)
                {
                    return *(EloBuddy.Native.InventorySlot.GetStacks(this.GetPtr()));
                }
                return 0;
            }
        }

        public string Tooltip
        {
            get
            {
                ItemNode* itemNode = this.GetItemNode();
                if (itemNode != null)
                {
                    ItemScript* scriptPtr = *(EloBuddy.Native.ItemNode.GetItemScript(itemNode));
                    if (scriptPtr != null)
                    {
                        ItemId unknown;
                        ItemNode* nodePtr = this.GetItemNode();
                        if (nodePtr != null)
                        {
                            unknown = *(EloBuddy.Native.ItemNode.GetItemId(nodePtr));
                        }
                        else
                        {
                            unknown = ItemId.Unknown;
                        }
                        sbyte modopt(IsSignUnspecifiedByte)* numPtr2 = EloBuddy.Native.ItemScript.GetTooltip(scriptPtr, *((int*) &unknown));
                        if (numPtr2 != null)
                        {
                            sbyte modopt(IsSignUnspecifiedByte)* numPtr = EloBuddy.Native.RiotString.TranslateString(numPtr2);
                            if (numPtr != null)
                            {
                                return new string(numPtr);
                            }
                        }
                    }
                }
                return "Unknown";
            }
        }
    }
}

