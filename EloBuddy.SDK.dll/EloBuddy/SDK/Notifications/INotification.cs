namespace EloBuddy.SDK.Notifications
{
    using System;
    using System.Drawing;

    public interface INotification
    {
        Color ContentColor { get; }

        string ContentText { get; }

        string FontName { get; }

        Color HeaderColor { get; }

        string HeaderText { get; }

        int RightPadding { get; }

        NotificationTexture Texture { get; }
    }
}

