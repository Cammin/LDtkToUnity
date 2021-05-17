using System;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable] 
    public class LDtkField
    {
        public const string PROP_IDENTIFIER = nameof(_identifier);
        public const string PROP_DATA = nameof(_data);
        public const string PROP_SINGLE = nameof(_isSingle);
        
        [SerializeField] private string _identifier;
        [SerializeField] internal LDtkFieldElement[] _data;
        [SerializeField] private bool _isSingle;

        public string Identifier => _identifier;
        
        public LDtkField(string identifier, LDtkFieldElement[] instances, bool isSingle)
        {
            _identifier = identifier;
            _data = instances;
            _isSingle = isSingle;
        }

        public LDtkFieldElement GetSingle()
        {
            if (!_isSingle)
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
            if (!_isSingle)
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