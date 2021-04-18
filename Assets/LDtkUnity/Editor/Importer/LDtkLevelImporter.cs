using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.JSON_LEVEL)]
    [ScriptedImporter(VERSION, EXTENSION)]
    public class LDtkLevelImporter : LDtkJsonImporter<LDtkLevelFile>
    {
        private const int VERSION = 0;
        private const string EXTENSION = "ldtkl";

        public override void OnImportAsset(AssetImportContext ctx)
        {
            LDtkLevelFile levelFile = ReadAssetText(ctx);
            
            ctx.AddObjectToAsset("levelFile", levelFile, LDtkIconLoader.LoadLevelFileIcon());
            ctx.SetMainObject(levelFile);
        }
    }
}