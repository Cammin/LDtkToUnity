using LDtkUnity.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class JsonParsingTest
    {
        private const string BASIC_PROJECT = "TestBasicProject.json";
        
        [Test]
        public void JsonDeserializeProject()
        {
            TextAsset jsonProject = TestJsonLoader.LoadJson(BASIC_PROJECT);
            Assert.NotNull(jsonProject, "Unsuccessful acquirement of json text asset");

            //attempt deserializing entire project
            LdtkJson project = LdtkJson.FromJson(jsonProject.text);
        }
        
        [Test]
        public void JsonDeserializeSchemaProject()
        {
            TextAsset jsonProject = TestJsonLoader.LoadJson(BASIC_PROJECT);
            Assert.NotNull(jsonProject, "Unsuccessful acquirement of json text asset");

            //attempt deserializing entire project
            LdtkJson project = LdtkJson.FromJson(jsonProject.text);
        }

        [Test]
        public void JsonDeserializeEntityInstance()
        {
            TextAsset jsonProject = TestJsonLoader.LoadJson(TestJsonLoader.MOCK_ENTITY_INSTANCE);
            
            //try deserializing entity
            EntityInstance entity = JsonConvert.DeserializeObject<EntityInstance>(jsonProject.text);
        }
    }
}
