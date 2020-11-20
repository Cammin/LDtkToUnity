using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

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
                    Debug.LogError($"LDtk: '{entity.__identifier}'s LDtk {fieldData.__type} field \"{fieldData.__identifier}\" could not find a matching Game Code field to inject into. Is the field not public?");
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
            object[] objs = GetParsedValues(fieldToInjectInto.Info.FieldType, instanceField.__value);

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
        public static object GetParsedValue(Type type, string stringValue)
        {
            //Main fixer for if something was null from the editor
            if (stringValue.NullOrEmpty())
            {
                Debug.LogError("stringValue null or empty");
                return default;
            }
            
            //stringValue = PostProcessValue(type, stringValue);

            ParseFieldValueAction action = LDtkFieldParser.GetParserMethodForType(type);
            return action?.Invoke(stringValue);
        }
        
        //todo may not be the best place to have this
        private static string PostProcessValue(Type fieldType, string value)
        {
            if (fieldType.IsArray)
            {
                Type element = fieldType.GetElementType();
                if (element.IsEnum)
                {
                    ProcessEnum(element);
                    return value;
                }
            }
            
            if (fieldType.IsEnum)
            {
                ProcessEnum(fieldType);
                return value;
            }

            void ProcessEnum(Type type)
            {
                value = value.Insert(0, type.Name + ".");
            }

            return value;
        }
        
        private static void CheckFieldDefinitionsExistence(string entityName, 
            ICollection<string> fieldsData,
            ICollection<string> fieldInfos)
        {
            foreach (string fieldData in fieldsData.Where(fieldData => fieldInfos.Contains(fieldData) == false))
            {
                Debug.LogError($"LDtk: \"{entityName}\"s LDtk field \"{fieldData}\" is defined but does not have a matching Game Code field. Misspelled or missing attribute?");
            }

            foreach (string fieldInfo in fieldInfos.Where(fieldInfo => fieldsData.Contains(fieldInfo) == false))
            {
                Debug.LogError($"LDtk: \"{entityName}\"s C# field \"{fieldInfo}\" is set as injectable but does not have a matching LDtk field. Misspelled, undefined in LEd editor, or unnessesary attribute?");
            }
        }
    }
}