using System;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable] 
    public class LDtkField
    {
        [SerializeField] private string _identifier;
        [SerializeField] private LDtkFieldElement[] _data;

        public string Identifier => _identifier;
        
        public LDtkField(string identifier, LDtkFieldElement[] instances)
        {
            _identifier = identifier;
            _data = instances;
        }

        public LDtkFieldElement GetSingle()
        {
            if (_data.IsNullOrEmpty())
            {
                Debug.LogError("Error getting single");
                return null;
            }

            if (_data.Length != 1)
            {
                Debug.LogError("Unexpected length when getting single");
                return null;
            }

            return _data[0];
        }
        public LDtkFieldElement[] GetArray()
        {
            if (_data == null)
            {
                Debug.LogError("Error getting array");
                return null;
            }
            
            return _data;
        }
    }
}