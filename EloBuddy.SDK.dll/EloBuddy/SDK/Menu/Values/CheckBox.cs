namespace EloBuddy.SDK.Menu.Values
{
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Rendering;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class CheckBox : ValueBase<bool>
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DynamicControl <ControlHandle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Text <TextHandle>k__BackingField;
        internal static readonly List<string> DeserializationNeededKeys;

        static CheckBox()
        {
            List<string> list1 = new List<string> { "CurrentValue" };
            DeserializationNeededKeys = list1;
        }

        public CheckBox(string displayName, bool defaultValue = true) : base(displayName, 0x19)
        {
            Text text;
            Text text1 = new Text(displayName, ValueBase.DefaultFont) {
                Color = ValueBase.DefaultColorGold,
                TextOrientation = Text.Orientation.Center
            };
            this.TextHandle = text = text1;
            base.TextObjects.Add(text);
            this.ControlHandle = new CheckBoxHandle();
            this.CurrentValue = defaultValue;
            this.ControlHandle.OnActiveStateChanged += delegate (DynamicControl <sender>, EventArgs <args>) {
                this.CurrentValue = this.ControlHandle.IsActive;
                if (base.IsMouseInside && !this.ControlHandle.IsMouseInside)
                {
                    this.ControlHandle.CurrentState = this.ControlHandle.IsActive ? DynamicControl.States.ActiveHover : DynamicControl.States.Hover;
                }
            };
            base.Add(this.ControlHandle);
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
                this.CurrentValue = (bool) data["CurrentValue"];
                return true;
            }
            return false;
        }

        internal override bool CallLeftMouseDown() => 
            (base.CallLeftMouseDown() && this.ControlHandle.CallLeftMouseDown());

        internal override bool CallLeftMouseUp() => 
            (base.CallLeftMouseUp() && this.ControlHandle.CallLeftMouseUp());

        internal override bool CallMouseEnter() => 
            (base.CallMouseEnter() && this.ControlHandle.CallMouseEnter());

        internal override bool CallMouseLeave() => 
            (base.CallMouseLeave() && this.ControlHandle.CallMouseLeave());

        protected internal override void OnThemeChange()
        {
            base.OnThemeChange();
            this.ControlHandle.AlignOffset = new Vector2(0f, (base.Size.Y - this.ControlHandle.Size.Y) / 2f);
            this.TextHandle.Padding = new Vector2(this.ControlHandle.Size.X, 0f);
            this.TextHandle.ApplyToControlPosition(this);
        }

        public override Dictionary<string, object> Serialize()
        {
            Dictionary<string, object> dictionary = base.Serialize();
            dictionary.Add("CurrentValue", this.CurrentValue);
            return dictionary;
        }

        internal DynamicControl ControlHandle { get; set; }

        public override bool CurrentValue
        {
            get => 
                base.CurrentValue;
            set
            {
                this.ControlHandle.IsActive = value;
                base.CurrentValue = value;
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

        internal Text TextHandle { get; set; }

        public override string VisibleName =>
            this.TextHandle.DisplayedText;

        protected internal override int Width =>
            (base.Width / 2);

        internal sealed class CheckBoxHandle : DynamicControl
        {
            internal CheckBoxHandle() : base(ThemeManager.SpriteType.ControlCheckBox)
            {
                this.OnThemeChange();
            }
        }
    }
}

