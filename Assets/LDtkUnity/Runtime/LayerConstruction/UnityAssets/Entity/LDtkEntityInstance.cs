using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Entity
{
    [CreateAssetMenu(fileName = nameof(LDtkEntityInstance), menuName = LDtkSOTool.SO_PATH + nameof(LDtkEntityInstance), order = LDtkSOTool.SO_ORDER)]
    public class LDtkEntityInstance : LDtkAsset<GameObject> {}
}