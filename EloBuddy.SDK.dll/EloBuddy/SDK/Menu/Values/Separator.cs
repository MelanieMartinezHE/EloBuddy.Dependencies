namespace EloBuddy.SDK.Menu.Values
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    public sealed class Separator : ValueBase<int>
    {
        internal static readonly List<string> DeserializationNeededKeys = new List<string>();

        public Separator(int height = 0x19) : base("", Math.Max(12, height))
        {
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

        public override bool Draw() => 
            true;

        public override Dictionary<string, object> Serialize() => 
            base.Serialize();

        public override int CurrentValue
        {
            get => 
                base.Height;
            set
            {
                base.Height = value;
                base.CurrentValue = base.Height;
            }
        }

        public override string DisplayName
        {
            get => 
                base.DisplayName;
            set
            {
            }
        }

        public override bool ShouldSerialize =>
            false;

        public override string VisibleName =>
            this.DisplayName;
    }
}

