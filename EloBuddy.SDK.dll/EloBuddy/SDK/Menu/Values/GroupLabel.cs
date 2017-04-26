namespace EloBuddy.SDK.Menu.Values
{
    using SharpDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class GroupLabel : Label
    {
        internal const float HeightMultiplier = 1.5f;

        public GroupLabel(string displayName) : base(displayName, 0x25)
        {
            FontDescription fontDescription = new FontDescription {
                FaceName = "Gill Sans MT Pro Book",
                Height = 20
            };
            base.TextHandle.ReplaceFont(fontDescription);
            base.TextHandle.Color = ValueBase.DefaultColorGreen;
        }

        protected internal override bool ApplySerializedData(Dictionary<string, object> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (base.ApplySerializedData(data))
            {
                if (Label.DeserializationNeededKeys.Any<string>(key => !data.ContainsKey(key)))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}

