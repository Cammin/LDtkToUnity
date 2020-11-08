using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets
{
    [CreateAssetMenu(fileName = nameof(LDtkLevelIdentifier), menuName = LDtkTool.SCRIPTABLE_OBJECT_PATH + nameof(LDtkLevelIdentifier), order = 0)]
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