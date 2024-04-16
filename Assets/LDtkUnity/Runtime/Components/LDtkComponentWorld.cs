using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LDtkUnity
{
    /// <summary>
    /// This component can be used to get certain LDtk information of a world.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_WORLD)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentWorld : MonoBehaviour
    {
        #region Custom
        
        [field: Tooltip("This world's project")]
        [field: SerializeField] public LDtkComponentProject Parent { get; private set; }
        
        #endregion
        
        [field: Space]
        [field: Tooltip("User defined unique identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: Tooltip("Unique instance identifier")]
        [field: SerializeField] public LDtkIid Iid { get; private set; }
        
        [field: Tooltip("All levels from this world. The order of this array is only relevant in `LinearHorizontal` and `linearVertical` world layouts (see `worldLayout` value). Otherwise, you should refer to the `worldX`,`worldY` coordinates of each Level.")]
        [field: SerializeField] public LDtkComponentLevel[] Levels { get; private set; }
        
        [field: Tooltip("Only 'GridVania' layouts. Size of the world grid in pixels.")]
        [field: SerializeField] public Vector2Int WorldGridSize { get; private set; }
        
        [field: Tooltip("An enum that describes how levels are organized in this project (ie. linearly or in a 2D space). Possible values: `Free`, `GridVania`, `LinearHorizontal`, `LinearVertical`, `null`")]
        [field: SerializeField] public WorldLayout WorldLayout { get; private set; }
        
        #region Internal
        
        [field: Header("Internal")]
        [field: Tooltip("Default new level size")]
        [field: SerializeField] public Vector2Int DefaultLevelSize { get; private set; }

        #endregion
        
        internal void OnImport(World world, LDtkComponentLevel[] levels, LDtkComponentProject parent, LDtkIid iid)
        {
            Identifier = world.Identifier;
            Iid = iid;
            Levels = levels;
            WorldGridSize = world.UnityWorldGridSize;
            WorldLayout = world.WorldLayout.GetValueOrDefault();
            DefaultLevelSize = world.UnityDefaultLevelSize;
            
            //custom
            Parent = parent;
        }
    }
}