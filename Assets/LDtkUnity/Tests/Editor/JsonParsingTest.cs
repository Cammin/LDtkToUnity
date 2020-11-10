using System;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection;
using LDtkUnity.Runtime.Tools;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Tests.Editor
{
    public class JsonParsingTest
    {

        
        [Test]
        public void JsonDeserializeProject()
        {
            TextAsset jsonProject = TestUtil.LoadJson(TestUtil.BASIC_PROJECT_PATH);
            Assert.NotNull(jsonProject, "Unsuccessful acquirement of json text asset");

            //attempt deserializing entire project
            LDtkDataProject project = LDtkProjectLoader.LoadProject(jsonProject.text);
        }

        [Test]
        public void JsonDeserializeEntityInstance()
        {
            TextAsset jsonProject = TestUtil.LoadJson(TestUtil.MOCK_ENTITY_INSTANCE);
            
            //try deserializing entity
            LDtkDataEntityInstance entity = JsonConvert.DeserializeObject<LDtkDataEntityInstance>(jsonProject.text);
        }

        
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
            TextAsset fieldAsset = TestUtil.LoadJson(TestUtil.MockFieldPath(key));
            
            //try deserializing field
            LDtkDataFieldInstance field = JsonConvert.DeserializeObject<LDtkDataFieldInstance>(fieldAsset.text);
            
            string identifier = field.__identifier;
            string type = field.__type;
            string[] values = field.__value;
            
            Debug.Log($"identifier: {identifier}");
            Debug.Log($"type: {type}");
            Debug.Log($"values: [\"{string.Join("\"], [\"", values)}\"]\n(Square brackets don't actually exist in the string; is only visual and represents the string literals)");

            Assert.False(values.NullOrEmpty(), "Field string array was null. Maybe this should not actually trigger failure.");
        }
        
        
    }
}
