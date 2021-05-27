using System;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    [Serializable]
    public class LDtkAssetIntGridValue : LDtkAsset<LDtkIntGridTile>
    {
        public LDtkAssetIntGridValue(string key, LDtkIntGridTile asset) : base(key, asset)
        {
        }
    }
}