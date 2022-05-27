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
            LDtkField[] fields = new LDtkField[_fieldInstances.Length];
            for (int i = 0; i < _fieldInstances.Length; i++)
            {
                fields[i] = GetFieldFromInstance(_fieldInstances[i]);
            }
            return fields;
        }

        private LDtkField GetFieldFromInstance(FieldInstance fieldInstance)
        {
            bool isArray = fieldInstance.Definition.IsArray;

            Profiler.BeginSample($"GetElements {fieldInstance.Identifier}");
            LDtkFieldElement[] elements = GetObjectElements(fieldInstance, isArray);
            Profiler.EndSample();
            
            LDtkField field = new LDtkField(fieldInstance.Identifier, elements, isArray);
            return field;
        }

        private LDtkFieldElement[] GetObjectElements(FieldInstance fieldInstance, bool isArray)
        {
            object[] elements = GetElements(GetParsedValue, fieldInstance, isArray);
            return elements.Select(p => new LDtkFieldElement(p, fieldInstance)).ToArray();
        }
        
        public static object[] GetElements(ParseAction action, FieldInstance fieldInstance, bool isArray)
        {
            if (isArray)
            {
                Array array = GetArray(action, fieldInstance);
                return array.Cast<object>().ToArray();
            }

            object single = GetSingle(action, fieldInstance);
            return new[] { single };
        }

        private static Array GetArray(ParseAction action, FieldInstance fieldInstance)
        {
            IEnumerable enumerableValue = (IEnumerable)fieldInstance.Value;

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
            
            //parse em
            object[] srcObjs = objs.Select(p => action.Invoke(fieldInstance, p)).ToArray();
            
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

        private static object GetSingle(ParseAction action, FieldInstance fieldInstance)
        {
            return action.Invoke(fieldInstance, fieldInstance.Value);
        }

        internal delegate object ParseAction(FieldInstance fieldInstanceType, object value);
        
        private static object GetParsedValue(FieldInstance fieldInstanceType, object value)
        {
            ParseFieldValueAction action = LDtkFieldParser.GetParserMethodForType(fieldInstanceType);
            return action?.Invoke(value);
        }
    }
}