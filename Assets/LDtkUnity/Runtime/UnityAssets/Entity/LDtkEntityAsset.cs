using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Entity
{
    [HelpURL(LDtkHelpURL.ENTITY_ASSET)]
    [CreateAssetMenu(fileName = nameof(LDtkEntityAsset), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkEntityAsset), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkEntityAsset : LDtkAsset<GameObject> {}
}