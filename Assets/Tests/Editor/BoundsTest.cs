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
            LDtkDataProject project = LDtkLoader.DeserializeJson(jsonProject.text);
            LDtkDataLevel level = project.levels.FirstOrDefault(p => p.identifier == lvlName);
            LDtkDataLayer layer = level.layerInstances.FirstOrDefault(p => p.IsIntGridLayer());
            Bounds bounds = layer.LayerUnitBounds();
            
            Debug.Log(bounds);
        }
    }
}
