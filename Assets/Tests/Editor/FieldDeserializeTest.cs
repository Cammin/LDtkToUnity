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
        [Test] public void DeserializeFieldEnum() => DeserializeField("Enum", typeof(string));
        [Test] public void DeserializeFieldColor() => DeserializeField("Color", typeof(string));
        [Test] public void DeserializeFieldPoint() => DeserializeField("Point", typeof(string));
        [Test] public void DeserializeFieldIntArray() => DeserializeField("IntArray", typeof(int[]));
        [Test] public void DeserializeFieldFloatArray() => DeserializeField("FloatArray", typeof(float[]));
        [Test] public void DeserializeFieldBoolArray() => DeserializeField("BoolArray", typeof(bool[]));
        [Test] public void DeserializeFieldStringArray() => DeserializeField("StringArray", typeof(string[]));
        [Test] public void DeserializeFieldEnumArray() => DeserializeField("EnumArray", typeof(string[]));
        [Test] public void DeserializeFieldColorArray() => DeserializeField("ColorArray", typeof(string[]));
        [Test] public void DeserializeFieldPointArray() => DeserializeField("PointArray", typeof(string[]));

        private void DeserializeField(string key, Type type)
        {
            TextAsset fieldAsset = TestJsonLoader.LoadJson($"/LDtkMockField_{key}.json");
            
            //try deserializing field
            FieldInstance field = JsonConvert.DeserializeObject<FieldInstance>(fieldAsset.text);
            
            string identifier = field.Identifier;
            //string type = field.Type;
            object value = field.Value;
            
            Debug.Log($"identifier: {identifier}");
            //Debug.Log($"type: {type}");
            Debug.Log($"values: {value}");

            Assert.NotNull(value, "Field string array was null. Maybe this should not actually trigger failure.");
            
            Assert.IsAssignableFrom(type, value, "Object not assignable to type.");
        }
    }
}