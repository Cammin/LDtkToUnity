using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Colliders
{
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTileCollection), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkIntGridTileCollection), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkIntGridTileCollection : LDtkAssetCollection<LDtkIntGridTile> {}
}