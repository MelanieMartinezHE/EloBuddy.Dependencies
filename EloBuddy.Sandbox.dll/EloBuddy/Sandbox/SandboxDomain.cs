namespace EloBuddy.Sandbox
{
    using EloBuddy.Sandbox.Shared;
    using EloBuddy.Sandbox.Utils;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Security.Policy;
    using System.Text.RegularExpressions;

    internal class SandboxDomain : MarshalByRefObject
    {
        internal static readonly List<string> LoadedAddons = new List<string>();
        internal static readonly Dictionary<string, Assembly> LoadedLibraries = new Dictionary<string, Assembly>();

        static SandboxDomain()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(SandboxDomain.DomainOnAssemblyResolve);
        }

        internal static Assembly AddonLoadFrom(string path)
        {
            if (IsSystemAssembly(path))
            {
                return Assembly.LoadFrom(path);
            }
            byte[] buffer = System.IO.File.ReadAllBytes(path);
            buffer.IsDosExecutable();
            if (buffer != null)
            {
                return Assembly.Load(buffer);
            }
            return null;
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        internal static SandboxDomain CreateDomain(string domainName)
        {
            SandboxDomain domain = null;
            try
            {
                if (string.IsNullOrEmpty(domainName))
                {
                    domainName = "Sandbox" + Guid.NewGuid().ToString("N") + "Domain";
                }
                AppDomainSetup info = new AppDomainSetup {
                    ApplicationName = domainName,
                    ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\"
                };
                PermissionSet grantSet = new PermissionSet(PermissionState.None);
                grantSet.AddPermission(new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME"));
                grantSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, Assembly.GetExecutingAssembly().Location));
                grantSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, SandboxConfig.DataDirectory));
                grantSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\..\"))));
                grantSet.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\..\"))));
                grantSet.AddPermission(new ReflectionPermission(PermissionState.Unrestricted));
                grantSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                grantSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Infrastructure));
                grantSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration));
                grantSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.SerializationFormatter));
                grantSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
                grantSet.AddPermission(new UIPermission(PermissionState.Unrestricted));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(\w+)\.lolnexus\.com\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(\w+)\.riotgames\.com\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(www\.)?champion\.gg\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(www\.)?elobuddy\.net\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/edge\.elobuddy\.net\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(www\.)?leaguecraft\.com\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(www\.)?lolbuilder\.net\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(www\.|raw.)?github(usercontent)?\.com\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(www|oce|las|ru|br|lan|tr|euw|na|eune|sk2)\.op\.gg\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/ddragon\.leagueoflegends\.com\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(global\.)?api\.pvp\.net\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"http?:\/\/strefainformatyka\.hekko24\.pl\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/strefainformatyka\.hekko24\.pl\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(www\.)?oktw\.me\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/localhost\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"http?:\/\/(www\.)?ebkappa\.bplaced\.net\/.*")));
                grantSet.AddPermission(new WebPermission(NetworkAccess.Connect, new Regex(@"https?:\/\/(www\.)?akaeb\.com\/.*")));
                if (SandboxConfig.Permissions != null)
                {
                    using (IEnumerator enumerator = SandboxConfig.Permissions.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            IPermission current = (IPermission) enumerator.Current;
                        }
                    }
                }
                StrongName[] second = new StrongName[] { Assembly.GetExecutingAssembly().Evidence.GetHostEvidence<StrongName>() };
                AppDomain domain2 = AppDomain.CreateDomain(domainName, null, info, grantSet, PublicKeys.AllKeys.Concat<StrongName>(second).ToArray<StrongName>());
                domain = (SandboxDomain) Activator.CreateInstanceFrom(domain2, Assembly.GetExecutingAssembly().Location, typeof(SandboxDomain).FullName).Unwrap();
                if (domain != null)
                {
                    domain.DomainHandle = domain2;
                    domain.Initialize();
                }
            }
            catch (Exception exception)
            {
                Logs.Log("Sandbox: An exception occurred creating the AppDomain!", new object[0]);
                Logs.Log(exception.ToString(), new object[0]);
            }
            return domain;
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        internal static Assembly DomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly executingAssembly = null;
            try
            {
                if (args.Name.Contains(".resources"))
                {
                    return null;
                }
                AssemblyName assemblyName = new AssemblyName(args.Name);
                string key = assemblyName.GenerateToken();
                if (Assembly.GetExecutingAssembly().FullName.Equals(args.Name))
                {
                    executingAssembly = Assembly.GetExecutingAssembly();
                }
                else if (EloBuddy.Sandbox.Sandbox.EqualsPublicToken(assemblyName, "7339047cb10f6e86"))
                {
                    executingAssembly = Assembly.LoadFrom(SandboxConfig.EloBuddyDllPath);
                }
                else
                {
                    string str2;
                    if (FindAddon(assemblyName, out str2) && VerifyFilePath(str2))
                    {
                        if (LoadedLibraries.ContainsKey(key))
                        {
                            executingAssembly = LoadedLibraries[key];
                        }
                        else
                        {
                            executingAssembly = Assembly.LoadFrom(str2);
                            LoadedLibraries.Add(key, executingAssembly);
                            if (executingAssembly.IsFullyTrusted && EloBuddy.Sandbox.Sandbox.EqualsPublicToken(assemblyName, "6b574a82b1ea937e"))
                            {
                                InitSDKBootstrap(executingAssembly);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logs.Log("Sandbox: Failed to resolve addon:", new object[0]);
                Logs.Log(exception.ToString(), new object[0]);
            }
            if (executingAssembly != null)
            {
                bool isFullyTrusted = executingAssembly.IsFullyTrusted;
            }
            return executingAssembly;
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        internal static bool FindAddon(AssemblyName assemblyName, out string resolvedPath)
        {
            resolvedPath = "";
            string[] textArray1 = new string[] { SandboxConfig.LibrariesDirectory, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) };
            foreach (string str in (from directory in textArray1
                where (directory != null) && Directory.Exists(directory)
                select directory).SelectMany<string, string>(new Func<string, IEnumerable<string>>(Directory.EnumerateFiles)))
            {
                try
                {
                    if (AssemblyName.GetAssemblyName(str).Name.Equals(assemblyName.Name))
                    {
                        resolvedPath = str;
                        return true;
                    }
                }
                catch (Exception)
                {
                }
            }
            object[] args = new object[] { assemblyName.Name };
            Logs.Log("Sandbox: Could not find addon '{0}'", args);
            return false;
        }

        internal void Initialize()
        {
            this.DomainHandle.UnhandledException += new UnhandledExceptionEventHandler(SandboxDomain.OnUnhandledException);
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        public override object InitializeLifetimeService() => 
            null;

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        private static void InitSDKBootstrap(Assembly sdk)
        {
            sdk.GetType("EloBuddy.SDK.Bootstrap").GetMethod("Init", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[1]);
        }

        internal static bool IsSystemAssembly(string path)
        {
            if (!path.EndsWith(".dll"))
            {
                return Path.GetDirectoryName(path).EndsWith("Libraries");
            }
            return true;
        }

        internal bool LoadAddon(string path, string[] args)
        {
            AssemblyName assemblyName = null;
            try
            {
                if (System.IO.File.Exists(path))
                {
                    assemblyName = AssemblyName.GetAssemblyName(path);
                    this.DomainHandle.ExecuteAssemblyByName(assemblyName, args);
                    if (!LoadedAddons.Contains(assemblyName.Name))
                    {
                        LoadedAddons.Add(assemblyName.Name);
                    }
                    return true;
                }
            }
            catch (MissingMethodException)
            {
                if ((assemblyName != null) && !LoadedLibraries.ContainsKey(assemblyName.GenerateToken()))
                {
                    try
                    {
                        Assembly sdk = this.DomainHandle.Load(assemblyName);
                        LoadedLibraries[assemblyName.GenerateToken()] = sdk;
                        if (sdk.IsFullyTrusted && EloBuddy.Sandbox.Sandbox.EqualsPublicToken(assemblyName, "6b574a82b1ea937e"))
                        {
                            InitSDKBootstrap(sdk);
                        }
                        return true;
                    }
                    catch (Exception exception)
                    {
                        Logs.Log("Sandbox: Failed to call Bootstrap for EloBuddy.SDK", new object[0]);
                        Logs.Log(exception.ToString(), new object[0]);
                    }
                }
            }
            catch (Exception exception2)
            {
                Logs.Log("Sandbox: Failed to load addon", new object[0]);
                Logs.Log(exception2.ToString(), new object[0]);
            }
            return false;
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Logs.Log("Sandbox: Unhandled addon exception:", new object[0]);
            Logs.PrintException(unhandledExceptionEventArgs.ExceptionObject);
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        internal static void UnloadDomain(SandboxDomain domain)
        {
            AppDomain.Unload(domain.DomainHandle);
        }

        [Obfuscation(Feature="virtualization", Exclude=false)]
        private static bool VerifyFilePath(string path)
        {
            if (SandboxConfig.LoaderFileData == null)
            {
                return false;
            }
            string name = Path.GetFileName(path);
            string str = Md5Hash.ComputeFromFile(path);
            FileData data = SandboxConfig.LoaderFileData.AddonFiles.FirstOrDefault<FileData>(f => Path.GetFileName(f.Path) == name);
            if (data != null)
            {
                if (!Md5Hash.Compare(str, data.Hash, false))
                {
                    return false;
                }
                if (data.RequiresBuddy)
                {
                    return SandboxConfig.IsBuddy;
                }
                return true;
            }
            data = SandboxConfig.LoaderFileData.SystemFiles.FirstOrDefault<FileData>(f => Path.GetFileName(f.Path) == name);
            return ((data != null) && Md5Hash.Compare(str, data.Hash, false));
        }

        internal AppDomain DomainHandle { get; private set; }

        internal static SandboxDomain Instance
        {
            [CompilerGenerated]
            get => 
                <Instance>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Instance>k__BackingField = value;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SandboxDomain.<>c <>9 = new SandboxDomain.<>c();
            public static Func<string, bool> <>9__4_0;

            internal bool <FindAddon>b__4_0(string directory) => 
                ((directory != null) && Directory.Exists(directory));
        }
    }
}

