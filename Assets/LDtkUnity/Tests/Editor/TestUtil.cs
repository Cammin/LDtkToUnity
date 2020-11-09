using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Tests.Editor
{
    public static class TestUtil
    {
        public const string BASIC_PROJECT_PATH = "/LDtkUnity/Tests/Editor/TestBasicProject.json";
        public const string PROJECT_PATH = "/LDtkUnity/Tests/Editor/TestProject.json";
        public const string MOCK_ENTITY_INSTANCE = "/LDtkUnity/Tests/Editor/LDtkMockEntity.json";
        
        public static string MockFieldPath(string name) => $"/LDtkUnity/Tests/Editor/LDtkMockField_{name}.json";
        
        public static TextAsset LoadJson(string path)
        {
            TextAsset jsonProject = AssetDatabase.LoadAssetAtPath<TextAsset>("Packages" + path);
            if (jsonProject == null)
            {
                //then try dev assets
                jsonProject = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets" + path);
            }
            
            Assert.NotNull(jsonProject, "Unsuccessful acquirement of json text asset");
            return jsonProject;
        }
    }
}