using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets.Assets
{
    [HelpURL(LDtkHelpURL.INT_GRID_VALUE_ASSET)]
    [CreateAssetMenu(fileName = nameof(LDtkIntGridValueAsset),
        menuName = LDtkToolScriptableObj.SO_PATH + "Int Grid Value",
        order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkIntGridValueAsset : LDtkAsset<Sprite>
    {
    }
}