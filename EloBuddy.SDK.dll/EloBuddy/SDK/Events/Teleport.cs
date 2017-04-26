namespace EloBuddy.SDK.Events
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class Teleport
    {
        internal static readonly Dictionary<int, TeleportEventArgs> TeleportDataNetId = new Dictionary<int, TeleportEventArgs>();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event TeleportHandler OnTeleport;

        static Teleport()
        {
            Obj_AI_Base.OnTeleport += new Obj_AI_BaseTeleport(Teleport.OnUnitTeleport);
        }

        internal static int GetDuration(GameObjectTeleportEventArgs args)
        {
            switch (GetType(args))
            {
                case TeleportType.Recall:
                    return GetRecallDuration(args);

                case TeleportType.Teleport:
                    return 0x1194;

                case TeleportType.TwistedFate:
                    return 0x5dc;

                case TeleportType.Shen:
                    return 0xbb8;
            }
            return 0xdac;
        }

        internal static int GetRecallDuration(GameObjectTeleportEventArgs args)
        {
            switch (args.RecallType.ToLower())
            {
                case "recall":
                    return 0x1f40;

                case "recallimproved":
                    return 0x1b58;

                case "odinrecall":
                    return 0x1194;

                case "odinrecallimproved":
                    return 0xfa0;

                case "superrecall":
                    return 0xfa0;

                case "superrecallimproved":
                    return 0xfa0;
            }
            return 0x1f40;
        }

        internal static TeleportType GetType(GameObjectTeleportEventArgs args)
        {
            switch (args.RecallName)
            {
                case "Recall":
                    return TeleportType.Recall;

                case "Teleport":
                    return TeleportType.Teleport;

                case "Gate":
                    return TeleportType.TwistedFate;

                case "Shen":
                    return TeleportType.Shen;
            }
            return TeleportType.Recall;
        }

        private static void OnUnitTeleport(Obj_AI_Base sender, GameObjectTeleportEventArgs args)
        {
            TeleportEventArgs args2 = new TeleportEventArgs {
                Status = TeleportStatus.Unknown,
                Type = TeleportType.Unknown
            };
            if (sender != null)
            {
                if (!TeleportDataNetId.ContainsKey(sender.NetworkId))
                {
                    TeleportDataNetId[sender.NetworkId] = args2;
                }
                if (!string.IsNullOrEmpty(args.RecallType))
                {
                    args2.TeleportName = args.RecallName;
                    args2.Status = TeleportStatus.Start;
                    args2.Duration = GetDuration(args);
                    args2.Type = GetType(args);
                    args2.Start = Core.GameTickCount;
                    TeleportDataNetId[sender.NetworkId] = args2;
                }
                else
                {
                    args2 = TeleportDataNetId[sender.NetworkId];
                    args2.Status = ((Core.GameTickCount - args2.Start) < (args2.Duration - 250)) ? TeleportStatus.Abort : TeleportStatus.Finish;
                }
                if (OnTeleport > null)
                {
                    OnTeleport(sender, args2);
                }
            }
        }

        public class TeleportEventArgs : EventArgs
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Duration>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Start>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private TeleportStatus <Status>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <TeleportName>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private TeleportType <Type>k__BackingField;

            public int Duration { get; internal set; }

            public int Start { get; internal set; }

            public TeleportStatus Status { get; internal set; }

            public string TeleportName { get; internal set; }

            public TeleportType Type { get; internal set; }
        }

        public delegate void TeleportHandler(Obj_AI_Base sender, Teleport.TeleportEventArgs args);
    }
}

