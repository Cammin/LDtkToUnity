using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkSectionEnums : LDtkSectionDataDrawer<EnumDefinition>
    {
        private readonly PathDrawer _pathDrawer;
        private readonly SerializedProperty _enumGenerateProp;
        private readonly SerializedProperty _enumPathProp;
        private readonly SerializedProperty _enumNamespaceProp;
        
        protected override string PropertyName => "";
        protected override string GuiText => "Enums";
        protected override string GuiTooltip => "The enums would be automatically generated as scripts.\n" +
                                                "The enum scripts will be created/updated at a defined relative path.";
        protected override Texture GuiImage => LDtkIconUtility.LoadEnumIcon();
        protected override string ReferenceLink => LDtkHelpURL.SECTION_ENUMS;

        private readonly GUIContent _generateLabel = new GUIContent
        {
            text = "Generate Enums",
            tooltip = "Toggle whether enums should be generated/overwritten."
        };
        private readonly GUIContent _pathLabel = new GUIContent
        {
            text = "Script Path",
            tooltip = "Use the folder button to set a relative path for the script to be generated.\n" +
                      "By default, the relative path is the same location as this .ldtk asset.\n" +
                      "If the path was changed, then the script at the old path will need to be manually deleted."
        };
        private readonly GUIContent _namespaceLabel = new GUIContent
        {
            text = "Namespace",
            tooltip = "Define a namespace for the enum script if desired."
        };

        protected override bool SupportsMultipleSelection => true;

        public LDtkSectionEnums(LDtkImporterEditor editor, SerializedObject serializedObject) : base(editor, serializedObject)
        {
            _enumGenerateProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_GENERATE);
            _enumPathProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_PATH);
            _enumNamespaceProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_NAMESPACE);
            
            _pathDrawer = new PathDrawer(_enumPathProp,
                _pathLabel, 
                ProjectImporter.assetPath,
                "Set the path for the location that the enum file will be generated", "cs", "Location for generated C# file");
        }

        protected override void GetDrawers(EnumDefinition[] defs, List<LDtkContentDrawer<EnumDefinition>> drawers)
        {
            //NO drawers
        }

        protected override void DrawDropdownContent()
        {
            GenerateEnumUI();
        }
        
        private void GenerateEnumUI()
        {
            EditorGUILayout.PropertyField(_enumGenerateProp, _generateLabel);

            if (!_enumGenerateProp.boolValue)
            {
                return;
            }
            
            _pathDrawer.DrawPathField();
            DrawNamespaceField();
        }

        private void DrawNamespaceField()
        {
            LDtkEditorGUI.PropertyFieldWithDefaultText(_enumNamespaceProp, _namespaceLabel, "<Global namespace>");

            if (CSharpCodeHelpers.IsEmptyOrProperNamespaceName(_enumNamespaceProp.stringValue))
            {
                return;
            }
            
            using (new EditorGUIUtility.IconSizeScope(Vector2.one * 32))
            {
                EditorGUILayout.HelpBox("Must be a valid C# namespace name", MessageType.Error);
            }
        }

    }
}