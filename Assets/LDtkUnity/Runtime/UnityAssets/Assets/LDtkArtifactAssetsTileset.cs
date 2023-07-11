using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    public sealed class LDtkArtifactAssetsTileset : ScriptableObject
    {
        internal const string PROPERTY_SPRITE_LIST = nameof(_sprites);
        internal const string PROPERTY_TILE_LIST = nameof(_tiles);
        
        [SerializeField] internal List<Sprite> _sprites = new List<Sprite>();
        [SerializeField] internal List<TileBase> _tiles = new List<TileBase>();
        
        // This class doesn't contain malformed shapes.
        // There isn't an easy way to index them in an optimized way when it comes to serialization 

        /// <summary>
        /// Indexed by tile id
        /// </summary>
        public IReadOnlyList<Sprite> Sprites => _sprites;

        /// <summary>
        /// Indexed by tile id
        /// </summary>
        public IReadOnlyList<TileBase> Tiles => _tiles;
    }
}