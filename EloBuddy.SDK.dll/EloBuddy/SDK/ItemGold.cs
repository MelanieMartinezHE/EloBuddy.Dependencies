namespace EloBuddy.SDK
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class ItemGold
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Base>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Purchasable>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Sell>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Total>k__BackingField;

        private ItemGold()
        {
        }

        public int Base { get; private set; }

        public bool Purchasable { get; private set; }

        public int Sell { get; private set; }

        public int Total { get; private set; }
    }
}

