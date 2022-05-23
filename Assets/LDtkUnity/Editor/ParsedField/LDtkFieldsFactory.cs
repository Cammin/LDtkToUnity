using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Profiling;

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
            
            Profiler.BeginSample("GetFields");
            LDtkField[] fieldData = GetFields();
            Profiler.EndSample();
            
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
            /*Type elementType = GetUnityTypeForFieldInstance(fieldInstance);
            if (elementType == null)
            {
                throw new InvalidOperationException();
            }*/

            IEnumerable enumerableValue = (IEnumerable)fieldInstance.Value;
            //Debug.Log($"field {fieldInstance.Identifier}");
            
            List<string> objs = new List<string>();
            foreach (object o in enumerableValue)
            {
                JToken jValue = JToken.FromObject(o);

                string add = null;
                
                dynamic value = jValue.Value<dynamic>();
                if (value != null)
                {
                    add = jValue.ToString();
                }
                
                objs.Add(add);
            }
            
            //IEnumerable<string> strings = objs.Select(x => x == null ? null : x.ToString());
            
            //parse em
            object[] srcObjs = objs.Select(p => GetParsedValue(fieldInstance, p)).ToArray();
            
            Array array = new object[srcObjs.Length];
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