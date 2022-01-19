using System;
using UnityEngine;
using UnityEngine.Internal;

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