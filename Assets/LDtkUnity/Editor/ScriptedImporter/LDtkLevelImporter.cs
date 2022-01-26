using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.IMPORTER_LDTK_LEVEL)]
    [ScriptedImporter(LDtkImporterConsts.LEVEL_VERSION, LDtkImporterConsts.LEVEL_EXT, LDtkImporterConsts.LEVEL_ORDER)]
    internal class LDtkLevelImporter : LDtkJsonImporter<LDtkLevelFile>
    {
        protected override void Import()
        {
            GameObject projectAsset = GetProjectAsset();
            if (projectAsset == null)
            {
                Debug.LogError("LDtk: A level was trying to import, but it's source project wasn't able to be loaded! Make sure the level is correctly imported is is a .ldtk file", this);
                return;
            }
            
            //depend on the project, in case the project changes.
            SetupAssetDependency(projectAsset);
            
            LDtkLevelFile levelFile = AddLevelFile();
            Level level = levelFile.FromJson;
            
            Object projectLevel = GetLevelFromProject(projectAsset, level.Identifier);
            if (projectLevel == null)
            {
                Debug.LogError($"LDtk: Issue locating the level in the project file for \"{projectAsset}\"");
                return;
            }
            
            GameObject newLevelObj = (GameObject)Instantiate(projectLevel);

            ImportContext.AddObjectToAsset("levelRoot", newLevelObj, LDtkIconUtility.LoadLevelFileIcon());
            ImportContext.SetMainObject(newLevelObj);
        }
        
        public GameObject GetProjectAsset()
        {
            LDtkRelativeGetterProject getter = new LDtkRelativeGetterProject();
            return getter.GetRelativeAsset(this, assetPath);
        }
        
        private static Object GetLevelFromProject(GameObject projectAsset, string levelIdentifier)
        {
            IEnumerable<Transform> children = GetChildren(projectAsset);
            GameObject[] subAssets = children.Select(p => p.gameObject).ToArray();
            Object projectLevel = subAssets.FirstOrDefault(p => p != null && p.name == levelIdentifier);
            return projectLevel;
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