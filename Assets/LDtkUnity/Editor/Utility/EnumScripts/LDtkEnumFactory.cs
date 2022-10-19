using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LDtkUnity.Editor
{
    //make this generate into the imported asset  
    internal sealed class LDtkEnumFactory
    {
        public const string TEMPLATES_PATH = "Editor/Utility/EnumScripts/";
        
        private const string PATH_NAMESPACE = TEMPLATES_PATH + "EnumNamespaceTemplate.txt";
        private const string PATH_NO_NAMESPACE = TEMPLATES_PATH + "EnumNoNamespaceTemplate.txt";
        
        private const string PATH_ENUM_INSTANCE = TEMPLATES_PATH + "EnumTemplate.txt";
        
        
        private const string TEMPLATE_HEADER = "#HEADER#";
        private const string TEMPLATE_NAMESPACE = "#NAMESPACE#";
        private const string TEMPLATE_DEF_ENUMS = "#ENUMS#";
        private const string TEMPLATE_DEFINITION = "#DEFINITION#";
        private const string TEMPLATE_VALUES = "#VALUES#";
        private const string TEMPLATE_PROJECT = "#PROJECT#";

        private const string GAP = "    ";
        
        private readonly LDtkEnumFactoryTemplate[] _templates;
        private readonly string _writePath;
        private readonly string _nameSpace;
        private string _header;


        public LDtkEnumFactory(LDtkEnumFactoryTemplate[] enums, string writePath, string nameSpace, string header)
        {
            _header = header;
            _templates = enums;
            _writePath = writePath;
            _nameSpace = nameSpace;
        }
        
        /// <returns>
        /// Whether the file creation was successful.
        /// </returns>
        public bool CreateEnumFile()
        {
            string directory = Path.GetDirectoryName(_writePath);
            
            string wholeText = GetWholeEnumText();
            LDtkPathUtility.TryCreateDirectory(directory);
            File.WriteAllText(_writePath, wholeText);

            return true;
        }

        private string GetWholeEnumText()
        {
            bool hasNamespace = !string.IsNullOrEmpty(_nameSpace);
            string rootTemplatePath = hasNamespace ? PATH_NAMESPACE : PATH_NO_NAMESPACE;

            
            TextAsset template = LDtkInternalUtility.Load<TextAsset>(rootTemplatePath);
            if (template == null)
            {
                LDtkDebug.LogError("Incorrectly loaded the enum definition path");
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(template.text);
            sb.Replace(TEMPLATE_NAMESPACE, _nameSpace);

            //replace the header
            sb.Replace(TEMPLATE_HEADER, _header);
            
            //put the enums into the base template
            string allEnumTextContent = GenerateEnumDefinitions(hasNamespace);
            sb.Replace(TEMPLATE_DEF_ENUMS, allEnumTextContent);

            //final pass for any inconsistent line endings
            sb.Replace("\r\n", "\n");
            
            return sb.ToString();
        }

        private string GenerateEnumDefinitions(bool hasNamespace)
        {
            IEnumerable<string> enumTemplates = _templates.Select(p => GenerateEnumDefinition(p, hasNamespace));
            return string.Join("\n\n", enumTemplates);
        }

        private string GenerateEnumDefinition(LDtkEnumFactoryTemplate template, bool hasNamespace)
        {
            string fileName = Path.GetFileNameWithoutExtension(_writePath);
            string projectName = fileName.Replace(' ', '_');
            string joinedValues = string.Join($",\n{GAP}", template.Values);

            string templateTxt = LDtkInternalUtility.Load<TextAsset>(PATH_ENUM_INSTANCE).text;
            StringBuilder builder = new StringBuilder(templateTxt);
            
            builder.Replace(TEMPLATE_PROJECT, projectName);
            builder.Replace(TEMPLATE_DEFINITION, template.Definition);
            builder.Replace(TEMPLATE_VALUES, joinedValues);
            builder.Replace("#t#", "GAP");

            string finalText = builder.ToString();

            return hasNamespace ? AddIndentationToAllLines(finalText) : finalText;
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