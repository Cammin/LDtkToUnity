using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets
{
    [HelpURL(LDtkHelpURL.LEVEL_IDENTIFIER)]
    [CreateAssetMenu(fileName = nameof(LDtkLevelIdentifier), menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkLevelIdentifier), order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkLevelIdentifier : ScriptableObject
    {
        public static implicit operator string(LDtkLevelIdentifier assoc)
        {
            if (assoc == null)
            {
                Debug.LogError("LDtkLevelIdentifier null");
            }
            return assoc.name;
        }
    }
}