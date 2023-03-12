using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity
{
    [Serializable]
    internal class LDtkFieldElement
    {
        public const string PROPERTY_TYPE = nameof(_type);
        public const string PROPERTY_CAN_NULL = nameof(_canBeNull);
        public const string PROPERTY_NULL = nameof(_isNotNull);
        
        public const string PROPERTY_INT = nameof(_int);
        public const string PROPERTY_FLOAT = nameof(_float);
        public const string PROPERTY_BOOL = nameof(_bool);
        public const string PROPERTY_STRING = nameof(_string);
        public const string PROPERTY_COLOR = nameof(_color);
        public const string PROPERTY_VECTOR2 = nameof(_vector2);
        public const string PROPERTY_SPRITE = nameof(_sprite);
        public const string PROPERTY_OBJ = nameof(_obj);
        public const string PROPERTY_ENTITY_REF = nameof(_entityRef);
        public const string PROPERTY_MIN = nameof(_min);
        public const string PROPERTY_MAX = nameof(_max);

        [SerializeField] private LDtkFieldType _type;
        [SerializeField] private bool _canBeNull;
        [SerializeField] private bool _isNotNull;
        [SerializeField] private float _min;
        [SerializeField] private float _max;
        
        [SerializeField] private int _int = 0;
        [SerializeField] private float _float = 0;
        [SerializeField] private bool _bool = false;
        [SerializeField] private string _string = string.Empty;
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private Vector2 _vector2 = Vector2.zero;
        [SerializeField] private Sprite _sprite = null;
        [SerializeField] private Object _obj = null;
        [SerializeField] private LDtkReferenceToAnEntityInstance _entityRef = null;

        public LDtkFieldType Type => _type;

        /// <param name="obj">This obj is already passed by a parsed interface</param>
        /// <param name="instance"></param>
        public LDtkFieldElement(object obj, FieldInstance instance)
        {
            FieldDefinition def = instance.Definition;
            
            _type = GetTypeForInstance(instance);
            _canBeNull = def.CanBeNull;
            _isNotNull = true;
            _min = def.Min ?? float.NaN;
            _max = def.Max ?? float.NaN;
            
            if (Equals(obj, default))
            {
                if (_canBeNull)
                {
                    _isNotNull = false;
                }

                //we don't complain about non-nullable values being null because it's a natural part of the process. In LDtk, it warns you but doesn't enforce you to assign values
                //Debug.LogError($"LDtk: An object was null but was not set as nullable for a field: {instance.Identifier}");
                return;
            }

            switch (_type)
            {
                case LDtkFieldType.Int:
                    _int = Convert.ToInt32(obj);
                    break;
                
                case LDtkFieldType.Float:
                    _float = Convert.ToSingle(obj);
                    break;
                
                case LDtkFieldType.Bool:
                    _bool = Convert.ToBoolean(obj);
                    break;
                
                case LDtkFieldType.String:
                case LDtkFieldType.Multiline:
                case LDtkFieldType.FilePath:
                case LDtkFieldType.Enum:
                    _string = Convert.ToString(obj);
                    break;
                
                case LDtkFieldType.EntityRef:
                    _entityRef = obj as LDtkReferenceToAnEntityInstance;
                    break;
                
                case LDtkFieldType.Color:
                    _color = (Color)obj;
                    break;
                case LDtkFieldType.Point:
                    _vector2 = (Vector2)obj;
                    break;
                
                case LDtkFieldType.Tile:
                    _sprite = (Sprite)obj;
                    break;
            }
        }

        public void SetPointLocalTransform(Transform trans)
        {
            Debug.Assert(_type == LDtkFieldType.Point);
            trans.position = _vector2;
            _obj = trans;
        }

        private LDtkFieldType GetTypeForInstance(FieldInstance instance)
        {
            if (instance.IsInt) return LDtkFieldType.Int;
            if (instance.IsFloat) return LDtkFieldType.Float;
            if (instance.IsBool) return LDtkFieldType.Bool;
            if (instance.IsString) return LDtkFieldType.String;
            if (instance.IsMultilines) return LDtkFieldType.Multiline;
            if (instance.IsFilePath) return LDtkFieldType.FilePath;
            if (instance.IsColor) return LDtkFieldType.Color;
            if (instance.IsEnum) return LDtkFieldType.Enum;
            if (instance.IsPoint) return LDtkFieldType.Point;
            if (instance.IsEntityRef) return LDtkFieldType.EntityRef;
            if (instance.IsTile) return LDtkFieldType.Tile;
            return LDtkFieldType.None;
        }

        public FieldsResult<int> GetIntValue() => GetData(_int, LDtkFieldType.Int);
        public FieldsResult<float> GetFloatValue() => GetData(_float, LDtkFieldType.Float);
        public FieldsResult<bool> GetBoolValue() => GetData(_bool, LDtkFieldType.Bool);
        public FieldsResult<string> GetStringValue() => GetData(_string, LDtkFieldType.String);
        public FieldsResult<string> GetMultilineValue() => GetData(_string, LDtkFieldType.Multiline);
        public FieldsResult<string> GetFilePathValue() => GetData(_string, LDtkFieldType.FilePath);
        public FieldsResult<Color> GetColorValue() => GetData(_color, LDtkFieldType.Color);
        public FieldsResult<TEnum> GetEnumValue<TEnum>() where TEnum : struct
        {
            FieldsResult<TEnum> result = FieldsResult<TEnum>.Null();
            
            // For enums, we do a runtime process in order to work around the fact that enums need to compile 
            FieldsResult<string> data = GetData(_string, LDtkFieldType.Enum);
            if (!data.Success)
            {
                return result;
            }

            Type type = typeof(TEnum);
            if (!type.IsEnum)
            {
                LDtkDebug.LogError($"Input type {type.Name} is not an enum");
                return result;
            }

            if (IsNull())
            {
                result.Success = true;
                return result;
            }

            if (Enum.IsDefined(type, _string))
            {
                result.Value = (TEnum)Enum.Parse(type, _string);
                result.Success = true;
                return result;
            }
                
            Array values = Enum.GetValues(typeof(TEnum));
            List<string> stringValues = new List<string>();
            foreach (object value in values)
            {
                string stringValue = Convert.ToString(value);
                stringValues.Add(stringValue);
            }
            string joined = string.Join("\", \"", stringValues);

            LDtkDebug.LogError($"C# enum \"{type.Name}\" does not define enum value \"{_string}\". Possible values are \"{joined}\"");
            return result;
        }
        public FieldsResult<Vector2> GetPointValue()
        {
            FieldsResult<Vector2> result = GetData(_vector2, LDtkFieldType.Point);
            if (!result.Success)
            {
                return result;
            }
            if (_obj is Transform transform)
            {
                result.Value = transform.position;
            }
            //it's okay if the transform doesn't exist, it just means that it's always the world position
            return result;
        }

        public FieldsResult<LDtkReferenceToAnEntityInstance> GetEntityRefValue() => GetData(_entityRef, LDtkFieldType.EntityRef);
        public FieldsResult<Sprite> GetTileValue() => GetData(_sprite, LDtkFieldType.Tile);

        /// <summary>
        /// This pass helps protects against getting the wrong type for a certain field identifier
        /// </summary>
        private FieldsResult<T> GetData<T>(T data, LDtkFieldType type)
        {
            FieldsResult<T> result = new FieldsResult<T>
            {
                Success = true,
                Value = data
            };

            if (_type == type)
            {
                return result;
            }
            
            //an exception to the matching rule, multilines implementation is an advanced setting option in LDtk
            //EDIT: for now, we are going to disable this just to make the tests pass properly because this is too niche
            /*if (_type == LDtkFieldType.String && type == LDtkFieldType.Multiline)
            {
                return result;
            }*/

            LDtkDebug.LogError($"Trying to get improper type \"{type}\" instead of \"{_type}\"");

            result.Success = false;//IsNull();
            result.Value = default;
            return result;
        }

        public bool IsOfType(LDtkFieldType type)
        {
            return _type == type;
        }

        public string GetValueAsString()
        {
            switch (_type)
            {
                case LDtkFieldType.Int:
                    return _int.ToString();

                case LDtkFieldType.Float:
                    return _float.ToString(CultureInfo.CurrentCulture);

                case LDtkFieldType.Bool:
                    return _bool.ToString().ToLower();

                case LDtkFieldType.String:
                case LDtkFieldType.Multiline:
                case LDtkFieldType.FilePath:
                case LDtkFieldType.Enum:
                case LDtkFieldType.EntityRef:
                    return _string;

                case LDtkFieldType.Color:
                    return _color.ToHex();
                
                case LDtkFieldType.Point:
                    return _vector2.ToString("0.#######");

                case LDtkFieldType.Tile:
                    return _sprite == null ? string.Empty : _sprite.name;
            }

            return "";
        }

        public bool IsNull()
        {
            if (!_canBeNull)
            {
                return false;
            }

            switch (_type)
            {
                case LDtkFieldType.None:
                    return true;
                    
                case LDtkFieldType.Bool: //these are values that are never able to be null from ldtk
                case LDtkFieldType.Color:
                    return false;

                //for our use cases, it's null when the value was set as null from the parsed data types.
                case LDtkFieldType.Int:
                case LDtkFieldType.Float:
                case LDtkFieldType.String:
                case LDtkFieldType.Multiline:
                case LDtkFieldType.FilePath:
                case LDtkFieldType.Enum:
                case LDtkFieldType.Point:
                case LDtkFieldType.EntityRef:
                    return !_isNotNull;
                    

                case LDtkFieldType.Tile: //for any unity engine object references, it will check if the value is actually null instead
                    if (!_isNotNull)
                    {
                        return true;
                    }

                    return _sprite == null;

                default:
                    return false;
            }
        }
    }
}