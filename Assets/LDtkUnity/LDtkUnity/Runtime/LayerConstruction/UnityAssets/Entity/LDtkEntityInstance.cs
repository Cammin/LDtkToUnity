using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Entity
{
    [CreateAssetMenu(fileName = nameof(LDtkEntityInstance), menuName = LDtkTool.SCRIPTABLE_OBJECT_PATH + nameof(LDtkEntityInstance), order = 0)]
    public class LDtkEntityInstance : LDtkAsset<GameObject> {}
}