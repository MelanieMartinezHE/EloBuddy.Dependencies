namespace EloBuddy.SDK.Menu
{
    using System;

    public sealed class EmptyControl : Control
    {
        public EmptyControl(ThemeManager.SpriteType type) : base(type)
        {
            base.DrawBase = false;
            this.OnThemeChange();
        }
    }
}

