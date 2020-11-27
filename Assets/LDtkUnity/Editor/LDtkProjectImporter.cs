using System.IO;
using LDtkUnity.Editor.AssetLoading;
using LDtkUnity.Runtime.UnityAssets;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.LDTK_PROJECT)]
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
