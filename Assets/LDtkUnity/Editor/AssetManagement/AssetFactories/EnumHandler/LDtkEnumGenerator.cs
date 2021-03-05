using System.Linq;
using UnityEditorInternal;

namespace LDtkUnity.Editor
{
    public static class LDtkEnumGenerator
    {
        public static void GenerateEnumScripts(EnumDefinition[] enums, string relativeFolderPath, string projectName, string nameSpace, AssemblyDefinitionAsset asmDef)
        {
            LDtkEnumFactoryTemplate[] templates = enums.Select(GenerateTemplate).ToArray();
            LDtkEnumFactory factory = new LDtkEnumFactory(templates, projectName, nameSpace, asmDef);
            factory.CreateEnumFile(relativeFolderPath);
        }

        private static LDtkEnumFactoryTemplate GenerateTemplate(EnumDefinition definition)
        {
            string[] values = definition.Values.Select(value => value.Id).ToArray();
            return new LDtkEnumFactoryTemplate(definition.Identifier, values);
        }


    }
}