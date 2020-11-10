using LDtkUnity.Runtime.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Colliders
{
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTile), menuName = LDtkSoTool.SO_PATH + nameof(LDtkIntGridTile), order = LDtkSoTool.SO_ORDER)]
    public class LDtkIntGridTile : LDtkAsset<Tile>{}
}