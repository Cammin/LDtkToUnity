using LDtkUnity.Runtime.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Colliders
{
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTile), menuName = LDtkTool.SCRIPTABLE_OBJECT_PATH + nameof(LDtkIntGridTile), order = 0)]
    public class LDtkIntGridTile : LDtkAsset<Tile>{}
}