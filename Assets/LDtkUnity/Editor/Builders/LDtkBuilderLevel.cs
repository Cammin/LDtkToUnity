using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderLevel
    {
        private readonly LDtkProjectImporter _importer;
        private readonly LdtkJson _json;
        private readonly Level _level;
        private readonly WorldLayout _worldLayout;
        private readonly LDtkLinearLevelVector _linearVector;
        private readonly LDtkPostProcessorCache _postProcess;
        
        private GameObject _levelGameObject;
        private LDtkComponentLevel _levelComponent;
        private LDtkFields _fieldsComponent;
        private MonoBehaviour[] _components;
        
        private GameObject _layerGameObject;
        private LDtkComponentLayer _layerComponent;
        private Grid _layerGrid;

        private LDtkSortingOrder _sortingOrder;
        private LDtkBuilderTileset _builderTileset;
        private LDtkBuilderIntGridValue _builderIntGrid;
        private LDtkBuilderEntity _entityBuilder;
        private LDtkBuilderLevelBackground _backgroundBuilder;

        public LDtkBuilderLevel(LDtkProjectImporter importer, LdtkJson json, WorldLayout world, Level level, LDtkPostProcessorCache postProcess, LDtkLinearLevelVector linearVector = null)
        {
            _importer = importer;
            _json = json;
            _level = level;
            _postProcess = postProcess;
            _worldLayout = world;
            _linearVector = linearVector;
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
            if (!CanTryBuildLevel())
            {
                return;
            }
            
            BuildLevelProcess();
            SetupPostProcessing();
        }

        private void SetupPostProcessing()
        {
            LDtkPostProcessorInvoker.AddPostProcessLevel(_postProcess, _levelGameObject, _json);
        }

        private bool CanTryBuildLevel()
        {
            if (_importer == null)
            {
                LDtkDebug.LogError("ProjectAssets object is null; not building level.");
                return false;
            }

            if (_json == null)
            {
                LDtkDebug.LogError("Project data null; not building level.");
                return false;
            }

            if (_level == null)
            {
                LDtkDebug.LogError("Level null; not building level.");
                return false;
            }

            if (_level == null)
            {
                LDtkDebug.LogError($"LevelToBuild null, not assigned?");
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
            
            Profiler.BeginSample("BuildLevelTrigger");
            BuildLevelTrigger();
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
            
            if (addedFields)
            {
                _postProcess.TryAddInterfaceEvent<ILDtkImportedFields>(monoBehaviours, levelComponent => levelComponent.OnLDtkImportFields(lDtkFields));
            }
            _postProcess.TryAddInterfaceEvent<ILDtkImportedLevel>(monoBehaviours, levelComponent => levelComponent.OnLDtkImportLevel(level));
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
                    _linearVector.Next(_level.PxWid);
                    break;
                case WorldLayout.LinearVertical:
                    _linearVector.Next(_level.PxHei);
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
            _backgroundBuilder = new LDtkBuilderLevelBackground(_importer, _levelGameObject, _sortingOrder, _level, _levelComponent.Size);
            _backgroundBuilder.BuildBackground();
        }

        private void CreateLevelComponent()
        {
            _levelComponent = _levelGameObject.AddComponent<LDtkComponentLevel>();
            _levelComponent.SetIdentifier(_level.Identifier);
            _levelComponent.SetSize((Vector2)_level.UnityPxSize / _importer.PixelsPerUnit);
            _levelComponent.SetBgColor(_level.UnityBgColor, _level.UnitySmartColor);
            _levelComponent.SetWorldDepth(_level.WorldDepth);
            _levelComponent.SetNeighbours(_level.Neighbours);
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
                
                _layerComponent = _layerGameObject.AddComponent<LDtkComponentLayer>();
                LayerDefinition def = layer.Definition;
                _layerComponent.SetIdentifier(layer.Identifier, def.Doc);
                _layerComponent.SetType(def.LayerDefinitionType);

                builtLayer = true;
            }
            void AddGrid()
            {
                if (!builtLayer)
                {
                    LDtkDebug.LogError("Tried adding grid component before the layer GameObject");
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
                    LDtkDebug.LogError("Tried constructing the tileset builder before the layer GameObject");
                    return;
                }
                if (builtTileBuilder)
                {
                    return;
                }
                _builderTileset = new LDtkBuilderTileset(_importer, _layerComponent, _sortingOrder);
                builtTileBuilder = true;
            }
            
            //ENTITIES
            if (layer.IsEntitiesLayer)
            {
                BuildLayerGameObject();
                
                _entityBuilder = new LDtkBuilderEntity(_importer, _layerComponent, _sortingOrder, _linearVector, _worldLayout, _postProcess);
                
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

                _builderIntGrid = new LDtkBuilderIntGridValue(_importer, _layerComponent, _sortingOrder);
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
                _layerGrid = null;
                Profiler.EndSample();
            }
        }
        
        private void BuildLevelTrigger()
        {
            if (!_importer.CreateLevelBoundsTrigger)
            {
                return;
            }

            Rect levelRect = new Rect(Vector2.zero, (Vector2)_level.UnityPxSize / _importer.PixelsPerUnit);
            
            PolygonCollider2D levelCollider = _levelGameObject.AddComponent<PolygonCollider2D>();
            Vector2 topLeft		= new Vector2(levelRect.xMin, levelRect.yMax);
            Vector2 bottomLeft	= new Vector2(levelRect.xMin, levelRect.yMin);
            Vector2 bottomRight = new Vector2(levelRect.xMax, levelRect.yMin);
            Vector2 topRight	= new Vector2(levelRect.xMax, levelRect.yMax);
            levelCollider.points = new[]
            {
                topLeft,
                bottomLeft,
                bottomRight,
                topRight
            };
            levelCollider.isTrigger = true;
        }
    }
}
