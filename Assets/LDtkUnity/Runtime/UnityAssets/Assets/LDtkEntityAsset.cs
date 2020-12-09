using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.UnityAssets
{
    [HelpURL(LDtkHelpURL.ENTITY_ASSET)]
    [CreateAssetMenu(fileName = nameof(LDtkEntityAsset), menuName = LDtkToolScriptableObj.SO_PATH + "Entity", order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkEntityAsset : LDtkAsset<GameObject> {}
}