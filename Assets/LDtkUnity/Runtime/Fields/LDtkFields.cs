using System;
using System.Linq;
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
    public class LDtkFields : MonoBehaviour
    {
        internal const string PROPERTY_FIELDS = nameof(_fields);
        
        [SerializeField] private LDtkField[] _fields;
        
        #region gets
        
        /// <summary>
        /// Gets an int field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public int GetInt(string identifier) => GetFieldSingle(identifier, element => element.GetIntValue());
        
        /// <summary>
        /// Gets an int field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public int[] GetIntArray(string identifier) => GetFieldArray(identifier, element => element.GetIntValue());

        /// <summary>
        /// Gets a float field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public float GetFloat(string identifier) => GetFieldSingle(identifier, element => element.GetFloatValue());
        
        /// <summary>
        /// Gets a float field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public float[] GetFloatArray(string identifier) => GetFieldArray(identifier, element => element.GetFloatValue());
        
        /// <summary>
        /// Gets a bool field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public bool GetBool(string identifier) => GetFieldSingle(identifier, element => element.GetBoolValue());
        
        /// <summary>
        /// Gets a bool field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public bool[] GetBoolArray(string identifier) => GetFieldArray(identifier, element => element.GetBoolValue());
        
        /// <summary>
        /// Gets a string field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string GetString(string identifier) => GetFieldSingle(identifier, element => element.GetStringValue());
        
        /// <summary>
        /// Gets a string field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string[] GetStringArray(string identifier) => GetFieldArray(identifier, element => element.GetStringValue());
        
        /// <summary>
        /// Gets a multiline field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string GetMultiline(string identifier) => GetFieldSingle(identifier, element => element.GetStringValue()); //todo swap this back once the LDtk type problem is fixed 
        
        /// <summary>
        /// Gets a multiline field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string[] GetMultilineArray(string identifier) => GetFieldArray(identifier, element => element.GetStringValue()); //todo swap this back once the LDtk type problem is fixed 
        
        /// <summary>
        /// Gets a file path field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string GetFilePath(string identifier) => GetFieldSingle(identifier, element => element.GetFilePathValue());
        
        /// <summary>
        /// Gets a file path field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string[] GetFilePathArray(string identifier) => GetFieldArray(identifier, element => element.GetFilePathValue());
        
        /// <summary>
        /// Gets a color field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Color GetColor(string identifier) => GetFieldSingle(identifier, element => element.GetColorValue());
        
        /// <summary>
        /// Gets a color field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Color[] GetColorArray(string identifier) => GetFieldArray(identifier, element => element.GetColorValue());

        /// <summary>
        /// Gets a enum field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <typeparam name="TEnum">
        /// The enum type to get.
        /// </typeparam>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public TEnum GetEnum<TEnum>(string identifier) where TEnum : struct => GetFieldSingle(identifier, element => element.GetEnumValue<TEnum>());
        
        /// <summary>
        /// Gets a enum field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <typeparam name="TEnum">
        /// The enum type to get.
        /// </typeparam>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public TEnum[] GetEnumArray<TEnum>(string identifier) where TEnum : struct => GetFieldArray(identifier, element => element.GetEnumValue<TEnum>());

        /// <summary>
        /// Gets a point field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Vector2 GetPoint(string identifier) => GetFieldSingle(identifier, element => element.GetPointValue());
        
        /// <summary>
        /// Gets a point field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Vector2[] GetPointArray(string identifier) => GetFieldArray(identifier, element => element.GetPointValue());
        
        #endregion gets
        
        /// <summary>
        /// Get the first occuring color. Used by entities to decide their color in LDtk.
        /// </summary>
        /// <param name="firstColor">
        /// The returned color. If no color was defined, it will return white.
        /// </param>
        /// <returns>
        /// True if getting the entity's color was a success.
        /// </returns>
        public bool GetSmartColor(out Color firstColor)
        {
            foreach (LDtkField field in _fields)
            {
                if (!field.GetFieldElementByType(LDtkFieldType.Color, out LDtkFieldElement element))
                {
                    continue;
                }
                firstColor = element.GetColorValue();
                return true;
            }
            
            firstColor = Color.white;
            return false;
        }
        
        [ExcludeFromDocs]
        [Obsolete("Deprecated. Use GetSmartColor instead.")]
        public bool GetFirstColor(out Color firstColor) => GetSmartColor(out firstColor);

        /// <summary>
        /// Compares whether a certain field is a specific value type. Indifferent if the type is an array.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="type">
        /// The type to compare for.
        /// </param>
        /// <returns>
        /// True if the field exists and the field type matches with the type parameter.
        /// </returns>
        internal bool IsFieldOfType(string identifier, LDtkFieldType type)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                return false;
            }

            return field.Type == type;
        }

        /// <summary>
        /// Checks if a certain field's type is an array.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// True if the particular field exists and is an array.
        /// </returns>
        internal bool IsFieldArray(string identifier)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                return false;
            }

            return field.IsArray;
        }
        
        internal void SetFieldData(LDtkField[] fields)
        {
            _fields = fields;
        }
        
        private delegate T LDtkElementSelector<out T>(LDtkFieldElement element);

        private T GetFieldSingle<T>(string identifier, LDtkElementSelector<T> selector)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                return default;
            }
            
            LDtkFieldElement element = field.GetSingle();
            return selector.Invoke(element);
        }
        
        private T[] GetFieldArray<T>(string identifier, LDtkElementSelector<T> selector)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                return default;
            }
            
            LDtkFieldElement[] elements = field.GetArray();
            return elements.Select(selector.Invoke).ToArray();
        }

        private bool TryGetField(string identifier, out LDtkField field)
        {
            field = _fields.FirstOrDefault(fld => fld.Identifier == identifier);
            if (field != null)
            {
                return field != null;
            }
            
            GameObject obj = gameObject;
            Debug.LogWarning($"LDtk: No field \"{identifier}\" exists in this field component for {obj.name}", obj);
            return field != null;
        }
    }
}