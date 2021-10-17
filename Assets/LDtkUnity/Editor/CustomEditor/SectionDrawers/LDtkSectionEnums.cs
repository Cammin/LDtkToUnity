using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkSectionEnums : LDtkSectionDataDrawer<EnumDefinition>
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

        public LDtkSectionEnums(SerializedObject serializedObject) : base(serializedObject)
        {
            _enumGenerateProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_GENERATE);
            _enumPathProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_PATH);
            _enumNamespaceProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_NAMESPACE);
            
            _pathDrawer = new PathDrawer(_enumPathProp,
                _pathLabel, 
                Importer.assetPath,
                "Set the path for the location that the enum file will be generated", "cs", "Location for generated C# file");
        }

        protected override void GetDrawers(EnumDefinition[] defs, List<LDtkContentDrawer<EnumDefinition>> drawers)
        {
        }

        protected override void DrawDropdownContent(EnumDefinition[] datas)
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

            if (!CSharpCodeHelpers.IsEmptyOrProperNamespaceName(_enumNamespaceProp.stringValue))
            {
                using (new LDtkIconSizeScope(Vector2.one * 16))
                {
                    EditorGUILayout.HelpBox("Must be a valid C# namespace name", MessageType.Error);
                }
            }
        }

        /*private static void DrawPathField()
        {
            GUIContent buttonContent = new GUIContent()
            {
                tooltip = "Set the path for the location that the enum file will be generated",
                image = LDtkIconUtility.GetUnityIcon("Folder"),
            };
            
            string assetPath = Path.GetFullPath(Importer.assetPath);
            string csPath = Path.ChangeExtension(assetPath, ".cs");

            string defaultRefPath = _pathDrawer.GetRelativePath(assetPath, csPath);

            const float buttonWidth = 26;
            Rect rect = LDtkEditorGUI.PropertyFieldWithDefaultText(_enumPathProp, _pathLabel, defaultRefPath, buttonWidth + 2);
            Rect buttonRect = new Rect(rect)
            {
                xMin = rect.xMax - buttonWidth
            };

            if (!GUI.Button(buttonRect, buttonContent, EditorStyles.miniButton))
            {
                return;
            }
            
            string destinationEnumPath = EditorUtility.SaveFilePanel("Location for generated C# file",
                Path.GetDirectoryName(csPath),
                Path.GetFileName(csPath), "cs");

            /*if (destinationEnumPath.StartsWith(Application.dataPath))
            {
                destinationEnumPath = "Assets/" + destinationEnumPath.Substring(Application.dataPath.Length + 1);
            }#1#
            
            if (!string.IsNullOrEmpty(destinationEnumPath))
            {
                string relPath = _pathDrawer.GetRelativePath(assetPath, destinationEnumPath);
                relPath = LDtkPathUtility.CleanPathSlashes(relPath);
                enumPathProp.stringValue = relPath; 
            }
        }*/

        
        

    }
}