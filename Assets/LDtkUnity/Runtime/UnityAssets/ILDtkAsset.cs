using UnityEngine;

namespace LDtkUnity.UnityAssets
{
    public interface ILDtkAsset
    {
        string Identifier { get; }
        string AssetTypeName { get; }
        bool AssetExists { get; }
        Object Object { get; } 
    }
}