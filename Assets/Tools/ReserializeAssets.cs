using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace LDtkUnity.Editor
{
    internal static class ReserializeAssets
    {
        [MenuItem("LDtkUnity/Reserialize Assets", false, 10)]
        private static void UpdateSamples()
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}
