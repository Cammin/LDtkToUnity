using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkParsedEntityRef : ILDtkValueParser
    {
        public bool TypeName(FieldInstance instance)
        {
            return instance.IsEntityRef;
        }

        public object ImportString(object input)
        {
            //input begins as a string in json format

            string inputString = input.ToString();
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
                Debug.LogError($"LDtk: Entity ref was null");
                return null;
            }
            
            //todo process how we should work out this reference
            
            return null;
        }
    }
}