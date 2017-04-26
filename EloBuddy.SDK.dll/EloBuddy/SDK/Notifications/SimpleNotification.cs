namespace EloBuddy.SDK.Notifications
{
    using EloBuddy.SDK.Properties;
    using EloBuddy.SDK.Rendering;
    using SharpDX;
    using SharpDX.Direct3D9;
    using System;
    using System.Runtime.CompilerServices;

    public class SimpleNotification : NotificationBase
    {
        private readonly string _contentText;
        private readonly string _headerText;
        private static readonly NotificationTexture NotificationTextureTexture;
        private static readonly EloBuddy.SDK.Rendering.TextureLoader TextureLoader = new EloBuddy.SDK.Rendering.TextureLoader();

        static SimpleNotification()
        {
            TextureLoader.Load("simpleNotification", Resources.SimpleNotification);
            NotificationTexture texture1 = new NotificationTexture();
            NotificationTexture.PartialTexture texture2 = new NotificationTexture.PartialTexture {
                Position = 0f,
                SourceRectangle = new Rectangle(0, 0, 0x12b, 3),
                Texture = new Func<SharpDX.Direct3D9.Texture>(<>c.<>9.<.cctor>b__2_0)
            };
            texture1.Header = texture2;
            NotificationTexture.PartialTexture texture3 = new NotificationTexture.PartialTexture {
                Position = new Vector2(0f, 3f),
                SourceRectangle = new Rectangle(0, 3, 0x12b, 0x27),
                Texture = new Func<SharpDX.Direct3D9.Texture>(<>c.<>9.<.cctor>b__2_1)
            };
            texture1.Content = texture3;
            NotificationTexture.PartialTexture texture4 = new NotificationTexture.PartialTexture {
                Position = new Vector2(0f, 42f),
                SourceRectangle = new Rectangle(0, 0x2a, 0x12b, 3),
                Texture = new Func<SharpDX.Direct3D9.Texture>(<>c.<>9.<.cctor>b__2_2)
            };
            texture1.Footer = texture4;
            NotificationTextureTexture = texture1;
        }

        public SimpleNotification(string header, string content)
        {
            this._headerText = header;
            this._contentText = content;
        }

        public override string ContentText =>
            this._contentText;

        public override string HeaderText =>
            this._headerText;

        public override NotificationTexture Texture =>
            NotificationTextureTexture;

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly SimpleNotification.<>c <>9 = new SimpleNotification.<>c();

            internal Texture <.cctor>b__2_0() => 
                SimpleNotification.TextureLoader["simpleNotification"];

            internal Texture <.cctor>b__2_1() => 
                SimpleNotification.TextureLoader["simpleNotification"];

            internal Texture <.cctor>b__2_2() => 
                SimpleNotification.TextureLoader["simpleNotification"];
        }
    }
}

