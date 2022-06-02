using UnityEditor;

namespace LDtkUnity.Editor
{
    public static class ExportNativePrefab
    {
        [MenuItem("LDtkUnity/Export Native Prefab")]
        private static void CreateWindow()
        {
            LDtkNativeExportWindow.CreateWindowWithContext(null);
        }
    }
}