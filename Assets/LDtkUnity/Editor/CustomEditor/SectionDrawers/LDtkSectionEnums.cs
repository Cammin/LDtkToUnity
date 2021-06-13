using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkSectionEnums : LDtkSectionDataDrawer<EnumDefinition>
    {
        protected override string PropertyName => "";
        protected override string GuiText => "Enums";
        protected override string GuiTooltip => "The enums would be automatically generated as scripts.\n" +
                                                "The enum scripts will be created/updated at a defined location.";
        protected override Texture GuiImage => LDtkIconUtility.LoadEnumIcon();
        protected override string ReferenceLink => "https://cammin.github.io/LDtkUnity/documentation/Importer/topic_Section_Enums.html";

        private readonly GUIContent _generateLabel = new GUIContent
        {
            text = "Generate Enums",
            tooltip = "Toggle whether enums should be generated/overwritten."
        };
        private readonly GUIContent _pathLabel = new GUIContent
        {
            text = "Script Path",
            tooltip = "Use the folder button to set a path for the script to be generated.\n" +
                      "By default, the path is the same location as this .ldtk asset.\n" +
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
                Vector2 prevSize = EditorGUIUtility.GetIconSize();
                EditorGUIUtility.SetIconSize(Vector2.one * 32);
                
                EditorGUILayout.HelpBox("Must be a valid C# namespace name", MessageType.Error);
                EditorGUIUtility.SetIconSize(prevSize);
            }
        }

        private void DrawPathField()
        {
            SerializedProperty enumPathProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_PATH);
            
            string assetPath = AssetDatabase.GetAssetPath(Importer);
            string defaultFileName = Path.ChangeExtension(assetPath, ".cs");

            const float buttonWidth = 26;
            
            Rect rect = PropertyFieldWithDefaultText(enumPathProp, _pathLabel, defaultFileName, buttonWidth + 2);

            GUIContent buttonContent = new GUIContent()
            {
                tooltip = "Set the path for the location that the enum file will be generated",
                image = LDtkIconUtility.GetUnityIcon("Folder"),
                
            };

            Rect buttonRect = new Rect(rect)
            {
                xMin = rect.xMax - buttonWidth
            };

            if (GUI.Button(buttonRect, buttonContent, EditorStyles.miniButton))
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
        }

        private static Rect PropertyFieldWithDefaultText(SerializedProperty prop, GUIContent label, string defaultText, float xMaxOffset = 0)
        {
            GUI.SetNextControlName(label.text);
            Rect rt = GUILayoutUtility.GetRect(label, GUI.skin.textField);
            Rect fieldRect = new Rect(rt)
            {
                xMax = rt.xMax - xMaxOffset
            };
            
            EditorGUI.PropertyField(fieldRect, prop, label);
            if (!string.IsNullOrEmpty(prop.stringValue) || GUI.GetNameOfFocusedControl() == label.text || Event.current.type != EventType.Repaint)
            {
                return rt;
            }
            
            using (new EditorGUI.DisabledScope(true))
            {
                //if new editor skin
#if UNITY_2019_3_OR_NEWER
                const float offset = 2;
#else
                const float offset = 0;
#endif
                
                fieldRect.xMin += EditorGUIUtility.labelWidth + offset;
                GUI.skin.textField.Draw(fieldRect, new GUIContent(defaultText), false, false, false, false);
            }

            return rt;
        }
    }
}