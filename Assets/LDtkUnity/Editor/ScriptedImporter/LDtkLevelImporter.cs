using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    [HelpURL(LDtkHelpURL.JSON_LEVEL)]
    [ScriptedImporter(LDtkImporterConsts.LEVEL_VERSION, LDtkImporterConsts.LEVEL_EXT, LDtkImporterConsts.LEVEL_ORDER)]
    public class LDtkLevelImporter : LDtkJsonImporter<LDtkLevelFile>
    {
        public GameObject GetProjectAsset()
        {
            LDtkRelativeGetterProject getter = new LDtkRelativeGetterProject();
            return getter.GetRelativeAsset(this, assetPath);
        }
        
        protected override void Import()
        {
            GameObject projectAsset = GetProjectAsset();
            if (projectAsset == null)
            {
                Debug.LogError("LDtk: Null loaded Project");
                return;
            }

            IEnumerable<Transform> children = GetChildren(projectAsset);

            LDtkLevelFile levelFile = AddLevelFile();
            Level level = levelFile.FromJson;
            
            GameObject[] subAssets = children.Select(p => p.gameObject).ToArray();
            Object projectLevel = subAssets.FirstOrDefault(p => p != null && p.name == level.Identifier);
            if (projectLevel == null)
            {
                Debug.LogError($"LDtk: Issue locating the level in the project file for \"{projectAsset}\"");
                return;
            }

            
            //make copy of the level object
            GameObject newLevelObj = (GameObject)Instantiate(projectLevel);
            
            ImportContext.AddObjectToAsset("levelRoot", newLevelObj, LDtkIconUtility.LoadLevelFileIcon());
            ImportContext.SetMainObject(newLevelObj);
            
            //depend on the project, in case the project changes.
            SetupAssetDependency(projectAsset);
        }

        private LDtkLevelFile AddLevelFile()
        {
            LDtkLevelFile levelFile = ReadAssetText();
            ImportContext.AddObjectToAsset("levelFile", levelFile, LDtkIconUtility.LoadLevelIcon());
            return levelFile;
        }

        private static IEnumerable<Transform> GetChildren(GameObject obj)
        {
            Transform[] children = new Transform[obj.transform.childCount];
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                children[i] = obj.transform.GetChild(i);
            }

            return children;
        }
    }
}