namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class RiotAsset
    {
        [return: MarshalAs(UnmanagedType.U1)]
        public static unsafe bool Exists(string asset)
        {
            IntPtr hglobal = Marshal.StringToHGlobalAnsi(asset);
            bool flag = EloBuddy.Native.RiotAsset.LoadAsset((sbyte modopt(IsSignUnspecifiedByte)*) hglobal.ToPointer());
            Marshal.FreeHGlobal(hglobal);
            return flag;
        }
    }
}

