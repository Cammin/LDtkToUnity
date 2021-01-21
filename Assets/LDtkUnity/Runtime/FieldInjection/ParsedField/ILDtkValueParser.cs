using System;

namespace LDtkUnity.FieldInjection
{
    public delegate object ParseFieldValueAction(object input);
    
    public interface ILDtkValueParser
    {
        string TypeName { get; }
        //bool IsType(Type triedType);
        object ParseValue(object input);
    }
}