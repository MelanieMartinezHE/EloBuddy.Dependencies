namespace EloBuddy.SDK.Menu
{
    using EloBuddy.SDK.Menu.Values;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public sealed class ValueContainer : ControlContainer<ValueBase>
    {
        internal string _serializationId;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AddonId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Dictionary<string, object>> <ChildrenSerializedData>k__BackingField;

        internal ValueContainer(string addonId, string serializationId) : base(ThemeManager.SpriteType.FormContentContainer, true, false, true, false)
        {
            this.AddonId = addonId;
            this._serializationId = serializationId;
            this.OnThemeChange();
            this.ChildrenSerializedData = new List<Dictionary<string, object>>();
            if ((((addonId != null) && (serializationId != null)) && ((MainMenu.SavedValues != null) && MainMenu.SavedValues.ContainsKey(this.AddonId))) && MainMenu.SavedValues[this.AddonId].ContainsKey(serializationId))
            {
                foreach (Dictionary<string, object> dictionary in MainMenu.SavedValues[this.AddonId][serializationId])
                {
                    if (dictionary > null)
                    {
                        Dictionary<string, object> item = new Dictionary<string, object>();
                        foreach (KeyValuePair<string, object> pair in dictionary)
                        {
                            if (!string.IsNullOrEmpty(pair.Key) && (pair.Value > null))
                            {
                                item.Add(pair.Key, pair.Value);
                            }
                        }
                        this.ChildrenSerializedData.Add(item);
                    }
                }
            }
        }

        protected override Control BaseAdd(Control control)
        {
            base.BaseAdd(control);
            ValueBase base2 = (ValueBase) control;
            this.DeserializeChild(base2);
            return base2;
        }

        internal bool DeserializeChild(ValueBase value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return this.ChildrenSerializedData.Any<Dictionary<string, object>>(new Func<Dictionary<string, object>, bool>(value.ApplySerializedData));
        }

        internal override void RecalculateAlignOffsets()
        {
            int num = 0;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            for (int i = 0; i < base.Children.Count; i++)
            {
                ValueBase base2 = base.Children[i];
                if (base2 is CheckBox)
                {
                    if (flag)
                    {
                        base2.AlignOffset = new Vector2((base.Size.X / 2f) - 5f, (float) num);
                        num += base.Padding + ((int) base2.Size.Y);
                        flag = false;
                    }
                    else
                    {
                        base2.AlignOffset = new Vector2(0f, (float) num);
                        flag = true;
                    }
                    flag2 = false;
                    flag3 = false;
                }
                else
                {
                    if (flag)
                    {
                        num += base.Padding + ((int) base.Children[i - 1].Size.Y);
                        flag = false;
                    }
                    KeyBind bind = base2 as KeyBind;
                    if (bind > null)
                    {
                        if (!flag2)
                        {
                            bind.DrawHeader = true;
                            if (flag3)
                            {
                                num -= 0x19;
                                ((Label) base.Children[i - 1]).TextWidthMultiplier = (0.5f * ValueBase.DefaultWidth) / ((float) ValueBase.DefaultWidth);
                            }
                        }
                        else
                        {
                            bind.DrawHeader = false;
                        }
                    }
                    else if (flag3)
                    {
                        ((Label) base.Children[i - 1]).TextWidthMultiplier = 1f;
                    }
                    base2.AlignOffset = new Vector2(0f, (float) num);
                    num += base.Padding + ((int) base2.Size.Y);
                    flag2 = base2 is KeyBind;
                    flag3 = base2 is Label;
                }
            }
            this.RecalculateBounding();
            base.ContainerView.UpdateChildrenCropping();
        }

        internal List<Dictionary<string, object>> Serialize()
        {
            if (this.ChildrenSerializedData == null)
            {
                this.ChildrenSerializedData = new List<Dictionary<string, object>>();
            }
            IEnumerable<string> currentIds = from o in base.Children select o.SerializationId;
            this.ChildrenSerializedData.RemoveAll(o => o.ContainsKey("SerializationId") && currentIds.Contains<object>(o["SerializationId"]));
            this.ChildrenSerializedData.AddRange(from o in base.Children
                where o.ShouldSerialize
                select o.Serialize());
            return new List<Dictionary<string, object>>(this.ChildrenSerializedData);
        }

        internal string AddonId { get; set; }

        internal List<Dictionary<string, object>> ChildrenSerializedData { get; set; }

        public string SerializationId =>
            this._serializationId;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly ValueContainer.<>c <>9 = new ValueContainer.<>c();
            public static Func<ValueBase, string> <>9__14_0;
            public static Func<ValueBase, bool> <>9__14_2;
            public static Func<ValueBase, Dictionary<string, object>> <>9__14_3;

            internal string <Serialize>b__14_0(ValueBase o) => 
                o.SerializationId;

            internal bool <Serialize>b__14_2(ValueBase o) => 
                o.ShouldSerialize;

            internal Dictionary<string, object> <Serialize>b__14_3(ValueBase o) => 
                o.Serialize();
        }
    }
}

