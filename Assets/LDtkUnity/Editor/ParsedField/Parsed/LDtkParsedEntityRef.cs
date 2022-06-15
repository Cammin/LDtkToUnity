using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal class LDtkParsedEntityRef : ILDtkValueParser
    {
        public bool TypeName(FieldInstance instance)
        {
            return instance.IsEntityRef;
        }

        public object ImportString(object input)
        {
            //input begins as a string in json format

            if (input == null)
            {
                return null;
            }
            
            string inputString = input.ToString();
            if (string.IsNullOrEmpty(inputString))
            {
                return string.Empty;
            }
            
            FieldInstanceEntityReference reference = null;
            
            try
            {
                reference = FieldInstanceEntityReference.FromJson(inputString);
            }
            catch (Exception e)
            {
                LDtkDebug.LogError($"Json error for entity ref:\n{e}");
                return null;
            }
            
            if (reference == null)
            {
                LDtkDebug.LogError($"Entity ref was null when deserializing");
                return null;
            }

            return reference.EntityIid;
        }
    }
}