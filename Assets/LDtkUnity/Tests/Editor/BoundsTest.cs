using System.Linq;
using LDtkUnity.Runtime.Data;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.Tools;
using NUnit.Framework;
using UnityEngine;

namespace LDtkUnity.Tests.Editor
{
    public class BoundsTest
    {
        [Test]
        public void GetLevelBounds()
        {
            const string lvlName = "Level";
            
            TextAsset jsonProject = TestJsonLoader.LoadJson(TestJsonLoader.GENERIC_PROJECT_PATH);
            LDtkDataProject project = LDtkToolProjectLoader.LoadProject(jsonProject.text);
            LDtkDataLevel level = project.levels.FirstOrDefault(p => p.identifier == lvlName);
            LDtkDataLayer layer = level.layerInstances.FirstOrDefault(p => p.IsIntGridLayer);
            Bounds bounds = layer.LayerUnitBounds;
            
            Debug.Log(bounds);
        }
        
        
        
    }
}
