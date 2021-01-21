using System;
using LDtkUnity;
using LDtkUnity.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class FieldDeserializeTest
    {
        [Test] public void DeserializeFieldInt() => DeserializeField("Int", typeof(int));
        [Test] public void DeserializeFieldFloat() => DeserializeField("Float", typeof(float));
        [Test] public void DeserializeFieldBool() => DeserializeField("Bool", typeof(bool));
        [Test] public void DeserializeFieldString() => DeserializeField("String", typeof(string));
        [Test] public void DeserializeFieldEnum() => DeserializeField("LocalEnum", typeof(string));
        [Test] public void DeserializeFieldColor() => DeserializeField("Color", typeof(string));
        [Test] public void DeserializeFieldPoint() => DeserializeField("Point", typeof(string));
        [Test] public void DeserializeFieldFilePath() => DeserializeField("FilePath", typeof(string));
        
        [Test] public void DeserializeFieldIntArray() => DeserializeField("Array<Int>", typeof(int[]));
        [Test] public void DeserializeFieldFloatArray() => DeserializeField("Array<Float>", typeof(float[]));
        [Test] public void DeserializeFieldBoolArray() => DeserializeField("Array<Bool>", typeof(bool[]));
        [Test] public void DeserializeFieldStringArray() => DeserializeField("Array<String>", typeof(string[]));
        [Test] public void DeserializeFieldEnumArray() => DeserializeField("Array<LocalEnum>", typeof(string[]));
        [Test] public void DeserializeFieldColorArray() => DeserializeField("Array<Color>", typeof(string[]));
        [Test] public void DeserializeFieldPointArray() => DeserializeField("Array<Point>", typeof(string[]));
        [Test] public void DeserializeFieldFilePathArray() => DeserializeField("Array<FilePath>", typeof(string[]));

        private void DeserializeField(string key, Type type)
        {
            TextAsset fieldAsset = TestJsonLoader.LoadJson($"/LDtkMockField_Project.json");
            
            //try deserializing field
            LdtkJson field = JsonConvert.DeserializeObject<LdtkJson>(fieldAsset.text);

            FieldInstance[] fieldInstances = field.Levels[0].LayerInstances[0].EntityInstances[0].FieldInstances;

            foreach (FieldInstance fieldInstance in fieldInstances)
            {
                Debug.Log(fieldInstance.Type);
            }

            /*string identifier = field.Identifier;
            //string type = field.Type;
            object value = field.Value;
            
            Debug.Log($"identifier: {identifier}");
            //Debug.Log($"type: {type}");
            Debug.Log($"values: {value}");

            Assert.NotNull(value, "Field string array was null. Maybe this should not actually trigger failure.");
            
            Assert.IsAssignableFrom(type, value, "Object not assignable to type.");*/
        }
        
        
    }
}