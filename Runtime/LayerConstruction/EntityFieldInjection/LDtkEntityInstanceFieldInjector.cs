using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using UnityEngine;

namespace LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection
{
    public static class LDtkEntityInstanceFieldInjector
    {
        public static void InjectInstanceFields(LDtkDataEntityInstance entity, GameObject instance)
        {
            if (entity.fieldInstances.IsNullOrEmpty()) return;
            
            MonoBehaviour[] behaviors = instance.GetComponents<MonoBehaviour>();
            List<LDtkInjectableField> injectableFields = GatherInjectableFields(behaviors);
            
            CheckFieldDefinitionsExistence(entity.__identifier,
                entity.fieldInstances.Select(p => p.__identifier).ToList(),
                injectableFields.Select(p => p.FieldIdentifier).ToList());
            
            //run though all of the LEd variables as the main proprietor.
            InjectAllFieldsIntoInstance(injectableFields, entity);
            
            RunPostInjectionEvents(behaviors);
        }

        private static void InjectAllFieldsIntoInstance(List<LDtkInjectableField> injectableFields, LDtkDataEntityInstance entity)
        {
            foreach (LDtkDataEntityInstanceField instanceField in entity.fieldInstances)
            {
                InjectFieldIntoInstance(entity, instanceField, injectableFields);
            }
        }

        private static void InjectFieldIntoInstance(LDtkDataEntityInstance entity, LDtkDataEntityInstanceField instanceField,
            List<LDtkInjectableField> injectableFields)
        {
            
            string dataFieldIdentifier = instanceField.__identifier;

            LDtkInjectableField fieldToInjectInto = GetInjectableFieldMatchingIdentifier(injectableFields, dataFieldIdentifier);

            if (fieldToInjectInto == null)
            {
                Debug.LogError($"LEd: '{entity.__identifier}'s LEd {instanceField.__type} field \"{dataFieldIdentifier}\" could not find a matching Game Code field to inject into. Is the field not public?");
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

        private static void InjectSingle(LDtkDataEntityInstanceField instanceField, LDtkInjectableField fieldToInjectInto)
        {
            object obj = GetValue(fieldToInjectInto.Info.FieldType, instanceField.__value[0]);
            fieldToInjectInto.Info.SetValue(fieldToInjectInto.ObjectRef, obj);
        }

        private static void InjectArray(LDtkDataEntityInstanceField instanceField, LDtkInjectableField fieldToInjectInto)
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


        private static LDtkInjectableField GetInjectableFieldMatchingIdentifier(List<LDtkInjectableField> injectableFields, string fieldNameLEd)
        {
            return injectableFields.FirstOrDefault(injectableField => injectableField.FieldIdentifier == fieldNameLEd);
        }

        private static List<LDtkInjectableField> GatherInjectableFields(MonoBehaviour[] behaviors)
        {
            return behaviors.SelectMany(GetAttributeFieldsFromComponents).ToList();
        }

        private static List<LDtkInjectableField> GetAttributeFieldsFromComponents(MonoBehaviour component)
        {
            return component.GetType()
                .GetFields()
                .Select(field => new
                {
                    field, 
                    attribute = field.GetCustomAttribute<LDtkInjectableFieldAttribute>()
                })
                .Where(t => t.attribute != null)
                .Select(t => new
                {
                    t, 
                    fieldName = t.attribute.IsCustomDefinedName ? t.attribute.DataIdentifier : t.field.Name
                })
                .Select(t => new LDtkInjectableField(t.t.field, t.fieldName, component)).ToList();
        }

        private static void RunPostInjectionEvents(MonoBehaviour[] behaviors)
        {
            foreach (MonoBehaviour component in behaviors)
            {
                if (component is ILDtkInjectedFieldEvent injectableEvent)
                {
                    injectableEvent.OnLDtkFieldsInjected();
                }
            }
        }


        private static object[] GetValues(Type type, IEnumerable<string> stringValues)
        {
            return stringValues.Select(stringValue => GetValue(type, stringValue)).ToArray();
        }
        public static object GetValue(Type type, string stringValue)
        {
            //Main fixer for if something was null from the editor
            if (stringValue.IsNullOrEmpty())
            {
                return default;
            }
            
            stringValue = PostProcessValue(type, stringValue);
            return LDtkEntityInstanceFieldParser.GetParserMethodForType(type).Invoke(stringValue);
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
                Debug.LogError($"LDtk: '{entityName}'s LEd field \"{fieldData}\" is defined but does not have a matching Game Code field. Misspelled or missing attribute?");
            }

            foreach (string fieldInfo in fieldInfos.Where(fieldInfo => fieldsData.Contains(fieldInfo) == false))
            {
                Debug.LogError($"LDtk: '{entityName}'s Game Code field \"{fieldInfo}\" is set as injectable but does not have a matching LEd field. Misspelled, undefined in LEd editor, or unnessesary attribute?");
            }
        }
    }
}