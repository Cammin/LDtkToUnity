using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionEnums  : LDtkProjectSectionDrawer<EnumDefinition>
    {
        protected override string PropertyName => "";
        protected override string GuiText => "Enums";
        protected override string GuiTooltip => "The enums would be automatically generated as scripts.";
        protected override Texture2D GuiImage => LDtkIconLoader.LoadEnumIcon();
        
        public LDtkProjectSectionEnums(SerializedObject serializedObject) : base(serializedObject)
        {
        }

        protected override void GetDrawers(EnumDefinition[] defs, List<LDtkContentDrawer<EnumDefinition>> drawers)
        {
            foreach (EnumDefinition def in defs)
            {
                drawers.Add(new LDtkDrawerEnum(def));
            }
        }

        protected override void DrawDropdownContent(EnumDefinition[] datas)
        {
            base.DrawDropdownContent(datas);
            
            GenerateEnumsButton(datas);
        }

        private string EnumsNamespaceField()
        {
            SerializedProperty namespaceProp = SerializedObject.FindProperty(LDtkProject.ENUM_NAMESPACE);

            GUIContent content = new GUIContent()
            {
                text = "Namespace",
                tooltip = "The namespace that the enums will live in. Leave empty for no namespace"
            };
            
            EditorGUILayout.PropertyField(namespaceProp, content);
            
            if (SerializedObject.hasModifiedProperties)
            {
                SerializedObject.ApplyModifiedProperties();
            }

            return namespaceProp.stringValue;
        }

        private AssemblyDefinitionAsset AssemblyDefinitionField()
        {
            SerializedProperty assemblyProp = SerializedObject.FindProperty(LDtkProject.ENUM_ASSEMBLY);
            
            GUIContent content = new GUIContent()
            {
                text = "Assembly",
                tooltip = "The assembly that the enum file will be referenced in. Leave empty for no assembly definition reference"
            };
            
            EditorGUILayout.PropertyField(assemblyProp, content);
            
            if (SerializedObject.hasModifiedProperties)
            {
                SerializedObject.ApplyModifiedProperties();
            }

            return (AssemblyDefinitionAsset)assemblyProp.objectReferenceValue;
        }
        
        private void GenerateEnumsButton(EnumDefinition[] enumDefinitions)
        {
            string projectName = SerializedObject.targetObject.name;
            
            string targetPath = AssetDatabase.GetAssetPath(Project);
            targetPath = Path.GetDirectoryName(targetPath);
            
            string nameSpace = EnumsNamespaceField();

            AssemblyDefinitionAsset assemblyDefinitionAsset = AssemblyDefinitionField();

            bool fileExists = LDtkEnumFactory.AssetExists(targetPath, projectName);
            string buttonMessage = fileExists ? "Update Enums" : "Generate Enums";

            if (GUILayout.Button(buttonMessage))
            {
                LDtkEnumGenerator.GenerateEnumScripts(enumDefinitions, targetPath, Project.name, nameSpace, assemblyDefinitionAsset);
            }
        }
    }
}