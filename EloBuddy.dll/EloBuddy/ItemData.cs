namespace EloBuddy
{
    using System;

    [Obsolete("Please use EloBuddy.SDK.ItemData")]
    public class ItemData
    {
        private uint m_itemId;

        public ItemData(uint itemId)
        {
            this.m_itemId = itemId;
        }
    }
}

