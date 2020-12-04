using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Assets
{
    [HelpURL(LDtkHelpURL.LEVEL_IDENTIFIER)]
    [CreateAssetMenu(fileName = nameof(LDtkLevelIdentifier), menuName = LDtkToolScriptableObj.SO_PATH + "Level Identifier", order = LDtkToolScriptableObj.SO_ORDER)]
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