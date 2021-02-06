using System.Diagnostics;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Builders
{
    public static class LDtkProjectBuilder
    {
        public static GameObject BuildProject(LDtkProject project, LdtkJson projectData, Level[] levels, bool disposeDefinitionMemoryAfterBuilt = false)
        {
            if (project == null)
            {
                Debug.LogError("LDtk: Project was null, not building project.");
                return null;
            }
            
            if (project.ProjectJson == null)
            {
                Debug.LogError("LDtk: Project File was null, not building project.");
                return null;
            }
            
            if (projectData == null)
            {
                Debug.LogError("LDtk: ProjectJson was null, not building project.");
                return null;
            }
            
            if (levels.NullOrEmpty())
            {
                Debug.LogError("LDtk: No levels specified, not building project.");
                return null;
            }

            GameObject projectRoot = new GameObject(project.ProjectJson.name);
            
            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            foreach (Level fileLevel in levels)
            {
                GameObject level = LDtkLevelBuilder.BuildLevel(project, projectData, fileLevel, disposeDefinitionMemoryAfterBuilt);
                level.transform.parent = projectRoot.transform;
            }

            levelBuildTimer.Stop();

            if (levels.Length > 1)
            {
                double ms = levelBuildTimer.ElapsedMilliseconds;
                Debug.Log($"LDtk: Built levels in {ms}ms ({ms/1000}s)");
            }

            return projectRoot;
        }
    }
}