namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.Sandbox;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal static class BootstrapRun
    {
        internal static readonly Dictionary<Action, string> AlwaysLoadAction;
        internal static readonly Dictionary<Action, string> ToLoadActions;

        static BootstrapRun()
        {
            Dictionary<Action, string> dictionary = new Dictionary<Action, string> {
                { 
                    new Action(MainMenu.Initialize),
                    null
                },
                { 
                    new Action(EntityManager.Initialize),
                    null
                },
                { 
                    new Action(Item.Initialize),
                    null
                },
                { 
                    new Action(SpellDatabase.Initialize),
                    "SpellDatabase loaded."
                }
            };
            AlwaysLoadAction = dictionary;
            dictionary = new Dictionary<Action, string> {
                { 
                    new Action(Core.Initialize),
                    null
                },
                { 
                    new Action(TargetSelector.Initialize),
                    "TargetSelector loaded."
                },
                { 
                    new Action(Orbwalker.Initialize),
                    "Orbwalker loaded."
                },
                { 
                    new Action(Prediction.Initialize),
                    "Prediction loaded."
                },
                { 
                    new Action(DamageLibrary.Initialize),
                    "DamageLibrary loaded."
                },
                { 
                    new Action(SummonerSpells.Initialize),
                    "SummonerSpells loaded."
                }
            };
            ToLoadActions = dictionary;
        }

        internal static void Initialize()
        {
            Loading.Initialize();
            Loading.AsyncLockedActions.Add(delegate {
                try
                {
                    if (Player.Instance > null)
                    {
                        Logger.Info("----------------------------------", new object[0]);
                        Logger.Info("Loading SDK Bootstrap", new object[0]);
                        Logger.Info("----------------------------------", new object[0]);
                        Loading.Locked = false;
                        return;
                    }
                }
                catch (Exception)
                {
                }
                Bootstrap.IsSpectatorMode = true;
                Logger.Info("-----------------------------------", new object[0]);
                Logger.Info("Spectating game, have fun watching!", new object[0]);
                Logger.Info("-----------------------------------", new object[0]);
                Loading.Locked = false;
            });
            Loading.AlwaysLoadActions.Add(delegate {
                Game.TicksPerSecond = 0x19;
                foreach (KeyValuePair<Action, string> pair in AlwaysLoadAction)
                {
                    TryLoad(pair.Key, pair.Value);
                }
                AlwaysLoadAction.Clear();
            });
            Loading.OnLoadingComplete += delegate (EventArgs <args>) {
                Auth.Initialize();
                if (!verifyConfig())
                {
                    Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Error, "SDK Bootstrap error", new object[0]);
                }
                else
                {
                    foreach (KeyValuePair<Action, string> pair in ToLoadActions)
                    {
                        TryLoad(pair.Key, pair.Value);
                    }
                    ToLoadActions.Clear();
                    if (SandboxConfig.IsBuddy)
                    {
                        Chat.Print("<font color=\"#0080ff\" >>> Welcome back, Buddy</font>");
                    }
                    Logger.Info("----------------------------------", new object[0]);
                    Logger.Info("SDK Bootstrap fully loaded!", new object[0]);
                    Logger.Info("----------------------------------", new object[0]);
                }
            };
        }

        internal static void TryLoad(Action action, string message)
        {
            try
            {
                if (Bootstrap.SkipInitialization.Contains(action.GetType()))
                {
                    Logger.Debug("Skipping initialization for " + action.GetType().Name, new object[0]);
                }
                else
                {
                    action();
                    if (!string.IsNullOrWhiteSpace(message))
                    {
                        Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Info, message, new object[0]);
                    }
                }
            }
            catch (Exception exception)
            {
                object[] args = new object[] { exception };
                Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Error, "SDK Bootstrap error:\n{0}", args);
            }
        }

        private static bool verifyConfig()
        {
            if ((string.IsNullOrEmpty(SandboxConfig.Username) || string.IsNullOrEmpty(SandboxConfig.PasswordHash)) || string.IsNullOrEmpty(SandboxConfig.Hwid))
            {
                return false;
            }
            if (SandboxConfig.Hwid.Count<char>(x => (x == '-')) != 4)
            {
                return false;
            }
            return true;
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly BootstrapRun.<>c <>9 = new BootstrapRun.<>c();
            public static Func<char, bool> <>9__0_0;
            public static Action <>9__3_0;
            public static Action <>9__3_1;
            public static Loading.LoadingCompleteHandler <>9__3_2;

            internal void <Initialize>b__3_0()
            {
                try
                {
                    if (Player.Instance > null)
                    {
                        Logger.Info("----------------------------------", new object[0]);
                        Logger.Info("Loading SDK Bootstrap", new object[0]);
                        Logger.Info("----------------------------------", new object[0]);
                        Loading.Locked = false;
                        return;
                    }
                }
                catch (Exception)
                {
                }
                Bootstrap.IsSpectatorMode = true;
                Logger.Info("-----------------------------------", new object[0]);
                Logger.Info("Spectating game, have fun watching!", new object[0]);
                Logger.Info("-----------------------------------", new object[0]);
                Loading.Locked = false;
            }

            internal void <Initialize>b__3_1()
            {
                Game.TicksPerSecond = 0x19;
                foreach (KeyValuePair<Action, string> pair in BootstrapRun.AlwaysLoadAction)
                {
                    BootstrapRun.TryLoad(pair.Key, pair.Value);
                }
                BootstrapRun.AlwaysLoadAction.Clear();
            }

            internal void <Initialize>b__3_2(EventArgs <args>)
            {
                Auth.Initialize();
                if (!BootstrapRun.verifyConfig())
                {
                    Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Error, "SDK Bootstrap error", new object[0]);
                }
                else
                {
                    foreach (KeyValuePair<Action, string> pair in BootstrapRun.ToLoadActions)
                    {
                        BootstrapRun.TryLoad(pair.Key, pair.Value);
                    }
                    BootstrapRun.ToLoadActions.Clear();
                    if (SandboxConfig.IsBuddy)
                    {
                        Chat.Print("<font color=\"#0080ff\" >>> Welcome back, Buddy</font>");
                    }
                    Logger.Info("----------------------------------", new object[0]);
                    Logger.Info("SDK Bootstrap fully loaded!", new object[0]);
                    Logger.Info("----------------------------------", new object[0]);
                }
            }

            internal bool <verifyConfig>b__0_0(char x) => 
                (x == '-');
        }
    }
}

