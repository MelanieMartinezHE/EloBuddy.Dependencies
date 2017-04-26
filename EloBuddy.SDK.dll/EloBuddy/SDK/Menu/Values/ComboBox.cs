namespace EloBuddy.SDK.Menu.Values
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Rendering;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class ComboBox : ValueBase<int>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ComboBoxHandle <ControlHandle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Text <TextHandle>k__BackingField;
        internal static readonly List<string> DeserializationNeededKeys;

        static ComboBox()
        {
            List<string> list1 = new List<string> { "CurrentValue" };
            DeserializationNeededKeys = list1;
        }

        public ComboBox(string displayName, IEnumerable<string> textValues, int defaultIndex = 0) : base(displayName, 30)
        {
            Text text;
            List<string> list = textValues.ToList<string>();
            if (defaultIndex >= list.Count)
            {
                throw new IndexOutOfRangeException("Default index cannot be greater than values count!");
            }
            Text text1 = new Text(displayName, ValueBase.DefaultFont) {
                Color = ValueBase.DefaultColorGold,
                TextOrientation = Text.Orientation.Center
            };
            this.TextHandle = text = text1;
            base.TextObjects.Add(text);
            this.ControlHandle = new ComboBoxHandle(list[defaultIndex]);
            base._currentValue = defaultIndex;
            OverlayContainer container1 = new OverlayContainer(this) {
                IsVisible = false,
                BackgroundColor = new System.Drawing.Color?(System.Drawing.Color.FromArgb(200, 14, 0x1b, 0x19))
            };
            base.OverlayControl = container1;
            foreach (string str in list)
            {
                this.Overlay.Add(new Button(Button.ButtonType.ComboBoxSub, str));
            }
            base.Add(this.ControlHandle);
            this.OnThemeChange();
            this.ControlHandle.OnActiveStateChanged += (<sender>, <args>) => (this.Overlay.IsVisible = this.ControlHandle.IsActive);
            Messages.OnMessage += delegate (Messages.WindowMessage args) {
                switch (args.Message)
                {
                    case WindowMessages.LeftButtonDown:
                    case WindowMessages.LeftButtonDoubleClick:
                    case WindowMessages.RightButtonDown:
                    case WindowMessages.RightButtonDoubleClick:
                    case WindowMessages.MiddleButtonDown:
                    case WindowMessages.MiddleButtonDoubleClick:
                    case WindowMessages.MouseWheel:
                    case WindowMessages.KeyUp:
                        this.CloseOverlayOnInteraction(args.Message == WindowMessages.MouseWheel);
                        break;
                }
            };
        }

        public ComboBox(string displayName, int defaultIndex = 0, params string[] textValues) : this(displayName, textValues, defaultIndex)
        {
        }

        public void Add(string value)
        {
            this.Overlay.Add(new Button(Button.ButtonType.ComboBoxSub, value));
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
                this.CurrentValue = Convert.ToInt32(data["CurrentValue"]);
                return true;
            }
            return false;
        }

        internal void CloseOverlayOnInteraction(bool scrolling)
        {
            if ((this.ControlHandle.IsVisible && this.ControlHandle.IsActive) && (scrolling || (!this.ControlHandle.IsInside(Game.CursorPos2D) && !this.Overlay.IsInside(Game.CursorPos2D))))
            {
                this.ControlHandle.IsActive = false;
            }
        }

        protected internal override void OnThemeChange()
        {
            base.OnThemeChange();
            this.ControlHandle.AlignOffset = new Vector2(base.Size.X - this.ControlHandle.Size.X, (base.Size.Y / 2f) - (this.ControlHandle.Size.Y / 2f));
            this.Overlay.AlignOffset = this.ControlHandle.AlignOffset + new Vector2(0f, this.ControlHandle.Size.Y);
        }

        public void Remove(string value)
        {
            Button item = this.Overlay.Children.FirstOrDefault<Button>(o => o.TextValue == value);
            if (item > null)
            {
                int index = this.Overlay.Children.IndexOf(item);
                this.Overlay.Remove(item);
                if (index < this.CurrentValue)
                {
                    this.CurrentValue = this.CurrentValue;
                }
            }
        }

        public void RemoveAt(int index)
        {
            Button control = this.Overlay.Children[index];
            this.Overlay.Remove(control);
            if (index < this.CurrentValue)
            {
                this.CurrentValue = this.CurrentValue;
            }
        }

        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> dictionary = base.Serialize();
            dictionary.Add("CurrentValue", this.CurrentValue);
            return dictionary;
        }

        internal ComboBoxHandle ControlHandle { get; set; }

        public override int CurrentValue
        {
            get => 
                base.CurrentValue;
            set
            {
                if (value < this.Overlay.Children.Count)
                {
                    base.CurrentValue = value;
                    this.ControlHandle.TextHandle.TextValue = this[value];
                }
            }
        }

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

        public string this[int index]
        {
            get => 
                this.Overlay.Children[index].TextValue;
            set
            {
                this.Overlay.Children[index].SetText(value);
            }
        }

        public OverlayContainer Overlay =>
            ((OverlayContainer) base.OverlayControl);

        public override Vector2 Position
        {
            get => 
                base.Position;
            internal set
            {
                base.Position = value;
                if (this.Overlay > null)
                {
                    this.Overlay.Position = value + this.Overlay.AlignOffset;
                }
            }
        }

        public int SelectedIndex
        {
            get => 
                this.CurrentValue;
            set
            {
                this.CurrentValue = value;
            }
        }

        public string SelectedText =>
            this.ControlHandle.TextHandle.TextValue;

        internal Text TextHandle { get; set; }

        public override string VisibleName =>
            this.TextHandle.DisplayedText;

        internal sealed class ComboBoxHandle : DynamicControl
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Text <TextHandle>k__BackingField;

            internal ComboBoxHandle(string defaultText) : base(ThemeManager.SpriteType.ControlComboBox)
            {
                Text text;
                Text text1 = new Text(defaultText, ValueBase.DefaultFont) {
                    Color = ValueBase.DefaultColorGold
                };
                this.TextHandle = text = text1;
                base.TextObjects.Add(text);
                this.OnThemeChange();
            }

            protected internal override void OnThemeChange()
            {
                base.OnThemeChange();
                this.TextHandle.Padding = new Vector2(15f, 0f);
                this.TextHandle.Width = (int) (base.Size.X - 35f);
            }

            internal Text TextHandle { get; set; }
        }

        public sealed class OverlayContainer : ControlContainer<Button>
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private ComboBox <Handle>k__BackingField;
            internal static readonly System.Drawing.Color BorderColor = System.Drawing.Color.FromArgb(0xff, 0xa2, 140, 0x63);
            internal static readonly EloBuddy.SDK.Rendering.Line BorderLine;

            static OverlayContainer()
            {
                EloBuddy.SDK.Rendering.Line line1 = new EloBuddy.SDK.Rendering.Line {
                    Color = BorderColor,
                    Antialias = false,
                    Width = 3f
                };
                BorderLine = line1;
            }

            public OverlayContainer(ComboBox handle) : base(ThemeManager.SpriteType.Empty, true, false, false, true)
            {
                this.Handle = handle;
            }

            protected override Control BaseAdd(Control control)
            {
                Button button = (Button) control;
                button.IsVisible = this.IsVisible;
                button.OnActiveStateChanged += new DynamicControl.DynamicControlHandler(this.OnActiveStateChanged);
                return base.BaseAdd(button);
            }

            protected override void BaseRemove(Control control)
            {
                Button button = (Button) control;
                button.OnActiveStateChanged -= new DynamicControl.DynamicControlHandler(this.OnActiveStateChanged);
                base.BaseRemove(control);
            }

            public override bool Draw()
            {
                if (base.Draw())
                {
                    Vector2[] vectorArray1 = new Vector2[] { this.Position + new Vector2(1f, 0f), this.Position + new Vector2(1f, base.Size.Y), (this.Position + base.Size) + new Vector2(-1f, 0f), this.Position + new Vector2(base.Size.X - 1f, 0f) };
                    BorderLine.ScreenVertices = vectorArray1;
                    BorderLine.Draw();
                    return true;
                }
                return false;
            }

            internal void OnActiveStateChanged(DynamicControl sender, EventArgs args)
            {
                if (sender.IsActive)
                {
                    Button item = (Button) sender;
                    this.Handle.SelectedIndex = base.Children.IndexOf(item);
                    this.Handle.ControlHandle.IsActive = false;
                    item.IsActive = false;
                }
            }

            internal ComboBox Handle { get; set; }

            public override bool IsVisible
            {
                get => 
                    base.IsVisible;
                set
                {
                    base.IsVisible = value;
                    foreach (Button button in base.Children)
                    {
                        button.IsVisible = value;
                    }
                }
            }
        }
    }
}

