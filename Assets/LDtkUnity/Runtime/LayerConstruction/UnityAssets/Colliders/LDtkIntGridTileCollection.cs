using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Colliders
{
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTileCollection), menuName = LDtkSOTool.SO_PATH + nameof(LDtkIntGridTileCollection), order = LDtkSOTool.SO_ORDER)]
    public class LDtkIntGridTileCollection : LDtkAssetCollection<LDtkIntGridTile> {}
}