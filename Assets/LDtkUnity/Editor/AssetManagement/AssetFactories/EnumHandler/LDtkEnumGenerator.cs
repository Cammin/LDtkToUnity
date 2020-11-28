using System.Linq;
using LDtkUnity.Runtime.Data.Definition;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetFactories.EnumHandler
{
    public static class LDtkEnumGenerator
    {
        public static void GenerateEnumScripts(LDtkDefinitionEnum[] enums, string relativeFolderPath, string projectName)
        {
            relativeFolderPath += "\\Enums\\";
                
            Debug.Log($"LDtk: Generating enum scripts at path: {relativeFolderPath}");
            
            foreach (LDtkDefinitionEnum enumDefinition in enums)
            {
                GenerateEnumScript(enumDefinition, relativeFolderPath, projectName);
            }
        }

        private static void GenerateEnumScript(LDtkDefinitionEnum definition, string folderPath, string projectName)
        {
            string[] values = definition.values.Select(value => value.id).ToArray();
            LDtkEnumFactory.CreateEnumFile(folderPath, definition.identifier, values, projectName);
        }
    }
}