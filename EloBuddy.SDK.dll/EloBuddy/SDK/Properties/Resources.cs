namespace EloBuddy.SDK.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        internal static string Config =>
            ResourceManager.GetString("Config", resourceCulture);

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get => 
                resourceCulture;
            set
            {
                resourceCulture = value;
            }
        }

        internal static string Gapclosers =>
            ResourceManager.GetString("Gapclosers", resourceCulture);

        internal static byte[] Gill_Sans_MT_Light =>
            ((byte[]) ResourceManager.GetObject("Gill_Sans_MT_Light", resourceCulture));

        internal static byte[] Gill_Sans_MT_Pro_Book =>
            ((byte[]) ResourceManager.GetObject("Gill_Sans_MT_Pro_Book", resourceCulture));

        internal static byte[] Gill_Sans_MT_Pro_Medium =>
            ((byte[]) ResourceManager.GetObject("Gill_Sans_MT_Pro_Medium", resourceCulture));

        internal static string ItemData =>
            ResourceManager.GetString("ItemData", resourceCulture);

        internal static string Priorities =>
            ResourceManager.GetString("Priorities", resourceCulture);

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan == null)
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("EloBuddy.SDK.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap SimpleNotification =>
            ((Bitmap) ResourceManager.GetObject("SimpleNotification", resourceCulture));

        internal static Bitmap Theme =>
            ((Bitmap) ResourceManager.GetObject("Theme", resourceCulture));
    }
}

