using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    [Serializable]
    public class LDtkFieldElement
    {
        public const string PROP_TYPE = nameof(_type);
        public const string PROP_INT = nameof(_int);
        public const string PROP_FLOAT = nameof(_float);
        public const string PROP_BOOL = nameof(_bool);
        public const string PROP_STRING = nameof(_string);
        public const string PROP_COLOR = nameof(_color);
        public const string PROP_VECTOR2 = nameof(_vector2);
        
        [SerializeField] private LDtkFieldType _type;
        
        [SerializeField] private int _int = 0;
        [SerializeField] private float _float = 0;
        [SerializeField] private bool _bool = false;
        [SerializeField] private string _string = string.Empty;
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private Vector2 _vector2 = Vector2.zero;

        public LDtkFieldType Type => _type;
        
        public LDtkFieldElement(object obj, FieldInstance instance)
        {
            _type = GetTypeForInstance(instance);
            switch (_type)
            {
                case LDtkFieldType.Int:
                    _int = (int)obj;
                    break;
                
                case LDtkFieldType.Float:
                    _float = (float)obj;
                    break;
                
                case LDtkFieldType.Boolean:
                    _bool = (bool)obj;
                    break;
                
                case LDtkFieldType.String:
                case LDtkFieldType.Multiline:
                case LDtkFieldType.FilePath:
                case LDtkFieldType.Enum:
                    _string = (string) obj;
                    break;
                
                case LDtkFieldType.Color:
                    _color = (Color) obj;
                    break;
                case LDtkFieldType.Point:
                    _vector2 = (Vector2) obj;
                    break;
            }

        }

        private LDtkFieldType GetTypeForInstance(FieldInstance instance)
        {
            if (instance.IsInt) return LDtkFieldType.Int;
            if (instance.IsFloat) return LDtkFieldType.Float;
            if (instance.IsBool) return LDtkFieldType.Boolean;
            if (instance.IsString) return LDtkFieldType.String;
            if (instance.IsMultilines) return LDtkFieldType.Multiline;
            if (instance.IsFilePath) return LDtkFieldType.FilePath;
            if (instance.IsColor) return LDtkFieldType.Color;
            if (instance.IsEnum) return LDtkFieldType.Enum;
            if (instance.IsPoint) return LDtkFieldType.Point;
            return LDtkFieldType.None;
        }
        
        public int GetIntValue() => GetData(_int, LDtkFieldType.Int);
        public float GetFloatValue() => GetData(_float, LDtkFieldType.Float);
        public bool GetBoolValue() => GetData(_bool, LDtkFieldType.Boolean);
        public string GetStringValue() => GetData(_string, LDtkFieldType.String);
        public string GetMultilineValue() => GetData(_string, LDtkFieldType.Multiline);
        public string GetFilePathValue() => GetData(_string, LDtkFieldType.FilePath);
        public Color GetColorValue() => GetData(_color, LDtkFieldType.Color);
        
        /// <summary>
        /// For enums, we do a runtime process in order to work around the fact that enums need to compile 
        /// </summary>
        public TEnum GetEnumValue<TEnum>() where TEnum : struct
        {
            string data = GetData(_string, LDtkFieldType.Enum);
            if (data == default)
            {
                return default;
            }

            Type type = typeof(TEnum);
            if (!type.IsEnum)
            {
                Debug.LogError($"LDtk: Input type {type.Name} is not an enum");
                return default;
            }

            if (!Enum.IsDefined(type, _string))
            {
                Debug.LogError($"LDtk: C# enum \"{type.Name}\" is not a defined enum");
                return default;
            }

            return (TEnum)Enum.Parse(type, _string);
        }
        
        public Vector2 GetPointValue() => GetData(_vector2, LDtkFieldType.Point);

        /// <summary>
        /// This pass helps protects against getting the wrong type for a certain field identifier
        /// </summary>
        private T GetData<T>(T data, LDtkFieldType type)
        {
            if (_type == type)
            {
                return data;
            }
            
            Debug.LogError($"LDtk: Trying to get improper type \"{type}\" instead of \"{_type}\"");
            return default;
        }
    }
}