namespace LDtkUnity.Editor
{
    public interface ILDtkValueParser
    {
        bool TypeName(FieldInstance instance);
        object ImportString(object input);
    }
}