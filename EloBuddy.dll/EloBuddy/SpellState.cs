namespace EloBuddy
{
    using System;

    public enum SpellState
    {
        Cooldown = 0x20,
        Disabled = 0x18,
        NoMana = 0x60,
        NotAvailable = 4,
        NotLearned = 12,
        Ready = 0,
        Surpressed = 8,
        Unknown = 0x61
    }
}

