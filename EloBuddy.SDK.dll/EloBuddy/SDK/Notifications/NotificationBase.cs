namespace EloBuddy.SDK.Notifications
{
    using System;
    using System.Drawing;

    public abstract class NotificationBase : INotification
    {
        public static readonly Color DefaultBackgroundColor = Color.White;
        public static readonly Color DefaultContentTextColor = Color.FromArgb(0xff, 0x2c, 0x63, 0x5e);
        public const string DefaultFontName = "Gill Sans MT Pro Medium";
        public static readonly Color DefaultHeaderTextColor = Color.FromArgb(0xff, 0x8f, 0x7a, 0x48);
        public static readonly int DefaultRightPadding = 0;

        protected NotificationBase()
        {
        }

        public virtual Color ContentColor =>
            DefaultContentTextColor;

        public abstract string ContentText { get; }

        public virtual string FontName =>
            "Gill Sans MT Pro Medium";

        public virtual Color HeaderColor =>
            DefaultHeaderTextColor;

        public abstract string HeaderText { get; }

        public virtual int RightPadding =>
            DefaultRightPadding;

        public abstract NotificationTexture Texture { get; }
    }
}

