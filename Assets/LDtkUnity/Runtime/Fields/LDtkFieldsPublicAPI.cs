using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    public partial class LDtkFields
    {
        #region Int
        
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
        /// Gets an int field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetInt(string identifier, out int value) => TryGetFieldSingle(identifier, element => element.GetIntValue(), out value);

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
        /// Gets an int field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetIntArray(string identifier, out int[] values) => TryGetFieldArray(identifier, element => element.GetIntValue(), out values);

        #endregion
        #region Float
        
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
        /// Gets a float field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetFloat(string identifier, out float value) => TryGetFieldSingle(identifier, element => element.GetFloatValue(), out value);
        
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
        /// Gets a float field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetFloatArray(string identifier, out float[] values) => TryGetFieldArray(identifier, element => element.GetFloatValue(), out values);
        
        #endregion
        #region Bool
        
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
        /// Gets a bool field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetBool(string identifier, out bool value) => TryGetFieldSingle(identifier, element => element.GetBoolValue(), out value);
        
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
        /// Gets a bool field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetBoolArray(string identifier, out bool[] values) => TryGetFieldArray(identifier, element => element.GetBoolValue(), out values);
        
        #endregion
        #region String
        
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
        /// Gets a string field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetString(string identifier, out string value) => TryGetFieldSingle(identifier, element => element.GetStringValue(), out value);
        
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
        /// Gets a string field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetStringArray(string identifier, out string[] values) => TryGetFieldArray(identifier, element => element.GetStringValue(), out values);
        
        #endregion
        #region Multiline
        
        /// <summary>
        /// Gets a multiline field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string GetMultiline(string identifier) => GetFieldSingle(identifier, element => element.GetMultilineValue());
        
        /// <summary>
        /// Gets a multiline field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetMultiline(string identifier, out string value) => TryGetFieldSingle(identifier, element => element.GetMultilineValue(), out value);
        
        /// <summary>
        /// Gets a multiline field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string[] GetMultilineArray(string identifier) => GetFieldArray(identifier, element => element.GetMultilineValue());
        
        /// <summary>
        /// Gets a multiline field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetMultilineArray(string identifier, out string[] values) => TryGetFieldArray(identifier, element => element.GetMultilineValue(), out values);
        
        #endregion
        #region FilePath
        
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
        /// Gets a file path field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetFilePath(string identifier, out string value) => TryGetFieldSingle(identifier, element => element.GetFilePathValue(), out value);
        
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
        /// Gets a file path field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetFilePathArray(string identifier, out string[] values) => TryGetFieldArray(identifier, element => element.GetFilePathValue(), out values);
        
        #endregion
        #region Color
        
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
        /// Gets a color field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetColor(string identifier, out Color value) => TryGetFieldSingle(identifier, element => element.GetColorValue(), out value);
        
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
        /// Gets a color field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetColorArray(string identifier, out Color[] values) => TryGetFieldArray(identifier, element => element.GetColorValue(), out values);
        
        #endregion
        #region Enum
        
        /// <summary>
        /// Gets a enum field's value. It's encouraged to use the auto-generated scripts with this, but you can also use your own scripts as long as the enum value names match.
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
        /// Gets an enum field's value. It's encouraged to use the auto-generated scripts with this, but you can also use your own scripts as long as the enum value names match.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <typeparam name="TEnum">
        /// The enum type to get.
        /// </typeparam>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetEnum<TEnum>(string identifier, out TEnum value) where TEnum : struct => TryGetFieldSingle(identifier, element => element.GetEnumValue<TEnum>(), out value);
        
        /// <summary>
        /// Gets a enum field's values. It's encouraged to use the auto-generated scripts with this, but you can also use your own scripts as long as the enum value names match.
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
        /// Gets an enum field's values. It's encouraged to use the auto-generated scripts with this, but you can also use your own scripts as long as the enum value names match.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetEnumArray<TEnum>(string identifier, out TEnum[] values) where TEnum : struct => TryGetFieldArray(identifier, element => element.GetEnumValue<TEnum>(), out values);
        
        #endregion
        #region Point
        
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
        /// Gets a point field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetPoint(string identifier, out Vector2 value) => TryGetFieldSingle(identifier, element => element.GetPointValue(), out value);
        
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
        
        /// <summary>
        /// Gets a point field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetPointArray(string identifier, out Vector2[] values) => TryGetFieldArray(identifier, element => element.GetPointValue(), out values);

        
        #endregion
        #region EntityRef
        
        /// <summary>
        /// Gets an entity reference field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        /// <remarks>
        /// This function uses Object.FindObjectsOfType if a cached component is not found, so it is slow and not recommended to use every frame.
        /// </remarks>
        public GameObject GetEntityReference(string identifier)
        {
            string iid = GetFieldSingle(identifier, element => element.GetEntityRefValue());
            if (iid == null)
            {
                return null;
            }

            LDtkIid iidComponent = LDtkIidComponentBank.FindObjectOfIid(iid);
            if (iidComponent == null)
            {
                return null;
            }

            return iidComponent.gameObject;
        }
        
        /// <summary>
        /// Gets an entity reference field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetEntityReference(string identifier, out GameObject value)
        {
            value = null;
            if (!TryGetFieldSingle(identifier, element => element.GetEntityRefValue(), out string iid))
            {
                return false;
            }

            LDtkIid iidComponent = LDtkIidComponentBank.FindObjectOfIid(iid);
            if (iidComponent == null)
            {
                return true; //returning true because it is only if the iid field exists
            }

            value = iidComponent.gameObject;
            return true;
        }

        /// <summary>
        /// Gets an entity reference field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        /// <remarks>
        /// This function uses Object.FindObjectsOfType if a cached component is not found, so it is slow and not recommended to use every frame.
        /// </remarks>
        public GameObject[] GetEntityReferenceArray(string identifier)
        {
            string[] iids = GetFieldArray(identifier, element => element.GetEntityRefValue());
            return iids?.Select(LDtkIidComponentBank.FindObjectOfIid).Select(p => p == null ? null : p.gameObject).ToArray();
        }
        
        /// <summary>
        /// Gets an enum field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetEntityReferenceArray(string identifier, out GameObject[] values)
        {
            if (TryGetFieldArray(identifier, element => element.GetEntityRefValue(), out string[] iids))
            {
                values = iids.Select(LDtkIidComponentBank.FindObjectOfIid).Select(p => p == null ? null : p.gameObject).ToArray();
                return true;
            }

            values = null;
            return false;
        }

        #endregion
        #region Tile
        
        /// <summary>
        /// Gets a tile field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Sprite GetTile(string identifier) => GetFieldSingle(identifier, element => element.GetTileValue());

        /// <summary>
        /// Gets a tile field's value.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetTile(string identifier, out Sprite value) => TryGetFieldSingle(identifier, element => element.GetTileValue(), out value);
        
        /// <summary>
        /// Gets a tile reference field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Sprite[] GetTileArray(string identifier) => GetFieldArray(identifier, element => element.GetTileValue());

        /// <summary>
        /// Gets a tile field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="values">
        /// The field's values.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetTileArray(string identifier, out Sprite[] values) => TryGetFieldArray(identifier, element => element.GetTileValue(), out values);

        #endregion
        #region FieldAsString
        
        /// <summary>
        /// Returns a field's value as a string. This is type-agnostic.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value as a string.
        /// </returns>
        public string GetValueAsString(string identifier)
        {
            if (TryGetField(identifier, out LDtkField field))
            {
                return field.GetValueAsString();
            }
            
            GameObject obj = gameObject;
            Debug.LogError($"LDtk: No field \"{identifier}\" exists in this field component for {obj.name}", obj);
            return string.Empty;
        }

        /// <summary>
        /// Returns a field's value as a string. This is type-agnostic.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's value as a string.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetValueAsString(string identifier, out string value)
        {
            if (TryGetField(identifier, out LDtkField field))
            {
                value = field.GetValueAsString();
                return true;
            }

            value = null;
            return false;
        }
        
        /// <summary>
        /// Returns a field's values as strings. This is type-agnostic.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's values as strings.
        /// </returns>
        public string[] GetValuesAsStrings(string identifier)
        {
            if (TryGetField(identifier, out LDtkField field))
            {
                return field.GetValuesAsStrings();
            }
            
            GameObject obj = gameObject;
            Debug.LogError($"LDtk: No field \"{identifier}\" exists in this field component for {obj.name}", obj);
            return Array.Empty<string>();
        }
        
        /// <summary>
        /// Returns a field's values as strings. This is type-agnostic.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <param name="value">
        /// The field's values as strings.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool TryGetValuesAsStrings(string identifier, out string[] value)
        {
            if (TryGetField(identifier, out LDtkField field))
            {
                value = field.GetValuesAsStrings();
                return true;
            }

            value = null;
            return false;
        }

        #endregion
        #region Misc
        
        /// <summary>
        /// Used to check if a field exists in this component.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// If the field exists.
        /// </returns>
        public bool ContainsField(string identifier)
        {
            //a more optimized way to check if it exists in update loops so that we avoid using linq when possible
            if (_keys.Contains(identifier))
            {
                return true;
            }

            return _fields.Any(p => p.Identifier == identifier);
        }
        
        [Obsolete("Use EntityInstance.UnitySmartColor instead.")]
        public bool GetSmartColor(out Color firstColor)
        {
            Debug.LogWarning("LDtk: Getting smart color is deprecated.");
            firstColor = Color.white;
            return true;
        }
        
        #endregion
    }
}