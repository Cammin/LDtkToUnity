using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Warning the user to not nest LDtk projects or levels. Because of a import order and it's relationship with prefabs.
    /// https://trello.com/c/oq3fucyU
    /// https://forum.unity.com/threads/create-additional-prefab-assets-in-scriptedimporter-and-track-them.1158734/#post-7792380
    /// https://github.com/Seanba/SuperTiled2Unity/issues/144#issuecomment-1011981650 
    /// </summary>
    internal class LDtkBrokenPrefabDetector : AssetPostprocessor
    {
        private void OnPostprocessPrefab(GameObject root)
        {
            if (!root.GetComponentsInChildren<LDtkComponentProject>().IsNullOrEmpty())
            {
                LogIssue("project");
                return;
            }
            
            if (!root.GetComponentsInChildren<LDtkComponentLevel>().IsNullOrEmpty())
            {
                LogIssue("level");
            }
        }

        private void LogIssue(string type)
        {
            Debug.LogWarning($"LDtk: There is a LDtk {type} nested inside of a prefab! This is currently not encouraged, and may result in recurring import bugs.\nIn path: {assetPath}");
        }
    }
}