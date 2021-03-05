namespace LDtkUnity
{
    public class LDtkParsedFilePath : ILDtkValueParser
    {
        public string TypeName => "FilePath";

        public object ParseValue(object input)
        {
            string stringInput = (string) input;
            
            return stringInput;
        }
    }
}