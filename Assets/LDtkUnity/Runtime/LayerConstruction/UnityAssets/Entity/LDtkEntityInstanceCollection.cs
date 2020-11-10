using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Entity
{
    [CreateAssetMenu(fileName = nameof(LDtkEntityInstanceCollection), menuName = LDtkSoTool.SO_PATH + nameof(LDtkEntityInstanceCollection), order = LDtkSoTool.SO_ORDER)]
    public class LDtkEntityInstanceCollection : LDtkAssetCollection<LDtkEntityInstance> { }
}