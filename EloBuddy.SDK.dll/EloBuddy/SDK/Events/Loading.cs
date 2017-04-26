namespace EloBuddy.SDK.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Utils;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class Loading
    {
        internal static bool _allAddonsLoaded;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsLoadingComplete>k__BackingField;
        internal static readonly List<Action> AlwaysLoadActions = new List<Action>();
        internal static readonly List<Action> AsyncLockedActions = new List<Action>();
        internal static readonly List<Delegate> LoadingCompleteNotified = new List<Delegate>();
        internal static bool Locked = true;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event LoadingCompleteHandler OnLoadingComplete;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event LoadingCompleteHandler OnLoadingCompleteSpectatorMode;

        internal static void CallLoadingComplete()
        {
            if (Locked)
            {
                foreach (Action action in AsyncLockedActions)
                {
                    try
                    {
                        Action asyncAction = action;
                        ThreadPool.QueueUserWorkItem(<state> => asyncAction(), null);
                    }
                    catch (Exception exception)
                    {
                        Logger.Warn("Failed to load async locked action, SDK internal error!", new object[0]);
                        Logger.Error(exception.ToString(), new object[0]);
                    }
                }
                AsyncLockedActions.Clear();
            }
            else
            {
                foreach (Action action2 in AlwaysLoadActions)
                {
                    try
                    {
                        action2();
                    }
                    catch (Exception exception2)
                    {
                        Logger.Warn("Failed to load internal action, SDK error!", new object[0]);
                        Logger.Error(exception2.ToString(), new object[0]);
                    }
                }
                AlwaysLoadActions.Clear();
                LoadingCompleteHandler handler = Bootstrap.IsSpectatorMode ? OnLoadingCompleteSpectatorMode : OnLoadingComplete;
                if (handler > null)
                {
                    foreach (Delegate delegate2 in (from o in handler.GetInvocationList()
                        where !LoadingCompleteNotified.Contains(o)
                        select o).ToArray<Delegate>())
                    {
                        LoadingCompleteNotified.Add(delegate2);
                        try
                        {
                            object[] args = new object[] { EventArgs.Empty };
                            delegate2.DynamicInvoke(args);
                        }
                        catch (Exception exception3)
                        {
                            object[] objArray2 = new object[] { exception3 };
                            Logger.Log(EloBuddy.SDK.Enumerations.LogLevel.Warn, "Failed to notify OnLoadingComlete listener!\n{0}", objArray2);
                        }
                    }
                }
                _allAddonsLoaded = true;
            }
        }

        internal static void Initialize()
        {
            Game.OnTick += new GameTick(Loading.OnTick);
        }

        internal static void OnTick(EventArgs args)
        {
            if (((Game.Mode == GameMode.Running) || (Game.Mode == GameMode.Paused)) || (Game.Mode == GameMode.Finished))
            {
                IsLoadingComplete = true;
                CallLoadingComplete();
            }
        }

        public static bool IsLoadingComplete
        {
            [CompilerGenerated]
            get => 
                <IsLoadingComplete>k__BackingField;
            [CompilerGenerated]
            internal set
            {
                <IsLoadingComplete>k__BackingField = value;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Loading.<>c <>9 = new Loading.<>c();
            public static Func<Delegate, bool> <>9__18_1;

            internal bool <CallLoadingComplete>b__18_1(Delegate o) => 
                !Loading.LoadingCompleteNotified.Contains(o);
        }

        public delegate void LoadingCompleteHandler(EventArgs args);
    }
}

