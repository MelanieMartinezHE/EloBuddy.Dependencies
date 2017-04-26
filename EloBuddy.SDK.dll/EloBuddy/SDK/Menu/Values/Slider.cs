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
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class Slider : ValueBase<int>
    {
        internal int _maxValue;
        internal int _minValue;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EmptyControl <Background>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DynamicControl <ControlHandle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <MoveOffset>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Text <TextHandle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Text <ValueDisplayHandle>k__BackingField;
        internal static readonly List<string> DeserializationNeededKeys;

        static Slider()
        {
            List<string> list1 = new List<string> { "CurrentValue" };
            DeserializationNeededKeys = list1;
        }

        public Slider(string displayName, int defaultValue = 0, int minValue = 0, int maxValue = 100) : base(displayName, 0x19 + ThemeManager.CurrentTheme.Config.SpriteAtlas.Backgrounds.Slider.Height)
        {
            Text[] collection = new Text[2];
            Text text1 = new Text(displayName, ValueBase.DefaultFont) {
                TextOrientation = Text.Orientation.Top,
                Color = ValueBase.DefaultColorGold
            };
            collection[0] = this.TextHandle = text1;
            Text text2 = new Text("placeholder", ValueBase.DefaultFont) {
                TextOrientation = Text.Orientation.Top,
                TextAlign = Text.Align.Right,
                Color = ValueBase.DefaultColorGold
            };
            collection[1] = this.ValueDisplayHandle = text2;
            base.TextObjects.AddRange(collection);
            EmptyControl control1 = new EmptyControl(ThemeManager.SpriteType.BackgroundSlider) {
                DrawBase = true
            };
            this.Background = control1;
            this.ControlHandle = new SliderHandle();
            this._minValue = Math.Min(minValue, maxValue);
            this._maxValue = Math.Max(minValue, maxValue);
            this.CurrentValue = defaultValue;
            base.Add(this.Background);
            base.Add(this.ControlHandle);
            this.Background.OnLeftMouseDown += delegate (Control <sender>, EventArgs <args>) {
                if (!this.ControlHandle.IsLeftMouseDown && this.Background.IsInside(Game.CursorPos2D))
                {
                    this.MoveOffset = 0 - (this.SliderSize / 2);
                    this.RecalculateSliderPosition(((int) Game.CursorPos2D.X) - (this.SliderSize / 2));
                    this.ControlHandle.IsLeftMouseDown = true;
                }
            };
            this.ControlHandle.OnLeftMouseDown += (<sender>, <args>) => (this.MoveOffset = (int) ((this.ControlHandle.Position.X - this.RelativeOffset.X) - Game.CursorPos2D.X));
            this.ControlHandle.OnActiveStateChanged += delegate (DynamicControl <sender>, EventArgs <args>) {
                if (this.ControlHandle.IsActive)
                {
                    this.ControlHandle.IsActive = false;
                }
            };
            this.OnThemeChange();
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

        public override bool Draw()
        {
            if (this.ControlHandle.IsLeftMouseDown)
            {
                this.RecalculateSliderPosition(((int) Game.CursorPos2D.X) + this.MoveOffset);
            }
            return base.Draw();
        }

        protected internal override void OnThemeChange()
        {
            base.OnThemeChange();
            base.Height = 0x19 + ((int) this.Background.Size.Y);
            this.TextHandle.Padding = new Vector2((this.Width - this.Background.Size.X) / 4f, ((float) (0x19 - this.TextHandle.Bounding.Height)) / 2f);
            this.TextHandle.ApplyToControlPosition(this);
            this.ValueDisplayHandle.Padding = new Vector2(-this.TextHandle.Padding.X, this.TextHandle.Padding.Y);
            this.ValueDisplayHandle.ApplyToControlPosition(this);
            this.Background.AlignOffset = new Vector2((base.Size.X / 2f) - (this.Background.Size.X / 2f), base.Size.Y - this.Background.Size.Y);
            this.RecalculateSliderPosition(-1);
        }

        internal void RecalculateSliderPosition(int posX = -1)
        {
            Vector2 vector = this.RelativeOffset + this.Background.AlignOffset;
            if ((this.MaxValue - this.MinValue) == 0)
            {
                this.ControlHandle.AlignOffset = vector;
            }
            else if (posX > 0)
            {
                if (posX <= this.Background.Position.X)
                {
                    this.CurrentValue = this.MinValue;
                    this.ControlHandle.AlignOffset = vector;
                }
                else if (posX >= (this.Background.Position.X + this.SliderWidth))
                {
                    this.CurrentValue = this.MaxValue;
                    this.ControlHandle.AlignOffset = vector + new Vector2((float) this.SliderWidth, 0f);
                }
                else
                {
                    this.CurrentValue = (int) Math.Round((double) (this.MinValue + (((posX - this.Background.Position.X) / ((float) this.SliderWidth)) * (this.MaxValue - this.MinValue))));
                    this.ControlHandle.AlignOffset = vector + new Vector2((((float) this.SliderWidth) / ((float) (this.MaxValue - this.MinValue))) * ((float) (this.CurrentValue - this.MinValue)), 0f);
                }
            }
            else if (this.CurrentValue == this.MinValue)
            {
                this.ControlHandle.AlignOffset = vector;
            }
            else if (this.CurrentValue == this.MaxValue)
            {
                this.ControlHandle.AlignOffset = vector + new Vector2((float) this.SliderWidth, 0f);
            }
            else
            {
                this.ControlHandle.AlignOffset = vector + new Vector2((((float) this.SliderWidth) / ((float) (this.MaxValue - this.MinValue))) * ((float) (this.CurrentValue - this.MinValue)), 0f);
            }
            if (this.DisplayName.Contains("{"))
            {
                this.TextHandle.TextValue = string.Format(this.DisplayName, this.CurrentValue, this.MinValue, this.MaxValue);
            }
            this.ValueDisplayHandle.TextValue = $"{this.CurrentValue}/{this.MaxValue}";
            this.ValueDisplayHandle.ApplyToControlPosition(this);
        }

        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> dictionary = base.Serialize();
            dictionary.Add("CurrentValue", this.CurrentValue);
            return dictionary;
        }

        internal EmptyControl Background { get; set; }

        internal DynamicControl ControlHandle { get; set; }

        public override int CurrentValue
        {
            get => 
                base.CurrentValue;
            set
            {
                int num = Math.Max(Math.Min(value, this.MaxValue), this.MinValue);
                if (base.CurrentValue != num)
                {
                    base.CurrentValue = Math.Max(Math.Min(value, this.MaxValue), this.MinValue);
                    this.RecalculateSliderPosition(-1);
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
                this.TextHandle.TextValue = string.Format(value, this.CurrentValue, this.MinValue, this.MaxValue);
            }
        }

        public int MaxValue
        {
            get => 
                this._maxValue;
            set
            {
                this._maxValue = Math.Max(value, this._minValue);
                this.RecalculateSliderPosition(-1);
                if (base._currentValue > this._maxValue)
                {
                    this.CurrentValue = this._maxValue;
                }
            }
        }

        public int MinValue
        {
            get => 
                this._minValue;
            set
            {
                this._minValue = Math.Min(value, this._maxValue);
                this.RecalculateSliderPosition(-1);
                if (base._currentValue < this._minValue)
                {
                    this.CurrentValue = this._minValue;
                }
            }
        }

        internal int MoveOffset { get; set; }

        internal Vector2 RelativeOffset =>
            new Vector2(-((this.ControlHandle.Size.X / 2f) - (((float) this.SliderSize) / 2f)), -((this.ControlHandle.Size.Y / 2f) - (((float) this.SliderSize) / 2f)));

        internal int SliderSize =>
            ((int) this.Background.Size.Y);

        internal int SliderWidth =>
            (((int) this.Background.Size.X) - this.SliderSize);

        internal Text TextHandle { get; set; }

        internal Text ValueDisplayHandle { get; set; }

        public override string VisibleName =>
            this.TextHandle.DisplayedText;

        internal sealed class SliderHandle : DynamicControl
        {
            public SliderHandle() : base(ThemeManager.SpriteType.ControlSlider)
            {
                this.OnThemeChange();
            }

            internal override bool CallLeftMouseUp() => 
                (base.CallLeftMouseUp() && (!base.IsLeftMouseDown || base.CallMouseLeave()));

            internal override bool CallMouseEnter()
            {
                if (base.IsLeftMouseDown && base.IsMouseInside)
                {
                    return false;
                }
                return base.CallMouseEnter();
            }

            internal override bool CallMouseLeave() => 
                (this.IsVisible && (base.IsLeftMouseDown || base.CallMouseLeave()));

            public override bool IsInside(Vector2 position)
            {
                if ((((int) base.Size.Y) + base.CropRectangle.Height) == 0)
                {
                    return false;
                }
                int height = ThemeManager.CurrentTheme.Config.SpriteAtlas.Backgrounds.Slider.Height;
                int num2 = (int) Math.Max((float) 0f, (float) (((float) (ThemeManager.CurrentTheme.Config.SpriteAtlas.Controls.Slider.Height - height)) / 2f));
                int num3 = (this.Crop == 0) ? 0 : ((this.Crop > 0) ? (this.Crop - num2) : (this.Crop + num2));
                Vector2 vector = this.Position + new Vector2(((float) (ThemeManager.CurrentTheme.Config.SpriteAtlas.Controls.Slider.Height - height)) / 2f);
                return new SharpDX.Rectangle((int) vector.X, ((int) vector.Y) + ((num3 > 0) ? num3 : 0), height, height + ((num3 < 0) ? num3 : 0)).IsInside(position);
            }
        }
    }
}

