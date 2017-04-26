namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameUpdateEventArgs : EventArgs
    {
        public delegate void GameOnUpdateEvent(EventArgs args);
    }
}

