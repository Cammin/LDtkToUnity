using UnityEditor;

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
