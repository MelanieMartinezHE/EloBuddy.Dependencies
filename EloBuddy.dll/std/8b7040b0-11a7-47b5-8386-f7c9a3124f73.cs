namespace std
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1), NativeCppClass]
    internal struct _Wrap_alloc<std::allocator<void *> >
    {
        [StructLayout(LayoutKind.Sequential, Size=1), NativeCppClass, CLSCompliant(false)]
        public struct rebind<void * *>
        {
        }

        [StructLayout(LayoutKind.Sequential, Size=1), CLSCompliant(false), NativeCppClass]
        public struct rebind<void *>
        {
        }
    }
}

