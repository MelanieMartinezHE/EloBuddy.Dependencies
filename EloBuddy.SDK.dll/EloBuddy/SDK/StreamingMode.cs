namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.Sandbox;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Utils;
    using SharpDX;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal static class StreamingMode
    {
        private static int _lastAttackProcessed;
        private static int _lastMovementProcessed;
        private static int _nextIssueOrderOffset = 250;
        private static int _nextTargetedOffset = 20;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static Vector3 <LastPosition>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static System.Random <Random>k__BackingField;
        private static int MaxDelay;
        private static int MinDelay;
        internal static EloBuddy.SDK.Menu.Menu StreamMenu;

        internal static void Initialize()
        {
            if (!SandboxConfig.IsBuddy)
            {
                Logger.Debug(" - StreamingMode mode is available only for buddy users. -", new object[0]);
            }
            else
            {
                Hacks.DisableDrawings = true;
                Loading.OnLoadingComplete += delegate (EventArgs <args>) {
                    Random = new System.Random(DateTime.Now.Millisecond);
                    LastPosition = Game.CursorPos;
                    Player.OnPostIssueOrder += new PlayerPostIssueOrder(StreamingMode.OnIssueOrder);
                    Spellbook.OnPostCastSpell += new SpellbookPostCastSpell(StreamingMode.OnCastSpell);
                    Messages.RegisterEventHandler<Messages.KeyUp>(new Messages.MessageHandler<Messages.KeyUp>(StreamingMode.OnKeyUp));
                };
                Logger.Debug(" - StreamingMode enabled! Press F4 to disable! -", new object[0]);
            }
        }

        internal static void LoadMenu()
        {
            StreamMenu = Core.Menu.AddSubMenu("Streaming Mode", "streamingmode", null);
            StreamMenu.AddGroupLabel("Click delay");
            StreamMenu.AddLabel("The less the slider values are the faster it will click.", 0x19);
            Slider minDelaySlider = StreamMenu.Add<Slider>("minSlider", new Slider("Minimum delay (Default 150)", 150, 50, 800));
            Slider maxDelaySlider = StreamMenu.Add<Slider>("maxSlider", new Slider("Maximum delay (Default 350)", 350, 150, 0x640));
            MinDelay = minDelaySlider.CurrentValue;
            minDelaySlider.OnValueChange += delegate (ValueBase<int> <sender>, ValueBase<int>.ValueChangeArgs <args>) {
                MinDelay = minDelaySlider.CurrentValue;
                maxDelaySlider.MinValue = minDelaySlider.CurrentValue + 200;
            };
            MaxDelay = maxDelaySlider.CurrentValue;
            maxDelaySlider.OnValueChange += (<sender>, <args>) => (MaxDelay = maxDelaySlider.CurrentValue);
        }

        private static void OnCastSpell(Spellbook sender, SpellbookCastSpellEventArgs args)
        {
            if (sender.Owner.IsMe && ((args.Target != null) && (args.Slot != SpellSlot.Recall)))
            {
                RandomizeTargetedClick(args.Target);
            }
        }

        internal static void OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            if (sender.IsMe && (((args.Order == GameObjectOrder.AttackTo) || (args.Order == GameObjectOrder.AttackUnit)) || (args.Order == GameObjectOrder.MoveTo)))
            {
                bool flag2 = args.TargetPosition.IsInRange(LastPosition, 300f);
                if ((args.Order == GameObjectOrder.AttackTo) || (args.Order == GameObjectOrder.AttackUnit))
                {
                    if ((args.Target != null) && ((Core.GameTickCount > (_lastAttackProcessed + _nextIssueOrderOffset)) || !flag2))
                    {
                        RandomizeTargetedClick(args.Target);
                        _lastAttackProcessed = Core.GameTickCount;
                        LastPosition = args.TargetPosition;
                        _nextIssueOrderOffset = Random.Next(MinDelay, MaxDelay);
                    }
                }
                else if ((args.Order == GameObjectOrder.MoveTo) && ((Core.GameTickCount > (_lastMovementProcessed + _nextIssueOrderOffset)) || !flag2))
                {
                    _lastMovementProcessed = Core.GameTickCount;
                    LastPosition = args.TargetPosition;
                    _nextIssueOrderOffset = Random.Next(MinDelay, MaxDelay);
                }
            }
        }

        internal static void OnKeyUp(Messages.KeyUp args)
        {
            if (args.Key == 0x73)
            {
                IsEnabled = !IsEnabled;
            }
        }

        private static void RandomizeTargetedClick(GameObject target)
        {
            int num = Random.Next(180);
            Vector2 vector = target.Position.To2D();
            Vector2 v = (Player.Instance.Position.To2D() - vector).Normalized();
            Vector2 vector3 = vector + (v.Rotated(((num * 3.141593f) / 180f)) * _nextTargetedOffset);
            _nextTargetedOffset = Random.Next((int) target.BoundingRadius);
        }

        internal static bool IsEnabled
        {
            get => 
                Bootstrap.IsStreamingMode;
            set
            {
                Bootstrap.IsStreamingMode = value;
                Hacks.DisableDrawings = value;
            }
        }

        internal static Vector3 LastPosition
        {
            [CompilerGenerated]
            get => 
                <LastPosition>k__BackingField;
            [CompilerGenerated]
            set
            {
                <LastPosition>k__BackingField = value;
            }
        }

        internal static System.Random Random
        {
            [CompilerGenerated]
            get => 
                <Random>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Random>k__BackingField = value;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly StreamingMode.<>c <>9 = new StreamingMode.<>c();
            public static Loading.LoadingCompleteHandler <>9__14_0;

            internal void <Initialize>b__14_0(EventArgs <args>)
            {
                StreamingMode.Random = new Random(DateTime.Now.Millisecond);
                StreamingMode.LastPosition = Game.CursorPos;
                Player.OnPostIssueOrder += new PlayerPostIssueOrder(StreamingMode.OnIssueOrder);
                Spellbook.OnPostCastSpell += new SpellbookPostCastSpell(StreamingMode.OnCastSpell);
                Messages.RegisterEventHandler<Messages.KeyUp>(new Messages.MessageHandler<Messages.KeyUp>(StreamingMode.OnKeyUp));
            }
        }
    }
}

