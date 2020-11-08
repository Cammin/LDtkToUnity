using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Entity
{
    [CreateAssetMenu(fileName = nameof(LDtkEntityInstanceCollection), menuName = LDtkSOTool.SO_PATH + nameof(LDtkEntityInstanceCollection), order = LDtkSOTool.SO_ORDER)]
    public class LDtkEntityInstanceCollection : LDtkAssetCollection<LDtkEntityInstance> { }
}