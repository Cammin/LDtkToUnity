using System;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable]
    internal class LDtkAssetEntity : LDtkAsset<GameObject>
    {
        public LDtkAssetEntity(string key, GameObject asset) : base(key, asset)
        {
        }
    }
}