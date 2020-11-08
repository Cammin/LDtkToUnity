using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Tileset
{
    [CreateAssetMenu(fileName = nameof(LDtkTilesetCollection), menuName = LDtkSOTool.SO_PATH + nameof(LDtkTilesetCollection), order = LDtkSOTool.SO_ORDER)]
    public class LDtkTilesetCollection : LDtkAssetCollection<LDtkTileset> {}
}