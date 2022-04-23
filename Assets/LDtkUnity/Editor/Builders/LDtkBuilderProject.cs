using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    internal class LDtkProjectBuilder
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
            
            LDtkDebug.Reset();
            LDtkUidBank.CacheUidData(_json);
            LDtkIidBank.CacheIidData(_json);
            LDtkIidComponentBank.Release();
            _actions = new LDtkPostProcessorCache();

            BuildProcess();
            
            LDtkUidBank.ReleaseDefinitions();
            LDtkIidBank.Release();
            LDtkIidComponentBank.Release();
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

            if (_json == null)
            {
                Debug.LogError("LDtk: ProjectJson was null, not building project.", _importer);
                return false;
            }

            if (_worlds.IsNullOrEmpty())
            {
                Debug.LogError("LDtk: No levels specified, not building project.", _importer);
                return false;
            }

            return true;
        }

        private void BuildProcess()
        {
            CreateRootObject();
            BuildWorlds();
            InvokeCustomPostProcessing();
        }
        
        private void BuildWorlds()
        {
            foreach (World world in _worlds)
            {
                LDtkBuilderWorld worldBuilder = new LDtkBuilderWorld(_importer, _json, world, _actions);
                GameObject worldObj = worldBuilder.BuildWorld();
                
                worldObj.transform.SetParent(RootObject.transform);
            }
        }

        private void InvokeCustomPostProcessing()
        {
            GameObject rootObject = RootObject;
            
            _actions.AddPostProcessAction(() =>
            {
                LDtkPostProcessorInvoker.PostProcessProject(rootObject);
            });
            
            _actions.PostProcess();
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