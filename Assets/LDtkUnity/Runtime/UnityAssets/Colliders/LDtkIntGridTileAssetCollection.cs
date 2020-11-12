using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Colliders
{
    [CreateAssetMenu(fileName = nameof(LDtkIntGridTileAssetCollection),
        menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkIntGridTileAssetCollection),
        order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkIntGridTileAssetCollection : LDtkAssetCollection<LDtkIntGridTileAsset>
    {
        [SerializeField] private bool _collisionTilesVisible = true;

        public bool CollisionTilesVisible => _collisionTilesVisible;
    }
}