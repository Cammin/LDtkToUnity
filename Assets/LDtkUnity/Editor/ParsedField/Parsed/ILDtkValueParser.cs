namespace LDtkUnity.Editor
{
    internal interface ILDtkValueParser
    {
        bool TypeName(FieldInstance instance);
        object ImportString(LDtkFieldParseContext ctx);
    }
}