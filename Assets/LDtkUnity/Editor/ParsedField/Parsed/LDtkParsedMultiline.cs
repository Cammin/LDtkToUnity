using JetBrains.Annotations;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal sealed class LDtkParsedMultiline : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance)
        {
            return instance.IsMultilines;
        }

        public object ImportString(LDtkFieldParseContext ctx)
        {
            object input = ctx.Input;
            //strings can be legally null
            if (input == null)
            {
                return default;
            }
            
            string stringInput = (string) input;

            //this is to correct the formatting for a Newline in Unity
            string properText = stringInput.Replace("\\n", "\n");
            
            return properText;
        }
    }
}