namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameObjectTeleportEventArgs : EventArgs
    {
        private string m_recallName;
        private string m_recallType;

        public GameObjectTeleportEventArgs(string recallName, string recallType)
        {
            this.m_recallName = recallName;
            this.m_recallType = recallType;
        }

        public string RecallName =>
            this.m_recallName;

        public string RecallType =>
            this.m_recallType;

        public delegate void GameObjectTeleportEvent(Obj_AI_Base sender, GameObjectTeleportEventArgs args);
    }
}

