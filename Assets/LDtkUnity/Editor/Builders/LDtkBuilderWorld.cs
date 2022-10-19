using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderWorld
    {
        private readonly LDtkProjectImporter _importer;
        private readonly LdtkJson _json;
        private readonly World _world;
        private readonly LDtkPostProcessorCache _postProcess;
        private readonly LDtkLinearLevelVector _linearVector = new LDtkLinearLevelVector();

        private readonly GameObject _worldObject;

        public LDtkBuilderWorld(GameObject worldObj, LDtkProjectImporter importer, LdtkJson json, World world, LDtkPostProcessorCache postProcess)
        {
            _worldObject = worldObj;
            _importer = importer;
            _json = json;
            _world = world;
            _postProcess = postProcess;
        }
        
        public void BuildWorld()
        {
            InitWorldObject();
            foreach (Level lvl in _world.Levels)
            {
                WorldLayout layout = _world.WorldLayout.HasValue ? _world.WorldLayout.Value : WorldLayout.Free;
                LDtkBuilderLevel levelBuilder = new LDtkBuilderLevel(_importer, _json, layout, lvl, _postProcess, _linearVector);
                
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
            LDtkIid iid = _worldObject.AddComponent<LDtkIid>();
            iid.SetIid(_world);

            if (_importer.DeparentInRuntime)
            {
                _worldObject.AddComponent<LDtkDetachChildren>();
            }
        }
    }
}