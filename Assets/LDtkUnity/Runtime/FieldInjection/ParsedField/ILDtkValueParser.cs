using System;

namespace LDtkUnity.FieldInjection
{
    public delegate object ParseFieldValueAction(string input);
    
    public interface ILDtkValueParser
    {
        bool IsType(Type triedType);
        object ParseValue(string input);
    }
}