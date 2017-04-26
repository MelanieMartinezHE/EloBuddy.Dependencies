namespace EloBuddy.SDK.Menu.Values
{
    using EloBuddy.SDK.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class Label : ValueBase<string>
    {
        internal float _textWidthMultiplier;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Text <TextHandle>k__BackingField;
        internal static readonly List<string> DeserializationNeededKeys = new List<string>();
        internal const float HeightMultiplier = 1f;

        public Label(string displayName) : this(displayName, 0x19)
        {
        }

        internal Label(string displayName, int height) : base(displayName, height)
        {
            Text text;
            this._textWidthMultiplier = 1f;
            Text text1 = new Text(displayName, ValueBase.DefaultFont) {
                Color = ValueBase.DefaultColorGreen
            };
            this.TextHandle = text = text1;
            base.TextObjects.Add(text);
            this.TextWidthMultiplier = 1f;
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
                return true;
            }
            return false;
        }

        protected internal sealed override void OnThemeChange()
        {
            base.OnThemeChange();
            this.TextHandle.Width = (int) (ValueBase.DefaultWidth * this.TextWidthMultiplier);
            this.TextHandle.ApplyToControlPosition(this);
        }

        public override Dictionary<string, object> Serialize() => 
            base.Serialize();

        public override string CurrentValue
        {
            get => 
                this.TextHandle.TextValue;
            set
            {
                this.TextHandle.TextValue = value;
                base.CurrentValue = value;
            }
        }

        public override string DisplayName
        {
            get => 
                this.CurrentValue;
            set
            {
                this.CurrentValue = value;
            }
        }

        public override bool ShouldSerialize =>
            false;

        internal Text TextHandle { get; set; }

        internal float TextWidthMultiplier
        {
            get => 
                this._textWidthMultiplier;
            set
            {
                if (Math.Abs((float) (this._textWidthMultiplier - value)) > float.Epsilon)
                {
                    this._textWidthMultiplier = value;
                    this.TextHandle.Width = (int) (ValueBase.DefaultWidth * value);
                }
            }
        }

        public override string VisibleName =>
            this.TextHandle.DisplayedText;
    }
}

