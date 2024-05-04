using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Tests
{
    public class TestImport
    {
        [Test]
        public void ImportProject()
        {
            string path = "Assets/Samples/Samples/AutoLayers_1_basic.ldtk";
            AssertAsset(path);
        }
        
        [Test]
        public void ImportLevel()
        {
            string path = "Assets/Samples/Samples/SeparateLevelFiles/World_Level_0.ldtkl";
            AssertAsset(path);
        }

        private void AssertAsset(string path)
        {
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            
            Object loadAssetAtPath = AssetDatabase.LoadAssetAtPath<Object>(path);
            Assert.NotNull(loadAssetAtPath);
        }
    }
}