using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity
{
    [Serializable]
    public class LDtkFieldElement
    {
        internal const string PROPERTY_TYPE = nameof(_type);
        internal const string PROPERTY_CAN_NULL = nameof(_canBeNull);
        internal const string PROPERTY_NULL = nameof(_isNotNull);
        
        internal const string PROPERTY_INT = nameof(_int);
        internal const string PROPERTY_FLOAT = nameof(_float);
        internal const string PROPERTY_BOOL = nameof(_bool);
        internal const string PROPERTY_STRING = nameof(_string);
        internal const string PROPERTY_COLOR = nameof(_color);
        internal const string PROPERTY_VECTOR2 = nameof(_vector2);
        internal const string PROPERTY_SPRITE = nameof(_sprite);
        internal const string PROPERTY_OBJ = nameof(_obj);
        internal const string PROPERTY_ENTITY_REF = nameof(_entityRef);
        internal const string PROPERTY_MIN = nameof(_min);
        internal const string PROPERTY_MAX = nameof(_max);

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
        /// <param name="def"></param>
        internal LDtkFieldElement(object obj, FieldDefinition def)
        {
            _type = GetTypeForInstance(def);
            _canBeNull = def.CanBeNull;
            _isNotNull = true;
            _min = def.Min ?? float.NaN;
            _max = def.Max ?? float.NaN;
            
            if (obj == null)
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

        internal void SetPointLocalTransform(Transform trans)
        {
            Debug.Assert(_type == LDtkFieldType.Point);
            trans.position = _vector2;
            _obj = trans;
        }

        private LDtkFieldType GetTypeForInstance(FieldDefinition def)
        {
            if (def.IsInt) return LDtkFieldType.Int;
            if (def.IsFloat) return LDtkFieldType.Float;
            if (def.IsBool) return LDtkFieldType.Bool;
            if (def.IsString) return LDtkFieldType.String;
            if (def.IsMultilines) return LDtkFieldType.Multiline;
            if (def.IsFilePath) return LDtkFieldType.FilePath;
            if (def.IsColor) return LDtkFieldType.Color;
            if (def.IsEnum) return LDtkFieldType.Enum;
            if (def.IsPoint) return LDtkFieldType.Point;
            if (def.IsEntityRef) return LDtkFieldType.EntityRef;
            if (def.IsTile) return LDtkFieldType.Tile;
            return LDtkFieldType.None;
        }
        
        internal int TryGetInt(out bool success) => ValidateValue(_int, LDtkFieldType.Int, out success);
        public int GetInt() => ValidateValue(_int, LDtkFieldType.Int);
        
        internal float TryGetFloat(out bool success) => ValidateValue(_float, LDtkFieldType.Float, out success);
        public float GetFloat() => ValidateValue(_float, LDtkFieldType.Float);
        
        internal bool TryGetBool(out bool success) => ValidateValue(_bool, LDtkFieldType.Bool, out success);
        public bool GetBool() => ValidateValue(_bool, LDtkFieldType.Bool);
        
        internal string TryGetString(out bool success) => ValidateValue(_string, LDtkFieldType.String, out success);
        public string GetString() => ValidateValue(_string, LDtkFieldType.String);
        
        internal string TryGetMultiline(out bool success) => ValidateValue(_string, LDtkFieldType.Multiline, out success);
        public string GetMultiline() => ValidateValue(_string, LDtkFieldType.Multiline);
        
        internal string TryGetFilePath(out bool success) => ValidateValue(_string, LDtkFieldType.FilePath, out success);
        public string GetFilePath() => ValidateValue(_string, LDtkFieldType.FilePath);
        
        internal Color TryGetColor(out bool success) => ValidateValue(_color, LDtkFieldType.Color, out success);
        public Color GetColor() => ValidateValue(_color, LDtkFieldType.Color);
        
        internal TEnum TryGetEnum<TEnum>(out bool success) where TEnum : struct
        {
            // For enums, we do a runtime process in order to work around the fact that enums need to compile 
            ValidateValue(_string, LDtkFieldType.Enum, out success);
            if (!success) return default;

            Type type = typeof(TEnum);
            if (!type.IsEnum)
            {
                LDtkDebug.LogError($"Input type {type.Name} is not an enum");
                return default;
            }

            //safely null result
            if (IsNull())
            {
                success = true;
                return default;
            }

            if (Enum.IsDefined(type, _string))
            {
                success = true;
                return (TEnum)Enum.Parse(type, _string);
            }
                
            //ERROR situation. prep debug message
            Array values = Enum.GetValues(typeof(TEnum));
            List<string> stringValues = new List<string>();
            foreach (object obj in values)
            {
                string stringValue = Convert.ToString(obj);
                stringValues.Add(stringValue);
            }
            string joined = string.Join("\", \"", stringValues);

            LDtkDebug.LogError($"C# enum \"{type.Name}\" does not define enum value \"{_string}\". Possible values are \"{joined}\"");
            return default;
        }
        public TEnum GetEnum<TEnum>() where TEnum : struct => TryGetEnum<TEnum>(out bool _);

        internal Transform TryGetPointTransform(out bool success)
        {
            ValidateValue(_vector2, LDtkFieldType.Point, out success);
            if (!success) return null;

            if (_obj is Transform transform)
            {
                success = true;
                return transform;
            }

            //it's okay to return null
            return null;
        }
        public Transform GetPointTransform() => TryGetPointTransform(out bool _);
        
        internal Vector2 TryGetPoint(out bool success)
        {
            ValidateValue(_vector2, LDtkFieldType.Point, out success);
            if (!success) return default;

            if (_obj is Transform transform)
            {
                return transform.position;
            }
            //it's okay if the transform doesn't exist, it just means that it's always the world position
            return _vector2;
        }
        public Vector2 GetPoint() => TryGetPoint(out bool _);
        
        internal LDtkReferenceToAnEntityInstance TryGetEntityReference(out bool success) => ValidateValue(_entityRef, LDtkFieldType.EntityRef, out success);
        public LDtkReferenceToAnEntityInstance GetEntityReference() => TryGetEntityReference(out bool _);
        
        public Sprite TryGetTile(out bool success) => ValidateValue(_sprite, LDtkFieldType.Tile, out success);
        public Sprite GetTile() => TryGetTile(out bool _);

        /// <summary>
        /// This pass helps protects against getting the wrong type for a certain field identifier
        /// </summary>
        private T ValidateValue<T>(in T data, LDtkFieldType type, out bool success)
        {
            success = _type == type;
            LDtkDebug.Assert(success, $"Trying to get improper type \"{type}\" instead of \"{_type}\"");
            return data;
            
            //an exception to the matching rule, multilines implementation is an advanced setting option in LDtk
            //EDIT: for now, we are going to disable this just to make the tests pass properly because this is too niche
            /*if (_type == LDtkFieldType.String && type == LDtkFieldType.Multiline)
            {value
                return result;
            }*/
        }
        private T ValidateValue<T>(in T data, LDtkFieldType type)
        {
            bool success = _type == type;
            LDtkDebug.Assert(success, $"Trying to get improper type \"{type}\" instead of \"{_type}\"");
            return data;
        }

        internal bool IsOfType(LDtkFieldType type)
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

        //if it's actually null or nullable
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