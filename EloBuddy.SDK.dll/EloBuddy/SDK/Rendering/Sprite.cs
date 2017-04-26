namespace EloBuddy.SDK.Rendering
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Utils;
    using SharpDX;
    using SharpDX.Direct3D9;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public sealed class Sprite
    {
        internal System.Drawing.Color _color;
        internal ColorBGRA _colorBrga;
        internal SharpDX.Direct3D9.Texture _texture;
        internal Func<SharpDX.Direct3D9.Texture> _textureDelegate;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector3? <CenterRef>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static SharpDX.Direct3D9.Sprite <Handle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsDrawing>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsOnEndScene>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.Rectangle? <Rectangle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float? <Rotation>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Vector2? <Scale>k__BackingField;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        internal static  event MenuDrawHandler OnMenuDraw;

        static Sprite()
        {
            Handle = new SharpDX.Direct3D9.Sprite(Drawing.Direct3DDevice);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(EloBuddy.SDK.Rendering.Sprite.OnAppDomainUnload);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(EloBuddy.SDK.Rendering.Sprite.OnAppDomainUnload);
            Drawing.OnEndScene += new DrawingEndScene(EloBuddy.SDK.Rendering.Sprite.OnEndScene);
            Drawing.OnFlushEndScene += new DrawingFlushEndScene(EloBuddy.SDK.Rendering.Sprite.OnFlush);
            Drawing.OnPreReset += new DrawingPreReset(EloBuddy.SDK.Rendering.Sprite.OnPreReset);
            Drawing.OnPostReset += new DrawingPostReset(EloBuddy.SDK.Rendering.Sprite.OnPostReset);
        }

        public Sprite(SharpDX.Direct3D9.Texture texture)
        {
            this._colorBrga = SharpDX.Color.White;
            this._color = System.Drawing.Color.White;
            this.Texture = texture;
        }

        public Sprite(Func<SharpDX.Direct3D9.Texture> textureDelegate)
        {
            this._colorBrga = SharpDX.Color.White;
            this._color = System.Drawing.Color.White;
            this._textureDelegate = textureDelegate;
        }

        public void Draw(Vector2 position)
        {
            this.Draw(position, this.Rectangle);
        }

        public void Draw(Vector2 position, SharpDX.Rectangle? rectangle)
        {
            this.Draw(position, rectangle, this.CenterRef, this.Rotation, this.Scale);
        }

        public void Draw(Vector2 position, SharpDX.Rectangle? rectangle, Vector3? centerRef, float? rotation = new float?(), Vector2? scale = new Vector2?())
        {
            if ((Handle != null) && !Handle.IsDisposed)
            {
                Vector3? nullable;
                if (!IsDrawing)
                {
                    Core.EndAllDrawing(Core.RenderingType.Sprite);
                    Handle.Begin(SpriteFlags.None);
                    IsDrawing = true;
                }
                if (!rotation.HasValue && !scale.HasValue)
                {
                    nullable = centerRef;
                    Handle.Draw(this.Texture, this._colorBrga, rectangle, centerRef, new Vector3?(new Vector3(position, 0f) + (nullable.HasValue ? nullable.GetValueOrDefault() : Vector3.Zero)));
                }
                else
                {
                    Matrix transform = Handle.Transform;
                    try
                    {
                        SharpDX.Direct3D9.Sprite handle = Handle;
                        Vector2? nullable2 = scale;
                        float? nullable3 = rotation;
                        nullable = centerRef;
                        handle.Transform *= (Matrix.Scaling(new Vector3(nullable2.HasValue ? nullable2.GetValueOrDefault() : new Vector2(1f), 0f)) * Matrix.RotationZ(nullable3.HasValue ? nullable3.GetValueOrDefault() : 0f)) * Matrix.Translation(new Vector3(position, 0f) + (nullable.HasValue ? nullable.GetValueOrDefault() : Vector3.Zero));
                        Handle.Draw(this.Texture, this._colorBrga, rectangle, centerRef, null);
                    }
                    catch (Exception exception)
                    {
                        Logger.Debug("Failed to draw sprite with transformation:", new object[0]);
                        Logger.Debug(exception.ToString(), new object[0]);
                    }
                    Handle.Transform = transform;
                }
            }
        }

        internal static void OnAppDomainUnload(object sender, EventArgs eventArgs)
        {
            if (Handle > null)
            {
                Core.EndAllDrawing(Core.RenderingType.None);
                Handle.Dispose();
                Handle = null;
            }
        }

        internal static void OnEndScene(EventArgs args)
        {
            IsOnEndScene = true;
        }

        internal static void OnFlush(EventArgs args)
        {
            if (IsOnEndScene && (OnMenuDraw > null))
            {
                IsOnEndScene = false;
                OnMenuDraw(EventArgs.Empty);
            }
            if (IsDrawing)
            {
                Handle.End();
                IsDrawing = false;
            }
        }

        internal static void OnPostReset(EventArgs args)
        {
            Handle.OnResetDevice();
        }

        internal static void OnPreReset(EventArgs args)
        {
            Handle.OnLostDevice();
        }

        public Vector3? CenterRef { get; set; }

        public System.Drawing.Color Color
        {
            get => 
                this._color;
            set
            {
                this._color = value;
                this._colorBrga = new ColorBGRA(value.R, value.G, value.B, value.A);
            }
        }

        internal static SharpDX.Direct3D9.Sprite Handle
        {
            [CompilerGenerated]
            get => 
                <Handle>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Handle>k__BackingField = value;
            }
        }

        internal static bool IsDrawing
        {
            [CompilerGenerated]
            get => 
                <IsDrawing>k__BackingField;
            [CompilerGenerated]
            set
            {
                <IsDrawing>k__BackingField = value;
            }
        }

        internal static bool IsOnEndScene
        {
            [CompilerGenerated]
            get => 
                <IsOnEndScene>k__BackingField;
            [CompilerGenerated]
            set
            {
                <IsOnEndScene>k__BackingField = value;
            }
        }

        public SharpDX.Rectangle? Rectangle { get; set; }

        public float? Rotation { get; set; }

        public Vector2? Scale { get; set; }

        public SharpDX.Direct3D9.Texture Texture
        {
            get => 
                (this._texture ?? this._textureDelegate());
            internal set
            {
                this._texture = value;
            }
        }

        internal delegate void MenuDrawHandler(EventArgs args);
    }
}

