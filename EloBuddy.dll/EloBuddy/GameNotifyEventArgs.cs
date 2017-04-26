namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameNotifyEventArgs : EventArgs
    {
        private GameEventId m_eventId;
        private uint m_networkId;

        public GameNotifyEventArgs(GameEventId eventId, uint networkId)
        {
            this.m_eventId = eventId;
            this.m_networkId = networkId;
        }

        public GameEventId EventId =>
            this.m_eventId;

        public uint NetworkId =>
            this.m_networkId;

        public delegate void GameNotifyEvent(EventArgs args);
    }
}

