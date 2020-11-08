using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Tileset
{
    [CreateAssetMenu(fileName = nameof(LDtkTileset), menuName = LDtkTool.SCRIPTABLE_OBJECT_PATH + nameof(LDtkTileset), order = 0)]
    public class LDtkTileset : LDtkAsset<Sprite> {}
}