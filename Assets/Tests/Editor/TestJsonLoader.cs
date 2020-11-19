using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.Editor
{
    public static class TestJsonLoader
    {
        private const string GENERIC_PROJECT_PATH = "TestProject.json";
        
        public const string MOCK_ENTITY_INSTANCE = "LDtkMockEntity.json";
        

        private const string TEST_PATH = "/Tests/Editor/";


        public static TextAsset LoadGenericProject() => LoadJson(GENERIC_PROJECT_PATH);
        public static TextAsset LoadJson(string path)
        {
            TextAsset jsonProject = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets" + TEST_PATH + path);
            
            Assert.NotNull(jsonProject, "Unsuccessful acquirement of json text asset");
            return jsonProject;
        }
    }
}