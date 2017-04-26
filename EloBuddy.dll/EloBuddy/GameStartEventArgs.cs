namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameStartEventArgs : EventArgs
    {
        public delegate void GameStartEvent(EventArgs args);
    }
}

