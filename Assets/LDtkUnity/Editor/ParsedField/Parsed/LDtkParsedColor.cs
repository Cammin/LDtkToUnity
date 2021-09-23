using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkParsedColor : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsColor;

        public object ImportString(object input)
        {
            //color can never be null, but just in case
            if (input == null)
            {
                Debug.LogWarning("LDtk: Color field was unexpectedly null");
                return "#000000".ToColor();
            }
            
            string colorString = (string) input;
            
            return colorString.ToColor();
        }
    }
}