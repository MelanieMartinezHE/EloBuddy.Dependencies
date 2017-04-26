namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class OnBeginSceneArgs : EventArgs
    {
        public OnBeginSceneArgs(EventArgs args)
        {
        }

        public delegate void OnBeginScene(EventArgs A_0);
    }
}

