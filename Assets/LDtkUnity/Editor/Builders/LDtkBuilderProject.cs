using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkProjectBuilder
    {
        private readonly LDtkProjectImporter _importer;
        private readonly LdtkJson _json;
        private readonly World[] _worlds;
        private LDtkPostProcessorCache _actions;

        public GameObject RootObject { get; private set; } = null;
        
        
        public LDtkProjectBuilder(LDtkProjectImporter importer, LdtkJson json)
        {
            _importer = importer;
            _json = json;
            _worlds = _json.UnityWorlds;
        }

        public void BuildProject()
        {
            if (!TryCanBuildProject())
            {
                return;
            }
            
            LDtkIidComponentBank.Release();
            
            _actions = new LDtkPostProcessorCache();
            BuildProcess();
            _actions.PostProcess();
            
            LDtkIidComponentBank.Release();
        }

        private bool TryCanBuildProject()
        {
            if (_importer == null)
            {
                LDtkDebug.LogError("Project was null, not building project.");
                return false;
            }

            if (_importer.JsonFile == null)
            {
                LDtkDebug.LogError("Project File was null, not building project.", _importer);
                return false;
            }

            if (_json == null)
            {
                LDtkDebug.LogError("ProjectJson was null, not building project.", _importer);
                return false;
            }

            if (_worlds.IsNullOrEmpty())
            {
                LDtkDebug.LogError("No levels specified, not building project.", _importer);
                return false;
            }

            return true;
        }

        private void BuildProcess()
        {
            CreateRootObject();
            BuildWorlds();
            AddProjectPostProcess();
        }
        
        private void BuildWorlds()
        {
            foreach (World world in _worlds)
            {
                Profiler.BeginSample("SetParent World to root");
                GameObject worldObj = new GameObject(world.Identifier);
                worldObj.transform.SetParent(RootObject.transform);
                Profiler.EndSample();

                Profiler.BeginSample($"BuildWorld {world.Identifier}");
                LDtkBuilderWorld worldBuilder = new LDtkBuilderWorld(worldObj, _importer, _json, world, _actions);
                worldBuilder.BuildWorld();
                Profiler.EndSample();
            }
        }

        private void AddProjectPostProcess()
        {
            LDtkPostProcessorInvoker.AddPostProcessProject(_actions, RootObject);
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