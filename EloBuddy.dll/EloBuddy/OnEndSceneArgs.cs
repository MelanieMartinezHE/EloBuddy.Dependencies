namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class OnEndSceneArgs : EventArgs
    {
        public OnEndSceneArgs(EventArgs args)
        {
        }

        public delegate void OnEndScene(EventArgs A_0);
    }
}

