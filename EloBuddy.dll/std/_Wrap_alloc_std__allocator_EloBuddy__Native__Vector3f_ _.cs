namespace std
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1), NativeCppClass]
    internal struct _Wrap_alloc<std::allocator<EloBuddy::Native::Vector3f> >
    {
        [StructLayout(LayoutKind.Sequential, Size=1), CLSCompliant(false), NativeCppClass]
        public struct rebind<EloBuddy::Native::Vector3f *>
        {
        }

        [StructLayout(LayoutKind.Sequential, Size=1), CLSCompliant(false), NativeCppClass]
        public struct rebind<EloBuddy::Native::Vector3f>
        {
        }
    }
}

