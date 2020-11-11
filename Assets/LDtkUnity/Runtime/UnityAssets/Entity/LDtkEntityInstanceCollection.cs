using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Entity
{
    [CreateAssetMenu(fileName = nameof(LDtkEntityInstanceCollection), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkEntityInstanceCollection), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkEntityInstanceCollection : LDtkAssetCollection<LDtkEntityInstance> { }
}