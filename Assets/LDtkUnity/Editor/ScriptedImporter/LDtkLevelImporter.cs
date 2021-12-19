using System.Collections.Generic;
using System.IO;
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
        [SerializeField] private int _value;

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
            Object foundLevel = subAssets.FirstOrDefault(p => p != null && p.name == level.Identifier);
            if (foundLevel == null)
            {
                Debug.LogError($"LDtk: Issue locating the level in the project file for \"{projectAsset}\"");
                return;
            }

            
            
            GameObject levelRoot = (GameObject)PrefabUtility.InstantiateAttachedAsset(foundLevel);
            ImportContext.AddObjectToAsset("levelRoot", levelRoot, LDtkIconUtility.LoadLevelFileIcon());
            ImportContext.SetMainObject(levelRoot);
            
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