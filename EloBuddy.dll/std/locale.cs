namespace std
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=4), NativeCppClass]
    internal struct locale
    {
        [StructLayout(LayoutKind.Sequential, Size=0x20), NativeCppClass, CLSCompliant(false)]
        public struct _Locimp
        {
        }

        [StructLayout(LayoutKind.Sequential, Size=8), NativeCppClass, CLSCompliant(false)]
        public struct facet
        {
        }

        [StructLayout(LayoutKind.Sequential, Size=4), NativeCppClass, CLSCompliant(false)]
        public struct id
        {
        }
    }
}

