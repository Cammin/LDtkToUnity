using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Tileset
{
    [CreateAssetMenu(fileName = nameof(LDtkTilesetAsset), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkTilesetAsset), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkTilesetAsset : LDtkAsset<Sprite> {}
}