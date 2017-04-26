namespace msclr.interop
{
    using System;
    using System.Runtime.CompilerServices;

    internal class context_node_base
    {
        public static bool modopt(IsConst) _Needs_Context = true;

        static context_node_base()
        {
            _Needs_Context = true;
        }
    }
}

