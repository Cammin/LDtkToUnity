using LDtkUnity.Runtime.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.UnityAssets.Colliders
{
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTileAsset), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkIntGridTileAsset), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkIntGridTileAsset : LDtkAsset<Tile>{}
}