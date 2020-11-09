using System;
using System.Linq;
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
    public class BoundsTest
    {
        [Test]
        public void GetLevelBounds()
        {
            const string lvlName = "Level";
            
            TextAsset jsonProject = TestUtil.LoadJson(TestUtil.PROJECT_PATH);
            LDtkDataProject project = LDtkProjectLoader.LoadProject(jsonProject.text);
            LDtkDataLevel level = project.levels.FirstOrDefault(p => p.identifier == lvlName);

            Bounds bounds = LDtkBoundsCalculator.GetLevelBounds(level, project.defaultGridSize);
            Debug.Log(bounds);

        }
    }
}
