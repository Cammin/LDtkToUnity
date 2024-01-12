using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderWorld
    {
        private readonly LDtkProjectImporter _project;
        private readonly LDtkJsonImporter _importer;
        private readonly LdtkJson _json;
        private readonly World _world;
        private readonly LDtkAssetProcessorActionCache _assetProcess;
        private readonly LDtkLinearLevelVector _linearVector = new LDtkLinearLevelVector();

        private readonly GameObject _worldObject;

        public LDtkBuilderWorld(GameObject worldObj, LDtkProjectImporter project, LdtkJson json, World world, LDtkAssetProcessorActionCache assetProcess, LDtkJsonImporter importer)
        {
            _worldObject = worldObj;
            _project = project;
            _json = json;
            _world = world;
            _assetProcess = assetProcess;
            _importer = importer;
        }
        
        public void BuildWorld()
        {
            InitWorldObject();
            foreach (Level lvl in _world.Levels)
            {
                WorldLayout layout = _world.WorldLayout.HasValue ? _world.WorldLayout.Value : WorldLayout.Free;
                LDtkBuilderLevel levelBuilder = new LDtkBuilderLevel(_project, _json, layout, lvl, _assetProcess, _importer, _linearVector);
                
                Profiler.BeginSample("SetParent Level to World");
                GameObject levelObj = levelBuilder.StubGameObject();
                levelObj.transform.SetParent(_worldObject.transform);
                Profiler.EndSample();

                Profiler.BeginSample($"BuildLevel {lvl.Identifier}");
                levelBuilder.BuildLevel();
                Profiler.EndSample();
                
            }
        }
        
        private void InitWorldObject()
        {
            LDtkComponentWorld world = _worldObject.AddComponent<LDtkComponentWorld>();
            world.Setup(_world);

            LDtkIid iid = _worldObject.AddComponent<LDtkIid>();
            iid.SetIid(_world);
        }
    }
}