using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    internal class LDtkBuilderLevel
    {
        private readonly LDtkProjectImporter _importer;
        private readonly LdtkJson _json;
        private readonly Level _level;
        private readonly WorldLayout _worldLayout;
        private readonly LDtkLinearLevelVector _linearVector;
        private readonly LDtkPostProcessorCache _postProcess;
        private readonly LDtkBuilderDependencies _dependencies;
        
        private GameObject _levelGameObject;
        private LDtkComponentLevel _levelComponent;
        private LDtkFields _fieldsComponent;
        private MonoBehaviour[] _components;
        
        private GameObject _layerGameObject;
        private Grid _layerGrid;

        private LDtkSortingOrder _sortingOrder;
        private LDtkBuilderTileset _builderTileset;
        private LDtkBuilderIntGridValue _builderIntGrid;
        private LDtkBuilderEntity _entityBuilder;
        private LDtkBuilderLevelBackground _backgroundBuilder;

        public LDtkBuilderLevel(LDtkProjectImporter importer, LdtkJson json, WorldLayout world, Level level, LDtkPostProcessorCache postProcess, LDtkBuilderDependencies dependencies, LDtkLinearLevelVector linearVector = null)
        {
            _importer = importer;
            _json = json;
            _level = level;
            _postProcess = postProcess;
            _worldLayout = world;
            _linearVector = linearVector;
            _dependencies = dependencies;
        }

        public GameObject StubGameObject()
        {
            if (!CanTryBuildLevel())
            {
                return null;
            }
            
            return CreateLevelGameObject();
        }
        
        public void BuildLevel()
        {
            Profiler.BeginSample($"BuildLevel {_level.Identifier}");
            if (!CanTryBuildLevel())
            {
                Profiler.EndSample();
                return;
            }
            
            BuildLevelProcess();
            SetupPostProcessing();
            
            Profiler.EndSample();
        }

        private void SetupPostProcessing()
        {
            GameObject levelGameObject = _levelGameObject;
            LdtkJson projectJson = _json;
            _postProcess.AddPostProcessAction(() =>
            {
                LDtkPostProcessorInvoker.PostProcessLevel(levelGameObject, projectJson);
            });
        }

        private bool CanTryBuildLevel()
        {
            if (_importer == null)
            {
                Debug.LogError("LDtk: ProjectAssets object is null; not building level.");
                return false;
            }

            if (_json == null)
            {
                Debug.LogError("LDtk: project data null; not building level.");
                return false;
            }

            if (_level == null)
            {
                Debug.LogError("LDtk: level null; not building level.");
                return false;
            }

            if (_level == null)
            {
                Debug.LogError($"LDtk: LevelToBuild null, not assigned?");
                return false;
            }

            return true;
        }

        private void BuildLevelProcess()
        {
            Profiler.BeginSample("CreateLevelComponent");
            CreateLevelComponent();
            Profiler.EndSample();

            Profiler.BeginSample("AddIidComponent");
            AddIidComponent();
            Profiler.EndSample();
            
            Profiler.BeginSample("AddDetachComponent");
            AddDetachComponent();
            Profiler.EndSample();

            Profiler.BeginSample("new LDtkSortingOrder");
            _sortingOrder = new LDtkSortingOrder();
            Profiler.EndSample();
            
            Profiler.BeginSample("BuildLayerInstances");
            BuildLayerInstances();
            Profiler.EndSample();
            
            Profiler.BeginSample("BuildBackground");
            BuildBackground();
            Profiler.EndSample();
            
            Profiler.BeginSample("BuildFields");
            BuildFields();
            Profiler.EndSample();
            
            Profiler.BeginSample("NextLinearVector");
            NextLinearVector();
            Profiler.EndSample();
        }

        private void BuildFields()
        {
            bool addedFields = TryAddFields();

            MonoBehaviour[] monoBehaviours = _components;
            Level level = _level;
            LDtkFields lDtkFields = _fieldsComponent;
            _postProcess.AddPostProcessAction(() =>
            {
                if (addedFields)
                {
                    LDtkInterfaceEvent.TryEvent<ILDtkImportedFields>(monoBehaviours, levelComponent => levelComponent.OnLDtkImportFields(lDtkFields));
                }
                LDtkInterfaceEvent.TryEvent<ILDtkImportedLevel>(monoBehaviours, levelComponent => levelComponent.OnLDtkImportLevel(level));
            });
        }

        private void AddIidComponent()
        {
            LDtkIid iid = _levelGameObject.AddComponent<LDtkIid>();
            iid.SetIid(_level);
        }

        private GameObject CreateLevelGameObject()
        {
            GetInitialLevelGameObject();
            _levelGameObject.name = _level.Identifier;
            
            int scaler = _linearVector != null ? _linearVector.Scaler : 0;
            _levelGameObject.transform.position = _level.UnityWorldSpaceCoord(_worldLayout, _importer.PixelsPerUnit, scaler);

            _components = _levelGameObject.GetComponents<MonoBehaviour>();
            return _levelGameObject;
        }

        private void GetInitialLevelGameObject()
        {
            GameObject prefab = _importer.CustomLevelPrefab;
            if (prefab != null)
            {
                _levelGameObject = LDtkPrefabFactory.Instantiate(prefab);
                _dependencies.AddDependency(prefab);
                return;
            }
            _levelGameObject = new GameObject();
        }

        private void NextLinearVector()
        {
            if (_linearVector == null)
            {
                return;
            }
            
            switch (_worldLayout)
            {
                case WorldLayout.LinearHorizontal:
                    _linearVector.Next((int)_level.PxWid);
                    break;
                case WorldLayout.LinearVertical:
                    _linearVector.Next((int)_level.PxHei);
                    break;
            }
        }

        private void BuildLayerInstances()
        {
            //build layers and background from front to back in terms of ordering 
            foreach (LayerInstance layer in _level.LayerInstances)
            {
                Profiler.BeginSample($"BuildLayerInstance {layer.Identifier}");
                BuildLayerInstance(layer);
                Profiler.EndSample();
            }
        }

        private void BuildBackground()
        {
            _backgroundBuilder = new LDtkBuilderLevelBackground(_dependencies, _importer, _levelGameObject, _sortingOrder, _level, _levelComponent.Size);
            _backgroundBuilder.BuildBackground();
        }

        private void CreateLevelComponent()
        {
            _levelComponent = _levelGameObject.AddComponent<LDtkComponentLevel>();
            _levelComponent.SetIdentifier(_level.Identifier);
            _levelComponent.SetSize((Vector2)_level.UnityPxSize / _importer.PixelsPerUnit);
            _levelComponent.SetBgColor(_level.UnityBgColor, _level.UnitySmartColor);
            _levelComponent.SetWorldDepth((int)_level.WorldDepth);
        }
        
        private bool TryAddFields()
        {
            if (_json.Defs.LevelFields.IsNullOrEmpty())
            {
                return false;
            }
            
            LDtkFieldParser.CacheRecentBuilder(null);
            LDtkFieldsFactory fieldsFactory = new LDtkFieldsFactory(_levelGameObject, _level.FieldInstances);
            fieldsFactory.SetEntityFieldsComponent();
            _fieldsComponent = fieldsFactory.FieldsComponent;
            return true;
        }


        private void AddDetachComponent()
        {
            if (_importer.DeparentInRuntime)
            {
                _levelGameObject.AddComponent<LDtkDetachChildren>();
            }
        }

        private void BuildLayerInstance(LayerInstance layer)
        {
            bool builtLayer = false;
            bool builtGrid = false;
            bool builtTileBuilder = false;
            
            void BuildLayerGameObject()
            {
                if (builtLayer)
                {
                    return;
                }
                _layerGameObject = _levelGameObject.CreateChildGameObject(layer.Identifier);
                
                LDtkIid iid = _layerGameObject.AddComponent<LDtkIid>();
                iid.SetIid(layer);
                
                builtLayer = true;
            }
            void AddGrid()
            {
                if (!builtLayer)
                {
                    Debug.LogError("Tried adding grid component before the layer GameObject");
                    return;
                }
                if (builtGrid)
                {
                    return;
                }
                _layerGrid = _layerGameObject.AddComponent<Grid>();
                builtGrid = true;
            }
            void SetupTileBuilder()
            {
                if (!builtLayer)
                {
                    Debug.LogError("Tried constructing the tileset builder before the layer GameObject");
                    return;
                }
                if (builtTileBuilder)
                {
                    return;
                }
                _builderTileset = new LDtkBuilderTileset(_importer, _layerGameObject, _sortingOrder);
                builtTileBuilder = true;
            }
            
            //ENTITIES
            if (layer.IsEntitiesLayer)
            {
                BuildLayerGameObject();
                
                _entityBuilder = new LDtkBuilderEntity(_importer, _layerGameObject, _sortingOrder, _linearVector, _worldLayout, _postProcess);
                _entityBuilder.SetLayer(layer);
                
                Profiler.BeginSample("BuildEntityLayerInstances");
                _entityBuilder.BuildEntityLayerInstances();
                Profiler.EndSample();
                return;
            }
            
            //TILE
            if (layer.IsTilesLayer)
            {
                BuildLayerGameObject();
                AddGrid();
                SetupTileBuilder();
                
                _builderTileset.SetLayer(layer);
                
                Profiler.BeginSample("BuildTileset GridTiles");
                _builderTileset.BuildTileset(layer.GridTiles);
                Profiler.EndSample();
            }
            
            //AUTO TILE (an int grid layer could additionally be an auto layer)
            if (layer.IsAutoLayer)
            {
                BuildLayerGameObject();
                AddGrid();
                SetupTileBuilder();
                
                _builderTileset.SetLayer(layer);
                
                Profiler.BeginSample("BuildTileset AutoLayerTiles");
                _builderTileset.BuildTileset(layer.AutoLayerTiles);
                Profiler.EndSample();
            }
            
            //INT GRID
            if (layer.IsIntGridLayer)
            {
                BuildLayerGameObject();
                AddGrid();

                _builderIntGrid = new LDtkBuilderIntGridValue(_importer, _layerGameObject, _sortingOrder);
                _builderIntGrid.SetLayer(layer);
                
                Profiler.BeginSample("BuildIntGridValues");
                _builderIntGrid.BuildIntGridValues();
                Profiler.EndSample();
            }

            //scale grid
            if (_layerGrid)
            {
                Profiler.BeginSample("ScaleTheGrid");
                float size = (float)layer.GridSize / _importer.PixelsPerUnit;
                Vector3 scale = new Vector3(size, size, 1);
                _layerGrid.transform.localScale = scale;
                Profiler.EndSample();
            }
        }
    }
}
