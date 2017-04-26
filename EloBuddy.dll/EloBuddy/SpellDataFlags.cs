namespace EloBuddy
{
    using System;

    [Flags]
    public enum SpellDataFlags : uint
    {
        AffectBarrackOnly = 0x200000,
        AffectBuildings = 0x1000,
        AffectDead = 0x80000,
        AffectEnemies = 0x400,
        AffectFriends = 0x800,
        AffectHeroes = 0x10000,
        AffectImportantBotTargets = 0x40,
        AffectMinions = 0x8000,
        AffectNeutral = 0x4000,
        AffectNotPet = 0x100000,
        AffectTurrets = 0x20000,
        AffectUntargetable = 0x200,
        AffectUseable = 0x8000000,
        AffectWards = 0x4000000,
        AllowWhileTaunted = 0x80,
        AlwaysSelf = 0x40000,
        Autocast = 2,
        Instacast = 4,
        NonDispellable = 0x10,
        NotAffectSelf = 0x2000,
        NotAffectZombie = 0x100,
        PersistThroughDeath = 8
    }
}

