using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.JSON_LEVEL)]
    [ScriptedImporter(LDtkImporterConsts.LEVEL_VERSION, LDtkImporterConsts.LEVEL_EXT, LDtkImporterConsts.LEVEL_ORDER)]
    public class LDtkLevelImporter : LDtkJsonImporter<LDtkLevelFile>
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            LDtkLevelFile levelFile = ReadAssetText();
            
            ctx.AddObjectToAsset("levelFile", levelFile, LDtkIconUtility.LoadLevelFileIcon());
            ctx.SetMainObject(levelFile);
        }
    }
}