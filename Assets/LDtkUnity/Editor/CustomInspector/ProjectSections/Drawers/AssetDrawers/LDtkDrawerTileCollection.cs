using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerTileCollection : LDtkAssetDrawer<TilesetDefinition, LDtkTileCollection>
    {
        public LDtkDrawerTileCollection(TilesetDefinition def, SerializedProperty obj, string key) : base(def, obj, key)
        {
        }

        public override bool HasProblem()
        {
            return false;
        }
    }
}