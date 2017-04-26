namespace EloBuddy
{
    using System;

    public class Mastery
    {
        public byte Id;
        public MasteryPage Page;
        public byte Points;

        public Mastery(byte id, MasteryPage page, byte points)
        {
            this.Id = id;
            this.Page = page;
            this.Points = points;
        }
    }
}

