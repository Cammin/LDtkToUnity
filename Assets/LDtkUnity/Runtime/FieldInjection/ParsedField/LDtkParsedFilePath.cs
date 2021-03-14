namespace LDtkUnity
{
    public class LDtkParsedFilePath : ILDtkValueParser
    {
        public string TypeName => "FilePath";

        public object ParseValue(object input)
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