using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal sealed class LDtkParsedTile : ILDtkValueParser
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

            TilesetRectangle tile = ConvertDict(input);
            if (tile == null)
            {
                //a tile can safely be null
                return default;
            }
            
            if (_importer == null)
            {
                LDtkDebug.LogError("Couldn't parse point, importer was null");
                return default;
            }
            
            Sprite sprite = _importer.GetSpriteArtifact(tile.Tileset, tile.UnityRect);
            return sprite;
        }
        
        public static TilesetRectangle ConvertDict(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is Dictionary<string, object> dict)
            {
                double tilesetUid = (double)dict["tilesetUid"];
                double x = (double)dict["x"];
                double y = (double)dict["y"];
                double w = (double)dict["w"];
                double h = (double)dict["h"];
                return new TilesetRectangle
                {
                    TilesetUid = (int)tilesetUid,
                    X = (int)x,
                    Y = (int)y,
                    W = (int)w,
                    H = (int)h
                };
            }

            Debug.LogError("Issue parsing tile");
            return null;
        }
    }
}