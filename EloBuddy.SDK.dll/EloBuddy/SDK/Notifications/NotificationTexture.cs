namespace EloBuddy.SDK.Notifications
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class NotificationTexture
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PartialTexture <Content>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PartialTexture <Footer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private PartialTexture <Header>k__BackingField;

        public PartialTexture Content { get; set; }

        public PartialTexture Footer { get; set; }

        public PartialTexture Header { get; set; }

        public sealed class PartialTexture
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Vector2? <Position>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Rectangle? <SourceRectangle>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Func<SharpDX.Direct3D9.Texture> <Texture>k__BackingField;

            public Vector2? Position { get; set; }

            public Rectangle? SourceRectangle { get; set; }

            public Func<SharpDX.Direct3D9.Texture> Texture { get; set; }
        }
    }
}

