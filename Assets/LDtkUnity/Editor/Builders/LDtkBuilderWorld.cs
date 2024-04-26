using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderWorld
    {
        private readonly LDtkProjectImporter _project;
        private readonly LdtkJson _json;
        private readonly World _world;
        private readonly LDtkAssetProcessorActionCache _assetProcess;
        private readonly LDtkLinearLevelVector _linearVector = new LDtkLinearLevelVector();

        private readonly GameObject _worldObject;
        private LDtkComponentWorld _worldComponent;
        private LDtkComponentProject _projectComponent;
        private LDtkIid _iidComponent;

        public LDtkBuilderWorld(GameObject worldObj, LDtkProjectImporter project, LdtkJson json, World world, LDtkAssetProcessorActionCache assetProcess, LDtkComponentProject projectComponent)
        {
            _worldObject = worldObj;
            _project = project;
            _json = json;
            _world = world;
            _assetProcess = assetProcess;
            _projectComponent = projectComponent;
        }
        
        public LDtkComponentWorld BuildWorld()
        {
            AddComponents();

            LDtkComponentLevel[] levels = null;
            
            //don't make levels if we are using separate levels
            if (!_json.ExternalLevels)
            {
                levels = BuildLevels();
            }
            
            InitWorldObject(levels);

            return _worldComponent;
        }

        private LDtkComponentLevel[] BuildLevels()
        {
            LDtkComponentLevel[] levels;
            levels = new LDtkComponentLevel[_world.Levels.Length];
            for (int i = 0; i < _world.Levels.Length; i++)
            {
                Level lvl = _world.Levels[i];
                WorldLayout layout = _world.WorldLayout.HasValue ? _world.WorldLayout.Value : WorldLayout.Free;
                LDtkBuilderLevel levelBuilder = new LDtkBuilderLevel(_project, _json, layout, lvl, null, _assetProcess, _project, _worldComponent, _linearVector);

                Profiler.BeginSample("SetParent Level to World");
                GameObject levelObj = levelBuilder.StubGameObject();
                levelObj.transform.SetParent(_worldObject.transform);
                Profiler.EndSample();

                Profiler.BeginSample($"BuildLevel {lvl.Identifier}");
                LDtkComponentLevel levelComponent = levelBuilder.BuildLevel();
                Profiler.EndSample();

                levels[i] = levelComponent;
            }
            return levels;
        }

        private void AddComponents()
        {
            _worldComponent = _worldObject.AddComponent<LDtkComponentWorld>();
            _iidComponent = _worldObject.AddComponent<LDtkIid>();
        }
        
        private void InitWorldObject(LDtkComponentLevel[] levels)
        {
            _iidComponent.SetIid(_world);
            _worldComponent.OnImport(_world, levels, _projectComponent, _iidComponent);
        }
    }
}