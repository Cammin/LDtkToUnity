using System;

namespace LDtkUnity
{
    [Serializable]
    public class LDtkAssetIntGridValue : LDtkAsset<LDtkIntGridTile>
    {
        public LDtkAssetIntGridValue(string key, LDtkIntGridTile asset) : base(key, asset)
        {
        }
    }
}