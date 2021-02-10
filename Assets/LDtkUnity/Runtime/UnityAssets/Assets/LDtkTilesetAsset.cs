using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.UnityAssets
{
    [HelpURL(LDtkHelpURL.ASSET_TILESET)]
    [CreateAssetMenu(fileName = nameof(LDtkTilesetAsset),
        menuName = LDtkToolScriptableObj.SO_PATH + "Tileset", order = LDtkToolScriptableObj.SO_ORDER)]
    public class LDtkTilesetAsset : LDtkAsset<Texture2D>
    {

    }
}