namespace msclr.interop
{
    using msclr.interop.details;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using System.Runtime.InteropServices;

    internal class context_node<char const *,System::String ^> : context_node_base, IDisposable
    {
        private unsafe sbyte modopt(IsSignUnspecifiedByte)* _ptr;

        private unsafe void !context_node<char const *,System::String ^>()
        {
            delete[](this._ptr);
        }

        public unsafe context_node<char const *,System::String ^>(sbyte modopt(IsSignUnspecifiedByte) modopt(IsConst)** modopt(IsImplicitlyDereferenced) _to_object, string _from_object)
        {
            char_buffer<char> local;
            this._ptr = null;
            if (_from_object == null)
            {
                _to_object[0] = 0;
            }
            else
            {
                uint num = msclr.interop.details.GetAnsiStringSize(_from_object);
                *((int*) &local) = new[](num);
                try
                {
                    if (*(((int*) &local)) == 0)
                    {
                        throw new InsufficientMemoryException();
                    }
                    msclr.interop.details.WriteAnsiString(*((sbyte modopt(IsSignUnspecifiedByte)**) &local), num, _from_object);
                    sbyte modopt(IsSignUnspecifiedByte)* numPtr = *((sbyte modopt(IsSignUnspecifiedByte)**) &local);
                    *((int*) &local) = 0;
                    this._ptr = numPtr;
                    _to_object[0] = numPtr;
                }
                fault
                {
                    ___CxxCallUnwindDtor(msclr.interop.details.char_buffer<char>.{dtor}, (void*) &local);
                }
                delete[](null);
            }
            try
            {
            }
            fault
            {
                ___CxxCallUnwindDtor(msclr.interop.details.char_buffer<char>.{dtor}, (void*) &local);
            }
        }

        private unsafe void ~context_node<char const *,System::String ^>()
        {
            delete[](this._ptr);
        }

        public sealed override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [HandleProcessCorruptedStateExceptions]
        protected virtual unsafe void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
        {
            if (A_0)
            {
                delete[](this._ptr);
            }
            else
            {
                try
                {
                    this.!context_node<char const *,System::String ^>();
                }
                finally
                {
                    base.Finalize();
                }
            }
        }

        protected override void Finalize()
        {
            this.Dispose(false);
        }
    }
}

