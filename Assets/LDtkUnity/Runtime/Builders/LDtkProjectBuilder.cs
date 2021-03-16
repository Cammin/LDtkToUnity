using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LDtkUnity
{
    public class LDtkProjectBuilder
    {
        private readonly LDtkProject _project;
        private readonly LdtkJson _projectData;
        private readonly Level[] _levels;

        public LDtkProjectBuilder(LDtkProject project, LdtkJson projectData, Level[] levels)
        {
            _project = project;
            _projectData = projectData;
            _levels = levels;
        }

        public GameObject BuildProject()
        {
            if (_project == null)
            {
                Debug.LogError("LDtk: Project was null, not building project.");
                return null;
            }
            
            if (_project.ProjectJson == null)
            {
                Debug.LogError("LDtk: Project File was null, not building project.");
                return null;
            }
            
            if (_projectData == null)
            {
                Debug.LogError("LDtk: ProjectJson was null, not building project.");
                return null;
            }
            
            if (_levels.NullOrEmpty())
            {
                Debug.LogError("LDtk: No levels specified, not building project.");
                return null;
            }

            GameObject projectRoot = new GameObject(_project.ProjectJson.name);
            
            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            foreach (Level fileLevel in _levels)
            {
                GameObject level = new LDtkLevelBuilder(_project, _projectData, fileLevel).BuildLevel();
                level.transform.parent = projectRoot.transform;
            }

            levelBuildTimer.Stop();

            if (_levels.Length > 1)
            {
                double ms = levelBuildTimer.ElapsedMilliseconds;
                Debug.Log($"LDtk: Built levels in {ms}ms ({ms/1000}s)");
            }

            return projectRoot;
        }
    }
}