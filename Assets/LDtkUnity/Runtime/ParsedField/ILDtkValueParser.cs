namespace LDtkUnity
{
    public interface ILDtkValueParser
    {
        bool TypeName(FieldInstance instance);
        object ImportString(object input);
    }
}