namespace EloBuddy.SDK.Rendering
{
    using EloBuddy;
    using EloBuddy.SDK;
    using SharpDX;
    using SharpDX.Direct3D9;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class Circle
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <BorderWidth>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EloBuddy.SDK.Rendering.Circle <CircleHandle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.ColorBGRA <ColorBGRA>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static SharpDX.Direct3D9.Effect <Effect>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <Filled>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float <Radius>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static EffectHandle <Technique>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static SharpDX.Direct3D9.VertexBuffer <VertexBuffer>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static SharpDX.Direct3D9.VertexDeclaration <VertexDeclaration>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static VertexElement[] <VertexElements>k__BackingField;
        internal const float DefaultBorderWidth = 1f;

        static Circle()
        {
            VertexBuffer = new SharpDX.Direct3D9.VertexBuffer(Drawing.Direct3DDevice, Utilities.SizeOf<Vector4>() * 6, Usage.WriteOnly, VertexFormat.None, Pool.Managed);
            Vector4[] data = new Vector4[] { new Vector4(-6500f, 0f, -6500f, 1f), new Vector4(-6500f, 0f, 6500f, 1f), new Vector4(6500f, 0f, -6500f, 1f), new Vector4(6500f, 0f, 6500f, 1f), new Vector4(-6500f, 0f, 6500f, 1f), new Vector4(6500f, 0f, -6500f, 1f) };
            VertexBuffer.Lock(0, 0, LockFlags.None).WriteRange<Vector4>(data);
            VertexBuffer.Unlock();
            VertexElements = new VertexElement[] { new VertexElement(0, 0, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Position, 0), new VertexElement(0, 0x10, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Color, 0), VertexElement.VertexDeclarationEnd };
            VertexDeclaration = new SharpDX.Direct3D9.VertexDeclaration(Drawing.Direct3DDevice, VertexElements);
            byte[] memory = new byte[] { 
                1, 9, 0xff, 0xfe, 0x58, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0,
                0, 0, 0, 0, 0x24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0x40, 0x10, 0, 0, 0,
                0x41, 0x4e, 0x54, 0x49, 0x41, 0x4c, 0x49, 0x41, 0x53, 0x5f, 0x57, 0x49, 0x44, 0x54, 0x48, 0,
                3, 0, 0, 0, 2, 0, 0, 0, 0x94, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 4, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x11, 0, 0, 0,
                80, 0x72, 0x6f, 0x6a, 0x65, 0x63, 0x74, 0x69, 0x6f, 110, 0x4d, 0x61, 0x74, 0x72, 0x69, 120,
                0, 0, 0, 0, 3, 0, 0, 0, 1, 0, 0, 0, 0xd8, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                6, 0, 0, 0, 0x43, 0x6f, 0x6c, 0x6f, 0x72, 0, 0, 0, 3, 0, 0, 0,
                0, 0, 0, 0, 4, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0,
                0x52, 0x61, 100, 0x69, 0x75, 0x73, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0,
                0x30, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
                1, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0x57, 0x69, 100, 0x74,
                0x68, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0x5c, 1, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
                0, 0, 0, 0, 7, 0, 0, 0, 70, 0x69, 0x6c, 0x6c, 0x65, 100, 0, 0,
                1, 0, 0, 0, 0, 0, 0, 0, 0x88, 1, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
                8, 0, 0, 0, 0x45, 110, 0x61, 0x62, 0x6c, 0x65, 90, 0, 0, 0, 0, 0,
                1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
                2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 6, 0, 0, 0,
                2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0,
                2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
                0x10, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 2, 0, 0, 0, 15, 0, 0, 0, 4, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0,
                80, 0x30, 0, 0, 5, 0, 0, 0, 0x4d, 0x61, 0x69, 110, 0, 0, 0, 0,
                7, 0, 0, 0, 1, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0,
                4, 0, 0, 0, 0x20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0x38, 0, 0, 0, 0x54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0xac, 0, 0, 0, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0xe4, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0x10, 1, 0, 0, 0x2c, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                60, 1, 0, 0, 0x58, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0x68, 1, 0, 0, 0x84, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0x4c, 2, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0x44, 2, 0, 0,
                0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0x98, 1, 0, 0, 0x94, 1, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0,
                0xb8, 1, 0, 0, 180, 1, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0,
                0xd8, 1, 0, 0, 0xd4, 1, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0,
                0xf8, 1, 0, 0, 0xf4, 1, 0, 0, 0x92, 0, 0, 0, 0, 0, 0, 0,
                0x18, 2, 0, 0, 20, 2, 0, 0, 0x93, 0, 0, 0, 0, 0, 0, 0,
                0x30, 2, 0, 0, 0x2c, 2, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff, 5, 0, 0, 0,
                0, 0, 0, 0, 0x24, 6, 0, 0, 0, 2, 0xff, 0xff, 0xfe, 0xff, 0x3b, 0,
                0x43, 0x54, 0x41, 0x42, 0x1c, 0, 0, 0, 0xb7, 0, 0, 0, 0, 2, 0xff, 0xff,
                3, 0, 0, 0, 0x1c, 0, 0, 0, 0, 0, 0, 0x20, 0xb0, 0, 0, 0,
                0x58, 0, 0, 0, 2, 0, 6, 0, 1, 0, 0, 0, 0x60, 0, 0, 0,
                0x70, 0, 0, 0, 0x80, 0, 0, 0, 2, 0, 8, 0, 1, 0, 0, 0,
                0x88, 0, 0, 0, 0x70, 0, 0, 0, 0x98, 0, 0, 0, 2, 0, 7, 0,
                1, 0, 0, 0, 160, 0, 0, 0, 0x70, 0, 0, 0, 0x43, 0x6f, 0x6c, 0x6f,
                0x72, 0, 0xab, 0xab, 1, 0, 3, 0, 1, 0, 4, 0, 1, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 70, 0x69, 0x6c, 0x6c, 0x65, 100, 0, 0xab, 0, 0, 1, 0,
                1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0x52, 0x61, 100, 0x69,
                0x75, 0x73, 0, 0xab, 0, 0, 3, 0, 1, 0, 1, 0, 1, 0, 0, 0,
                0, 0, 0, 0, 0x70, 0x73, 0x5f, 50, 0x5f, 0x30, 0, 0x4d, 0x69, 0x63, 0x72, 0x6f,
                0x73, 0x6f, 0x66, 0x74, 0x20, 40, 0x52, 0x29, 0x20, 0x48, 0x4c, 0x53, 0x4c, 0x20, 0x53, 0x68,
                0x61, 100, 0x65, 0x72, 0x20, 0x43, 0x6f, 0x6d, 0x70, 0x69, 0x6c, 0x65, 0x72, 0x20, 0x39, 0x2e,
                50, 0x39, 0x2e, 0x39, 0x35, 50, 0x2e, 0x33, 0x31, 0x31, 0x31, 0, 0xfe, 0xff, 0xe3, 0,
                80, 0x52, 0x45, 0x53, 1, 2, 0x58, 70, 0xfe, 0xff, 0x4b, 0, 0x43, 0x54, 0x41, 0x42,
                0x1c, 0, 0, 0, 0xf7, 0, 0, 0, 1, 2, 0x58, 70, 4, 0, 0, 0,
                0x1c, 0, 0, 0, 0, 1, 0, 0x20, 0xf4, 0, 0, 0, 0x6c, 0, 0, 0,
                2, 0, 0, 0, 1, 0, 0, 0, 0x7c, 0, 0, 0, 140, 0, 0, 0,
                0x9c, 0, 0, 0, 2, 0, 1, 0, 1, 0, 0, 0, 0xa4, 0, 0, 0,
                180, 0, 0, 0, 0xc4, 0, 0, 0, 2, 0, 2, 0, 1, 0, 0, 0,
                0xcc, 0, 0, 0, 180, 0, 0, 0, 220, 0, 0, 0, 2, 0, 3, 0,
                1, 0, 0, 0, 0xe4, 0, 0, 0, 180, 0, 0, 0, 0x41, 0x4e, 0x54, 0x49,
                0x41, 0x4c, 0x49, 0x41, 0x53, 0x5f, 0x57, 0x49, 0x44, 0x54, 0x48, 0, 0, 0, 3, 0,
                1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x40,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x43, 0x6f, 0x6c, 0x6f,
                0x72, 0, 0xab, 0xab, 1, 0, 3, 0, 1, 0, 4, 0, 1, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0x52, 0x61, 100, 0x69, 0x75, 0x73, 0, 0xab, 0, 0, 3, 0,
                1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0x57, 0x69, 100, 0x74,
                0x68, 0, 0xab, 0xab, 0, 0, 3, 0, 1, 0, 1, 0, 1, 0, 0, 0,
                0, 0, 0, 0, 0x74, 120, 0, 0x4d, 0x69, 0x63, 0x72, 0x6f, 0x73, 0x6f, 0x66, 0x74,
                0x20, 40, 0x52, 0x29, 0x20, 0x48, 0x4c, 0x53, 0x4c, 0x20, 0x53, 0x68, 0x61, 100, 0x65, 0x72,
                0x20, 0x43, 0x6f, 0x6d, 0x70, 0x69, 0x6c, 0x65, 0x72, 0x20, 0x39, 0x2e, 50, 0x39, 0x2e, 0x39,
                0x35, 50, 0x2e, 0x33, 0x31, 0x31, 0x31, 0, 0xfe, 0xff, 12, 0, 80, 0x52, 0x53, 0x49,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
                6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xfe, 0xff, 0x2a, 0,
                0x43, 0x4c, 0x49, 0x54, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xe0, 0x3f,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0xfe, 0xff, 0x5b, 0, 70, 0x58, 0x4c, 0x43,
                9, 0, 0, 0, 1, 0, 80, 160, 2, 0, 0, 0, 0, 0, 0, 0,
                2, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
                0x10, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0,
                1, 0, 0x40, 160, 2, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 8, 0, 0, 0,
                0, 0, 0, 0, 7, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0x40, 160,
                2, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 4, 0, 0, 0,
                0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                7, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0x10, 1, 0, 0, 0,
                0, 0, 0, 0, 7, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0,
                4, 0, 0, 0, 4, 0, 0, 0, 1, 0, 80, 160, 2, 0, 0, 0,
                0, 0, 0, 0, 7, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
                7, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0,
                0, 0, 0, 0, 1, 0, 0x40, 160, 2, 0, 0, 0, 0, 0, 0, 0,
                7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 12, 0, 0, 0,
                1, 0, 0, 0x10, 1, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 8, 0, 0, 0,
                1, 0, 0x30, 0x10, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0x10, 0, 0, 0,
                3, 0, 0, 0x10, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0,
                4, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 20, 0, 0, 0,
                240, 240, 240, 240, 15, 15, 15, 15, 0xff, 0xff, 0, 0, 0x51, 0, 0, 5,
                9, 0, 15, 160, 0, 0, 0, 0, 0, 0, 0, 0x80, 0, 0, 0x80, 0xbf,
                0, 0, 0, 0, 0x1f, 0, 0, 2, 0, 0, 0, 0x80, 0, 0, 7, 0xb0,
                5, 0, 0, 3, 0, 0, 8, 0x80, 0, 0, 170, 0xb0, 0, 0, 170, 0xb0,
                4, 0, 0, 4, 0, 0, 1, 0x80, 0, 0, 0, 0xb0, 0, 0, 0, 0xb0,
                0, 0, 0xff, 0x80, 7, 0, 0, 2, 0, 0, 2, 0x80, 0, 0, 0, 0x80,
                2, 0, 0, 3, 0, 0, 1, 0x80, 0, 0, 0, 0x81, 0, 0, 0, 160,
                6, 0, 0, 2, 0, 0, 2, 0x80, 0, 0, 0x55, 0x80, 2, 0, 0, 3,
                0, 0, 4, 0x80, 0, 0, 0x55, 0x81, 7, 0, 0, 160, 2, 0, 0, 3,
                0, 0, 2, 0x80, 0, 0, 0x55, 0x80, 1, 0, 0, 0xa1, 1, 0, 0, 2,
                0, 0, 8, 0x80, 9, 0, 0x55, 160, 0x58, 0, 0, 4, 0, 0, 2, 0x80,
                0, 0, 0x55, 0x80, 0, 0, 0xff, 0x80, 8, 0, 0, 0xa1, 0x23, 0, 0, 2,
                0, 0, 4, 0x80, 0, 0, 170, 0x80, 2, 0, 0, 3, 1, 0, 8, 0x80,
                0, 0, 170, 0x81, 3, 0, 0, 160, 5, 0, 0, 3, 1, 0, 1, 0x80,
                1, 0, 0xff, 0x80, 4, 0, 0, 160, 5, 0, 0, 3, 1, 0, 1, 0x80,
                1, 0, 0, 0x80, 6, 0, 0xff, 160, 2, 0, 0, 3, 1, 0, 2, 0x80,
                0, 0, 170, 0x80, 3, 0, 0, 0xa1, 2, 0, 0, 3, 0, 0, 4, 0x80,
                0, 0, 170, 0x80, 2, 0, 0, 0xa1, 0x58, 0, 0, 4, 0, 0, 4, 0x80,
                0, 0, 170, 0x80, 9, 0, 0x55, 160, 9, 0, 170, 160, 0x58, 0, 0, 4,
                0, 0, 4, 0x80, 8, 0, 0, 0xa1, 0, 0, 170, 0x80, 0, 0, 0xff, 0x80,
                0x58, 0, 0, 4, 0, 0, 8, 0x80, 1, 0, 0x55, 0x80, 9, 0, 0, 160,
                1, 0, 0, 0x80, 0x58, 0, 0, 4, 0, 0, 4, 0x80, 0, 0, 170, 0x80,
                0, 0, 0xff, 0x80, 6, 0, 0xff, 160, 0x58, 0, 0, 4, 0, 0, 2, 0x80,
                0, 0, 0x55, 0x80, 0, 0, 170, 0x80, 6, 0, 0xff, 160, 0x58, 0, 0, 4,
                0, 0, 8, 0x80, 0, 0, 0, 0x80, 0, 0, 0x55, 0x80, 9, 0, 0, 160,
                1, 0, 0, 2, 0, 0, 7, 0x80, 5, 0, 0xe4, 160, 1, 0, 0, 2,
                0, 8, 15, 0x80, 0, 0, 0xe4, 0x80, 0xff, 0xff, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff, 4, 0, 0, 0, 0, 0, 0, 0,
                0x4c, 1, 0, 0, 0, 2, 0xfe, 0xff, 0xfe, 0xff, 0x34, 0, 0x43, 0x54, 0x41, 0x42,
                0x1c, 0, 0, 0, 0x9b, 0, 0, 0, 0, 2, 0xfe, 0xff, 1, 0, 0, 0,
                0x1c, 0, 0, 0, 0, 0, 0, 0x20, 0x94, 0, 0, 0, 0x30, 0, 0, 0,
                2, 0, 0, 0, 4, 0, 0, 0, 0x44, 0, 0, 0, 0x54, 0, 0, 0,
                80, 0x72, 0x6f, 0x6a, 0x65, 0x63, 0x74, 0x69, 0x6f, 110, 0x4d, 0x61, 0x74, 0x72, 0x69, 120,
                0, 0xab, 0xab, 0xab, 3, 0, 3, 0, 4, 0, 4, 0, 1, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0x76, 0x73, 0x5f, 50, 0x5f, 0x30, 0, 0x4d, 0x69, 0x63, 0x72, 0x6f,
                0x73, 0x6f, 0x66, 0x74, 0x20, 40, 0x52, 0x29, 0x20, 0x48, 0x4c, 0x53, 0x4c, 0x20, 0x53, 0x68,
                0x61, 100, 0x65, 0x72, 0x20, 0x43, 0x6f, 0x6d, 0x70, 0x69, 0x6c, 0x65, 0x72, 0x20, 0x39, 0x2e,
                50, 0x39, 0x2e, 0x39, 0x35, 50, 0x2e, 0x33, 0x31, 0x31, 0x31, 0, 0x1f, 0, 0, 2,
                0, 0, 0, 0x80, 0, 0, 15, 0x90, 0x1f, 0, 0, 2, 10, 0, 0, 0x80,
                1, 0, 15, 0x90, 9, 0, 0, 3, 0, 0, 1, 0xc0, 0, 0, 0xe4, 0x90,
                0, 0, 0xe4, 160, 9, 0, 0, 3, 0, 0, 2, 0xc0, 0, 0, 0xe4, 0x90,
                1, 0, 0xe4, 160, 9, 0, 0, 3, 0, 0, 4, 0xc0, 0, 0, 0xe4, 0x90,
                2, 0, 0xe4, 160, 9, 0, 0, 3, 0, 0, 8, 0xc0, 0, 0, 0xe4, 0x90,
                3, 0, 0xe4, 160, 1, 0, 0, 2, 0, 0, 15, 0xd0, 1, 0, 0xe4, 0x90,
                1, 0, 0, 2, 0, 0, 15, 0xe0, 0, 0, 0xe4, 0x90, 0xff, 0xff, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0xff, 0xff, 0xff, 0xff, 0, 0, 0, 0,
                0, 0, 0, 0, 220, 0, 0, 0, 0, 2, 0x58, 70, 0xfe, 0xff, 0x24, 0,
                0x43, 0x54, 0x41, 0x42, 0x1c, 0, 0, 0, 0x5b, 0, 0, 0, 0, 2, 0x58, 70,
                1, 0, 0, 0, 0x1c, 0, 0, 0, 0, 1, 0, 0x20, 0x58, 0, 0, 0,
                0x30, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0x38, 0, 0, 0,
                0x48, 0, 0, 0, 0x45, 110, 0x61, 0x62, 0x6c, 0x65, 90, 0, 0, 0, 1, 0,
                1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0x74, 120, 0, 0x4d,
                0x69, 0x63, 0x72, 0x6f, 0x73, 0x6f, 0x66, 0x74, 0x20, 40, 0x52, 0x29, 0x20, 0x48, 0x4c, 0x53,
                0x4c, 0x20, 0x53, 0x68, 0x61, 100, 0x65, 0x72, 0x20, 0x43, 0x6f, 0x6d, 0x70, 0x69, 0x6c, 0x65,
                0x72, 0x20, 0x39, 0x2e, 50, 0x39, 0x2e, 0x39, 0x35, 50, 0x2e, 0x33, 0x31, 0x31, 0x31, 0,
                0xfe, 0xff, 2, 0, 0x43, 0x4c, 0x49, 0x54, 0, 0, 0, 0, 0xfe, 0xff, 12, 0,
                70, 0x58, 0x4c, 0x43, 1, 0, 0, 0, 1, 0, 0, 0x10, 1, 0, 0, 0,
                0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                4, 0, 0, 0, 0, 0, 0, 0, 240, 240, 240, 240, 15, 15, 15, 15,
                0xff, 0xff, 0, 0
            };
            Effect = SharpDX.Direct3D9.Effect.FromMemory(Drawing.Direct3DDevice, memory, ShaderFlags.None);
            Technique = Effect.GetTechnique(0);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(<>c.<>9.<.cctor>b__21_0);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(<>c.<>9.<.cctor>b__21_1);
            Drawing.OnPreReset += new DrawingPreReset(<>c.<>9.<.cctor>b__21_2);
            Drawing.OnPostReset += new DrawingPostReset(<>c.<>9.<.cctor>b__21_3);
            CircleHandle = new EloBuddy.SDK.Rendering.Circle();
        }

        public Circle() : this(SharpDX.Color.Moccasin, 300f, 1f, false)
        {
        }

        public Circle(SharpDX.ColorBGRA color, float radius, float borderWidth = 1f, bool filled = false)
        {
            this.ColorBGRA = color;
            this.Filled = filled;
            this.Radius = radius;
            this.BorderWidth = borderWidth;
        }

        public void Draw(params Vector3[] positions)
        {
            if ((((positions.Length != 0) && (Effect != null)) && !Effect.IsDisposed) && (this.BorderWidth > 0f))
            {
                Core.EndAllDrawing(Core.RenderingType.None);
                SharpDX.Direct3D9.VertexDeclaration vertexDeclaration = Drawing.Direct3DDevice.VertexDeclaration;
                Effect.Technique = Technique;
                Effect.Begin();
                foreach (Vector3 vector in positions)
                {
                    Effect.BeginPass(0);
                    Effect.SetValue("ProjectionMatrix", (Matrix.Translation(new Vector3(vector.X, vector.Z, vector.Y)) * Drawing.View) * Drawing.Projection);
                    Effect.SetValue("Color", this.VectorColor);
                    Effect.SetValue("Radius", this.Radius);
                    Effect.SetValue("Width", this.BorderWidth);
                    Effect.SetValue("Filled", this.Filled);
                    Effect.SetValue("EnableZ", false);
                    Effect.EndPass();
                    Drawing.Direct3DDevice.SetStreamSource(0, VertexBuffer, 0, Utilities.SizeOf<Vector4>());
                    Drawing.Direct3DDevice.VertexDeclaration = VertexDeclaration;
                    Drawing.Direct3DDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
                }
                Effect.End();
                Drawing.Direct3DDevice.VertexDeclaration = vertexDeclaration;
            }
        }

        public static void Draw(SharpDX.ColorBGRA color, float radius, params GameObject[] objects)
        {
            Draw(color, radius, 1f, objects);
        }

        public static void Draw(SharpDX.ColorBGRA color, float radius, params Vector3[] positions)
        {
            Draw(color, radius, 1f, positions);
        }

        public static void Draw(SharpDX.ColorBGRA color, float radius, float borderWidth = 1f, params GameObject[] objects)
        {
            Draw(color, radius, borderWidth, (from o in objects select new Vector3(o.Position.X, o.Position.Y, NavMesh.GetHeightForPosition(o.Position.X, o.Position.Y))).ToArray<Vector3>());
        }

        public static void Draw(SharpDX.ColorBGRA color, float radius, float borderWidth = 1f, params Vector3[] positions)
        {
            CircleHandle.ColorBGRA = color;
            CircleHandle.Radius = radius;
            CircleHandle.BorderWidth = borderWidth;
            CircleHandle.Draw(positions);
        }

        internal static void OnDispose()
        {
            if (Effect > null)
            {
                Effect.Dispose();
                Effect = null;
            }
            if (Technique > null)
            {
                Technique.Dispose();
                Technique = null;
            }
            if (VertexBuffer > null)
            {
                VertexBuffer.Dispose();
                VertexBuffer = null;
            }
            if (VertexDeclaration > null)
            {
                VertexDeclaration.Dispose();
                VertexDeclaration = null;
            }
        }

        public float BorderWidth { get; set; }

        internal static EloBuddy.SDK.Rendering.Circle CircleHandle
        {
            [CompilerGenerated]
            get => 
                <CircleHandle>k__BackingField;
            [CompilerGenerated]
            set
            {
                <CircleHandle>k__BackingField = value;
            }
        }

        public System.Drawing.Color Color
        {
            get => 
                System.Drawing.Color.FromArgb(this.ColorBGRA.A, this.ColorBGRA.R, this.ColorBGRA.G, this.ColorBGRA.B);
            set
            {
                this.ColorBGRA = new SharpDX.ColorBGRA(value.R, value.G, value.B, value.A);
            }
        }

        public SharpDX.ColorBGRA ColorBGRA { get; set; }

        internal static SharpDX.Direct3D9.Effect Effect
        {
            [CompilerGenerated]
            get => 
                <Effect>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Effect>k__BackingField = value;
            }
        }

        public bool Filled { get; set; }

        public float Radius { get; set; }

        internal static EffectHandle Technique
        {
            [CompilerGenerated]
            get => 
                <Technique>k__BackingField;
            [CompilerGenerated]
            set
            {
                <Technique>k__BackingField = value;
            }
        }

        internal Vector4 VectorColor =>
            new Vector4(((float) this.ColorBGRA.R) / 255f, ((float) this.ColorBGRA.G) / 255f, ((float) this.ColorBGRA.B) / 255f, ((float) this.ColorBGRA.A) / 255f);

        internal static SharpDX.Direct3D9.VertexBuffer VertexBuffer
        {
            [CompilerGenerated]
            get => 
                <VertexBuffer>k__BackingField;
            [CompilerGenerated]
            set
            {
                <VertexBuffer>k__BackingField = value;
            }
        }

        internal static SharpDX.Direct3D9.VertexDeclaration VertexDeclaration
        {
            [CompilerGenerated]
            get => 
                <VertexDeclaration>k__BackingField;
            [CompilerGenerated]
            set
            {
                <VertexDeclaration>k__BackingField = value;
            }
        }

        internal static VertexElement[] VertexElements
        {
            [CompilerGenerated]
            get => 
                <VertexElements>k__BackingField;
            [CompilerGenerated]
            set
            {
                <VertexElements>k__BackingField = value;
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly EloBuddy.SDK.Rendering.Circle.<>c <>9 = new EloBuddy.SDK.Rendering.Circle.<>c();
            public static Func<GameObject, Vector3> <>9__27_0;

            internal void <.cctor>b__21_0(object <sender>, EventArgs <e>)
            {
                EloBuddy.SDK.Rendering.Circle.OnDispose();
            }

            internal void <.cctor>b__21_1(object <sender>, EventArgs <e>)
            {
                EloBuddy.SDK.Rendering.Circle.OnDispose();
            }

            internal void <.cctor>b__21_2(EventArgs e)
            {
                EloBuddy.SDK.Rendering.Circle.Effect.OnLostDevice();
            }

            internal void <.cctor>b__21_3(EventArgs e)
            {
                EloBuddy.SDK.Rendering.Circle.Effect.OnResetDevice();
            }

            internal Vector3 <Draw>b__27_0(GameObject o) => 
                new Vector3(o.Position.X, o.Position.Y, NavMesh.GetHeightForPosition(o.Position.X, o.Position.Y));
        }
    }
}

