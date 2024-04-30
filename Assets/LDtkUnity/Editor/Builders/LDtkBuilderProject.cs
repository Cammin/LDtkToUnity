using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkProjectBuilder
    {
        private readonly LDtkProjectImporter _project;
        private readonly LdtkJson _json;
        private readonly World[] _worlds;
        private LDtkAssetProcessorActionCache _actions;
        private LDtkComponentProject _projectComponent;
        private LDtkIid _iidComponent;

        public GameObject RootObject { get; private set; } = null;
        
        
        public LDtkProjectBuilder(LDtkProjectImporter importer, LdtkJson json)
        {
            _project = importer;
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
            
            _actions = new LDtkAssetProcessorActionCache();
            CreateRootObject();
            BuildWorlds();
            LDtkAssetProcessorInvoker.AddPostProcessProject(_actions, _project, RootObject);
            _actions.Process();
            
            LDtkIidComponentBank.Release();
        }

        private bool TryCanBuildProject()
        {
            if (_project == null)
            {
                LDtkDebug.LogError("Project was null, not building project.");
                return false;
            }

            if (_project.JsonFile == null)
            {
                LDtkDebug.LogError("Project File was null, not building project.", _project);
                return false;
            }

            if (_json == null)
            {
                LDtkDebug.LogError("ProjectJson was null, not building project.", _project);
                return false;
            }

            if (_worlds.IsNullOrEmpty())
            {
                LDtkDebug.LogError("No levels specified, not building project.", _project);
                return false;
            }

            return true;
        }
        
        private void BuildWorlds()
        {
            var worlds = new LDtkComponentWorld[_worlds.Length];
            for (int i = 0; i < _worlds.Length; i++)
            {
                World world = _worlds[i];
                Profiler.BeginSample("SetParent World to root");
                GameObject worldObj = new GameObject(world.Identifier);
                worldObj.transform.SetParent(RootObject.transform);
                Profiler.EndSample();

                Profiler.BeginSample($"BuildWorld {world.Identifier}");
                LDtkBuilderWorld worldBuilder = new LDtkBuilderWorld(worldObj, _project, _json, world, _actions, _projectComponent);
                LDtkComponentWorld worldComponent = worldBuilder.BuildWorld();
                Profiler.EndSample();

                worlds[i] = worldComponent;
            }
            
            _projectComponent.OnImport(_json, _project.JsonFile, _project.Toc, worlds, _iidComponent);
        }
        
        private void CreateRootObject()
        {
            RootObject = new GameObject(_project.AssetName);

            _projectComponent = RootObject.AddComponent<LDtkComponentProject>();
            
            _iidComponent = RootObject.AddComponent<LDtkIid>();
            _iidComponent.SetIid(_json);
        }
    }
}