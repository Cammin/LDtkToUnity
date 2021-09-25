using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LDtkUnity.Editor
{
    //make this generate into the imported asset  
    public class LDtkEnumFactory
    {
        public const string TEMPLATES_PATH = "Editor/Utility/EnumScripts/";
        
        private const string TEMPLATE_DEF_PATH = TEMPLATES_PATH + "EnumNamespaceTemplate.txt";
        private const string TEMPLATE_NAMESPACE = "#NAMESPACE#";
        private const string TEMPLATE_DEF_ENUMS = "#ENUMS#";
        
        private const string TEMPLATE_PATH = TEMPLATES_PATH + "EnumTemplate.txt";
        private const string TEMPLATE_DEFINITION = "#DEFINITION#";
        private const string TEMPLATE_VALUES = "#VALUES#";
        private const string TEMPLATE_PROJECT = "#PROJECT#";

        private const string GAP = "    ";
        
        private readonly LDtkEnumFactoryTemplate[] _templates;
        private readonly string _writePath;
        private readonly string _nameSpace;


        public LDtkEnumFactory(LDtkEnumFactoryTemplate[] enums, string writePath, string nameSpace)
        {
            _templates = enums;
            _writePath = writePath;
            _nameSpace = nameSpace;
        }
        
        /// <returns>
        /// Whether the file creation was successful.
        /// </returns>
        public bool CreateEnumFile()
        {
            string directory = Path.GetDirectoryName(_writePath);;
            
            string wholeText = GetWholeEnumText();
            LDtkPathUtility.TryCreateDirectory(directory);
            File.WriteAllText(_writePath, wholeText);

            return true;
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
            TextAsset template = LDtkInternalUtility.Load<TextAsset>(TEMPLATE_DEF_PATH);
            if (template == null)
            {
                Debug.LogError("LDtk: Incorrectly loaded the enum definition path");
                return string.Empty;
            }
            string startText = template.text;
            return startText.Replace(TEMPLATE_NAMESPACE, _nameSpace);
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

            string templateTxt = LDtkInternalUtility.Load<TextAsset>(TEMPLATE_PATH).text;
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