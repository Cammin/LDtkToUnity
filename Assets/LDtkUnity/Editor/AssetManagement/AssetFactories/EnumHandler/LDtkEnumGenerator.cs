using System.Linq;
using LDtkUnity.Data;

namespace LDtkUnity.Editor
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