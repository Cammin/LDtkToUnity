using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.UnityAssets
{
    [HelpURL(LDtkHelpURL.ASSET_ENTITY)]
    [CreateAssetMenu(fileName = nameof(LDtkEntityAsset), menuName = LDtkToolScriptableObj.SO_PATH + "Entity", order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkEntityAsset : LDtkAsset<GameObject> {}
}