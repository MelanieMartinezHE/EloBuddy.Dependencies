﻿namespace EloBuddy.SDK.Menu
{
    using System;
    using System.Runtime.InteropServices;

    public sealed class SimpleControlContainer<T> : ControlContainer<T> where T: Control
    {
        public SimpleControlContainer(ThemeManager.SpriteType type, bool stackAlign = true, bool drawBase = false, bool cropChildren = true, bool autoSize = false) : base(type, stackAlign, drawBase, cropChildren, autoSize)
        {
            this.OnThemeChange();
        }
    }
}

