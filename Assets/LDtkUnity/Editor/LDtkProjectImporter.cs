using System.IO;
using LDtkUnity.UnityAssets;
using UnityEditor;
using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.JSON_LDTK_PROJECT)]
    [ScriptedImporter(1, "ldtk")]
    public class LDtkProjectImporter : ScriptedImporter
    {       
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string path = ctx.assetPath;
            string content = File.ReadAllText(path);
            TextAsset textAsset = new TextAsset(content);

            Texture2D tex = LDtkIconLoader.LoadProjectIcon();

            ctx.AddObjectToAsset("ldtk", textAsset, tex);
            ctx.SetMainObject(textAsset);
            AssetDatabase.Refresh();
            
            //Debug.Log("Detected updated LDtk project");
        }
    }
}
