namespace EloBuddy.SDK
{
    using EloBuddy.Sandbox;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class Bootstrap
    {
        internal static bool _initialized;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsSpectatorMode>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsStreamingMode>k__BackingField;
        internal static readonly Process CurrentProcess = Process.GetCurrentProcess();
        internal static readonly List<Type> SkipInitialization = new List<Type>();

        public static void Init(string[] args)
        {
            if (!_initialized)
            {
                _initialized = true;
                CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                if (SandboxConfig.StreamingMode)
                {
                    IsStreamingMode = true;
                    StreamingMode.Initialize();
                }
                BootstrapRun.Initialize();
            }
        }

        public static bool IsSpectatorMode
        {
            [CompilerGenerated]
            get => 
                <IsSpectatorMode>k__BackingField;
            [CompilerGenerated]
            internal set
            {
                <IsSpectatorMode>k__BackingField = value;
            }
        }

        public static bool IsStreamingMode
        {
            [CompilerGenerated]
            get => 
                <IsStreamingMode>k__BackingField;
            [CompilerGenerated]
            internal set
            {
                <IsStreamingMode>k__BackingField = value;
            }
        }
    }
}

