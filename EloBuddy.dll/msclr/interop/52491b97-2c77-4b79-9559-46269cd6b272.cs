namespace msclr.interop
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using System.Runtime.InteropServices;

    internal class context_node<wchar_t const *,System::String ^> : context_node_base, IDisposable
    {
        private IntPtr _ip;

        private void !context_node<wchar_t const *,System::String ^>()
        {
            if (this._ip != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this._ip);
            }
        }

        public unsafe context_node<wchar_t const *,System::String ^>(char modopt(IsConst)** modopt(IsImplicitlyDereferenced) _to_object, string _from_object)
        {
            IntPtr ptr = Marshal.StringToHGlobalUni(_from_object);
            this._ip = ptr;
            _to_object[0] = (char modopt(IsConst)** modopt(IsImplicitlyDereferenced)) this._ip.ToPointer();
        }

        private void ~context_node<wchar_t const *,System::String ^>()
        {
            this.!context_node<wchar_t const *,System::String ^>();
        }

        public sealed override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [HandleProcessCorruptedStateExceptions]
        protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
        {
            if (A_0)
            {
                this.!context_node<wchar_t const *,System::String ^>();
            }
            else
            {
                try
                {
                    this.!context_node<wchar_t const *,System::String ^>();
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

