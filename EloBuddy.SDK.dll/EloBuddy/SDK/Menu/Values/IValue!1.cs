namespace EloBuddy.SDK.Menu.Values
{
    using EloBuddy.SDK.Menu;
    using System;
    using System.Runtime.CompilerServices;

    public interface IValue<T> : ISerializeable
    {
        event ValueBase<T>.ValueChangeHandler OnValueChange;

        T CurrentValue { get; set; }
    }
}

