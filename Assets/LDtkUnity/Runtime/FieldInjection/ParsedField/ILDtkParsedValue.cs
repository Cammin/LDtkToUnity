using System;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public interface ILDtkParsedValue
    {
        Type Type { get; }
        Type TypeArray { get; }
        string TypeString { get; }
        object ParseValue(string input);
    }
}