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
        [SerializeField] private LDtkFieldElement[] _data;
        [SerializeField] private bool _isSingle;
        [SerializeField] private LDtkFieldType _type;

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
            if (IsArray)
            {
                Debug.LogError($"LDtk: Tried accessing an array when \"{_identifier}\" is a single value");
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
        
        public LDtkFieldElement[] GetArray()
        {
            if (_isSingle)
            {
                Debug.LogError($"LDtk: Tried accessing a single value when \"{_identifier}\" is an array");
                return Array.Empty<LDtkFieldElement>();
            }
            
            if (_data == null)
            {
                Debug.LogError("LDtk: Error getting array");
                return Array.Empty<LDtkFieldElement>();
            }
            
            return _data;
        }
        
        public string GetValueAsString()
        {
            LDtkFieldElement element = GetSingle();
            return element == null ? string.Empty : element.GetValueAsString();
        }
        public string[] GetValuesAsStrings()
        {
            LDtkFieldElement[] elements = GetArray();
            return elements.IsNullOrEmpty() ? Array.Empty<string>() : elements.Select(p => p.GetValueAsString()).ToArray();
        }

        public bool IsSingleNull()
        {
            LDtkFieldElement element = GetSingle();
            return element == null || element.IsNull();
        }

        public bool IsArrayElementNull(int index)
        {
            LDtkFieldElement[] elements = GetArray();
            if (elements.Length == 0)
            {
                return true;
            }
            
            if (index < 0 || index >= elements.Length)
            {
                Debug.LogError($"LDtk: Out of range when checking if an array's element index {index} was null for {_identifier}");
                return true;
            }

            LDtkFieldElement element = elements[index];
            return element == null || element.IsNull();
        }
    }
}