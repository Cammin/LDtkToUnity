using System;
using UnityEngine;

namespace LDtkUnity
{
    public partial class LDtkFields
    {
        [Obsolete("Use GetField() instead to access it's definition.")]
        public LDtkDefinitionObjectField GetDefinition(string identifier)
        {
            if (!TryGetField(identifier, out LDtkField field))
            {
                GameObject obj = gameObject;
                LDtkDebug.LogError($"No field \"{identifier}\" exists in this field component for {obj.name}", obj);
                return null;
            }
            return field.Definition;
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
        public int GetInt(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Int, (LDtkFieldElement element, out bool success) => element.TryGetInt(out success));

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
        public bool TryGetInt(string identifier, out int value) => TryGetFieldSingle(identifier, LDtkFieldType.Int, (LDtkFieldElement element, out bool success) => element.TryGetInt(out success), out value);

        /// <summary>
        /// Gets an int field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public int[] GetIntArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Int, (LDtkFieldElement element, out bool success) => element.TryGetInt(out success));
        
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
        public bool TryGetIntArray(string identifier, out int[] values) => TryGetFieldArray(identifier, LDtkFieldType.Int, (LDtkFieldElement element, out bool success) => element.TryGetInt(out success), out values);

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
        public float GetFloat(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Float, (LDtkFieldElement element, out bool success) => element.TryGetFloat(out success));
        
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
        public bool TryGetFloat(string identifier, out float value) => TryGetFieldSingle(identifier, LDtkFieldType.Float, (LDtkFieldElement element, out bool success) => element.TryGetFloat(out success), out value);
        
        /// <summary>
        /// Gets a float field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public float[] GetFloatArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Float, (LDtkFieldElement element, out bool success) => element.TryGetFloat(out success));
        
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
        public bool TryGetFloatArray(string identifier, out float[] values) => TryGetFieldArray(identifier, LDtkFieldType.Float, (LDtkFieldElement element, out bool success) => element.TryGetFloat(out success), out values);
        
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
        public bool GetBool(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Bool, (LDtkFieldElement element, out bool success) => element.TryGetBool(out success));
        
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
        public bool TryGetBool(string identifier, out bool value) => TryGetFieldSingle(identifier, LDtkFieldType.Bool, (LDtkFieldElement element, out bool success) => element.TryGetBool(out success), out value);
        
        /// <summary>
        /// Gets a bool field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public bool[] GetBoolArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Bool, (LDtkFieldElement element, out bool success) => element.TryGetBool(out success));
        
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
        public bool TryGetBoolArray(string identifier, out bool[] values) => TryGetFieldArray(identifier, LDtkFieldType.Bool, (LDtkFieldElement element, out bool success) => element.TryGetBool(out success), out values);
        
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
        public string GetString(string identifier) => GetFieldSingle(identifier, LDtkFieldType.String, (LDtkFieldElement element, out bool success) => element.TryGetString(out success));
        
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
        public bool TryGetString(string identifier, out string value) => TryGetFieldSingle(identifier, LDtkFieldType.String, (LDtkFieldElement element, out bool success) => element.TryGetString(out success), out value);
        
        /// <summary>
        /// Gets a string field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string[] GetStringArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.String, (LDtkFieldElement element, out bool success) => element.TryGetString(out success));
        
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
        public bool TryGetStringArray(string identifier, out string[] values) => TryGetFieldArray(identifier, LDtkFieldType.String, (LDtkFieldElement element, out bool success) => element.TryGetString(out success), out values);
        
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
        public string GetMultiline(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Multiline, (LDtkFieldElement element, out bool success) => element.TryGetMultiline(out success));
        
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
        public bool TryGetMultiline(string identifier, out string value) => TryGetFieldSingle(identifier, LDtkFieldType.Multiline, (LDtkFieldElement element, out bool success) => element.TryGetMultiline(out success), out value);

        /// <summary>
        /// Gets a multiline field's values. IMPORTANT: Make sure that the LDtk project is configured to use "Multilines" in it's advanced settings
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string[] GetMultilineArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Multiline, (LDtkFieldElement element, out bool success) => element.TryGetMultiline(out success));
        
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
        public bool TryGetMultilineArray(string identifier, out string[] values) => TryGetFieldArray(identifier, LDtkFieldType.Multiline, (LDtkFieldElement element, out bool success) => element.TryGetMultiline(out success), out values);
        
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
        public string GetFilePath(string identifier) => GetFieldSingle(identifier, LDtkFieldType.FilePath, (LDtkFieldElement element, out bool success) => element.TryGetFilePath(out success));
        
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
        public bool TryGetFilePath(string identifier, out string value) => TryGetFieldSingle(identifier, LDtkFieldType.FilePath, (LDtkFieldElement element, out bool success) => element.TryGetFilePath(out success), out value);
        
        /// <summary>
        /// Gets a file path field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public string[] GetFilePathArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.FilePath, (LDtkFieldElement element, out bool success) => element.TryGetFilePath(out success));
        
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
        public bool TryGetFilePathArray(string identifier, out string[] values) => TryGetFieldArray(identifier, LDtkFieldType.FilePath, (LDtkFieldElement element, out bool success) => element.TryGetFilePath(out success), out values);
        
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
        public Color GetColor(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Color, (LDtkFieldElement element, out bool success) => element.TryGetColor(out success));
        
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
        public bool TryGetColor(string identifier, out Color value) => TryGetFieldSingle(identifier, LDtkFieldType.Color, (LDtkFieldElement element, out bool success) => element.TryGetColor(out success), out value);
        
        /// <summary>
        /// Gets a color field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Color[] GetColorArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Color, (LDtkFieldElement element, out bool success) => element.TryGetColor(out success));

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
        public bool TryGetColorArray(string identifier, out Color[] values) => TryGetFieldArray(identifier, LDtkFieldType.Color, (LDtkFieldElement element, out bool success) => element.TryGetColor(out success), out values);
        
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
        public TEnum GetEnum<TEnum>(string identifier) where TEnum : struct => GetFieldSingle(identifier, LDtkFieldType.Enum, (LDtkFieldElement element, out bool success) => element.TryGetEnum<TEnum>(out success));
        
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
        public bool TryGetEnum<TEnum>(string identifier, out TEnum value) where TEnum : struct => TryGetFieldSingle(identifier, LDtkFieldType.Enum, (LDtkFieldElement element, out bool success) => element.TryGetEnum<TEnum>(out success), out value);
        
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
        public TEnum[] GetEnumArray<TEnum>(string identifier) where TEnum : struct => GetFieldArray(identifier, LDtkFieldType.Enum, (LDtkFieldElement element, out bool success) => element.TryGetEnum<TEnum>(out success));

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
        public bool TryGetEnumArray<TEnum>(string identifier, out TEnum[] values) where TEnum : struct => TryGetFieldArray(identifier, LDtkFieldType.Enum, (LDtkFieldElement element, out bool success) => element.TryGetEnum<TEnum>(out success), out values);
        
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
        public Vector2 GetPoint(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Point, (LDtkFieldElement element, out bool success) => element.TryGetPoint(out success));
        /// <summary>
        /// Gets a point field's value as transform.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Transform GetPointTransform(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Point, (LDtkFieldElement element, out bool success) => element.TryGetPointTransform(out success));
        
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
        public bool TryGetPoint(string identifier, out Vector2 value) => TryGetFieldSingle(identifier, LDtkFieldType.Point, (LDtkFieldElement element, out bool success) => element.TryGetPoint(out success), out value);
        
        /// <summary>
        /// Gets a point field's value as transform.
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
        public bool TryGetPointTransform(string identifier, out Transform value) => TryGetFieldSingle(identifier, LDtkFieldType.Point, (LDtkFieldElement element, out bool success) => element.TryGetPointTransform(out success), out value);
        
        /// <summary>
        /// Gets a point field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Vector2[] GetPointArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Point, (LDtkFieldElement element, out bool success) => element.TryGetPoint(out success));
        
        /// <summary>
        /// Gets a point field's values as transforms.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Transform[] GetPointTransformArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Point, (LDtkFieldElement element, out bool success) => element.TryGetPointTransform(out success));
        
        /// <summary>
        /// Gets a point field's values as transforms.
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
        public bool TryGetPointTransformArray(string identifier, out Transform[] values) => TryGetFieldArray(identifier, LDtkFieldType.Point, (LDtkFieldElement element, out bool success) => element.TryGetPointTransform(out success), out values);
        
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
        public bool TryGetPointArray(string identifier, out Vector2[] values) => TryGetFieldArray(identifier, LDtkFieldType.Point, (LDtkFieldElement element, out bool success) => element.TryGetPoint(out success), out values);

        
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
        public LDtkReferenceToAnEntityInstance GetEntityReference(string identifier) => GetFieldSingle(identifier, LDtkFieldType.EntityRef, (LDtkFieldElement element, out bool success) => element.TryGetEntityReference(out success));

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
        public bool TryGetEntityReference(string identifier, out LDtkReferenceToAnEntityInstance value) => TryGetFieldSingle(identifier, LDtkFieldType.EntityRef, (LDtkFieldElement element, out bool success) => element.TryGetEntityReference(out success), out value);

        /// <summary>
        /// Gets an entity reference field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public LDtkReferenceToAnEntityInstance[] GetEntityReferenceArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.EntityRef, (LDtkFieldElement element, out bool success) => element.TryGetEntityReference(out success));

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
        public bool TryGetEntityReferenceArray(string identifier, out LDtkReferenceToAnEntityInstance[] values) => TryGetFieldArray(identifier, LDtkFieldType.EntityRef, (LDtkFieldElement element, out bool success) => element.TryGetEntityReference(out success), out values);

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
        public Sprite GetTile(string identifier) => GetFieldSingle(identifier, LDtkFieldType.Tile, (LDtkFieldElement element, out bool success) => element.TryGetTile(out success));

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
        public bool TryGetTile(string identifier, out Sprite value) => TryGetFieldSingle(identifier, LDtkFieldType.Tile, (LDtkFieldElement element, out bool success) => element.TryGetTile(out success), out value);
        
        /// <summary>
        /// Gets a tile reference field's values.
        /// </summary>
        /// <param name="identifier">
        /// The field instance's identifier. Case sensitive.
        /// </param>
        /// <returns>
        /// The field's value. If the field doesn't exist, then returns a default value type.
        /// </returns>
        public Sprite[] GetTileArray(string identifier) => GetFieldArray(identifier, LDtkFieldType.Tile, (LDtkFieldElement element, out bool success) => element.TryGetTile(out success));

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
        public bool TryGetTileArray(string identifier, out Sprite[] values) => TryGetFieldArray(identifier, LDtkFieldType.Tile, (LDtkFieldElement element, out bool success) => element.TryGetTile(out success), out values);

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

            return field.GetValueAsString();
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
            
            value = field.TryGetValueAsString(out bool success);
            if (!success)
            {
                value = null;
                return false;
            }
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
            
            return field.GetValuesAsStrings();
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
                value = Array.Empty<string>();
                return false;
            }

            value = field.TryGetValuesAsStrings(out bool success);
            return success;
        }

        #endregion
    }
}