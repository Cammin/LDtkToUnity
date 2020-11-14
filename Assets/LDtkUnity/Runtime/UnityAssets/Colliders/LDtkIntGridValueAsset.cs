using LDtkUnity.Runtime.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Runtime.UnityAssets.Colliders
{
    [CreateAssetMenu(fileName = nameof(LDtkIntGridValueAsset),
        menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkIntGridValueAsset),
        order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkIntGridValueAsset : LDtkAsset<Sprite>
    {
    }
}