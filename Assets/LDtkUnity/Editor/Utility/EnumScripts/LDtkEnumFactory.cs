using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace LDtkUnity.Editor
{
    //make this generate into the imported asset  
    public class LDtkEnumFactory
    {
        public const string TEMPLATES_PATH = "Editor/AssetManagement/AssetFactories/EnumHandler/";
        
        private const string TEMPLATE_DEF_PATH = TEMPLATES_PATH + "EnumNamespaceTemplate.txt";
        private const string TEMPLATE_NAMESPACE = "#NAMESPACE#";
        private const string TEMPLATE_DEF_ENUMS = "#ENUMS#";
        
        private const string TEMPLATE_PATH = TEMPLATES_PATH + "EnumTemplate.txt";
        private const string TEMPLATE_DEFINITION = "#DEFINITION#";
        private const string TEMPLATE_VALUES = "#VALUES#";
        private const string TEMPLATE_PROJECT = "#PROJECT#";

        private const string GAP = "    ";


        private readonly LDtkEnumFactoryTemplate[] _templates;
        private readonly Object _project;
        private readonly string _nameSpace;
        private readonly AssemblyDefinitionAsset _assembly;
        

        public LDtkEnumFactory(EnumDefinition[] enums, Object project, string nameSpace, AssemblyDefinitionAsset assembly)
        {
            _templates = enums.Select(LDtkEnumFactoryTemplate.FromDefinition).ToArray();
            _project = project;
            _nameSpace = nameSpace;
            _assembly = assembly;
        }

        private string Directory => LDtkPathUtility.SiblingDirectoryOfAsset(_project);
        private string EnumScriptPath => $"{Directory}/{_project.name}_Enums.cs";
        public bool IsScriptExists => File.Exists(EnumScriptPath);
        
        public void CreateEnumFile()
        {
            string wholeText = GetWholeEnumText();
            LDtkPathUtility.TryCreateDirectory(Directory);
            File.WriteAllText(EnumScriptPath, wholeText);
            LDtkAsmRefFactory.CreateAssemblyDefinitionReference(Directory, _assembly);
            
            AssetDatabase.Refresh();
        }

        private string GetWholeEnumText()
        {
            bool hasNamespace = !string.IsNullOrEmpty(_nameSpace);
            string namespacePass = string.IsNullOrEmpty(_nameSpace) ? TEMPLATE_DEF_ENUMS : ReplaceNamespaceContents();

            string allEnumTextContent = GenerateEnumDefinitions(hasNamespace);

            //put the enums into the base template
            string wholeText = namespacePass.Replace(TEMPLATE_DEF_ENUMS, allEnumTextContent);

            //final pass for any inconsistent line endings
            wholeText = wholeText.Replace("\r\n", "\n");
            
            return wholeText;
        }

        private string ReplaceNamespaceContents()
        {
            string startText = LDtkInternalUtility.Load<TextAsset>(TEMPLATE_DEF_PATH).text;
            return startText.Replace(TEMPLATE_NAMESPACE, _nameSpace);
        }

        private string GenerateEnumDefinitions(bool hasNamespace)
        {
            IEnumerable<string> enumTemplates = _templates.Select(p => GenerateEnumDefinition(p, hasNamespace));
            return string.Join("\n\n", enumTemplates);
        }

        private string GenerateEnumDefinition(LDtkEnumFactoryTemplate template, bool hasNamespace)
        {
            string templateTxt = LDtkInternalUtility.Load<TextAsset>(TEMPLATE_PATH).text;
            string projectName = _project.name.Replace(' ', '_');
            string joinedValues = string.Join($",\n{GAP}", template.Values);
            
            templateTxt = templateTxt.Replace(TEMPLATE_PROJECT, projectName);
            templateTxt = templateTxt.Replace(TEMPLATE_DEFINITION, template.Definition);
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
    }
}