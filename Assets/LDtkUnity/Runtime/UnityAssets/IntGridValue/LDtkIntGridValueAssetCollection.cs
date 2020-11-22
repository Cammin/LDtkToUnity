using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.IntGridValue
{
    [HelpURL(LDtkHelpURL.INT_GRID_VALUE_ASSET)]
    [CreateAssetMenu(fileName = nameof(LDtkIntGridValueAssetCollection),
        menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkIntGridValueAssetCollection),
        order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkIntGridValueAssetCollection : LDtkAssetCollection<LDtkIntGridValueAsset>
    {
        [SerializeField] private bool _collisionTilesVisible = false;

        public bool CollisionTilesVisible => _collisionTilesVisible;
    }
}