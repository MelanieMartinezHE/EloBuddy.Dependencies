namespace EloBuddy.SDK.Menu
{
    using EloBuddy.SDK;
    using EloBuddy.SDK.Rendering;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public abstract class ControlContainerBase : Control
    {
        internal bool _autoSize;
        internal List<Control> _baseChildren;
        internal ControlList<Control> _children;
        internal bool _cropChildren;
        internal bool _stackAlign;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Drawing.Color? <BackgroundColor>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.Rectangle <BoundingRectangle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EloBuddy.SDK.Menu.ContainerView <ContainerView>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector2 <DrawOffset>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <DrawOutline>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <OutlineReady>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <Padding>k__BackingField;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event MouseWheelHandler OnMouseWheel;

        protected ControlContainerBase(ThemeManager.SpriteType type, bool stackAlign = true, bool drawBase = false, bool cropChildren = true, bool autoSize = false) : base(type)
        {
            this.ContainerView = new EloBuddy.SDK.Menu.ContainerView(this);
            this._baseChildren = new List<Control>();
            this._stackAlign = stackAlign;
            base.DrawBase = drawBase;
            this._cropChildren = cropChildren;
            this._autoSize = autoSize;
        }

        protected virtual Control BaseAdd(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            if (!this.Children.Contains(control))
            {
                control.SetParent(this);
                this.Children.Add(control);
                this.RecalculateAlignOffsets();
                if (this.IsOverlay)
                {
                    control.IsOverlay = true;
                }
            }
            this.RecalculateSize();
            return control;
        }

        protected virtual void BaseRemove(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            control.SetParent(null);
            this.Children.Remove(control);
            this.RecalculateAlignOffsets();
            if (this.IsOverlay)
            {
                control.IsOverlay = false;
            }
            this.RecalculateSize();
        }

        internal virtual bool CallMouseWheel(Messages.MouseWheel args)
        {
            if (this.IsVisible)
            {
                if (this.OnMouseWheel > null)
                {
                    this.OnMouseWheel(this, args);
                }
                return true;
            }
            return false;
        }

        public override bool Draw()
        {
            if (this.IsVisible)
            {
                if (this.BackgroundColor.HasValue)
                {
                    SharpDX.Rectangle rectangle = !this.AutoSize ? ((this.Crop == 0) ? this.Rectangle : base.DrawingRectangle) : new SharpDX.Rectangle(0, 0, (int) base.Size.X, (int) base.Size.Y);
                    if (rectangle.Height > 0)
                    {
                        Vector2 vector = (this.Crop == 0) ? this.Position : (this.Position + new Vector2(0f, (this.Crop > 0) ? ((float) this.Crop) : ((float) 0)));
                        Vector2[] screenVertices = new Vector2[] { vector + new Vector2(((float) rectangle.Width) / 2f, 0f), vector + new Vector2(((float) rectangle.Width) / 2f, (float) rectangle.Height) };
                        EloBuddy.SDK.Rendering.Line.DrawLine(this.BackgroundColor.Value, (float) rectangle.Width, screenVertices);
                    }
                }
                if (!base.DrawBase || base.Draw())
                {
                    foreach (Control control in from child in this.Children
                        where !child.ExcludeFromParent
                        where child.Draw()
                        select child)
                    {
                        control.TextObjects.ForEach(delegate (Text o) {
                            o.Draw();
                        });
                    }
                    this.ContainerView.DrawScrollbar();
                    this.OutlineReady = true;
                    return true;
                }
            }
            return false;
        }

        protected internal override bool OnKeyDown(Messages.KeyDown args)
        {
            if (base.OnKeyDown(args))
            {
                foreach (Control control in this.Children)
                {
                    control.OnKeyDown(args);
                }
                return true;
            }
            return false;
        }

        protected internal override bool OnKeyUp(Messages.KeyUp args)
        {
            if (base.OnKeyUp(args))
            {
                foreach (Control control in this.Children)
                {
                    control.OnKeyUp(args);
                }
                return true;
            }
            return false;
        }

        protected internal override void OnThemeChange()
        {
            if (!this.AutoSize)
            {
                base.OnThemeChange();
            }
            this.RecalculateSize();
            this.ContainerView.OnThemeChange();
            foreach (Control control in this.Children)
            {
                control.OnThemeChange();
            }
        }

        internal virtual void RecalculateAlignOffsets()
        {
            if (!this.StackAlign)
            {
                foreach (Control control in from child in this.Children
                    where !child.ExcludeFromParent
                    select child)
                {
                    control.AlignOffset = Vector2.Zero;
                }
            }
            else
            {
                int num = 0;
                foreach (Control control2 in from child in this.Children
                    where !child.ExcludeFromParent
                    select child)
                {
                    control2.AlignOffset = new Vector2(0f, (float) num);
                    num += this.Padding + ((int) control2.Size.Y);
                }
            }
            this.RecalculateBounding();
            this.ContainerView.UpdateChildrenCropping();
        }

        internal virtual void RecalculateBounding()
        {
            Vector2 zero = Vector2.Zero;
            Vector2 vector2 = Vector2.Zero;
            foreach (Control control in from child in this.Children
                where !child.ExcludeFromParent
                select child)
            {
                if (control.RelativePosition.Y < zero.Y)
                {
                    zero = control.RelativePosition;
                }
                if ((control.RelativePosition + control.Size).Y > vector2.Y)
                {
                    vector2 = control.RelativePosition + control.Size;
                }
            }
            this.BoundingRectangle = new SharpDX.Rectangle(0, (int) zero.Y, (int) base.Size.Y, (int) vector2.Y);
        }

        protected void RecalculateSize()
        {
            if (this.AutoSize)
            {
                float x = 0f;
                float y = 0f;
                foreach (Control control in this.Children)
                {
                    if ((control.Size.X + control.AlignOffset.X) > x)
                    {
                        x = control.Size.X + control.AlignOffset.X;
                    }
                    if ((control.Size.Y + control.AlignOffset.Y) > y)
                    {
                        y = control.Size.Y + control.AlignOffset.Y;
                    }
                }
                base.Size = new Vector2(x, y) - ((Vector2) 1f);
            }
        }

        protected internal bool AutoSize
        {
            get => 
                this._autoSize;
            set
            {
                if (this._autoSize != value)
                {
                    this._autoSize = value;
                    if (!value)
                    {
                        this.OnThemeChange();
                    }
                }
            }
        }

        protected internal System.Drawing.Color? BackgroundColor { get; set; }

        internal SharpDX.Rectangle BoundingRectangle { get; set; }

        internal ControlList<Control> Children =>
            (this._children ?? (this._children = new ControlList<Control>(ref this._baseChildren)));

        internal EloBuddy.SDK.Menu.ContainerView ContainerView { get; set; }

        public override int Crop
        {
            get => 
                base.Crop;
            internal set
            {
                if (base.Crop != value)
                {
                    base.Crop = value;
                    this.ContainerView.UpdateChildrenCropping();
                }
            }
        }

        internal bool CropChildren
        {
            get => 
                this._cropChildren;
            set
            {
                if (this._cropChildren != value)
                {
                    this._cropChildren = value;
                    if (value)
                    {
                        this.ContainerView.UpdateChildrenCropping();
                    }
                    else
                    {
                        foreach (Control control in this.Children)
                        {
                            control.Crop = 0;
                        }
                    }
                }
            }
        }

        internal virtual Vector2 DrawOffset { get; set; }

        internal static bool DrawOutline
        {
            [CompilerGenerated]
            get => 
                <DrawOutline>k__BackingField;
            [CompilerGenerated]
            set
            {
                <DrawOutline>k__BackingField = value;
            }
        }

        protected internal override bool IsOverlay
        {
            get => 
                base.IsOverlay;
            internal set
            {
                if (base.IsOverlay != value)
                {
                    base.IsOverlay = value;
                    foreach (Control control in this.Children)
                    {
                        control.IsOverlay = value;
                    }
                }
            }
        }

        internal bool OutlineReady { get; set; }

        internal int Padding { get; set; }

        public override Vector2 Position
        {
            get => 
                base.Position;
            internal set
            {
                base.Position = value;
                foreach (Control control in this.Children)
                {
                    control.RecalculatePosition();
                }
            }
        }

        internal bool StackAlign
        {
            get => 
                this._stackAlign;
            set
            {
                if (this._stackAlign != value)
                {
                    this._stackAlign = value;
                    this.RecalculateAlignOffsets();
                }
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly ControlContainerBase.<>c <>9 = new ControlContainerBase.<>c();
            public static Func<Control, bool> <>9__58_0;
            public static Func<Control, bool> <>9__58_1;
            public static Func<Control, bool> <>9__59_0;
            public static Func<Control, bool> <>9__61_0;
            public static Func<Control, bool> <>9__61_1;
            public static Action<Text> <>9__61_2;

            internal bool <Draw>b__61_0(Control child) => 
                !child.ExcludeFromParent;

            internal bool <Draw>b__61_1(Control child) => 
                child.Draw();

            internal void <Draw>b__61_2(Text o)
            {
                o.Draw();
            }

            internal bool <RecalculateAlignOffsets>b__58_0(Control child) => 
                !child.ExcludeFromParent;

            internal bool <RecalculateAlignOffsets>b__58_1(Control child) => 
                !child.ExcludeFromParent;

            internal bool <RecalculateBounding>b__59_0(Control child) => 
                !child.ExcludeFromParent;
        }

        public delegate void MouseWheelHandler(Control sender, Messages.MouseWheel args);
    }
}

