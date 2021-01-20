using LDtkUnity;
using LDtkUnity.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class FieldDeserializeTest
    {
        [Test] public void DeserializeFieldInt() => DeserializeField("Int");
        [Test] public void DeserializeFieldFloat() => DeserializeField("Float");
        [Test] public void DeserializeFieldBool() => DeserializeField("Bool");
        [Test] public void DeserializeFieldString() => DeserializeField("String");
        [Test] public void DeserializeFieldEnum() => DeserializeField("Enum");
        [Test] public void DeserializeFieldColor() => DeserializeField("Color");
        [Test] public void DeserializeFieldPoint() => DeserializeField("Point");
        [Test] public void DeserializeFieldIntArray() => DeserializeField("IntArray");
        [Test] public void DeserializeFieldFloatArray() => DeserializeField("FloatArray");
        [Test] public void DeserializeFieldBoolArray() => DeserializeField("BoolArray");
        [Test] public void DeserializeFieldStringArray() => DeserializeField("StringArray");
        [Test] public void DeserializeFieldEnumArray() => DeserializeField("EnumArray");
        [Test] public void DeserializeFieldColorArray() => DeserializeField("ColorArray");
        [Test] public void DeserializeFieldPointArray() => DeserializeField("PointArray");

        private void DeserializeField(string key)
        {
            TextAsset fieldAsset = TestJsonLoader.LoadJson($"/LDtkMockField_{key}.json");
            
            //try deserializing field
            FieldInstance field = JsonConvert.DeserializeObject<FieldInstance>(fieldAsset.text);
            
            string identifier = field.Identifier;
            string type = field.Type;
            string[] values = (string[])field.Value;
            
            Debug.Log($"identifier: {identifier}");
            Debug.Log($"type: {type}");
            Debug.Log($"values: [\"{string.Join("\"], [\"", values)}\"]\n(Square brackets don't actually exist in the string; is only visual and represents the string literals)");

            Assert.False(values == null, "Field string array was null. Maybe this should not actually trigger failure.");
        }
    }
}