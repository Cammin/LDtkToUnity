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
            return LDtkIconUtility.GetRenderStaticPreview(StaticPreview, width, height);
        }
    }
}