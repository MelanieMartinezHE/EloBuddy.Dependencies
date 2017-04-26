namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class OnDrawArgs : EventArgs
    {
        public OnDrawArgs(EventArgs args)
        {
        }

        public delegate void OnDraw(EventArgs A_0);
    }
}

