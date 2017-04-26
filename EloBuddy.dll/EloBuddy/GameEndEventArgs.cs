namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameEndEventArgs : EventArgs
    {
        private int m_winningTeam;

        public GameEndEventArgs(int winningTeam)
        {
            this.m_winningTeam = winningTeam;
        }

        public GameObjectTeam LosingTeam =>
            ((this.m_winningTeam == 100) ? GameObjectTeam.Chaos : GameObjectTeam.Order);

        public GameObjectTeam WinningTeam =>
            ((GameObjectTeam) this.m_winningTeam);

        public delegate void GameEndEvent(EventArgs args);
    }
}

