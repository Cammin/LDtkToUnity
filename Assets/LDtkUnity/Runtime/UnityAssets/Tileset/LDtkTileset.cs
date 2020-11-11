using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Tileset
{
    [CreateAssetMenu(fileName = nameof(LDtkTileset), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkTileset), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkTileset : LDtkAsset<Sprite> {}
}