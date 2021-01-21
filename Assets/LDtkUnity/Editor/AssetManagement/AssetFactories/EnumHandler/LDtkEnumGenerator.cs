using System.Linq;

namespace LDtkUnity.Editor
{
    public static class LDtkEnumGenerator
    {
        public static void GenerateEnumScripts(EnumDefinition[] enums, string relativeFolderPath, string projectName)
        {
            LDtkEnumFactoryTemplate[] templates = enums.Select(GenerateTemplate).ToArray();
            LDtkEnumFactory.CreateEnumFile(relativeFolderPath, templates, projectName);
        }

        private static LDtkEnumFactoryTemplate GenerateTemplate(EnumDefinition definition)
        {
            string[] values = definition.Values.Select(value => value.Id).ToArray();
            return new LDtkEnumFactoryTemplate(definition.Identifier, values);
        }


    }
}