using System;
using UnityEngine;

namespace LDtkUnity
{
    internal struct TileKeyFormat : IEquatable<TileKeyFormat>
    {
        public string assetName;
        public Rect srcRect;

        public bool Equals(TileKeyFormat other)
        {
            return assetName == other.assetName && srcRect.Equals(other.srcRect);
        }

        public override bool Equals(object obj)
        {
            return obj is TileKeyFormat other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(assetName, srcRect);
        }

        public override string ToString()
        {
            return $"{assetName}_{srcRect.x}_{srcRect.y}_{srcRect.width}_{srcRect.height}";
        }
    }
}