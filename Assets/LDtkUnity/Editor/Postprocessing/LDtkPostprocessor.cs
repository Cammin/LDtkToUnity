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
        internal const string METHOD_BACKGROUND = nameof(OnPostProcessBackground);
        internal const string METHOD_ENTITY = nameof(OnPostProcessEntity);
        internal const string METHOD_INT_GRID_LAYER = nameof(OnPostProcessIntGridLayer);
        internal const string METHOD_AUTO_LAYER = nameof(OnPostProcessAutoLayer);
        
        /// <summary>
        /// todo add documentation for this
        /// </summary>
        protected void OnPostProcessProject(GameObject projectObj, LdtkJson project) { }
        protected void OnPostProcessLevel(GameObject levelObj, Level level) { }
        protected void OnPostProcessBackground(GameObject backgroundObj) { }
        protected void OnPostProcessEntity(GameObject entityObj, EntityInstance entity) { }
        
        protected void OnPostProcessIntGridLayer(GameObject layerObj, LayerInstance layer, Tilemap[] tilemaps) { }
        protected void OnPostProcessAutoLayer(GameObject layerObj, LayerInstance layer, Tilemap[] tilemaps) { }
    }
}