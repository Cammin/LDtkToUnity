using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Colliders
{
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTileCollection), menuName = LDtkSoTool.SO_PATH + nameof(LDtkIntGridTileCollection), order = LDtkSoTool.SO_ORDER)]
    public class LDtkIntGridTileCollection : LDtkAssetCollection<LDtkIntGridTile> {}
}