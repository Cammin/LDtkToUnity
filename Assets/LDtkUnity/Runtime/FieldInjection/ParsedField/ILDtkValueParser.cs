using System;

namespace LDtkUnity.Runtime.FieldInjection.ParsedField
{
    public delegate object ParseFieldValueAction(string input);
    
    public interface ILDtkValueParser
    {
        Type Type { get; }
        Type TypeArray { get; }
        string TypeString { get; }
        object ParseValue(string input);
    }
}