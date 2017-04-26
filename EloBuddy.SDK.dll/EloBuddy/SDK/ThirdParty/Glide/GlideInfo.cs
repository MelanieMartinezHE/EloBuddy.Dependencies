namespace EloBuddy.SDK.ThirdParty.Glide
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class GlideInfo
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <PropertyName>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Type <PropertyType>k__BackingField;
        private FieldInfo field;
        private static BindingFlags flags = (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        private PropertyInfo prop;
        private object Target;

        public GlideInfo(object target, FieldInfo info)
        {
            this.Target = target;
            this.field = info;
            this.PropertyName = info.Name;
            this.PropertyType = info.FieldType;
        }

        public GlideInfo(object target, PropertyInfo info)
        {
            this.Target = target;
            this.prop = info;
            this.PropertyName = info.Name;
            this.PropertyType = this.prop.PropertyType;
        }

        public GlideInfo(object target, string property, bool writeRequired = true)
        {
            this.Target = target;
            this.PropertyName = property;
            Type type = (target as Type) ?? target.GetType();
            this.field = type.GetField(property, flags);
            if (this.field != null)
            {
                this.PropertyType = this.field.FieldType;
            }
            else
            {
                this.prop = type.GetProperty(property, flags);
                if (this.prop == null)
                {
                    throw new Exception($"Field or {writeRequired ? "read/write" : "readable"} property '{property}' not found on object of type {type.FullName}.");
                }
                this.PropertyType = this.prop.PropertyType;
            }
        }

        public string PropertyName { get; private set; }

        public Type PropertyType { get; private set; }

        public object Value
        {
            get => 
                ((this.field != null) ? this.field.GetValue(this.Target) : this.prop.GetValue(this.Target, null));
            set
            {
                if (this.field != null)
                {
                    this.field.SetValue(this.Target, value);
                }
                else
                {
                    this.prop.SetValue(this.Target, value, null);
                }
            }
        }
    }
}

