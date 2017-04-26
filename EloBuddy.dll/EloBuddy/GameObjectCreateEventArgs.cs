namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class GameObjectCreateEventArgs : EventArgs
    {
        private EloBuddy.GameObject obj;

        public GameObjectCreateEventArgs(EloBuddy.GameObject sender)
        {
            this.obj = sender;
        }

        public EloBuddy.GameObject GameObject =>
            this.obj;

        public delegate void GameObjectCreateEvent(GameObject sender, EventArgs args);
    }
}

