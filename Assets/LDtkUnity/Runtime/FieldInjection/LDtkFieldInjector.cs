using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.FieldInjection.ParsedField;
using LDtkUnity.Runtime.Providers;
using LDtkUnity.Runtime.Tools;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace LDtkUnity.Runtime.FieldInjection
{
    public static class LDtkFieldInjector
    {
        public static void InjectEntityFields(LDtkDataEntity entityData, GameObject instance)
        {
            if (entityData.fieldInstances.NullOrEmpty())
            {
                return;
            }
            
            LDtkInjectionErrorContext.SetLogErrorContext(instance);
            
            List<LDtkFieldInjectorData> injectableFields = instance
                .GetComponents<MonoBehaviour>()
                .SelectMany(GetAttributeFieldsFromComponent).ToList();
            
            CheckFieldDefinitionsExistence(entityData.__identifier,
                entityData.fieldInstances.Select(p => p.__identifier).ToList(),
                injectableFields.Select(p => p.FieldIdentifier).ToList());
            
            //run though all of the LEd variables as the main proprietor.
            InjectAllFieldsIntoInstance(entityData, injectableFields);
            
        }
        
        private static List<LDtkFieldInjectorData> GetAttributeFieldsFromComponent(MonoBehaviour component)
        {
            return component.GetType()
                .GetFields()
                .Select(field => new
                {
                    field, 
                    attribute = field.GetCustomAttribute<LDtkFieldAttribute>()
                })
                .Where(t => t.attribute != null)
                .Select(t => new
                {
                    t, 
                    fieldName = t.attribute.IsCustomDefinedName ? t.attribute.DataIdentifier : t.field.Name
                })
                .Select(t => new LDtkFieldInjectorData(t.t.field, t.fieldName, component)).ToList();
        }

        private static void InjectAllFieldsIntoInstance(LDtkDataEntity entity, List<LDtkFieldInjectorData> injectableFields)
        {
            foreach (LDtkDataField fieldData in entity.fieldInstances)
            {
                LDtkFieldInjectorData fieldToInjectInto = injectableFields
                    .FirstOrDefault(injectableField => injectableField.FieldIdentifier == fieldData.__identifier);
                
                if (fieldToInjectInto == null)
                {
                    Debug.LogError($"LDtk: '{entity.__identifier}'s LDtk {fieldData.__type} field \"{fieldData.__identifier}\" could not find a matching Game Code field to inject into. Is the field not public?", LDtkInjectionErrorContext.Context);
                    continue;
                }
                
                InjectFieldIntoInstance(fieldData, fieldToInjectInto);
            }
        }

        private static void InjectFieldIntoInstance(LDtkDataField fieldData, LDtkFieldInjectorData fieldToInjectInto)
        {
            if (fieldToInjectInto.Info.FieldType.IsArray)
            {
                InjectArray(fieldData, fieldToInjectInto);
            }
            else
            {
                InjectSingle(fieldData, fieldToInjectInto);
            }
        }

        private static void InjectSingle(LDtkDataField instanceField, LDtkFieldInjectorData fieldToInjectInto)
        {
            string field = instanceField.__value.NullOrEmpty() ? string.Empty : instanceField.__value[0];

            object obj = GetParsedValue(fieldToInjectInto.Info.FieldType, field);
            fieldToInjectInto.Info.SetValue(fieldToInjectInto.ObjectRef, obj);
        }
        private static void InjectArray(LDtkDataField instanceField, LDtkFieldInjectorData fieldToInjectInto)
        {
            object[] objs = GetParsedValues(fieldToInjectInto.Info.FieldType.GetElementType(), instanceField.__value);

            Type elementType = fieldToInjectInto.Info.FieldType.GetElementType();
            if (elementType == null)
            {
                throw new InvalidOperationException();
            }

            Array array = Array.CreateInstance(elementType, objs.Length);
            Array.Copy(objs, array, objs.Length);

            fieldToInjectInto.Info.SetValue(fieldToInjectInto.ObjectRef, array);
        }
        
        private static object[] GetParsedValues(Type type, IEnumerable<string> stringValues)
        {
            return stringValues.Select(stringValue => GetParsedValue(type, stringValue)).ToArray();
        }

        private static object GetParsedValue(Type type, string stringValue)
        {
            ParseFieldValueAction action = LDtkFieldParser.GetParserMethodForType(type);
            return action?.Invoke(stringValue);
        }
       
        private static void CheckFieldDefinitionsExistence(string entityName, 
            ICollection<string> fieldsData,
            ICollection<string> fieldInfos)
        {
            foreach (string fieldData in fieldsData.Where(fieldData => !fieldInfos.Contains(fieldData)))
            {
                Debug.LogError($"LDtk: \"{entityName}\"s LDtk field \"{fieldData}\" is defined but does not have a matching C# field. Misspelled or missing attribute?", LDtkInjectionErrorContext.Context);
            }

            foreach (string fieldInfo in fieldInfos.Where(fieldInfo => !fieldsData.Contains(fieldInfo)))
            {
                Debug.LogError($"LDtk: \"{entityName}\" C# field \"{fieldInfo}\" uses [LDtkField] but does not have a matching LDtk field. Misspelled, undefined in LDtk editor, or unnessesary attribute?", LDtkInjectionErrorContext.Context);
            }
        }
    }
}