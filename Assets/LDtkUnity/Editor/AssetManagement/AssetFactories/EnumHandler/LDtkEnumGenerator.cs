using System.Collections.Generic;
using System.Linq;
using LDtkUnity.Runtime.Data.Definition;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetFactories.EnumHandler
{
    public static class LDtkEnumGenerator
    {
        public static void GenerateEnumScripts(LDtkDefinitionEnum[] enums, string relativeFolderPath, string projectName)
        {
            

            LDtkEnumFactoryTemplate[] templates = enums.Select(GenerateTemplate).ToArray();

            LDtkEnumFactory.CreateEnumFile(relativeFolderPath, templates, projectName);
        }

        private static LDtkEnumFactoryTemplate GenerateTemplate(LDtkDefinitionEnum definition)
        {
            string[] values = definition.values.Select(value => value.id).ToArray();
            return new LDtkEnumFactoryTemplate(definition.identifier, values);
        }


    }
}