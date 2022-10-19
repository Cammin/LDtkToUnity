using JetBrains.Annotations;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal sealed class LDtkParsedColor : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsColor;

        public object ImportString(object input)
        {
            //color can never be null, but just in case
            if (input == null)
            {
                LDtkDebug.LogWarning("Color field was unexpectedly null");
                return "#000000".ToColor();
            }
            
            string colorString = (string) input;
            
            return colorString.ToColor();
        }
    }
}