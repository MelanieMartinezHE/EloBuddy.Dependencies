namespace EloBuddy.SDK.Menu
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class ControlContainer : ControlContainerBase, IControlContainer<Control>
    {
        protected ControlContainer(ThemeManager.SpriteType type, bool stackAlign = true, bool drawBase = false, bool cropChildren = true, bool autoSize = false) : base(type, stackAlign, drawBase, cropChildren, autoSize)
        {
        }

        public Control Add(Control control) => 
            this.BaseAdd(control);

        public void Remove(Control control)
        {
            this.BaseRemove(control);
        }
    }
}

