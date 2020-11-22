using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.IntGridValue
{
    [HelpURL(LDtkHelpURL.INT_GRID_VALUE_ASSET)]
    [CreateAssetMenu(fileName = nameof(LDtkIntGridValueAsset),
        menuName = LDtkToolScriptableObj.SO_PATH + nameof(LDtkIntGridValueAsset),
        order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkIntGridValueAsset : LDtkAsset<Sprite>
    {
    }
}