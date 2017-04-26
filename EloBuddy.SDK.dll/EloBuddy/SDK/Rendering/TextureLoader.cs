namespace EloBuddy.SDK.Rendering
{
    using EloBuddy;
    using SharpDX.Direct3D9;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class TextureLoader : IDisposable
    {
        internal readonly Dictionary<string, Tuple<Bitmap, Texture>> Textures = new Dictionary<string, Tuple<Bitmap, Texture>>();

        public TextureLoader()
        {
            Drawing.OnPostReset += new DrawingPostReset(this.OnReset);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(this.OnAppDomainUnload);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(this.OnAppDomainUnload);
        }

        public static Texture BitmapToTexture(Bitmap bitmap) => 
            Texture.FromMemory(Drawing.Direct3DDevice, (byte[]) new ImageConverter().ConvertTo(bitmap, typeof(byte[])), bitmap.Width, bitmap.Height, 0, Usage.None, Format.A1, Pool.Managed, -1, -1, 0);

        public void Dispose()
        {
            foreach (Tuple<Bitmap, Texture> tuple in this.Textures.Values)
            {
                tuple.Item1.Dispose();
                tuple.Item2.Dispose();
            }
            this.Textures.Clear();
            AppDomain.CurrentDomain.DomainUnload -= new EventHandler(this.OnAppDomainUnload);
            AppDomain.CurrentDomain.ProcessExit -= new EventHandler(this.OnAppDomainUnload);
        }

        public Texture Load(Bitmap bitmap, out string uniqueKey)
        {
            string str;
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }
            do
            {
                str = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            }
            while (this.Textures.ContainsKey(str));
            uniqueKey = str;
            return this.Load(str, bitmap);
        }

        public Texture Load(string key, Bitmap bitmap)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            if (this.Textures.ContainsKey(key))
            {
                throw new ArgumentException($"The given key '{key}' is already present!");
            }
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }
            this.Textures[key] = new Tuple<Bitmap, Texture>(bitmap, BitmapToTexture(bitmap));
            return this.Textures[key].Item2;
        }

        internal void OnAppDomainUnload(object sender, EventArgs eventArgs)
        {
            this.Dispose();
        }

        internal void OnReset(EventArgs args)
        {
            foreach (KeyValuePair<string, Tuple<Bitmap, Texture>> pair in this.Textures.ToList<KeyValuePair<string, Tuple<Bitmap, Texture>>>())
            {
                pair.Value.Item2.Dispose();
                this.Textures[pair.Key] = new Tuple<Bitmap, Texture>(pair.Value.Item1, BitmapToTexture(pair.Value.Item1));
            }
        }

        public bool Unload(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            if (this.Textures.ContainsKey(key))
            {
                this.Textures[key].Item2.Dispose();
                this.Textures.Remove(key);
                return true;
            }
            return false;
        }

        public Texture this[string key] =>
            this.Textures[key].Item2;
    }
}

