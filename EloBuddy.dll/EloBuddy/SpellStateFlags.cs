namespace EloBuddy
{
    using System;

    [Flags]
    public enum SpellStateFlags
    {
        Cooldown = 0x20,
        NoMana = 0x40,
        NotLearned = 4,
        Ready = 0,
        Supressed = 8
    }
}

