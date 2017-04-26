namespace EloBuddy.SDK.Menu.Values
{
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Rendering;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class KeyBind : ValueBase<bool>
    {
        internal bool _drawHeader;
        internal Tuple<uint, uint> _keys;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BindTypes <BindType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Tuple<KeyButtonHandle, KeyButtonHandle> <Buttons>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CheckBoxHandle <ControlHandle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <DefaultValue>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Tuple<Text, Text> <HeaderText>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Tuple<string, string> <KeyStrings>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Text <TextHandle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private KeyButtonHandle <WaitingForInput>k__BackingField;
        internal const string BindKeyText = "Press a new key";
        internal static readonly List<uint> CurrentlyDownKeys = new List<uint>();
        internal static readonly List<string> DeserializationNeededKeys;
        internal const uint EscapeKey = 0x1b;
        internal const int HeaderHeight = 0x19;
        internal static readonly Dictionary<uint, string> ReadableKeys;
        public const uint UnboundKey = 0x1b;
        internal const string UnboundText = "---";

        static KeyBind()
        {
            Dictionary<uint, string> dictionary1 = new Dictionary<uint, string> {
                { 
                    8,
                    "Backspace"
                },
                { 
                    9,
                    "Tab"
                },
                { 
                    13,
                    "Enter (Return)"
                },
                { 
                    0x10,
                    "Shift"
                },
                { 
                    0x11,
                    "Control (CTRL)"
                },
                { 
                    0x13,
                    "Pause"
                },
                { 
                    20,
                    "Shift (Toggle)"
                },
                { 
                    0x1b,
                    "Escape (ESC)"
                },
                { 
                    0x20,
                    "Spacebar"
                },
                { 
                    0x21,
                    "Page Up"
                },
                { 
                    0x22,
                    "Page Down"
                },
                { 
                    0x23,
                    "End"
                },
                { 
                    0x24,
                    "Home"
                },
                { 
                    0x25,
                    "Left"
                },
                { 
                    0x26,
                    "Up"
                },
                { 
                    0x27,
                    "Right"
                },
                { 
                    40,
                    "Down"
                },
                { 
                    0x2d,
                    "Insert"
                },
                { 
                    0x2e,
                    "Delete"
                },
                { 
                    0x6a,
                    "* (NumPad)"
                },
                { 
                    0x6b,
                    "+ (NumPad)"
                },
                { 
                    0x6d,
                    "- (NumPad)"
                },
                { 
                    110,
                    "Delete (NumPad)"
                },
                { 
                    0x6f,
                    "/ (NumPad)"
                },
                { 
                    0x90,
                    "Num Lock (Toggle)"
                },
                { 
                    0x91,
                    "Scroll"
                },
                { 
                    0xc0,
                    "`"
                }
            };
            ReadableKeys = dictionary1;
            List<string> list1 = new List<string> { 
                "CurrentValue",
                "Key1",
                "Key2"
            };
            DeserializationNeededKeys = list1;
            for (uint i = 0x41; i <= 90; i++)
            {
                ReadableKeys.Add(i, Convert.ToString((char) i));
            }
            for (uint j = 0x30; j <= 0x39; j++)
            {
                ReadableKeys.Add(j, Convert.ToString((char) j));
            }
            int num = 0;
            for (uint k = 0x60; k <= 0x69; k++)
            {
                ReadableKeys.Add(k, num++ + " (NumPad)");
            }
            num = 1;
            for (uint m = 0x70; m <= 0x7b; m++)
            {
                ReadableKeys.Add(m, "F" + num++);
            }
            Messages.OnMessage += new Messages.MessageHandler(KeyBind.OnMessage);
        }

        public KeyBind(string displayName, bool defaultValue, BindTypes bindType, Tuple<uint, uint> defaultKeys) : base(displayName, 0x19)
        {
            Text text;
            CheckBoxHandle handle1 = new CheckBoxHandle(bindType) {
                IsActive = defaultValue
            };
            this.ControlHandle = handle1;
            Text text1 = new Text(this.DisplayName, ValueBase.DefaultFont) {
                TextOrientation = Text.Orientation.Bottom,
                Color = ValueBase.DefaultColorGold
            };
            this.TextHandle = text = text1;
            base.TextObjects.Add(text);
            this.DefaultValue = defaultValue;
            this.CurrentValue = defaultValue;
            this.BindType = bindType;
            this.Keys = defaultKeys;
            this.Buttons = new Tuple<KeyButtonHandle, KeyButtonHandle>(new KeyButtonHandle(this.KeyStrings.Item1, this, true), new KeyButtonHandle(this.KeyStrings.Item2, this, false));
            this.OnThemeChange();
            this.ControlHandle.OnActiveStateChanged += (sender, args) => (this.CurrentValue = sender.IsActive);
            base.Add(this.ControlHandle);
            base.Add(this.Buttons.Item1);
            base.Add(this.Buttons.Item2);
            this.Buttons.Item1.OnActiveStateChanged += new DynamicControl.DynamicControlHandler(this.OnActiveStateChanged);
            this.Buttons.Item2.OnActiveStateChanged += new DynamicControl.DynamicControlHandler(this.OnActiveStateChanged);
        }

        public KeyBind(string displayName, bool defaultValue, BindTypes bindType, char defaultKey1, char defaultKey2) : this(displayName, defaultValue, bindType, new Tuple<uint, uint>(char.ToUpper(defaultKey1), char.ToUpper(defaultKey2)))
        {
        }

        public KeyBind(string displayName, bool defaultValue, BindTypes bindType, uint defaultKey1 = 0x1b, uint defaultKey2 = 0x1b) : this(displayName, defaultValue, bindType, new Tuple<uint, uint>(defaultKey1, defaultKey2))
        {
        }

        protected internal override bool ApplySerializedData(Dictionary<string, object> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (base.ApplySerializedData(data))
            {
                if (DeserializationNeededKeys.Any<string>(key => !data.ContainsKey(key)))
                {
                    return false;
                }
                this.CurrentValue = (bool) data["CurrentValue"];
                this.Keys = new Tuple<uint, uint>(Convert.ToUInt32(data["Key1"]), Convert.ToUInt32(data["Key2"]));
                return true;
            }
            return false;
        }

        public override bool Draw()
        {
            if (base.Draw())
            {
                if (this.DrawHeader)
                {
                    this.HeaderText.Item1.Draw();
                    this.HeaderText.Item2.Draw();
                }
                return true;
            }
            return false;
        }

        internal void OnActiveStateChanged(Control sender, EventArgs args)
        {
            KeyButtonHandle handle = (KeyButtonHandle) sender;
            this.WaitingForInput = handle.IsActive ? handle : null;
        }

        protected internal override bool OnKeyDown(Messages.KeyDown args)
        {
            if (!base.OnKeyDown(args))
            {
                return false;
            }
            if (this.WaitingForInput > null)
            {
                this.Keys = new Tuple<uint, uint>(this.WaitingForInput.IsFirstButton ? args.Key : this.Keys.Item1, !this.WaitingForInput.IsFirstButton ? args.Key : this.Keys.Item2);
                this.WaitingForInput.IsActive = false;
            }
            if (args.Key != 0x1b)
            {
                BindTypes bindType = this.BindType;
                if (bindType != BindTypes.HoldActive)
                {
                    if ((bindType == BindTypes.PressToggle) && ((this.Keys.Item1 == args.Key) || (this.Keys.Item2 == args.Key)))
                    {
                        this.ControlHandle.IsActive = !this.ControlHandle.IsActive;
                    }
                }
                else
                {
                    CurrentlyDownKeys.Add(args.Key);
                    this.UpdateHoldActiveState();
                }
            }
            return true;
        }

        protected internal override bool OnKeyUp(Messages.KeyUp args)
        {
            if (base.OnKeyUp(args))
            {
                if ((args.Key != 0x1b) && (this.BindType == BindTypes.HoldActive))
                {
                    CurrentlyDownKeys.RemoveAll(o => o == args.Key);
                    this.UpdateHoldActiveState();
                }
                return true;
            }
            return false;
        }

        internal static void OnMessage(Messages.WindowMessage args)
        {
            switch (args.Message)
            {
                case WindowMessages.KeyDown:
                    CurrentlyDownKeys.Add(args.Handle.WParam);
                    break;

                case WindowMessages.KeyUp:
                    CurrentlyDownKeys.RemoveAll(o => o == args.Handle.WParam);
                    break;
            }
        }

        protected internal override void OnThemeChange()
        {
            base.OnThemeChange();
            this.ControlHandle.AlignOffset = new Vector2(0f, ((25f - this.ControlHandle.Size.Y) / 2f) + (this.DrawHeader ? ((float) 0x19) : ((float) 0)));
            this.TextHandle.Padding = new Vector2(this.ControlHandle.Size.Y, ((float) (0x19 - this.TextHandle.Bounding.Height)) / -2f);
            this.TextHandle.Width = (int) ((ValueBase.DefaultWidth - ((ValueBase.DefaultWidth * 0.25f) * 2f)) - this.ControlHandle.Size.Y);
            this.TextHandle.ApplyToControlPosition(this);
            if (this.DrawHeader)
            {
                this.HeaderText.Item1.Padding = new Vector2((this.Width - ((ValueBase.DefaultWidth * 0.25f) * 2f)) + (((ValueBase.DefaultWidth * 0.25f) - this.HeaderText.Item1.Bounding.Width) / 2f), ((float) (0x19 - this.HeaderText.Item1.Bounding.Height)) / 2f);
                this.HeaderText.Item2.Padding = new Vector2((this.Width - (ValueBase.DefaultWidth * 0.25f)) + (((ValueBase.DefaultWidth * 0.25f) - this.HeaderText.Item1.Bounding.Width) / 2f), ((float) (0x19 - this.HeaderText.Item1.Bounding.Height)) / 2f);
                this.HeaderText.Item1.ApplyToControlPosition(this);
                this.HeaderText.Item2.ApplyToControlPosition(this);
            }
        }

        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> dictionary = base.Serialize();
            dictionary.Add("CurrentValue", this.CurrentValue);
            dictionary.Add("Key1", this.Keys.Item1);
            dictionary.Add("Key2", this.Keys.Item2);
            return dictionary;
        }

        internal static string UnicodeToKeyBindString(uint character)
        {
            if (character == 0x1b)
            {
                return "---";
            }
            return UnicodeToReadableString(character);
        }

        public static string UnicodeToReadableString(uint character) => 
            (ReadableKeys.ContainsKey(character) ? ReadableKeys[character] : $"Unknown ({Convert.ToString(character)})");

        internal void UpdateHoldActiveState()
        {
            this.ControlHandle.IsActive = this.DefaultValue ? CurrentlyDownKeys.All<uint>(o => ((o != this.Keys.Item1) && (o != this.Keys.Item2))) : CurrentlyDownKeys.Any<uint>(o => ((o == this.Keys.Item1) || (o == this.Keys.Item2)));
        }

        public BindTypes BindType { get; internal set; }

        internal Tuple<KeyButtonHandle, KeyButtonHandle> Buttons { get; set; }

        internal CheckBoxHandle ControlHandle { get; set; }

        public override bool CurrentValue
        {
            get => 
                base.CurrentValue;
            set
            {
                if (this.BindType == BindTypes.HoldActive)
                {
                    if (this.ControlHandle.IsActive != base.CurrentValue)
                    {
                        base.CurrentValue = this.ControlHandle.IsActive;
                    }
                }
                else
                {
                    this.ControlHandle.IsActive = value;
                    base.CurrentValue = value;
                }
            }
        }

        public bool DefaultValue { get; internal set; }

        public override string DisplayName
        {
            get => 
                base.DisplayName;
            set
            {
                base.DisplayName = value;
                this.TextHandle.TextValue = value;
            }
        }

        internal bool DrawHeader
        {
            get => 
                this._drawHeader;
            set
            {
                if (this._drawHeader != value)
                {
                    this._drawHeader = value;
                    if (value && (this.HeaderText == null))
                    {
                        Text text1 = new Text("Key 1", ValueBase.DefaultFont) {
                            Color = ValueBase.DefaultColorGreen,
                            TextOrientation = Text.Orientation.Top
                        };
                        Text text2 = new Text("Key 2", ValueBase.DefaultFont) {
                            Color = ValueBase.DefaultColorGreen,
                            TextOrientation = Text.Orientation.Top
                        };
                        this.HeaderText = new Tuple<Text, Text>(text1, text2);
                    }
                    base.Height = 0x19 + (value ? 0x19 : 0);
                    this.RecalculateBounding();
                    base.ContainerView.UpdateChildrenCropping();
                }
            }
        }

        internal Tuple<Text, Text> HeaderText { get; set; }

        public Tuple<uint, uint> Keys
        {
            get => 
                this._keys;
            set
            {
                this._keys = value;
                this.KeyStrings = new Tuple<string, string>(UnicodeToKeyBindString(value.Item1), UnicodeToKeyBindString(value.Item2));
                if (this.Buttons > null)
                {
                    this.Buttons.Item1.KeyText = this.KeyStrings.Item1;
                    this.Buttons.Item2.KeyText = this.KeyStrings.Item2;
                }
            }
        }

        public Tuple<string, string> KeyStrings { get; internal set; }

        public override Vector2 Position
        {
            get => 
                base.Position;
            internal set
            {
                base.Position = value;
                if (this.DrawHeader)
                {
                    this.HeaderText.Item1.ApplyToControlPosition(this);
                    this.HeaderText.Item2.ApplyToControlPosition(this);
                }
            }
        }

        internal Text TextHandle { get; set; }

        public override string VisibleName =>
            this.TextHandle.DisplayedText;

        internal KeyButtonHandle WaitingForInput { get; set; }

        public enum BindTypes
        {
            HoldActive,
            PressToggle
        }

        internal sealed class CheckBoxHandle : DynamicControl
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private KeyBind.BindTypes <BindType>k__BackingField;

            internal CheckBoxHandle(KeyBind.BindTypes bindType) : base(ThemeManager.SpriteType.ControlCheckBox)
            {
                this.BindType = bindType;
                this.OnThemeChange();
            }

            internal override bool CallLeftMouseDown()
            {
                if (this.BindType == KeyBind.BindTypes.HoldActive)
                {
                    return false;
                }
                return base.CallLeftMouseDown();
            }

            internal override bool CallLeftMouseUp()
            {
                if (this.BindType == KeyBind.BindTypes.HoldActive)
                {
                    return false;
                }
                return base.CallLeftMouseUp();
            }

            internal KeyBind.BindTypes BindType { get; set; }

            public override DynamicControl.States CurrentState
            {
                get => 
                    base.CurrentState;
                internal set
                {
                    if (this.CurrentState != value)
                    {
                        if (this.BindType != KeyBind.BindTypes.HoldActive)
                        {
                            base.CurrentState = value;
                        }
                        else
                        {
                            switch (value)
                            {
                                case DynamicControl.States.ActiveDown:
                                case DynamicControl.States.ActiveHover:
                                    base.CurrentState = DynamicControl.States.ActiveNormal;
                                    return;

                                case DynamicControl.States.Down:
                                case DynamicControl.States.Hover:
                                    base.CurrentState = DynamicControl.States.Normal;
                                    return;
                            }
                            base.CurrentState = value;
                        }
                    }
                }
            }
        }

        internal sealed class KeyButtonHandle : DynamicControl
        {
            internal SharpDX.Rectangle _rectangle;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsFirstButton>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private KeyBind <ParentHandle>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Text <TextHandle>k__BackingField;
            internal const float WidthMultiplier = 0.25f;

            public KeyButtonHandle(string keyText, KeyBind parent, bool isFirstButton = false) : base(ThemeManager.SpriteType.Empty)
            {
                Text text;
                Text text1 = new Text(keyText, ValueBase.DefaultFont) {
                    TextAlign = Text.Align.Center,
                    Color = ValueBase.DefaultColorGold
                };
                this.TextHandle = text = text1;
                base.TextObjects.Add(text);
                this.KeyText = keyText;
                this.ParentHandle = parent;
                this.IsFirstButton = isFirstButton;
                this.OnThemeChange();
            }

            public override bool Draw() => 
                true;

            internal void OnMessage(Messages.WindowMessage args)
            {
                if (args.Message == WindowMessages.LeftButtonDown)
                {
                    this.OnMouseDown((Messages.LeftButtonDown) args);
                }
            }

            internal void OnMouseDown(Messages.LeftButtonDown args)
            {
                this.IsActive = base.IsMouseInside;
            }

            protected internal override void OnThemeChange()
            {
                this._rectangle = new SharpDX.Rectangle(0, 0, this.Width, 0x19);
                Theme.DynamicRectangle rectangle1 = new Theme.DynamicRectangle {
                    Offset = new Vector2((float) (ValueBase.DefaultWidth - (this.Width * (this.IsFirstButton ? 2 : 1))), (float) (this.ParentHandle.Height - 0x19))
                };
                base.DynamicRectangle = rectangle1;
                base.Size = new Vector2((float) this.Rectangle.Width, (float) this.Rectangle.Height);
                base.SizeRectangle = new SharpDX.Rectangle(0, 0, this.Rectangle.Width, this.Rectangle.Height);
                base.UpdateCropRectangle();
                this.TextHandle.ApplyToControlPosition(this);
            }

            public override DynamicControl.States CurrentState
            {
                get => 
                    base.CurrentState;
                internal set
                {
                    base.CurrentState = value;
                    if (this.TextHandle > null)
                    {
                        this.TextHandle.Color = base.CurrentColorModificationValue.Combine(ValueBase.DefaultColorGold);
                    }
                }
            }

            public override bool IsActive
            {
                get => 
                    base.IsActive;
                set
                {
                    if (base.IsActive != value)
                    {
                        base.IsActive = value;
                        this.TextHandle.TextValue = value ? "Press a new key" : (this.IsFirstButton ? this.ParentHandle.KeyStrings.Item1 : this.ParentHandle.KeyStrings.Item2);
                        this.TextHandle.ApplyToControlPosition(this);
                        if (value)
                        {
                            Messages.OnMessage += new Messages.MessageHandler(this.OnMessage);
                        }
                        else
                        {
                            Messages.OnMessage -= new Messages.MessageHandler(this.OnMessage);
                        }
                    }
                }
            }

            internal bool IsFirstButton { get; set; }

            internal string KeyText
            {
                get => 
                    this.TextHandle.TextValue;
                set
                {
                    this.TextHandle.TextValue = value;
                    this.TextHandle.ApplyToControlPosition(this);
                }
            }

            internal KeyBind ParentHandle { get; set; }

            internal override SharpDX.Rectangle Rectangle =>
                this._rectangle;

            internal Text TextHandle { get; set; }

            internal int Width =>
                ((int) (this.ParentHandle.Width * 0.25f));
        }
    }
}

