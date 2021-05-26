using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    [Serializable] 
    public class LDtkField
    {
        public const string PROP_IDENTIFIER = nameof(_identifier);
        public const string PROP_DATA = nameof(_data);
        public const string PROP_SINGLE = nameof(_isSingle);

        [SerializeField] private string _identifier;
        [SerializeField] private LDtkFieldElement[] _data;
        [SerializeField] private bool _isSingle;
        [SerializeField] private LDtkFieldType _type;

        public string Identifier => _identifier;
        public bool IsArray => !_isSingle;
        public LDtkFieldType Type => _type;

        public LDtkField(string identifier, LDtkFieldElement[] instances, bool isSingle)
        {
            _identifier = identifier;
            _data = instances;
            _isSingle = isSingle;
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
                Debug.LogError($"LDtk: Tried accessing a a single value when \"{_identifier}\" is an array");
            }
            
            if (_data == null)
            {
                Debug.LogError("LDtk: Error getting array");
                return null;
            }
            
            return _data;
        }
    }
}