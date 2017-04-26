namespace EloBuddy.SDK
{
    using EloBuddy;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class Messages
    {
        internal static Vector2 _previousMousePosition = Game.CursorPos2D;
        internal static readonly Dictionary<Type, List<object>> EventHandlers = new Dictionary<Type, List<object>>();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public static  event MessageHandler OnMessage;

        static Messages()
        {
            Game.OnWndProc += new GameWndProc(Messages.OnWndProc);
        }

        internal static void NotifyEventHandlers(WindowMessage message)
        {
            Type key = message.GetType();
            if (EventHandlers.ContainsKey(key))
            {
                EventHandlers[key].ForEach(delegate (object handler) {
                    try
                    {
                        switch (message.Message)
                        {
                            case WindowMessages.KeyDown:
                                ((MessageHandler<KeyDown>) handler)((KeyDown) message);
                                return;

                            case WindowMessages.KeyUp:
                                ((MessageHandler<KeyUp>) handler)((KeyUp) message);
                                return;

                            case WindowMessages.MouseMove:
                                ((MessageHandler<MouseMove>) handler)((MouseMove) message);
                                return;

                            case WindowMessages.LeftButtonDown:
                                ((MessageHandler<LeftButtonDown>) handler)((LeftButtonDown) message);
                                return;

                            case WindowMessages.LeftButtonUp:
                                ((MessageHandler<LeftButtonUp>) handler)((LeftButtonUp) message);
                                return;

                            case WindowMessages.LeftButtonDoubleClick:
                                ((MessageHandler<LeftButtonDoubleClick>) handler)((LeftButtonDoubleClick) message);
                                return;

                            case WindowMessages.RightButtonDown:
                                ((MessageHandler<RightButtonDown>) handler)((RightButtonDown) message);
                                return;

                            case WindowMessages.RightButtonUp:
                                ((MessageHandler<RightButtonUp>) handler)((RightButtonUp) message);
                                return;

                            case WindowMessages.RightButtonDoubleClick:
                                ((MessageHandler<RightButtonDoubleClick>) handler)((RightButtonDoubleClick) message);
                                return;

                            case WindowMessages.MiddleButtonDown:
                                ((MessageHandler<MiddleButtonDown>) handler)((MiddleButtonDown) message);
                                return;

                            case WindowMessages.MiddleButtonUp:
                                ((MessageHandler<MiddleButtonUp>) handler)((MiddleButtonUp) message);
                                return;

                            case WindowMessages.MiddleButtonDoubleClick:
                                ((MessageHandler<MiddleButtonDoubleClick>) handler)((MiddleButtonDoubleClick) message);
                                return;

                            case WindowMessages.MouseWheel:
                                ((MessageHandler<MouseWheel>) handler)((MouseWheel) message);
                                return;
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Error notifying event handler for message '{0}'", message);
                        Console.WriteLine(exception);
                    }
                });
            }
        }

        internal static void OnWndProc(WndEventArgs args)
        {
            if (OnMessage > null)
            {
                WindowMessage message = null;
                switch (args.Msg)
                {
                    case 0x100:
                        NotifyEventHandlers(message = new KeyDown(args));
                        break;

                    case 0x101:
                        NotifyEventHandlers(message = new KeyUp(args));
                        break;

                    case 0x200:
                        NotifyEventHandlers(message = new MouseMove(args));
                        break;

                    case 0x201:
                        NotifyEventHandlers(message = new LeftButtonDown(args));
                        break;

                    case 0x202:
                        NotifyEventHandlers(message = new LeftButtonUp(args));
                        break;

                    case 0x203:
                        NotifyEventHandlers(message = new LeftButtonDoubleClick(args));
                        break;

                    case 0x204:
                        NotifyEventHandlers(message = new RightButtonDown(args));
                        break;

                    case 0x205:
                        NotifyEventHandlers(message = new RightButtonUp(args));
                        break;

                    case 0x206:
                        NotifyEventHandlers(message = new RightButtonDoubleClick(args));
                        break;

                    case 0x207:
                        NotifyEventHandlers(message = new MiddleButtonDown(args));
                        break;

                    case 520:
                        NotifyEventHandlers(message = new MiddleButtonUp(args));
                        break;

                    case 0x209:
                        NotifyEventHandlers(message = new MiddleButtonDoubleClick(args));
                        break;

                    case 0x20a:
                        NotifyEventHandlers(message = new MouseWheel(args));
                        break;
                }
                if (message > null)
                {
                    OnMessage(message);
                    args.Process = message.Process;
                    if (message is MouseEvent)
                    {
                        _previousMousePosition = Game.CursorPos2D;
                    }
                }
            }
        }

        public static void RegisterEventHandler<T>(MessageHandler<T> handler) where T: WindowMessage
        {
            Type key = typeof(T);
            if (!EventHandlers.ContainsKey(key))
            {
                EventHandlers.Add(key, new List<object>());
            }
            EventHandlers[key].Add(handler);
        }

        public static void UnregisterEventHandler<T>(MessageHandler<T> handler) where T: WindowMessage
        {
            Type key = typeof(T);
            if (EventHandlers.ContainsKey(key))
            {
                EventHandlers[key].Remove(handler);
                if (EventHandlers[key].Count == 0)
                {
                    EventHandlers.Remove(key);
                }
            }
        }

        public class KeyDown : Messages.KeyEvent
        {
            public KeyDown(WndEventArgs args) : base(args)
            {
            }
        }

        public abstract class KeyEvent : Messages.WindowMessage
        {
            protected KeyEvent(WndEventArgs args) : base(args)
            {
            }

            public uint Key =>
                base.Handle.WParam;

            public char KeyChar =>
                ((char) base.Handle.WParam);
        }

        public class KeyUp : Messages.KeyEvent
        {
            public KeyUp(WndEventArgs args) : base(args)
            {
            }
        }

        public class LeftButtonDoubleClick : Messages.MouseEvent
        {
            public LeftButtonDoubleClick(WndEventArgs args) : base(args)
            {
            }
        }

        public class LeftButtonDown : Messages.MouseEvent
        {
            public LeftButtonDown(WndEventArgs args) : base(args)
            {
            }
        }

        public class LeftButtonUp : Messages.MouseEvent
        {
            public LeftButtonUp(WndEventArgs args) : base(args)
            {
            }
        }

        public delegate void MessageHandler(Messages.WindowMessage args);

        public delegate void MessageHandler<in T>(T args) where T: Messages.WindowMessage;

        public class MiddleButtonDoubleClick : Messages.MouseEvent
        {
            public MiddleButtonDoubleClick(WndEventArgs args) : base(args)
            {
            }
        }

        public class MiddleButtonDown : Messages.MouseEvent
        {
            public MiddleButtonDown(WndEventArgs args) : base(args)
            {
            }
        }

        public class MiddleButtonUp : Messages.MouseEvent
        {
            public MiddleButtonUp(WndEventArgs args) : base(args)
            {
            }
        }

        public abstract class MouseEvent : Messages.WindowMessage
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Vector2 <MousePosition>k__BackingField;

            protected MouseEvent(WndEventArgs args) : base(args)
            {
                this.MousePosition = Game.CursorPos2D;
            }

            public bool IsCtrlDown =>
                ((base.Handle.WParam & 8) > 0);

            public bool IsLeftButtonDown =>
                ((base.Handle.WParam & 1) > 0);

            public bool IsMiddleButtonDown =>
                ((base.Handle.WParam & 0x10) > 0);

            public bool IsRightButtonDown =>
                ((base.Handle.WParam & 2) > 0);

            public bool IsShiftDown =>
                ((base.Handle.WParam & 4) > 0);

            public Vector2 MousePosition { get; internal set; }

            public Vector2 PreviousMousePosition =>
                Messages._previousMousePosition;
        }

        public class MouseMove : Messages.MouseEvent
        {
            public MouseMove(WndEventArgs args) : base(args)
            {
            }
        }

        public class MouseWheel : Messages.MouseEvent
        {
            public const int WHEEL_DELTA = 120;

            public MouseWheel(WndEventArgs args) : base(args)
            {
            }

            public Directions Direction =>
                ((this.Rotation < 0) ? Directions.Down : Directions.Up);

            public short Rotation =>
                ((short) (base.Handle.WParam >> 0x10));

            public int ScrollSteps =>
                (Math.Abs(this.Rotation) / 120);

            public enum Directions
            {
                Down,
                Up
            }
        }

        public class RightButtonDoubleClick : Messages.MouseEvent
        {
            public RightButtonDoubleClick(WndEventArgs args) : base(args)
            {
            }
        }

        public class RightButtonDown : Messages.MouseEvent
        {
            public RightButtonDown(WndEventArgs args) : base(args)
            {
            }
        }

        public class RightButtonUp : Messages.MouseEvent
        {
            public RightButtonUp(WndEventArgs args) : base(args)
            {
            }
        }

        public abstract class WindowMessage : EventArgs
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private WndEventArgs <Handle>k__BackingField;

            protected WindowMessage(WndEventArgs args)
            {
                this.Handle = args;
            }

            public WndEventArgs Handle { get; protected internal set; }

            public WindowMessages Message =>
                ((WindowMessages) this.Handle.Msg);

            public bool Process
            {
                get => 
                    this.Handle.Process;
                set
                {
                    this.Handle.Process = value;
                }
            }
        }
    }
}

