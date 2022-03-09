using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkFieldsFactory
    {
        private readonly GameObject _instance;
        private readonly FieldInstance[] _fieldInstances;
        
        public LDtkFields FieldsComponent { get; private set; }
        
        public LDtkFieldsFactory(GameObject instance, FieldInstance[] fieldInstances)
        {
            _instance = instance;
            _fieldInstances = fieldInstances;
        }

        public void SetEntityFieldsComponent()
        {
            if (_fieldInstances.IsNullOrEmpty())
            {
                return;
            }
            
            if (!_instance.TryGetComponent(out LDtkFields fields))
            {
                fields = _instance.AddComponent<LDtkFields>();
            }
            
            LDtkField[] fieldData = GetFields();
            fields.SetFieldData(fieldData);

            FieldsComponent = fields;
        }

        private LDtkField[] GetFields()
        {
            return _fieldInstances.Select(GetFieldFromInstance).ToArray();
        }

        private LDtkField GetFieldFromInstance(FieldInstance fieldInstance)
        {
            bool isArray = fieldInstance.Definition.IsArray;

            LDtkFieldElement[] elements = GetElements(fieldInstance, isArray);
            LDtkField field = new LDtkField(fieldInstance.Identifier, elements, isArray);
            return field;
        }

        private LDtkFieldElement[] GetElements(FieldInstance fieldInstance, bool isArray)
        {
            Type type = GetUnityTypeForFieldInstance(fieldInstance);
            
            if (isArray)
            {
                Array array = GetArray(fieldInstance);
                return array.Cast<object>().Select(p => new LDtkFieldElement(p, fieldInstance, type)).ToArray();
            }

            object single = GetSingle(fieldInstance);
            return new[] { new LDtkFieldElement(single, fieldInstance, type) };
        }

        private Array GetArray(FieldInstance fieldInstance)
        {
            Type elementType = GetUnityTypeForFieldInstance(fieldInstance);
            if (elementType == null)
            {
                throw new InvalidOperationException();
            }

            object[] values = ((IEnumerable) fieldInstance.Value).Cast<object>()
                .Select(x => x == null ? default : x.ToString()).ToArray();
            
            object[] srcObjs = values.Select(value => GetParsedValue(fieldInstance, value)).ToArray();
            
            Array array = Array.CreateInstance(elementType, srcObjs.Length);

            try
            {
                Array.Copy(srcObjs, array, srcObjs.Length);
            }
            catch(Exception e)
            {
                string srcObjsStrings = string.Join(", ", srcObjs);
                Debug.LogError($"LDtk: Issue copying array for field instance \"{fieldInstance.Identifier}\"; LDtk type: {fieldInstance.Type}, ParsedObjects: {srcObjsStrings}. {e}");
            }

            return array;
        }
        private object GetSingle(FieldInstance fieldInstance)
        {
            return GetParsedValue(fieldInstance, fieldInstance.Value);
        }
        
        private object GetParsedValue(FieldInstance fieldInstanceType, object value)
        {
            ParseFieldValueAction action = LDtkFieldParser.GetParserMethodForType(fieldInstanceType);
            return action?.Invoke(value);
        }

        private Type GetBasicTypeFromFieldInstance(FieldInstance instance) //todo could this be something worthwhile eventually?
        {
            if (instance.IsInt) return typeof(int);
            if (instance.IsFloat) return typeof(float);
            if (instance.IsBool) return typeof(bool);
            if (instance.IsString) return typeof(string);
            if (instance.IsFilePath) return typeof(string);
            if (instance.IsMultilines) return typeof(string);
            if (instance.IsEnum) return typeof(string);
            if (instance.IsColor) return typeof(string);
            if (instance.IsPoint) return typeof(GridPoint);
            if (instance.IsEntityRef) return typeof(EntityReferenceInfos);
            if (instance.IsTile) return typeof(TilesetRectangle);

            return null;
        }
        
        private Type GetUnityTypeForFieldInstance(FieldInstance instance)
        {
            if (instance.IsInt) return typeof(int);
            if (instance.IsFloat) return typeof(float);
            if (instance.IsBool) return typeof(bool);
            if (instance.IsString) return typeof(string);
            if (instance.IsFilePath) return typeof(string);
            if (instance.IsMultilines) return typeof(string);
            if (instance.IsEnum) return typeof(string);
            if (instance.IsColor) return typeof(Color);
            if (instance.IsPoint) return typeof(Vector2);
            if (instance.IsEntityRef) return typeof(string);
            if (instance.IsTile) return typeof(Sprite);

            return null;
        }
    }
}