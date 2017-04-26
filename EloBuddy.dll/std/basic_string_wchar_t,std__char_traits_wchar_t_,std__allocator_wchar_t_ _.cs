namespace std
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=0x18), UnsafeValueType, NativeCppClass]
    internal struct basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >
    {
        public static unsafe void <MarshalCopy>(basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >* A_0, basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >* A_1)
        {
            *((int*) (A_0 + 0x10)) = 0;
            *((int*) (A_0 + 20)) = 0;
            std.basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >._Tidy(A_0, false, 0);
            std.basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >.assign(A_0, (basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> > modopt(IsConst)* modopt(IsImplicitlyDereferenced)) A_1, 0, uint.MaxValue);
        }

        public static unsafe void <MarshalDestroy>(basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >* A_0)
        {
            std.basic_string<wchar_t,std::char_traits<wchar_t>,std::allocator<wchar_t> >._Tidy(A_0, true, 0);
        }
    }
}

