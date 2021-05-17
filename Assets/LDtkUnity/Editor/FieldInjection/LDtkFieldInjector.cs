using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    public class LDtkFieldInjector
    {
        private readonly GameObject _instance;
        private readonly FieldInstance[] _fieldInstances;
        
        public LDtkFieldInjector(GameObject instance, FieldInstance[] fieldInstances)
        {
            _instance = instance;
            _fieldInstances = fieldInstances;
        }

        public void InjectEntityFields()
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
        }

        private LDtkField[] GetFields()
        {
            return _fieldInstances.Select(GetFieldFromInstance).ToArray();
        }

        private LDtkField GetFieldFromInstance(FieldInstance fieldInstance)
        {
            LDtkFieldElement[] elements;
            if (fieldInstance.Type.Contains("Array"))
            {
                Array array = GetArray(fieldInstance);
                elements = array.Cast<object>().Select(p => new LDtkFieldElement(p, fieldInstance)).ToArray();
            }
            else
            {
                object single = GetSingle(fieldInstance);
                elements = new[] {new LDtkFieldElement(single, fieldInstance)};
            }

            LDtkField field = new LDtkField(fieldInstance.Identifier, elements);
            return field;
        }
        
        private Array GetArray(FieldInstance fieldInstance)
        {
            Type elementType = GetTypeFromFieldInstance(fieldInstance);
            if (elementType == null)
            {
                throw new InvalidOperationException();
            }

            object[] values = ((IEnumerable) fieldInstance.Value).Cast<object>()
                .Select(x => x == null ? (object)null : x.ToString()).ToArray();
            
            object[] objs = values.Select(value => GetParsedValue(fieldInstance, value)).ToArray();
            
            Array array = Array.CreateInstance(elementType, objs.Length);
            Array.Copy(objs, array, objs.Length);

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

        private Type GetTypeFromFieldInstance(FieldInstance instance)
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

            return null;
        }
    }
}