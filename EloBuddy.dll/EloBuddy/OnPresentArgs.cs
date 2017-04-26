namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class OnPresentArgs : EventArgs
    {
        public OnPresentArgs(EventArgs args)
        {
        }

        public delegate void OnPresent(EventArgs A_0);
    }
}

