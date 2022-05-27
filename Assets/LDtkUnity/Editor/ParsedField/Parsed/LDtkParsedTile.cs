using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal class LDtkParsedTile : ILDtkValueParser
    {
        private static LDtkProjectImporter _importer;
        
        public static void CacheRecentImporter(LDtkProjectImporter lDtkProjectImporter)
        {
            _importer = lDtkProjectImporter;
        }
        
        bool ILDtkValueParser.TypeName(FieldInstance instance)
        {
            return instance.IsTile;
        }

        public object ImportString(object input)
        {
            //input begins as a string in json format
            //example of a tile instance:
            //{ "tilesetUid": 104, "srcRect": [144,128,16,16] },
            //or
            //null
            
            if (input == null)
            {
                return default;
            }
            string inputString = input.ToString();
            
            TilesetRectangle tile = GetTilesetRectOfValue(inputString);
            if (tile == null)
            {
                Debug.LogError($"LDtk: Tile was null after trying to deserialize");
                return default;
            }
            
            if (_importer == null)
            {
                Debug.LogError("LDtk: Couldn't parse point, importer was null");
                return default;
            }
            
            Sprite sprite = _importer.GetSprite(tile);
            return sprite;
        }
        
        public static TilesetRectangle GetTilesetRectOfValue(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                //a tile can safely be null
                return default;
            }
            
            TilesetRectangle tile = null;
            try
            {
                tile = TilesetRectangle.FromJson(inputString);
            }
            catch (Exception e)
            {
                Debug.LogError($"LDtk: Json FromJson error for Parsed tile:\n{inputString}\n{e}");
                return default;
            }
            return tile;
        }
        
    }
}