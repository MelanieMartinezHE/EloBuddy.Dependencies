namespace std
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1), NativeCppClass]
    internal struct _Wrap_alloc<std::allocator<wchar_t> >
    {
        [StructLayout(LayoutKind.Sequential, Size=1), NativeCppClass, CLSCompliant(false)]
        public struct rebind<wchar_t *>
        {
        }

        [StructLayout(LayoutKind.Sequential, Size=1), NativeCppClass, CLSCompliant(false)]
        public struct rebind<wchar_t>
        {
        }
    }
}

