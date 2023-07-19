using System;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    [Serializable]
    internal class LDtkAssetIntGridValue : LDtkAsset<TileBase>
    {
        public LDtkAssetIntGridValue(string key, TileBase asset) : base(key, asset)
        {
        }
    }
}