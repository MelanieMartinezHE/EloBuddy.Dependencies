namespace EloBuddy.SDK.Menu
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;
    using EloBuddy.SDK.ThirdParty.Glide;
    using SharpDX;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;

    internal class ContainerView : IDisposable
    {
        internal int _currentViewIndex;
        internal static int _lastUpdate = Core.GameTickCount;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Tween <CurrentTween>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.Rectangle <CurrentViewRectangle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ControlContainerBase <Handle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsCursorOnScrollbar>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsScrollbarMoving>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <MoveOffset>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ScrollbarMaxIndex>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Tweening>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.Rectangle <ViewSize>k__BackingField;
        internal const int MinScrollbarHeight = 20;
        internal const int ScrollbarWidth = 10;
        internal const int ScrollStep = 200;
        internal static readonly EloBuddy.SDK.ThirdParty.Glide.Tweener Tweener = new EloBuddy.SDK.ThirdParty.Glide.Tweener();

        static ContainerView()
        {
            Drawing.OnEndScene += new DrawingEndScene(<>c.<>9.<.cctor>b__7_0);
        }

        internal ContainerView(ControlContainerBase container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.Handle = container;
            this.Handle.OnMouseMove += new Control.ControlHandler(this.OnMouseMove);
            this.Handle.OnMouseWheel += new ControlContainerBase.MouseWheelHandler(this.OnMouseWheel);
            Messages.RegisterEventHandler<Messages.LeftButtonUp>(new Messages.MessageHandler<Messages.LeftButtonUp>(this.OnLeftMouseUp));
        }

        internal bool CheckScrollbarDown(Vector2 mousePosition)
        {
            if (this.IsScrollbarNeeded && this.ScrollbarArea.Contains(Game.CursorPos2D))
            {
                this.MoveOffset = this.ScrollbarIndex - ((int) mousePosition.Y);
                this.IsScrollbarMoving = true;
                if (this.CurrentTween > null)
                {
                    this.CurrentTween.Cancel();
                    this.Tweening = false;
                }
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            if (this.Handle > null)
            {
                this.Handle.OnMouseWheel -= new ControlContainerBase.MouseWheelHandler(this.OnMouseWheel);
                this.Handle = null;
            }
        }

        internal void DrawScrollbar()
        {
            if (this.IsScrollbarNeeded && (this.Tweening || this.IsScrollbarVisible))
            {
                SharpDX.Rectangle scrollbarPosition = this.ScrollbarPosition;
                Vector2[] screenVertices = new Vector2[] { new Vector2(scrollbarPosition.X + (((float) scrollbarPosition.Width) / 2f), (float) scrollbarPosition.Y), new Vector2(scrollbarPosition.X + (((float) scrollbarPosition.Width) / 2f), (float) (scrollbarPosition.Y + scrollbarPosition.Height)) };
                EloBuddy.SDK.Rendering.Line.DrawLine(ScrollbarColor, 10f, screenVertices);
            }
        }

        internal void OnLeftMouseUp(Messages.LeftButtonUp args)
        {
            this.IsScrollbarMoving = false;
        }

        internal void OnMouseMove(Control sender, EventArgs args)
        {
            if (this.IsScrollbarMoving)
            {
                this.ScrollbarIndex = ((int) Game.CursorPos2D.Y) + this.MoveOffset;
            }
        }

        internal void OnMouseWheel(Control sender, Messages.MouseWheel args)
        {
            if (this.CanScroll)
            {
                int num = (args.Direction == Messages.MouseWheel.Directions.Down) ? 1 : -1;
                int num2 = args.ScrollSteps * 200;
                int num3 = -Math.Min(this.MaxViewIndex, Math.Max(0, -this.CurrentViewIndex + (num * num2)));
                if (num3 != this.CurrentViewIndex)
                {
                    if (this.CurrentTween > null)
                    {
                        this.CurrentTween.Cancel();
                    }
                    this.CurrentTween = Tweener.Tween<ContainerView>(this, new { CurrentViewIndex = -Math.Min(this.MaxViewIndex, Math.Max(0, -this.CurrentViewIndex + (num * num2))) }, 0.5f, 0f, true);
                    this.CurrentTween.Ease(new Func<float, float>(Ease.CircOut));
                    this.CurrentTween.OnBegin(() => this.Tweening = true);
                    this.CurrentTween.OnComplete(() => this.Tweening = false);
                }
            }
        }

        internal void OnThemeChange()
        {
            this.UpdateChildrenCropping();
        }

        internal void UpdateChildrenCropping()
        {
            this.UpdateViewSize();
            if (this.Handle != MainMenu.Instance)
            {
                this.Handle.RecalculatePosition();
            }
            this.UpdateChildrenCropping(this.Handle);
        }

        internal void UpdateChildrenCropping(ControlContainerBase container)
        {
            if (container.CropChildren)
            {
                foreach (Control control in container.Children)
                {
                    this.UpdateCropping(control);
                    ControlContainerBase base2 = control as ControlContainerBase;
                    if (base2 > null)
                    {
                        base2.ContainerView.UpdateChildrenCropping();
                    }
                }
            }
        }

        internal void UpdateCropping(Control control)
        {
            if ((control != MainMenu.Instance) && !control.ExcludeFromParent)
            {
                if (!this.ViewSize.IsPartialInside(control.RelativePositionRectangle))
                {
                    control.Crop = 0x7fffffff;
                }
                else if (this.ViewSize.IsCompletlyInside(control.RelativePositionRectangle))
                {
                    control.Crop = 0;
                }
                else
                {
                    control.Crop = (control.RelativePosition.Y > this.ViewSize.Y) ? (this.ViewSize.Bottom - control.RelativePositionRectangle.Bottom) : -(control.RelativePositionRectangle.Top - this.ViewSize.Top);
                }
                control.RecalculatePosition();
            }
        }

        internal void UpdateViewSize()
        {
            if (this.Crop == 0)
            {
                this.ViewSize = new SharpDX.Rectangle(0, 0, (int) this.Size.X, (int) this.Size.Y);
            }
            else if (((this.Crop > 0) && (this.Crop >= this.Size.Y)) || ((this.Crop < 0) && (this.Crop <= -this.Size.Y)))
            {
                this.ViewSize = SharpDX.Rectangle.Empty;
            }
            else if (this.Crop > 0)
            {
                this.ViewSize = new SharpDX.Rectangle(0, this.Crop, (int) this.Size.X, ((int) this.Size.Y) - this.Crop);
            }
            else
            {
                this.ViewSize = new SharpDX.Rectangle(0, 0, (int) this.Size.X, ((int) this.Size.Y) + this.Crop);
            }
            if (this.MaxViewIndex == 0)
            {
                this._currentViewIndex = 0;
            }
            this.CurrentViewRectangle = this.ViewSize.Add(new SharpDX.Rectangle(0, this.CurrentViewIndex, 0, 0));
        }

        internal bool CanScroll =>
            (this.Handle.CropChildren && (this.MaxViewIndex > 0));

        internal SharpDX.Rectangle ContentSize =>
            this.Handle.BoundingRectangle;

        internal int Crop =>
            this.Handle.Crop;

        internal Tween CurrentTween { get; set; }

        internal int CurrentViewIndex
        {
            get => 
                this._currentViewIndex;
            set
            {
                if (this._currentViewIndex != value)
                {
                    this._currentViewIndex = value;
                    this.UpdateChildrenCropping();
                }
            }
        }

        internal SharpDX.Rectangle CurrentViewRectangle { get; set; }

        internal ControlContainerBase Handle { get; set; }

        internal bool IsCursorOnScrollbar { get; set; }

        internal bool IsScrollbarMoving { get; set; }

        internal bool IsScrollbarNeeded =>
            ((this.CanScroll && (this.MaxViewIndex > 0)) && !(this.Handle is ValueBase));

        internal bool IsScrollbarVisible =>
            (this.IsScrollbarMoving || this.ScrollbarArea.IsNear(Game.CursorPos2D, 20));

        internal int MaxViewIndex =>
            Math.Max(0, this.ContentSize.Height - this.ViewSize.Height);

        internal int MoveOffset { get; set; }

        internal SharpDX.Rectangle ScrollbarArea =>
            new SharpDX.Rectangle((((int) this.Handle.Position.X) + this.ViewSize.Width) - 10, (int) this.Handle.Position.Y, 10, this.ViewSize.Height);

        internal static System.Drawing.Color ScrollbarColor =>
            ThemeManager.CurrentTheme.Config.Colors.Scrollbar.DrawingColor;

        internal int ScrollbarHeight =>
            Math.Max(Math.Min(20, this.ViewSize.Height), (int) (this.ScrollbarHeightPercent * this.ViewSize.Height));

        internal float ScrollbarHeightPercent =>
            (((float) Math.Max(Math.Min(20, this.ViewSize.Height), this.ViewSize.Height)) / ((float) this.ContentSize.Height));

        internal int ScrollbarIndex
        {
            get => 
                ((int) Math.Ceiling((double) (-this.CurrentViewIndex * (((float) this.ScrollbarHeight) / ((float) this.ViewSize.Height)))));
            set
            {
                int num = (int) Math.Min((float) this.MaxViewIndex, Math.Max((float) 0f, (float) (((float) value) / (((float) this.ScrollbarHeight) / ((float) this.ViewSize.Height)))));
                this.CurrentViewIndex = -num;
            }
        }

        internal int ScrollbarMaxIndex { get; set; }

        internal SharpDX.Rectangle ScrollbarPosition =>
            new SharpDX.Rectangle((((int) this.Handle.Position.X) + this.ViewSize.Width) - 10, ((int) this.Handle.Position.Y) + this.ScrollbarIndex, 10, this.ScrollbarHeight);

        internal Vector2 Size =>
            this.Handle.Size;

        internal bool Tweening { get; set; }

        internal SharpDX.Rectangle ViewSize { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly ContainerView.<>c <>9 = new ContainerView.<>c();

            internal void <.cctor>b__7_0(EventArgs <args>)
            {
                ContainerView.Tweener.Update(((float) (Core.GameTickCount - ContainerView._lastUpdate)) / 1000f);
                ContainerView._lastUpdate = Core.GameTickCount;
            }
        }
    }
}

