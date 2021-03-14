using UnityEngine;

namespace LDtkUnity
{
    public class LDtkParsedColor : ILDtkValueParser
    {
        public string TypeName => "Color";
        
        public object ParseValue(object input)
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