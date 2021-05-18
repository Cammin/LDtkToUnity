using System;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable]
    public class LDtkAssetEntity : LDtkAsset<GameObject>
    {
        public LDtkAssetEntity(string key, GameObject asset) : base(key, asset)
        {
        }
    }
}