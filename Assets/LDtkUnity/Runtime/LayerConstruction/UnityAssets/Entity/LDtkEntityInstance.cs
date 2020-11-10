using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Entity
{
    [CreateAssetMenu(fileName = nameof(LDtkEntityInstance), menuName = LDtkSoTool.SO_PATH + nameof(LDtkEntityInstance), order = LDtkSoTool.SO_ORDER)]
    public class LDtkEntityInstance : LDtkAsset<GameObject> {}
}