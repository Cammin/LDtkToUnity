namespace LDtkUnity
{
    public class LDtkParsedColor : ILDtkValueParser
    {
        public string TypeName => "Color";
        
        public object ParseValue(object input)
        {
            string colorString = (string) input;
            
            return colorString.ToColor();
        }
    }
}