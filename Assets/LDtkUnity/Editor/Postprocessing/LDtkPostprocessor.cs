using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Inherit from this class to 
    /// </summary>
    
    public abstract class LDtkPostprocessor
    {
        /// <summary>
        /// todo add documentation for this
        /// </summary>
        protected void OnPostProcessLDtkProject(GameObject projectObj, LdtkJson project) { }
        protected void OnPostProcessLDtkLevel(GameObject levelObj, Level level) { }
        protected void OnPostProcessLDtkBackground(GameObject backgroundObj) { }
        protected void OnPostProcessLDtkEntity(GameObject entityObj, EntityInstance entity) { }
        
        protected void OnPostProcessLDtkIntGridLayer(GameObject layerObj, LayerInstance layer, Tilemap[] tilemaps) { }
        protected void OnPostProcessLDtkAutoLayer(GameObject layerObj, LayerInstance layer, Tilemap[] tilemaps) { }
    }
}