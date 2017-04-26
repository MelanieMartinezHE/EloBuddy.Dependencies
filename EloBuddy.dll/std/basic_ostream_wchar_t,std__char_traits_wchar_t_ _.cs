namespace std
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=80), NativeCppClass]
    internal struct basic_ostream<wchar_t,std::char_traits<wchar_t> >
    {
        [StructLayout(LayoutKind.Sequential, Size=4), NativeCppClass, CLSCompliant(false)]
        public struct _Sentry_base
        {
        }

        [StructLayout(LayoutKind.Sequential, Size=8), CLSCompliant(false), NativeCppClass]
        public struct sentry
        {
        }
    }
}

