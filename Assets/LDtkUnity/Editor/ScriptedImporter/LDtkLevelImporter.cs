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
        public override void OnImportAsset(AssetImportContext ctx)
        {
            LDtkLevelFile levelFile = ReadAssetText();

            //find the path above us, our level is always a child in a directory with the same name as the project name
            string directory = Path.GetDirectoryName(assetPath);
            string directoryName = Path.GetFileName(directory);

            string relPath = $"/../../{directoryName}.ldtk";
            
            string endPath = $"{assetPath}{relPath}";

            endPath = LDtkPathUtility.CleanPath(endPath);
            
            bool exists = LDtkRelativeGetterLevels.IsAssetRelativeToAssetPathExists(endPath);
            if (!exists)
            {
                return;
            }

            GameObject loadAssetAtPath = AssetDatabase.LoadAssetAtPath<GameObject>(endPath);
            EditorGUIUtility.PingObject(loadAssetAtPath);

            Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(endPath);

            Level level = levelFile.FromJson;

            //string joint = string.Join(subAssets.Select(p => p.name));
            foreach (Object subAsset in subAssets)
            {
                
                Debug.Log(subAsset);
            }
            
            Object foundLevel = subAssets.FirstOrDefault(p => p != null && p.name == level.Identifier);
            
            
            if (foundLevel == null)
            {
                Debug.LogError($"LDtk: Issue locating the level in the project file for \"{endPath}\"");
                ctx.SetMainObject(null);
                return;
            }
            

            GameObject levelRoot = (GameObject)foundLevel;


            ctx.AddObjectToAsset("levelRoot", levelRoot);
            ctx.AddObjectToAsset("levelFile", levelFile, LDtkIconUtility.LoadLevelFileIcon());
            ctx.SetMainObject(levelRoot);
            
            
            
            //to import a level, we need to read from the project, which already generated the assets so that we can get them.
            //So, we need to import a project first. 
            //the project 
            
            //AssetDatabase.ImportAsset("filepath", ImportAssetOptions.ForceUpdate);
            
        }
    }
}