using UnityEngine;

namespace LDtkUnity
{
    public interface ILDtkAsset
    {
        string Identifier { get; }
        bool AssetExists { get; }
        Object Object { get; } 
    }
}