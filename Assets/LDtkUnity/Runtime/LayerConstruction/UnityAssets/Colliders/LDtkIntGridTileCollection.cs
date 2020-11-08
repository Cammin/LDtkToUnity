using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Colliders
{
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTileCollection), menuName = LDtkTool.SCRIPTABLE_OBJECT_PATH + nameof(LDtkIntGridTileCollection), order = 0)]
    public class LDtkIntGridTileCollection : LDtkAssetCollection<LDtkIntGridTile> {}
}