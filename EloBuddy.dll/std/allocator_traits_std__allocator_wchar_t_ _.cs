namespace std
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1), NativeCppClass]
    internal struct allocator_traits<std::allocator<wchar_t> >
    {
    }
}

