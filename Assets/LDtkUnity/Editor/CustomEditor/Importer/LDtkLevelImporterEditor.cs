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
        //protected override bool needsApplyRevert => false;
        private GameObject _projectAsset;

        public override void OnEnable()
        {
            base.OnEnable();
            LDtkLevelImporter importer = (LDtkLevelImporter)serializedObject.targetObject;
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
            

            //EditorGUILayout.BeginHorizontal();

            GUIContent buttonContent = new GUIContent()
            {
                text = "LDtk Project"
            };
            /*if (GUILayout.Button(buttonContent))
            {
                EditorGUIUtility.PingObject(_projectAsset);
            }*/

            
            //SerializedObject projectObj = new SerializedObject(_projectAsset);

            using (new LDtkGUIScope(false))
            {
                EditorGUILayout.ObjectField(buttonContent, _projectAsset, typeof(GameObject), false);
            }

            //EditorGUILayout.EndHorizontal();
        }
    }
}