namespace EloBuddy.SDK.Menu.Values
{
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using SharpDX;
    using SharpDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public abstract class ValueBase : ControlContainer<Control>
    {
        internal string _displayName;
        internal int _height;
        internal string _serializationId;
        protected internal static readonly System.Drawing.Color DefaultColorGold;
        protected internal static readonly System.Drawing.Color DefaultColorGreen;
        protected internal static readonly FontDescription DefaultFont;
        protected internal const int DefaultHeight = 0x19;

        static ValueBase()
        {
            FontDescription description = new FontDescription {
                FaceName = "Gill Sans MT Pro Book",
                Height = 0x10
            };
            DefaultFont = description;
            DefaultColorGold = System.Drawing.Color.FromArgb(0xff, 0x8f, 0x7a, 0x48);
            DefaultColorGreen = System.Drawing.Color.FromArgb(0xff, 0x2c, 0x63, 0x5e);
        }

        protected internal ValueBase(string displayName, int height) : base(ThemeManager.SpriteType.Empty, false, false, true, false)
        {
            this._displayName = displayName;
            this._height = height;
        }

        protected internal abstract bool ApplySerializedData(Dictionary<string, object> data);
        internal override bool CallMouseWheel(Messages.MouseWheel args) => 
            ((base.Parent != null) && base.Parent.CallMouseWheel(args));

        public T Cast<T>() where T: ValueBase => 
            ((T) this);

        protected internal override void OnThemeChange()
        {
            Theme.StaticRectangle rectangle1 = new Theme.StaticRectangle {
                X = ThemeManager.CurrentTheme.Config.SpriteAtlas.MainForm.ContentContainer.X,
                Y = ThemeManager.CurrentTheme.Config.SpriteAtlas.MainForm.ContentContainer.Y,
                Width = this.Width,
                Height = this.Height,
                Offset = new Vector2(0f, 7f)
            };
            base.StaticRectangle = rectangle1;
            base.Size = new Vector2((float) this.Rectangle.Width, (float) this.Rectangle.Height);
            base.SizeRectangle = new SharpDX.Rectangle(0, 0, this.Rectangle.Width, this.Rectangle.Height);
            base.UpdateCropRectangle();
            base.ContainerView.OnThemeChange();
            foreach (Control control in base.Children)
            {
                control.OnThemeChange();
            }
        }

        public abstract Dictionary<string, object> Serialize();

        protected internal static int DefaultWidth =>
            ThemeManager.CurrentTheme.Config.SpriteAtlas.MainForm.ContentContainer.Width;

        public virtual string DisplayName
        {
            get => 
                this._displayName;
            set
            {
                this._displayName = value;
            }
        }

        protected internal int Height
        {
            get => 
                this._height;
            set
            {
                if (this._height != value)
                {
                    this._height = value;
                    this.OnThemeChange();
                }
            }
        }

        public abstract string SerializationId { get; }

        public abstract bool ShouldSerialize { get; }

        public abstract string VisibleName { get; }

        protected internal virtual int Width =>
            DefaultWidth;
    }
}

