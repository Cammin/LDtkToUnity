using System;
using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LDtkFields
    {
        /// <summary>
        /// Used to check if a field exists in this component.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool ContainsField(string identifier)
        {
            //a more optimized way to check if it exists in update loops so that we avoid using linq when possible
            if (_keys.ContainsKey(identifier))
            {
                return true;
            }

            return _fields.Any(p => p.Identifier == identifier);
        }

        /// <summary>
        /// Used to check if a single field is null in this component.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// If the field doesn't exist or is null.
        /// </returns>
        public bool IsNull(string identifier)
        {
            return !TryGetField(identifier, out LDtkField field) || field.IsSingleNull();
        }
        
        /// <summary>
        /// Used to check if a field's array element is null in this component.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="index">
        /// Index of the array.
        /// </param>
        /// <returns>
        /// If the field doesn't exist, is null, or if the index is outside the array's bounds.
        /// </returns>
        public bool IsNullAtArrayIndex(string identifier, int index)
        {
            return !TryGetField(identifier, out LDtkField field) || field.IsArrayElementNull(index);
        }

        /// <summary>
        /// Used to check if a field is an array.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// If the field is an array. If the field is not found, returns false.
        /// </returns>
        public bool IsArray(string identifier)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                LDtkDebug.LogError($"Didn't check if field is an array for \"{identifier}\", couldn't find the field.");
                return false;
            }

            return field.IsArray;
        }
        
        /// <summary>
        /// Used to get the size of an array field.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The array's length. If the field is not found or is not an array, the value is 0.
        /// </returns>
        public int GetArraySize(string identifier)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                LDtkDebug.LogError($"Didn't get array size for \"{identifier}\", couldn't find the field.");
                return 0;
            }

            if (!field.IsArray)
            {
                LDtkDebug.LogError($"Didn't get array size for \"{identifier}\", this field is not an array.");
                return 0;
            }

            FieldsResult<LDtkFieldElement[]> result = field.GetArray();
            return result.Success ? result.Value.Length : 0;
        }
        
        [Obsolete("Use EntityInstance.UnitySmartColor instead.")]
        public bool GetSmartColor(out Color firstColor)
        {
            LDtkDebug.LogWarning("Getting smart color is deprecated.");
            firstColor = Color.white;
            return true;
        }
    }
}