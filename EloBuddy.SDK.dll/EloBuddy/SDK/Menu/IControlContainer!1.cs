namespace EloBuddy.SDK.Menu
{
    using System;

    public interface IControlContainer<T> where T: Control
    {
        T Add(T control);
        void Remove(T control);
    }
}

