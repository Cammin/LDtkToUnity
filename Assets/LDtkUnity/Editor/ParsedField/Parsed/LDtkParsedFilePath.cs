using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkParsedFilePath : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsFilePath;

        public object ImportString(object input)
        {
            //strings can be legally null
            if (input == null)
            {
                return string.Empty;
            }
            
            string stringInput = (string) input;
            
            return stringInput;
        }
    }
}