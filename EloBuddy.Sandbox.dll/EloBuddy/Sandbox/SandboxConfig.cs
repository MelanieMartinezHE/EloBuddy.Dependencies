namespace EloBuddy.Sandbox
{
    using EloBuddy.Sandbox.Data;
    using EloBuddy.Sandbox.Shared;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security;
    using System.Security.Permissions;

    public class SandboxConfig
    {
        public static int MenuKey = 0x10;
        public static int MenuToggleKey = 120;
        public static int ReloadAndRecompileKey = 0x77;
        public static int ReloadKey = 0x74;
        public static bool StreamingMode;
        public static int UnloadKey = 0x75;

        static SandboxConfig()
        {
            Reload();
        }

        [Obfuscation(Feature="virtualization", Exclude=false), PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        internal static void Reload()
        {
            Configuration config = null;
            try
            {
                config = ServiceFactory.CreateProxy<ILoaderService>().GetConfiguration(EloBuddy.Sandbox.Sandbox.Pid);
            }
            catch (Exception exception)
            {
                Logs.Log("Sandbox: Reload, getting configuration failed", new object[0]);
                Logs.Log(exception.ToString(), new object[0]);
            }
            if (config != null)
            {
                Authenticator.Verify(config);
                DataDirectory = config.DataDirectory;
                EloBuddyDllPath = config.EloBuddyDllPath;
                LibrariesDirectory = config.LibrariesDirectory;
                Permissions = config.Permissions;
                MenuKey = config.MenuKey;
                MenuToggleKey = config.MenuToggleKey;
                ReloadKey = config.ReloadKey;
                ReloadAndRecompileKey = config.ReloadAndRecompileKey;
                UnloadKey = config.UnloadKey;
                AntiAfk = config.AntiAfk;
                DisableRangeIndicator = config.DisableRangeIndicator;
                Console = config.Console;
                TowerRange = config.TowerRange;
                ExtendedZoom = config.ExtendedZoom;
                MovementHack = config.MovementHack;
                DrawWaterMark = config.DrawWatermark;
                DisableChatFunction = config.DisableChatFunction;
                StreamingMode = config.StreamingMode;
                IsBuddy = config.IsBuddyEx && Authenticator.IsBuddy;
                LoaderFileData = config.LoaderFileData;
                Username = config.Username;
                PasswordHash = config.PasswordHash;
                Hwid = config.Hwid;
            }
            else
            {
                Logs.Log("Sandbox: Reload, config is null", new object[0]);
            }
        }

        public static bool AntiAfk
        {
            [CompilerGenerated]
            get => 
                <AntiAfk>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <AntiAfk>k__BackingField = value;
            }
        }

        public static bool Console
        {
            [CompilerGenerated]
            get => 
                <Console>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <Console>k__BackingField = value;
            }
        }

        public static string DataDirectory
        {
            [CompilerGenerated]
            get => 
                <DataDirectory>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <DataDirectory>k__BackingField = value;
            }
        }

        public static bool DisableChatFunction
        {
            [CompilerGenerated]
            get => 
                <DisableChatFunction>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <DisableChatFunction>k__BackingField = value;
            }
        }

        public static bool DisableRangeIndicator
        {
            [CompilerGenerated]
            get => 
                <DisableRangeIndicator>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <DisableRangeIndicator>k__BackingField = value;
            }
        }

        public static bool DrawWaterMark
        {
            [CompilerGenerated]
            get => 
                <DrawWaterMark>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <DrawWaterMark>k__BackingField = value;
            }
        }

        public static string EloBuddyDllPath
        {
            [CompilerGenerated]
            get => 
                <EloBuddyDllPath>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <EloBuddyDllPath>k__BackingField = value;
            }
        }

        public static bool ExtendedZoom
        {
            [CompilerGenerated]
            get => 
                <ExtendedZoom>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <ExtendedZoom>k__BackingField = value;
            }
        }

        public static string Hwid
        {
            [CompilerGenerated]
            get => 
                <Hwid>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <Hwid>k__BackingField = value;
            }
        }

        public static bool IsBuddy
        {
            [CompilerGenerated]
            get => 
                <IsBuddy>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <IsBuddy>k__BackingField = value;
            }
        }

        public static string LibrariesDirectory
        {
            [CompilerGenerated]
            get => 
                <LibrariesDirectory>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <LibrariesDirectory>k__BackingField = value;
            }
        }

        public static EloBuddy.Sandbox.Shared.LoaderFileData LoaderFileData
        {
            [CompilerGenerated]
            get => 
                <LoaderFileData>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <LoaderFileData>k__BackingField = value;
            }
        }

        public static bool MovementHack
        {
            [CompilerGenerated]
            get => 
                <MovementHack>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <MovementHack>k__BackingField = value;
            }
        }

        public static string PasswordHash
        {
            [CompilerGenerated]
            get => 
                <PasswordHash>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <PasswordHash>k__BackingField = value;
            }
        }

        public static PermissionSet Permissions
        {
            [CompilerGenerated]
            get => 
                <Permissions>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <Permissions>k__BackingField = value;
            }
        }

        public static bool TowerRange
        {
            [CompilerGenerated]
            get => 
                <TowerRange>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <TowerRange>k__BackingField = value;
            }
        }

        public static string Username
        {
            [CompilerGenerated]
            get => 
                <Username>k__BackingField;
            [CompilerGenerated]
            private set
            {
                <Username>k__BackingField = value;
            }
        }
    }
}

