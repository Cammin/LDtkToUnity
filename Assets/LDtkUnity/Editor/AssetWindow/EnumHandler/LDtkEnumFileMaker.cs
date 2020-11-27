using System.IO;
using LDtkUnity.Editor.AssetLoading;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor.AssetWindow.EnumHandler
{
    public static class LDtkEnumFileMaker
    {
        private const string ENUM_TEMPLATE_PATH = "LDtkUnity/Editor/AssetWindow/EnumHandler/EnumTemplate.txt";
        private const string TEMPLATE_DEFINITION = "#DEFINITION#";
        private const string TEMPLATE_VALUES = "#VALUES#";
        private const string TEMPLATE_NAMESPACE = "#NAMESPACE#";

        private const string ASM_REF_TEMPLATE_PATH = "LDtkUnity/Editor/AssetWindow/EnumHandler/AsmRefTemplate.txt";
        private const string ASM_REF_NAME = "LDtkUnity.Runtime";

        
        public static void CreateEnumFile(string folderPath, string type, string[] values, string projectName)
        {
            if (!LDtkEditorAssetLoader.IsValidFolder(folderPath))
            {
                LDtkEditorAssetLoader.CreateDirectory(folderPath);
            }
            
            string template = GenerateEnumTemplate(type, values, projectName);
            
            string writeFilePath = folderPath + type + ".cs";
            using (StreamWriter streamWriter = new StreamWriter(writeFilePath))
            {
                streamWriter.Write(template);
            }

            TryCreateAssemblyDefinitionReference(folderPath);

            AssetDatabase.Refresh();
        }

        private static void TryCreateAssemblyDefinitionReference(string folderPath)
        {
            string asmRefPath = folderPath + ASM_REF_NAME + ".asmref";
            
            //bool exists = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionReferenceAsset>(asmRefPath);
            //if (exists) return;
            
            string asmrefContents = LDtkEditorAssetLoader.Load<TextAsset>(ASM_REF_TEMPLATE_PATH).text;
            
            using (StreamWriter streamWriter = new StreamWriter(asmRefPath))
            {
                streamWriter.Write(asmrefContents);
            }
            
            /*
            TextAsset asmRef = new TextAsset(asmrefContents);
            AssetDatabase.CreateAsset(asmRef, asmRefPath);*/
            
        }

        private static string GenerateEnumTemplate(string type, string[] values, string projectName)
        {
            string template = LDtkEditorAssetLoader.Load<TextAsset>(ENUM_TEMPLATE_PATH).text;
            string joinedValues = string.Join(",\n        ", values);

            projectName = projectName.Replace(" ", "_");

            template = template.Replace(TEMPLATE_NAMESPACE, projectName);
            template = template.Replace(TEMPLATE_DEFINITION, type);
            template = template.Replace(TEMPLATE_VALUES, joinedValues);
            
            return template;
        }
    }
}