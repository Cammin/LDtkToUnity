using System;
using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// A class that contains a single piece of data for a field, whether single or array.
    /// </summary>
    [Serializable] 
    internal class LDtkField
    {
        public const string PROPERTY_IDENTIFIER = nameof(_identifier);
        public const string PROPERTY_DATA = nameof(_data);
        public const string PROPERTY_SINGLE = nameof(_isSingle);

        [SerializeField] private string _identifier;
        [SerializeField] private bool _isSingle;
        [SerializeField] private LDtkFieldType _type;
        [SerializeField] private LDtkFieldElement[] _data;

        public string Identifier => _identifier;
        public bool IsArray => !_isSingle;
        public LDtkFieldType Type => _type;

        public LDtkField(string identifier, LDtkFieldElement[] instances, bool isArray)
        {
            _identifier = identifier;
            _data = instances;
            _isSingle = !isArray;
            _type = _data != null && _data.Length > 0 ? _data.First().Type : LDtkFieldType.None;
        }
        
        public bool GetFieldElementByType(LDtkFieldType type, out LDtkFieldElement element)
        {
            element = _data.FirstOrDefault(e => e.Type == type);
            return element != null;
        }

        public LDtkFieldElement GetSingle()
        {
            if (!ValidateSingle())
            {
                return null;
            }
            
            if (_data.IsNullOrEmpty())
            {
                Debug.LogError("LDtk: Error getting single");
                return null;
            }

            if (_data.Length != 1)
            {
                Debug.LogError("LDtk: Unexpected length when getting single");
                return null;
            }

            return _data[0];
        }
        
        public FieldsResult<LDtkFieldElement[]> GetArray()
        {
            FieldsResult<LDtkFieldElement[]> result = FieldsResult<LDtkFieldElement[]>.Null();
            
            if (!ValidateArray())
            {
                return result;
            }
            
            if (_data == null)
            {
                Debug.LogError("LDtk: Error getting array");
                return result;
            }

            result.Success = true;
            result.Value = _data;
            return result;
        }
        
        public string GetValueAsString()
        {
            if (!ValidateSingle())
            {
                return null;
            }
            
            LDtkFieldElement element = GetSingle();
            return element == null ? string.Empty : element.GetValueAsString();
        }
        public FieldsResult<string[]> GetValuesAsStrings()
        {
            FieldsResult<string[]> result = FieldsResult<string[]>.Null();
            
            if (!ValidateArray())
            {
                return result;
            }

            FieldsResult<LDtkFieldElement[]> resultElements = GetArray();
            if (resultElements.Success)
            {
                LDtkFieldElement[] elements = resultElements.Value; 
                result.Value = elements.Select(p => p.GetValueAsString()).ToArray();
                result.Success = true;
            }

            return result;
        }

        public bool IsSingleNull()
        {
            if (!ValidateSingle())
            {
                return true;
            }
            
            LDtkFieldElement element = GetSingle();
            return element == null || element.IsNull();
        }
        
        public bool IsArrayElementNull(int index)
        {
            if (!ValidateArray())
            {
                return true;
            }

            FieldsResult<LDtkFieldElement[]> result = GetArray();
            LDtkFieldElement[] elements = result.Value; 
            if (elements.IsNullOrEmpty())
            {
                return true;
            }
            
            bool outOfBounds = index < 0 || index >= elements.Length;
            if (outOfBounds)
            {
                Debug.LogError($"LDtk: Out of range when checking if an array's element index {index} was null for \"{_identifier}\"");
                return true;
            }

            LDtkFieldElement element = elements[index];
            return element == null || element.IsNull();
        }
        
        private bool ValidateSingle()
        {
            if (_isSingle)
            {
                return true;
            }
            
            Debug.LogError($"LDtk: Tried accessing a single value when \"{_identifier}\" is an array");
            return false;

        }
        private bool ValidateArray()
        {
            if (IsArray)
            {
                return true;
            }
            
            Debug.LogError($"LDtk: Tried accessing an array when \"{_identifier}\" is a single value");
            return false;
        }
    }
}