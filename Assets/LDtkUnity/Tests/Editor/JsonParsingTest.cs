using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

namespace LDtkUnity.Tests.Editor
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
            LDtkDataProject project = LDtkToolProjectLoader.LoadProject(jsonProject.text);
        }

        [Test]
        public void JsonDeserializeEntityInstance()
        {
            TextAsset jsonProject = TestJsonLoader.LoadJson(TestJsonLoader.MOCK_ENTITY_INSTANCE);
            
            //try deserializing entity
            LDtkDataEntity entity = JsonConvert.DeserializeObject<LDtkDataEntity>(jsonProject.text);
        }
    }
}
