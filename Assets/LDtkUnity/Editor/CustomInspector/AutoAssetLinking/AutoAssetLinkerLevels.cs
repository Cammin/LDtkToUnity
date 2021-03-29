using UnityEngine;

namespace LDtkUnity.Editor
{
    public class AutoAssetLinkerLevels : AutoAssetLinker<Level, LDtkLevelFile>
    {
        protected override GUIContent ButtonContent => new GUIContent()
        {
            text = "Auto-Assign Levels",
            tooltip = "Automatically assigns the references to the level files by using the json's relative paths.",
            image = LDtkIconLoader.LoadLevelIcon()
        };
        protected override string GetRelPath(Level definition) => definition.ExternalRelPath;
    }
}