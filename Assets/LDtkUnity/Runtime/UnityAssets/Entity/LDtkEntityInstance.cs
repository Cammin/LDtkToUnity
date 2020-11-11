using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Entity
{
    [CreateAssetMenu(fileName = nameof(LDtkEntityInstance), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkEntityInstance), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkEntityInstance : LDtkAsset<GameObject> {}
}