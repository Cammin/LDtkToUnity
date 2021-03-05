using System;
using System.Collections;
using System.Linq;
using LDtkUnity;
using Newtonsoft.Json;
using NUnit.Framework;
using Samples.TestAllFields;
using Samples.TestAllFields.TestAllFields_Data;
using UnityEngine;

namespace Tests.Editor
{
    public class FieldDeserializeTest
    {
        [Test] public void DeserializeFieldInt() => DeserializeField("Int", typeof(int));
        [Test] public void DeserializeFieldFloat() => DeserializeField("Float", typeof(float));
        [Test] public void DeserializeFieldBool() => DeserializeField("Bool", typeof(bool));
        [Test] public void DeserializeFieldString() => DeserializeField("String", typeof(string));
        [Test] public void DeserializeFieldEnum() => DeserializeField("LocalEnum", typeof(SomeEnum));
        [Test] public void DeserializeFieldColor() => DeserializeField("Color", typeof(string));
        [Test] public void DeserializeFieldPoint() => DeserializeField("Point", typeof(string));
        [Test] public void DeserializeFieldFilePath() => DeserializeField("FilePath", typeof(string));
        
        [Test] public void DeserializeFieldIntArray() => DeserializeField("Array<Int>", typeof(int[]));
        [Test] public void DeserializeFieldFloatArray() => DeserializeField("Array<Float>", typeof(float[]));
        [Test] public void DeserializeFieldBoolArray() => DeserializeField("Array<Bool>", typeof(bool[]));
        [Test] public void DeserializeFieldStringArray() => DeserializeField("Array<String>", typeof(string[]));
        [Test] public void DeserializeFieldEnumArray() => DeserializeField("Array<LocalEnum", typeof(SomeEnum));
        [Test] public void DeserializeFieldColorArray() => DeserializeField("Array<Color>", typeof(string[]));
        [Test] public void DeserializeFieldPointArray() => DeserializeField("Array<Point>", typeof(string[]));
        [Test] public void DeserializeFieldFilePathArray() => DeserializeField("Array<FilePath>", typeof(string[]));

        private void DeserializeField(string key, Type type)
        {
            TextAsset fieldAsset = TestJsonLoader.LoadJson($"/LDtkMockField_Project.json");
            
            //try deserializing field
            LdtkJson field = JsonConvert.DeserializeObject<LdtkJson>(fieldAsset.text);

            FieldInstance[] fieldInstances = field.Levels[0].LayerInstances[0].EntityInstances[0].FieldInstances;

            FieldInstance instance = fieldInstances.First(p =>  p.Type.Contains(key));

            
            
            if (instance.Type.Contains("Array"))
            {
                object[] objs = ((IEnumerable) instance.Value).Cast<object>()
                    .Select(x => x == null ? x : x.ToString())
                    .ToArray();
                foreach (object o in objs)
                {
                    object obj = GetObject(type, instance.Type, o);   
                    Debug.Log(obj);
                }
            }
            else
            {
                object obj = GetObject(type, instance.Type, instance.Value);   
                Debug.Log(obj);
            }
        }

        private static object GetObject(Type type, string instanceTypeName, object value)
        {
            if (instanceTypeName.Contains("LocalEnum"))
            {
                return LDtkFieldParser.GetEnumMethod(type).Invoke(value);
            }
            else
            {
                return LDtkFieldParser.GetParserMethodForType(instanceTypeName).Invoke(value);
            }
        }
    }
}