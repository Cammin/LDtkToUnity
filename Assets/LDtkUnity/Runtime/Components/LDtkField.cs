using System;
using System.Reflection;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable]
    public class LDtkField
    {
        public int _int = default;
        public float _float = default;
        public bool _bool = default;
        public string _string = default;
        public string _multiLine = default;
        public string _enum = default;
        public Color _color = default;
        public Vector2 _point = default;
        public string _filePath = default;




        private FieldInfo GetField(string identifier)
        {
            FieldInfo fieldInfo = GetType().GetField(identifier);
            if (fieldInfo != null)
            {
                return fieldInfo;
            }
            
            Debug.LogError("FieldInfo Problem");
            return null;
        }
    }
}