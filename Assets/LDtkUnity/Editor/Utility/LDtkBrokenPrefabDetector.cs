using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    //todo will remove this in the future, as this is now fixed!
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
            //setup a dependency on ldtk projects if they are contained in here!

            //LDtkComponentProjects<LDtkComponentProject>(root);
            //LDtkComponentProjects<LDtkComponentLevel>(root);
            
            
            /*if (!root.GetComponentsInChildren<LDtkComponentLevel>().IsNullOrEmpty())
            {
                LogIssue("level");
            }*/
        }

        private void LDtkComponentProjects<T>(GameObject root) where T : Component
        {
            /*T[] projectCompoennts = root.GetComponentsInChildren<T>();
            
            
            
            foreach (T projects in projectCompoennts)
            {
                T correspondingObjectFromSource = PrefabUtility.GetCorrespondingObjectFromSource(projects);

                string path = AssetDatabase.GetAssetPath(correspondingObjectFromSource);
                Debug.Log(path);
                context.DependsOnArtifact(path);
            }*/
        }

        private void LogIssue(string type)
        {
            Debug.LogWarning($"LDtk: There is a LDtk {type} nested inside of a prefab! This is currently not encouraged, and may result in recurring import bugs.\nIn path: {assetPath}");
        }
    }
}