using System;
using UnityEngine;

namespace LDtkUnity
{
    //todo this should not be an object; instead this could be a LDtkTilesetTile. do that later
    //[Serializable]
    [HelpURL(LDtkHelpURL.LDTK_JSON_TilesetRect)]
    public sealed class LDtkDefinitionObjectTilesetRectangle : ScriptableObject
    {
        [field: Tooltip("Rect in the tileset image")]
        [field: SerializeField] public RectInt Rectangle { get; private set; }
        
        [field: Tooltip("The tileset")]
        [field: SerializeField] public LDtkDefinitionObjectTileset Tileset { get; private set; }

        
        //public LDtkTilesetTile Tile; //todo eventually add a reference to the actual tile that's in the tileset definition file. maybe this object doesnt need to exist at all. 
        
        public void Populate(LDtkDefinitionObjectsCache cache, TilesetRectangle def)
        {
            name = def.ToString();
            Tileset = cache.GetObject(cache.Tilesets, def.TilesetUid);
            Rectangle = def.UnityRectInt;
        }
    }
}