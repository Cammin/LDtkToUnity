using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    [CustomEditor(typeof(LDtkLevelImporter))]
    public class LDtkLevelImporterEditor : LDtkImporterEditor
    {
        private readonly GUIContent _buttonContent = new GUIContent()
        {
            text = "LDtk Project"
        };
        
        private GameObject _projectAsset;

        public override void OnEnable()
        {
            base.OnEnable();
            LDtkLevelImporter importer = (LDtkLevelImporter)target;
            _projectAsset = importer.GetProjectAsset();
        }

        public override void OnInspectorGUI()
        {
            try
            {
                TryDrawProjectReferenceButton();
            }
            finally
            {
                ApplyRevertGUI();
            }
        }

        private void TryDrawProjectReferenceButton()
        {
            if (!_projectAsset)
            {
                return;
            }

            using (new LDtkGUIScope(false))
            {
                EditorGUILayout.ObjectField(_buttonContent, _projectAsset, typeof(GameObject), false);
            }
        }
    }
}