using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderLevel
    {
        private readonly LDtkProjectImporter _project;
        private readonly LDtkJsonImporter _importer;
        private readonly LdtkJson _json;
        private readonly Level _level;
        private readonly WorldLayout _worldLayout;
        private readonly LDtkLinearLevelVector _linearVector;
        private readonly LDtkAssetProcessorActionCache _assetProcess;
        private readonly LDtkLevelFile _lvlFile;
        
        private LDtkComponentWorld _worldComponent;
        private GameObject _levelGameObject;
        private LDtkComponentLevel _levelComponent;
        private LDtkFields _fieldsComponent;
        private LDtkIid _iidComponent;
        private LDtkComponentLayer[] _layerComponents;
        private MonoBehaviour[] _components;
        
        private GameObject _layerGameObject;
        private LDtkComponentLayer _layerComponent;
        private LDtkComponentLayerParallax _parallax;
        private Grid _layerGrid;
        private LDtkComponentLayerTilesetTiles _layerTiles;
        private LDtkComponentLayerIntGridValues _layerIntGrid;

        private LDtkSortingOrder _sortingOrder;
        private LDtkBuilderTileset _builderTileset;
        private LDtkBuilderIntGridValue _builderIntGrid;
        private LDtkBuilderEntity _entityBuilder;
        private LDtkBuilderLevelBackground _backgroundBuilder;

        public LDtkBuilderLevel(LDtkProjectImporter project, LdtkJson json, WorldLayout world, Level level, LDtkLevelFile levelFile, LDtkAssetProcessorActionCache assetProcess, LDtkJsonImporter importer, LDtkComponentWorld worldComponent, LDtkLinearLevelVector linearVector)
        {
            _project = project;
            _json = json;
            _level = level;
            _lvlFile = levelFile;
            _assetProcess = assetProcess;
            _importer = importer;
            _worldLayout = world;
            _worldComponent = worldComponent;
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
        
        public LDtkComponentLevel BuildLevel()
        {
            if (!CanTryBuildLevel())
            {
                return null;
            }
            
            BuildLevelProcess();
            LDtkAssetProcessorInvoker.AddPostProcessLevel(_assetProcess, _importer, _levelGameObject, _json);
            return _levelComponent;
        }

        private bool CanTryBuildLevel()
        {
            if (_project == null)
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
            
            return true;
        }

        private void BuildLevelProcess()
        {
            LDtkProfiler.BeginSample("CreateLevelComponent");
            CreateLevelComponent();
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("AddIidComponent");
            AddIidComponent();
            LDtkProfiler.EndSample();

            LDtkProfiler.BeginSample("LDtkSortingOrder");
            var sortingOrders = _project.LayerSortingOrders.ToDictionary(x => x.Layer, x => x.Order);
            _sortingOrder = new LDtkSortingOrder(sortingOrders);
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("BuildLayerInstances");
            BuildLayerInstances();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("BuildLevelTrigger");
            BuildLevelTrigger();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("BuildBackground");
            BuildBackground();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("BuildFields");
            BuildFields();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("NextLinearVector");
            NextLinearVector();
            LDtkProfiler.EndSample();
            
            LDtkProfiler.BeginSample("PopulateLevelComponent");
            PopulateLevelComponent();
            LDtkProfiler.EndSample();
        }

        private void BuildFields()
        {
            bool addedFields = TryAddFields();

            MonoBehaviour[] monoBehaviours = _components;
            
            //caching here to make the import context work properly.
            Level level = _level;
            LDtkFields lDtkFields = _fieldsComponent;
            
            if (addedFields)
            {
                _assetProcess.TryAddInterfaceEvent<ILDtkImportedFields>(monoBehaviours, levelComponent => levelComponent.OnLDtkImportFields(lDtkFields));
            }
            _assetProcess.TryAddInterfaceEvent<ILDtkImportedLevel>(monoBehaviours, levelComponent => levelComponent.OnLDtkImportLevel(level));
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
            _levelGameObject.transform.position = _level.UnityWorldSpaceCoord(_worldLayout, _project.PixelsPerUnit, scaler);

            _components = _levelGameObject.GetComponents<MonoBehaviour>();
            return _levelGameObject;
        }

        private void GetInitialLevelGameObject()
        {
            GameObject prefab = _project.CustomLevelPrefab;
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
            _layerComponents = new LDtkComponentLayer[_level.LayerInstances.Length];
            
            //build layers and background from front to back in terms of ordering 
            for (int i = 0; i < _level.LayerInstances.Length; i++)
            {
                LayerInstance layer = _level.LayerInstances[i];
                LDtkProfiler.BeginSample($"BuildLayerInstance {layer.Identifier}");
                LDtkComponentLayer layerComponent = BuildLayerInstance(layer);
                LDtkProfiler.EndSample();

                _layerComponents[i] = layerComponent;
            }
        }

        private void BuildBackground()
        {
            Vector2 size = ((Vector2)_level.UnityPxSize / _project.PixelsPerUnit);
            _backgroundBuilder = new LDtkBuilderLevelBackground(_project, _levelGameObject, _sortingOrder, _level, size);
            _backgroundBuilder.BuildBackground();
        }

        private void CreateLevelComponent()
        {
            _levelComponent = _levelGameObject.AddComponent<LDtkComponentLevel>();
        }

        private void PopulateLevelComponent()
        {
            _levelComponent.OnImport(_level, _lvlFile, _layerComponents, _fieldsComponent, _worldComponent, _iidComponent, _project.PixelsPerUnit);
        }
        
        private bool TryAddFields()
        {
            if (_json.Defs.LevelFields.IsNullOrEmpty())
            {
                return false;
            }
            
            LDtkFieldParser.CacheRecentBuilder(null);
            LDtkFieldsFactory fieldsFactory = new LDtkFieldsFactory(_levelGameObject, _level.FieldInstances, _project, _importer);
            fieldsFactory.SetEntityFieldsComponent();
            _fieldsComponent = fieldsFactory.FieldsComponent;
            return true;
        }

        //todo this could be refactored to live in the BuilderLayer
        private LDtkComponentLayer BuildLayerInstance(LayerInstance layer)
        {
            bool builtLayer = false;
            bool builtGrid = false;
            bool builtTileBuilder = false;
            bool populatedComponent = false;

            LDtkComponentEntity[] entities = null;
            float layerScale = 1;

            _layerComponent = null;
            _layerIntGrid = null;
            _layerTiles = null;
            
            void TryBuildLayerGameObject()
            {
                if (builtLayer)
                {
                    return;
                }
                _layerGameObject = _levelGameObject.CreateChildGameObject(layer.Identifier);
                
                _layerComponent = _layerGameObject.AddComponent<LDtkComponentLayer>();

                if (_project.UseParallax)
                {
                    _parallax = _layerGameObject.AddComponent<LDtkComponentLayerParallax>();
                }

                _iidComponent = _layerGameObject.AddComponent<LDtkIid>();
                _iidComponent.SetIid(layer);

                builtLayer = true;
            }
            void TryAddGrid()
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
                _layerGrid.cellGap = Vector3.back;
                builtGrid = true;
            }
            void TrySetupTileBuilder()
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
                _builderTileset = new LDtkBuilderTileset(_project, _level, _layerComponent, _sortingOrder, _importer);
                builtTileBuilder = true;
            }
            void AddTilesetTilesComponent()
            {
                _layerTiles = _layerGameObject.AddComponent<LDtkComponentLayerTilesetTiles>();
                _layerTiles.OnImport(_builderTileset.Map);
            }
            void TryPopulateLayerComponent(ref bool populated)
            {
                if (!_layerComponent)
                {
                    return;
                }
                if (populated)
                {
                    return;
                }
                
                //now that everything is gathered, do the special OnImport and populate that component
                _layerComponent.OnImport(_importer.DefinitionObjects, layer, _levelComponent, _iidComponent, entities, _layerIntGrid, _layerTiles, layerScale);
                if (_project.UseParallax)
                {
                    Vector2 halfLvlSize = (Vector2)_level.UnityPxSize / _project.PixelsPerUnit * 0.5f;
                    _parallax.OnImport(layer.Definition.ParallaxFactor, halfLvlSize, layer.Definition.ParallaxScaling);
                }
                populated = true;
            }
            
            //ENTITIES
            if (layer.IsEntitiesLayer)
            {
                TryBuildLayerGameObject();
                
                _entityBuilder = new LDtkBuilderEntity(_project, _level, _layerComponent, _sortingOrder, _linearVector, _worldLayout, _assetProcess, _importer);
                layerScale = _entityBuilder.SetLayerAndScale(layer);
                
                LDtkProfiler.BeginSample("BuildEntityLayerInstances");
                entities = _entityBuilder.BuildEntityLayerInstances();
                LDtkProfiler.EndSample();

                TryPopulateLayerComponent(ref populatedComponent);
                return _layerComponent;
            }
            
            //TILE
            if (layer.IsTilesLayer)
            {
                TryBuildLayerGameObject();
                TryAddGrid();
                TrySetupTileBuilder();
                
                layerScale = _builderTileset.SetLayerAndScale(layer);
                
                LDtkProfiler.BeginSample("BuildTileset GridTiles");
                _builderTileset.BuildTileset(layer.GridTiles);
                LDtkProfiler.EndSample();
                
                AddTilesetTilesComponent();
            }
            
            //AUTO TILE (an int grid layer could additionally be an auto layer)
            if (layer.IsAutoLayer)
            {
                TryBuildLayerGameObject();
                TryAddGrid();
                TrySetupTileBuilder();
                
                layerScale = _builderTileset.SetLayerAndScale(layer);
                
                LDtkProfiler.BeginSample("BuildTileset AutoLayerTiles");
                _builderTileset.BuildTileset(layer.AutoLayerTiles);
                LDtkProfiler.EndSample();
                
                AddTilesetTilesComponent();
            }
            
            //INT GRID
            if (layer.IsIntGridLayer)
            {
                TryBuildLayerGameObject();
                TryAddGrid();

                _builderIntGrid = new LDtkBuilderIntGridValue(_project, _level, _layerComponent, _sortingOrder, _importer);
                layerScale = _builderIntGrid.SetLayerAndScale(layer);
                
                LDtkProfiler.BeginSample("BuildIntGridValues");
                _builderIntGrid.BuildIntGridValues();
                LDtkProfiler.EndSample();
                
                _layerIntGrid = _layerGameObject.AddComponent<LDtkComponentLayerIntGridValues>();
                _layerIntGrid.OnImport(_importer.DefinitionObjects, layer);
            }

            //scale grid
            if (_layerGrid)
            {
                LDtkProfiler.BeginSample("ScaleTheGrid");
                float size = (float)layer.GridSize / _project.PixelsPerUnit;
                Vector3 scale = new Vector3(size, size, 1);
                _layerGrid.transform.localScale = scale;
                _layerGrid = null;
                LDtkProfiler.EndSample();
            }

            TryPopulateLayerComponent(ref populatedComponent);

            return _layerComponent;
        }
        
        private void BuildLevelTrigger()
        {
            if (!_project.CreateLevelBoundsTrigger)
            {
                return;
            }

            Rect levelRect = new Rect(Vector2.zero, (Vector2)_level.UnityPxSize / _project.PixelsPerUnit);
            
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
