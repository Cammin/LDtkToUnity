using System;

namespace LDtkUnity
{
    [Serializable]
    internal class LDtkAssetIntGridValue : LDtkAsset<LDtkIntGridTile>
    {
        public LDtkAssetIntGridValue(string key, LDtkIntGridTile asset) : base(key, asset)
        {
        }
    }
}