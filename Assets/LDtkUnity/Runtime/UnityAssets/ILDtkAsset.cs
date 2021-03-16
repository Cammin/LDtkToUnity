using UnityEngine;

namespace LDtkUnity
{
    public interface ILDtkAsset
    {
        string Identifier { get; }
        string AssetTypeName { get; }
        bool AssetExists { get; }
        Object Object { get; } 
    }
}