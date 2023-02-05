using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkSectionDependencies : LDtkSectionDrawer
    {
        private readonly SerializedObject _serializedObject;
        private string[] _dependencies;
        private Object[] _dependencyAssets;
        
        protected override string GuiText => "Dependencies";
        protected override string GuiTooltip => "Dependencies that were defined since the last import.\n" +
                                                "If any of these dependencies save changes, then this asset will automatically reimport.";
        protected override Texture GuiImage => LDtkIconUtility.GetUnityIcon("UnityEditor.FindDependencies", "");
        protected override string ReferenceLink => LDtkHelpURL.SECTION_DEPENDENCIES;
        protected override bool SupportsMultipleSelection => false;
        
        public LDtkSectionDependencies(LDtkImporterEditor editor, SerializedObject serializedObject) : base(editor, serializedObject)
        {
            _serializedObject = serializedObject;
            UpdateDependencies();
        }

        public void UpdateDependencies() //originally this was updated less regularly for performance, but was difficult to find the right event for.
        {
            if (Editor == null || _serializedObject == null)
            {
                return;
            }

            _serializedObject.Update();
            if (_serializedObject.targetObject == null)
            {
                return;
            }
            
            string importerPath = AssetDatabase.GetAssetPath(_serializedObject.targetObject);
            
            _dependencies = LDtkDependencyCache.Load(importerPath);
            _dependencyAssets = new Object[_dependencies.Length];

            for (int i = 0; i < _dependencies.Length; i++)
            {
                _dependencyAssets[i] = AssetDatabase.LoadAssetAtPath<Object>(_dependencies[i]);
            }
        }

        public override void Draw()
        {
            if (_dependencyAssets.All(p => p == null))
            {
                return;
            }
            
            LDtkEditorGUIUtility.DrawDivider();
            base.Draw();
        }
        
        protected override void DrawDropdownContent()
        {
            for (int i = 0; i < _dependencies.Length; i++)
            {
                string dependency = _dependencies[i];
                Object dependencyAsset = _dependencyAssets[i];

                if (dependencyAsset == null)
                {
                    continue;
                }
                
                GUIContent content = new GUIContent(dependencyAsset.name, dependency);

                using (new LDtkGUIEnabledScope(false))
                {
                    EditorGUILayout.ObjectField(content, dependencyAsset, typeof(Object), false);
                }
            }
        }
    }
}