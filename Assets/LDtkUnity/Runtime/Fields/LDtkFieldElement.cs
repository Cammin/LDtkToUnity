using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable]
    internal class LDtkFieldElement
    {
        public const string PROPERTY_TYPE = nameof(_type);
        public const string PROPERTY_INT = nameof(_int);
        public const string PROPERTY_FLOAT = nameof(_float);
        public const string PROPERTY_BOOL = nameof(_bool);
        public const string PROPERTY_STRING = nameof(_string);
        public const string PROPERTY_COLOR = nameof(_color);
        public const string PROPERTY_VECTOR2 = nameof(_vector2);
        public const string PROPERTY_SPRITE = nameof(_sprite);

        [SerializeField] private LDtkFieldType _type;
        
        [SerializeField] private int _int = 0;
        [SerializeField] private float _float = 0;
        [SerializeField] private bool _bool = false;
        [SerializeField] private string _string = string.Empty;
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private Vector2 _vector2 = Vector2.zero;
        [SerializeField] private Sprite _sprite = null;

        public LDtkFieldType Type => _type;
        
        public LDtkFieldElement(object obj, FieldInstance instance)
        {
            _type = GetTypeForInstance(instance);
            switch (_type)
            {
                case LDtkFieldType.FInt:
                    _int = Convert.ToInt32(obj);
                    break;
                
                case LDtkFieldType.FFloat:
                    _float = (float)obj;
                    break;
                
                case LDtkFieldType.FBool:
                    _bool = (bool)obj;
                    break;
                
                case LDtkFieldType.FString:
                case LDtkFieldType.FText:
                case LDtkFieldType.FPath:
                case LDtkFieldType.FEnum:
                case LDtkFieldType.FEntityRef:
                    _string = (string)obj;
                    break;
                
                case LDtkFieldType.FColor:
                    _color = (Color)obj;
                    break;
                case LDtkFieldType.FPoint:
                    _vector2 = (Vector2)obj;
                    break;
                
                case LDtkFieldType.FTile:
                    _sprite = (Sprite)obj;
                    break;
            }

        }

        private LDtkFieldType GetTypeForInstance(FieldInstance instance)
        {
            if (instance.IsInt) return LDtkFieldType.FInt;
            if (instance.IsFloat) return LDtkFieldType.FFloat;
            if (instance.IsBool) return LDtkFieldType.FBool;
            if (instance.IsString) return LDtkFieldType.FString;
            if (instance.IsMultilines) return LDtkFieldType.FText;
            if (instance.IsFilePath) return LDtkFieldType.FPath;
            if (instance.IsColor) return LDtkFieldType.FColor;
            if (instance.IsEnum) return LDtkFieldType.FEnum;
            if (instance.IsPoint) return LDtkFieldType.FPoint;
            if (instance.IsEntityRef) return LDtkFieldType.FEntityRef;
            if (instance.IsTile) return LDtkFieldType.FTile;
            return LDtkFieldType.None;
        }
        
        public int GetIntValue() => GetData(_int, LDtkFieldType.FInt);
        public float GetFloatValue() => GetData(_float, LDtkFieldType.FFloat);
        public bool GetBoolValue() => GetData(_bool, LDtkFieldType.FBool);
        public string GetStringValue() => GetData(_string, LDtkFieldType.FString);
        public string GetMultilineValue() => GetData(_string, LDtkFieldType.FText);
        public string GetFilePathValue() => GetData(_string, LDtkFieldType.FPath);
        public Color GetColorValue() => GetData(_color, LDtkFieldType.FColor);
        public TEnum GetEnumValue<TEnum>() where TEnum : struct
        {
            if (string.IsNullOrEmpty(_string))
            {
                return default;
            }
            
            // For enums, we do a runtime process in order to work around the fact that enums need to compile 
            string data = GetData(_string, LDtkFieldType.FEnum);
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

            if (Enum.IsDefined(type, _string))
            {
                return (TEnum)Enum.Parse(type, _string);
            }
            
            Array values = Enum.GetValues(typeof(TEnum));
            List<string> stringValues = new List<string>();
            foreach (object value in values)
            {
                string stringValue = Convert.ToString(value);
                stringValues.Add(stringValue);
            }
            string joined = string.Join("\", \"", stringValues);

            Debug.LogError($"LDtk: C# enum \"{type.Name}\" does not define enum value \"{_string}\". Possible values are \"{joined}\"");
            return default;
        }
        public Vector2 GetPointValue() => GetData(_vector2, LDtkFieldType.FPoint);
        public string GetEntityRefValue() => GetData(_string, LDtkFieldType.FEntityRef);
        public Sprite GetTileValue() => GetData(_sprite, LDtkFieldType.FTile);

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