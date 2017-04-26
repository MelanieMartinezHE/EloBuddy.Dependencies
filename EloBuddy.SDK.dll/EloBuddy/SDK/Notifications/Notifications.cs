namespace EloBuddy.SDK.Notifications
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Rendering;
    using EloBuddy.SDK.ThirdParty.Glide;
    using SharpDX;
    using SharpDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    public static class Notifications
    {
        internal static int _lastUpdate = Core.GameTickCount;
        internal static readonly List<ActiveNotification> ActiveNotifications = new List<ActiveNotification>();
        internal const int FadeInTime = 500;
        internal const int FadeOutTime = 500;
        internal const int HeaderToContentSpace = 2;
        public static int MaxNotificationHeight = 500;
        internal const int MoveDownTime = 200;
        internal const int NotificationPadding = 20;
        internal static readonly int NotificationStart = (Drawing.Height - 0x1a9);
        internal const int TextHeight = 0x10;
        internal static readonly Vector2 TextOuterPadding = new Vector2(8f);
        internal static readonly EloBuddy.SDK.Rendering.TextureLoader TextureLoader = new EloBuddy.SDK.Rendering.TextureLoader();
        internal static readonly EloBuddy.SDK.ThirdParty.Glide.Tweener Tweener = new EloBuddy.SDK.ThirdParty.Glide.Tweener();

        static Notifications()
        {
            Game.OnUpdate += new GameUpdate(EloBuddy.SDK.Notifications.Notifications.OnUpdate);
            Drawing.OnEndScene += new DrawingEndScene(<>c.<>9.<.cctor>b__13_0);
        }

        internal static int GetIndexY(int index) => 
            ((from o in ActiveNotifications
                where o.Index <= index
                select o).TakeWhile<ActiveNotification>(((Func<ActiveNotification, bool>) (o => (o.Index <= index)))).Sum<ActiveNotification>(((Func<ActiveNotification, int>) (o => (o.Height + 20)))) - 20);

        internal static void OnDraw()
        {
            List<ActiveNotification> activeNotifications = ActiveNotifications;
            lock (activeNotifications)
            {
                foreach (ActiveNotification notification in from notification in ActiveNotifications
                    where notification._initialized
                    select notification)
                {
                    notification.Draw();
                }
            }
        }

        internal static void OnUpdate(EventArgs args)
        {
            List<ActiveNotification> activeNotifications = ActiveNotifications;
            lock (activeNotifications)
            {
                ActiveNotifications.RemoveAll(o => (o.Tween == null) && !o.IsValid);
                int num = 1;
                foreach (ActiveNotification notification in from notification in ActiveNotifications
                    where notification._initialized
                    select notification)
                {
                    if ((notification.Tween == null) && (notification.OffsetX < 0))
                    {
                        notification.Tween = Tweener.Tween<ActiveNotification>(notification, new { 
                            OffsetX = 0,
                            AlphaLevel = 0xff
                        }, 0.5f, 0f, true);
                    }
                    if ((notification.Index > num) && (notification.Tween == null))
                    {
                        notification.Index = num;
                        notification.Tween = Tweener.Tween<ActiveNotification>(notification, new { PositionY = GetIndexY(notification.Index) }, 0.2f, 0f, true);
                        notification.ExtraTime += 200;
                    }
                    if ((notification.Tween == null) && notification.ShouldFadeOut)
                    {
                        notification.Tween = Tweener.Tween<ActiveNotification>(notification, new { 
                            OffsetX = 100,
                            AlphaLevel = 0
                        }, 0.5f, 0f, true);
                        notification.Index = 0x7fffffff;
                        notification.IsFadingOut = true;
                    }
                    if (!notification.IsFadingOut)
                    {
                        num++;
                    }
                }
            }
        }

        public static void Show(INotification notification, int duration = 0x1388)
        {
            ActiveNotification item = new ActiveNotification(notification, duration);
            List<ActiveNotification> activeNotifications = ActiveNotifications;
            lock (activeNotifications)
            {
                ActiveNotifications.Add(item);
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly EloBuddy.SDK.Notifications.Notifications.<>c <>9 = new EloBuddy.SDK.Notifications.Notifications.<>c();
            public static Predicate<EloBuddy.SDK.Notifications.Notifications.ActiveNotification> <>9__14_0;
            public static Func<EloBuddy.SDK.Notifications.Notifications.ActiveNotification, bool> <>9__14_1;
            public static Func<EloBuddy.SDK.Notifications.Notifications.ActiveNotification, bool> <>9__15_0;
            public static Func<EloBuddy.SDK.Notifications.Notifications.ActiveNotification, int> <>9__16_2;

            internal void <.cctor>b__13_0(EventArgs <args>)
            {
                EloBuddy.SDK.Notifications.Notifications.Tweener.Update(((float) (Core.GameTickCount - EloBuddy.SDK.Notifications.Notifications._lastUpdate)) / 1000f);
                EloBuddy.SDK.Notifications.Notifications._lastUpdate = Core.GameTickCount;
                EloBuddy.SDK.Notifications.Notifications.OnDraw();
            }

            internal int <GetIndexY>b__16_2(EloBuddy.SDK.Notifications.Notifications.ActiveNotification o) => 
                (o.Height + 20);

            internal bool <OnDraw>b__15_0(EloBuddy.SDK.Notifications.Notifications.ActiveNotification notification) => 
                notification._initialized;

            internal bool <OnUpdate>b__14_0(EloBuddy.SDK.Notifications.Notifications.ActiveNotification o) => 
                ((o.Tween == null) && !o.IsValid);

            internal bool <OnUpdate>b__14_1(EloBuddy.SDK.Notifications.Notifications.ActiveNotification notification) => 
                notification._initialized;
        }

        internal class ActiveNotification
        {
            internal bool _initialized;
            internal EloBuddy.SDK.ThirdParty.Glide.Tween _tween;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <AlphaLevel>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Rendering.Sprite <Content>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float <ContentScale>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Text <ContentText>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Duration>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <ExtraTime>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Rendering.Sprite <Footer>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private INotification <Handle>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Rendering.Sprite <Header>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Text <HeaderText>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Height>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Index>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private bool <IsFadingOut>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <OffsetX>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <PositionY>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <StartTick>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int <Width>k__BackingField;

            internal ActiveNotification(INotification handle, int duration)
            {
                this.Handle = handle;
                this.Duration = duration;
                this.OffsetX = -200;
                this.ContentScale = 1f;
                FontDescription fontDescription = new FontDescription {
                    FaceName = this.Handle.FontName,
                    Height = 0x10,
                    Quality = FontQuality.Antialiased,
                    Weight = FontWeight.Medium
                };
                Text text1 = new Text(this.Handle.HeaderText, fontDescription) {
                    Color = this.Handle.HeaderColor,
                    DrawFlags = FontDrawFlags.WordBreak | FontDrawFlags.Right
                };
                this.HeaderText = text1;
                fontDescription = new FontDescription {
                    FaceName = this.Handle.FontName,
                    Height = 0x10,
                    Quality = FontQuality.Antialiased,
                    Weight = FontWeight.Medium
                };
                Text text2 = new Text(this.Handle.ContentText, fontDescription) {
                    Color = this.Handle.ContentColor,
                    DrawFlags = FontDrawFlags.WordBreak | FontDrawFlags.Right
                };
                this.ContentText = text2;
                if (this.Handle.Texture.Header > null)
                {
                    this.Header = new EloBuddy.SDK.Rendering.Sprite(this.Handle.Texture.Header.Texture);
                }
                if (this.Handle.Texture.Content > null)
                {
                    this.Content = new EloBuddy.SDK.Rendering.Sprite(this.Handle.Texture.Content.Texture);
                }
                if (this.Handle.Texture.Footer > null)
                {
                    this.Footer = new EloBuddy.SDK.Rendering.Sprite(this.Handle.Texture.Footer.Texture);
                }
                Task.Run(delegate {
                    int num = 0;
                    int num2 = 0;
                    NotificationTexture.PartialTexture[] source = new NotificationTexture.PartialTexture[] { this.Handle.Texture.Header, this.Handle.Texture.Content, this.Handle.Texture.Footer };
                    foreach (NotificationTexture.PartialTexture texture in from texture in source
                        where texture > null
                        select texture)
                    {
                        int width;
                        int height;
                        if (texture.SourceRectangle.HasValue)
                        {
                            width = texture.SourceRectangle.Value.Width;
                            height = texture.SourceRectangle.Value.Height;
                        }
                        else
                        {
                            SurfaceDescription levelDescription = texture.Texture().GetLevelDescription(0);
                            width = levelDescription.Width;
                            height = levelDescription.Height;
                        }
                        if (num < width)
                        {
                            num = width;
                        }
                        if (num2 < height)
                        {
                            num2 = height;
                        }
                    }
                    this.Width = num;
                    this.Height = num2;
                    SharpDX.Rectangle rectangle = this.HeaderText.MeasureBounding(this.Handle.HeaderText, new SharpDX.Rectangle(0, 0, this.Width - (((int) EloBuddy.SDK.Notifications.Notifications.TextOuterPadding.X) * 2), EloBuddy.SDK.Notifications.Notifications.MaxNotificationHeight - (((int) EloBuddy.SDK.Notifications.Notifications.TextOuterPadding.Y) * 2)), new FontDrawFlags?(this.HeaderText.DrawFlags));
                    this.HeaderText.Size = new Vector2((float) rectangle.Width, (float) rectangle.Height);
                    rectangle = this.ContentText.MeasureBounding(this.Handle.ContentText, new SharpDX.Rectangle(0, 0, this.Width - (((int) EloBuddy.SDK.Notifications.Notifications.TextOuterPadding.X) * 2), ((EloBuddy.SDK.Notifications.Notifications.MaxNotificationHeight - (((int) EloBuddy.SDK.Notifications.Notifications.TextOuterPadding.Y) * 2)) - rectangle.Height) - 2), new FontDrawFlags?(this.ContentText.DrawFlags));
                    this.ContentText.Size = new Vector2((float) rectangle.Width, (float) rectangle.Height);
                    float num3 = (((EloBuddy.SDK.Notifications.Notifications.TextOuterPadding.Y * 2f) + this.HeaderText.Size.Y) + this.ContentText.Size.Y) + 2f;
                    if (num3 > num2)
                    {
                        this.Height = (int) Math.Ceiling((double) num3);
                        if (this.Handle.Texture.Content > null)
                        {
                            int num6 = this.Height;
                            if (this.Handle.Texture.Header > null)
                            {
                                if (this.Handle.Texture.Header.SourceRectangle.HasValue)
                                {
                                    num6 -= this.Handle.Texture.Header.SourceRectangle.Value.Height;
                                }
                                else
                                {
                                    num6 -= this.Handle.Texture.Header.Texture().GetLevelDescription(0).Height;
                                }
                            }
                            if (this.Handle.Texture.Footer > null)
                            {
                                if (this.Handle.Texture.Footer.SourceRectangle.HasValue)
                                {
                                    num6 -= this.Handle.Texture.Footer.SourceRectangle.Value.Height;
                                }
                                else
                                {
                                    num6 -= this.Handle.Texture.Footer.Texture().GetLevelDescription(0).Height;
                                }
                            }
                            int num7 = this.Handle.Texture.Content.SourceRectangle.HasValue ? this.Handle.Texture.Content.SourceRectangle.Value.Height : this.Handle.Texture.Content.Texture().GetLevelDescription(0).Height;
                            this.ContentScale = ((float) num6) / ((float) num7);
                        }
                    }
                    List<EloBuddy.SDK.Notifications.Notifications.ActiveNotification> activeNotifications = EloBuddy.SDK.Notifications.Notifications.ActiveNotifications;
                    lock (activeNotifications)
                    {
                        int? nullable2 = (from o in EloBuddy.SDK.Notifications.Notifications.ActiveNotifications
                            where !o.IsFadingOut
                            select o).Max<EloBuddy.SDK.Notifications.Notifications.ActiveNotification>((Func<EloBuddy.SDK.Notifications.Notifications.ActiveNotification, int?>) (o => new int?(o.Index)));
                        this.Index = (nullable2.HasValue ? nullable2.GetValueOrDefault() : 0) + 1;
                        this.PositionY = EloBuddy.SDK.Notifications.Notifications.GetIndexY(this.Index);
                    }
                    this.StartTick = Core.GameTickCount;
                    this._initialized = true;
                });
            }

            internal void Draw()
            {
                Vector2? nullable;
                Vector2 vector2;
                Vector2? nullable2;
                Vector2? nullable3;
                Vector2 position = this.Position;
                if (this.Header > null)
                {
                    this.Header.Color = System.Drawing.Color.FromArgb(this.AlphaLevel, this.Content.Color);
                    vector2 = position;
                    nullable2 = this.Handle.Texture.Header.Position;
                    nullable = nullable2.HasValue ? new Vector2?(vector2 + nullable2.GetValueOrDefault()) : ((Vector2?) (nullable3 = null));
                    this.Header.Draw(nullable.HasValue ? nullable.GetValueOrDefault() : Vector2.Zero, this.Handle.Texture.Header.SourceRectangle);
                }
                if (this.Content > null)
                {
                    this.Content.Color = System.Drawing.Color.FromArgb(this.AlphaLevel, this.Content.Color);
                    vector2 = position;
                    nullable2 = this.Handle.Texture.Content.Position;
                    nullable = nullable2.HasValue ? new Vector2?(vector2 + nullable2.GetValueOrDefault()) : ((Vector2?) (nullable3 = null));
                    this.Content.Draw(nullable.HasValue ? nullable.GetValueOrDefault() : Vector2.Zero, this.Handle.Texture.Content.SourceRectangle, null, null, new Vector2(1f, this.ContentScale));
                }
                if (this.Footer > null)
                {
                    this.Footer.Color = System.Drawing.Color.FromArgb(this.AlphaLevel, this.Footer.Color);
                    if (this.ContentScale > 1f)
                    {
                        int num;
                        if (this.Handle.Texture.Footer.SourceRectangle.HasValue)
                        {
                            num = this.Height - this.Handle.Texture.Footer.SourceRectangle.Value.Height;
                        }
                        else
                        {
                            num = this.Height - this.Footer.Texture.GetLevelDescription(0).Height;
                        }
                        this.Footer.Draw(position + new Vector2(0f, (float) num), this.Handle.Texture.Footer.SourceRectangle);
                    }
                    else
                    {
                        vector2 = position;
                        nullable = vector2 + this.Handle.Texture.Footer.Position;
                        this.Footer.Draw(nullable.HasValue ? nullable.GetValueOrDefault() : Vector2.Zero, this.Handle.Texture.Footer.SourceRectangle);
                    }
                }
                SharpDX.Rectangle[] positions = new SharpDX.Rectangle[] { new SharpDX.Rectangle((int) (((position.X + this.Width) - this.HeaderText.Width) - EloBuddy.SDK.Notifications.Notifications.TextOuterPadding.X), (int) (position.Y + EloBuddy.SDK.Notifications.Notifications.TextOuterPadding.Y), (int) this.HeaderText.Size.X, (int) this.HeaderText.Size.Y) };
                this.HeaderText.Draw(this.Handle.HeaderText, System.Drawing.Color.FromArgb(this.AlphaLevel, this.HeaderText.Color), positions);
                SharpDX.Rectangle[] rectangleArray2 = new SharpDX.Rectangle[] { new SharpDX.Rectangle((int) (((position.X + this.Width) - this.ContentText.Width) - EloBuddy.SDK.Notifications.Notifications.TextOuterPadding.X), (int) (((position.Y + EloBuddy.SDK.Notifications.Notifications.TextOuterPadding.Y) + this.HeaderText.Size.Y) + 2f), (int) this.ContentText.Size.X, (int) this.ContentText.Size.Y) };
                this.ContentText.Draw(this.Handle.ContentText, System.Drawing.Color.FromArgb(this.AlphaLevel, this.ContentText.Color), rectangleArray2);
            }

            internal int AlphaLevel { get; set; }

            internal EloBuddy.SDK.Rendering.Sprite Content { get; set; }

            internal float ContentScale { get; set; }

            internal Text ContentText { get; set; }

            internal int Duration { get; set; }

            internal int ExtraTime { get; set; }

            internal EloBuddy.SDK.Rendering.Sprite Footer { get; set; }

            internal INotification Handle { get; set; }

            internal EloBuddy.SDK.Rendering.Sprite Header { get; set; }

            internal Text HeaderText { get; set; }

            internal int Height { get; set; }

            internal int Index { get; set; }

            internal bool IsFadingOut { get; set; }

            internal bool IsValid =>
                (!this._initialized || (((((this.StartTick + this.Duration) + this.ExtraTime) + 500) + 500) > Core.GameTickCount));

            internal int OffsetX { get; set; }

            internal Vector2 Position =>
                new Vector2((float) (((Drawing.Width - this.Handle.RightPadding) - this.Width) + this.OffsetX), (float) (EloBuddy.SDK.Notifications.Notifications.NotificationStart - this.PositionY));

            internal int PositionY { get; set; }

            internal bool ShouldFadeOut =>
                ((((this.StartTick + this.Duration) + this.ExtraTime) + 500) < Core.GameTickCount);

            internal int StartTick { get; set; }

            internal EloBuddy.SDK.ThirdParty.Glide.Tween Tween
            {
                get => 
                    this._tween;
                set
                {
                    this._tween = value;
                    if (value > null)
                    {
                        value.OnComplete(() => this._tween = null);
                    }
                }
            }

            internal int Width { get; set; }

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly EloBuddy.SDK.Notifications.Notifications.ActiveNotification.<>c <>9 = new EloBuddy.SDK.Notifications.Notifications.ActiveNotification.<>c();
                public static Func<NotificationTexture.PartialTexture, bool> <>9__79_1;
                public static Func<EloBuddy.SDK.Notifications.Notifications.ActiveNotification, bool> <>9__79_2;
                public static Func<EloBuddy.SDK.Notifications.Notifications.ActiveNotification, int?> <>9__79_3;

                internal bool <.ctor>b__79_1(NotificationTexture.PartialTexture texture) => 
                    (texture > null);

                internal bool <.ctor>b__79_2(EloBuddy.SDK.Notifications.Notifications.ActiveNotification o) => 
                    !o.IsFadingOut;

                internal int? <.ctor>b__79_3(EloBuddy.SDK.Notifications.Notifications.ActiveNotification o) => 
                    new int?(o.Index);
            }
        }
    }
}

