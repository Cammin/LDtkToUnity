using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkEnumFactory
    {
        private const string TEMPLATE_DEF_PATH = LDtkEnumFactoryPath.PATH + "EnumNamespaceTemplate.txt";
        private const string TEMPLATE_NAMESPACE = "#NAMESPACE#";
        private const string TEMPLATE_DEF_ENUMS = "#ENUMS#";
        
        private const string TEMPLATE_PATH = LDtkEnumFactoryPath.PATH + "EnumTemplate.txt";
        private const string TEMPLATE_DEFINITION = "#DEFINITION#";
        private const string TEMPLATE_VALUES = "#VALUES#";
        private const string TEMPLATE_PROJECT = "#PROJECT#";

        private const string GAP = "    ";


        private readonly LDtkEnumFactoryTemplate[] _templates;
        private readonly string _projectName;
        private readonly string _nameSpace;
        private readonly AssemblyDefinitionAsset _assembly;
        

        public LDtkEnumFactory(LDtkEnumFactoryTemplate[] templates, string projectName, string nameSpace, AssemblyDefinitionAsset assembly)
        {
            _templates = templates;
            _projectName = projectName;
            _nameSpace = nameSpace;
            _assembly = assembly;
        }

        private static string FilePath(string folderPath, string projectName) => $"{folderPath}/{projectName}/{projectName}_Enums.cs";
        private static string FolderPath(string folderPath, string projectName) => $"{folderPath}/{projectName}";
        
        public void CreateEnumFile(string folderPath)
        {
            string actualFolderPath = FolderPath(folderPath, _projectName);
            string writeFilePath = FilePath(folderPath, _projectName);
            
            LDtkAssetDirectory.CreateDirectoryIfNotValidFolder(actualFolderPath);
            
            //Debug.Log($"LDtk: Generating enum script: {writeFilePath}");

            bool hasNamespace = !string.IsNullOrEmpty(_nameSpace);
            string namespacePass = string.IsNullOrEmpty(_nameSpace) ? TEMPLATE_DEF_ENUMS : GenerateNameSpaceContents();
            
            string enums = GenerateEnumDefinitions(hasNamespace);

            string wholeText = namespacePass.Replace(TEMPLATE_DEF_ENUMS, enums);

            //final pass for any inconsistent line endings
            wholeText = wholeText.Replace("\r\n","\n");

            using (StreamWriter streamWriter = new StreamWriter(writeFilePath))
            {
                streamWriter.Write(wholeText);
            }

            LDtkAsmRefFactory.CreateAssemblyDefinitionReference(actualFolderPath, _assembly);
            
            AssetDatabase.Refresh();
        }

        private string GenerateNameSpaceContents()
        {
            string startText = LDtkInternalLoader.Load<TextAsset>(TEMPLATE_DEF_PATH).text;
            return startText.Replace(TEMPLATE_NAMESPACE, _nameSpace);
        }

        private string GenerateEnumDefinitions(bool hasNamespace)
        {
            IEnumerable<string> enumTemplates = _templates.Select(p => GenerateEnumDefinition(p, hasNamespace));
            return string.Join("\n\n", enumTemplates);
        }

        private string GenerateEnumDefinition(LDtkEnumFactoryTemplate template, bool hasNamespace)
        {
            string templateTxt = LDtkInternalLoader.Load<TextAsset>(TEMPLATE_PATH).text;
            string projectName = _projectName.Replace(' ', '_');
            string joinedValues = string.Join($",\n{GAP}", template.Values);
            
            templateTxt = templateTxt.Replace(TEMPLATE_PROJECT, projectName);
            templateTxt = templateTxt.Replace(TEMPLATE_DEFINITION, template.EnumType);
            templateTxt = templateTxt.Replace(TEMPLATE_VALUES, joinedValues);
            templateTxt = templateTxt.Replace("#t#", "GAP");

            return hasNamespace ? AddIndentationToAllLines(templateTxt) : templateTxt;
        }

        private string AddIndentationToAllLines(string text)
        {
            string[] lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Insert(0, GAP);
            }

            return string.Join("\n", lines);
        }

        public static bool AssetExists(string folderPath, string projectName)
        {
            string filePath = FilePath(folderPath, projectName);
            return File.Exists(filePath);
        }
    }
}