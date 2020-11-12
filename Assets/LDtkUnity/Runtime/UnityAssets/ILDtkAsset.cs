using UnityEngine;

namespace LDtkUnity.Runtime.UnityAssets
{
    public interface ILDtkAsset
    {
        string Identifier { get; }
        string AssetTypeName { get; }
        bool AssetExists { get; }
        Object Object { get; } 
    }
}