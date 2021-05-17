using UnityEditor;
using UnityEditor.AssetImporters;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelImporter))]
    public class LDtkLevelImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            ApplyRevertGUI();
        }
    }
}