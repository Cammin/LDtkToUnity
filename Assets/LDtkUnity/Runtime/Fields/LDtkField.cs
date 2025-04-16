using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity
{
    /// <summary>
    /// Encapsulates a field, whether single or array.
    /// </summary>
    [Serializable] 
    public class LDtkField
    {
        internal const string PROPERTY_DATA = nameof(_data);
        internal const string PROPERTY_DEF = nameof(_def);

        [SerializeField] private LDtkDefinitionObjectField _def;
        [SerializeField] private LDtkFieldType _type;
        [SerializeField] internal LDtkFieldElement[] _data;
        
        public LDtkDefinitionObjectField Def => _def;
        public string Identifier => _def.Identifier;
        public bool IsArray => _def.IsArray;
        public LDtkFieldType Type => _type;

        internal LDtkField(LDtkDefinitionObjectField def, LDtkFieldElement[] instances)
        {
            _def = def;
            _data = instances;
            _type = _data != null && _data.Length > 0 ? _data.First().Type : LDtkFieldType.None;
        }

        public LDtkFieldElement GetSingle()
        {
            bool success = TryGetSingle(out var element);
            if (!success)
            {
                LDtkDebug.LogError($"Error getting single element for \"{_def.Identifier}\"");
            }
            return element;
        }
        public LDtkFieldElement[] GetArray()
        {
            bool success = TryGetArray(out var elements);
            if (!success)
            {
                LDtkDebug.LogError($"Error getting array for \"{_def.Identifier}\"");
            }
            return elements;
        }
        
        public bool TryGetSingle(out LDtkFieldElement element)
        {
            element = null;
            
            if (!ValidateSingle())
            {
                return false;
            }
            
            if (_data.IsNullOrEmpty())
            {
                LDtkDebug.LogError("Error getting single");
                return false;
            }

            if (_data.Length != 1)
            {
                LDtkDebug.LogError("Unexpected length when getting single");
                return false;
            }
            
            element = _data[0];
            return true;
        }
        
        public bool TryGetArray(out LDtkFieldElement[] array)
        {
            array = null;
            
            if (!ValidateArray())
            {
                return false;
            }
            
            if (_data == null)
            {
                LDtkDebug.LogError("Error getting array");
                return false;
            }
            
            array = _data;
            return true;
        }
        
        internal string TryGetValueAsString(out bool success)
        {
            success = false;
            if (!ValidateSingle()) return null;
            
            success = TryGetSingle(out var element);
            if (!success) return null;
            
            return element.GetValueAsString();
        }
        public string GetValueAsString()
        {
            if (!ValidateSingle()) return null;
            
            bool success = TryGetSingle(out var element);
            if (!success) return null;
            
            return element.GetValueAsString();
        }
        internal string[] TryGetValuesAsStrings(out bool success)
        {
            success = false;
            if (!ValidateArray()) return null;

            success = TryGetArray(out var elements);
            if (!success) return null;
            
            return elements.Select(p => p.GetValueAsString()).ToArray();
        }
        public string[] GetValuesAsStrings()
        {
            if (!ValidateArray()) return null;

            bool success = TryGetArray(out var elements);
            if (!success) return null;
            
            return elements.Select(p => p.GetValueAsString()).ToArray();
        }

        public bool IsSingleNull()
        {
            if (!ValidateSingle()) return true;

            bool success = TryGetSingle(out var element);
            if (!success)
            {
                return true;
            }
            
            return element.IsNull();
        }
        
        internal bool IsArrayElementNull(int index)
        {
            if (!ValidateArray()) return true;

            var success = TryGetArray(out var elements);
            if (!success)
            {
                return true;
            }
            
            if (elements.IsNullOrEmpty())
            {
                return true;
            }
            
            bool outOfBounds = index < 0 || index >= elements.Length;
            if (outOfBounds)
            {
                LDtkDebug.LogError($"Out of range when checking if an array's element index {index} was null for \"{_def.Identifier}\"");
                return true;
            }

            LDtkFieldElement element = elements[index];
            return element == null || element.IsNull();
        }
        
        private bool ValidateSingle()
        {
            if (_def == null)
            {
                LDtkDebug.LogError("Tried accessing a field when the definition object was null");
                return false;
            }
            
            if (!IsArray) return true;
            
            LDtkDebug.LogError($"Tried accessing a single value when \"{_def.Identifier}\" is an array");
            return false;

        }
        private bool ValidateArray()
        {
            if (_def == null)
            {
                LDtkDebug.LogError("Tried accessing a field when the definition object was null");
                return false;
            }
            
            if (IsArray) return true;

            LDtkDebug.LogError($"Tried accessing an array when \"{_def.Identifier}\" is a single value");
            return false;
        }

        internal bool ValidateElementTypes(LDtkFieldType type, Object ctx)
        {
            if (_type == LDtkFieldType.None)
            {
                return false;
            }
            
            if (type != _type)
            {
                LDtkDebug.LogError($"Tried getting a field \"{_def.Identifier}\" as type \"{type}\" but the field was a \"{_type}\" type instead", ctx);
                return false;
            }
            
            for (int i = 0; i < _data.Length; i++)
            {
                LDtkFieldElement element = _data[i];
                if (element == null)
                {
                    LDtkDebug.LogError("An array element in LDtkField was null", ctx);
                    continue;
                }
                
                if (!element.IsOfType(_type))
                {
                    return false;
                }
            }
            return true;
        }
    }
}