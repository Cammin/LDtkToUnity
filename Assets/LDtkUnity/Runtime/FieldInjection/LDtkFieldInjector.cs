using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LDtkUnity
{
    public static class LDtkFieldInjector
    {
        public static void InjectEntityFields(EntityInstance entityData, GameObject instance, int gridSize)
        {
            if (entityData.FieldInstances.NullOrEmpty())
            {
                return;
            }
            
            LDtkInjectionErrorContext.SetLogErrorContext(instance);
            
            List<LDtkFieldInjectorData> injectableFields = instance
                .GetComponents<MonoBehaviour>()
                .SelectMany(GetAttributeFieldsFromComponent).ToList();
            
            //validation
            CheckFieldDefinitionsExistence(entityData.Identifier,
                entityData.FieldInstances.Select(p => p.Identifier).ToList(),
                injectableFields.Select(p => p.FieldIdentifier).ToList());
            
            //run though all of the LDtk variables as the main proprietor.
            InjectAllFieldsIntoInstance(entityData, injectableFields, gridSize);
            
        }
        
        private static List<LDtkFieldInjectorData> GetAttributeFieldsFromComponent(MonoBehaviour component)
        {
            return (from fieldInfo in component.GetType().GetFields() 
                
                let attribute = fieldInfo.GetCustomAttribute<LDtkFieldAttribute>() where attribute != null
                let fieldName = attribute.IsCustomDefinedName ? attribute.DataIdentifier : fieldInfo.Name
                
                select new LDtkFieldInjectorData(fieldInfo, fieldName, component)).ToList();
        }

        private static void InjectAllFieldsIntoInstance(EntityInstance entity, List<LDtkFieldInjectorData> injectableFields, int gridSize)
        {
            foreach (FieldInstance fieldData in entity.FieldInstances)
            {
                LDtkFieldInjectorData fieldToInjectInto = injectableFields
                    .FirstOrDefault(injectableField => injectableField.FieldIdentifier == fieldData.Identifier);
                
                if (fieldToInjectInto == null)
                {
                    Debug.LogError($"LDtk: '{entity.Identifier}'s LDtk field {fieldData.Type} \"{fieldData.Identifier}\" could not find a matching C# field to inject into. Is the field not public?", LDtkInjectionErrorContext.Context);
                    continue;
                }
                
                InjectFieldIntoInstance(fieldData, fieldToInjectInto);
                
                //todo bring this back later once we can figure out how to dirty the added component correctly.
               // TryAddPointDrawer(fieldData, fieldToInjectInto, entity, gridSize);
            }
        }

        private static void InjectFieldIntoInstance(FieldInstance fieldInstance, LDtkFieldInjectorData fieldToInjectInto)
        {
            if (fieldInstance.Type.Contains("Array"))
            {
                //validate that the field is an array
                if (!fieldToInjectInto.Info.FieldType.IsArray)
                {
                    Debug.LogError($"LDtk: The LDtk field \"{fieldInstance.Identifier}\" is an array but the C# field is not.");
                    return;
                }

                InjectArray(fieldInstance, fieldToInjectInto);
            }
            else
            {
                //validate that the field is NOT an array
                if (fieldToInjectInto.Info.FieldType.IsArray)
                {
                    Debug.LogError($"LDtk: The LDtk field \"{fieldInstance.Identifier}\" is not an array but the C# is.");
                    return;
                }
                
                InjectSingle(fieldInstance, fieldToInjectInto);
            }
        }
        
        private static void InjectArray(FieldInstance fieldInstance, LDtkFieldInjectorData fieldToInjectInto)
        {
            Type elementType = fieldToInjectInto.Info.FieldType.GetElementType();
            if (elementType == null)
            {
                throw new InvalidOperationException();
            }

            object[] values = ((IEnumerable) fieldInstance.Value).Cast<object>()
                .Select(x => x == null ? (object)null : x.ToString()).ToArray();
            
            object[] objs = values.Select(value => GetParsedValue(fieldInstance.Type, value, elementType)).ToArray();
            
            Array array = Array.CreateInstance(elementType, objs.Length);
            Array.Copy(objs, array, objs.Length);

            fieldToInjectInto.SetField(array);
        }
        private static void InjectSingle(FieldInstance fieldInstance, LDtkFieldInjectorData fieldToInjectInto)
        {
            Type type = fieldToInjectInto.Info.FieldType;
            object field = GetParsedValue(fieldInstance.Type, fieldInstance.Value, type);
            fieldToInjectInto.SetField(field);
        }
        
        private static object GetParsedValue(string fieldInstanceType, object value, Type type)
        {
            ParseFieldValueAction action;
            if (type.IsEnum || type.IsArray && type.GetElementType().IsEnum)
            {
                action = LDtkFieldParser.GetEnumMethod(type);
            }
            else
            {
                action = LDtkFieldParser.GetParserMethodForType(fieldInstanceType);
            }
            
            return action?.Invoke(value);
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
        
        
        private static bool DrawerEligibility(EditorDisplayMode? mode, Type type)
        {
            switch (mode)
            {
                case null:
                    return false;
                case EditorDisplayMode.RadiusGrid:
                case EditorDisplayMode.RadiusPx:
                {
                    if (type == typeof(int) || type == typeof(float))
                    {
                        return true;
                    }

                    break;
                }
                case EditorDisplayMode.PointPath:
                case EditorDisplayMode.PointStar:
                {
                    if (type == typeof(Vector2) || type == typeof(Vector2[]))
                    {
                        return true;
                    }

                    break;
                }
            }

            return false;
        }
        
        private static void TryAddPointDrawer(FieldInstance fieldData, LDtkFieldInjectorData fieldToInjectInto, EntityInstance entityData, int gridSize)
        {
            if (!DrawerEligibility(fieldData.Definition.EditorDisplayMode, fieldToInjectInto.Info.FieldType))
            {
                return;
            }

            Component component = (Component)fieldToInjectInto.ObjectRef;
            
            LDtkSceneDrawer drawer = component.gameObject.AddComponent<LDtkSceneDrawer>();
            

            EditorDisplayMode? editorDisplayMode = fieldData.Definition.EditorDisplayMode;
            if (editorDisplayMode != null)
            {
                EditorDisplayMode displayMode = editorDisplayMode.Value;
                drawer.SetReference(component, fieldToInjectInto.Info, entityData, displayMode, gridSize);
            }
            
            LDtkEditorUtil.Dirty(drawer);
        }
    }
}