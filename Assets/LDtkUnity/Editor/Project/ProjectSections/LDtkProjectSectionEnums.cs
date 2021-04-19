using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkProjectSectionEnums : LDtkProjectSectionDrawer<EnumDefinition>
    {
        protected override string PropertyName => "";
        protected override string GuiText => "Enums";
        protected override string GuiTooltip => "The enums would be automatically generated as scripts.";
        protected override Texture GuiImage => LDtkIconLoader.LoadEnumIcon();
        
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
            SerializedProperty namespaceProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_NAMESPACE);

            GUIContent content = new GUIContent()
            {
                text = "Namespace",
                tooltip = "The namespace that the enums will live in. Leave empty for no namespace"
            };
            
            EditorGUILayout.PropertyField(namespaceProp, content);
            
            SerializedObject.ApplyModifiedProperties();

            return namespaceProp.stringValue;
        }

        private AssemblyDefinitionAsset AssemblyDefinitionField()
        {
            SerializedProperty assemblyProp = SerializedObject.FindProperty(LDtkProjectImporter.ENUM_ASSEMBLY);
            
            GUIContent content = new GUIContent()
            {
                text = "Assembly",
                tooltip = "The assembly that the enum file will be referenced in. Leave empty for no assembly definition reference"
            };
            
            assemblyProp.objectReferenceValue = EditorGUILayout.ObjectField(content, assemblyProp.objectReferenceValue, typeof(AssemblyDefinitionAsset), false);
            

            SerializedObject.ApplyModifiedProperties();


            return (AssemblyDefinitionAsset)assemblyProp.objectReferenceValue;
        }
        
        private void GenerateEnumsButton(EnumDefinition[] defs)
        {
            if (Project == null)
            {
                return;
            }
            
            string nameSpace = EnumsNamespaceField();
            AssemblyDefinitionAsset asmDef = AssemblyDefinitionField();
            
            

            LDtkEnumFactory factory = new LDtkEnumFactory(defs, Project, nameSpace, asmDef);

            GUIContent content = new GUIContent()
            {
                text = factory.IsScriptExists ? "Update Enums" : "Generate Enums",
                image = EditorGUIUtility.IconContent("cs Script Icon").image
            };
            
            if (GUILayout.Button(content))
            {
                factory.CreateEnumFile();
            }
        }
    }
}