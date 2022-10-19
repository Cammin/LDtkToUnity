using JetBrains.Annotations;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal sealed class LDtkParsedFilePath : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsFilePath;

        public object ImportString(object input)
        {
            //strings can be legally null
            if (input == null)
            {
                return default;
            }
            
            string stringInput = (string) input;
            
            return stringInput;
        }
    }
}