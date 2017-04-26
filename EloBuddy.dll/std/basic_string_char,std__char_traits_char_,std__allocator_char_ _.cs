namespace std
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=0x18), NativeCppClass, UnsafeValueType]
    internal struct basic_string<char,std::char_traits<char>,std::allocator<char> >
    {
        public static unsafe void <MarshalCopy>(basic_string<char,std::char_traits<char>,std::allocator<char> >* A_0, basic_string<char,std::char_traits<char>,std::allocator<char> >* A_1)
        {
            *((int*) (A_0 + 0x10)) = 0;
            *((int*) (A_0 + 20)) = 0;
            std.basic_string<char,std::char_traits<char>,std::allocator<char> >._Tidy(A_0, false, 0);
            std.basic_string<char,std::char_traits<char>,std::allocator<char> >.assign(A_0, (basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsImplicitlyDereferenced)) A_1, 0, uint.MaxValue);
        }

        public static unsafe void <MarshalDestroy>(basic_string<char,std::char_traits<char>,std::allocator<char> >* A_0)
        {
            std.basic_string<char,std::char_traits<char>,std::allocator<char> >._Tidy(A_0, true, 0);
        }
    }
}

