namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class OnResetArgs : EventArgs
    {
        public OnResetArgs(EventArgs args)
        {
        }

        public delegate void OnReset(EventArgs A_0);
    }
}

