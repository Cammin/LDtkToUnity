using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkParsedBool : ILDtkValueParser
    {
        public string TypeName => "Bool";

        public object ParseValue(object input)
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