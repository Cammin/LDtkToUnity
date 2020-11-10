using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Tileset
{
    [CreateAssetMenu(fileName = nameof(LDtkTileset), menuName = LDtkSoTool.SO_PATH + nameof(LDtkTileset), order = LDtkSoTool.SO_ORDER)]
    public class LDtkTileset : LDtkAsset<Sprite> {}
}