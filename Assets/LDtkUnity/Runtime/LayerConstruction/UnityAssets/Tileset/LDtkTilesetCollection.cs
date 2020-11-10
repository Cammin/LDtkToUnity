using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Tileset
{
    [CreateAssetMenu(fileName = nameof(LDtkTilesetCollection), menuName = LDtkSoTool.SO_PATH + nameof(LDtkTilesetCollection), order = LDtkSoTool.SO_ORDER)]
    public class LDtkTilesetCollection : LDtkAssetCollection<LDtkTileset> {}
}