using System;
using UnityEngine;

namespace LDtkUnity
{
    //todo this should not be an object; instead this could be a LDtkTilesetTile. do that later
    //[Serializable]
    [HelpURL(LDtkHelpURL.LDTK_JSON_TilesetRect)]
    public sealed class LDtkDefinitionObjectTilesetRectangle : ScriptableObject
    {
        /// <summary>
        /// Rect in the tileset image
        /// </summary>
        [field: SerializeField] public RectInt Rectangle { get; private set; }
        
        /// <summary>
        /// The tileset
        /// </summary>
        [field: SerializeField] public LDtkDefinitionObjectTileset Tileset { get; private set; }

        
        //public LDtkTilesetTile Tile; //todo eventually add a reference to the tile. 

        
        public void Populate(LDtkDefinitionObjectsCache cache, TilesetRectangle def)
        {
            name = def.ToString();
            Tileset = cache.GetObject(cache.Tilesets, def.TilesetUid);
            Rectangle = def.UnityRectInt;
        }
    }
}