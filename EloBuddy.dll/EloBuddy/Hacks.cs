namespace EloBuddy
{
    using System;
    using System.Runtime.InteropServices;

    public class Hacks
    {
        public static bool AntiAFK
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                EloBuddy.Native.Hacks.GetAntiAFK();
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                EloBuddy.Native.Hacks.SetAntiAFK(value);
            }
        }

        public static bool Console
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                EloBuddy.Native.Hacks.GetConsole();
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                EloBuddy.Native.Hacks.SetConsole(value);
            }
        }

        public static bool DisableDrawings
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                EloBuddy.Native.Hacks.GetDisableDrawings();
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                EloBuddy.Native.Hacks.SetDisableDrawings(value);
            }
        }

        public static bool DisableRangeIndicator
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                EloBuddy.Native.Hacks.GetDisableRangeIndicator();
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                EloBuddy.Native.Hacks.SetDisableRangeIndicator(value);
            }
        }

        public static bool DisableTextures
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                EloBuddy.Native.Hacks.GetDisableTextures();
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                EloBuddy.Native.Hacks.SetDisableTextures(value);
            }
        }

        public static bool IngameChat
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                EloBuddy.Native.Hacks.GetPwConsole();
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                EloBuddy.Native.Hacks.SetPwConsole(value);
            }
        }

        public static bool MovementHack
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                EloBuddy.Native.Hacks.GetMovementHack();
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                EloBuddy.Native.Hacks.SetMovementHack(value);
            }
        }

        public static bool RenderWatermark
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                EloBuddy.Native.Hacks.GetDrawWatermark();
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                EloBuddy.Native.Hacks.SetDrawWatermark(value);
            }
        }

        public static bool TowerRanges
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                EloBuddy.Native.Hacks.GetTowerRanges();
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                EloBuddy.Native.Hacks.SetTowerRanges(value);
            }
        }

        public static bool ZoomHack
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                EloBuddy.Native.Hacks.GetZoomHack();
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                EloBuddy.Native.Hacks.SetZoomHack(value);
            }
        }
    }
}

