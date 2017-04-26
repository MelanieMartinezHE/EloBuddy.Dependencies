namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class OnHeroDeathEventArgs : EventArgs
    {
        private float m_deathDuration;
        private Obj_AI_Base m_sender;

        public OnHeroDeathEventArgs(Obj_AI_Base sender, float deathDuration)
        {
            this.m_sender = sender;
            this.m_deathDuration = deathDuration;
        }

        public float DeathDuration =>
            this.m_deathDuration;

        public Obj_AI_Base Sender =>
            this.m_sender;

        public delegate void OnHeroDeath(OnHeroDeathEventArgs args);
    }
}

