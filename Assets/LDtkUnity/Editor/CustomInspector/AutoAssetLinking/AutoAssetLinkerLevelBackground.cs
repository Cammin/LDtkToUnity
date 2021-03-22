using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class AutoAssetLinkerLevelBackground : AutoAssetLinker<Level>
    {
        protected override GUIContent ButtonContent => new GUIContent()
        {
            text = "Auto-Assign Level Background",
            tooltip = "Automatically assigns the references to the level's background texture by using the json's relative paths.",
            
            image = LDtkIconLoader.GetUnityIcon("Texture2D")
            //image = EditorGUIUtility.IconContent("Texture2D Icon").image
        };
        protected override string GetRelPath(Level definition)
        {
            return definition.BgRelPath;
        }
    }
}