using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Internal;

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
        [SerializeField] private LDtkField[] _fields;
        
        private readonly Dictionary<string, int> _keys = new Dictionary<string, int>();

        private void Awake()
        {
            CacheFields();
        }
        
        private void CacheFields()
        {
            for (int i = 0; i < _fields.Length; i++)
            {
                LDtkField field = _fields[i];
                _keys.Add(field.Identifier, i);
            }
        }

        internal void SetFieldData(LDtkField[] fields)
        {
            _fields = fields;
        }
        
        internal bool TryGetField(string identifier, out LDtkField field)
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

        private T GetFieldSingle<T>(string identifier, LDtkElementSelector<T> selector)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                GameObject obj = gameObject;
                Debug.LogError($"LDtk: No field \"{identifier}\" exists in this field component for {obj.name}", obj);
                return default;
            }
            
            LDtkFieldElement element = field.GetSingle();
            if (element == null)
            {
                return default;
            }

            FieldsResult<T> result = selector.Invoke(element);
            if (!result.Success)
            {
                Debug.LogError("LDtk: Failed to get field");
            }
            
            return result.Value;
        }
        

        private T[] GetFieldArray<T>(string identifier, LDtkElementSelector<T> selector)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                GameObject obj = gameObject;
                Debug.LogError($"LDtk: No array field \"{identifier}\" exists in this field component for {obj.name}", obj);
                return Array.Empty<T>();
            }


            FieldsResult<LDtkFieldElement[]> result = field.GetArray();
            if (!result.Success)
            {
                return Array.Empty<T>();
            }
            
            LDtkFieldElement[] elements = result.Value;
            T[] value = new T[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                FieldsResult<T> response = selector.Invoke(elements[i]);
                value[i] = response.Value;
            }
            
            return value;
        }
        
        private bool TryGetFieldSingle<T>(string identifier, LDtkElementSelector<T> selector, out T value)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                value = default;
                return false;
            }
            
            LDtkFieldElement element = field.GetSingle();
            if (element == null)
            {
                value = default;
                return false;
            }

            FieldsResult<T> r = selector.Invoke(element);
            value = r.Value;
            return r.Success;
        }
        private bool TryGetFieldArray<T>(string identifier, LDtkElementSelector<T> selector, out T[] value)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                value = default;
                return false;
            }

            FieldsResult<LDtkFieldElement[]> result = field.GetArray();
            if (!result.Success)
            {
                value = default;
                return false;
            }

            LDtkFieldElement[] elements = result.Value;

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