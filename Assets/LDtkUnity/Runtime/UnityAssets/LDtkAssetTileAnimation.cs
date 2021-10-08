using System;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    [Serializable]
    public class LDtkAssetTileAnimation : LDtkAsset<LDtkArtTileAnimation>
    {
        public LDtkAssetTileAnimation(string key, LDtkArtTileAnimation asset) : base(key, asset)
        {
        }
    }
}