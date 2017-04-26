namespace EloBuddy
{
    using System;

    [Flags]
    public enum CollisionFlags : short
    {
        Building = 0x40,
        GlobalVision = 0x100,
        Grass = 1,
        None = 0,
        Prop = 0x80,
        Wall = 2
    }
}

