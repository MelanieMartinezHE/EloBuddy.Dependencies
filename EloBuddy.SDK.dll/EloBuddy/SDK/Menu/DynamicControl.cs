namespace EloBuddy.SDK.Menu
{
    using EloBuddy.SDK.Rendering;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public abstract class DynamicControl : Control
    {
        internal States _currentState;
        internal bool _isActive;
        internal bool _isDisabled;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EloBuddy.SDK.Menu.Theme.DynamicRectangle <DynamicRectangle>k__BackingField;
        internal static readonly Dictionary<States, ColorModificationValue> ColorModificationValues;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event DynamicControlHandler OnActiveStateChanged;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event DynamicControlHandler OnStateChanged;

        static DynamicControl()
        {
            Dictionary<States, ColorModificationValue> dictionary1 = new Dictionary<States, ColorModificationValue>();
            ColorModificationValue value2 = new ColorModificationValue();
            dictionary1.Add(States.Normal, value2);
            value2 = new ColorModificationValue {
                R = 50,
                G = 50,
                B = 50
            };
            dictionary1.Add(States.Hover, value2);
            value2 = new ColorModificationValue {
                R = 50,
                G = 50,
                B = 50
            };
            dictionary1.Add(States.Down, value2);
            value2 = new ColorModificationValue {
                R = 100,
                G = 100,
                B = 100
            };
            dictionary1.Add(States.ActiveNormal, value2);
            value2 = new ColorModificationValue {
                R = 100,
                G = 100,
                B = 100
            };
            dictionary1.Add(States.ActiveHover, value2);
            value2 = new ColorModificationValue {
                R = 100,
                G = 100,
                B = 100
            };
            dictionary1.Add(States.ActiveDown, value2);
            value2 = new ColorModificationValue {
                Gray = true
            };
            dictionary1.Add(States.Disabled, value2);
            ColorModificationValues = dictionary1;
        }

        protected DynamicControl(ThemeManager.SpriteType type) : base(type)
        {
        }

        internal override bool CallLeftMouseDown()
        {
            if (!this.IsDisabled && base.CallLeftMouseDown())
            {
                this.CurrentState = this.IsActive ? States.ActiveDown : States.Down;
                return true;
            }
            return false;
        }

        internal override bool CallLeftMouseUp()
        {
            if (!this.IsDisabled && base.CallLeftMouseUp())
            {
                this.IsActive = !this.IsActive;
                return true;
            }
            return false;
        }

        internal override bool CallMouseEnter()
        {
            if (!this.IsDisabled && base.CallMouseEnter())
            {
                this.CurrentState = this.IsActive ? States.ActiveHover : States.Hover;
                return true;
            }
            return false;
        }

        internal override bool CallMouseLeave()
        {
            if (!this.IsDisabled && base.CallMouseLeave())
            {
                this.CurrentState = this.IsActive ? States.ActiveNormal : States.Normal;
                return true;
            }
            return false;
        }

        internal override bool CallMouseMove() => 
            (!this.IsDisabled && base.CallMouseMove());

        protected internal override void OnThemeChange()
        {
            this.DynamicRectangle = ThemeManager.GetDynamicRectangle(this);
            SharpDX.Rectangle rectangle = (from state in Enum.GetValues(typeof(States)).Cast<States>() select this.DynamicRectangle.GetRectangle(state)).FirstOrDefault<SharpDX.Rectangle>(rect => !rect.IsEmpty);
            base.Size = new Vector2((float) rectangle.Width, (float) rectangle.Height);
            base.SizeRectangle = new SharpDX.Rectangle(0, 0, rectangle.Width, rectangle.Height);
            base.UpdateCropRectangle();
            base.TextObjects.ForEach(o => o.ApplyToControlPosition(this));
        }

        internal virtual void SetDefaultState()
        {
            if (this.IsDisabled)
            {
                this.CurrentState = States.Disabled;
            }
            else
            {
                this.CurrentState = this.IsActive ? States.ActiveNormal : States.Normal;
            }
        }

        internal ColorModificationValue CurrentColorModificationValue =>
            ColorModificationValues[this.CurrentState];

        public virtual States CurrentState
        {
            get => 
                this._currentState;
            internal set
            {
                if (this._currentState != value)
                {
                    this._currentState = value;
                    base.UpdateCropRectangle();
                    if (this.OnStateChanged > null)
                    {
                        this.OnStateChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        internal EloBuddy.SDK.Menu.Theme.DynamicRectangle DynamicRectangle { get; set; }

        public virtual bool IsActive
        {
            get => 
                this._isActive;
            set
            {
                if (this._isActive != value)
                {
                    this._isActive = value;
                    if (base.IsMouseInside)
                    {
                        if (base.IsLeftMouseDown)
                        {
                            this.CurrentState = value ? States.ActiveDown : States.Down;
                        }
                        else
                        {
                            this.CurrentState = value ? States.ActiveHover : States.Hover;
                        }
                    }
                    else
                    {
                        this.CurrentState = value ? States.ActiveNormal : States.Normal;
                    }
                    if (this.OnActiveStateChanged > null)
                    {
                        this.OnActiveStateChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool IsDisabled
        {
            get => 
                this._isDisabled;
            set
            {
                if (this._isDisabled != value)
                {
                    this._isDisabled = value;
                    this.CurrentState = value ? States.Disabled : States.Normal;
                }
            }
        }

        public override Vector2 Offset =>
            this.DynamicRectangle.Offset;

        internal override SharpDX.Rectangle Rectangle =>
            this.DynamicRectangle.GetRectangle(this.CurrentState);

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly DynamicControl.<>c <>9 = new DynamicControl.<>c();
            public static Func<SharpDX.Rectangle, bool> <>9__33_1;

            internal bool <OnThemeChange>b__33_1(SharpDX.Rectangle rect) => 
                !rect.IsEmpty;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct ColorModificationValue
        {
            internal byte A;
            internal byte R;
            internal byte G;
            internal byte B;
            internal bool Gray;
            internal bool IsNull =>
                ((((this.A == 0) && (this.R == 0)) && (this.G == 0)) && (this.B == 0));
            internal System.Drawing.Color Combine(System.Drawing.Color color) => 
                (this.Gray ? System.Drawing.Color.DimGray : System.Drawing.Color.FromArgb(this.Validate(color.A + this.A), this.Validate(color.R + this.R), this.Validate(color.G + this.G), this.Validate(color.B + this.B)));

            internal byte Validate(int value) => 
                ((byte) Math.Max(0, Math.Min(0xff, value)));
        }

        public delegate void DynamicControlHandler(DynamicControl sender, EventArgs args);

        public enum States
        {
            Normal,
            ActiveDown,
            ActiveHover,
            ActiveNormal,
            Down,
            Hover,
            Disabled
        }
    }
}

