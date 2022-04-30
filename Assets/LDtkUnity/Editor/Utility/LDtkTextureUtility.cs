using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkTextureUtility
    {
        public static Sprite CreateSprite(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit)
        {
            //fullRect for performance reasons on import speed https://forum.unity.com/threads/any-way-to-speed-up-sprite-create.529525/
            return Sprite.Create(texture, rect, pivot, pixelsPerUnit, 0U, SpriteMeshType.FullRect); 
        }
        
        public static Color[] TintPixels(Texture2D srcTex, Color tint)
        {
            Color[] pixels = srcTex.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                Color color = pixels[i];
                Color newColor = new Color(tint.r, tint.g, tint.b, color.a);
                pixels[i] = newColor;
            }

            return pixels;
        }
        
        //from https://stackoverflow.com/questions/56949217/how-to-resize-a-texture2d-using-height-and-width
        public static Texture2D ResizeStretchAsCopy(Texture2D source, int newWidth, int newHeight)
        {
            source.filterMode = FilterMode.Point;
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;
            Graphics.Blit(source, rt);
            Texture2D nTex = new Texture2D(newWidth, newHeight);
            nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0,0);
            nTex.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
            return nTex;
        }

        public static void Resize(Texture2D tex, int width, int height)
        {
#if UNITY_2021_2_OR_NEWER
            tex.Reinitialize(width, height);
#else
            tex.Resize(width, height);
#endif
        }
        
        public static void ResizeStretch(Texture2D source, int newWidth, int newHeight, FilterMode filterMode = FilterMode.Trilinear)
        {
            source.filterMode = filterMode;
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            rt.filterMode = filterMode;
            RenderTexture.active = rt;
            Graphics.Blit(source, rt);
            Resize(source, newWidth, newHeight);
            source.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0,0);
            source.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
        }

        public static Texture2D CreateSlice(Texture2D src, RectInt slice)
        {
            Texture2D dest = new Texture2D(slice.width, slice.height);
            
            Graphics.CopyTexture(src, 0, 0, slice.x, slice.y, slice.width, slice.height, dest, 0, 0, 0, 0);

            return dest;
        }
    }
}