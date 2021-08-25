using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkParsedBool : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsBool;

        public object ImportString(object input)
        {
            //bool can never be null but just in case
            if (input == null)
            {
                Debug.LogWarning("LDtk: Bool field was unexpectedly null");
                return false;
            }
            
            return Convert.ToBoolean(input);
        }
    }
}