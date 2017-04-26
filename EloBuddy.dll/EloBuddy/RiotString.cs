namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class RiotString
    {
        public static unsafe int HashAnimation(string animationName)
        {
            IntPtr hglobal = Marshal.StringToHGlobalAnsi(animationName);
            Marshal.FreeHGlobal(hglobal);
            return EloBuddy.Native.RiotString.HashAnimationName((sbyte modopt(IsSignUnspecifiedByte)*) hglobal.ToPointer());
        }

        public static unsafe string Translate(string hashedString)
        {
            IntPtr hglobal = Marshal.StringToHGlobalAnsi(hashedString);
            Marshal.FreeHGlobal(hglobal);
            return new string(EloBuddy.Native.RiotString.TranslateString((sbyte modopt(IsSignUnspecifiedByte)*) hglobal.ToPointer()));
        }
    }
}

