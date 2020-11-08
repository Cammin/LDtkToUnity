using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets
{
    [CreateAssetMenu(fileName = nameof(LDtkLevelIdentifier), menuName = LDtkSOTool.SO_PATH + nameof(LDtkLevelIdentifier), order = LDtkSOTool.SO_ORDER)]
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