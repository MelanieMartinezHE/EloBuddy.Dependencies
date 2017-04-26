namespace EloBuddy.SDK.Menu.Values
{
    using EloBuddy.SDK.Menu;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public abstract class ValueBase<T> : ValueBase, IValue<T>, ISerializeable
    {
        internal T _currentValue;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event ValueChangeHandler<T> OnValueChange;

        protected internal ValueBase(string displayName, int height) : base(displayName, height)
        {
        }

        protected internal ValueBase(string serializationId, string displayName, int height) : base(displayName, height)
        {
            base._serializationId = serializationId;
        }

        protected internal override bool ApplySerializedData(Dictionary<string, object> data) => 
            (((data.ContainsKey("SerializationId") && (((string) data["SerializationId"]) == this.SerializationId)) && data.ContainsKey("Type")) && (((string) data["Type"]) == base.GetType().FullName));

        internal string GenerateUniqueSerializationId()
        {
            string str = this.GenerateUniqueSerializationId(this.Parent);
            return ((str.Length > 0) ? (str + "." + this.DisplayName) : this.DisplayName);
        }

        internal string GenerateUniqueSerializationId(ControlContainerBase parentContainer)
        {
            StringBuilder builder = new StringBuilder();
            if (parentContainer.Parent > null)
            {
                builder.Append(this.GenerateUniqueSerializationId(parentContainer.Parent));
                builder.Append(".");
            }
            return builder.Append(parentContainer.GetType().Name).ToString();
        }

        public override Dictionary<string, object> Serialize() => 
            new Dictionary<string, object> { 
                { 
                    "SerializationId",
                    this.SerializationId
                },
                { 
                    "Type",
                    base.GetType().FullName
                }
            };

        public virtual T CurrentValue
        {
            get => 
                this._currentValue;
            set
            {
                ValueChangeArgs<T> args = new ValueChangeArgs<T>(this._currentValue, value);
                this._currentValue = value;
                if (this.OnValueChange > null)
                {
                    this.OnValueChange((ValueBase<T>) this, args);
                }
            }
        }

        internal ValueContainer Parent
        {
            get => 
                ((ValueContainer) base.Parent);
            set
            {
                base.Parent = value;
            }
        }

        public override string SerializationId =>
            (base._serializationId ?? (base._serializationId = this.GenerateUniqueSerializationId()));

        public override bool ShouldSerialize =>
            true;

        public class ValueChangeArgs : EventArgs
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private T <NewValue>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private T <OldValue>k__BackingField;

            internal ValueChangeArgs(T oldValue, T newValue)
            {
                this.OldValue = oldValue;
                this.NewValue = newValue;
            }

            public T NewValue { get; internal set; }

            public T OldValue { get; internal set; }
        }

        public delegate void ValueChangeHandler(ValueBase<T> sender, ValueBase<T>.ValueChangeArgs args);
    }
}

