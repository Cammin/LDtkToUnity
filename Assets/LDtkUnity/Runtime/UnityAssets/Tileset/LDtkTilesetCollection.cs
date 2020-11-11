using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Tileset
{
    [CreateAssetMenu(fileName = nameof(LDtkTilesetCollection), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkTilesetCollection), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkTilesetCollection : LDtkAssetCollection<LDtkTileset> {}
}