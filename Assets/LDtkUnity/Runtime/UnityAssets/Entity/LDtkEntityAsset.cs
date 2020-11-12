using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Entity
{
    [CreateAssetMenu(fileName = nameof(LDtkEntityAsset), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkEntityAsset), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkEntityAsset : LDtkAsset<GameObject> {}
}