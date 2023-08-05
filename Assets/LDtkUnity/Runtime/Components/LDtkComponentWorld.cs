using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    /// <summary>
    /// This component can be used to get certain LDtk information of a world.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_WORLD)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentWorld : MonoBehaviour
    {
        [ExcludeFromDocs] public const string PROPERTY_IDENTIFIER = nameof(_identifier);
        [ExcludeFromDocs] public const string PROPERTY_WORLD_GRID_SIZE = nameof(_worldGridSize);
        [ExcludeFromDocs] public const string PROPERTY_WORLD_LAYOUT = nameof(_worldLayout);
        
        [SerializeField] internal string _identifier;
        [SerializeField] internal Vector2Int _worldGridSize;
        [SerializeField] internal WorldLayout _worldLayout;
        
        /// <value>
        /// User defined unique identifier
        /// </value>
        [PublicAPI] public string Identifier => _identifier;        
        
        /// <value>
        /// Height of the world grid in pixels.
        /// </value>
        [PublicAPI] public Vector2Int WorldGridSize => _worldGridSize;
            
        /// <value>
        /// An enum that describes how levels are organized in this project (ie. linearly or in a 2D
        /// space). Possible values: `Free`, `GridVania`, `LinearHorizontal`, `LinearVertical`, `null`
        /// </value>
        [PublicAPI] public WorldLayout WorldLayout => _worldLayout;
        
        internal void Setup(World world)
        {
            _identifier = world.Identifier;
            _worldGridSize = world.UnityWorldGridSize;
            _worldLayout = world.WorldLayout.GetValueOrDefault();
        }
    }
}