using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    [Serializable]
    public class LDtkAssetEntity : LDtkAsset<GameObject>
    {
        public LDtkAssetEntity(string key, GameObject asset) : base(key, asset)
        {
        }
    }
}