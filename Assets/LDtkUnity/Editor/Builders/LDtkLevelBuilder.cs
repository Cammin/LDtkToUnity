using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor.Builders
{
    public class LDtkLevelBuilder
    {
        private int _layerSortingOrder;
        private Transform _currentLevelBuildRoot;

        private readonly LDtkProjectImporter _importer;
        private readonly LdtkJson _json;
        private readonly Level _level;

        public LDtkLevelBuilder(LDtkProjectImporter importer, LdtkJson json, Level level)
        {
            _importer = importer;
            _json = json;
            _level = level;
        }

        /// <summary>
        /// Returns the root of the object hierarchy of the layers
        /// </summary>
        public GameObject BuildLevel()
        {
            if (!CanTryBuildLevel())
            {
                return null;
            }
            
            Stopwatch levelBuildTimer = Stopwatch.StartNew();

            BuildLayerInstances();
            
            levelBuildTimer.Stop();

            if (_importer.LogBuildTimes)
            {
                double ms = levelBuildTimer.ElapsedMilliseconds;
                Debug.Log($"LDtk: Built level \"{_level.Identifier}\" in {ms}ms ({ms/1000}s)");
            }

            return _currentLevelBuildRoot.gameObject;
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

            if (!DoesLevelsContainLevel(_json.Levels, _level))
            {
                Debug.LogError("LDtk: level not contained within these levels in the project; not building level.");
                return false;
            }

            return true;
        }

        private bool DoesLevelsContainLevel(Level[] levels, Level levelToBuild)
        {
            if (levelToBuild == null)
            {
                Debug.LogError($"LDtk: LevelToBuild null, not assigned?");
                return false;
            }
            
            if (levels.Any(lvl => string.Equals(lvl.Identifier, levelToBuild.Identifier)))
            {
                return true;
            }
            
            Debug.LogError($"LDtk: No level named \"{levelToBuild}\" exists in the LDtk Project");
            return false;
        }
        
        private void BuildLayerInstances()
        {
            _layerSortingOrder = 0;
            _currentLevelBuildRoot = InstantiateLevelRootObject().transform;
            
            foreach (LayerInstance layer in _level.LayerInstances)
            {
                BuildLayerInstance(layer);
            }
            
            BuildLevelBackground();
        }
        
        private void BuildLevelBackground()
        {
            if (string.IsNullOrEmpty(_level.BgRelPath))
            {
                return;
            }
            
            DecrementLayer();
            
            LDtkRelativeGetterLevelBackground getter = new LDtkRelativeGetterLevelBackground();
            Texture2D levelBackground = getter.GetRelativeAsset(_level, _importer.assetPath);
            LDtkLevelBackgroundBuilder backgroundBuilder = new LDtkLevelBackgroundBuilder(_currentLevelBuildRoot, _level, levelBackground, _layerSortingOrder, _importer.PixelsPerUnit);

            Sprite bgSprite = backgroundBuilder.BuildBackground();
            bgSprite.hideFlags = HideFlags.HideInHierarchy;
            _importer.AutomaticallyGeneratedArtifacts.AddSprite(bgSprite);
            _importer.ImportContext.AddObjectToAsset(bgSprite.name, bgSprite);
        }

        private Texture2D GetLevelBackground()
        {
            //todo address this
            //Texture2D levelBackground = _importer.GetLevelBackground(_level.Identifier);
            return null;
        }

        private GameObject InstantiateLevelRootObject()
        {
            if (_json.Defs.LevelFields.IsNullOrEmpty())
            {
                return DefaultObject();
            }
            
            if (_importer.LevelFieldsPrefab != null)
            {
                return GetFieldInjectedLevelObject();
            }
                
            Debug.LogWarning("The LDtk project has level fields defined, but there is no scripted level prefab assigned.");
            return DefaultObject();

            GameObject DefaultObject()
            {
                return new GameObject(_level.Identifier);
            }
        }

        private GameObject GetFieldInjectedLevelObject()
        {
            GameObject obj = Object.Instantiate(_importer.LevelFieldsPrefab);
            obj.name = _level.Identifier;
            
            LDtkFieldInjector fieldInjector = new LDtkFieldInjector(obj, _level.FieldInstances);
            fieldInjector.InjectEntityFields();
            return obj;
        }

        private void BuildLayerInstance(LayerInstance layer)
        {
            if (layer.IsIntGridLayer) BuildIntGridLayer(layer);
            if (layer.IsAutoLayer) BuildTilesetLayer(layer, layer.AutoLayerTiles);
            if (layer.IsTilesLayer) BuildTilesetLayer(layer, layer.GridTiles);
            if (layer.IsEntitiesLayer) BuildEntityInstanceLayer(layer);
            
            SetLevelPosition(layer);
        }

        private void SetLevelPosition(LayerInstance layer)
        {
            Transform levelTransform = _currentLevelBuildRoot.transform;
            
            levelTransform.position = layer.UnityWorldPosition;
            
            LDtkEditorUtil.Dirty(levelTransform);
        }

        //todo these 2 functions below are very common, split 'em
        private void BuildIntGridLayer(LayerInstance layer)
        {
            Grid tilemapPrefab = GetTilemapPrefab(layer.Definition);
            if (tilemapPrefab == null)
            {
                return;
            }
            
            DecrementLayer();
            
            Tilemap tilemap = MakeTilemapInstance(layer, layer.Identifier, tilemapPrefab);
            if (tilemap == null)
            {
                return;
            }

            LDtkBuilderIntGridValue builder = new LDtkBuilderIntGridValue(layer, _importer);
            builder.BuildIntGridValues(tilemap);
            new LDtkGridPrefabBuilder().SetTilesetOpacity(tilemap, layer.Opacity);
        }
        
        private void BuildTilesetLayer(LayerInstance layer, TileInstance[] tiles)
        {
            Grid tilemapPrefab = GetTilemapPrefab(layer.Definition);
            if (tilemapPrefab == null)
            {
                return;
            }
            
            ILookup<Vector2Int, Vector2Int> grouped = tiles.Select(p => p.UnityPx).ToLookup(x => x);
            int maxRepetitions = grouped.Max(x => x.Count());

            string gameObjectName = layer.Identifier;

            if (layer.IsIntGridLayer)
            {
                gameObjectName += "_AutoLayer";
            }

            //this is for the potential of having multiple rules apply to the same tile. make multiple Unity Tilemaps.
            Tilemap[] tilemapInstances = new Tilemap[maxRepetitions];


            for (int i = 0; i < maxRepetitions; i++)
            {
                DecrementLayer();
                
                string name = gameObjectName;
                name += $"_{i}";

                Tilemap tilemap = MakeTilemapInstance(layer, name, tilemapPrefab);
                
                if (tilemap == null)
                {
                    return;
                }
                
                tilemapInstances[i] = tilemap;
            }

            LDtkBuilderTileset builder = new LDtkBuilderTileset(layer, _importer, tiles, tilemapInstances);
            builder.BuildTileset();

            //set each layer's alpha
            foreach (Tilemap tilemapInstance in tilemapInstances)
            {
                new LDtkGridPrefabBuilder().SetTilesetOpacity(tilemapInstance, layer.Opacity);
            }
        }
        
        private Grid GetTilemapPrefab(LayerDefinition def)
        {
            GameObject prefab = _importer.GetTilemapPrefab(def.Identifier);
            
            //ensure we got the tilemap component from the project. Though, this error would never happen because we always have a default prefab.
            if (prefab == null)
            {
                Debug.LogError($"LDtk: Grid prefab is null; not building layer. Check the Grid prefab that was used to build the level.");
                return null;
            }

            //validate components
            if (!LDtkGridPrefabValidator.ValidateGridPrefabComponents(prefab, def, out string errorMsg))
            {
                Debug.LogError($"LDtk: {errorMsg}", prefab);
                return null;
            }

            return prefab.GetComponent<Grid>();
        }

        private Tilemap MakeTilemapInstance(LayerInstance layer, string name, Grid tilemapPrefab)
        {
            Tilemap tilemap = new LDtkGridPrefabBuilder().BuildUnityTileset(name, tilemapPrefab, _layerSortingOrder, _importer.PixelsPerUnit, (int)layer.GridSize);

            if (tilemap == null)
            {
                return null;
            }

            Transform tilemapTrans = tilemap.transform.parent;
            tilemapTrans.parent = _currentLevelBuildRoot;
            tilemapTrans.localPosition = Vector3.zero;
            return tilemap;
        }

        private void BuildEntityInstanceLayer(LayerInstance layer)
        {
            DecrementLayer();
            LDtkBuilderEntity entityBuilder = new LDtkBuilderEntity(layer, _importer);
            GameObject root = entityBuilder.BuildEntityLayerInstances(_layerSortingOrder);

            if (_importer.DeparentInRuntime)
            {
                root.AddComponent<LDtkDetachChildren>();
            }
            
            root.transform.parent = _currentLevelBuildRoot;
            root.transform.localPosition = Vector3.zero;
        }

        public void DecrementLayer()
        {
            _layerSortingOrder--;
            //Debug.Log(_layerSortingOrder);
        }
    }
}
