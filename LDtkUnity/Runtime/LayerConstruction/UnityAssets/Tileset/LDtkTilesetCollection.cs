using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Tileset
{
    [CreateAssetMenu(fileName = nameof(LDtkTilesetCollection), menuName = LDtkTool.SCRIPTABLE_OBJECT_PATH + nameof(LDtkTilesetCollection), order = 0)]
    public class LDtkTilesetCollection : LDtkAssetCollection<LDtkTileset> {}
}