namespace EloBuddy.SDK.Menu
{
    using EloBuddy.SDK;
    using EloBuddy.SDK.Rendering;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class Control
    {
        internal Vector2 _alignOffset;
        internal int _crop;
        internal bool _drawBase = true;
        internal bool _isVisible = true;
        internal Control _overlayControl;
        internal Vector2 _position;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.Rectangle <CropRectangle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <ExcludeFromParent>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <FullCrop>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsLeftMouseDown>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsMouseInside>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsRightMouseDown>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ControlContainerBase <Parent>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector2 <Size>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.Rectangle <SizeRectangle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EloBuddy.SDK.Menu.Theme.StaticRectangle <StaticRectangle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ThemeManager.SpriteType <Type>k__BackingField;
        internal static readonly List<Control> OverlayControls = new List<Control>();
        protected internal readonly List<Text> TextObjects = new List<Text>();

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event ControlHandler OnLeftMouseDown;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event ControlHandler OnLeftMouseUp;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event ControlHandler OnMouseEnter;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event ControlHandler OnMouseLeave;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event ControlHandler OnMouseMove;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event ControlHandler OnRightMouseDown;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event ControlHandler OnRightMouseUp;

        protected Control(ThemeManager.SpriteType type)
        {
            this.Type = type;
        }

        internal virtual bool CallLeftMouseDown()
        {
            if (this.IsVisible)
            {
                if (this.OnLeftMouseDown > null)
                {
                    this.OnLeftMouseDown(this, EventArgs.Empty);
                }
                return true;
            }
            return false;
        }

        internal virtual bool CallLeftMouseUp()
        {
            if (this.IsVisible)
            {
                if (this.OnLeftMouseUp > null)
                {
                    this.OnLeftMouseUp(this, EventArgs.Empty);
                }
                return true;
            }
            return false;
        }

        internal virtual bool CallMouseEnter()
        {
            if (this.IsVisible)
            {
                if (this.OnMouseEnter > null)
                {
                    this.OnMouseEnter(this, EventArgs.Empty);
                }
                return true;
            }
            return false;
        }

        internal virtual bool CallMouseLeave()
        {
            if (this.IsVisible)
            {
                this.IsLeftMouseDown = false;
                this.IsRightMouseDown = false;
                if (this.OnMouseLeave > null)
                {
                    this.OnMouseLeave(this, EventArgs.Empty);
                }
                return true;
            }
            return false;
        }

        internal virtual bool CallMouseMove()
        {
            if (this.IsVisible)
            {
                if (this.OnMouseMove > null)
                {
                    this.OnMouseMove(this, EventArgs.Empty);
                }
                return true;
            }
            return false;
        }

        internal virtual bool CallRightMouseDown()
        {
            if (this.IsVisible)
            {
                if (this.OnRightMouseDown > null)
                {
                    this.OnRightMouseDown(this, EventArgs.Empty);
                }
                return true;
            }
            return false;
        }

        internal virtual bool CallRightMouseUp()
        {
            if (this.IsVisible)
            {
                if (this.OnRightMouseUp > null)
                {
                    this.OnRightMouseUp(this, EventArgs.Empty);
                }
                return true;
            }
            return false;
        }

        public virtual bool Draw()
        {
            if (this.IsVisible && !this.FullCrop)
            {
                if (!this.DrawBase)
                {
                    return true;
                }
                if (this.Crop == 0)
                {
                    if (this.Rectangle.Height > 0)
                    {
                        this.Sprite.Draw(this.Position, new SharpDX.Rectangle?(this.Rectangle));
                    }
                    return true;
                }
                SharpDX.Rectangle drawingRectangle = this.DrawingRectangle;
                if (drawingRectangle.Height > 0)
                {
                    this.Sprite.Draw(this.Position + new Vector2(0f, (this.Crop > 0) ? ((float) this.Crop) : ((float) 0)), new SharpDX.Rectangle?(drawingRectangle));
                    return true;
                }
                return ((this.Parent != null) && this.Parent.ContainerView.ViewSize.IsPartialInside(this.RelativePositionRectangle));
            }
            return false;
        }

        public virtual bool IsInside(Vector2 position)
        {
            if ((((int) this.Size.Y) + this.CropRectangle.Height) == 0)
            {
                return false;
            }
            Vector2 vector = position - this.Position;
            int num = (this.CropRectangle.Y == 0) ? 0 : ((this.CropRectangle.Y > 0) ? this.CropRectangle.Y : 0);
            return ((((vector.X >= 0f) && (vector.Y >= num)) && (vector.X < this.Size.X)) && (vector.Y < (num + (this.Size.Y + this.CropRectangle.Height))));
        }

        internal static bool IsOnOverlay(Vector2 mousePosition) => 
            OverlayControls.Any<Control>(o => (o.IsVisible && o.IsInside(mousePosition)));

        protected internal virtual bool OnKeyDown(Messages.KeyDown args) => 
            true;

        protected internal virtual bool OnKeyUp(Messages.KeyUp args) => 
            true;

        protected internal virtual void OnThemeChange()
        {
            this.StaticRectangle = ThemeManager.GetStaticRectangle(this);
            this.Size = new Vector2((float) this.Rectangle.Width, (float) this.Rectangle.Height);
            this.SizeRectangle = new SharpDX.Rectangle(0, 0, this.Rectangle.Width, this.Rectangle.Height);
            this.UpdateCropRectangle();
            this.TextObjects.ForEach(o => o.ApplyToControlPosition(this));
        }

        internal virtual void RecalculatePosition()
        {
            this.Position = ((this.Parent == null) ? Vector2.Zero : this.Parent.Position) + this.RelativePosition;
        }

        internal virtual void SetParent(ControlContainerBase parent)
        {
            if (parent == null)
            {
                this.Parent = null;
                this.AlignOffset = Vector2.Zero;
                this.RecalculatePosition();
            }
            else
            {
                this.Parent = parent;
            }
        }

        internal void UpdateCropRectangle()
        {
            if (this.Crop == 0)
            {
                this.FullCrop = false;
                this.CropRectangle = SharpDX.Rectangle.Empty;
            }
            else if (((this.Crop > 0) && (this.Crop >= this.Size.Y)) || ((this.Crop < 0) && (this.Crop <= -this.Size.Y)))
            {
                this.FullCrop = true;
                this.CropRectangle = new SharpDX.Rectangle(0, 0, (int) -this.Size.X, (int) -this.Size.Y);
            }
            else
            {
                this.FullCrop = false;
                this.CropRectangle = (this.Crop > 0) ? new SharpDX.Rectangle(0, this.Crop, 0, -this.Crop) : new SharpDX.Rectangle(0, 0, 0, this.Crop);
            }
        }

        internal Vector2 AlignOffset
        {
            get => 
                this._alignOffset;
            set
            {
                if (this._alignOffset != value)
                {
                    this._alignOffset = value;
                    this.RecalculatePosition();
                }
            }
        }

        public virtual int Crop
        {
            get => 
                this._crop;
            internal set
            {
                this._crop = value;
                if (!this.Size.IsZero)
                {
                    this.UpdateCropRectangle();
                }
            }
        }

        internal SharpDX.Rectangle CropRectangle { get; set; }

        internal bool DrawBase
        {
            get => 
                this._drawBase;
            set
            {
                this._drawBase = value;
            }
        }

        internal SharpDX.Rectangle DrawingRectangle =>
            ((this.Rectangle.Height == 0) ? this.Rectangle : this.Rectangle.Add(this.CropRectangle));

        internal virtual bool ExcludeFromParent { get; set; }

        internal bool FullCrop { get; set; }

        public bool IsLeftMouseDown { get; internal set; }

        public bool IsMouseInside { get; internal set; }

        protected internal virtual bool IsOverlay
        {
            get => 
                OverlayControls.Contains(this);
            internal set
            {
                if (value)
                {
                    if (!this.IsOverlay)
                    {
                        OverlayControls.Add(this);
                    }
                }
                else
                {
                    OverlayControls.Remove(this);
                }
            }
        }

        public bool IsRightMouseDown { get; internal set; }

        public virtual bool IsVisible
        {
            get => 
                this._isVisible;
            set
            {
                if (this._isVisible != value)
                {
                    this._isVisible = value;
                    if (!value)
                    {
                        this.IsLeftMouseDown = false;
                        this.IsRightMouseDown = false;
                        this.IsMouseInside = false;
                    }
                }
            }
        }

        internal Vector2 MaxPos =>
            (this.Position + this.Size);

        internal Vector2 MinPos =>
            this.Position;

        public virtual Vector2 Offset =>
            this.StaticRectangle.Offset;

        protected internal Control OverlayControl
        {
            get => 
                this._overlayControl;
            set
            {
                if (this._overlayControl != value)
                {
                    if (this._overlayControl > null)
                    {
                        this._overlayControl.IsOverlay = false;
                        OverlayControls.Remove(this._overlayControl);
                    }
                    this._overlayControl = value;
                    if (this._overlayControl > null)
                    {
                        this._overlayControl.IsOverlay = true;
                        OverlayControls.Add(this._overlayControl);
                    }
                }
            }
        }

        internal ControlContainerBase Parent { get; set; }

        public virtual Vector2 Position
        {
            get => 
                this._position;
            internal set
            {
                this._position = value;
                foreach (Text text in this.TextObjects)
                {
                    text.ApplyToControlPosition(this);
                }
            }
        }

        internal virtual SharpDX.Rectangle Rectangle =>
            ((this.Type == ThemeManager.SpriteType.FormContentContainer) ? this.StaticRectangle.Rectangle.Add(new SharpDX.Rectangle(0, 0, 10, 0)) : this.StaticRectangle.Rectangle);

        public Vector2 RelativePosition =>
            ((this.Parent == null) ? Vector2.Zero : ((this.Offset + this.AlignOffset) + new Vector2(0f, (float) this.Parent.ContainerView.CurrentViewIndex)));

        public SharpDX.Rectangle RelativePositionRectangle =>
            ((this.Parent == null) ? SharpDX.Rectangle.Empty : new SharpDX.Rectangle((int) this.RelativePosition.X, (int) this.RelativePosition.Y, (int) this.Size.X, (int) this.Size.Y));

        public Vector2 Size { get; internal set; }

        public SharpDX.Rectangle SizeRectangle { get; internal set; }

        internal EloBuddy.SDK.Rendering.Sprite Sprite =>
            MainMenu.Sprite;

        internal EloBuddy.SDK.Menu.Theme.StaticRectangle StaticRectangle { get; set; }

        internal ThemeManager.SpriteType Type { get; set; }

        public delegate void ControlHandler(Control sender, EventArgs args);
    }
}

