using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Shared class for processing a field, used by both LDtkFieldsFactory and LDtkTocFieldFactory
    /// </summary>
    internal sealed class LDtkFieldFactory
    {
        private readonly LDtkProjectImporter _project;
        private readonly LDtkJsonImporter _importer;
        
        public LDtkFieldFactory(LDtkProjectImporter project, LDtkJsonImporter importer)
        {
            _project = project;
            _importer = importer;
        }
        
        /// <summary>
        /// Make an LDtkField from a field instance.
        /// </summary>
        /// <param name="def"></param>
        /// <param name="value">the field value, whether array or not. Depends on the field def</param>
        /// <returns></returns>
        public LDtkField GetFieldFromInstance(FieldDefinition def, object value)
        {
            LDtkProfiler.BeginSample($"GetObjectElements {def.Identifier}");
            LDtkFieldElement[] elements = GetObjectElements(def, value);
            LDtkProfiler.EndSample();

            LDtkDefinitionObjectField defObj = null;
            
            Debug.Assert(_importer.DefinitionObjects != null);
            if (_importer.DefinitionObjects != null)
            {
                defObj = _importer.DefinitionObjects.GetObject<LDtkDefinitionObjectField>(def.Uid);
            }
            
            LDtkField field = new LDtkField(defObj, elements);
            return field;
        }

        private LDtkFieldElement[] GetObjectElements(FieldDefinition def, object value)
        {
            LDtkProfiler.BeginSample($"GetElements");
            object[] elements = GetElements(def, value);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample($"new LDtkFieldElements");
            LDtkFieldElement[] fieldElements = new LDtkFieldElement[elements.Length];
            for (int i = 0; i < fieldElements.Length; i++)
            {
                fieldElements[i] = new LDtkFieldElement(elements[i], def);
            }
            LDtkProfiler.EndSample();
            
            return fieldElements;
        }

        private object[] GetElements(FieldDefinition def, object value)
        {
            if (def.IsArray)
            {
                LDtkProfiler.BeginSample("GetArray");
                Array array = GetArray(def, value);
                LDtkProfiler.EndSample();
                
                object[] objArray = new object[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    objArray[i] = array.GetValue(i);
                }
                return objArray;
            }

            LDtkProfiler.BeginSample("GetSingle");
            object single = GetParsedValue(def, value);
            
            LDtkProfiler.EndSample();
            
            return new[] { single };
        }

        private Array GetArray(FieldDefinition def, object value)
        {
            List<object> objs;
            
            if (value is List<object> list)
            {
                objs = list;
            }
            else
            {
                LDtkDebug.LogError($"Not list (was {value.GetType().GetGenericArguments().First().Name}), not populating field instance \"{def.Identifier}\"");
                return Array.Empty<object>();
            }

            //parse em
            LDtkProfiler.BeginSample("CopyArray");
            object[] srcObjs = new object[objs.Count];
            for (int i = 0; i < objs.Count; i++)
            {
                srcObjs[i] = GetParsedValue(def, objs[i]);
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
                LDtkDebug.LogError($"Issue copying array for field instance \"{def.Identifier}\"; LDtk type: {def.Type}, ParsedObjects: {srcObjsStrings}. {e}");
            }
            LDtkProfiler.EndSample();
            
            return array;
        }

        private object GetParsedValue(FieldDefinition field, object value)
        {
            ParseFieldValueAction action = LDtkFieldParser.GetParserMethodForType(field);
            
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