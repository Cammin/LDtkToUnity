using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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

        private Dictionary<string, int> _keys;
        
        public LDtkField[] Fields => _fields;

        private void Awake()
        {
            CacheFields();
        }
        
        internal void CacheFields()
        {
            _keys = new Dictionary<string, int>(_fields.Length);
            for (int i = 0; i < _fields.Length; i++)
            {
                _keys.Add(_fields[i].Identifier, i);
            }
        }

        internal void SetFieldData(LDtkField[] fields)
        {
            _fields = fields;
        }

        /// <summary>
        /// Gets a field. Use this to then call GetSingle() or GetArray() on the field.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        [PublicAPI]
        public LDtkField GetField(string identifier)
        {
            return TryGetField(identifier, out var value) ? value : null;
        }
        
        [PublicAPI]
        public bool TryGetField(string identifier, out LDtkField field)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                field = default;
                return false;
            }
            
            if (_keys != null && _keys.ContainsKey(identifier))
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

            bool success = field.TryGetSingle(out LDtkFieldElement element);
            if (!success)
            {
                return false;
            }
            
            value = selector.Invoke(element, out success);
            if (log && !success)
            {
                LDtkDebug.LogError($"Failed to get field \"{identifier}\"");
            }
            
            return success;
        }
        
        private T[] GetFieldArray<T>(string identifier, LDtkFieldType type, LDtkElementSelector<T> selector)
        {
            TryGetFieldArray(identifier, type, selector, out T[] value, true);
            return value;
        }
        
        private bool TryGetFieldArray<T>(string identifier, LDtkFieldType type, LDtkElementSelector<T> selector, out T[] value, bool log = false)
        {
            value = null;
            
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
            
            bool success = field.TryGetArray(out var elements);
            if (!success)
            {
                if (log)
                {
                    LDtkDebug.LogError($"Failed to get array field \"{identifier}\"");
                }

                return false;
            }

            if (elements.Any(p => !p.IsOfType(type)))
            {
                if (log)
                {
                    LDtkDebug.LogError($"Array element types does not match, they were not C# type \"{typeof(T).Name}\"");
                }

                return false;
            }

            bool[] successes = new bool[elements.Length];
            value = new T[elements.Length];

            for (int i = 0; i < elements.Length; i++)
            {
                LDtkFieldElement element = elements[i];
                value[i] = selector.Invoke(element, out successes[i]);
            }

            return successes.All(b => b);
        }
    }
}