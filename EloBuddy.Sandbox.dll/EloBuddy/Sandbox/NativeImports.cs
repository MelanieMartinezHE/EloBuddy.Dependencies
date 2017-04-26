namespace EloBuddy.Sandbox
{
    using System;
    using System.Runtime.InteropServices;

    internal static class NativeImports
    {
        [DllImport("kernel32.dll")]
        internal static extern bool AllocConsole();
    }
}

