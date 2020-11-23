using System.IO;
using LDtkUnity.Runtime.UnityAssets;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [HelpURL(LDtkHelpURL.LDTK_PROJECT)]
    [ScriptedImporter(1, "ldtk")]
    public class LDtkImporter : ScriptedImporter 
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string path = ctx.assetPath;
            string content = File.ReadAllText(path);
            TextAsset textAsset = new TextAsset(content);
            
            Texture2D tex = GetTexture2D();

            ctx.AddObjectToAsset ("ldtk", textAsset, tex);
            ctx.SetMainObject(textAsset);
            AssetDatabase.Refresh();
            
            //Debug.Log("Detected updated LDtk project");
        }

        private static Texture2D GetTexture2D()
        {
            Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Packages/com.cammin.ldtkunity/Icons/LDtkProjectIcon.png", typeof(Texture2D));
            
            if (tex != null) return tex;
            
            //for repo development use
            tex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/LDtkUnity/Icons/LDtkProjectIcon.png", typeof(Texture2D));
                
            if (tex == null)
            {
                Debug.LogError("tex null");
            }
            return tex;
        }
    }
}
