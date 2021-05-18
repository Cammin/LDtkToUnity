using UnityEditor;
using UnityEditor.AssetImporters;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkLevelImporter))]
    public class LDtkLevelImporterEditor : ScriptedImporterEditor
    {
        protected override bool needsApplyRevert => false;

        public override void OnInspectorGUI()
        {
            //draw nothing
        }
    }
}