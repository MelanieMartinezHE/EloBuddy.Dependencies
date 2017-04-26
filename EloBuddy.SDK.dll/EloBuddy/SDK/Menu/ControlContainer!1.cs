namespace EloBuddy.SDK.Menu
{
    using System;
    using System.Runtime.InteropServices;

    public abstract class ControlContainer<T> : ControlContainerBase, IControlContainer<T> where T: Control
    {
        internal ControlList<T> _typedChildren;

        protected ControlContainer(ThemeManager.SpriteType type, bool stackAlign = true, bool drawBase = false, bool cropChildren = true, bool autoSize = false) : base(type, stackAlign, drawBase, cropChildren, autoSize)
        {
        }

        public T Add(T control) => 
            ((T) this.BaseAdd(control));

        public void Remove(T control)
        {
            this.BaseRemove(control);
        }

        public ControlList<T> Children =>
            (this._typedChildren ?? (this._typedChildren = new ControlList<T>(ref this._baseChildren)));
    }
}

