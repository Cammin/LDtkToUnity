using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkParsedTile : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance)
        {
            return instance.IsTile;
        }

        public object ImportString(object input)
        {
            FieldInstanceTile tile = (FieldInstanceTile)input;

            if (tile == null)
            {
                Debug.LogError("LDtk: parse error for tile");
                return null;
            }

            Sprite tileSprite = null;

            
            //todo do something to get from here with the casted tile data. 
            
            return tileSprite;
        }
    }
}