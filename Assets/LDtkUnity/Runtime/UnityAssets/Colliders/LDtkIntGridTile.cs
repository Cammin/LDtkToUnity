using LDtkUnity.Runtime.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.UnityAssets.Colliders
{
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTile), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkIntGridTile), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkIntGridTile : LDtkAsset<Tile>{}
}