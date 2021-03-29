using System.Collections.Generic;
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

        public GameObject RootObject { get; private set; } = null;
        public GameObject[] LevelObjects { get; private set; } = new GameObject[0];

        public LDtkProjectBuilder(LDtkProject project, LdtkJson projectData, Level[] levels)
        {
            _project = project;
            _projectData = projectData;
            _levels = levels;
        }

        public void BuildProject(bool logBuildTimes)
        {
            if (_project == null)
            {
                Debug.LogError("LDtk: Project was null, not building project.");
                return;
            }
            
            if (_project.ProjectJson == null)
            {
                Debug.LogError("LDtk: Project File was null, not building project.");
                return;
            }
            
            if (_projectData == null)
            {
                Debug.LogError("LDtk: ProjectJson was null, not building project.");
                return;
            }
            
            if (_levels.NullOrEmpty())
            {
                Debug.LogError("LDtk: No levels specified, not building project.");
                return;
            }

            RootObject = new GameObject(_project.ProjectJson.name);

            if (_project.DeparentInRuntime)
            {
                RootObject.AddComponent<LDtkDetachGameObject>();
            }
            
            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            List<GameObject> levelObjects = new List<GameObject>();
            foreach (Level fileLevel in _levels)
            {
                LDtkLevelBuilder levelBuilder = new LDtkLevelBuilder(_project, _projectData, fileLevel);
                GameObject level = levelBuilder.BuildLevel(logBuildTimes);
                level.transform.parent = RootObject.transform;

                if (_project.DeparentInRuntime)
                {
                    level.AddComponent<LDtkDetachGameObject>();
                }
                
                levelObjects.Add(level);
            }

            LevelObjects = levelObjects.ToArray();

            levelBuildTimer.Stop();

            if (logBuildTimes && _levels.Length > 1)
            {
                double ms = levelBuildTimer.ElapsedMilliseconds;
                Debug.Log($"LDtk: Built levels in {ms}ms ({ms/1000}s)");
            }
        }
    }
}