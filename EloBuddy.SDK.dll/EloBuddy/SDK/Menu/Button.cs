namespace EloBuddy.SDK.Menu
{
    using EloBuddy.SDK.Rendering;
    using SharpDX;
    using SharpDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class Button : DynamicControl
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ButtonType <CurrentButtonType>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Text <TextHandle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <TextValue>k__BackingField;
        internal static readonly Dictionary<ButtonType, System.Drawing.Color> DefaultColorValues;
        internal static readonly Dictionary<ButtonType, Func<string, Text>> TextDictionary;

        static Button()
        {
            Dictionary<ButtonType, System.Drawing.Color> dictionary1 = new Dictionary<ButtonType, System.Drawing.Color> {
                { 
                    ButtonType.Addon,
                    System.Drawing.Color.FromArgb(0xff, 0x43, 0x89, 0x7b)
                },
                { 
                    ButtonType.AddonSub,
                    System.Drawing.Color.FromArgb(0xff, 0x43, 0x89, 0x7b)
                },
                { 
                    ButtonType.Confirm,
                    System.Drawing.Color.FromArgb(0xff, 0xa4, 0xf3, 0xd7)
                },
                { 
                    ButtonType.Mini,
                    System.Drawing.Color.FromArgb(0xff, 0xa2, 140, 0x63)
                },
                { 
                    ButtonType.Normal,
                    System.Drawing.Color.FromArgb(0xff, 0xa2, 140, 0x63)
                },
                { 
                    ButtonType.ComboBoxSub,
                    System.Drawing.Color.FromArgb(0xff, 0xa2, 140, 0x63)
                }
            };
            DefaultColorValues = dictionary1;
            Dictionary<ButtonType, Func<string, Text>> dictionary2 = new Dictionary<ButtonType, Func<string, Text>> {
                { 
                    ButtonType.Addon,
                    new Func<string, Text>(<>c.<>9.<.cctor>b__26_0)
                },
                { 
                    ButtonType.AddonSub,
                    new Func<string, Text>(<>c.<>9.<.cctor>b__26_1)
                },
                { 
                    ButtonType.Confirm,
                    new Func<string, Text>(<>c.<>9.<.cctor>b__26_2)
                },
                { 
                    ButtonType.Mini,
                    new Func<string, Text>(<>c.<>9.<.cctor>b__26_3)
                },
                { 
                    ButtonType.Normal,
                    new Func<string, Text>(<>c.<>9.<.cctor>b__26_4)
                },
                { 
                    ButtonType.ComboBoxSub,
                    new Func<string, Text>(<>c.<>9.<.cctor>b__26_5)
                }
            };
            TextDictionary = dictionary2;
        }

        internal Button(ButtonType buttonType, string displayText = null) : base(GetSpriteType(buttonType))
        {
            this.CurrentButtonType = buttonType;
            this.OnThemeChange();
            switch (buttonType)
            {
                case ButtonType.AddonSub:
                case ButtonType.ComboBoxSub:
                    base.DrawBase = false;
                    if (buttonType == ButtonType.AddonSub)
                    {
                        this.ExcludeFromParent = true;
                    }
                    break;
            }
            if (displayText > null)
            {
                this.SetText(displayText);
            }
        }

        internal static ThemeManager.SpriteType GetSpriteType(ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.Exit:
                    return ThemeManager.SpriteType.ButtonExit;

                case ButtonType.Addon:
                case ButtonType.AddonSub:
                    return ThemeManager.SpriteType.ButtonAddon;

                case ButtonType.Normal:
                    return ThemeManager.SpriteType.ButtonNormal;

                case ButtonType.Confirm:
                    return ThemeManager.SpriteType.ButtonConfirm;

                case ButtonType.Mini:
                    return ThemeManager.SpriteType.ButtonMini;

                case ButtonType.ComboBoxSub:
                    return ThemeManager.SpriteType.ControlComboBox;
            }
            throw new ArgumentException($"ButtonType '{buttonType}' was not found!", "buttonType");
        }

        protected internal override void OnThemeChange()
        {
            switch (this.CurrentButtonType)
            {
                case ButtonType.AddonSub:
                case ButtonType.ComboBoxSub:
                {
                    base.DynamicRectangle = ThemeManager.GetDynamicRectangle(this);
                    SharpDX.Rectangle rectangle = (from state in Enum.GetValues(typeof(DynamicControl.States)).Cast<DynamicControl.States>() select base.DynamicRectangle.GetRectangle(state)).FirstOrDefault<SharpDX.Rectangle>(rect => !rect.IsEmpty);
                    base.Size = new Vector2((float) rectangle.Width, (float) ((int) (rectangle.Height * ((this.CurrentButtonType == ButtonType.AddonSub) ? 0.75f : 1f))));
                    base.SizeRectangle = new SharpDX.Rectangle(0, 0, (int) base.Size.X, (int) base.Size.Y);
                    base.UpdateCropRectangle();
                    base.TextObjects.ForEach(o => o.ApplyToControlPosition(this));
                    break;
                }
                default:
                    base.OnThemeChange();
                    break;
            }
        }

        internal void RemoveText()
        {
            this.TextValue = null;
            if (this.TextHandle > null)
            {
                this.TextHandle.Dispose();
                this.TextHandle = null;
            }
        }

        internal void SetText(string text)
        {
            this.TextValue = text;
            switch (this.CurrentButtonType)
            {
                case ButtonType.Exit:
                    return;

                case ButtonType.Addon:
                case ButtonType.AddonSub:
                case ButtonType.ComboBoxSub:
                    this.SetText(text, Text.Align.Left, (int) (15f * ((this.CurrentButtonType == ButtonType.AddonSub) ? 1.5f : 1f)), 0);
                    return;
            }
            this.SetText(text, Text.Align.Center, 0, 0);
        }

        internal void SetText(string text, Text.Align align, int xOffset = 0, int yOffset = 0)
        {
            this.TextValue = text;
            if (this.TextHandle == null)
            {
                if (!TextDictionary.ContainsKey(this.CurrentButtonType))
                {
                    return;
                }
                this.TextHandle = TextDictionary[this.CurrentButtonType]((this.CurrentButtonType == ButtonType.Addon) ? text.ToUpper() : text);
                this.TextHandle.Color = base.CurrentColorModificationValue.Combine(DefaultColorValues[this.CurrentButtonType]);
                base.TextObjects.Add(this.TextHandle);
            }
            else
            {
                this.TextHandle.TextValue = (this.CurrentButtonType == ButtonType.Addon) ? text.ToUpper() : text;
            }
            this.TextHandle.TextAlign = align;
            this.TextHandle.Padding = new Vector2((float) xOffset, (float) yOffset);
            this.TextHandle.ApplyToControlPosition(this);
        }

        internal ButtonType CurrentButtonType { get; set; }

        public override DynamicControl.States CurrentState
        {
            get => 
                base.CurrentState;
            internal set
            {
                base.CurrentState = value;
                if ((this.TextHandle != null) && DefaultColorValues.ContainsKey(this.CurrentButtonType))
                {
                    this.TextHandle.Color = base.CurrentColorModificationValue.Combine(DefaultColorValues[this.CurrentButtonType]);
                }
            }
        }

        public string DisplayedText =>
            ((this.TextHandle != null) ? this.TextHandle.DisplayedText : string.Empty);

        internal Text TextHandle { get; set; }

        public string TextValue { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Button.<>c <>9 = new Button.<>c();
            public static Func<SharpDX.Rectangle, bool> <>9__25_1;

            internal Text <.cctor>b__26_0(string text) => 
                new Text(text, new FontDescription { 
                    FaceName = "Gill Sans MT Pro Book",
                    Height = 15,
                    Quality = FontQuality.Antialiased
                }) { Color = Button.DefaultColorValues[Button.ButtonType.Addon] };

            internal Text <.cctor>b__26_1(string text) => 
                new Text(text, new FontDescription { 
                    FaceName = "Gill Sans MT Pro Book",
                    Height = 14,
                    Quality = FontQuality.Antialiased
                }) { Color = Button.DefaultColorValues[Button.ButtonType.AddonSub] };

            internal Text <.cctor>b__26_2(string text) => 
                new Text(text, new FontDescription { 
                    FaceName = "Gill Sans MT Pro Book",
                    Height = 0x12,
                    Quality = FontQuality.Antialiased
                }) { Color = Button.DefaultColorValues[Button.ButtonType.Confirm] };

            internal Text <.cctor>b__26_3(string text) => 
                new Text(text, new FontDescription { 
                    FaceName = "Gill Sans MT Pro Book",
                    Height = 0x1c,
                    Weight = FontWeight.Bold,
                    Width = 12,
                    Quality = FontQuality.Antialiased
                }) { Color = Button.DefaultColorValues[Button.ButtonType.Mini] };

            internal Text <.cctor>b__26_4(string text) => 
                new Text(text, new FontDescription { 
                    FaceName = "Gill Sans MT Pro Book",
                    Height = 0x1c,
                    Weight = FontWeight.Bold,
                    Width = 12,
                    Quality = FontQuality.Antialiased
                }) { Color = Button.DefaultColorValues[Button.ButtonType.Normal] };

            internal Text <.cctor>b__26_5(string text) => 
                new Text(text, new FontDescription { 
                    FaceName = "Gill Sans MT Pro Book",
                    Height = 14,
                    Quality = FontQuality.Antialiased
                }) { Color = Button.DefaultColorValues[Button.ButtonType.ComboBoxSub] };

            internal bool <OnThemeChange>b__25_1(SharpDX.Rectangle rect) => 
                !rect.IsEmpty;
        }

        internal enum ButtonType
        {
            Exit,
            Addon,
            AddonSub,
            Normal,
            Confirm,
            Mini,
            ComboBoxSub
        }
    }
}

