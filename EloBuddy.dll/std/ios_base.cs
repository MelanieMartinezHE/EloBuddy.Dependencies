namespace std
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=0x38), NativeCppClass]
    internal struct ios_base
    {
        [StructLayout(LayoutKind.Sequential, Size=12), NativeCppClass]
        internal struct _Fnarray
        {
        }

        [StructLayout(LayoutKind.Sequential, Size=0x10), NativeCppClass]
        internal struct _Iosarray
        {
        }

        [NativeCppClass, CLSCompliant(false)]
        public enum @event
        {
        }

        [StructLayout(LayoutKind.Sequential, Size=20), CLSCompliant(false), NativeCppClass]
        public struct failure
        {
        }

        [StructLayout(LayoutKind.Sequential, Size=1), NativeCppClass, CLSCompliant(false)]
        public struct Init
        {
        }
    }
}

