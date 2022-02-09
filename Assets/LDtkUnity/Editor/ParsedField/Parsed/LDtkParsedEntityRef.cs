using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    public class LDtkParsedEntityRef : ILDtkValueParser
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
            if (inputString.IsNullOrEmpty())
            {
                return string.Empty;
            }
            
            EntityReferenceInfos reference = null;
            
            try
            {
                reference = EntityReferenceInfos.FromJson(inputString);
            }
            catch (Exception e)
            {
                Debug.LogError($"LDtk: Json error for entity ref:\n{e}");
                return null;
            }
            
            if (reference == null)
            {
                Debug.LogError($"LDtk: Entity ref was null when deserializing");
                return null;
            }

            return reference.EntityIid;
        }
    }
}