using System.Linq;
using LDtkUnity;
using LDtkUnity.Data;
using NUnit.Framework;
using UnityEngine;

namespace Tests.Editor
{
    public class BoundsTest
    {
        [Test]
        public void GetLevelBounds()
        {
            const string lvlName = "Level";
            
            TextAsset jsonProject = TestJsonLoader.LoadGenericProject();
            LdtkJson project = LdtkJson.FromJson(jsonProject.text);
            Level level = project.Levels.FirstOrDefault(p => p.Identifier == lvlName);
            LayerInstance layer = level.LayerInstances.FirstOrDefault(p => p.IsIntGridLayer);
            Bounds bounds = layer.UnityWorldBounds;
            
            Debug.Log(bounds);
        }
    }
}
