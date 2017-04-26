namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.Sandbox;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class Core
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EloBuddy.SDK.Menu.Menu <Menu>k__BackingField;
        internal static readonly List<DelayedAction> ActionQueue = new List<DelayedAction>();

        static Core()
        {
            Game.OnUpdate += new GameUpdate(Core.OnUpdate);
        }

        public static void DelayAction(Action action, int delayTime)
        {
            DelayedAction item = new DelayedAction {
                Action = action,
                DelayTime = GameTickCount + delayTime
            };
            ActionQueue.Add(item);
        }

        internal static void EndAllDrawing(RenderingType exclusion = 0)
        {
            if ((exclusion != RenderingType.Sprite) && Sprite.IsDrawing)
            {
                Sprite.Handle.End();
                Sprite.IsDrawing = false;
            }
            if ((exclusion != RenderingType.Line) && EloBuddy.SDK.Rendering.Line.IsDrawing)
            {
                EloBuddy.SDK.Rendering.Line.Handle.End();
                EloBuddy.SDK.Rendering.Line.IsDrawing = false;
            }
        }

        internal static void Initialize()
        {
            Menu = MainMenu.AddMenu("Core", "Core", null);
            Slider slider = Menu.Add<Slider>("TicksPerSecond", new Slider("Ticks Per Second", 0x19, 1, 0x4b));
            slider.OnValueChange += (sender, args) => (Game.TicksPerSecond = args.NewValue);
            Game.TicksPerSecond = slider.CurrentValue;
            Menu.AddLabel("         Recommended value: 25.", 0x19);
            Menu.AddLabel("Note: Ticks per second means how often the Game.OnTick event should be fired.", 0x19);
            Menu.AddLabel("This means this option is not a humanizer, just a performance option.", 0x19);
            Menu.AddLabel("Example: 25 t/s means Game.OnTick is being fired 25 times per second.", 0x19);
            Menu.AddLabel("Higher values mean more load each second, which could reduce FPS slightly.", 0x19);
            Gapcloser.AddMenu();
            EloBuddy.SDK.Menu.Menu menu = Menu.AddSubMenu("Hacks", null, null);
            CheckBox box = menu.Add<CheckBox>("IngameChat", new CheckBox("Enable InGame Chat", Hacks.IngameChat));
            box.OnValueChange += (sender, args) => (Hacks.IngameChat = args.NewValue);
            Hacks.IngameChat = box.CurrentValue;
            CheckBox box2 = menu.Add<CheckBox>("AntiAFK", new CheckBox("Enable Anti AFK", Hacks.AntiAFK));
            box2.OnValueChange += (sender, args) => (Hacks.AntiAFK = args.NewValue);
            Hacks.AntiAFK = box2.CurrentValue;
            CheckBox box3 = menu.Add<CheckBox>("MovementHack", new CheckBox("Enable Movement Hack", Hacks.MovementHack));
            box3.OnValueChange += (sender, args) => (Hacks.MovementHack = args.NewValue);
            Hacks.MovementHack = box3.CurrentValue;
            CheckBox box4 = menu.Add<CheckBox>("TowerRanges", new CheckBox("Draw Tower Ranges", Hacks.TowerRanges));
            box4.OnValueChange += (sender, args) => (Hacks.TowerRanges = args.NewValue);
            Hacks.TowerRanges = box4.CurrentValue;
            CheckBox box5 = menu.Add<CheckBox>("RenderWatermark", new CheckBox("Draw EloBuddy Watermark", Hacks.RenderWatermark));
            box5.OnValueChange += (sender, args) => (Hacks.RenderWatermark = args.NewValue);
            Hacks.RenderWatermark = box5.CurrentValue;
            Hacks.ZoomHack = false;
            if (SandboxConfig.IsBuddy)
            {
                StreamingMode.LoadMenu();
            }
        }

        private static void OnUpdate(EventArgs args)
        {
            foreach (DelayedAction action in (from o in ActionQueue
                where o.DelayTime < GameTickCount
                select o).ToArray<DelayedAction>())
            {
                try
                {
                    action.Action();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                if ((action.RepeatEndTime == 0) || (action.RepeatEndTime < GameTickCount))
                {
                    ActionQueue.Remove(action);
                }
            }
        }

        public static void RepeatAction(Action action, int delayTime, int repeatEndTime)
        {
            DelayedAction item = new DelayedAction {
                Action = action,
                DelayTime = GameTickCount + delayTime,
                RepeatEndTime = (GameTickCount + delayTime) + repeatEndTime
            };
            ActionQueue.Add(item);
        }

        public static int GameTickCount =>
            ((int) (Game.Time * 1000f));

        internal static EloBuddy.SDK.Menu.Menu Menu
        {
            [CompilerGenerated]
            get => 
                <Menu>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Menu>k__BackingField = value;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Core.<>c <>9 = new Core.<>c();
            public static ValueBase<int>.ValueChangeHandler <>9__8_0;
            public static ValueBase<bool>.ValueChangeHandler <>9__8_1;
            public static ValueBase<bool>.ValueChangeHandler <>9__8_2;
            public static ValueBase<bool>.ValueChangeHandler <>9__8_3;
            public static ValueBase<bool>.ValueChangeHandler <>9__8_4;
            public static ValueBase<bool>.ValueChangeHandler <>9__8_5;
            public static Func<Core.DelayedAction, bool> <>9__9_0;

            internal void <Initialize>b__8_0(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
            {
                Game.TicksPerSecond = args.NewValue;
            }

            internal void <Initialize>b__8_1(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                Hacks.IngameChat = args.NewValue;
            }

            internal void <Initialize>b__8_2(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                Hacks.AntiAFK = args.NewValue;
            }

            internal void <Initialize>b__8_3(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                Hacks.MovementHack = args.NewValue;
            }

            internal void <Initialize>b__8_4(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                Hacks.TowerRanges = args.NewValue;
            }

            internal void <Initialize>b__8_5(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                Hacks.RenderWatermark = args.NewValue;
            }

            internal bool <OnUpdate>b__9_0(Core.DelayedAction o) => 
                (o.DelayTime < Core.GameTickCount);
        }

        internal class DelayedAction
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private System.Action <Action>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <DelayTime>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <RepeatEndTime>k__BackingField;

            public System.Action Action { get; set; }

            public int DelayTime { get; set; }

            public int RepeatEndTime { get; set; }
        }

        internal enum RenderingType
        {
            None,
            Sprite,
            Line
        }
    }
}

