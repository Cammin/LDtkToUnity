using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal abstract class LDtkSubImporterEditor : LDtkImporterEditor
    {
        private static readonly GUIContent ReimportProjectButton = new GUIContent()
        {
            text = "Reimport Project",
            tooltip = "Reimport this asset's project."
        };
        
        private GUIContent _buttonContent;
        protected GameObject _projectAsset;

        public override void OnEnable()
        {
            base.OnEnable();
            
            _buttonContent = new GUIContent
            {
                text = "Source Project",
                image = LDtkIconUtility.LoadProjectFileIcon()
            };
        }

        protected void TryDrawProjectReferenceButton()
        {
            if (!_projectAsset)
            {
                DrawTextBox("Could not locate the source project asset. Make sure LDtk can also load this asset from it's project, and try again.");
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    using (new EditorGUIUtility.IconSizeScope(Vector2.one * 16))
                    {
                        EditorGUILayout.ObjectField(_buttonContent, _projectAsset, typeof(GameObject), false);
                    }
                }

                if (GUILayout.Button(ReimportProjectButton, GUILayout.Width(105)))
                {
                    string assetPath = AssetDatabase.GetAssetPath(_projectAsset);
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
            }
        }
    }
}