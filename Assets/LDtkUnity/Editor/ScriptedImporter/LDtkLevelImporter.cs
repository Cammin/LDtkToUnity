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
            
            Transform projectLevel = GetLevelFromProject(projectAsset, level.Identifier);
            if (projectLevel == null)
            {
                Debug.LogError($"LDtk: Issue loading the level \"{level.Identifier}\" in the project file \"{projectAsset.name}\"", projectAsset);
                return;
            }
            
            GameObject newLevelObj = Instantiate(projectLevel.gameObject);

            ImportContext.AddObjectToAsset("levelRoot", newLevelObj, LDtkIconUtility.LoadLevelFileIcon());
            ImportContext.SetMainObject(newLevelObj);
        }
        
        public GameObject GetProjectAsset()
        {
            LDtkRelativeGetterProject getter = new LDtkRelativeGetterProject();
            return getter.GetRelativeAsset(this, assetPath);
        }
        
        private static Transform GetLevelFromProject(GameObject projectAsset, string levelIdentifier)
        {
            Transform[] worlds = GetChildren(projectAsset.transform);//.Cast<Transform>().ToArray();
            if (worlds.IsNullOrEmpty())
            {
                Debug.LogWarning($"LDtk: There are no worlds in the project \"{projectAsset.name}\"");
                return null;
            }
            
            Transform[] levels = worlds.SelectMany(GetChildren).ToArray();
            if (levels.IsNullOrEmpty())
            {
                Debug.LogWarning($"LDtk: There are no levels in the project \"{projectAsset.name}\"");
                return null;
            }

            Transform projectLevel = levels.FirstOrDefault(p => p != null && p.name == levelIdentifier);
            return projectLevel;
        }

        private LDtkLevelFile AddLevelFile()
        {
            LDtkLevelFile levelFile = ReadAssetText();
            ImportContext.AddObjectToAsset("levelFile", levelFile, LDtkIconUtility.LoadLevelIcon());
            return levelFile;
        }

        private static Transform[] GetChildren(Transform transform)
        {
            return transform.Cast<Transform>().ToArray();
        }
    }
}