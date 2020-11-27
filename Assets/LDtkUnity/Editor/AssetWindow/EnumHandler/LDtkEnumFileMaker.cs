using System.IO;
using LDtkUnity.Editor.AssetLoading;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetWindow.EnumHandler
{
    public static class LDtkEnumFileMaker
    {
        private const string TEMPLATE_PATH = "LDtkUnity/Editor/AssetWindow/EnumHandler/EnumTemplate.txt";
        private const string TEMPLATE_DEFINITION = "#DEFINITION#";
        private const string TEMPLATE_VALUES = "#VALUES#";
        
        public static void CreateEnumFile(string folderPath, string type, string[] values)
        {
            if (!LDtkEditorAssetLoader.IsValidFolder(folderPath))
            {
                LDtkEditorAssetLoader.CreateDirectory(folderPath);
            }
            
            string template = GenerateTemplate(type, values);
            
            string writeFilePath = folderPath + type + ".cs";
            using (StreamWriter streamWriter = new StreamWriter(writeFilePath))
            {
                streamWriter.Write(template);
            }
            AssetDatabase.Refresh();
        }

        private static string GenerateTemplate(string type, string[] values)
        {
            string template = LDtkEditorAssetLoader.Load<TextAsset>(TEMPLATE_PATH).text;
            string joinedValues = string.Join(",\n        ", values);

            template = template.Replace(TEMPLATE_DEFINITION, type);
            template = template.Replace(TEMPLATE_VALUES, joinedValues);
            
            return template;
        }
    }
}