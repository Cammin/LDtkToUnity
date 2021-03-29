using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class AutoAssetLinkerTilesets : AutoAssetLinker<TilesetDefinition, Texture2D>
    {
        protected override GUIContent ButtonContent => new GUIContent()
        {
            text = "Auto-Assign Tilesets",
            tooltip = "Automatically assigns the references to the textures by using the json's relative paths.",
            image = EditorGUIUtility.IconContent("Texture2D Icon").image
        };
        protected override string GetRelPath(TilesetDefinition definition) => definition.RelPath;
    }
}