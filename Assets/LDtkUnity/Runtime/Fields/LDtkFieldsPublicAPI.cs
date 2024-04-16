using System;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LDtkFields
    {
        public LDtkDefinitionObjectField GetDefinition(string identifier)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                GameObject obj = gameObject;
                LDtkDebug.LogError($"No field \"{identifier}\" exists in this field component for {obj.name}", obj);
                return null;
            }
            return field.Def;
        }
        
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
        public int GetInt(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Int, element => element.GetIntValue());
        
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
        public bool TryGetInt(string identifier, out int value) => TryGetFieldSingle(identifier, LDtkFieldType.Int, element => element.GetIntValue(), out value);

        /// <summary>
        /// Gets an int field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public int[] GetIntArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Int, element => element.GetIntValue());
        
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
        public bool TryGetIntArray(string identifier, out int[] values) => TryGetFieldArray(identifier, LDtkFieldType.Int, element => element.GetIntValue(), out values);

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
        public float GetFloat(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Float, element => element.GetFloatValue());
        
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
        public bool TryGetFloat(string identifier, out float value) => TryGetFieldSingle(identifier, LDtkFieldType.Float, element => element.GetFloatValue(), out value);
        
        /// <summary>
        /// Gets a float field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public float[] GetFloatArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Float, element => element.GetFloatValue());
        
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
        public bool TryGetFloatArray(string identifier, out float[] values) => TryGetFieldArray(identifier, LDtkFieldType.Float, element => element.GetFloatValue(), out values);
        
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
        public bool GetBool(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Bool, element => element.GetBoolValue());
        
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
        public bool TryGetBool(string identifier, out bool value) => TryGetFieldSingle(identifier, LDtkFieldType.Bool, element => element.GetBoolValue(), out value);
        
        /// <summary>
        /// Gets a bool field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public bool[] GetBoolArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Bool, element => element.GetBoolValue());
        
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
        public bool TryGetBoolArray(string identifier, out bool[] values) => TryGetFieldArray(identifier, LDtkFieldType.Bool, element => element.GetBoolValue(), out values);
        
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
        public string GetString(string identifier) => GetFieldSingle(identifier, LDtkFieldType.String, element => element.GetStringValue());
        
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
        public bool TryGetString(string identifier, out string value) => TryGetFieldSingle(identifier, LDtkFieldType.String, element => element.GetStringValue(), out value);
        
        /// <summary>
        /// Gets a string field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string[] GetStringArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.String, element => element.GetStringValue());
        
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
        public bool TryGetStringArray(string identifier, out string[] values) => TryGetFieldArray(identifier, LDtkFieldType.String, element => element.GetStringValue(), out values);
        
        #endregion
        #region Multiline
        
        /// <summary>
        /// Gets a multiline field's value. IMPORTANT: Make sure that the LDtk project is configured to use "Multilines" in it's advanced settings
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string GetMultiline(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Multiline, element => element.GetMultilineValue());
        
        /// <summary>
        /// Gets a multiline field's value. IMPORTANT: Make sure that the LDtk project is configured to use "Multilines" in it's advanced settings
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
        public bool TryGetMultiline(string identifier, out string value) => TryGetFieldSingle(identifier, LDtkFieldType.Multiline, element => element.GetMultilineValue(), out value);

        /// <summary>
        /// Gets a multiline field's values. IMPORTANT: Make sure that the LDtk project is configured to use "Multilines" in it's advanced settings
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string[] GetMultilineArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Multiline, element => element.GetMultilineValue());
        
        /// <summary>
        /// Gets a multiline field's values. IMPORTANT: Make sure that the LDtk project is configured to use "Multilines" in it's advanced settings
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
        public bool TryGetMultilineArray(string identifier, out string[] values) => TryGetFieldArray(identifier, LDtkFieldType.Multiline, element => element.GetMultilineValue(), out values);
        
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
        public string GetFilePath(string identifier) => GetFieldSingle(identifier, LDtkFieldType.FilePath, element => element.GetFilePathValue());
        
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
        public bool TryGetFilePath(string identifier, out string value) => TryGetFieldSingle(identifier, LDtkFieldType.FilePath, element => element.GetFilePathValue(), out value);
        
        /// <summary>
        /// Gets a file path field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string[] GetFilePathArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.FilePath, element => element.GetFilePathValue());
        
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
        public bool TryGetFilePathArray(string identifier, out string[] values) => TryGetFieldArray(identifier, LDtkFieldType.FilePath, element => element.GetFilePathValue(), out values);
        
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
        public Color GetColor(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Color, element => element.GetColorValue());
        
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
        public bool TryGetColor(string identifier, out Color value) => TryGetFieldSingle(identifier, LDtkFieldType.Color, element => element.GetColorValue(), out value);
        
        /// <summary>
        /// Gets a color field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Color[] GetColorArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Color, element => element.GetColorValue());

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
        public bool TryGetColorArray(string identifier, out Color[] values) => TryGetFieldArray(identifier, LDtkFieldType.Color, element => element.GetColorValue(), out values);
        
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
        public TEnum GetEnum<TEnum>(string identifier) where TEnum : struct => GetFieldSingle(identifier, LDtkFieldType.Enum, element => element.GetEnumValue<TEnum>());
        
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
        public bool TryGetEnum<TEnum>(string identifier, out TEnum value) where TEnum : struct => TryGetFieldSingle(identifier, LDtkFieldType.Enum, element => element.GetEnumValue<TEnum>(), out value);
        
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
        public TEnum[] GetEnumArray<TEnum>(string identifier) where TEnum : struct => GetFieldArray(identifier, LDtkFieldType.Enum, element => element.GetEnumValue<TEnum>());

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
        public bool TryGetEnumArray<TEnum>(string identifier, out TEnum[] values) where TEnum : struct => TryGetFieldArray(identifier, LDtkFieldType.Enum, element => element.GetEnumValue<TEnum>(), out values);
        
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
        public Vector2 GetPoint(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Point, element => element.GetPointValue());
        
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
        public bool TryGetPoint(string identifier, out Vector2 value) => TryGetFieldSingle(identifier, LDtkFieldType.Point, element => element.GetPointValue(), out value);
        
        /// <summary>
        /// Gets a point field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Vector2[] GetPointArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Point, element => element.GetPointValue());
        
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
        public bool TryGetPointArray(string identifier, out Vector2[] values) => TryGetFieldArray(identifier, LDtkFieldType.Point, element => element.GetPointValue(), out values);

        
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
        public LDtkReferenceToAnEntityInstance GetEntityReference(string identifier) => GetFieldSingle(identifier, LDtkFieldType.EntityRef,element => element.GetEntityRefValue());

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
        public bool TryGetEntityReference(string identifier, out LDtkReferenceToAnEntityInstance value) => TryGetFieldSingle(identifier, LDtkFieldType.EntityRef, element => element.GetEntityRefValue(), out value);

        /// <summary>
        /// Gets an entity reference field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public LDtkReferenceToAnEntityInstance[] GetEntityReferenceArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.EntityRef, element => element.GetEntityRefValue());

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
        public bool TryGetEntityReferenceArray(string identifier, out LDtkReferenceToAnEntityInstance[] values) => TryGetFieldArray(identifier, LDtkFieldType.EntityRef, element => element.GetEntityRefValue(), out values);

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
        public Sprite GetTile(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Tile, element => element.GetTileValue());

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
        public bool TryGetTile(string identifier, out Sprite value) => TryGetFieldSingle(identifier, LDtkFieldType.Tile, element => element.GetTileValue(), out value);
        
        /// <summary>
        /// Gets a tile reference field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Sprite[] GetTileArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Tile, element => element.GetTileValue());

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
        public bool TryGetTileArray(string identifier, out Sprite[] values) => TryGetFieldArray(identifier, LDtkFieldType.Tile, element => element.GetTileValue(), out values);

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
            if (!TryGetField(identifier, out LDtkField field))
            {
                GameObject obj = gameObject;
                LDtkDebug.LogError($"No field \"{identifier}\" exists in this field component for {obj.name}", obj);
                return null;
            }

            FieldsResult<string> result = field.GetValueAsString();
            return result.Success ? result.Value : null;
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
            if (!TryGetField(identifier, out LDtkField field))
            {
                value = null;
                return false;
            }
            
            FieldsResult<string> result = field.GetValueAsString();
            if (!result.Success)
            {
                value = null;
                return false;
            }

            value = result.Value;
            return true;
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
            if (!TryGetField(identifier, out LDtkField field))
            {
                GameObject obj = gameObject;
                LDtkDebug.LogError($"No field \"{identifier}\" exists in this field component for {obj.name}", obj);
                return Array.Empty<string>();
            }

            FieldsResult<string[]> result = field.GetValuesAsStrings();
            if (!result.Success)
            {
                return Array.Empty<string>();
            }
            
            return result.Value;
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
            if (!TryGetField(identifier, out LDtkField field))
            {
                value = null;
                value = Array.Empty<string>();
                return false;
            }

            FieldsResult<string[]> result = field.GetValuesAsStrings();
            if (!result.Success)
            {
                value = Array.Empty<string>();
                return false;
            }

            value = result.Value;
            return true;
        }

        #endregion
    }
}