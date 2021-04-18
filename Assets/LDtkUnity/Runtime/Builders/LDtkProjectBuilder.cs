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

        public GameObject RootObject { get; private set; } = null;
        public GameObject[] LevelObjects { get; private set; } = new GameObject[0];

        public LDtkProjectBuilder(LDtkProject project, LdtkJson projectData)
        {
            _project = project;
            _projectData = projectData;
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
            
            if (_projectData.Levels.NullOrEmpty())
            {
                Debug.LogError("LDtk: No levels specified, not building project.");
                return;
            }

            InitStaticTools();
            BuildProcess(logBuildTimes);
            DisposeStaticTools();
        }

        private void BuildProcess(bool logBuildTimes)
        {
            CreateRootObject();

            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            List<GameObject> levelObjects = new List<GameObject>();
            foreach (Level fileLevel in _projectData.Levels)
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

            if (logBuildTimes && _projectData.Levels.Length > 1)
            {
                double ms = levelBuildTimer.ElapsedMilliseconds;
                Debug.Log($"LDtk: Built levels in {ms}ms ({ms / 1000}s)");
            }
        }

        private void CreateRootObject()
        {
            RootObject = new GameObject(_project.ProjectJson.name);

            LDtkComponentProjectFile component = RootObject.AddComponent<LDtkComponentProjectFile>();
            component.SetJson(_projectData);


            if (_project.DeparentInRuntime)
            {
                RootObject.AddComponent<LDtkDetachGameObject>();
            }
        }

        public void InitStaticTools()
        {
            LDtkUidBank.CacheUidData(_projectData);
            LDtkProviderErrorIdentifiers.Init();
        }
        public void DisposeStaticTools()
        {
            LDtkUidBank.DisposeDefinitions(); 
            LDtkProviderErrorIdentifiers.Dispose();
        }
    }
}