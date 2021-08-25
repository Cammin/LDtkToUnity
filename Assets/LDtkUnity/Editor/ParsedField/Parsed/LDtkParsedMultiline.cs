using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkParsedMultiline : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance)
        {
            return instance.IsMultilines;
        }

        public object ImportString(object input)
        {
            //strings can be legally null
            if (input == null)
            {
                return string.Empty;
            }
            
            string stringInput = (string) input;
            
            //this is to correct the formatting for a Newline in Unity
            string properText = stringInput.Replace("\\n", "\n");
            
            return properText;
        }
    }
}