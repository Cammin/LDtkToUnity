using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkSectionEnums : LDtkSectionDrawer<EnumDefinition>
    {
        protected override string PropertyName => "";
        protected override string GuiText => "Enums";
        protected override string GuiTooltip => "The enums would be automatically generated as scripts.";
        protected override Texture GuiImage => LDtkIconUtility.LoadEnumIcon();
        
        private readonly GUIContent _generateLabel = EditorGUIUtility.TrTextContent("Generate Enums");
        private readonly GUIContent _pathLabel = EditorGUIUtility.TrTextContent("File Path");
        private readonly GUIContent _namespaceLabel = EditorGUIUtility.TrTextContent("Namespace");

        protected override bool SupportsMultipleSelection => true;

        public LDtkSectionEnums(SerializedObject serializedObject) : base(serializedObject)
        {
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
            // Importer settings UI.
            SerializedProperty enumGenerateProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_GENERATE);
            EditorGUILayout.PropertyField(enumGenerateProp, _generateLabel);

            if (!enumGenerateProp.boolValue)
            {
                return;
            }
            
            DrawPathField();
            DrawNamespaceField();
        }

        private void DrawNamespaceField()
        {
            SerializedProperty enumNamespaceProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_NAMESPACE);
            
            PropertyFieldWithDefaultText(enumNamespaceProp, _namespaceLabel, "<Global namespace>");

            if (!CSharpCodeHelpers.IsEmptyOrProperNamespaceName(enumNamespaceProp.stringValue))
            {
                EditorGUILayout.HelpBox("Must be a valid C# namespace name", MessageType.Error);
            }
        }

        private void DrawPathField()
        {
            SerializedProperty enumPathProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_PATH);
            
            EditorGUILayout.BeginHorizontal();
            string assetPath = AssetDatabase.GetAssetPath(Importer);
            string defaultFileName = Path.ChangeExtension(assetPath, ".cs");

            PropertyFieldWithDefaultText(enumPathProp, _pathLabel, defaultFileName);

            GUIContent buttonContent = new GUIContent()
            {
                tooltip = "Set the path for the location that the enum file will be generated",
                image = LDtkIconUtility.GetUnityIcon("Folder"),
                
            };
            
            if (GUILayout.Button(buttonContent, EditorStyles.miniButton, GUILayout.MaxWidth(25)))
            {
                string fileName = EditorUtility.SaveFilePanel("Location for generated C# file",
                    Path.GetDirectoryName(defaultFileName),
                    Path.GetFileName(defaultFileName), "cs");

                if (!string.IsNullOrEmpty(fileName))
                {
                    if (fileName.StartsWith(Application.dataPath))
                    {
                        fileName = "Assets/" + fileName.Substring(Application.dataPath.Length + 1);
                    }

                    enumPathProp.stringValue = fileName;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void PropertyFieldWithDefaultText(SerializedProperty prop, GUIContent label, string defaultText)
        {
            GUI.SetNextControlName(label.text);
            Rect rt = GUILayoutUtility.GetRect(label, GUI.skin.textField);

            EditorGUI.PropertyField(rt, prop, label);
            if (!string.IsNullOrEmpty(prop.stringValue) || GUI.GetNameOfFocusedControl() == label.text || Event.current.type != EventType.Repaint)
            {
                return;
            }
            
            using (new EditorGUI.DisabledScope(true))
            {
                rt.xMin += EditorGUIUtility.labelWidth;
                GUI.skin.textField.Draw(rt, new GUIContent(defaultText), false, false, false, false);
            }
        }
    }
}