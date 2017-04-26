namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.Sandbox;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Utils;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class AddonManager
    {
        internal static readonly List<Delegate> NotifiedListeners = new List<Delegate>();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event AllAddonsLoadedHandler OnAllAddonsLoaded;

        static AddonManager()
        {
            Game.OnTick += new GameTick(AddonManager.OnTick);
        }

        public static bool IsAddonLoaded(string name)
        {
            name = name.ToLower();
            return LoadedAddons.Any<string>(addon => addon.ToLower().Equals(name));
        }

        internal static void OnTick(EventArgs args)
        {
            if ((EloBuddy.Sandbox.Sandbox.AllAddonsLoaded || Loading._allAddonsLoaded) && (OnAllAddonsLoaded > null))
            {
                foreach (Delegate delegate2 in (from o in OnAllAddonsLoaded.GetInvocationList()
                    where !NotifiedListeners.Contains(o)
                    select o).ToArray<Delegate>())
                {
                    NotifiedListeners.Add(delegate2);
                    try
                    {
                        object[] objArray1 = new object[2];
                        objArray1[1] = EventArgs.Empty;
                        delegate2.DynamicInvoke(objArray1);
                    }
                    catch (Exception exception)
                    {
                        object[] objArray2 = new object[] { exception };
                        Logger.Warn("Failed to notify OnAllAddonsLoaded listener!\n{0}", objArray2);
                    }
                }
            }
        }

        internal static IEnumerable<string> LoadedAddons =>
            (from o in SandboxDomain.LoadedLibraries.Values
                where o != null
                select o.GetName().Name).Concat<string>(SandboxDomain.LoadedAddons);

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly AddonManager.<>c <>9 = new AddonManager.<>c();
            public static Func<Assembly, bool> <>9__6_0;
            public static Func<Assembly, string> <>9__6_1;
            public static Func<Delegate, bool> <>9__8_0;

            internal bool <get_LoadedAddons>b__6_0(Assembly o) => 
                (o != null);

            internal string <get_LoadedAddons>b__6_1(Assembly o) => 
                o.GetName().Name;

            internal bool <OnTick>b__8_0(Delegate o) => 
                !AddonManager.NotifiedListeners.Contains(o);
        }

        public delegate void AllAddonsLoadedHandler(object sender, EventArgs args);
    }
}

