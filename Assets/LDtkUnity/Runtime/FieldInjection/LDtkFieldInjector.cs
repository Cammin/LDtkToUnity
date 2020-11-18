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
        public static void InjectInstanceFields(LDtkDataEntity entity, GameObject instance)
        {
            if (entity.fieldInstances.NullOrEmpty()) return;
            
            MonoBehaviour[] behaviors = instance.GetComponents<MonoBehaviour>();
            List<LDtkFieldInjectorData> injectableFields = GatherInjectableFields(behaviors);
            
            CheckFieldDefinitionsExistence(entity.__identifier,
                entity.fieldInstances.Select(p => p.__identifier).ToList(),
                injectableFields.Select(p => p.FieldIdentifier).ToList());
            
            //run though all of the LEd variables as the main proprietor.
            InjectAllFieldsIntoInstance(injectableFields, entity);
            
        }

        private static void InjectAllFieldsIntoInstance(List<LDtkFieldInjectorData> injectableFields, LDtkDataEntity entity)
        {
            foreach (LDtkDataField instanceField in entity.fieldInstances)
            {
                InjectFieldIntoInstance(entity, instanceField, injectableFields);
            }
        }

        private static void InjectFieldIntoInstance(LDtkDataEntity entity, LDtkDataField instanceField,
            List<LDtkFieldInjectorData> injectableFields)
        {
            
            string dataFieldIdentifier = instanceField.__identifier;

            LDtkFieldInjectorData fieldToInjectInto = GetInjectableFieldMatchingIdentifier(injectableFields, dataFieldIdentifier);

            if (fieldToInjectInto == null)
            {
                Debug.LogError($"LDtk: '{entity.__identifier}'s LDtk {instanceField.__type} field \"{dataFieldIdentifier}\" could not find a matching Game Code field to inject into. Is the field not public?");
                return;
            }
            
            if (fieldToInjectInto.Info.FieldType.IsArray)
            {
                InjectArray(instanceField, fieldToInjectInto);
            }
            else
            {
                InjectSingle(instanceField, fieldToInjectInto);
            }
        }

        private static void InjectSingle(LDtkDataField instanceField, LDtkFieldInjectorData fieldToInjectInto)
        {
            string field = instanceField.__value.NullOrEmpty() ? string.Empty : instanceField.__value[0];

            object obj = GetValue(fieldToInjectInto.Info.FieldType, field);
            fieldToInjectInto.Info.SetValue(fieldToInjectInto.ObjectRef, obj);
        }

        private static void InjectArray(LDtkDataField instanceField, LDtkFieldInjectorData fieldToInjectInto)
        {
            object[] objs = GetValues(fieldToInjectInto.Info.FieldType, instanceField.__value);

            Type elementType = fieldToInjectInto.Info.FieldType.GetElementType();
            if (elementType == null)
            {
                throw new InvalidOperationException();
            }

            Array array = Array.CreateInstance(elementType, objs.Length);
            Array.Copy(objs, array, objs.Length);

            fieldToInjectInto.Info.SetValue(fieldToInjectInto.ObjectRef, array);
        }


        private static LDtkFieldInjectorData GetInjectableFieldMatchingIdentifier(List<LDtkFieldInjectorData> injectableFields, string fieldNameLDtk)
        {
            return injectableFields.FirstOrDefault(injectableField => injectableField.FieldIdentifier == fieldNameLDtk);
        }

        private static List<LDtkFieldInjectorData> GatherInjectableFields(MonoBehaviour[] behaviors)
        {
            return behaviors.SelectMany(GetAttributeFieldsFromComponents).ToList();
        }

        private static List<LDtkFieldInjectorData> GetAttributeFieldsFromComponents(MonoBehaviour component)
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




        private static object[] GetValues(Type type, IEnumerable<string> stringValues)
        {
            return stringValues.Select(stringValue => GetValue(type, stringValue)).ToArray();
        }
        public static object GetValue(Type type, string stringValue)
        {
            //Main fixer for if something was null from the editor
            if (stringValue.NullOrEmpty())
            {
                return default;
            }
            
            stringValue = PostProcessValue(type, stringValue);

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
                    Process(element);
                    return value;
                }
            }
            
            if (fieldType.IsEnum)
            {
                Process(fieldType);
                return value;
            }

            void Process(Type type)
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