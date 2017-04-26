namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ShopActionEventArgs : EventArgs
    {
        private int m_itemId;
        private ItemId m_ItemId;
        private int m_maxStacks;
        private string m_name;
        private int m_price;
        private bool m_process;
        private int[] m_recipeItemIds;
        private AIHeroClient m_sender;

        public ShopActionEventArgs(AIHeroClient sender, int managedItemId, int price, int maxStacks, string name, int[] recipeItemIds)
        {
            this.m_sender = sender;
            this.m_itemId = managedItemId;
            this.m_process = true;
            this.m_price = price;
            this.m_maxStacks = maxStacks;
            this.m_name = name;
            this.m_recipeItemIds = recipeItemIds;
            if ((name.Length == 0) || (this.m_name == "Unknown"))
            {
                this.m_name = Enum.GetName(typeof(ItemId), this.m_ItemId);
            }
        }

        public int Id =>
            this.m_itemId;

        public int MaxStacks =>
            this.m_maxStacks;

        public string Name =>
            this.m_name;

        public int Price =>
            this.m_price;

        public bool Process
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                this.m_process;
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                this.m_process = value;
            }
        }

        public int[] RecipeItemIds =>
            this.m_recipeItemIds;

        public delegate void ShopActionEvent(AIHeroClient sender, ShopActionEventArgs args);
    }
}

