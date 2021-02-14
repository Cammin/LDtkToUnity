using System;
using LDtkUnity.Tools;
using UnityEngine;

namespace LDtkUnity.UnityAssets
{
    [Serializable]
    public class LDtkEntityAsset : LDtkAsset<GameObject>
    {
        public LDtkEntityAsset(string key, GameObject asset) : base(key, asset)
        {
        }
    }
}