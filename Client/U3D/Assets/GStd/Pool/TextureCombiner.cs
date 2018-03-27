namespace GStd
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public sealed class TextureCombiner : Singleton<TextureCombiner>
    {
        private Dictionary<List<string>, Struct10> dictionary_0 = new Dictionary<List<string>, Struct10>(ListComparer<string>.Default);
        private List<string> list_0 = new List<string>();

        public void Combine(Texture2D[] textures, out Texture2D atlas, out Rect[] rects)
        {
            Struct10 struct2;
            foreach (Texture2D textured in textures)
            {
                this.list_0.Add(textured.name);
            }
            this.list_0.Sort();
            if (this.dictionary_0.TryGetValue(this.list_0, out struct2))
            {
                if (struct2.weakReference_0.IsAlive)
                {
                    atlas = (Texture2D) struct2.weakReference_0.Target;
                    rects = struct2.rect_0;
                    this.list_0.Clear();
                    return;
                }
                this.dictionary_0.Remove(this.list_0);
            }
            Texture2D textured2 = textures[0];
            TextureFormat format = textured2.format;
            bool mipmap = textured2.mipmapCount > 0;
            atlas = new Texture2D(0, 0, format, mipmap);
            atlas.name = "Combined Atlas";
            atlas.hideFlags = HideFlags.DontSave;
            rects = atlas.PackTextures(textures, 0, 0x1000, true);
            if ((atlas.width > 0x800) || (atlas.height > 0x800))
            {
                Debug.LogWarning(string.Concat(new object[] { "The atlas is too large: ", atlas.width, ", ", atlas.height }));
            }
            this.dictionary_0.Add(new List<string>(this.list_0), new Struct10(atlas, rects));
            this.list_0.Clear();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct10
        {
            public WeakReference weakReference_0;
            public Rect[] rect_0;
            public Struct10(Texture2D texture2D_0, Rect[] rect_1)
            {
                this.weakReference_0 = new WeakReference(texture2D_0);
                this.rect_0 = rect_1;
            }
        }
    }
}

