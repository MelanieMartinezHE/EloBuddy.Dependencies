namespace EloBuddy.SDK.Rendering
{
    using EloBuddy;
    using EloBuddy.SDK;
    using SharpDX;
    using SharpDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class Line
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Antialias>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Drawing.Color <Color>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static SharpDX.Direct3D9.Line <Handle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static bool <IsDrawing>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EloBuddy.SDK.Rendering.Line <LineHandle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IEnumerable<Vector2> <ScreenVertices>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Matrix? <Transform>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Width>k__BackingField;
        internal const float DefaultWidth = 2f;

        static Line()
        {
            SharpDX.Direct3D9.Line line1 = new SharpDX.Direct3D9.Line(Drawing.Direct3DDevice) {
                Antialias = 1
            };
            Handle = line1;
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(EloBuddy.SDK.Rendering.Line.OnAppDomainUnload);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(EloBuddy.SDK.Rendering.Line.OnAppDomainUnload);
            Drawing.OnFlushEndScene += new DrawingFlushEndScene(EloBuddy.SDK.Rendering.Line.OnFlush);
            Drawing.OnPreReset += new DrawingPreReset(EloBuddy.SDK.Rendering.Line.OnPreReset);
            Drawing.OnPostReset += new DrawingPostReset(EloBuddy.SDK.Rendering.Line.OnPostReset);
            LineHandle = new EloBuddy.SDK.Rendering.Line();
        }

        public Line()
        {
            this.Width = 2f;
            this.Antialias = true;
        }

        public void Draw()
        {
            if (this.ScreenVertices != null)
            {
                this.Draw(this.Color, this.Transform, this.ScreenVertices.ToArray<Vector2>(), this.Width, true);
            }
        }

        public void Draw(System.Drawing.Color color, params Vector2[] screenVertices)
        {
            this.Draw(color, null, screenVertices, 2f, true);
        }

        public void Draw(System.Drawing.Color color, params Vector3[] worldVertices)
        {
            this.Draw(color, null, (from o in worldVertices select o.WorldToScreen()).ToArray<Vector2>(), 2f, true);
        }

        public void Draw(System.Drawing.Color color, float width = 2f, params Vector2[] screenVertices)
        {
            this.Draw(color, null, screenVertices, width, true);
        }

        public void Draw(System.Drawing.Color color, float width = 2f, params Vector3[] worldVertices)
        {
            this.Draw(color, null, (from o in worldVertices select o.WorldToScreen()).ToArray<Vector2>(), width, true);
        }

        internal void Draw(System.Drawing.Color color, Matrix? transform, Vector2[] vertices, float width = 2f, bool antialias = true)
        {
            if ((((Handle != null) && !Handle.IsDisposed) && (vertices.Length >= 2)) && (width > 0f))
            {
                if (!IsDrawing)
                {
                    Core.EndAllDrawing(Core.RenderingType.Line);
                    Handle.Antialias = antialias;
                    Handle.Begin();
                    IsDrawing = true;
                }
                if (Math.Abs((float) (Handle.Width - width)) > float.Epsilon)
                {
                    Handle.End();
                    Handle.Width = width;
                    Handle.Begin();
                }
                if (!transform.HasValue)
                {
                    Handle.Draw(vertices, new ColorBGRA(color.R, color.G, color.B, color.A));
                }
                else
                {
                    Handle.DrawTransform<Vector2>(vertices, transform.Value, new ColorBGRA(color.R, color.G, color.B, color.A));
                }
                if (!antialias)
                {
                    Handle.End();
                    Handle.Antialias = 0;
                    IsDrawing = false;
                }
            }
        }

        public static void DrawLine(System.Drawing.Color color, params Vector2[] screenVertices)
        {
            DrawLine(color, 2f, screenVertices);
        }

        public static void DrawLine(System.Drawing.Color color, params Vector3[] worldVertices)
        {
            DrawLine(color, 2f, worldVertices);
        }

        public static void DrawLine(System.Drawing.Color color, float width = 2f, params Vector2[] screenVertices)
        {
            LineHandle.Draw(color, width, screenVertices);
        }

        public static void DrawLine(System.Drawing.Color color, float width = 2f, params Vector3[] worldVertices)
        {
            LineHandle.Draw(color, width, worldVertices);
        }

        public static void DrawLineTransform(System.Drawing.Color color, Matrix transform, params Vector2[] screenVertices)
        {
            DrawLineTransform(color, transform, 2f, screenVertices);
        }

        public static void DrawLineTransform(System.Drawing.Color color, Matrix transform, params Vector3[] worldVertices)
        {
            DrawLineTransform(color, transform, 2f, worldVertices);
        }

        public static void DrawLineTransform(System.Drawing.Color color, Matrix transform, float width = 2f, params Vector2[] screenVertices)
        {
            LineHandle.DrawTransform(color, transform, width, screenVertices);
        }

        public static void DrawLineTransform(System.Drawing.Color color, Matrix transform, float width = 2f, params Vector3[] worldVertices)
        {
            LineHandle.DrawTransform(color, transform, width, worldVertices);
        }

        public void DrawTransform(System.Drawing.Color color, Matrix transform, params Vector2[] screenVertices)
        {
            this.Draw(color, new Matrix?(transform), screenVertices, 2f, true);
        }

        public void DrawTransform(System.Drawing.Color color, Matrix transform, params Vector3[] worldVertices)
        {
            this.Draw(color, new Matrix?(transform), (from o in worldVertices select o.WorldToScreen()).ToArray<Vector2>(), 2f, true);
        }

        public void DrawTransform(System.Drawing.Color color, Matrix transform, float width = 2f, params Vector2[] screenVertices)
        {
            this.Draw(color, new Matrix?(transform), screenVertices, width, true);
        }

        public void DrawTransform(System.Drawing.Color color, Matrix transform, float width = 2f, params Vector3[] worldVertices)
        {
            this.Draw(color, new Matrix?(transform), (from o in worldVertices select o.WorldToScreen()).ToArray<Vector2>(), width, true);
        }

        internal static void OnAppDomainUnload(object sender, EventArgs e)
        {
            if (Handle > null)
            {
                Core.EndAllDrawing(Core.RenderingType.None);
                Handle.Dispose();
                Handle = null;
            }
        }

        internal static void OnFlush(EventArgs args)
        {
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

        public bool Antialias { get; set; }

        public System.Drawing.Color Color { get; set; }

        internal static SharpDX.Direct3D9.Line Handle
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

        internal static EloBuddy.SDK.Rendering.Line LineHandle
        {
            [CompilerGenerated]
            get => 
                <LineHandle>k__BackingField;
            [CompilerGenerated]
            set
            {
                <LineHandle>k__BackingField = value;
            }
        }

        public IEnumerable<Vector2> ScreenVertices { get; set; }

        public Matrix? Transform { get; set; }

        public float Width { get; set; }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly EloBuddy.SDK.Rendering.Line.<>c <>9 = new EloBuddy.SDK.Rendering.Line.<>c();
            public static Func<Vector3, Vector2> <>9__50_0;
            public static Func<Vector3, Vector2> <>9__51_0;
            public static Func<Vector3, Vector2> <>9__54_0;
            public static Func<Vector3, Vector2> <>9__55_0;

            internal Vector2 <Draw>b__50_0(Vector3 o) => 
                o.WorldToScreen();

            internal Vector2 <Draw>b__51_0(Vector3 o) => 
                o.WorldToScreen();

            internal Vector2 <DrawTransform>b__54_0(Vector3 o) => 
                o.WorldToScreen();

            internal Vector2 <DrawTransform>b__55_0(Vector3 o) => 
                o.WorldToScreen();
        }
    }
}

