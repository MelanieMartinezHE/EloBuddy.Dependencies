namespace EloBuddy.Sandbox
{
    using EloBuddy;
    using EloBuddy.Sandbox.Data;
    using EloBuddy.Sandbox.Shared;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Threading;

    public static class Sandbox
    {
        static Sandbox()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(EloBuddy.Sandbox.Sandbox.DomainOnProcessExit);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(SandboxDomain.DomainOnAssemblyResolve);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(<>c.<>9.<.cctor>b__0_0);
        }

        [Obfuscation(Feature="virtualization", Exclude=false)]
        public static int Bootstrap(string param)
        {
            try
            {
                bool flag = Authenticator.IsBuddy || SandboxConfig.IsBuddy;
                using (WebClient client = new WebClient())
                {
                    string str = client.DownloadString($"https://edge.elobuddy.net/api.php?action=verifyAccount&accountName={SandboxConfig.Username}");
                    if (string.IsNullOrEmpty(str) || !str.Equals("ilovestefsot"))
                    {
                        NativeImports.AllocConsole();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Your account has reached the active game limit. Please upgrade to Buddy. Your active game limit is {flag ? "10" : "2"} active games. 
 Press any key to exit.");
                        Console.ReadKey();
                        Process.Start("https://www.elobuddy.net/account.php");
                        Process.GetCurrentProcess().Kill();
                        return 1;
                    }
                }
                Reload();
                Input.Subscribe();
            }
            catch (Exception exception)
            {
                Logs.Log("Sandbox: Bootstrap error", new object[0]);
                Logs.Log(exception.ToString(), new object[0]);
                return 1;
            }
            return 0;
        }

        private static void CreateApplicationDomain()
        {
            if (SandboxDomain.Instance == null)
            {
                try
                {
                    SandboxDomain.Instance = SandboxDomain.CreateDomain("SandboxDomain");
                    if (SandboxDomain.Instance == null)
                    {
                        Logs.Log("Sandbox: AppDomain creation failed, please report this error!", new object[0]);
                    }
                }
                catch (Exception exception)
                {
                    Logs.Log("Sandbox: Error during AppDomain creation", new object[0]);
                    Logs.Log(exception.ToString(), new object[0]);
                }
            }
        }

        private static void DomainOnProcessExit(object sender, EventArgs e)
        {
            Logs.Log("Sandbox: Shutting down...", new object[0]);
        }

        internal static bool EqualsPublicToken(AssemblyName assemblyName, string publicToken)
        {
            string[] second = new string[] { string.Empty };
            return ((from o in assemblyName.GetPublicKeyToken() select o.ToString("x2")).Concat<string>(second).Aggregate<string>(new Func<string, string, string>(string.Concat)) == publicToken);
        }

        internal static void Load()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            AllAddonsLoaded = false;
            if (SandboxDomain.Instance != null)
            {
                try
                {
                    Logs.Log("Loading EloBuddy", new object[0]);
                    LoadLibrary("7339047cb10f6e86");
                    Logs.Log("Loading EloBuddy.SDK", new object[0]);
                    LoadLibrary("6b574a82b1ea937e");
                    List<SharedAddon> assemblyList = ServiceFactory.CreateProxy<ILoaderService>().GetAssemblyList((int) Game.GameId);
                    object[] args = new object[] { assemblyList.Count, ((assemblyList.Count < 1) || (assemblyList.Count > 1)) ? "s" : "" };
                    Logs.Log("Loading {0} Addon{1}", args);
                    foreach (SharedAddon addon in assemblyList)
                    {
                        SandboxDomain.Instance.LoadAddon(addon.PathToBinary, new string[1]);
                    }
                }
                catch (Exception exception)
                {
                    Logs.Log("Sandbox: Loading assemblies failed", new object[0]);
                    Logs.Log(exception.ToString(), new object[0]);
                    return;
                }
                AllAddonsLoaded = true;
            }
        }

        internal static void LoadLibrary(string publicToken)
        {
            foreach (string str in Directory.GetFiles(SandboxConfig.LibrariesDirectory))
            {
                try
                {
                    if (EqualsPublicToken(AssemblyName.GetAssemblyName(str), publicToken))
                    {
                        SandboxDomain.Instance.LoadAddon(str, new string[1]);
                        break;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        internal static void Recompile()
        {
            UpdateConfig();
            Unload();
            try
            {
                ServiceFactory.CreateProxy<ILoaderService>().Recompile(Process.GetCurrentProcess().Id);
            }
            catch (Exception exception)
            {
                Logs.Log("Sandbox: Remote recompiling failed", new object[0]);
                Logs.Log(exception.ToString(), new object[0]);
            }
            CreateApplicationDomain();
            Load();
        }

        public static void Reload()
        {
            UpdateConfig();
            Unload();
            CreateApplicationDomain();
            Load();
        }

        internal static void Unload()
        {
            AllAddonsLoaded = false;
            if (SandboxDomain.Instance != null)
            {
                try
                {
                    SandboxDomain.UnloadDomain(SandboxDomain.Instance);
                }
                catch (Exception exception)
                {
                    Logs.Log("Sandbox: Unloading AppDomain failed", new object[0]);
                    Logs.Log(exception.ToString(), new object[0]);
                }
                SandboxDomain.Instance = null;
            }
        }

        private static void UpdateConfig()
        {
            SandboxConfig.Reload();
            Hacks.TowerRanges = SandboxConfig.TowerRange;
            Hacks.AntiAFK = SandboxConfig.AntiAfk;
            Hacks.MovementHack = false;
            Hacks.RenderWatermark = true;
            Hacks.ZoomHack = SandboxConfig.ExtendedZoom;
            Hacks.Console = SandboxConfig.Console;
            Hacks.IngameChat = !SandboxConfig.DisableChatFunction;
            Hacks.DisableRangeIndicator = SandboxConfig.DisableRangeIndicator;
        }

        internal static bool AllAddonsLoaded
        {
            [CompilerGenerated]
            get => 
                <AllAddonsLoaded>k__BackingField;
            [CompilerGenerated]
            set
            {
                <AllAddonsLoaded>k__BackingField = value;
            }
        }

        internal static int Pid =>
            Process.GetCurrentProcess().Id;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly EloBuddy.Sandbox.Sandbox.<>c <>9 = new EloBuddy.Sandbox.Sandbox.<>c();
            public static Func<byte, string> <>9__11_0;

            internal void <.cctor>b__0_0(object sender, UnhandledExceptionEventArgs args)
            {
                new PermissionSet(PermissionState.Unrestricted).Assert();
                Logs.Log("Sandbox: Unhandled Sandbox exception", new object[0]);
                Logs.PrintException(args.ExceptionObject);
                CodeAccessPermission.RevertAssert();
            }

            internal string <EqualsPublicToken>b__11_0(byte o) => 
                o.ToString("x2");
        }
    }
}

