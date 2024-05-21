﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkFieldsFactory
    {
        private readonly GameObject _instance;
        private readonly FieldInstance[] _fieldInstances;
        private readonly LDtkProjectImporter _project;
        private readonly LDtkJsonImporter _importer;
        
        public LDtkFields FieldsComponent { get; private set; }
        
        public LDtkFieldsFactory(GameObject instance, FieldInstance[] fieldInstances, LDtkProjectImporter project, LDtkJsonImporter importer)
        {
            _instance = instance;
            _fieldInstances = fieldInstances;
            _project = project;
            _importer = importer;
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
            
            LDtkProfiler.BeginSample("GetFields");
            LDtkField[] fieldData = GetFields();
            LDtkProfiler.EndSample();
            
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
            FieldDefinition def = fieldInstance.Definition;
            bool isArray = def.IsArray;

            LDtkProfiler.BeginSample($"GetObjectElements {fieldInstance.Identifier}");
            LDtkFieldElement[] elements = GetObjectElements(fieldInstance, isArray);
            LDtkProfiler.EndSample();

            LDtkDefinitionObjectField defObj = null;
            
            Debug.Assert(_importer.DefinitionObjects != null);
            
            if (_importer.DefinitionObjects != null)
            {
                defObj = _importer.DefinitionObjects.GetObject<LDtkDefinitionObjectField>(fieldInstance.DefUid);
            }
            LDtkField field = new LDtkField(defObj, fieldInstance.Identifier, def.Doc, elements, isArray);
            return field;
        }

        private LDtkFieldElement[] GetObjectElements(FieldInstance fieldInstance, bool isArray)
        {
            LDtkProfiler.BeginSample($"GetElements");
            object[] elements = GetElements(fieldInstance, isArray);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample($"new LDtkFieldElements");
            LDtkFieldElement[] fieldElements = new LDtkFieldElement[elements.Length];
            for (int i = 0; i < fieldElements.Length; i++)
            {
                fieldElements[i] = new LDtkFieldElement(elements[i], fieldInstance);
            }
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample($"Setup point transforms");
            //setup transforms for the points so that they are easy to follow along on
            if (fieldInstance.IsPoint)
            {
                for (int i = 0; i < fieldElements.Length; i++)
                {
                    LDtkFieldElement element = fieldElements[i];
                    Transform newPoint = new GameObject($"{fieldInstance.Identifier}_{i}").transform;
                    element.SetPointLocalTransform(newPoint);
                    newPoint.SetParent(_instance.transform, true);
                }
            }
            LDtkProfiler.EndSample();
            
            
            return fieldElements;
        }
        
        public object[] GetElements(FieldInstance fieldInstance, bool isArray)
        {
            if (isArray)
            {
                LDtkProfiler.BeginSample("GetArray");
                Array array = GetArray(fieldInstance);
                LDtkProfiler.EndSample();
                
                object[] objArray = new object[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    objArray[i] = array.GetValue(i);
                }
                return objArray;
            }

            LDtkProfiler.BeginSample("GetSingle");
            object single = GetSingle(fieldInstance); 
            LDtkProfiler.EndSample();
            
            return new[] { single };
        }

        private Array GetArray(FieldInstance fieldInstance)
        {
            List<object> objs = null;
            
            if (fieldInstance.Value is List<object> list)
            {
                objs = list;
            }
            else
            {
                LDtkDebug.LogError($"Not list (was {fieldInstance.Value.GetType().GetGenericArguments().First().Name}), not populating field instance \"{fieldInstance.Identifier}\"");
                return Array.Empty<object>();
            }

            //parse em
            LDtkProfiler.BeginSample("CopyArray");
            object[] srcObjs = new object[objs.Count];
            for (int i = 0; i < objs.Count; i++)
            {
                srcObjs[i] = GetParsedValue(fieldInstance, objs[i]);
            }
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("CopyArray");
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
            LDtkProfiler.EndSample();
            
            return array;
        }

        private object GetSingle(FieldInstance fieldInstance)
        {
            return GetParsedValue(fieldInstance, fieldInstance.Value);
        }

        public object GetParsedValue(FieldInstance fieldInstanceType, object value)
        {
            ParseFieldValueAction action = LDtkFieldParser.GetParserMethodForType(fieldInstanceType);
            
            LDtkFieldParseContext ctx = new LDtkFieldParseContext()
            {
                Project = _project,
                Importer = _importer,
                Input = value
            };
            
            return action?.Invoke(ctx);
        }
    }
}