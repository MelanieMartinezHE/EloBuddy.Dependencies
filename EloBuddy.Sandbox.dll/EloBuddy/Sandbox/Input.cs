namespace EloBuddy.Sandbox
{
    using EloBuddy;
    using System;

    internal static class Input
    {
        internal static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg == 0x101)
            {
                if (args.WParam == SandboxConfig.ReloadKey)
                {
                    EloBuddy.Sandbox.Sandbox.Reload();
                }
                if (args.WParam == SandboxConfig.ReloadAndRecompileKey)
                {
                    EloBuddy.Sandbox.Sandbox.Recompile();
                }
                if (args.WParam == SandboxConfig.UnloadKey)
                {
                    EloBuddy.Sandbox.Sandbox.Unload();
                }
            }
        }

        internal static void Subscribe()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(SandboxDomain.DomainOnAssemblyResolve);
            Game.OnWndProc += new GameWndProc(Input.Game_OnWndProc);
        }
    }
}

