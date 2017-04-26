namespace EloBuddy
{
    using EloBuddy.Native;
    using System;
    using System.Runtime.InteropServices;

    public class MenuGUI
    {
        public static bool IsChatOpen
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.MenuGUI* uguiPtr = EloBuddy.Native.MenuGUI.GetInstance();
                if (uguiPtr != null)
                {
                    return *(EloBuddy.Native.MenuGUI.GetIsChatOpen(uguiPtr));
                }
                return false;
            }
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                if (value)
                {
                    EloBuddy.Native.pwConsole.Show(EloBuddy.Native.pwConsole.GetInstance());
                }
                else
                {
                    EloBuddy.Native.pwConsole.Close(EloBuddy.Native.pwConsole.GetInstance());
                }
            }
        }
    }
}

