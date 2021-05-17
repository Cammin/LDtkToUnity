using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
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
            //nothing
            /*foreach (EnumDefinition def in defs)
            {
                drawers.Add(new LDtkDrawerEnum(def));
            }*/
        }

        protected override void DrawDropdownContent(EnumDefinition[] datas)
        {
            //base.DrawDropdownContent(datas);
            
            //GenerateEnumsButton(datas);
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
            //DrawClassField();
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

        /*private void DrawClassField()
        {
            PropertyFieldWithDefaultText(wrapperClassNameProperty, m_WrapperClassNameLabel, CSharpCodeHelpers.MakeTypeName(Importer.name));

            if (!CSharpCodeHelpers.IsEmptyOrProperIdentifier(wrapperClassNameProperty.stringValue))
            {
                EditorGUILayout.HelpBox("Must be a valid C# identifier", MessageType.Error);
            }
        }*/

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


        public static void PropertyFieldWithDefaultText(SerializedProperty prop, GUIContent label, string defaultText)
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
        

        /*private string EnumsNamespaceField()
        {
            SerializedProperty namespaceProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_NAMESPACE);

            GUIContent content = new GUIContent()
            {
                text = "Namespace",
                tooltip = "The namespace that the enums will live in. Leave empty for no namespace"
            };
            
            EditorGUILayout.PropertyField(namespaceProp, content);
            
            SerializedObject.ApplyModifiedProperties();

            return namespaceProp.stringValue;
        }*/

        /*private AssemblyDefinitionAsset AssemblyDefinitionField()
        {
            SerializedProperty assemblyProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_PATH);
            
            GUIContent content = new GUIContent()
            {
                text = "Assembly",
                tooltip = "The assembly that the enum file will be referenced in. Leave empty for no assembly definition reference"
            };
            
            assemblyProp.objectReferenceValue = EditorGUILayout.ObjectField(content, assemblyProp.objectReferenceValue, typeof(AssemblyDefinitionAsset), false);
            

            SerializedObject.ApplyModifiedProperties();


            return (AssemblyDefinitionAsset)assemblyProp.objectReferenceValue;
        }*/

        /*private void GenerateEnumsButton(EnumDefinition[] defs)
        {
            if (Importer == null)
            {
                return;
            }
            
            string nameSpace = EnumsNamespaceField();
            //AssemblyDefinitionAsset asmDef = AssemblyDefinitionField();
            

            LDtkEnumFactory factory = new LDtkEnumFactory(defs, Importer.assetPath, nameSpace);

            GUIContent content = new GUIContent()
            {
                text = factory.IsScriptExists ? "Update Enums" : "Generate Enums",
                image = EditorGUIUtility.IconContent("cs Script Icon").image
            };
            
            if (GUILayout.Button(content))
            {
                factory.CreateEnumFile();
            }
        }*/
    }
}