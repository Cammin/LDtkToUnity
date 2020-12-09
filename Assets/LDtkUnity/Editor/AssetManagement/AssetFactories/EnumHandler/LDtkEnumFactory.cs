using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkEnumFactory
    {
        private const string TEMPLATE_DEF_PATH = LDtkEnumFactoryPath.PATH + "EnumDefinitionTemplate.txt";
        private const string TEMPLATE_DEF_ENUMS = "#ENUMS#";
        
        private const string TEMPLATE_PATH = LDtkEnumFactoryPath.PATH + "EnumTemplate.txt";
        private const string TEMPLATE_DEFINITION = "#DEFINITION#";
        private const string TEMPLATE_VALUES = "#VALUES#";
        private const string TEMPLATE_PROJECT = "#PROJECT#";

        private const string GAP = "    ";
        
        public static string FilePath(string folderPath, string projectName) => $"{folderPath}/{projectName}/{projectName}_Enums.cs";
        public static string FolderPath(string folderPath, string projectName) => $"{folderPath}/{projectName}";
        
        public static void CreateEnumFile(string folderPath, LDtkEnumFactoryTemplate[] templates, string projectName)
        {
            string actualFolderPath = FolderPath(folderPath, projectName);
            string writeFilePath = FilePath(folderPath, projectName);
            
            LDtkAssetDirectory.CreateDirectoryIfNotValidFolder(actualFolderPath);
            
            Debug.Log($"LDtk: Generating enum script: {writeFilePath}");
            string enumFullText = GenerateFullFileContents(templates, projectName);
            using (StreamWriter streamWriter = new StreamWriter(writeFilePath))
            {
                streamWriter.Write(enumFullText);
            }

            LDtkAsmRefFactory.CreateAssemblyDefinitionReference(actualFolderPath);
            AssetDatabase.Refresh();
        }

        private static string GenerateFullFileContents(LDtkEnumFactoryTemplate[] templates, string projectName)
        {
            string startText = LDtkInternalLoader.Load<TextAsset>(TEMPLATE_DEF_PATH).text;
            string enums = GenerateEnumTemplates(templates, projectName);
            return startText.Replace(TEMPLATE_DEF_ENUMS, enums);
        }

        private static string GenerateEnumTemplates(LDtkEnumFactoryTemplate[] templates, string projectName)
        {
            IEnumerable<string> enumTemplates = templates.Select(p => GenerateEnumTemplate(p, projectName));
            return string.Join("\n\n", enumTemplates);
        }
        
        private static string GenerateEnumTemplate(LDtkEnumFactoryTemplate template, string projectName)
        {
            string templateTxt = LDtkInternalLoader.Load<TextAsset>(TEMPLATE_PATH).text;
            string joinedValues = string.Join($",\n{GAP}{GAP}", template.Values);

            projectName = projectName.Replace(" ", "_");

            templateTxt = templateTxt.Replace(TEMPLATE_PROJECT, projectName);
            templateTxt = templateTxt.Replace(TEMPLATE_DEFINITION, template.EnumType);
            templateTxt = templateTxt.Replace(TEMPLATE_VALUES, joinedValues);
            templateTxt = templateTxt.Replace("#t#", "GAP");
            
            return templateTxt;
        }

        public static bool AssetExists(string folderPath, string projectName)
        {
            string filePath = FilePath(folderPath, projectName);
            return File.Exists(filePath);
        }
        
        
        
        
    }
}