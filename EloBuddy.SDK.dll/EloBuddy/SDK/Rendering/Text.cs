namespace EloBuddy.SDK.Rendering
{
    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Properties;
    using EloBuddy.SDK.Utils;
    using SharpDX;
    using SharpDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Text;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    public sealed class Text : IDisposable
    {
        internal ColorBGRA _color;
        internal int _crop;
        internal FontDrawFlags _drawFlags;
        internal Vector2 _padding;
        internal Vector2 _position;
        internal Vector2 _size;
        internal string _textValue;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.Rectangle <Bounding>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.Rectangle <BoundingIgnoredSizeAndFlags>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <DisplayedText>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <FullCrop>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.Rectangle <PositionRectangle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Align <TextAlign>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SharpDX.Direct3D9.Font <TextHandle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Orientation <TextOrientation>k__BackingField;
        internal static readonly List<byte[]> DefaultFonts;
        internal static readonly string FontDirectoryPath = (DefaultSettings.EloBuddyPath + Path.DirectorySeparatorChar + "Fonts");
        internal static readonly List<IntPtr> FontHandles;
        internal static readonly PrivateFontCollection PrivateFonts;
        internal static readonly string[] ValidFontEndings;

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        static Text()
        {
            List<byte[]> list1 = new List<byte[]> {
                Resources.Gill_Sans_MT_Light,
                Resources.Gill_Sans_MT_Pro_Book,
                Resources.Gill_Sans_MT_Pro_Medium
            };
            DefaultFonts = list1;
            ValidFontEndings = new string[] { ".fon", ".fnt", ".ttf", ".ttc", ".fot", ".otf", ".mmm", ".pfb", ".pfm" };
            FontHandles = new List<IntPtr>();
            PrivateFonts = new PrivateFontCollection();
            try
            {
                foreach (byte[] buffer in DefaultFonts)
                {
                    LoadFont(buffer);
                }
                Directory.CreateDirectory(FontDirectoryPath);
                foreach (string str in Directory.GetFiles(FontDirectoryPath).SelectMany<string, string, string>(new Func<string, IEnumerable<string>>(<>c.<>9.<.cctor>b__10_0), new Func<string, string, string>(<>c.<>9.<.cctor>b__10_1)))
                {
                    using (FileStream stream = new FileStream(str, FileMode.Open, FileAccess.ReadWrite))
                    {
                        byte[] buffer2 = new byte[stream.Length];
                        stream.Read(buffer2, 0, buffer2.Length);
                        LoadFont(buffer2);
                    }
                }
                AppDomain.CurrentDomain.DomainUnload += new EventHandler(Text.OnStaticUnload);
                AppDomain.CurrentDomain.ProcessExit += new EventHandler(Text.OnStaticUnload);
                object[] args = new object[] { PrivateFonts.Families.Length };
                Logger.Info("Loaded a total of {0} fonts:", args);
                foreach (FontFamily family in PrivateFonts.Families)
                {
                    object[] objArray2 = new object[] { family.Name };
                    Logger.Info(" -> {0}", objArray2);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                OnStaticUnload(null, EventArgs.Empty);
            }
        }

        public Text()
        {
            this._size = new Vector2(10000f, 10000f);
            this._drawFlags = FontDrawFlags.Left;
            this._color = SharpDX.Color.White;
            this.RegisterEventHandlers();
        }

        public Text(string textValue, FontDescription fontDescription)
        {
            this._size = new Vector2(10000f, 10000f);
            this._drawFlags = FontDrawFlags.Left;
            this._color = SharpDX.Color.White;
            this._textValue = textValue;
            this.ReplaceFont(fontDescription);
            this.RegisterEventHandlers();
        }

        public Text(string textValue, System.Drawing.Font font)
        {
            this._size = new Vector2(10000f, 10000f);
            this._drawFlags = FontDrawFlags.Left;
            this._color = SharpDX.Color.White;
            this._textValue = textValue;
            this.ReplaceFont(font);
            this.RegisterEventHandlers();
        }

        public Text(string textValue, int height, int width, FontWeight weight, int mipLevels, bool isItalic, FontCharacterSet characterSet, FontPrecision precision, FontQuality quality, FontPitchAndFamily pitchAndFamily, string faceName)
        {
            this._size = new Vector2(10000f, 10000f);
            this._drawFlags = FontDrawFlags.Left;
            this._color = SharpDX.Color.White;
            this._textValue = textValue;
            this.ReplaceFont(height, width, weight, mipLevels, isItalic, characterSet, precision, quality, pitchAndFamily, faceName);
            this.RegisterEventHandlers();
        }

        [DllImport("gdi32.dll")]
        public static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        [DllImport("gdi32.dll")]
        internal static extern int AddFontResourceEx(string lpszFilename, uint fl, IntPtr pdv);
        public void ApplyToControlPosition(Control control)
        {
            float x = 0f;
            float y = 0f;
            switch (this.TextAlign)
            {
                case Align.Center:
                    x = (control.Size.X - this.BoundingIgnoredSizeAndFlags.Width) / 2f;
                    break;

                case Align.Right:
                    x = control.Size.X - this.BoundingIgnoredSizeAndFlags.Width;
                    break;
            }
            switch (this.TextOrientation)
            {
                case Orientation.Center:
                    y = (control.Size.Y - this.BoundingIgnoredSizeAndFlags.Height) / 2f;
                    break;

                case Orientation.Bottom:
                    y = control.Size.Y - this.BoundingIgnoredSizeAndFlags.Height;
                    break;
            }
            Vector2 vector = new Vector2(x, y);
            this.Position = (new Vector2(control.Position.X, control.Position.Y) + this.Padding) + vector;
            this.Width = (int) (control.Size.X - (this.Position.X - control.Position.X));
            if (control.FullCrop)
            {
                this.Crop = 0x7fffffff;
            }
            else if (control.Crop > 0)
            {
                float num3 = this.Padding.Y + vector.Y;
                if (control.Crop > 0)
                {
                    this.Crop = (int) Math.Max((float) 0f, (float) (control.Crop - num3));
                }
                else
                {
                    num3 = control.Size.Y - (num3 + this.Bounding.Height);
                    this.Crop = (int) Math.Min((float) 0f, (float) (control.Crop + num3));
                }
            }
            else
            {
                this.Crop = 0;
            }
        }

        public void Dispose()
        {
            if (this.TextHandle > null)
            {
                this.TextHandle.Dispose();
                this.TextHandle = null;
            }
            Drawing.OnPreReset -= new DrawingPreReset(this.OnPreReset);
            Drawing.OnPostReset -= new DrawingPostReset(this.OnPostReset);
            AppDomain.CurrentDomain.ProcessExit -= new EventHandler(this.OnUnload);
            AppDomain.CurrentDomain.DomainUnload -= new EventHandler(this.OnUnload);
        }

        public void Draw()
        {
            if (!this.FullCrop)
            {
                if (!this.IsDrawing)
                {
                    Core.EndAllDrawing(Core.RenderingType.Sprite);
                    this.SpriteHandle.Begin(SpriteFlags.None);
                    this.IsDrawing = true;
                }
                this.TextHandle.DrawText(this.SpriteHandle, this.DisplayedText, this.PositionRectangle, this.DrawFlags, this._color);
            }
        }

        public void Draw(string text, SharpDX.Color color, params SharpDX.Rectangle[] positions)
        {
            if (!this.IsDrawing)
            {
                Core.EndAllDrawing(Core.RenderingType.Sprite);
                this.SpriteHandle.Begin(SpriteFlags.None);
                this.IsDrawing = true;
            }
            foreach (SharpDX.Rectangle rectangle in positions)
            {
                this.TextHandle.DrawText(this.SpriteHandle, text, rectangle, this.DrawFlags, new ColorBGRA(color.R, color.G, color.B, color.A));
            }
        }

        public void Draw(string text, SharpDX.Color color, params Vector2[] positions)
        {
            if (!this.IsDrawing)
            {
                Core.EndAllDrawing(Core.RenderingType.Sprite);
                this.SpriteHandle.Begin(SpriteFlags.None);
                this.IsDrawing = true;
            }
            foreach (Vector2 vector in positions)
            {
                this.TextHandle.DrawText(this.SpriteHandle, text, (int) vector.X, (int) vector.Y, new ColorBGRA(color.R, color.G, color.B, color.A));
            }
        }

        public void Draw(string text, System.Drawing.Color color, Vector2 screenPosition)
        {
            this.Draw(text, (SharpDX.Color) new ColorBGRA(color.R, color.G, color.B, color.A), (int) screenPosition.X, (int) screenPosition.Y);
        }

        public void Draw(string text, System.Drawing.Color color, params SharpDX.Rectangle[] positions)
        {
            this.Draw(text, (SharpDX.Color) new ColorBGRA(color.R, color.G, color.B, color.A), positions);
        }

        public void Draw(string text, System.Drawing.Color color, params Vector2[] positions)
        {
            this.Draw(text, (SharpDX.Color) new ColorBGRA(color.R, color.G, color.B, color.A), positions);
        }

        public void Draw(string text, SharpDX.Color color, int x, int y)
        {
            if (!this.IsDrawing)
            {
                Core.EndAllDrawing(Core.RenderingType.Sprite);
                this.SpriteHandle.Begin(SpriteFlags.None);
                this.IsDrawing = true;
            }
            this.TextHandle.DrawText(this.SpriteHandle, text, x, y, color);
        }

        public void Draw(string text, System.Drawing.Color color, int x, int y)
        {
            this.Draw(text, (SharpDX.Color) new ColorBGRA(color.R, color.G, color.B, color.A), x, y);
        }

        public void Draw(string text, System.Drawing.Color color, Obj_AI_Base obj, int extraX, int extraY)
        {
            Vector2 vector = obj.Position.WorldToScreen();
            Vector2 vector2 = new Vector2(vector.X + extraX, vector.Y + extraY);
            this.Draw(text, (SharpDX.Color) new ColorBGRA(color.R, color.G, color.B, color.A), (int) vector2.X, (int) vector2.Y);
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        internal static unsafe void LoadFont(byte[] fontData)
        {
            fixed (byte* numRef = fontData)
            {
                IntPtr memory = new IntPtr((void*) numRef);
                PrivateFonts.AddMemoryFont(memory, fontData.Length);
                uint pcFonts = 1;
                FontHandles.Add(AddFontMemResourceEx(memory, (uint) fontData.Length, IntPtr.Zero, ref pcFonts));
            }
        }

        public SharpDX.Rectangle MeasureBounding() => 
            this.MeasureBounding(this.TextValue, new SharpDX.Rectangle?(this.PositionRectangle), new FontDrawFlags?(this.DrawFlags));

        public SharpDX.Rectangle MeasureBounding(string text, SharpDX.Rectangle? maxSize = new SharpDX.Rectangle?(), FontDrawFlags? flags = new FontDrawFlags?())
        {
            FontDrawFlags? nullable;
            return (!maxSize.HasValue ? this.TextHandle.MeasureText(this.SpriteHandle, text, (nullable = flags).HasValue ? nullable.GetValueOrDefault() : FontDrawFlags.NoClip) : this.TextHandle.MeasureText(this.SpriteHandle, text, maxSize.Value, (nullable = flags).HasValue ? nullable.GetValueOrDefault() : FontDrawFlags.WordBreak));
        }

        internal void OnPostReset(EventArgs args)
        {
            this.TextHandle.OnResetDevice();
        }

        internal void OnPreReset(EventArgs args)
        {
            this.TextHandle.OnLostDevice();
        }

        [PermissionSet(SecurityAction.Assert, Unrestricted=true)]
        internal static void OnStaticUnload(object sender, EventArgs e)
        {
            if ((FontHandles != null) && (FontHandles.Count > 0))
            {
                foreach (IntPtr ptr in FontHandles)
                {
                    RemoveFontMemResourceEx(ptr);
                }
                FontHandles.Clear();
            }
        }

        internal void OnUnload(object sender, EventArgs e)
        {
            this.Dispose();
        }

        internal void RecalculateBoundingAndDisplayedText()
        {
            this.BoundingIgnoredSizeAndFlags = this.TextHandle.MeasureText(this.SpriteHandle, this.TextValue, FontDrawFlags.NoClip);
            this.DisplayedText = this.TextValue;
            this.Bounding = this.BoundingIgnoredSizeAndFlags;
            if ((this.Bounding.Width - 4) > this.Width)
            {
                while (((this.Bounding.Width - 4) > this.Width) && (this.DisplayedText.Length > 3))
                {
                    this.DisplayedText = this.DisplayedText.Substring(0, this.DisplayedText.Length - 4) + "...";
                    this.Bounding = this.TextHandle.MeasureText(this.SpriteHandle, this.DisplayedText, this.DrawFlags);
                }
            }
            this.RecalculatePositionRectangle();
            this.TextHandle.PreloadText(this.DisplayedText);
        }

        internal void RecalculatePositionRectangle()
        {
            this.FullCrop = false;
            if (this.Crop == 0)
            {
                this.PositionRectangle = new SharpDX.Rectangle((int) this.Position.X, (int) this.Position.Y, this.Bounding.Width, this.Bounding.Height);
            }
            else
            {
                if ((this.Crop >= this.Height) || (this.Crop <= -this.Height))
                {
                    this.FullCrop = true;
                    this.PositionRectangle = new SharpDX.Rectangle((int) this.Position.X, (int) this.Position.Y, 0, 0);
                }
                else if (this.Crop > 0)
                {
                    this.PositionRectangle = new SharpDX.Rectangle((int) this.Position.X, ((int) this.Position.Y) + this.Crop, this.Bounding.Width, this.Bounding.Height - this.Crop);
                    this.DrawFlags = FontDrawFlags.Bottom | FontDrawFlags.Center;
                }
                else
                {
                    this.PositionRectangle = new SharpDX.Rectangle((int) this.Position.X, (int) this.Position.Y, this.Bounding.Width, this.Bounding.Height + this.Crop);
                    this.DrawFlags = FontDrawFlags.Center;
                }
                if (this.PositionRectangle.Height <= 0)
                {
                    this.FullCrop = true;
                }
            }
        }

        internal void RegisterEventHandlers()
        {
            Drawing.OnPreReset += new DrawingPreReset(this.OnPreReset);
            Drawing.OnPostReset += new DrawingPostReset(this.OnPostReset);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(this.OnUnload);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(this.OnUnload);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool RemoveFontMemResourceEx(IntPtr fh);
        public void ReplaceFont(FontDescription fontDescription)
        {
            if (this.TextHandle > null)
            {
                this.TextHandle.Dispose();
            }
            this.TextHandle = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, fontDescription);
            this.RecalculateBoundingAndDisplayedText();
        }

        public void ReplaceFont(System.Drawing.Font font)
        {
            if (this.TextHandle > null)
            {
                this.TextHandle.Dispose();
            }
            this.TextHandle = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, font);
            this.RecalculateBoundingAndDisplayedText();
        }

        public void ReplaceFont(int height, int width, FontWeight weight, int mipLevels, bool isItalic, FontCharacterSet characterSet, FontPrecision precision, FontQuality quality, FontPitchAndFamily pitchAndFamily, string faceName)
        {
            if (this.TextHandle > null)
            {
                this.TextHandle.Dispose();
            }
            this.TextHandle = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, height, width, weight, mipLevels, isItalic, characterSet, precision, quality, pitchAndFamily, faceName);
            this.RecalculateBoundingAndDisplayedText();
        }

        public SharpDX.Rectangle Bounding { get; internal set; }

        public SharpDX.Rectangle BoundingIgnoredSizeAndFlags { get; internal set; }

        public System.Drawing.Color Color
        {
            get => 
                System.Drawing.Color.FromArgb(this._color.A, this._color.R, this._color.G, this._color.B);
            set
            {
                this._color = new ColorBGRA(value.R, value.G, value.B, value.A);
            }
        }

        internal int Crop
        {
            get => 
                this._crop;
            set
            {
                if (this._crop != value)
                {
                    this._crop = value;
                    this.RecalculatePositionRectangle();
                }
            }
        }

        public static List<string> CustomFonts =>
            new List<string>(from o in PrivateFonts.Families select o.Name);

        public FontDescription Description =>
            this.TextHandle.Description;

        public string DisplayedText { get; internal set; }

        public FontDrawFlags DrawFlags
        {
            get => 
                this._drawFlags;
            set
            {
                this._drawFlags = value;
            }
        }

        internal bool FullCrop { get; set; }

        public int Height
        {
            get => 
                ((int) this.Size.Y);
            set
            {
                if (value != this.Height)
                {
                    this.Size = new Vector2(this.Size.X, (float) value);
                }
            }
        }

        internal bool IsDrawing
        {
            get => 
                EloBuddy.SDK.Rendering.Sprite.IsDrawing;
            set
            {
                EloBuddy.SDK.Rendering.Sprite.IsDrawing = value;
            }
        }

        public Vector2 Padding
        {
            get => 
                this._padding;
            set
            {
                if (this._padding != value)
                {
                    this._padding = value;
                    this.RecalculatePositionRectangle();
                }
            }
        }

        public Vector2 Position
        {
            get => 
                this._position;
            set
            {
                if (this._position != value)
                {
                    this._position = value;
                    this.RecalculatePositionRectangle();
                }
            }
        }

        public SharpDX.Rectangle PositionRectangle { get; internal set; }

        public Vector2 Size
        {
            get => 
                this._size;
            set
            {
                this._size = value;
                this.RecalculateBoundingAndDisplayedText();
            }
        }

        internal SharpDX.Direct3D9.Sprite SpriteHandle =>
            EloBuddy.SDK.Rendering.Sprite.Handle;

        public Align TextAlign { get; set; }

        internal SharpDX.Direct3D9.Font TextHandle { get; set; }

        public Orientation TextOrientation { get; set; }

        public string TextValue
        {
            get => 
                this._textValue;
            set
            {
                if (this._textValue != value)
                {
                    this._textValue = value;
                    this.RecalculateBoundingAndDisplayedText();
                }
            }
        }

        public int Width
        {
            get => 
                ((int) this.Size.X);
            set
            {
                if (value != this.Width)
                {
                    this.Size = new Vector2((float) value, this.Size.Y);
                }
            }
        }

        public int X
        {
            get => 
                ((int) this.Position.X);
            set
            {
                this.Position = new Vector2((float) value, this.Position.Y);
            }
        }

        public int Y
        {
            get => 
                ((int) this.Position.Y);
            set
            {
                this.Position = new Vector2(this.Position.X, (float) value);
            }
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Text.<>c <>9 = new Text.<>c();
            public static Func<FontFamily, string> <>9__9_0;

            internal IEnumerable<string> <.cctor>b__10_0(string file) => 
                Text.ValidFontEndings.Where<string>(new Func<string, bool>(file.EndsWith));

            internal string <.cctor>b__10_1(string file, string ending) => 
                file;

            internal string <get_CustomFonts>b__9_0(FontFamily o) => 
                o.Name;
        }

        public enum Align
        {
            Left,
            Center,
            Right
        }

        public enum Orientation
        {
            Center,
            Top,
            Bottom
        }
    }
}

