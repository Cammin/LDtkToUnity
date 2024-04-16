using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// This is a component that stores the field instances for entities/levels, Conveniently converted for use in Unity.
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    [HelpURL(LDtkHelpURL.COMPONENT_FIELDS)]
    public partial class LDtkFields : MonoBehaviour
    {
        internal const string PROPERTY_FIELDS = nameof(_fields);
        
        [SerializeField] private LDtkField[] _fields = Array.Empty<LDtkField>();
        
        private readonly Dictionary<string, int> _keys = new Dictionary<string, int>();

        private void Awake()
        {
            CacheFields();
        }
        
        private void CacheFields()
        {
            for (int i = 0; i < _fields.Length; i++)
            {
                _keys.Add(_fields[i].Identifier, i);
            }
        }

        internal void SetFieldData(LDtkField[] fields)
        {
            _fields = fields;
        }

        private bool TryGetField(string identifier, out LDtkField field)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                field = default;
                return false;
            }
            
            if (_keys.ContainsKey(identifier))
            {
                int key = _keys[identifier];
                field = _fields[key];
                return field != null;
            }

            field = _fields.FirstOrDefault(fld => fld.Identifier == identifier);
            return field != null;
        }
        
        internal bool IsFieldOfType(string identifier, LDtkFieldType type)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                return false;
            }

            return field.Type == type;
        }
        
        internal bool IsFieldArray(string identifier)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                return false;
            }

            return field.IsArray;
        }
        
        private T GetFieldSingle<T>(string identifier, LDtkFieldType type, LDtkElementSelector<T> selector)
        {
            TryGetFieldSingle(identifier, type, selector, out T value, true);
            return value;
        }
        
        private bool TryGetFieldSingle<T>(string identifier, LDtkFieldType type, LDtkElementSelector<T> selector, out T value, bool log = false)
        {
            value = default;
            
            if (!TryGetField(identifier, out LDtkField field))
            {
                if (log)
                {
                    GameObject obj = gameObject;
                    LDtkDebug.LogError($"No field \"{identifier}\" exists in this field component for {obj.name}", obj);
                }
                
                return false;
            }
            
            if (!field.ValidateElementTypes(type, gameObject))
            {
                return false;
            }

            FieldsResult<LDtkFieldElement> elementResult = field.GetSingle();
            if (!elementResult.Success)
            {
                return false;
            }

            LDtkFieldElement element = elementResult.Value;
            FieldsResult<T> result = selector.Invoke(element);
            if (log && !result.Success)
            {
                LDtkDebug.LogError($"Failed to get field \"{identifier}\"");
            }
            
            value = result.Value;
            return result.Success;
        }
        
        private T[] GetFieldArray<T>(string identifier, LDtkFieldType type, LDtkElementSelector<T> selector)
        {
            TryGetFieldArray(identifier, type, selector, out T[] value, true);
            return value;
        }
        
        private bool TryGetFieldArray<T>(string identifier, LDtkFieldType type, LDtkElementSelector<T> selector, out T[] value, bool log = false)
        {
            value = Array.Empty<T>();
            
            if (!TryGetField(identifier, out LDtkField field))
            {
                if (log)
                {
                    GameObject obj = gameObject;
                    LDtkDebug.LogError($"No array field \"{identifier}\" exists in this field component for {obj.name}", obj);
                }
                return false;
            }

            if (!field.ValidateElementTypes(type, gameObject))
            {
                return false;
            }
            
            FieldsResult<LDtkFieldElement[]> result = field.GetArray();
            if (!result.Success)
            {
                if (log)
                {
                    LDtkDebug.LogError($"Failed to get array field \"{identifier}\"");
                }

                return false;
            }

            LDtkFieldElement[] elements = result.Value;

            if (elements.Any(p => !p.IsOfType(type)))
            {
                if (log)
                {
                    LDtkDebug.LogError($"Array element types does not match, they were not C# type \"{typeof(T).Name}\"");
                }

                return false;
            }

            bool[] success = new bool[elements.Length];
            value = new T[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                LDtkFieldElement element = elements[i];
                FieldsResult<T> response = selector.Invoke(element);
                value[i] = response.Value;
                success[i] = response.Success;
            }

            return success.All(b => b);
        }
    }
}