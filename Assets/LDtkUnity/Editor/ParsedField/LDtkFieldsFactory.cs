using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkFieldsFactory
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

            Profiler.BeginSample($"GetObjectElements {fieldInstance.Identifier}");
            LDtkFieldElement[] elements = GetObjectElements(fieldInstance, isArray);
            Profiler.EndSample();
            
            LDtkField field = new LDtkField(fieldInstance.Identifier, elements, isArray);
            return field;
        }

        private LDtkFieldElement[] GetObjectElements(FieldInstance fieldInstance, bool isArray)
        {
            Profiler.BeginSample($"GetElements");
            object[] elements = GetElements(GetParsedValue, fieldInstance, isArray);
            Profiler.EndSample();
            
            Profiler.BeginSample($"new LDtkFieldElements");
            LDtkFieldElement[] fieldElements = new LDtkFieldElement[elements.Length];
            for (int i = 0; i < fieldElements.Length; i++)
            {
                fieldElements[i] = new LDtkFieldElement(elements[i], fieldInstance);
            }
            Profiler.EndSample();

            return fieldElements;
        }
        
        public static object[] GetElements(ParseAction action, FieldInstance fieldInstance, bool isArray)
        {
            if (isArray)
            {
                Profiler.BeginSample("GetArray");
                Array array = GetArray(action, fieldInstance);
                Profiler.EndSample();
                
                object[] objArray = new object[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    objArray[i] = array.GetValue(i);
                }
                return objArray;
            }

            Profiler.BeginSample("GetSingle");
            object single = GetSingle(action, fieldInstance); 
            Profiler.EndSample();
            
            return new[] { single };
        }

        private static Array GetArray(ParseAction action, FieldInstance fieldInstance)
        {
            List<string> objs = new List<string>();
            
            if (fieldInstance.Value is JArray jArray)
            {
                Profiler.BeginSample("GetAndPopulateObjs");
                foreach (JToken jToken in jArray)
                {
                    Profiler.BeginSample("DoJTokenElement");

                    string add = null;
                    if (jToken.Type != JTokenType.Null)
                    {
                        add = jToken.Value<object>()?.ToString();
                    }
                    objs.Add(add);
                    Profiler.EndSample();
                }
                Profiler.EndSample();
            }
            else
            {
                LDtkDebug.LogError($"Not JArray, not populating field instance \"{fieldInstance.Identifier}\"");
            }

            //parse em
            Profiler.BeginSample("CopyArray");
            object[] srcObjs = new object[objs.Count];
            for (int i = 0; i < objs.Count; i++)
            {
                srcObjs[i] = action.Invoke(fieldInstance, objs[i]);
            }
            Profiler.EndSample();

            Profiler.BeginSample("CopyArray");
            Array array = new object[srcObjs.Length];
            try
            {
                Array.Copy(srcObjs, array, srcObjs.Length);
            }
            catch(Exception e)
            {
                string srcObjsStrings = string.Join(", ", srcObjs);
                LDtkDebug.LogError($"Issue copying array for field instance \"{fieldInstance.Identifier}\"; LDtk type: {fieldInstance.Type}, ParsedObjects: {srcObjsStrings}. {e}");
            }
            Profiler.EndSample();
            
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