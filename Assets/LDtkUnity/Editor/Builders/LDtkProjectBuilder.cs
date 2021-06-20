using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    public class LDtkProjectBuilder
    {
        private readonly LDtkProjectImporter _importer;
        private readonly LdtkJson _projectData;

        public GameObject RootObject { get; private set; } = null;
        public GameObject[] LevelObjects { get; private set; } = new GameObject[0];
        
        public LDtkProjectBuilder(LDtkProjectImporter importer, LdtkJson projectData)
        {
            _importer = importer;
            _projectData = projectData;
        }

        public void BuildProject()
        {
            if (!TryCanBuildProject())
            {
                return;
            }
            
            LDtkUidBank.CacheUidData(_projectData);
            BuildProcess();
            LDtkUidBank.DisposeDefinitions();
        }

        private bool TryCanBuildProject()
        {
            if (_importer == null)
            {
                Debug.LogError("LDtk: Project was null, not building project.");
                return false;
            }

            if (_importer.JsonFile == null)
            {
                Debug.LogError("LDtk: Project File was null, not building project.", _importer);
                return false;
            }

            if (_projectData == null)
            {
                Debug.LogError("LDtk: ProjectJson was null, not building project.", _importer);
                return false;
            }

            if (_projectData.Levels.IsNullOrEmpty())
            {
                Debug.LogError("LDtk: No levels specified, not building project.", _importer);
                return false;
            }

            return true;
        }

        private void BuildProcess()
        {
            CreateRootObject();

            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            List<GameObject> levelObjects = new List<GameObject>();
            foreach (Level lvl in _projectData.Levels)
            {
                LDtkLevelBuilder levelBuilder = new LDtkLevelBuilder(_importer, _projectData, lvl);
                
                GameObject levelObj = levelBuilder.BuildLevel();
                levelObj.transform.parent = RootObject.transform;
                
                levelObjects.Add(levelObj);
            }

            LevelObjects = levelObjects.ToArray();

            levelBuildTimer.Stop();

            if (LDtkPrefs.LogBuildTimes && _projectData.Levels.Length > 1)
            {
                double ms = levelBuildTimer.ElapsedMilliseconds;
                Debug.Log($"LDtk: Built levels in {ms}ms ({ms / 1000}s)");
            }
        }

        private void CreateRootObject()
        {
            RootObject = new GameObject(_importer.AssetName);

            LDtkComponentProject component = RootObject.AddComponent<LDtkComponentProject>();
            component.SetJson(_importer.JsonFile);


            if (_importer.DeparentInRuntime)
            {
                RootObject.AddComponent<LDtkDetachChildren>();
            }
        }
    }
}