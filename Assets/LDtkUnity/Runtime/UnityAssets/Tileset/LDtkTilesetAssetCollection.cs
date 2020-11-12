using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Tileset
{
    [CreateAssetMenu(fileName = nameof(LDtkTilesetAssetCollection), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkTilesetAssetCollection), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkTilesetAssetCollection : LDtkAssetCollection<LDtkTilesetAsset> {}
}