namespace msclr.interop
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class marshal_context : IDisposable
    {
        internal readonly LinkedList<object> modreq(IsByValue) _clean_up_list = new LinkedList<object>();

        private void ~marshal_context()
        {
            LinkedList<object>.Enumerator enumerator = this._clean_up_list.GetEnumerator();
            if (enumerator.MoveNext())
            {
                do
                {
                    IDisposable current = enumerator.Current as IDisposable;
                    if (current != null)
                    {
                        current.Dispose();
                    }
                }
                while (enumerator.MoveNext());
            }
        }

        public sealed override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
        {
            if (A_0)
            {
                this.~marshal_context();
            }
            else
            {
                base.Finalize();
            }
        }
    }
}

