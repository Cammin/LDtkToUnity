using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Base class for any editor instances in this package for assets (not components or scriptedimporters). Contains some universal things that all of them use
    /// </summary>
    internal abstract class LDtkEditor : UnityEditor.Editor
    {
        protected abstract Texture2D StaticPreview { get; }

        //allows some of the editor icons to be for the correct corresponding editor theme, instead of always being white/black 
        public sealed override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            return GetRenderStaticPreview(StaticPreview, width, height);
        }
        
        private static Texture2D GetRenderStaticPreview(Texture2D icon, int width, int height)
        {
            if (icon == null)
            {
                return null;
            }

            //from example https://docs.unity3d.com/ScriptReference/Editor.RenderStaticPreview.html
            // example.PreviewIcon must be a supported format: ARGB32, RGBA32, RGB24, Alpha8 or one of float formats
            
            Texture2D tex = new Texture2D(width, height);
            EditorUtility.CopySerialized(icon, tex);
            return tex;
        }
    }
}