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
            LDtkAssetProcessorInvoker.AddPostProcessProject(_actions, RootObject);
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
            foreach (World world in _worlds)
            {
                Profiler.BeginSample("SetParent World to root");
                GameObject worldObj = new GameObject(world.Identifier);
                worldObj.transform.SetParent(RootObject.transform);
                Profiler.EndSample();

                Profiler.BeginSample($"BuildWorld {world.Identifier}");
                LDtkBuilderWorld worldBuilder = new LDtkBuilderWorld(worldObj, _project, _json, world, _actions, _project);
                worldBuilder.BuildWorld();
                Profiler.EndSample();
            }
        }

        private void CreateRootObject()
        {
            RootObject = new GameObject(_project.AssetName);

            LDtkComponentProject component = RootObject.AddComponent<LDtkComponentProject>();
            component.SetJson(_project.JsonFile);
            
            LDtkIid iid = RootObject.AddComponent<LDtkIid>();
            iid.SetIid(_json);
        }
    }
}