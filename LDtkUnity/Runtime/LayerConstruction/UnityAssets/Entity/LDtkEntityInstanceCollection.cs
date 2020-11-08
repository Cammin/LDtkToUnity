using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.UnityAssets.Entity
{
    [CreateAssetMenu(fileName = nameof(LDtkEntityInstanceCollection), menuName = LDtkTool.SCRIPTABLE_OBJECT_PATH + nameof(LDtkEntityInstanceCollection), order = 0)]
    public class LDtkEntityInstanceCollection : LDtkAssetCollection<LDtkEntityInstance> { }
}