using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Inherit from this class to 
    /// </summary>
    
    public abstract class LDtkPostprocessor
    {
        internal const string METHOD_PROJECT = nameof(OnPostProcessProject);
        internal const string METHOD_LEVEL = nameof(OnPostProcessLevel);
        internal const string METHOD_BACKGROUND_COLOR = nameof(OnPostProcessBackgroundColor);
        internal const string METHOD_BACKGROUND_TEXTURE = nameof(OnPostProcessBackgroundTexture);
        internal const string METHOD_ENTITY = nameof(OnPostProcessEntity);
        internal const string METHOD_INT_GRID_LAYER = nameof(OnPostProcessIntGridLayer);
        internal const string METHOD_AUTO_LAYER = nameof(OnPostProcessAutoLayer);
        
        /// <summary>
        /// todo add documentation for this
        /// </summary>
        protected virtual void OnPostProcessProject(GameObject projectObj, LdtkJson project) { }
        protected virtual void OnPostProcessLevel(GameObject levelObj, Level level) { }
        protected virtual void OnPostProcessBackgroundColor(GameObject backgroundObj) { }
        protected virtual void OnPostProcessBackgroundTexture(GameObject backgroundObj) { }
        protected virtual void OnPostProcessEntity(GameObject entityObj, EntityInstance entity) { }
        protected virtual void OnPostProcessIntGridLayer(GameObject layerObj, LayerInstance layer, Tilemap[] tilemaps) { }
        protected virtual void OnPostProcessAutoLayer(GameObject layerObj, LayerInstance layer, Tilemap[] tilemaps) { }

        public virtual int GetPostprocessOrder()
        {
            return 0;
        }
    }
}