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
            
            TextAsset jsonProject = TestUtil.LoadJson(TestUtil.PROJECT_PATH);
            LDtkDataProject project = LDtkToolProjectLoader.LoadProject(jsonProject.text);
            LDtkDataLevel level = project.levels.FirstOrDefault(p => p.identifier == lvlName);

            Bounds bounds = LDtkToolBoundsCalculator.GetLevelBounds(level, project.defaultGridSize);
            Debug.Log(bounds);

        }
    }
}
