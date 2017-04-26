namespace EloBuddy
{
    using System;

    [Flags]
    public enum GameObjectCharacterState
    {
        Asleep = 0x400,
        CanAttack = 1,
        CanCast = 2,
        CanMove = 4,
        Charmed = 0x4000,
        DisableAmbientGold = 0x40000,
        DisableAmbientXP = 0x80000,
        DodgePiercing = 0x20000,
        Feared = 0x80,
        Fleeing = 0x100,
        ForceRenderParticles = 0x10000,
        Ghosted = 0x1000,
        GhostProof = 0x2000,
        Immovable = 8,
        IsStealth = 0x10,
        NearSight = 0x800,
        NoRender = 0x8000,
        RevealSpecificUnit = 0x20,
        Surpressed = 0x200,
        Taunted = 0x40
    }
}

